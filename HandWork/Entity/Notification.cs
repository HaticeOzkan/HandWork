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
    //public class NotificationBox:IEntity
    //{
    //    public virtual Member Member { get; set; }
    //    public int ID { get; set; }
    //    public List<NotificationItem> Notifications { get; set; }
    //}
    public class Notification:IEntity
    {
        public int ID { get; set; }
        //public virtual NotificationBox NotificationBox { get; set; }
        //public virtual Member  { get; set; }
        public DateTime SendDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }   
        public virtual Member Receiver { get; set; }
    }
}