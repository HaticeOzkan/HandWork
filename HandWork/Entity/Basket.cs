using Entity.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Entity
{
    public class Basket : IEntity
    {
        public int ID { get; set; }
        public virtual Member Member { get; set; }
        public virtual List<ProductItem> ProductItems { get; set; }
        public decimal SubTotal
        {
            get
            {
                return ProductItems.Sum(x => x.TotalPrice);
            }
        }
    }
    public class ProductItem:IEntity
    {
        public int ID { get; set; }
        public int ItemCount { get; set; }
        public virtual Product Product { get; set; }
        public virtual Basket Basket { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return Product.Price * ItemCount;
            }
        }
        public ProductItem()
        {
            ItemCount = 1;
        }
    }
}