using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Services.ActivityService;
using SeniorConnect.API.Services.ActivityService.Interface;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("ActivityController")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        [HttpGet]
        [Route("ActivityList")]
        public ActionResult GetActivityList()
        {
            try
            {
                var activities = _activityService.GetActivities();
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
                    ModelState.AddModelError("No ActivityId Found", "No ActivityId was sent");
                if (!int.TryParse(rawId, out ActivityId))
                    ModelState.AddModelError("Invalid ActivityId", "Invalid ActivityId was sent");
                if (ActivityId < 0)
                    ModelState.AddModelError("Invalid ActivityId", "Invalid ActivityId was sent");

                var activity = _activityService.GetSingleActivity(ActivityId);

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
            _activityService.SetActivity(activity);
            return Ok("Add/Update an activity to the database");
        }

        [HttpPost("AddUserToActivity")]
        public async Task<IActionResult> AddUserToActivity([FromBody] AbstractUserActivty userActivity)
        {
            _activityService.AddUserToActivity(userActivity);

            return Ok("Add/Update an activity to the database");
        }

        [HttpGet("GetUserToActivity/{userId}")]
        public async Task<IActionResult> GetUserToActivity()
        {
            int userId = -1;
            string? rawId = Request.RouteValues["userId"]?.ToString();
            if (string.IsNullOrEmpty(rawId))
                ModelState.AddModelError("No userId Found", "No userId was sent");
            if (!int.TryParse(rawId, out userId))
                ModelState.AddModelError("Invalid userId", "Invalid userId was sent");
            if (userId < 0)
                ModelState.AddModelError("Invalid userId", "Invalid userId was sent");

            var activities = _activityService.GetUserToActivity(userId);

            return Json(activities);
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
                    ModelState.AddModelError("No ActivityId Found", "No ActivityId was sent");
                if (!int.TryParse(rawId, out ActivityId))
                    ModelState.AddModelError("Invalid ActivityId", "Invalid ActivityId was sent");
                if (ActivityId < 0)
                    ModelState.AddModelError("Invalid ActivityId", "Invalid ActivityId was sent");

                // Implement the deletion logic here

                return Ok("Deletes a specific activity: " + ActivityId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
