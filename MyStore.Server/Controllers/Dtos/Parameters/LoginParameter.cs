using System.ComponentModel.DataAnnotations;

namespace MyStore.Server.Controllers.Dtos.Parameters
{
    public class LoginParameter
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string RecaptchaToken { get; set; }
    }
}
