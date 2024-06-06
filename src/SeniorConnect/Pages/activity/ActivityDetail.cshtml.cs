using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.Models.Activities;

namespace SeniorConnect.Pages.activity
{
    public class ActivityDetailModel() : PageModel
    {

        public ActivityDto Activity = new();

        public void OnGet(int Id)
        {
           
        }

        public void OnPost()
        {
        }
    }
}
