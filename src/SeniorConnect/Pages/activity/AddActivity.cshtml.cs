using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Models.Activities;
using System;

namespace SeniorConnect.Pages.activity
{
    public class AddActivityModel() : PageModel
    {
        public ActivityDto abActivity { get; set; }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            DateTime.TryParse(Request.Form["Date"], out DateTime dateTime);
            int.TryParse(Request.Form["MaxParticipants"], out int maxPart);
            int userId = 1;
            //var user = Request.Form["userId"];
        }
    }
}
