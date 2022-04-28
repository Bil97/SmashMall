using SmashMall.Models.Files;
using SmashMall.Models.Items;
using SQLite;
using System.Collections.Generic;

namespace SmashMall.Models.Goods
{
    public class GoodSubcategory : Subcategory
    {
        public GoodSubcategoryImage Image { get; set; }

        public string ImageId { get; set; }

        public GoodCategory GoodCategory { get; set; }

        [NotNull]
        public string GoodCategoryId { get; set; }

        public ICollection<GoodBrand> Brands { get; set; }
    }
}
