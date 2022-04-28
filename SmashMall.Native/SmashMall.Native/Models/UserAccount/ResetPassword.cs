namespace SmashMall.Models.UserAccount
{
    public class ResetPassword
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
