using Emulator.Config;
using Emulator.Config.Interfaces;
using Emulator.Models;
using Emulator.Services;
using Emulator.Services.Interfaces;
using SimpleInjector;
using System;
using System.Diagnostics;

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

            for (var i = 0; i < 1000; i++) {
                var res = container.GetInstance<IHttpClient>()
                        .Get<string>("api/log-collector/")
                        .Result;
            }

            stopwatch.Start();

             Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);

            //var res2 = container.GetInstance<IHttpClient>()
            //            .Post<RestCall, string>("api/log-collector/test", new RestCall { Name = 123 })
            //            .Result;

            Console.ReadLine();
        }
    }
}
