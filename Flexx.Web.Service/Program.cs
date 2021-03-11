using com.drewchaseproject.net.Flexx.Core.Data;
using com.drewchaseproject.net.Flexx.Media.Libraries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace com.drewchaseproject.net.Flexx.Web.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configuration.Init();
            LibraryListModel.Singleton.CreateLibrary("movies", Path.Combine(Values.RootDirectory, "media"));
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch
            {
                CreateAltHostBuilder(args).Build().Run();
            }
        }

        /// <summary>
        /// Alternative Host Builder, Usually means executed in IDE or via IIS
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateAltHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());
                webBuilder.UseIISIntegration();
                webBuilder.UseStartup<Startup>();
            });
        }

        /// <summary>
        /// Default Host Builder using Kestral
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().UseKestrel(options =>
                {
                    int port = Configuration.WebPort;
                    options.ListenAnyIP(port);
                });
            });
        }
    }
}
