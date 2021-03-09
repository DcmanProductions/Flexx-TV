using System;
using System.IO;

namespace com.drewchaseproject.net.Flexx.Core.Data
{
    public class Values
    {
        #region Singleton
        private static Values _singleton;
        public static Values Singleton { get { _singleton = _singleton == null ? new Values() : _singleton; return _singleton; } }
        #endregion
        #region Static Values
        public static string ApplicationName => "Flexx";
        public static string CompanyName => "Chase Labs";
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
        public static string TheMovieDBAPIKey => "378ae44c6e7f5dde094cd8c8456378e0";

        public static string FORMATTED_TIME => DateTime.Now.ToString().Replace(":", "-").Replace("\\", "").Replace("/", "-").Replace("?", "");

        public static string GetTrailerURL(string ID)
        {
            return $"https://api.themoviedb.org/3/movie/{ID}/videos?api_key={TheMovieDBAPIKey}";
        }
    }
}
