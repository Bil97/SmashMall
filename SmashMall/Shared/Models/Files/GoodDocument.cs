using SmashMall.Shared.Models.Goods;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmashMall.Shared.Models.Files
{
    public class GoodDocument : File
    {
        public Good Good { get; set; }

        [ForeignKey(nameof(Good))]
        public string GoodId { get; set; }
    }
}
