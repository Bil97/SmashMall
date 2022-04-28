using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.Pickup
{
    public class Town
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<PickupStation> PickupStations { get; set; }
    }
}
