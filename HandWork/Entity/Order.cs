using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity
{
    public class Order : IEntity
    {
        public int ID { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Member Member { get; set; }
        public bool IsPaid { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return OrderItems.Sum(x => x.TotalPrice);
            }
        }
        public Order()
        {
            OrderDate = DateTime.Now;
        }
    }
    public class OrderItem:IEntity
    {
        public int ID { get; set; }
        public int ItemCount { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return Product.Price * ItemCount;
            }
        }
    }

}