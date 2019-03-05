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
    public class HandWorkContext : IdentityDbContext<Member>
    {
        public HandWorkContext() : base("HandContext")
        {

        }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<ProfilPhoto> ProfilPhotos { get; set; }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.ID);
            modelBuilder.Entity<Order>().HasKey(x => x.ID);
            modelBuilder.Entity<ProductImage>().HasKey(x => x.ID);
            modelBuilder.Entity<ProfilPhoto>().HasKey(x => x.ID);
            modelBuilder.Entity<Notification>().HasKey(x => x.ID);
            modelBuilder.Entity<ProductItem>().HasKey(x => x.ID);
            modelBuilder.Entity<OrderItem>().HasKey(x => x.ID);

            modelBuilder.Entity<Product>().HasRequired(x => x.Member).WithMany(x => x.Products);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductImages).WithRequired(x => x.Product);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductItems).WithRequired(x => x.Product);
            modelBuilder.Entity<Product>().HasMany(x => x.OrderItems).WithRequired(x => x.Product);
            modelBuilder.Entity<Basket>().HasMany(x => x.ProductItems).WithRequired(x => x.Basket);
            modelBuilder.Entity<Member>().HasOptional(x => x.Basket).WithRequired(x=>x.Member);
            modelBuilder.Entity<Member>().HasOptional(x => x.ProfilPhoto).WithRequired(x => x.Member);
            modelBuilder.Entity<Notification>().HasRequired(x => x.Member).WithMany(x => x.Notifications);
            base.OnModelCreating(modelBuilder);
        }
    }
}
