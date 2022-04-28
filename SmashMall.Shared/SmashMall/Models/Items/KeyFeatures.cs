using SQLite;

namespace SmashMall.Models.Items
{
    public class KeyFeature
    {
        public string Id { get; set; }

        [NotNull]
        public string Feature { get; set; }

    }
}
