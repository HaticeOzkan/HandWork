using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UnitOfWork
    {
        public HandWorkContext Db;
        public BaseRepository<Order> OrderRepo;
        public BaseRepository<Product> ProductRepo;
        public BaseRepository<ProductImage> ImageRepo;
        public BaseRepository<ShoppingCart> ShoppingCartRepo;
        public BaseRepository<Tag> TagRepo;

        public UnitOfWork()
        {
            if (Db == null)
                Db = new HandWorkContext();
            OrderRepo = new BaseRepository<Order>(Db);
            ProductRepo = new BaseRepository<Product>(Db);
            ImageRepo = new BaseRepository<ProductImage>(Db);
            ShoppingCartRepo = new BaseRepository<ShoppingCart>(Db);
            TagRepo = new BaseRepository<Tag>(Db);
        }
        public bool Complete()
        {
            try
            {
                Db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static HandWorkContext Create()//??zaten yukarıda oluşturduk
        {
            return new HandWorkContext();

        }
    }
}
