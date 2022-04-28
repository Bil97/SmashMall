using SmashMall.Shared.Models.Goods;
using SmashMall.Shared.Models.Items;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Orders
{
    public class GoodCart : Cart
    {
        public int Quantity { get; set; }

        public double TotalWeight { get; set; }

        [Display(Name = "Item")]
        public Good Good { get; set; }

        [Display(Name = "Item")]
        [ForeignKey(nameof(Good))]
        public string GoodId { get; set; }

    }
}
