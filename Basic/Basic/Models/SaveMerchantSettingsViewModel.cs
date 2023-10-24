namespace Basic.Models
{
    public class SaveMerchantSettingsViewModel
    {
        public int MerchantId { get; set; }

        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string ReceiptEmail { get; set; }

        public string AppPassword { get; set; }
    }
}
