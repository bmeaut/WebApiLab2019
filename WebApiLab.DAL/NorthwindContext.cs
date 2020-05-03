using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiLab.Entities;

namespace WebApiLab.DAL
{
    public class NorthwindContext : DbContext
    {                              
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()               
                .Property(c => c.Name)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Category>().HasData(new Category {Id = 1, Name = "Ital"});

            modelBuilder.Entity<Product>().HasData(
                new Product
                        {
                            Id =1, Name = "Sör", UnitPrice = 50, CategoryId = 1,
                            ShipmentRegion = ShipmentRegion.Asia
                        },
                         new Product { Id=2, Name = "Bor", UnitPrice = 550, CategoryId = 1 },
                         new Product { Id=3, Name = "Tej", UnitPrice = 260, CategoryId = 1 },
                         new Product
                         {
                             Id = 4, Name = "Whiskey", UnitPrice = 960, CategoryId = 1,
                             ShipmentRegion = ShipmentRegion.Australia
                         },
                         new Product
                         {
                             Id = 5, Name = "Rum", UnitPrice = 1860, CategoryId = 1,
                             ShipmentRegion = ShipmentRegion.EU | ShipmentRegion.NorthAmerica
                         }
              );

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ProductOrders)
                .WithOne(po => po.Order)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasData(new Order {Id = 1, OrderDate = new DateTime(2019, 02, 01)});

            modelBuilder.Entity<Order>()
                .HasData(new Order { Id = 2, OrderDate = new DateTime(2020, 04, 01) });

            modelBuilder.Entity<ProductOrder>()
                .HasData(new ProductOrder { Id = 1, OrderId = 1, ProductId = 1},
                        new ProductOrder { Id = 2, OrderId = 1, ProductId = 2 });

            modelBuilder.Entity<ProductOrder>()
                .HasData(new ProductOrder { Id = 3, OrderId = 2, ProductId = 2 },
                        new ProductOrder { Id = 4, OrderId = 2, ProductId = 4 },
                        new ProductOrder { Id = 5, OrderId = 2, ProductId = 5 });

            var converter = new EnumToStringConverter<ShipmentRegion>();
            modelBuilder
                .Entity<Product>()
                .Property(e => e.ShipmentRegion)
                .HasConversion(converter);

            modelBuilder.Entity<Product>()
                    .Property(p => p.RowVersion)
                    .ValueGeneratedOnAddOrUpdate()
                    .IsConcurrencyToken();
        }
    }
}
