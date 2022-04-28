using SQLite;

namespace SmashMall.Models.Goods
{
    public class RejectedGood
    {
        [PrimaryKey]
        public string Id { get; set; } // updating this will cause wrong results if not updated in the controller

        public Good Good { get; set; }

        public string ReasonForRejection { get; set; }
    }
}
