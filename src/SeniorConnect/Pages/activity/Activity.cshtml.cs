using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System.Net.Http;

namespace SeniorConnect.Pages.activity
{
    public class ActivityModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _dataContext;

        public List<ActivityDto> Activitys = new();
        public ActivityModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/ActivityList");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activitys = JsonConvert.DeserializeObject<List<ActivityDto>>(content);
            }
        }
    }
}
