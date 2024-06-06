namespace SeniorConnect.Models.User
{
    public class UserLoginGoogleRequestDto
    {
        public string GoogleId { get; set; }
        public string GoogleEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
