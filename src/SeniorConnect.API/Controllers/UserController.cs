using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using SeniorConnect.API.Service.UserService;
using SeniorConnect.API.Services.UserService.Interface;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IAuthenticationService _authenticationService;
        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [HttpGet("{user-id}")]
        public ActionResult GetUser()
        {
            try
            {
                int UserId = -1;
                string? rawId = Request.RouteValues["UserId"]?.ToString();
                if (string.IsNullOrEmpty(rawId))
                    ModelState.AddModelError("No UserId Found", "No userId was send");
                if (!int.TryParse(rawId, out UserId))
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");
                if (UserId < 0)
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");

                return Ok("shows a spesific activity: " + UserId);
            }
            catch (Exception)
            {
                throw;
            }
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

            LoginResponse loginResponse = new LoginResponse
            {
                UserId = user.UserId.ToString(),
                UserName = user.Email
            };

            return Ok(loginResponse);
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

        [HttpPost("forgot-password")]
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

        [HttpPost("reset-password")]
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

            LoginResponse loginResponse = new LoginResponse
            {
                UserId = user.UserId.ToString(),
                UserName = user.Email
            };

            return Ok(loginResponse);
        }
    }
}
