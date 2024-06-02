using Azure.Core;
using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Entities;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using SeniorConnect.API.Services.UserService.Interface;


namespace SeniorConnect.API.Service.UserService
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AuthenticationService(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task CreateUser(UserRegisterRequest userRegisterRequest)
        {
            this.CreatePasswordHash(
                userRegisterRequest.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            var userNew = new User
            {
                FirstName = userRegisterRequest.FirstName,
                LastName = userRegisterRequest.LastName,
                Preposition = userRegisterRequest.Preposition,
                Email = userRegisterRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = _tokenService.CreateRandomToken(),
            };

            _dataContext.Users.Add(userNew);
            await _dataContext.SaveChangesAsync();
        }

        public bool VerifyPasswordHash(UserLoginRequest userLoginRequest, User user)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLoginRequest.Password));

                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        public async Task<bool> VerifyToken(string token)
        {
            User? user = await _dataContext.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                return false;
            }

            user.VerifiedAt = DateTime.Now;
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public void CreateResetPasswordToken(User user)
        {
            user.PasswordResetToken = _tokenService.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _dataContext.SaveChangesAsync();
        }

        public async Task<bool> ResetPassword(UserPasswordResetRequest userPasswordResetRequest)
        {
            if (userPasswordResetRequest.ConfirmPassword != userPasswordResetRequest.Password)
            {
                return false;
            }

            User? user = await _dataContext.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == userPasswordResetRequest.Token);

            if (user == null)
            {
                return false;
            }

            if (user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }

            this.CreatePasswordHash(
                userPasswordResetRequest.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _dataContext.SaveChangesAsync();

            return true;
        }


        public async Task<User> LoginGoogleAccountSync(UserLoginGoogleAsyncRequest userLoginGoogleAsyncRequest)
        {
            User? userExist = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginGoogleAsyncRequest.GoogleEmail);

            if (userExist != null)
            {
                if (userExist.GoogleId == userLoginGoogleAsyncRequest.GoogleId)
                {
                    return userExist;
                }

                userExist.GoogleId = userLoginGoogleAsyncRequest.GoogleId;
                await _dataContext.SaveChangesAsync();

                return userExist;
            }

            var userNew = new User
            {
                FirstName = userLoginGoogleAsyncRequest.FirstName,
                LastName = userLoginGoogleAsyncRequest.LastName,
                Email = userLoginGoogleAsyncRequest.GoogleEmail,
                VerificationToken = _tokenService.CreateRandomToken(),
                GoogleId = userLoginGoogleAsyncRequest.GoogleId,
            };

            _dataContext.Users.Add(userNew);
            await _dataContext.SaveChangesAsync();

            return userNew;
        }
    }
}
