using SmashMall.Models.Goods;
using SmashMall.Models.Items;

namespace SmashMall.Models.Orders
{
    public class GoodCart : Cart
    {
        public int Quantity { get; set; }

        public double TotalWeight { get; set; }

        public Good Good { get; set; }

        public string GoodId { get; set; }

    }
}
