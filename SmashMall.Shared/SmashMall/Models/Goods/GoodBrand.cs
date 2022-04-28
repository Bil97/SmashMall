using SmashMall.Models.Files;
using SmashMall.Models.Items;
using System.Collections.Generic;

namespace SmashMall.Models.Goods
{
    public class GoodBrand : Brand
    {
        public GoodBrandImage Image { get; set; }

        public string ImageId { get; set; }

        public GoodSubcategory Subcategory { get; set; }

        public string SubcategoryId { get; set; }

        public ICollection<Good> Goods { get; set; }
    }
}
