using X.PagedList;

namespace Basic.Models
{
    public class DashboardViewModel
    {
        public Merchant? Merchant { get; set; }

        public IPagedList<TransactionHistory>? Transactions { get; set; }

        public IPagedList<CardPayment>? CardPayments { get; set; }

        public IPagedList<Invoice>? Invoices { get; set; }
    }
}
