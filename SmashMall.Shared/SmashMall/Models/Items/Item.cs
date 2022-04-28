using SmashMall.Models.UserAccount;
using SQLite;
using System;

namespace SmashMall.Models.Items
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        [Ignore]
        public UserDetails UserDetails { get; set; }

        public string UserId { get; set; }

        public bool? IsApprovedForSale { get; set; }

    }
}
