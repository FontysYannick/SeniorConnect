﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SeniorConnect.Helpers;
using SeniorConnect.Models.User;
using SeniorConnect.Services;
using System.Security.Claims;

namespace SeniorConnect.Controllers.AuthController
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService authService;
        public AuthController(AuthService _authService)
        {
            authService = _authService;
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            await HttpContext.SignOutAsync();
            NotificationHelper.SetNotification(
                    TempData,
                    "U bent uitgelogd.",
                    NotificationType.success
                );
            return RedirectToAction("Index", "Home");
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult SignInWithGoogle()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/callback-google" }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("/callback-google")]
        public async Task<IActionResult> GoogleLoginCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded || authenticateResult == null)
            {
                NotificationHelper.SetNotification(TempData, "Error", NotificationType.error, "Inloggen met google account is niet gelukt probeert u later opnieuw of inloggen met een account.");
                return RedirectToAction("Index", "Home");
            }

            // Retrieve the user's information
            var emailClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Email);
            var firstNameClaim = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName);
            var LastNameClaim = authenticateResult.Principal.FindFirst(ClaimTypes.Surname);
            var googleIdClaim = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier);

            if (emailClaim == null || googleIdClaim == null)
            {
                return BadRequest("Unable to retrieve user information from Google.");
            }

            var googleUser = new UserLoginGoogleRequestDto()
            {
                FirstName = firstNameClaim.Value,
                LastName = LastNameClaim.Value,
                GoogleEmail = emailClaim.Value,
                GoogleId = googleIdClaim.Value
            };

            //sync google acount to datasbase
            var loginResponse = await authService.LoginWitGoogleAsync(googleUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.UserName),
                new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create new principal with the updated identity
            var principal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the updated user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            NotificationHelper.SetNotification(
                TempData,
                 "Welkom " + loginResponse.UserName,
                NotificationType.success
            );

            // Redirect to a page after successful login
            return RedirectToAction("Index", "Home");
        }
    }
}
