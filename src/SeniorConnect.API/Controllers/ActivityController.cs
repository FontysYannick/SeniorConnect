using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Data;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Services.ActivityService;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("ActivityController")]
    public class ActivityController : Controller
    {
        private readonly ActivityService _activityHelper;

        private readonly DataContext _dataContext;

        public ActivityController(ActivityService activityHelper, DataContext dataContext)
        {
            _activityHelper = activityHelper;
            this._dataContext = dataContext;
        }

        [HttpGet]
        [Route("ActivityList")]
        public ActionResult GetActivityList()
        {
            try
            {
                var activities = _dataContext.Activities.Where(a => a.Date > DateTime.Now).OrderBy(a => a.Date).ToList();

                return Json(activities);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("Activity/{ActivityId}")]
        public async Task<IActionResult> GetActivity()
        {
            try
            {
                int ActivityId = -1;
                string? rawId = Request.RouteValues["ActivityId"]?.ToString();
                if (string.IsNullOrEmpty(rawId))
                    ModelState.AddModelError("No ActivityId Found", "No ActivityId was send");
                if (!int.TryParse(rawId, out ActivityId))
                    ModelState.AddModelError("Invald ActivityId", "Invald ActivityId was send");
                if (ActivityId < 0)
                    ModelState.AddModelError("Invald ActivityId", "Invald ActivityId was send");

                var activity = await _dataContext.Activities.Include(a => a.Organizer).FirstOrDefaultAsync(a => a.ActivityId == ActivityId);

                return Ok(activity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("PostActivity")]
        public async Task<IActionResult> PostActivity([FromBody] AbstractActivity activity)
        {
            _activityHelper.setActivty(activity);

            return Ok("Add/Update an activity to the database: ");
        }


        [HttpPost("AddUserToActivity")]
        public async Task<IActionResult> AddUserToActivity([FromBody] AbstractUserActivty userActivity)
        {
            _activityHelper.addUserToActivty(userActivity);

            return Ok("Add/Update an activity to the database: ");
        }


        [HttpGet("GetUserToActivity/{userId}")]
        public async Task<IActionResult> GetUserToActivity()
        {
            int userId = -1;
            string? rawId = Request.RouteValues["userId"]?.ToString();
            if (string.IsNullOrEmpty(rawId))
                ModelState.AddModelError("No userId Found", "No userId was send");
            if (!int.TryParse(rawId, out userId))
                ModelState.AddModelError("Invald userId", "Invald userId was send");
            if (userId < 0)
                ModelState.AddModelError("Invald userId", "Invald userId was send");

            var Activitys = _dataContext.ActivityUsers.Where(a => a.UserId == userId && a.Activity.Date > DateTime.Now).Include(a => a.Activity).Select(a => a.Activity).OrderBy(a => a.Date).ToList();

            return Json(Activitys);
        }

        [HttpDelete]
        [Route("Activity/{ActivityId}")]
        public ActionResult DeleteActivity()
        {
            try
            {
                int ActivityId = -1;
                string? rawId = Request.RouteValues["ActivityId"]?.ToString();
                if (string.IsNullOrEmpty(rawId))
                    ModelState.AddModelError("No ActivityId Found", "No ActivityId was send");
                if (!int.TryParse(rawId, out ActivityId))
                    ModelState.AddModelError("Invald ActivityId", "Invald ActivityId was send");
                if (ActivityId < 0)
                    ModelState.AddModelError("Invald ActivityId", "Invald ActivityId was send");

                return Ok("Deletes a spesific activity: " + ActivityId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
