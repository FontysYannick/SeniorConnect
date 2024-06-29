using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SeniorConnect.Models.User;

namespace SeniorConnect.Services
{
    public class AuthService : AbstractService
    {
        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<LoginResponseDto> LoginAsync(UserLoginRequestDto userLoginRequest)
        {
            var response = await GetClient().PostAsJsonAsync("api/users/login", userLoginRequest);

            if (response.IsSuccessStatusCode == false)
            {
                return new LoginResponseDto
                {
                    response = response
                };
            }

            return await CreatLoginResponseDto(response);
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserRegisterRequestDto userRegisterRequest)
        {
            return await GetClient().PostAsJsonAsync("api/users/register", userRegisterRequest);
        }

        public async Task<LoginResponseDto> LoginWitGoogleAsync(UserLoginGoogleRequestDto userLoginGoogleAsyncRequest)
        {
            var response = await GetClient().PostAsJsonAsync("api/users/login-google", userLoginGoogleAsyncRequest);

            if (response.IsSuccessStatusCode == false)
            {
                return new LoginResponseDto
                {
                    response = response
                };
            }

            return await CreatLoginResponseDto(response);
        }

        public async Task<UserInfoDto?> GetUserAsync(int userId)
        {
            var client = GetAuthorizedClient();
            var response = await client.GetAsync($"api/users/{userId}");

            if (response.IsSuccessStatusCode == false)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserInfoDto>();
        }

        public async Task<HttpResponseMessage> ChangeUserInfoAsync(UserChangeInfoRequestDto userChangeInfoRequest)
        {
            var client = GetAuthorizedClient();

            return await client.PostAsJsonAsync("api/users/change-info", userChangeInfoRequest);
        }

        private async Task<LoginResponseDto> CreatLoginResponseDto(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonSerializer.Deserialize<string>(jsonString);

            if (jwtToken == null)
            {
                throw new AuthenticationException("JWT token is null!");
            }

            var handler = new JwtSecurityTokenHandler();

            var userName = handler.ReadJwtToken(jwtToken).Claims
                .First(claim => claim.Type == JwtRegisteredClaimNames.Name)
                .Value;
            var userId = handler.ReadJwtToken(jwtToken).Claims
                .First(claim => claim.Type == JwtRegisteredClaimNames.NameId)
                .Value;

            SetAuthUserToCookie(jwtToken, userName, userId);

            return new LoginResponseDto()
            {
                UserId = userId,
                UserName = userName,
                response = response
            };
        }

        private async void SetAuthUserToCookie(string jwtToken, string userName, string userId)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.NameIdentifier, userId)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }
    }
}