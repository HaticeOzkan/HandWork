using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Order : IEntity
    {
        public int ID { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return OrderItems.Sum(x => x.Price);
            }
        }
        public bool Paid { get; set; }

    }
    public class OrderItem
    {
        public int ItemID { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

    }
}
