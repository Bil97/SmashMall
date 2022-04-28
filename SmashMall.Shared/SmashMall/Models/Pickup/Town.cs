using SQLite;
using System.Collections.Generic;

namespace SmashMall.Models.Pickup
{
    public class Town
    {
        public string Id{ get; set; }

       [NotNull]
        public string Name { get; set; }

        public ICollection<PickupStation> PickupStations { get; set; }
    }
}
