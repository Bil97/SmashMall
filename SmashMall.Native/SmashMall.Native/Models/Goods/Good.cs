using SmashMall.Models.Files;
using SmashMall.Models.Items;
using System.Collections.Generic;
namespace SmashMall.Models.Goods
{
    public class Good : Item
    {
        public string Code { get; set; }

        public GoodSubCategory GoodSubCategory { get; set; }

        public string GoodSubCategoryId { get; set; }

        public ICollection<GoodImage> Images { get; set; }
    }
}
