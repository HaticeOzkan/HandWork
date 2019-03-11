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
        public string ProductName { get; set; }
        public virtual Member Member { get; set; }
        public string MemberID { get; set; }
        public string Content { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public virtual List<ProductImage> ProductImages { get; set; }
        public virtual List<ProductItem> ProductItems{ get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Categories { get; set; }
        public bool HasPhoto { get; set; }
        public Product()
        {
            LikeCount = 0;
            DisLikeCount = 0;
            HasPhoto = false;
        }

    }
}
