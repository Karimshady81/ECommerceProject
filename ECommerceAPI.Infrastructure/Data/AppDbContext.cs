using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("User_Id");

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.HasIndex(e => e.Email)
                       .IsUnique()
                       .HasDatabaseName("IX_User_Email");
                      
                entity.Property(e => e.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.LastName)
                       .IsRequired()
                       .HasMaxLength(100);

                entity.Property(e => e.Phone)
                      .HasMaxLength(20);

                entity.Property(e => e.Address)
                       .HasMaxLength(250);

                entity.Property(e => e.City)
                      .HasMaxLength(100);

                entity.Property(e => e.Country)
                      .HasMaxLength(100);

                entity.Property(e => e.Role)
                      .HasConversion<string>()
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                       .HasDefaultValueSql("GETDATE()")
                       .ValueGeneratedOnAdd();


                entity.HasMany(e => e.Orders)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.CartItems)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("Product_Id");

                entity.Property(e => e.Name)
                          .IsRequired()
                          .HasMaxLength(200);

                entity.Property(e => e.Description)
                          .HasMaxLength(1000);

                entity.Property(e => e.Price)
                          .IsRequired()
                          .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Image)
                          .HasMaxLength(500);

                entity.Property(e => e.StockQuantity)
                          .IsRequired()
                          .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                          .IsRequired();

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .ValueGeneratedOnAdd();


                entity.HasOne(e => e.Category)
                          .WithMany(e => e.Products)
                          .HasForeignKey(e => e.CategoryId)
                          .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.OrderItems)
                          .WithOne(e => e.Product)
                          .HasForeignKey(e => e.ProductId)
                          .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.CartItems)
                          .WithOne(e => e.Product)
                          .HasForeignKey(e => e.ProductId)
                          .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("Order_Id");

                entity.Property(e => e.OrderNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(e => e.OrderNumber)
                      .IsUnique();

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.ShippingAddress)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .ValueGeneratedOnAdd();

                entity.HasOne(e => e.User)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);   

                entity.HasMany(e => e.OrderItems)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Payment)
                        .WithOne(e => e.Order)
                        .HasForeignKey<Payment>(e => e.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("OrderItem_Id");
                      
                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.Property(e => e.UnitPrice)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();                

                entity.Property(e => e.TotalPrice)
                       .HasColumnType("decimal(18,2)")
                       .IsRequired();

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("CartItem_Id");

                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.Property(e => e.CreatedAt)
                       .HasDefaultValueSql("GETDATE()")
                       .ValueGeneratedOnAdd();

                entity.HasOne(e => e.User)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(c => new { c.ProductId, c.UserId })
                      .IsUnique();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("Category_Id");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(e => e.Name)
                      .IsUnique();

                entity.Property(e => e.Description)
                       .HasMaxLength(500)
                       .IsRequired(false);

                entity.Property(e => e.IsActive)
                      .IsRequired();

               entity.HasMany(e => e.Products)
                      .WithOne(e => e.Category)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id)
                      .HasName("Payment_Id");

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(e => e.PaymentMethod)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.Property(e => e.PaymentStatus)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(50);

                entity.Property(e => e.PaymentDate)
                      .HasDefaultValueSql("GETDATE()")
                      .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Order)
                      .WithOne(e => e.Payment)
                      .HasForeignKey<Payment>(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
