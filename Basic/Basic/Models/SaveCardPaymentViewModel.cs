namespace Basic.Models
{
    public class SaveCardPaymentViewModel
    {
        public Merchant? Merchant { get; set; }

        public int MerchantId { get; set; }

        public string CardNumber { get; set; }

        public string Expiration { get; set; }

        public string SecurityCode { get; set; }

        public double Amount { get; set; }

        public string RecipitentEmail { get; set; }
    }
}
