using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models.User;
using SeniorConnect.Services;


namespace SeniorConnect.Pages.login
{
    public class IndexModel : PageModel
    {
        private readonly AuthService _authService;

        public IndexModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty] public UserLoginRequestDto UserLoginRequest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var loginResponse = await _authService.LoginAsync(UserLoginRequest);

            if (loginResponse.response?.IsSuccessStatusCode == false)
            {
                ModelState.AddModelError(string.Empty, await loginResponse.response.Content.ReadAsStringAsync());
                return Page();
            }
            
            var redirectAfterLogin = HttpContext.Request.Cookies["redirectAfterLogin"];
            if (redirectAfterLogin != null)
            {
                Response.Cookies.Delete("redirectAfterLogin");
                
                NotificationHelper.SetNotification(
                    TempData,
                    "Welkom " + loginResponse.UserName,
                    NotificationType.success,
                    "U kunt nu inschrijven voor activiteiten."
                );
                
                return Redirect(redirectAfterLogin);
            }

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
            }
        }
    }
}