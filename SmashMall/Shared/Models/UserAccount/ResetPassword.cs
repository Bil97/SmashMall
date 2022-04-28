using System.ComponentModel.DataAnnotations;

namespace SmashMall.Shared.Models.UserAccount
{
    public class ResetPassword
    {
        public string UserId { get; set; }

        public string Code { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new password")]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
    }
}
