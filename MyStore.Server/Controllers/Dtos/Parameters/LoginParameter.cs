using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Server.Controllers.Dtos.Parameters
{
    public class LoginParameter
    {
        [Required]
        [DisplayName("電子郵件")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayName("密碼")]
        public string Password { get; set; }
        [Required]
        public string RecaptchaToken { get; set; }
    }
}
