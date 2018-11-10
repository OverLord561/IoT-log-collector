using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello from first Api Controller";
        }

        [HttpPost]
        [Route("test")]
        public IActionResult Send([FromBody] RestCall model)
        {
            return Ok("hello");
        }
    }
}