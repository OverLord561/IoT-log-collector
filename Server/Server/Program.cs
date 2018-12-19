using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //var host = new HostBuilder()
            //    .Build();

            //using (host)
            //{
            //    host.Start();

            //    await host.StopAsync(TimeSpan.FromSeconds(5));
            //}

            //var host = new HostBuilder()
            //    .Build();

            //await host.RunAsync().Wait();

            var webHost = CreateWebHostBuilder(args)
                //.ConfigureServices((hostContext, services) =>
                //{
                //    services.Configure<HostOptions>(option =>
                //    {
                //        option.ShutdownTimeout = System.TimeSpan.FromSeconds(7);
                //    });
                //})
                .Build();

            //using (webHost)
            //{
            //    webHost.Run();
            //   // webHost.WaitForShutdown();
            //}

            webHost.Run();

            //webHost.Run();


            //await webHost.StopAsync(TimeSpan.FromSeconds(20));

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
