using SQLite;

namespace SmashMall.Models.Items
{
    public class Category
    {
        public string Id{ get; set; }

       [NotNull]
        public string Name { get; set; }

       [NotNull]
        public string Details { get; set; }

    }
}
