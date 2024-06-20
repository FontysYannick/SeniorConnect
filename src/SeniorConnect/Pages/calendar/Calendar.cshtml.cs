using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SeniorConnect.Models.Activities;
using System.Security.Claims;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ActivityDto> Activitys = new();

        public CalendarModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToPage("/");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/api/Activity/GetUserToActivity/" + userId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activitys = JsonConvert.DeserializeObject<List<ActivityDto>>(content);
            }

            return Page();
        }
    }
}
