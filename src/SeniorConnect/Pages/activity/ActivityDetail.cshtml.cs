using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Helpers;
using SeniorConnect.Models;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel : PageModel
    {
        public List<Activity> Activitys = new();
        public Activity Activity = new();
        SetData data = new SetData();

        public ActivityDetailModel()
        {

        }

        public void OnGet(int Id)
        {
            Activitys = data.setActivty();

            Activity = Activitys.Where(a => a.ActivityId == Id).FirstOrDefault();
        }
    }
}
