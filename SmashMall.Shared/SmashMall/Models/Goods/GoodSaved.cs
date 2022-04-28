using SQLite;

namespace SmashMall.Models.Goods
{
    public class GoodSaved
    {
        [PrimaryKey]
        public string Id{ get; set; }

        public string GoodId { get; set; }

        public Good Good { get; set; }

        public string UserId { get; set; }
    }
}
