using Azure.Core;
using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Entities;
using System.Security.Cryptography;


namespace SeniorConnect.API.Service.UserService
{
public class AuthenticationService
    {
        private readonly DataContext _dataContext;

        public AuthenticationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void CreateUser(UserRegisterRequest userRegisterRequest)
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
                Email = userRegisterRequest.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = this.CreateRandomToken()
            };

            _dataContext.Users.Add(userNew);
            _dataContext.SaveChangesAsync();
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
            user.PasswordResetToken = this.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            _dataContext.SaveChangesAsync();
        }

        public async Task<bool> ResetPassword(UserPasswordResetRequest userPasswordResetRequest)
        {
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
    }
}
