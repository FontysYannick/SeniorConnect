namespace SeniorConnect.API.Models.Users
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public HttpResponseMessage response { get; set; }
    }
}
