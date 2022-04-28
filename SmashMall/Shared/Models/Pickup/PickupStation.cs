using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Pickup
{
    public class PickupStation
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public Town Town { get; set; }

        [ForeignKey(nameof(Town))]
        [Required]
        public string TownId { get; set; }

        [Required]
        public double InitialWeight { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal InitialWeightCost { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal ExcessWeightCost { get; set; }

        [Column(TypeName = "decimal(18,2")]
        public decimal LocalDeliveryCost { get; set; }

    }
}
