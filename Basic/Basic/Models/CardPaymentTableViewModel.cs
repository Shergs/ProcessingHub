using X.PagedList;

namespace Basic.Models
{
    public class CardPaymentTableViewModel
    {
        public IPagedList<CardPayment>? CardPayments { get; set; }

        public int MerchantId { get; set; }
    }
}
