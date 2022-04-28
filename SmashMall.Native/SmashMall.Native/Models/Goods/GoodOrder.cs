using SmashMall.Models.Items;

namespace SmashMall.Models.Goods
{
    public class GoodOrder : Order
    {
        public int Quantity { get; set; }

        public Good Good { get; set; }

        public string GoodId { get; set; }
    }
}
