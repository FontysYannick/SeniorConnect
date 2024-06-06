using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Models.Users;
using SeniorConnect.Helpers;
using SeniorConnect.Services;
using System.Security.Claims;

namespace SeniorConnect.Pages.login
{
    public class IndexModel : PageModel
    {

        private readonly AuthService _authService;

        public IndexModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public UserLoginRequest UserLoginRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginResponse = await _authService.LoginAsync(UserLoginRequest);

            if (loginResponse.UserName == "")
            {
                ModelState.AddModelError(string.Empty, await loginResponse.response.Content.ReadAsStringAsync());
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginResponse.UserName),
                new Claim(ClaimTypes.NameIdentifier, loginResponse.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddYears(1)
            };


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
                );

            NotificationHelper.SetNotification(
                    TempData,
                    "Welkom " + loginResponse.UserName,
                    NotificationType.success
                );

            return RedirectToPage("/index");
        }

        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect("/");
                return;
            }
        }
    }
}
