using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
   public class Message:IEntity
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public virtual Member Owner { get; set; }


    }
}
