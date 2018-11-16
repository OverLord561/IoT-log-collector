using DataProviderFacade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

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

            //StandardizedDevice device = new Samsung_RT38F(25).ConverterToStandard();

            //_dataStoragePlugin.First().Operations.Add(device);
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var setOfData = _dataStoragePlugin.First().Operations.All();

            BinaryFormatter bf = new BinaryFormatter();

            foreach (var device in setOfData) {
                using (MemoryStream ms = new MemoryStream(device.Message))
                {
                    var obj = bf.Deserialize(ms) as IStandardizedDeviceOperations;

                    var res = obj.PrepareDataForUI();

                }
            }

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