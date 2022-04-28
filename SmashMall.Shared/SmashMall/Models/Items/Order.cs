using SQLite;
using System;

namespace SmashMall.Models.Items
{
    public class Order
    {
        public string Id{ get; set; }

        public string UserOrderId { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsConfirmed { get; set; }

       [NotNull]
        public string UserId { get; set; }

       [NotNull]
        public string SellerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
