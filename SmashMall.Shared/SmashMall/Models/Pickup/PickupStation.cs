using SQLite;

namespace SmashMall.Models.Pickup
{
    public class PickupStation
    {
        public string Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public Town Town { get; set; }

        [NotNull]
        public string TownId { get; set; }

        [NotNull]
        public double InitialWeight { get; set; }

        public decimal InitialWeightCost { get; set; }

        public decimal ExcessWeightCost { get; set; }

        public decimal LocalDeliveryCost { get; set; }

    }
}
