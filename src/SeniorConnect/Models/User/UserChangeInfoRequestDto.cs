using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.Models.User
{
    public class UserChangeInfoRequestDto
    {
        [Required]
        public int userId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? Preposition { get; set; }

        [Required]
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
