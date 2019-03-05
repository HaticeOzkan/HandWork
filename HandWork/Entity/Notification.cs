using Entity.Abstract;
using System;
using System.Collections.Generic;

namespace Entity
{
    public class Notification:IEntity
    {
        public virtual Member Member { get; set; }
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime NotiDate { get; set; }



    }
}