using Azure;
using SeniorConnect.API.Entities;
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

        public async Task<LoginResponse> LoginWitGoogleAsync(UserLoginGoogleAsyncRequest userLoginGoogleAsyncRequest)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");

            var response = await client.PostAsJsonAsync("api/users/login-google", userLoginGoogleAsyncRequest);

            if (response.IsSuccessStatusCode == true)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }

            return new LoginResponse
            {
                response = response
            };
        }

        public async Task<GetUserInfoResponse?> GetUserAsync(int userId) 
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync($"api/users/{userId}");

            if (response.IsSuccessStatusCode == false)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<GetUserInfoResponse>();
        }

        public async Task<HttpResponseMessage> ChangeUserInfoAsync(UserChangeInfoRequest userChangeInfoRequest)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");

            return await client.PostAsJsonAsync("api/users/change-info", userChangeInfoRequest);
        }
    }
}
