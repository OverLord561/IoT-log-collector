﻿using DataProviderCommon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Helpers;
using Server.Repository;
using Server.Services;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration;
        private Container container = new Container();

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSPAAccess",
                    builder => builder.WithOrigins("http://localhost:60365", "https://localhost:44344"));
            });

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            InitializeContainer(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            container.Verify();
            app.UseCors("AllowSPAAccess");

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            // to redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();
            //создается единственный в приложении маршрут, который позволит сопоставлять запросы с контроллерами и их методами.
            app.UseMvc();
        }

        private void OnShutdown()
        {

            Console.WriteLine("In Shutdown 1");

            Thread.Sleep(2000);

            Console.WriteLine("In Shutdown 2");
            Thread.Sleep(2000);


            Console.WriteLine("In Shutdown 3");
            Thread.Sleep(2000);


            Console.WriteLine("In Shutdown 4");
            Thread.Sleep(2000);

        }

        #region SimpleInjector
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
                new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(container));

            services.EnableSimpleInjectorCrossWiring(container);
            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        private void InitializeContainer(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            //container.Register<IFirstRepository, FirstRepository<First>>(Lifestyle.Scoped);
            container.Register<DataStoragesHelperType>();
            container.Register<DeviceHelperType>();
            container.Register<DBWriterHelper>();
            container.Register<IDevicesLogsRepository, DevicesLogsRepository>();
            container.Register<IDevicesLogsService, DevicesLogsService>();
            container.RegisterSingleton<CollectionOfLogs>();

            // Allow Simple Injector to resolve services from ASP.NET Core.

            ConfigureDbProviders(env);

            container.AutoCrossWireAspNetComponents(app);
        }

        #endregion

        #region Plugins

        void ConfigureDbProviders(IHostingEnvironment env)
        {
            string pluginDirectory = Path.Combine(env.ContentRootPath, "Plugins");

            var assemblies = new List<Assembly>();

            foreach (var file in new DirectoryInfo(pluginDirectory).GetFiles())
            {
                if (file.Extension.ToLower() == ".dll")
                {
                    var assm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);

                    assemblies.Add(assm);
                }
            }

            container.Collection.Register<IDataStoragePlugin>(assemblies);
            container.Collection.Register<IDevicePlugin>(assemblies);


            //var t = container.GetInstance<ITestType>(); //throw SimpleInjector.ActivationException
            //IServiceProvider provider = container;
            //object instance = provider.GetService(typeof(ITestType)); // return null
        }

        #endregion

    }
}
