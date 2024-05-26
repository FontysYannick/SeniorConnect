namespace SeniorConnect.API.Models.Users
{
    public class LoginResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public HttpResponseMessage response { get; set; }
    }
}
