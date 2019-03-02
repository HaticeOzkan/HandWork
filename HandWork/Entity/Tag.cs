using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Tag : IEntity
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
