using Flexx.Core.Data;
using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace Flexx.Web.API
{
    public class Program
    {
        /// <summary>
        /// Application starting point 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Initializes the Configuration Class.
            Configuration.Init();
            // Instaciates the singleton object of the transcoding library.
            Transcoding.Instaciate();

            LoadLibraries(); // TODO: Remove line in production build.

            Start(args);
        }

        /// <summary>
        /// Loads the Libraries.
        /// <para><b><i>This is only Temporary</i></b></para>
        /// </summary>
        [System.Obsolete("This is only Temporary", error: false)]
        private static void LoadLibraries()
        {
            LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.TV, @"C:\Users\drew_\AppData\Roaming\Chase Labs\Flexx\media\tv");
            //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.TV, @"Z:\Tv Shows");

            //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.Movies, @"C:\Users\drew_\AppData\Roaming\Chase Labs\Flexx\media\movies");
            //LibraryListModel.Singleton.CreateLibrary(Values.LibraryType.Movies, @"Z:\Movies");
        }
        /// <summary>
        /// Starts the server
        /// </summary>
        /// <param name="args"></param>
        private static void Start(string[] args)
        {
            try
            {
                // Attempts to build a Kestral Server
                CreateHostBuilder(args).Build().Run();
            }
            catch
            {
                // If unable to build Kestral Server
                // Run default IIS server.
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
                // Gets the port from config file.
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
