using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SeniorConnect.Models.Activities;
using System.Net.Http;

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
        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/Activity/1");
            int.TryParse(Request.Form["activityId"], out int activityid);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activity = JsonConvert.DeserializeObject<ActivityDto>(content);
            }
        }

            _dataContext.ActivityUsers.Add(AU);
            await _dataContext.SaveChangesAsync();

            var name = _dataContext.Activities.Where(a => a.ActivityId == activityid).Select(a => a.Title).FirstOrDefault();

            NotificationHelper.SetNotification(
                TempData,
                "U ben ingeschreven voor " + name,
                NotificationType.success
            );

            return RedirectToPage("/calendar/Calendar");
        }
    }
}
