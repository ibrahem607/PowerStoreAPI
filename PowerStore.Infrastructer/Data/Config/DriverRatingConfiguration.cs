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
    public class DriverRatingConfiguration : IEntityTypeConfiguration<DriverRating>
    {
        public void Configure(EntityTypeBuilder<DriverRating> builder)
        {
            builder
                .HasOne(d => d.Driver)
                .WithMany(dd => dd.DriverRatings)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.NoAction);
           
        }
    }
}
