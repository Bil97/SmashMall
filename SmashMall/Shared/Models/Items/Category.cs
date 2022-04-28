using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.Items
{
    public class Category
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Details { get; set; }

    }
}
