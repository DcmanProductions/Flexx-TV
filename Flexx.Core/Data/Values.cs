using System;
using System.IO;

namespace com.drewchaseproject.net.Flexx.Core.Data
{
    /// <summary>
    /// A Collection of variables used by all facets of the application.
    /// </summary>
    public class Values
    {
        #region Singleton
        private static Values _singleton;
        public static Values Singleton { get { _singleton = _singleton == null ? new Values() : _singleton; return _singleton; } }
        #endregion
        #region Static Values
        /// <summary>
        /// Rather a Library is for Movies or Tv Shows
        /// </summary>
        public enum LibraryType
        {
            Movies,
            TV
        }
        /// <summary>
        /// The Application Name will be used around the application.
        /// </summary>
        public static string ApplicationName => "Flexx";
        /// <summary>
        /// The Company to whom created this.
        /// </summary>
        public static string CompanyName => "Chase Labs";
        /// <summary>
        /// The Movie Database API Key
        /// </summary>
        public static string TheMovieDBAPIKey => "378ae44c6e7f5dde094cd8c8456378e0";
        /// <summary>
        /// Converts the Current Time to a File Safe String.
        /// </summary>
        public static string FORMATTED_TIME => DateTime.Now.ToString().Replace(":", "-").Replace("\\", "").Replace("/", "-").Replace("?", "");
        #endregion
        #region Paths
        #region Directories
        public static string RootDirectory
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CompanyName, ApplicationName);
                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public static string ConfigDirectory
        {
            get
            {
                string path = Path.Combine(RootDirectory, "Configuration");
                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public static string LogDirectory
        {
            get
            {
                string path = Path.Combine(RootDirectory, "Logs");
                if (Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        #endregion
        #region Files
        public static string ApplicationSettingFile => Path.Combine(ConfigDirectory, "app.conf");
        #endregion
        #endregion
        #region Functions
        /// <summary>
        /// Gets the Youtube Trailer URL based on The Movie Database ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetTrailerURL(string ID)
        {
            return $"https://api.themoviedb.org/3/movie/{ID}/videos?api_key={TheMovieDBAPIKey}";
        }
        #endregion

    }
}
