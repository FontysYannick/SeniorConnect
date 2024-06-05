using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Controllers;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using System.Net.Http;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _dataContext;
        public Activity Activity = new();

        public ActivityDetailModel(IHttpClientFactory httpClientFactory, DataContext dataContext)
        {
            _httpClientFactory = httpClientFactory;
            _dataContext = dataContext;
        }

        public async Task OnGet(int Id)
        {
            Activity = _dataContext.Activities.Include(a => a.Organizer).Where(a => a.ActivityId == Id).FirstOrDefault();

            //var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
            //var response = await client.GetAsync("/ActivityController/Activity/"+Id);

            //Activity = await response.Content.ReadFromJsonAsync<Activity>();
        }

        public async Task<IActionResult> OnPost()
        {
            int.TryParse(Request.Form["activityId"], out int activityid);
            int userId = 1;
            //var user = Request.Form["userId"];

            ActivityUsers AU = new()
            {
                ActivityId = activityid,
                UserId = userId
            };

            _dataContext.ActivityUsers.Add(AU);
            await _dataContext.SaveChangesAsync();

            return RedirectToPage("/index");
        }
    }
}
