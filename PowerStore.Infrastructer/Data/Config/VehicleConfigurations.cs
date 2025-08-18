﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Data.Config
{
    internal class VehicleConfigurations : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            //builder.HasKey(v => v.Id);

          
            builder.Property(v => v.Colour).IsRequired().HasMaxLength(100);
            builder.Property(v => v.VehicleLicenseIdBack).IsRequired().HasMaxLength(100);
            builder.Property(v => v.VehicleLicenseIdFront).IsRequired().HasMaxLength(100);


            // Relationships
            builder.HasOne(v => v.Driver)
                .WithMany(d => d.Vehicles)
                .HasForeignKey(v => v.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many Relationship
            builder.HasOne(v => v.vehicleModel)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.VehicleModelId);
        }
    }
}
