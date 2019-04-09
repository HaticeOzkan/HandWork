using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
  public class LikeModel:IEntity
    {
        public int ID { get; set; }
        public virtual List<Member> MemberLiked { get; set; }//beğendiği kişiler
        public virtual List<Product> ProductLiked { get; set; }//beğendiği ürünler

    }
}
