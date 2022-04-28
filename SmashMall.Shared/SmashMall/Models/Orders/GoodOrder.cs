using SmashMall.Models.Goods;
using SmashMall.Models.Items;
using SmashMall.Models.Pickup;
using SQLite;

namespace SmashMall.Models.Orders
{
    public class GoodOrder : Order
    {
        public int Quantity { get; set; }

        public double TotalWeight { get; set; }

        public Good Good { get; set; }

        public string GoodId { get; set; }

        public PickupStation PickupStation { get; set; }

        [NotNull]
        public string PickupStationId { get; set; }

        public bool LocalDelivery { get; set; }

        public bool IsApproved { get; set; }
    }
}
