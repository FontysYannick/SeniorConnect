using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models.User;
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

        [BindProperty] public UserInfoDto? UserInfoResponse { get; set; }

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
                NotificationHelper.SetNotificationSomethingWentWrong(TempData);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                NotificationHelper.SetNotification(TempData, "You must be logged in to view your profile.",
                    NotificationType.info);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            var response = await _authService.ChangeUserInfoAsync(
                new UserChangeInfoRequestDto()
                {
                    userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    FirstName = UserInfoResponse.FirstName,
                    LastName = UserInfoResponse.LastName,
                    Preposition = UserInfoResponse.Preposition,
                    PhoneNumber = UserInfoResponse.PhoneNumber
                }
            );

            if (response.IsSuccessStatusCode == false)
            {
                NotificationHelper.SetNotification(
                    TempData,
                    "Something went wrong!",
                    NotificationType.error,
                    "Our team is working on it. Please try again later."
                );

                return RedirectToPage();
            }

            NotificationHelper.SetNotification(TempData, "Profile updated!", NotificationType.success);

            return RedirectToPage();
        }
    }
}