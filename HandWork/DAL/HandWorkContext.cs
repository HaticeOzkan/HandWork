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
        public virtual DbSet<ProductItem> ProductItems { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.ID);
            modelBuilder.Entity<Order>().HasKey(x => x.ID);
            modelBuilder.Entity<ProductImage>().HasKey(x => x.ID);
            modelBuilder.Entity<ProfilPhoto>().HasKey(x => x.ID);
            modelBuilder.Entity<Message>().HasKey(x => x.ID);
            modelBuilder.Entity<Notification>().HasKey(x => x.ID);
            modelBuilder.Entity<ProductItem>().HasKey(x => x.ID);
            modelBuilder.Entity<OrderItem>().HasKey(x => x.ID);
            modelBuilder.Entity<Category>().HasKey(x => x.ID);
            modelBuilder.Entity<Conversation>().HasKey(x => x.ID);
           
            modelBuilder.Entity<Category>().HasMany(x => x.Products).WithRequired(x=>x.Categories).HasForeignKey(x=>x.CategoryID);
            modelBuilder.Entity<Product>().HasRequired(x => x.Member).WithMany(x => x.Products).HasForeignKey(x=>x.MemberID);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductImages).WithRequired(x => x.Product);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductItems).WithRequired(x => x.Product);
            modelBuilder.Entity<Product>().HasMany(x => x.OrderItems).WithRequired(x => x.Product);
            modelBuilder.Entity<Basket>().HasMany(x => x.ProductItems).WithRequired(x => x.Basket);
            modelBuilder.Entity<Member>().HasOptional(x => x.Basket).WithRequired(x=>x.Member);
            modelBuilder.Entity<Member>().HasOptional(x => x.ProfilPhoto).WithRequired(x => x.Member);
            modelBuilder.Entity<Member>().HasMany(x => x.Messages);
            modelBuilder.Entity<Conversation>().HasMany(x => x.Messages).WithRequired(x => x.Conversation);
            modelBuilder.Entity<Member>().HasMany(x => x.Notifications).WithRequired(x => x.Owner);
            base.OnModelCreating(modelBuilder);
        }
    }
}
