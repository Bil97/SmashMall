using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Items
{
    public class Cart
    {
        public string Id { get; set; }

        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }

    }
}
