using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SeniorConnect.Models.Activities;
using System.Security.Claims;
using SeniorConnect.Helpers;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ActivityDto> Activitys = new();
        
        [BindProperty]
        public int ActivityId { get; set; }

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
            var token = HttpContext.Request.Cookies["JwtToken"];
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("/ActivityController/GetUserToActivity/" + userId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activitys = JsonConvert.DeserializeObject<List<ActivityDto>>(content);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return RedirectToPage("/");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var token = HttpContext.Request.Cookies["JwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"/ActivityController/DeleteActivityUser/{userId}/{ActivityId}" );

            if (response.IsSuccessStatusCode == false)
            {
                NotificationHelper.SetNotificationSomethingWentWrong(TempData);
                return RedirectToPage("/calendar/calendar");
            }
            
            NotificationHelper.SetNotification(TempData, "De activiteit is geannuleerd", NotificationType.success);
            return RedirectToPage("/calendar/calendar");
        }
    }
}
