using SmashMall.Models.Items;

namespace SmashMall.Models.Goods
{
    public class CustomerFeedback : Feedback
    {
        public Good Good { get; set; }

        public string GoodId { get; set; }
    }
}
