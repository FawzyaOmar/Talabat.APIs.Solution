using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<StoreContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        });
        builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

        });

        //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();

        builder.Services.AddSingleton<IConnectionMultiplexer>(Options => {

            var Connection = builder.Configuration.GetConnectionString("RedisConnection");
            return ConnectionMultiplexer.Connect(Connection);
        
        });
        builder.Services.AddApplicationServices();

        builder.Services.AddIdentityServices(builder.Configuration);

        var app = builder.Build();


        //StoreContext dbContext = new StoreContext();
        // await dbContext.Database.MigrateAsync();

        using var Scope = app.Services.CreateScope();
        var Services = Scope.ServiceProvider;
        var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
        try
        {


            var DbContext = Services.GetRequiredService<StoreContext>();
            var IdenityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
            await IdenityDbContext.Database.MigrateAsync();

            var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
            await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

            await DbContext.Database.MigrateAsync();
           
           
            await StoreContextSeed.SeedAsync(DbContext);



        }
        catch (Exception ex)
        {

            var Logger = LoggerFactory.CreateLogger<Program>();
            Logger.LogError(ex, "An Error Occure During Updating database when Appling the Migration");
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwaggerMiddlewares();
        }
        app.UseStaticFiles();
        app.UseStatusCodePagesWithReExecute("/errors/{0}");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}