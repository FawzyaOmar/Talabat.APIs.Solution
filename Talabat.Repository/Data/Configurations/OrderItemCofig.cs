﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderItemCofig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // OrderItem Own ProductItemOrdered => One Table
            builder.OwnsOne(OI => OI.Product, P => P.WithOwner());

            // decimal warning
            builder.Property(O => O.Price)
                .HasColumnType("decimal(18,2)");
        }







    }
}
