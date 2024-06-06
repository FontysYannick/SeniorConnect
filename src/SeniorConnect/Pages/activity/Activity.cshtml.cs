using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System.Net.Http;

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
            var response = await client.GetAsync("/ActivityController/ActivityList");
            
        }
    }
}
