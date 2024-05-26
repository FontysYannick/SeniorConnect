using SeniorConnect.API.Models.Users;

namespace SeniorConnect.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResponse> LoginAsync(UserLoginRequest userLoginRequest)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.PostAsJsonAsync("api/users/login", userLoginRequest);

            if (response.IsSuccessStatusCode == true)
            {
                LoginResponse newResponse = new LoginResponse
                {
                    response = response
                };
                
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }

           return new LoginResponse { 
               response = response
           };
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserRegisterRequest userRegisterRequest)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");

            return await client.PostAsJsonAsync("api/users/register", userRegisterRequest);
        }
    }
}
