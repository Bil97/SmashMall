using SmashMall.Models.Files;
using SmashMall.Models.Items;
using System.Collections.Generic;

namespace SmashMall.Models.Goods
{
    public class GoodSubCategory : SubCategory
    {
        public GoodSubCategoryImage Image { get; set; }

        public string ImageId { get; set; }

        public GoodCategory GoodCategory { get; set; }

        public string GoodCategoryId { get; set; }

        public ICollection<Good> Goods { get; set; }
    }
}
