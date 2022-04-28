using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.Items
{
    public class KeyFeature
    {
        public string Id { get; set; }

        [Required]
        public string Feature { get; set; }

    }
}
