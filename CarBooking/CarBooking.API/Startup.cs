using CarBooking.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace CarBooking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarBookingDataContext>(options => options.UseSqlServer(
                Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddControllers().AddNewtonsoftJson(settings =>
                settings.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //// Source: https://stackoverflow.com/questions/41090881/migrating-at-runtime-with-entity-framework-core
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // FIXME: Database connection can sometimes not be established.
                scope.ServiceProvider.GetService<CarBookingDataContext>().Database.Migrate();

                // Load and execute the import script
                var assembly = typeof(Startup).GetTypeInfo().Assembly;

                Stream stream = assembly.GetManifestResourceStream("CarBooking.API.Import.sql");
                StreamReader reader = new StreamReader(stream);
                string importScript = reader.ReadToEnd();

                scope.ServiceProvider.GetService<CarBookingDataContext>().Database.ExecuteSqlRaw(importScript);
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static bool CanConnect(CarBookingDataContext service)
        {
            try
            {
                return service.Database.CanConnect();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
