using SQLite;

namespace SmashMall.Models.Files
{
    public class SupportDocuments
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string ItemId { get; set; }

        public string Path { get; set; }
    }
}
