using Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HandWorkContext : IdentityDbContext<Customer>
    {
        public HandWorkContext() : base("HandWorkContext")
        {

        }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<MyProduct> MyProducts { get; set; } 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(x => x.Age).IsOptional();
            modelBuilder.Entity<Customer>().Property(x => x.NameSurname).HasMaxLength(50);
            modelBuilder.Entity<Order>().HasKey(x => x.ID);
            modelBuilder.Entity<Product>().HasKey(x => x.ID).Property(x => x.ProductName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(x => x.Description).HasMaxLength(200);
            modelBuilder.Entity<OrderItem>().HasKey(x => x.ID);
            modelBuilder.Entity<ProductImage>().HasKey(x => x.ID);
            modelBuilder.Entity<Tag>().HasKey(x => x.ID).Property(x => x.TagName).HasMaxLength(15);
            modelBuilder.Entity<ShoppingCart>().HasKey(x => x.ID);
            modelBuilder.Entity<ShoppingCart>().HasMany(x => x.Products).WithMany(x => x.ShoppingCarts);
            modelBuilder.Entity<ShoppingCart>().HasRequired(x => x.Customer).WithOptional(x => x.ShoppingCart);
            modelBuilder.Entity<ProductImage>().HasRequired(x => x.Product).WithMany(x => x.ProductImages);
            modelBuilder.Entity<Tag>().HasMany(x => x.Products).WithMany(x => x.Tags);
            modelBuilder.Entity<Product>().HasMany(x => x.OrderItems).WithRequired(x => x.Product);
            modelBuilder.Entity<Customer>().HasMany(x => x.MyProducts).WithRequired(x => x.Seller);
            modelBuilder.Entity<Order>().HasMany(x => x.OrderItems).WithRequired(x => x.Order);
            modelBuilder.Entity<Customer>().HasMany(x => x.Orders).WithRequired(x => x.Customer);
            base.OnModelCreating(modelBuilder);
        }
    }
}
