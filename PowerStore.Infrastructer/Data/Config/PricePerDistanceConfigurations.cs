using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PowerStore.Core.Entities.Price_Estimate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Data.Config
{
    internal class PricePerDistanceConfigurations : IEntityTypeConfiguration<PricePerDistance>
    {
        public void Configure(EntityTypeBuilder<PricePerDistance> builder)
        {
            builder.Property(p => p.PricePerKm).HasColumnType("decimal");
            builder.Property(p => p.DistanceLimit).HasColumnType("decimal");
        }
    }
}
