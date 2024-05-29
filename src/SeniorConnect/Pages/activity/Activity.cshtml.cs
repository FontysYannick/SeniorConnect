using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.API.Data;
using SeniorConnect.API.Entities;
using SeniorConnect.Helpers;

namespace SeniorConnect.Pages.activity
{
    public class ActivityModel(DataContext dataContext) : PageModel
    {
        public readonly DataContext dataContext = dataContext;

        public List<Activity> Activitys = new();

        public void OnGet()
        {
            Activitys = dataContext.Activities.ToList();
        }
    }
}
