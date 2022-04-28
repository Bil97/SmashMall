using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Goods
{
    public class GoodSaved
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("Good")]
        public string GoodId { get; set; }

        public Good Good { get; set; }

        public string UserId { get; set; }
    }
}
