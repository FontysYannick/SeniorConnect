using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel : PageModel
    {
        public List<Activity> Activitys = new();
        SetData data = new SetData();

        public void OnGet()
        {
            Activitys = data.setActivty();
        }
    }
}
