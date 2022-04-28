using SQLite;

namespace SmashMall.Models.Items
{
    public class Subcategory
    {
        public string Id{ get; set; }

       [NotNull]
        public string Name { get; set; }
    }
}
