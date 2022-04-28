using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.UserAccount
{
    public class ForgotPassword
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
