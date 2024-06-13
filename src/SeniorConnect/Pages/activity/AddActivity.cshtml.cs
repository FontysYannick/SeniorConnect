using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System.Security.Claims;

namespace SeniorConnect.Pages.activity
{
    public class AddActivityModel : PageModel
    {
        [BindProperty]
        public AddActivityRequestDto addActivityRequestDto { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public AddActivityModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                Response.Redirect("/");
                return;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToPage("/");
            }

            ModelState.ClearValidationState(nameof(addActivityRequestDto));
            if (!TryValidateModel(addActivityRequestDto, nameof(addActivityRequestDto)))
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            addActivityRequestDto.OrganizerId = userId;

            var response = await client.PostAsJsonAsync("/ActivityController/PostActivity", addActivityRequestDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kan activiteit niet maken. Probeer het later opnieuw");
                return Page();
            }

            NotificationHelper.SetNotification(TempData, $"De Activiteit {addActivityRequestDto.Title} is aangemaakt", NotificationType.success);

            return RedirectToPage("/activity/Activity");
        }
    }
}
