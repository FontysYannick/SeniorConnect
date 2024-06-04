using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Users;
using SeniorConnect.Helpers;
using System.Net.Http;

namespace SeniorConnect.Pages.activity
{
    public class ActivityModel : PageModel
    {
        public List<Activity> Activitys = new();

        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/ActivityList");
            
            Activitys = await response.Content.ReadFromJsonAsync<List<Activity>>();
        }
    }
}
