using SmashMall.Shared.Models.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.UserAccount
{
    public class UserDetails
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Email confirmed")]
        public bool EmailConfirmed { get; set; }

        public string Phonenumber { get; set; }

        [Display(Name = "Phonenumber confirmed")]
        public bool PhonenumberConfirmed { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string Name => $"{FirstName} {LastName}";

        [Display(Name = "National ID number or Passport number")]
        public int? NationalIdNumber { get; set; }

        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
