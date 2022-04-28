using System;

namespace SmashMall.Shared.Models.Items
{
    public class Feedback
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
