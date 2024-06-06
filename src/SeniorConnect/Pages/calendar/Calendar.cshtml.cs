using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.Helpers;
using System.Security.Claims;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext _dataContext = dataContext;

        public List<Activity> Activitys = new();

        public void OnGet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Activitys = _dataContext.ActivityUsers.Where(a => a.UserId == userId && a.Activity.Date > DateTime.Now).Include(a => a.Activity).Select(a => a.Activity).OrderBy(a => a.Date).ToList();

        }
    }
}
