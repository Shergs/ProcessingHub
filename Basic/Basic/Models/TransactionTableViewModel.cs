using X.PagedList;

namespace Basic.Models
{
    public class TransactionTableViewModel
    {
        public IPagedList<TransactionHistory>? Transactions { get; set; }

        public int MerchantId { get; set; }
    }
}
