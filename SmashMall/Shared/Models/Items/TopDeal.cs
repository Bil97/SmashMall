using SmashMall.Shared.Models.Files;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Items
{
    public class TopDeal
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string PageUrl { get; set; }

        public TopDealImage Image { get; set; }

        [ForeignKey(nameof(Image))]
        [Required]
        public string ImageId { get; set; }
    }
}
