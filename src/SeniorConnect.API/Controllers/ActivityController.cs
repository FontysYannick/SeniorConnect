using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorConnect.API.Models.Activity;
using SeniorConnect.API.Services.ActivityService.Interface;
using System.Threading.Tasks;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(IActivityService activityService, ILogger<ActivityController> logger)
        {
            _activityService = activityService;
            _logger = logger;
        }

        [HttpGet("ActivityList")]
        public IActionResult GetActivityList()
        {
            try
            {
                var activities = _activityService.GetActivities();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het ophalen van de activiteitenlijst.");
                return StatusCode(500, "Er is een fout opgetreden bij het ophalen van de activiteitenlijst.");
            }
        }

        [HttpGet("Activity/{ActivityId}")]
        public async Task<IActionResult> GetActivity(int ActivityId)
        {
            try
            {
                if (ActivityId <= 0)
                {
                    return BadRequest("Invalid ActivityId");
                }

                var activity = _activityService.GetSingleActivity(ActivityId);

                if (activity == null)
                {
                    return NotFound();
                }

                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het ophalen van de activiteit.");
                return StatusCode(500, "Er is een fout opgetreden bij het ophalen van de activiteit.");
            }
        }

        [HttpPost("PostActivity")]
        public async Task<IActionResult> PostActivity([FromBody] AbstractActivity activity)
        {
            try
            {
                if (activity == null)
                {
                    return BadRequest("Activity cannot be null");
                }

                _activityService.SetActivity(activity);
                return Ok("Activity added/updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het toevoegen/bijwerken van de activiteit.");
                return StatusCode(500, "Er is een fout opgetreden bij het toevoegen/bijwerken van de activiteit.");
            }
        }

        [HttpPost("AddUserToActivity")]
        public async Task<IActionResult> AddUserToActivity([FromBody] AbstractUserActivty userActivity)
        {
            try
            {
                if (userActivity == null)
                {
                    return BadRequest("User activity cannot be null");
                }

                _activityService.AddUserToActivity(userActivity);
                return Ok("User added to activity successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het toevoegen van de gebruiker aan de activiteit.");
                return StatusCode(500, "Er is een fout opgetreden bij het toevoegen van de gebruiker aan de activiteit.");
            }
        }

        [HttpGet("GetUserToActivity/{userId}")]
        public async Task<IActionResult> GetUserToActivity(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest("Invalid userId");
                }

                var activities = _activityService.GetUserToActivity(userId);
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het ophalen van de activiteiten van de gebruiker.");
                return StatusCode(500, "Er is een fout opgetreden bij het ophalen van de activiteiten van de gebruiker.");
            }
        }

        [HttpDelete("Activity/{ActivityId}")]
        public async Task<IActionResult> DeleteActivity(int ActivityId)
        {
            try
            {
                if (ActivityId <= 0)
                {
                    return BadRequest("Invalid ActivityId");
                }

                // Implement the deletion logic here
                // Assuming a method DeleteActivity exists in the service
                bool isDeleted = _activityService.DeleteActivity(ActivityId);

                if (!isDeleted)
                {
                    return NotFound("Activity not found");
                }

                return Ok("Activity deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Er is een fout opgetreden bij het verwijderen van de activiteit.");
                return StatusCode(500, "Er is een fout opgetreden bij het verwijderen van de activiteit.");
            }
        }
    }
}
