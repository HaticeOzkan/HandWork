using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Product : IEntity
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public int UnitStock { get; set; }
        public string ProductName { get; set; }      
        public string Description { get; set; }
        public virtual List<ProductImage> ProductImages { get; set; } 
        public string ImageURL { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public virtual Customer Seller { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<ShoppingCart> ShoppingCarts { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public Product()
        {
            LikeCount = 0;
            DisLikeCount = 0;
        }
    }
    
}
