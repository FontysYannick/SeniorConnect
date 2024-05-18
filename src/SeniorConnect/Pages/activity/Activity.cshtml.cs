using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SeniorConnect.Pages.activity
{
    public class ActivityModel : PageModel
    {
        public List<Activity> Activitys = new();
        SetData data = new SetData();

        public ActivityModel()
        {
        }

        public void OnGet()
        {
            Activitys = data.setActivty();
        }
    }
}
