using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models.Activities;
using System;
using System.Net.Http;
using System.Security.Claims;

namespace SeniorConnect.Pages.activity
{
    public class AddActivityModel : PageModel
    {
        public AddActivityRequestDto addActivityRequestDto { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public AddActivityModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("SeniorConnectAPI");


            string title = Request.Form["Title"];
            DateTime.TryParse(Request.Form["Date"], out DateTime dateTime); 
            int.TryParse(Request.Form["MaxParticipants"], out int maxPart);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 

            AddActivityRequestDto AA = new() { 
                Title = title, 
                OrganizerId = userId, 
                Description = Request.Form["Description"],
                Image = Request.Form["Image"], 
                Date = dateTime, 
                Place = Request.Form["Place"], 
                MaxParticipants = maxPart, 
                Awards = Request.Form["Awards"]
            };

            var response = await client.PostAsJsonAsync("/ActivityController/PostActivity",AA );

            NotificationHelper.SetNotification(TempData, "De Activiteit " + title + " is aangemaakt", NotificationType.success); 
            
            return RedirectToPage("/activity/Activity");

            var test = response;
            //DateTime.TryParse(Request.Form["Date"], out DateTime dateTime);
            //int.TryParse(Request.Form["MaxParticipants"], out int maxPart);
            //int userId = 1;
            //var user = Request.Form["userId"];
        }
    }
}
