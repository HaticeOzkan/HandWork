using Entity.Abstract;
using System;
using System.Collections.Generic;

namespace Entity
{
    public enum NotificationType
    {
        Like,
        DisLike,
        Order
    }
    public class Notification:IEntity
    {
        public int ID { get; set; }
        public DateTime SendDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Content { get; set; }
        public string SenderMemId { get; set; }
        public string ReceiverMemId { get; set; }   
        public virtual Member Owner { get; set; }
        
    }
}