using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Services.UserService.Interface
{
    public interface ITokenService
    {
        string CreateRandomToken();

        string CreateJwtTokenForLoginUser(User user);
    }
}
