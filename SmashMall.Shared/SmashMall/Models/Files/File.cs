using SQLite;

namespace SmashMall.Models.Files
{
    public class File
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
