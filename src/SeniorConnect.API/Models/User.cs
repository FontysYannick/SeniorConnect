namespace SeniorConnect.API.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phonenumber { get; set; }
        public string Pawssword { get; set; }


        public User() { }
    }
}
