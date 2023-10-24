namespace Basic.Models
{
    public class SendInvoiceViewModel
    {
        public int MerchantId { get; set; }

        public Merchant? Merchant { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<InvoiceItemViewModel> InvoiceItems { get; set; } = new List<InvoiceItemViewModel>();
    }

    public class InvoiceItemViewModel
    {
        public string Item { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}

