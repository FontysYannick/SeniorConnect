using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext dataContext = dataContext;

        public Activity Activity = new();

        public void OnGet(int Id)
        {
            Activity = dataContext.Activities.Include(a => a.Organizer).Where(a => a.ActivityId == Id).FirstOrDefault();
        }

        public void OnPost()
        {
            int.TryParse(Request.Form["activityId"], out int activityid);
            int userId = 1;
            //var user = Request.Form["userId"];

            ActivityUsers AU = new()
            {
                ActivityId = activityid,
                UserId = userId
            };

            dataContext.ActivityUsers.Add(AU);
            dataContext.SaveChanges();
        }
    }
}
