namespace Basic.Models
{
    public class PaymentRequest
    {
        public string SecurityKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        // ... Add other fields as needed
    }
}
