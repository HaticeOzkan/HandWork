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
        public virtual Conversation Conversation { get; set; }
        
    }
    public class Conversation : IEntity
    {
        public int ID { get; set; }
        public virtual List<Message> Messages { get; set; }
        public DateTime LastConversationDate { get; set; }
        public virtual Member ReceiverMember { get; set; }
        public string Title { get; set; }
        public Conversation()
        {
            DateTime LastDate = Messages.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault();
            LastConversationDate = LastDate;
        }
    }
}
