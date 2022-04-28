using SmashMall.Models.UserAccount;

namespace SmashMall.Models.Items
{
    public class Order
    {
        public string Id { get; set; }

        public decimal TotalCost { get; set; }

        public bool TransactionCpmpleted { get; set; }

        public UserDetails UserDetails { get; set; }

        public string UserId { get; set; }
    }
}
