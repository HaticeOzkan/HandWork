using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public interface IPaymentModel
    {
        int Id { get; set; }
        string TC { get; set; }
        string NameSurname { get; set; }
        bool IsApproved { get; set; }
    }

    public class BankTransferPayment : IPaymentModel
    {
        public int Id { get; set; }
        public string TC { get; set; }
        public string NameSurname { get; set; }
        public bool IsApproved { get; set; }
        public string IBAN { get; set; }
        public string AccountNo { get; set; }
        public virtual Order Order { get; set; }
    }

    public class CreditCardPayment : IPaymentModel
    {
        public int Id { get; set; }
        public string TC { get; set; }
        public string NameSurname { get; set; }
        public string CartNumber { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public short CV2 { get; set; }
        public bool IsApproved { get; set; }
        public virtual Order Order { get; set; }
    }
}
