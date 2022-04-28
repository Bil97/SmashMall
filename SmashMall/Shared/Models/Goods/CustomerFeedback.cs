using SmashMall.Shared.Models.Items;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class CustomerFeedback : Feedback
    {
        public Good Good { get; set; }

        [ForeignKey(nameof(Good))]
        public string GoodId { get; set; }
    }
}
