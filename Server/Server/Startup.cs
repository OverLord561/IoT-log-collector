using DataProviderFacade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Models;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

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
            services.AddMvc();

            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            container.Verify();

            // to redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();
            //создается единственный в приложении маршрут, который позволит сопоставлять запросы с контроллерами и их методами.
            app.UseMvc();
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

            // Allow Simple Injector to resolve services from ASP.NET Core.

            ConfigureDbProviders(env);

            container.AutoCrossWireAspNetComponents(app);
        }


        #endregion

        #region Plugins

        void ConfigureDbProviders(IHostingEnvironment env)
        {
            var userSettings = new UserSettings();
            Configuration.Bind("userSettings", userSettings);

            string pluginDirectory = Path.Combine(env.ContentRootPath, "Plugins");


            var dataProvidersAssms = new List<Assembly>();

            foreach (var file in new DirectoryInfo(pluginDirectory).GetFiles())
            {
                if (file.Extension.ToLower() == ".dll")
                {
                    var assm = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);

                    if (assm.GetName().Name == userSettings.DataProviderPluginName)
                    {
                        dataProvidersAssms.Add(assm);
                    }
                }
            }

            container.Collection.Register<IDataStoragePlugin>(dataProvidersAssms);
        }

        #endregion

    }
}
