using SmashMall.Shared.Models.Items;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class GoodKeyFeature : KeyFeature
    {
        public Good Good { get; set; }

        [ForeignKey(nameof(Good))]
        public string GoodId { get; set; }
    }
}
