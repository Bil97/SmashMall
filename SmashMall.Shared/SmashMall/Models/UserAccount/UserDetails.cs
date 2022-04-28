using System;
using System.Collections.Generic;

namespace SmashMall.Models.UserAccount
{
    public class UserDetails
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string Phonenumber { get; set; }

        public bool PhonenumberConfirmed { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}";

        public int? NationalIdNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
