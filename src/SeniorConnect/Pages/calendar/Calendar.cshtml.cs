using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.Models.Activities;

namespace SeniorConnect.Pages.calendar
{
    public class CalendarModel() : PageModel
    {
        public List<ActivityDto> Activitys = new();

        public void OnGet()
        {

        }
    }
}
