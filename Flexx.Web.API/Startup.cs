using Flexx.Core.Data;
using LettuceEncrypt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flexx.Web.API
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            // Specifies allowed sites via the COR Policy - Cross Origin Policy.
            services.AddCors(options => options.AddPolicy(name: MyAllowSpecificOrigins, builder => builder.WithOrigins("https://flexx-tv.tk", "http://flexx-tv.tk", "http://127.0.0.1:5500", "http://127.0.0.1:3223").AllowAnyHeader().AllowAnyMethod()));
            // Adding dynamic HTTPS encryption using LettuceEncrypt
            services.AddLettuceEncrypt().PersistDataToDirectory(new System.IO.DirectoryInfo(System.IO.Path.Combine(Values.LibDirectory, ".cert")), "flexx-tv"); ;
            // Adds the ability to use the MVC structure.
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Runs if the application detects its being run in a dev environment
                app.UseDeveloperExceptionPage();
            }
            // Allows the usage of route pre-processor above methods and classes
            app.UseRouting();

            // Adds specified COR Policy
            app.UseCors(MyAllowSpecificOrigins);

            // Specifies endpoint template.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
