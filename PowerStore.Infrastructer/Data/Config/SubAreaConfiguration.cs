using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Data.Config
{
    public class SubAreaConfiguration : IEntityTypeConfiguration<SubArea>
    {
        public void Configure(EntityTypeBuilder<SubArea> builder)
        {
            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Configure relationship
            builder.HasOne(sa => sa.MainArea)
                .WithMany(ma => ma.SubAreas)
                .HasForeignKey(sa => sa.MainAreaId)
                .OnDelete(DeleteBehavior.Restrict); // or Cascade based on your needs

            builder.HasQueryFilter(sa => !sa.IsDeleted);
        }
    }
}
