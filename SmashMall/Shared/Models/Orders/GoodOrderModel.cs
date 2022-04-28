using System;

namespace SmashMall.Shared.Models.Orders
{
    public class GoodOrderModel
    {
        public string PickupStationId { get; set; }

        public bool LocalDelivery { get; set; }
    }
}
