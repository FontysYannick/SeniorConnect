using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SeniorConnect.API.Data;
using SeniorConnect.API.Services.UserService.Interface;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SeniorConnect.API.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SeniorConnect.API.Services.UserService
{
    public class TokenService : ITokenService
    {
        private readonly DataContext _dataContext;

        private readonly IConfiguration _configuration;

        public TokenService(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public string CreateRandomToken()
        {
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            bool isTokenExist =
                _dataContext.Users.Any(user => user.PasswordResetToken == token || user.VerificationToken == token);
            if (isTokenExist == true)
            {
                return CreateRandomToken();
            }

            return token;
        }

        public string CreateJwtTokenForLoginUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Name,  $"{user.FirstName} {user.Preposition ?? " "} {user.LastName}")
            };

            var keyBytes = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token") ?? string.Empty);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}