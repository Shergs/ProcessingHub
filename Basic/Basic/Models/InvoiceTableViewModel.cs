using X.PagedList;

namespace Basic.Models
{
    public class InvoiceTableViewModel
    {
        public IPagedList<Invoice>? Invoices { get; set; }

        public int MerchantId { get; set; }
    }
}
