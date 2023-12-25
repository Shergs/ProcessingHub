using Basic.Models;
using System.Net;

namespace Basic.Helpers
{
    public class PaymentService
    {
        public string SubmitPayment(PaymentRequest request)
        {
            string url = "https://secure.networkmerchants.com/api/transact.php";
            string strPost = $"security_key={request.SecurityKey}&firstname={request.FirstName}" +
                             $"&lastname={request.LastName}&address1={request.Address1}" +
                             $"&city={request.City}&state={request.State}&zip={request.Zip}" +
                             $"&payment=creditcard&type=sale&amount=1.00" +
                             $"&ccnumber=4111111111111111&ccexp=1015&cvv=123";  // Note: Don't hardcode credit card details

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }

            using (HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse())
            {
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
