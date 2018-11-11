using AppSettingsConfigurationPlugin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySQLDataProviderPlugin;
using Server.Models;
using System;

namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration;
        private readonly Action<DbContextOptionsBuilder> DbContextOptionsBuilder;

        public Startup(IConfiguration configuration)
        {
            Configuration = new MicrosoftConfiguration("appsettings.json").config;

            DbContextOptionsBuilder = new MySqlDataProvider(Configuration.GetConnectionString("MySQLServer")).DbContextOptionsBuilder;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
           
            services.AddDbContext<IoTLogCollectorDataContext>(this.DbContextOptionsBuilder);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // to redirect HTTP requests to HTTPS
            app.UseHttpsRedirection();
            //создается единственный в приложении маршрут, который позволит сопоставлять запросы с контроллерами и их методами.
            app.UseMvc();

            // app.UseMvcWithDefaultRoute(); // TO REPLACE app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}{id?}"); });
        }
    }
}
