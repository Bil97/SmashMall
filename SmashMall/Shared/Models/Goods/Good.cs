using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Items;
using SmashMall.Shared.Models.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SmashMall.Shared.Models.Goods
{
    public class Good : Item
    {
        [Required]
        public string Code { get; set; }

        public GoodBrand Brand { get; set; }

        [Required]
        [ForeignKey(nameof(Brand))]
        public string BrandId { get; set; }

        [NotMapped]
        public bool IsSaved { get; set; }

        public double Discount { get; set; }

        // good specification

        [Required]
        public double Weight { get; set; }

        [Display(Name = "Size (L x W x H), cm")]
        public string Size { get; set; }

        public string Model { get; set; }

        // end good specification

        public ICollection<GoodImage> Images { get; set; }

        public ICollection<GoodDocument> Documents { get; set; }

        public ICollection<GoodKeyFeature> KeyFeatures { get; set; }

        public ICollection<CustomerFeedback> Feedback { get; set; }

        public ICollection<GoodOrder> Orders { get; set; }

    }
}
