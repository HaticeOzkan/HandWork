using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ShoppingCart : IEntity
    {
        public int ID { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal SubTotal
        {
            get
            {
                return Products.Sum(x => x.Price);
            }
        }
        public ShoppingCart()
        {
            Date = DateTime.Now;
        }
    }
}
