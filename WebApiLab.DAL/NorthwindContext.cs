using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLab.DAL.Entities;

namespace WebApiLab.DAL
{
    public class NorthwindContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"connectionstring")
        //        .LogTo(Console.WriteLine, LogLevel.Debug);
        //}

        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(15)
                .IsRequired();
            
            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Ital" }
             );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Sör", UnitPrice = 50, CategoryId = 1, ShipmentRegion = ShipmentRegion.Asia },
                new Product { Id = 2, Name = "Bor", UnitPrice = 550, CategoryId = 1 },
                new Product { Id = 3, Name = "Tej", UnitPrice = 260, CategoryId = 1 },
                new Product
                {
                    Id = 4,
                    Name = "Whiskey",
                    UnitPrice = 960,
                    CategoryId = 1,
                    ShipmentRegion = ShipmentRegion.Australia
                },
                new Product
                {
                    Id = 5,
                    Name = "Rum",
                    UnitPrice = 960,
                    CategoryId = 1,
                    ShipmentRegion = ShipmentRegion.EU | ShipmentRegion.NorthAmerica
                }
            );

            modelBuilder.Entity<Order>().HasData(
                    new Order { Id = 1, OrderDate = new DateTime(2019, 02, 01) }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1 },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2 }
            );

            modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithMany(o => o.Products)
            .UsingEntity<OrderItem>(
                j => j
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Restrict),
                j => j
                    .HasOne(oi => oi.Product)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(oi => oi.ProductId),
                j =>
                {
                    j.HasKey(oi => oi.Id);
                });

            var converter = new EnumToStringConverter<ShipmentRegion>();
            modelBuilder
                .Entity<Product>()
                .Property(e => e.ShipmentRegion)
                .HasConversion(converter);

            modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
