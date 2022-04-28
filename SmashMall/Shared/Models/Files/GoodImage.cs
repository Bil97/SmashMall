using SmashMall.Shared.Models.Goods;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Files
{
    public class GoodImage : File
    {
        public Good Good { get; set; }

        [ForeignKey(nameof(Good))]
        [Display(Name = "Item")]
        public string GoodId { get; set; }
    }
}
