using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TestApiController(IConfiguration configuration) {
            _configuration = configuration;

            var test = _configuration.GetValue<string>("test");
        }

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