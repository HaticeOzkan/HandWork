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
    public class NotificationBox:IEntity
    {
        public virtual Member Member { get; set; }
        public int ID { get; set; }
        public List<NotificationItem> Notifications { get; set; }
    }
    public class NotificationItem:IEntity
    {
        public int ID { get; set; }
        public virtual NotificationBox NotificationBox { get; set; }
        public virtual Member Member { get; set; }
        public DateTime SendDate { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Content { get; set; }
    }
}