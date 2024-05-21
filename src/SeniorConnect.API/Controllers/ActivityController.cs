using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Entities;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("ActivityController")]
    public class ActivityController : Controller
    {
        [HttpGet]
        [Route("ActivityList")]
        public ActionResult GetActivityList()
        {
            try
            {
                Temp temp = new Temp();
                List<Activity> list = temp.setActivty();
                return Json(list);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("Activity/{ActivityId}")]
        public ActionResult GetActivity()
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

                return Ok("shows a spesific activity: " + ActivityId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("Activity")]
        public ActionResult PostActivity([FromBody] Activity activity)
        {
            try
            {
                long ActivityId = activity.ActivityId;
                return Ok("Add/Update an activity to the database: " + ActivityId);
            }
            catch (Exception)
            {

                throw;
            }
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
