using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using NuGet.Common;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Service.UserService;
using SeniorConnect.API.Services.UserService.Interface;

namespace UnitTestSeniorConnect.SeniorConnectAPI.Services
{
    public class AuthenticationServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<DataContext> _mockContext;
        private readonly Mock<ITokenService> _mockTokenService;


        public AuthenticationServiceTest()
        {
            _fixture = new Fixture();
            _mockContext = new Mock<DataContext>(new DbContextOptions<DataContext>());
            _mockTokenService = new Mock<ITokenService>();
        }

        private Fixture UserFixtureWithOutReference()
        {
            var userFixture = new Fixture();
            userFixture.Customize<User>(user => user
            .Without(u => u.ActivityUsers)
            .Without(u => u.Activities)
            );

            userFixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return userFixture;
        }

        private async Task<DataContext> GetDataContextMock()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: $"SeniorConnectDbTest_{Guid.NewGuid()}")
                .Options;

            var dataContext = new DataContext(options);

            dataContext.Database.EnsureCreated();

            return dataContext;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        [Fact]
        public async Task CreateUserShouldCreateUserWithPasswordHashAndSalt()
        {
            var password = "Test12345";
            var token = "TestToken";

            // Arrange
            var userRegisterRequestMock = _fixture.Build<UserRegisterRequest>()
                .With(u => u.Password, password)
                .Create();

            var mockDbContext = await GetDataContextMock();
            _mockTokenService.Setup(t => t.CreateRandomToken()).Returns(token);

            // Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            await authenticationService.CreateUser(userRegisterRequestMock);

            var result = await mockDbContext.Users.FirstOrDefaultAsync(u => u.Email == userRegisterRequestMock.Email);


            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.PasswordHash);
            Assert.NotNull(result.PasswordSalt);

            using (var hmac = new HMACSHA512(result.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterRequestMock.Password));

                Assert.Equal(result.PasswordHash, computedHash);
            }

            Assert.Equal(result.Email, userRegisterRequestMock.Email);
            Assert.Equal(result.FirstName, userRegisterRequestMock.FirstName);
            Assert.Equal(result.LastName, userRegisterRequestMock.LastName);
            Assert.Equal(result.Preposition, userRegisterRequestMock.Preposition);
            Assert.Equal(result.VerificationToken, token);
        }


        [Fact]
        public async void VerifyPasswordHashWillReturnFalse()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            var userLoginRequestMock = _fixture.Create<UserLoginRequest>();
            var userMock = UserFixtureWithOutReference().Create<User>();

            //Act
            var result = authenticationService.VerifyPasswordHash(userLoginRequestMock, userMock);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void VerifyPasswordHashWillReturnTrue()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            var userLoginRequestMock = _fixture.Create<UserLoginRequest>();

            var mockUser = new Fixture();

            CreatePasswordHash(
                userLoginRequestMock.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );

            mockUser.Customize<User>(
                u => u.With(u => u.PasswordHash, passwordHash)
                    .With(u => u.PasswordSalt, passwordSalt)
                    .Without(u => u.ActivityUsers)
                    .Without(u => u.Activities)
                    );
            mockUser.Behaviors.Add(new OmitOnRecursionBehavior());

            var userMock = mockUser.Create<User>();


            //Act
            var result = authenticationService.VerifyPasswordHash(userLoginRequestMock, userMock);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void CreateResetPasswordToken()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var token = "TestToken";
            var userMock = UserFixtureWithOutReference().Create<User>();
            _mockTokenService.Setup(t => t.CreateRandomToken()).Returns(token);

            mockDbContext.Users.Add(userMock);
            await mockDbContext.SaveChangesAsync();
            //Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            authenticationService.CreateResetPasswordToken(userMock);
            var result = await mockDbContext.Users.FirstOrDefaultAsync(user => user.Email == userMock.Email);

            //Assert
            Assert.Equal(result.PasswordResetToken, token);
            Assert.True(result.ResetTokenExpires > DateTime.Now);
        }

        [Fact]
        public async void ResetPasswordWhenUserIsNullAndReturnFalse()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var userPasswordRequestResetMock = _fixture.Create<UserPasswordResetRequest>();
            //Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            bool result = await authenticationService.ResetPassword(userPasswordRequestResetMock);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void ResetPasswordWhenResetTokenIsExpiresAndReturnFalse()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var userPasswordRequestResetMock = _fixture.Create<UserPasswordResetRequest>();
            var userMock = UserFixtureWithOutReference().Create<User>();

            userPasswordRequestResetMock.Token = userMock.PasswordResetToken;
            userMock.ResetTokenExpires = DateTime.Now.AddDays(-1);

            mockDbContext.Users.Add(userMock);
            await mockDbContext.SaveChangesAsync();

            //Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            bool result = await authenticationService.ResetPassword(userPasswordRequestResetMock);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void ResetPasswordWhenConfirmPasswordNotEqualToPasswordAndReturnFalse()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var userPasswordRequestResetMock = _fixture.Create<UserPasswordResetRequest>();

            //Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            bool result = await authenticationService.ResetPassword(userPasswordRequestResetMock);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void ResetPasswordWhenResetSuccessfullyAndReturnTrue()
        {
            //Arrange
            var mockDbContext = await GetDataContextMock();
            var userPasswordRequestResetMock = _fixture.Create<UserPasswordResetRequest>();
            var userMock = UserFixtureWithOutReference().Create<User>();
            var passwordHashMock = userMock.PasswordHash;
            var passwordSaltMock = userMock.PasswordSalt;

            userPasswordRequestResetMock.Token = userMock.PasswordResetToken;
            userMock.ResetTokenExpires = DateTime.Now.AddDays(1);
            userPasswordRequestResetMock.ConfirmPassword = userPasswordRequestResetMock.Password;


            mockDbContext.Users.Add(userMock);
            await mockDbContext.SaveChangesAsync();

            //Act
            var authenticationService = new AuthenticationService(mockDbContext, _mockTokenService.Object);
            bool result = await authenticationService.ResetPassword(userPasswordRequestResetMock);
            var userAfterChangePassword = await mockDbContext.Users.FirstOrDefaultAsync(user => user.Email == userMock.Email);

            //Assert
            Assert.True(result);
            Assert.Null(userAfterChangePassword.PasswordResetToken);
            Assert.Null(userAfterChangePassword.ResetTokenExpires);
            Assert.NotEqual(userAfterChangePassword.PasswordHash, passwordHashMock);
            Assert.NotEqual(userAfterChangePassword.PasswordSalt, passwordSaltMock);
        }
    }
}
