using System;
using com.drewchaseproject.net.Flexx.Web.Service.Areas.Identity.Data;
using com.drewchaseproject.net.Flexx.Web.Service.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
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
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ContextConnection")));

                services.AddDefaultIdentity<FlexxUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<Context>();
            });
        }
    }
}