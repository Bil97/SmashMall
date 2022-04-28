using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Items
{
    public class Order
    {
        public string Id { get; set; }

        public string UserOrderId { get; set; }

        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Checked")]
        public bool IsConfirmed { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string SellerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
