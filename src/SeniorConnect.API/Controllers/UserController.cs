using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Services.UserService.Interface;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IAuthenticationService _authenticationService;

        private readonly ITokenService _tokenService;

        public UserController(
            IUserService userService,
            IAuthenticationService authenticationService,
            ITokenService tokenService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpGet("{user-id}"),Authorize]
        public async Task<IActionResult> GetUser()
        {
            int userId = Convert.ToInt32((string)Request.RouteValues["user-id"]);
            User? user = await _userService.FindUserById(userId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            GetUserInfoResponse userResponse = new GetUserInfoResponse()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                PhoneNumber = user.Phonenumber,
                LastName = user.LastName,
                Preposition = user.Preposition
            };

            return Ok(userResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_userService.IsUserEmailExist(request) == true)
            {
                return BadRequest("Het opgegeven e-mailadres is al in gebruik. Probeer een ander e-mailadres");
            }

            //TODO: for future after user is created send email to user for verification
            _authenticationService.CreateUser(request);

            return Ok("Account is aan gemaakt.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            User? user = await _userService.FindUser(request);

            if (user == null)
            {
                return BadRequest("De combinatie van e-mailadres en wachtwoord is niet geldig.");
            }

            //if (user.VerifiedAt == null)
            //{
            //    return BadRequest("Gebruiker is niet geverifieerd!");
            //}

            if (_authenticationService.VerifyPasswordHash(request, user) == false)
            {
                return BadRequest("De combinatie van e-mailadres en wachtwoord is niet geldig.");
            }

            var jwtToken = _tokenService.CreateJwtTokenForLoginUser(user);
            
            return Ok(jwtToken);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            bool isVerify = await _authenticationService.VerifyToken(token);

            if (isVerify == false)
            {
                return BadRequest("Invalid token!");
            }

            return Ok("Gebruiker is geverifieerd!");
        }

        [HttpPost("forgot-password"), Authorize]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            User? user = await _userService.FindUser(new UserLoginRequest { Email = email });

            if (user == null)
            {
                return BadRequest("Gebruiker niet gevonden!");
            }

            _authenticationService.CreateResetPasswordToken(user);

            return Ok("Een e-mail reset wachtwoord is verzonden naar uw e-mailadres.");
        }

        [HttpPost("reset-password"), Authorize]
        public async Task<IActionResult> ResetPassword(UserPasswordResetRequest resetPasswordRequest)
        {
            bool isResetSuccess = await _authenticationService.ResetPassword(resetPasswordRequest);

            if (isResetSuccess == false)
            {
                return BadRequest("Invalid token");
            }

            return Ok("Uw wachtwoord is gereset.");
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> LoginGoogle(UserLoginGoogleAsyncRequest userLoginGoogleAsyncRequest)
        {
            var user = await _authenticationService.LoginGoogleAccountSync(userLoginGoogleAsyncRequest);

            var jwtToken = _tokenService.CreateJwtTokenForLoginUser(user);

            return Ok(jwtToken);
        }
        
        [HttpPost("change-info"), Authorize]
        public async Task<IActionResult> ChangeInformation(UserChangeInfoRequest userChangeInfoRequest)
        {
            var user = await _userService.FindUserById(userChangeInfoRequest.userId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            await _userService.ChangeUserInformation(userChangeInfoRequest);
            return Ok("Informatie is aangepast.");
        }
    }
}