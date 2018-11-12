using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProviderFacade;
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

        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugin;

        public TestApiController(IConfiguration configuration, IEnumerable<IDataStoragePlugin> dataStoragePlugin)
        {
            _configuration = configuration;
            _dataStoragePlugin = dataStoragePlugin;

            var test = _dataStoragePlugin.First().Operations.All();
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