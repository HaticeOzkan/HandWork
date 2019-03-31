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
        public virtual Conversation Conversation { get; set; }
        public string SenderID { get; set; }
        public virtual Member ReceiverMember { get; set; }
        public Message()
        {
            Date = DateTime.Now;
        }
        
    }
    public class Conversation : IEntity
    {
        public int ID { get; set; }
        public virtual List<Message> Messages { get; set; }
        public string SenderID { get; set; }
        public virtual Member ReceiverMember { get; set; }
        public DateTime? LastConversationDate
        {
            get
            {
                if (Messages != null || Messages.Count != 0)
                {
                    DateTime LastDate = Messages.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault();
                    return LastDate;
                }
                else return null;
                
            }
        }
        public string Title
        {
            get
            {
                string Content = Messages.OrderBy(x => x.Date).Select(x => x.Content).FirstOrDefault();
                string NewContent = "";
                foreach (var item in Content.Split(' ').Take(5))
                {
                    NewContent = NewContent + item;
                    NewContent = NewContent + " ";
                }
                return NewContent;

            }
        }      
    }
}
