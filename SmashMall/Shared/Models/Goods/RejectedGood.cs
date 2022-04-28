using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class RejectedGood
    {
        [Key]
        [ForeignKey(nameof(Good))]
        public string Id { get; set; } // updating this will cause wrong results if not updated in the controller

        public Good Good { get; set; }

        public string ReasonForRejection { get; set; }
    }
}
