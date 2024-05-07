using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeniorConnect.API.Models;

namespace SeniorConnect.API.Controllers
{
    [ApiController]
    [Route("UserController")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("UserList")]
        public ActionResult GetUserList()
        {
			try
			{
                return Ok("Returns a list of all users");
			}
			catch (Exception)
			{

				throw;
			}
        }

        [HttpGet]
        [Route("User/{UserId}")]
        public ActionResult GetUser()
        {
            try
            {
                int UserId = -1;
                string? rawId = Request.RouteValues["UserId"]?.ToString();
                if (string.IsNullOrEmpty(rawId))
                    ModelState.AddModelError("No UserId Found", "No userId was send");
                if (!int.TryParse(rawId, out UserId))
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");
                if (UserId < 0)
                    ModelState.AddModelError("Invald UserId", "Invald userId was send");

                return Ok("shows a spesific activity: " + UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("User")]
        public ActionResult PostUser([FromBody]User user)
        {
            try
            {
                int UserId = user.UserId;
                return Ok("Add/Update an activity to the database: " + UserId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
