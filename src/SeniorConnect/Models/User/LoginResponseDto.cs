namespace SeniorConnect.Models.User
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public HttpResponseMessage response { get; set; }
    }
}
