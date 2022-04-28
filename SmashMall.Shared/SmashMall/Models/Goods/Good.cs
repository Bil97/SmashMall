using SmashMall.Models.Files;
using SmashMall.Models.Items;
using SmashMall.Models.Orders;
using SQLite;
using System.Collections.Generic;
namespace SmashMall.Models.Goods
{
    public class Good : Item
    {
        [NotNull]
        public string Code { get; set; }

        public GoodBrand Brand { get; set; }

       [NotNull]
        public string BrandId { get; set; }

        [Ignore]
        public bool IsSaved { get; set; }

        public double Discount { get; set; }

        // good specification

       [NotNull]
        public double Weight { get; set; }

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
