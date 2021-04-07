using ChaseLabs.CLConfiguration.List;
using System.IO;

namespace Flexx.Core.Data
{
    /// <summary>
    /// Persistent Data that will be saved to various config files based on category.
    /// </summary>
    public abstract class Configuration
    {
        /// <summary>
        /// The Port that the Kestral Server will use.
        /// </summary>
        public static int WebPort { get => manager.GetConfigByKey("web_port").ParseInt(); set => manager.GetConfigByKey("web_port").Value = value.ToString(); }
        /// <summary>
        /// Saved list of all Libraries.
        /// </summary>
        public static int LibraryManifest { get => manager.GetConfigByKey("libraries").ParseInt(); set => manager.GetConfigByKey("libraries").Value = value.ToString(); }

        private static ConfigManager manager;

        /// <summary>
        /// Initializes the Default config values or loads existing values.
        /// </summary>
        public static void Init()
        {
            manager = new ConfigManager(Values.ApplicationSettingFile);
            manager.Add("web_port", "2112");
            manager.Add("libraries", Path.Combine(Values.ConfigDirectory, "libraries.manifest"));
        }
    }
}
