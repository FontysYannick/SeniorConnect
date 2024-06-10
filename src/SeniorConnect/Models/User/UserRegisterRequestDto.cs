using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.Models.User
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? Preposition { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress(ErrorMessage = "Uw email Address is niet correct.")]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Gebruik minimaal 6 en maximaal 100 tekens.")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
