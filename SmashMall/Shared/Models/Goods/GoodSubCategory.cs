using SmashMall.Shared.Models.Files;
using SmashMall.Shared.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class GoodSubcategory : Subcategory
    {
        public GoodSubcategoryImage Image { get; set; }

        [Display(Name = "Image")]
        [ForeignKey(nameof(Image))]
        public string ImageId { get; set; }

        public GoodCategory GoodCategory { get; set; }

        [Required]
        [ForeignKey(nameof(GoodCategory))]
        [Display(Name = "Category")]
        public string GoodCategoryId { get; set; }

        public ICollection<GoodBrand> Brands { get; set; }
    }
}
