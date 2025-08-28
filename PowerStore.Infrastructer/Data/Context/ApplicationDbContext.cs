using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Infrastructer.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            //// ADD SOFT DELETE FILTER FOR ALL BaseEntity CLASSES
            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            //    {
            //        builder.Entity(entityType.ClrType)
            //            .HasQueryFilter(e => EF.Property<bool>(e, "IsDeleted") == false);
            //    }
            //}
        }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> invoiceItems { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<MainArea> mainAreas { get; set; }
        public DbSet<SubArea> subAreas { get; set; }
        public DbSet<Representative> representatives { get; set; }
        public DbSet<StoreKeeper> storeKeepers { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Branch> branches { get; set; }

       
    }
}
