using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.API.Models.Users
{
    public class UserRegisterRequest : AbstractAuthRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? Preposition { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
