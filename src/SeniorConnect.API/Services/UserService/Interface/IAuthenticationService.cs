using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using System.Security.Cryptography;

namespace SeniorConnect.API.Services.UserService.Interface
{
    public interface IAuthenticationService
    {

        public void CreateUser(UserRegisterRequest userRegisterRequest);

        public string CreateRandomToken();

        public bool VerifyPasswordHash(UserLoginRequest userLoginRequest, User user);

        public Task<bool> VerifyToken(string token);

        public void CreateResetPasswordToken(User user);

        public  Task<bool> ResetPassword(UserPasswordResetRequest userPasswordResetRequest);

        public Task<User> LoginGoogleAccountSync(UserLoginGoogleAsyncRequest userLoginGoogleAsyncRequest);
    }
}
