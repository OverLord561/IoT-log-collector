using DataProviderFacade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Server.Extensions;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/log-collector")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDataStoragePlugin _dataStoragePlugin;

        public DeviceController(IConfiguration configuration, IEnumerable<IDataStoragePlugin> dataStoragePluginsCollection)
        {
            _dataStoragePlugin = dataStoragePluginsCollection.GetDataStorageProvider();
        }

        [HttpPost]
        [Route("write-log")]
        public async Task<IActionResult> WriteLog([FromBody] StandardizedDevice standardizedDevice)
        {
            //var list = new List<StandardizedDevice>();

            //for (var i = 0; i < 10000; i++)
            //{
            //    list.Add(new StandardizedDevice()
            //    {
            //        Id = Guid.NewGuid(),
            //        DateStamp = DateTime.Now
            //    });
            //}

            //await _dataStoragePlugin.Operations.AddRangeAsync(list);

            _dataStoragePlugin.Operations.Add(standardizedDevice);

            return Ok("success");
        }
    }
}