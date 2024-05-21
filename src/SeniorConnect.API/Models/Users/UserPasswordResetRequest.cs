using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.API.Models.Users
{
    public class UserPasswordResetRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Gebruik minimaal 6 en maximaal 100 tekens.")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
