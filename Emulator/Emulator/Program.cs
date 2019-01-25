using Emulator.Config;
using Emulator.Config.Interfaces;
using Emulator.Services;
using RestSharp;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Emulator
{
    class Program
    {
        static readonly Container container;
        static int counter = 0;

        static IEmulatorConfiguration cnf = new EmulatorConfiguration();

        static Program()
        {
            //container = new Container();

            //container.RegisterSingleton<IEmulatorConfiguration, EmulatorConfiguration>(); //container.Register<IConfiguration, Configuration>(Lifestyle.Singleton);
            // container.RegisterSingleton<IHttpClient, RestSharpHttpClient>();

            //container.Verify(); // iterates registered service to check if something is not correct, will throw an exception before any execution of the progam
        }

        static void Main(string[] args)
        {
            var alldata = new List<double>();

            var allTasks = Enumerable.Range(1, 10).Select(x =>
           {
               return Task.Run(() =>
               {

                   HttpClient httpClient = new HttpClient();
                   httpClient.BaseAddress = new Uri("https://localhost:44373");
                   httpClient.DefaultRequestHeaders
                         .Accept
                         .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                   StringContent httpContent = new StringContent("{\"PluginName\":\"SamsungDPlugin\",\"DeviceData\":{\"Temperature\":30.0,\"Humidity\":40.0}}", Encoding.UTF8, "application/json");

                   var ll = Enumerable.Range(1, 1500).Select(y =>
                   {
                       var sw = Stopwatch.StartNew();

                       var res = httpClient.PostAsync("api/log-collector/write-log", httpContent).Result;

                       sw.Stop();

                       return sw.Elapsed.TotalMilliseconds;
                   }).ToArray();

                   alldata.AddRange(ll);

                   httpContent.Dispose();
                   httpClient.Dispose();

               });
           }).ToArray();


            Task.WaitAll(allTasks);

            Debugger.Break();

            Console.ReadLine();
        }

    }
}
