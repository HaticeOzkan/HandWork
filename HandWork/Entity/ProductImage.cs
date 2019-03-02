using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ProductImage : IEntity
    {
        public int ID { get; set; }
        public string LargeImageURL { get; set; }
        public string ThumbImageURL { get; set; }
        public virtual Product Product { get; set; }
    }
}
