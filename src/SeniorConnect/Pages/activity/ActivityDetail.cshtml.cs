using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System.Net.Http;
using System.Security.Claims;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel : PageModel
    {
        public ActivityDto Activity = new();

        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet(int Id)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/Activity/" + Id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activity = JsonConvert.DeserializeObject<ActivityDto>(content);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            AddActivityUserDTO AA = new() {
                UserId = userId,
                ActivityId = Activity.ActivityId
            };

            var response = await client.PostAsJsonAsync("/ActivityController/AddUserToActivity", AA);

            NotificationHelper.SetNotification(
                TempData,
                "U ben ingeschreven voor " + Activity.Title,
                NotificationType.success
            );

            return RedirectToPage("/calendar/Calendar");
        }
    }
}
