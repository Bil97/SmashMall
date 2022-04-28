namespace SmashMall.Models.Items
{
    public class CartModel
    {
        public string UserId { get; set; }

        public int ItemsCount { get; set; }

        public decimal TotalPrice { get; set; }

        public double Weight { get; set; }
    }
}
