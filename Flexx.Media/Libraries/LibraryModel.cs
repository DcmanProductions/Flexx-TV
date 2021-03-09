using ChaseLabs.CLConfiguration.List;
using System;
using System.Collections.Generic;
using System.Text;

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
