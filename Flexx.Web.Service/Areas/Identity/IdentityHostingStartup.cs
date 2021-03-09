using com.drewchaseproject.net.Flexx.Web.Service.Areas.Identity.Data;
using com.drewchaseproject.net.Flexx.Web.Service.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(com.drewchaseproject.net.Flexx.Web.Service.Areas.Identity.IdentityHostingStartup))]
namespace com.drewchaseproject.net.Flexx.Web.Service.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ContextConnection")));

                services.AddDefaultIdentity<FlexxUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;

                })
                    .AddEntityFrameworkStores<Context>();
            });
        }
    }
}