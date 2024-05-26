using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Entities;
using System.Security.Claims;

namespace SeniorConnect.Controllers.AuthController
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [Route("[controller]/[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("[controller]/[action]")]
        [HttpGet]
        public IActionResult SignInWithGoogle()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/signin-google" }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/signin-google")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
            {
                // Handle authentication failure
                return RedirectToAction("Index", "Home"); // Redirect to home page
            }

            // Retrieve the user's information
            var emailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            var nameClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Name);

            if (emailClaim == null || nameClaim == null)
            {
                return BadRequest("Unable to retrieve user information from Google.");
            }

            var email = emailClaim.Value;
            var name = nameClaim.Value;

            // Check if the user already exists in the database
        

            
                // Create a new user object


                // Add the user to the database

                // Optionally update existing user information

            // Redirect to a page after successful login
            return RedirectToAction("Index", "Home"); // Redirect to home page
        }
    }
}
