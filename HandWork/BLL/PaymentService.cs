using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class PaymentService
    {
        public abstract bool MakePayment(IPaymentModel pm);
    }
    //concrete class 
    public class BankTransferService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (BankTransferPayment)pm;

            //1.Bankaya bağlanıp ödeme var mı kontrol et
            //2.Ödeme varsa true döndür
            //3.Yoksa false döndür
            return true;
        }
    }

    public class CreditCartService : PaymentService
    {
        public override bool MakePayment(IPaymentModel pm)
        {
            var info = (CreditCardPayment)pm;
            //1.Kart bilgileri geçerli mi ve tutar çekiliyor mu
            //2.Ödeme alındıysa true döndür
            //3.Ödeme başarısız olduysa false döndür
            return true;
        }
    }
}
