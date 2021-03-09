using ChaseLabs.CLConfiguration.List;
using System.IO;

namespace com.drewchaseproject.net.Flexx.Core.Data
{
    public class Configuration
    {
        public static int WebPort { get => manager.GetConfigByKey("web_port").ParseInt(); set => manager.GetConfigByKey("web_port").Value = value.ToString(); }
        public static int LibraryManifest { get => manager.GetConfigByKey("libraries").ParseInt(); set => manager.GetConfigByKey("libraries").Value = value.ToString(); }

        private static ConfigManager manager;

        public static void Init()
        {
            manager = new ConfigManager(Values.ApplicationSettingFile);
            manager.Add("web_port", "89715");
            manager.Add("libraries", Path.Combine(Values.ConfigDirectory, "libraries.manifest"));
        }
        private Configuration() { }
    }
}
