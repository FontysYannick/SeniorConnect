using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.Helpers;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext dataContext = dataContext;

        public List<Activity> Activitys = new();

        public void OnGet()
        {
            int userId = 1;
            Activitys = dataContext.ActivityUsers.Where(a => a.UserId == userId).Include(a => a.Activity).Select(a => a.Activity).ToList();

        }
    }
}
