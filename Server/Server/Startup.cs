﻿using AppSettingsConfigurationPlugin;
using DataProviderFacade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Models;
using Server.Repositories;
using Server.Repositories.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration;
        private Action<DbContextOptionsBuilder> DbContextOptionsBuilder;
        private Container container = new Container();

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = new MicrosoftConfiguration("appsettings.json").config;

            //DbContextOptionsBuilder = new MySqlDataProvider(Configuration.GetConnectionString("MySQLServer")).DbContextOptionsBuilder;

            ConfigureDbProviders(configuration, env);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IoTLogCollectorDataContext>(DbContextOptionsBuilder);

            services.AddMvc();

            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);

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

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            container.Register<IFirstRepository, FirstRepository<First>>(Lifestyle.Scoped);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            container.AutoCrossWireAspNetComponents(app);
        }


        #endregion

        #region Plugins

        void ConfigureDbProviders(IConfiguration configuration, IHostingEnvironment env)
        {

            var pluginsPath = Path.Combine(env.ContentRootPath, "Plugins");
            foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                var asm = Assembly.LoadFile(file);

                foreach (var type in asm.GetTypes())
                {
                    if (type.GetInterfaces().Contains(typeof(IDataProvider)))
                    {
                        Object[] args = { Configuration.GetConnectionString("MySQLServer") };

                        var dataProvider = Activator.CreateInstance(type, args) as IDataProvider;
                        DbContextOptionsBuilder = dataProvider.DbContextOptionsBuilder;
                    }
                }
            }
        }

        #endregion

    }
}
