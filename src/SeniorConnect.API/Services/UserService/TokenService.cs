using SeniorConnect.API.Data;
using SeniorConnect.API.Services.UserService.Interface;
using System.Security.Cryptography;

namespace SeniorConnect.API.Services.UserService
{
    public class TokenService: ITokenService
    {
        DataContext _dataContext;
        public TokenService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string CreateRandomToken()
        {
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            bool isTokenExist = _dataContext.Users.Any(user => user.PasswordResetToken == token || user.VerificationToken == token);
            if (isTokenExist == true)
            {
                return CreateRandomToken();
            }

            return token;
        }
    }
}
