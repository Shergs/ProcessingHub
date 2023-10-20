using X.PagedList;

namespace Basic.Models
{
    public class DashboardViewModel
    {
        public Merchant? Merchant { get; set; }

        public IPagedList<TransactionHistory>? Transactions { get; set; }

    }
}
