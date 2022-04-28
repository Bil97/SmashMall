using SmashMall.Models.Files;
using SQLite;

namespace SmashMall.Models.Items
{
    public class TopDeal
    {
        public string Id { get; set; }
        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string PageUrl { get; set; }

        public TopDealImage Image { get; set; }

        [NotNull]
        public string ImageId { get; set; }
    }
}
