using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SeniorConnect.API.Controllers;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Services.UserService.Interface;

namespace UnitTestSeniorConnect.SeniorConnectAPI.Controller
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAuthenticationService> _autServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private Fixture _fixture;

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _autServiceMock = new Mock<IAuthenticationService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _fixture = new Fixture();
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

        [Fact]
        public async void RegisterUserWhenUserAccountAlreadyExistReturnBadRequest()
        {
            //Arrange
            var userRegisterRequest = _fixture.Create<UserRegisterRequest>();
            _userServiceMock.Setup(s => s.IsUserEmailExist(userRegisterRequest)).Returns(true);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Register(userRegisterRequest);
            var badRequestResult = result as BadRequestObjectResult;
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Het opgegeven e-mailadres is al in gebruik. Probeer een ander e-mailadres",
                badRequestResult.Value);
        }

        [Fact]
        public async void RegisterUserWhenUserAccountDoesNotExistReturnOk()
        {
            //Arrange
            var userRegisterRequest = _fixture.Create<UserRegisterRequest>();
            _userServiceMock.Setup(s => s.IsUserEmailExist(userRegisterRequest)).Returns(false);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Register(userRegisterRequest);
            var response = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Account is aan gemaakt.", response.Value);
        }

        [Fact]
        public async void LoginUserWhenUserNotFoundReturnBadRequest()
        {
            //Arrange
            var userLoginRequest = _fixture.Create<UserLoginRequest>();
            _userServiceMock.Setup(s => s.FindUser(userLoginRequest)).ReturnsAsync((User)null);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Login(userLoginRequest);
            var repsonse = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("De combinatie van e-mailadres en wachtwoord is niet geldig.", repsonse.Value);
        }

        [Fact]
        public async void LoginUserWhenVerificationPasswordHashIsFalseReturnBadRequest()
        {
            //Arrange
            var userLoginRequest = _fixture.Create<UserLoginRequest>();
            var user = UserFixtureWithOutReference().Create<User>();

            _userServiceMock.Setup(s => s.FindUser(userLoginRequest)).ReturnsAsync(user);
            _autServiceMock.Setup(s => s.VerifyPasswordHash(userLoginRequest, user)).Returns(false);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Login(userLoginRequest);
            var repsonse = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("De combinatie van e-mailadres en wachtwoord is niet geldig.", repsonse.Value);
        }

        [Fact]
        public async void LoginUserWhenLoginSuccessfullyAndReturnOkRequestWithJwtToken()
        {
            //Arrange
            var userLoginRequest = _fixture.Create<UserLoginRequest>();
            var user = UserFixtureWithOutReference().Create<User>();

            _userServiceMock.Setup(s => s.FindUser(userLoginRequest)).ReturnsAsync(user);
            _autServiceMock.Setup(s => s.VerifyPasswordHash(userLoginRequest, user)).Returns(true);
            _tokenServiceMock.Setup(s => s.CreateJwtTokenForLoginUser(user)).Returns("JWTToken");
            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Login(userLoginRequest);
            var repsonse = result as OkObjectResult;
            var value = repsonse.Value as string;
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("JWTToken", value);
        }

        [Fact]
        public async void UserVerifyWhenTokenIsInvalidReturnBadRequest()
        {
            //Arrange
            var token = _fixture.Create<string>();
            _autServiceMock.Setup(s => s.VerifyToken(token)).ReturnsAsync(false);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Verify(token);
            var repsonse = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid token!", repsonse.Value);
        }

        [Fact]
        public async void UserVerifyWhenTokenIsValidOkRequest()
        {
            //Arrange
            var token = _fixture.Create<string>();
            _autServiceMock.Setup(s => s.VerifyToken(token)).ReturnsAsync(true);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.Verify(token);
            var repsonse = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Gebruiker is geverifieerd!", repsonse.Value);
        }


        [Fact]
        public async void UserForgotPasswordWhenUserNotFoundReturnBadRequest()
        {
            //Arrange
            var email = _fixture.Create<string>();
            _userServiceMock.Setup(s => s.FindUser(It.IsAny<UserLoginRequest>())).ReturnsAsync((User)null);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.ForgotPassword(email);
            var repsonse = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Gebruiker niet gevonden!", repsonse.Value);
        }

        [Fact]
        public async void UserForgotPasswordWhenUserFoundReturnOkRequest()
        {
            //Arrange
            var email = _fixture.Create<string>();
            var user = UserFixtureWithOutReference().Create<User>();

            _userServiceMock.Setup(s => s.FindUser(It.IsAny<UserLoginRequest>())).ReturnsAsync(user);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.ForgotPassword(email);
            var repsonse = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Een e-mail reset wachtwoord is verzonden naar uw e-mailadres.", repsonse.Value);
        }

        [Fact]
        public async void UserResetPasswordWhenIsResetSuccessReturnFalseWithBadRequest()
        {
            //Arrange
            var userPasswordResetRequest = _fixture.Create<UserPasswordResetRequest>();
            _autServiceMock.Setup(s => s.ResetPassword(userPasswordResetRequest)).ReturnsAsync(false);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.ResetPassword(userPasswordResetRequest);
            var repsonse = result as BadRequestObjectResult;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid token", repsonse.Value);
        }

        [Fact]
        public async void UserResePasswordWhenIsResetSuccessReturnTrueWithOkeRequest()
        {
            //Arrange
            var userPasswordResetRequest = _fixture.Create<UserPasswordResetRequest>();
            _autServiceMock.Setup(s => s.ResetPassword(userPasswordResetRequest)).ReturnsAsync(true);

            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.ResetPassword(userPasswordResetRequest);
            var repsonse = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Uw wachtwoord is gereset.", repsonse.Value);
        }

        [Fact]
        public async void UserLoginGoogleReturnJwtTokenWithOkRequest()
        {
            //Arrange
            var userLoginGoogleAsyncRequest = _fixture.Create<UserLoginGoogleAsyncRequest>();
            var user = UserFixtureWithOutReference().Create<User>();

            _autServiceMock.Setup(s => s.LoginGoogleAccountSync(userLoginGoogleAsyncRequest)).ReturnsAsync(user);
            _tokenServiceMock.Setup(s => s.CreateJwtTokenForLoginUser(user)).Returns("JWTToken");
            
            //Act
            var userController =
                new UserController(_userServiceMock.Object, _autServiceMock.Object, _tokenServiceMock.Object);
            var result = await userController.LoginGoogle(userLoginGoogleAsyncRequest);
            var repsonse = result as OkObjectResult;
            var value = repsonse.Value as string;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(value, "JWTToken");
        }
    }
}