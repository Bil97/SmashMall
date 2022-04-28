using System;

namespace SmashMall.Models.UserAccount
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? NationalIdNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public string AuthToken { get; set; }

        public string Theme { get; set; }
    }
}
