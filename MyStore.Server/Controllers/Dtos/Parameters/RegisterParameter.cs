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
        [MinLength(5)]
        [MaxLength(16)]
        public string Password { get; set; }
        [Required]
        [DisplayName("請再次輸入密碼")]
        [Compare("Password")]
        [MinLength(5)]
        [MaxLength(16)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string RecaptchaToken { get; set; }
    }
}
