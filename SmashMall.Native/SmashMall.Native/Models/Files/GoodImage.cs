using SmashMall.Models.Goods;

namespace SmashMall.Models.Files
{
    public class GoodImage : File
    {
        public Good Good { get; set; }

        public string GoodId { get; set; }
    }
}
