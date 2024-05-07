using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Models;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("ActivityController")]
    public class ActivityController : Controller
    {
        [HttpGet]
        [Route("ActivityList")]
        public ActionResult GetUserList()
        {
            try
            {
                return Ok("works");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("Activity/{ActivityId}")]
        public ActionResult GetUser()
        {
            try
            {
                int ActivityId = -1;
                string? rawId = Request.RouteValues["ActivityID"]?.ToString();
                if (string.IsNullOrEmpty(rawId))
                    ModelState.AddModelError("No UserId Found", "No userId was send");
                if (!int.TryParse(rawId, out ActivityId))
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");
                if (ActivityId < 0)
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");

                return Ok("works" + ActivityId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("Activity")]
        public ActionResult PostUser([FromBody] Activity activity)
        {
            try
            {
                int UserId = -1;
                string? rawId = Request.RouteValues["activity"]?.ToString();
                return Ok("works");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("Activity")]
        public ActionResult UpdateUser([FromBody] Activity activity)
        {
            try
            {
                int activityId = -1;
                string? rawId = Request.RouteValues["activity"]?.ToString();
                return Ok("works");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
