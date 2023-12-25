using Basic.Helpers;
using Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController()
        {
            _paymentService = new PaymentService();
        }

        public ActionResult SubmitPayment()
        {
            PaymentRequest request = new PaymentRequest
            {
                SecurityKey = "6457Thfj624V5r7WUwc5v6a68Zsd6YEm",
                FirstName = "John",
                LastName = "Smith",
                Address1 = "1234 Main St.",
                City = "Chicago",
                State = "IL",
                Zip = "60193"
                // ... populate other fields as needed
            };

            string response = _paymentService.SubmitPayment(request);
            ViewBag.PaymentResponse = response;  // Sending the response to the view via ViewBag
            return View();
        }
    }
}
