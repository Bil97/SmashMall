using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.UserAccount
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "National ID number or Passport number")]
        public int? NationalIdNumber { get; set; }

        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }
    }
}
