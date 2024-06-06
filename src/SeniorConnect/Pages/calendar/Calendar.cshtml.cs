using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SeniorConnect.Models.Activities;
using System.Net.Http;
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


        public async Task OnGet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/GetUserToActivity/" + userId);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activitys = JsonConvert.DeserializeObject<List<ActivityDto>>(content);
            }
        }
    }
}
