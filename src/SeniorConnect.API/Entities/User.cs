using System.ComponentModel.DataAnnotations;

namespace SeniorConnect.API.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string? Preposition { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string? Phonenumber { get; set; }

        public byte[] PasswordHash { get; set; } = new byte[32];

        public byte[] PasswordSalt { get; set; } = new byte[32];

        public string? VerificationToken { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<ActivityUsers> ActivityUsers { get; set; } = new List<ActivityUsers>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        public string? GoogleId { get; set; }

        public User() { }

        public bool isGoogleAccount()
        {
            return this.GoogleId != null;
        }
    }
}
