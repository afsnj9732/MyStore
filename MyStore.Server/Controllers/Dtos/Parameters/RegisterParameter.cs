using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyStore.Server.Controllers.Dtos.Parameters
{
    public class RegisterParameter
    {
        [Required]
        [DisplayName("電子郵件")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayName("密碼")]
        public string Password { get; set; }
        [Required]
        [DisplayName("請再次輸入密碼")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string RecaptchaToken { get; set; }
    }
}
