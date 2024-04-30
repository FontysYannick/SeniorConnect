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
                return Ok("works");
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

                return Ok("works" + UserId);
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
                int UserId = -1;
                string? rawId = Request.RouteValues["UserId"]?.ToString();
                return Ok("works");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
