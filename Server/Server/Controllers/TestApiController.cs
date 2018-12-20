using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Server is running!";
        }
    }
}