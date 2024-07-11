﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager) {

            if (!userManager.Users.Any()) {
                var User = new AppUser()
                {
                    DisplayName="Fawzya Omar",
                    Email="FawzyaOmar.route@gmail.com",
                    UserName="FawzyaOmar",
                    PhoneNumber="0104163854"
                    


                };
                await userManager.CreateAsync(User,"PassWORD");
           
            
            }
        
        
        
        }

    }
}
