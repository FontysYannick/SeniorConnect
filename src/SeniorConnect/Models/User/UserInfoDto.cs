using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.Models.User
{
    public class UserInfoDto
    {
        [Required(ErrorMessage = "Voer uw geldig voornaam in")]
        public string FirstName { get; set; }
        public string? Preposition { get; set; }

        [Required(ErrorMessage = "Voer uw geldig achternaam in")]
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
