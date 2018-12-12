using DataProviderCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Server.Helpers;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IDataStoragePlugin _dataStoragePlugin;

        public TestApiController(IConfiguration configuration, DataStoragesHelperType dataStorages, DataStoragesHelperType testType)
        {
            _configuration = configuration;
            _dataStoragePlugin = dataStorages.GetDataStoragePlugin() ?? throw new ArgumentNullException(nameof(dataStorages));
        }

        [HttpGet]
        public ActionResult<string> Get()
        { 
            return "Hello from first Api Controller";
        }

        [HttpPost]
        [Route("test")]
        public IActionResult Send()
        {
            return Ok("hello");
        }
    }
}