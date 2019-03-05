using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Category:IEntity
    {
        public List<Product> Products { get; set; }
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageURL { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual List<Category> SubCategory { get; set; }
    }
}
