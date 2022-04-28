using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class GoodBrand:Brand
    {
        public GoodBrandImage Image { get; set; }

        [Display(Name = "Image")]
        [ForeignKey(nameof(Image))]
        public string ImageId { get; set; }

        public GoodSubcategory Subcategory { get; set; }

        [ForeignKey(nameof(Subcategory))]
        public string SubcategoryId { get; set; }

        public ICollection<Good> Goods { get; set; }
    }
}
