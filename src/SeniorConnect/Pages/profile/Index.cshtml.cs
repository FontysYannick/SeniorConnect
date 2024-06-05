using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Models.Users;
using SeniorConnect.Helpers;
using SeniorConnect.Services;
using System.Security.Claims;

namespace SeniorConnect.Pages.profile
{
    public class IndexModel : PageModel
    {
        private readonly AuthService _authService;
        public IndexModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public GetUserInfoResponse? UserInfoResponse { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/login/index");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserInfoResponse = await _authService.GetUserAsync(userId);

            if (UserInfoResponse == null)
            {
                NotificationHelper.SetNotification(TempData, "Something went wrong!", NotificationType.error);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                NotificationHelper.SetNotification(TempData, "You must be logged in to view your profile.", NotificationType.info);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            await _authService.ChangeUserInfoAsync(
                new UserChangeInfoRequest()
                {
                    userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    FirstName = UserInfoResponse.FirstName,
                    LastName = UserInfoResponse.LastName,
                    Preposition = UserInfoResponse.Preposition,
                    PhoneNumber = UserInfoResponse.PhoneNumber
                }
            );

            NotificationHelper.SetNotification(TempData, "Profile updated!", NotificationType.success);

            return RedirectToPage();
        }
    }
}
