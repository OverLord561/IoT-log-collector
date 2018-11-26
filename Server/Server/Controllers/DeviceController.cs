using DataProviderFacade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IEnumerable<IDataStoragePlugin> _dataStoragePlugin;

        public DeviceController(IConfiguration configuration, IEnumerable<IDataStoragePlugin> dataStoragePlugin)
        {
            _dataStoragePlugin = dataStoragePlugin;
        }


        [HttpPost]
        [Route("write-log")]
        public async Task<IActionResult> WriteLog([FromBody] StandardizedDevice standardizedDevice)
        {

            //StandardizedDevice device = new Samsung_RT38F(25).ConverterToStandard();

            //_dataStoragePlugin.First().Operations.Add(device);

            var list = new List<StandardizedDevice>();

            for (var i = 0; i < 10000; i++)
            {
                list.Add(new StandardizedDevice()
                {
                    Id = Guid.NewGuid(),
                    DateStamp = DateTime.Now
                });
            }

            await _dataStoragePlugin.First().Operations.AddRangeAsync(list);

            //_dataStoragePlugin.First().Operations.Add(standardizedDevice);

            return Ok("hello");
        }
    }
}