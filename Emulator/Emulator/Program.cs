using DataProviderFacade;
using Emulator.Config;
using Emulator.Config.Interfaces;
using Emulator.Models;
using Emulator.Services;
using Emulator.Services.Interfaces;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace Emulator
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            container = new Container();

            container.RegisterSingleton<IEmulatorConfiguration, EmulatorConfiguration>(); //container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);
            container.RegisterSingleton<IHttpClient, RestSharpHttpClient>();

            container.Verify(); // iterates registered service to check if something is not correct, will throw an exception before any execution of the progam
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //for (var i = 0; i < 1000; i++)
            //{
            //    var res = container.GetInstance<IHttpClient>()
            //                .Post<StandardizedDevice, string>("api/log-collector/write-log",
            //                new StandardizedDevice
            //                {
            //                    Id = Guid.NewGuid(),
            //                    DateStamp = DateTime.Now
            //                });
            //}

            var uploadTasks = new List<Task>();
            for (var i = 0; i < 1; i++)
            {
                var res = container.GetInstance<IHttpClient>()
                            .Post<string, string>("api/log-collector/write-log",
                            "{\"PluginType\":\"SamsungTemperatureControllerPlugin\",\"deviceCharacteristics\":{\"Temperature\":10.0,\"Humidity\":10.0}}");

                uploadTasks.Add(res);
            }
            stopwatch.Start();

            Task.WhenAll(uploadTasks).Wait();

            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}
