namespace Basic.Models
{
    public class SaveSalesSettingsViewModel
    {
        public int MerchantId { get; set; }
        public double TaxRate { get; set; }
        public double FeePercent { get; set; }
        public string FeeName { get; set; }
    }
}
