using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.Items
{
    public class Subcategory
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
