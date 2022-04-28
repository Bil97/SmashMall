using SmashMall.Models.Items;

namespace SmashMall.Models.Goods
{
    public class GoodKeyFeature : KeyFeature
    {
        public Good Good { get; set; }

        public string GoodId { get; set; }
    }
}
