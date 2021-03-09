using ChaseLabs.CLConfiguration.List;

namespace com.drewchaseproject.net.Flexx.Media.Libraries
{
    public class LibraryModel
    {
        public enum LibraryType
        {
            Movies,
            TV
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public ConfigManager Manifest => new ConfigManager(System.IO.Path.Combine(Path, $"{Name}.manifest"));
    }
}
