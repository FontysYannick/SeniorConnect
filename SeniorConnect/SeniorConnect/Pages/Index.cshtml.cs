using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SeniorConnect.Models;

namespace SeniorConnect.Pages
{
    public class IndexModel : PageModel
    {
        public Activity Activity { get; set; }

        public IndexModel()
        {

        }

        public void OnGet()
        {
            Activity = new Activity { ActivityId = new Guid(), ActivityName = "Test", ActivityDescription = "grote test" };
        }
    }
}
