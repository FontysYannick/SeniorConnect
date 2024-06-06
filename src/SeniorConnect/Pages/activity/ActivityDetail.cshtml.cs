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
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            var response = await client.GetAsync("/ActivityController/Activity/1");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Activity = JsonConvert.DeserializeObject<ActivityDto>(content);
            }
        }

        public void OnPost()
        {
        }
    }
}
