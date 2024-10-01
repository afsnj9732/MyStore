using System.ComponentModel.DataAnnotations;

namespace MyStore.Server.Controllers.Dtos.Parameters
{
    public class RegisterParameter
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(16)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [MinLength(5)]
        [MaxLength(16)]
        public string ConfirmPassword { get; set; }
        [Required]
        public string RecaptchaToken { get; set; }
    }
}
