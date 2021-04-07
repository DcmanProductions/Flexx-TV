using Flexx.Core.Data;
using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Timers;
using LettuceEncrypt;

using Microsoft.AspNetCore.Server.Kestrel.Https;


namespace Flexx.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configuration.Init();
            _ = Transcoding.Singleton;
            var timer = new Timer(1 * 1000);
            timer.Elapsed += (s, e) =>
            {
                LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.Movies, @"C:\Users\drew_\AppData\Roaming\Chase Labs\Flexx\media\movies");
                //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.TV, @"C:\Users\drew_\AppData\Roaming\Chase Labs\Flexx\media\tv");
                //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.Movies, @"Z:\Movies");
                //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.TV, @"Z:\Tv Shows");
                timer.Close();
            };
            timer.Start();
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
        /// Default Host Builder using Kestral
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                int port = Configuration.WebPort;
                webBuilder.UseStartup<Startup>().UseKestrel(kestral =>
                {
                    kestral.ListenAnyIP(port);
                });
            });
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
    }
}
