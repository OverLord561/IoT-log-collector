using DataProviderCommon;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class DeviceHelperType
    {
        private readonly IConfiguration _configuration;
        private readonly IEnumerable<IDevicePlugin> _devicePluginsCollection;


        public DeviceHelperType(IConfiguration configuration, IEnumerable<IDevicePlugin> devicePluginsCollection)
        {
            _configuration = configuration;
            _devicePluginsCollection = devicePluginsCollection;
        }

        public IDevicePlugin GetDevicePlugin()
        {
            return _devicePluginsCollection.FirstOrDefault();
        }
    }
}
