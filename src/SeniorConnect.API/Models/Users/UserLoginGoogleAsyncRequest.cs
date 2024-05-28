using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Models.Users
{
    public class UserLoginGoogleAsyncRequest
    {
        public string GoogleId { get; set; }
        public string GoogleEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
