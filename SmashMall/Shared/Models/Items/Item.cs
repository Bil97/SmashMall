using SmashMall.Shared.Models.UserAccount;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Items
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }

        //[DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        [Display(Name = "Date added")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [NotMapped]
        public UserDetails UserDetails { get; set; }

        public string UserId { get; set; }

        public bool? IsApprovedForSale { get; set; }

    }
}
