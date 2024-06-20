using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SeniorConnect.Models.Activities;

namespace SeniorConnect.Pages.activity
{
    public class ActivityModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<ActivityDto> Activitys = new();

        public ActivityModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("api/Activity/ActivityList");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activitys = JsonConvert.DeserializeObject<List<ActivityDto>>(content);
            }
        }
    }
}
