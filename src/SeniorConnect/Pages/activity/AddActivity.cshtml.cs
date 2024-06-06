using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.Helpers;
using System;
using System.Security.Claims;

namespace SeniorConnect.Pages.activity
{
    public class AddActivityModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext _dataContext = dataContext;
        public AbstractActivity abActivity { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            string title = Request.Form["Title"];
            DateTime.TryParse(Request.Form["Date"], out DateTime dateTime);
            int.TryParse(Request.Form["MaxParticipants"], out int maxPart);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Activity AA = new()
            {
                Title = title,
                OrganizerId = userId,
                Description = Request.Form["Description"],
                Image = Request.Form["Image"],
                Date = dateTime,
                Place = Request.Form["Place"],
                MaxParticipants = maxPart,
                Awards = Request.Form["Awards"],
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _dataContext.Activities.Add(AA);
            await _dataContext.SaveChangesAsync();

            NotificationHelper.SetNotification(
                TempData,
                "De Activiteit " + title + " is aangemaakt" ,
                NotificationType.success
            );

            return RedirectToPage("/activity/Activity");
        }
    }
}
