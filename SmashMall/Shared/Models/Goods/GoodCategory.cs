using SmashMall.Shared.Models.Items;
using System.Collections.Generic;

namespace SmashMall.Shared.Models.Goods
{
    public class GoodCategory : Category
    {
        public ICollection<GoodSubcategory> Subcategories { get; set; }
    }
}
