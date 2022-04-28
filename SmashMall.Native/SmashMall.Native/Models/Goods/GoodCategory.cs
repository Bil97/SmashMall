using SmashMall.Models.Items;
using System.Collections.Generic;

namespace SmashMall.Models.Goods
{
    public class GoodCategory : Category
    {
        public ICollection<GoodSubCategory> SubCategories { get; set; }
    }
}
