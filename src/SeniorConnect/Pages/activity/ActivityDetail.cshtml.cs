using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System.Security.Claims;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel : PageModel
    {
        public ActivityDto Activity = new();

        private readonly IHttpClientFactory _httpClientFactory;

        public string address { get; set; }

        public string zipCode { get; set; }

        public ActivityDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet(int Id)
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("api/Activity/Activity/" + Id);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activity = JsonConvert.DeserializeObject<ActivityDto>(content);

                var part = Activity.Place?.Split(", ");
                if (part?.Length >= 2)
                {
                    address = part[0];
                    zipCode = part[1];

                    return;
                }

                address = Activity.Place;
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            int.TryParse(Request.Form["activityId"], out int activityid);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            AddActivityUserDTO AA = new()
            {
                UserId = userId,
                ActivityId = activityid
            };

            var response = await client.PostAsJsonAsync("/api/Activity/AddUserToActivity", AA);

            NotificationHelper.SetNotification(
                TempData,
                "U ben ingeschreven voor " + Request.Form["acitivtyTitle"],
                NotificationType.success
            );

            return RedirectToPage("/calendar/Calendar");
        }
    }
}
