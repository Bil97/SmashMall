using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Models.Pickup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Orders
{
    public class GoodOrder : Order
    {
        public int Quantity { get; set; }

        public double TotalWeight { get; set; }

        [Display(Name = "Item")]
        public Good Good { get; set; }

        [Display(Name = "Item")]
        [ForeignKey(nameof(Good))]
        public string GoodId { get; set; }

        public PickupStation PickupStation { get; set; }

        [ForeignKey(nameof(PickupStation))]
        [Required]
        public string PickupStationId { get; set; }

        public bool LocalDelivery { get; set; }

        public bool IsApproved { get; set; }
    }
}
