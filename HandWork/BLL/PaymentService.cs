using Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            WebClient myWebClient = new WebClient();
            NameValueCollection myNameValueCollection = new NameValueCollection();
            string uriString = "https://apis-bank-test.apigee.net/apis/v1.0.1/oauth/token";
            byte[] responseArray = myWebClient.UploadValues(uriString, "POST", myNameValueCollection);
            Encoding.ASCII.GetString(responseArray);
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
