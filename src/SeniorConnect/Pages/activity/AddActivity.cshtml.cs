using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.API.Models.Activity;
using System;

namespace SeniorConnect.Pages.activity
{
    public class AddActivityModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext dataContext = dataContext;
        public AbstractActivity abActivity { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            DateTime.TryParse(Request.Form["Date"], out DateTime dateTime);
            int.TryParse(Request.Form["MaxParticipants"], out int maxPart);
            int userId = 1;
            //var user = Request.Form["userId"];

            Activity AA = new()
            {
                Title = Request.Form["Title"],
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

            dataContext.Activities.Add(AA);
            dataContext.SaveChanges();
        }
    }
}
