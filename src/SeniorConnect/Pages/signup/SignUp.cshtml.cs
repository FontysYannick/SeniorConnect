using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Models.Users;
using SeniorConnect.Helpers;
using SeniorConnect.Services;

namespace SeniorConnect.Pages.signup
{
    public class signupModel : PageModel
    {

        [BindProperty]
        public UserRegisterRequest UserRegisterRequest { get; set; }

        private readonly AuthService authService;
        public signupModel(AuthService authService)
        {
            this.authService = authService;
        }

        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                Response.Redirect("/");

                return;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registerResponse = await authService.RegisterAsync(UserRegisterRequest);
            var message = await registerResponse.Content.ReadAsStringAsync();
            message = message.Trim('"');

            if (registerResponse.IsSuccessStatusCode == false)
            {
                ModelState.AddModelError(string.Empty, message);
                return Page();
            }

            NotificationHelper.SetNotification(
                    TempData, 
                    message, 
                    NotificationType.success, 
                    "U kunt nu inloggen met u aangemaakte account."
                );

            return RedirectToPage("/login/index");
        }
    }
}
