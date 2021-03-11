using ChaseLabs.CLConfiguration.List;
using com.drewchaseproject.net.Flexx.Media.Libraries.Movies;
using static com.drewchaseproject.net.Flexx.Core.Data.Values;

namespace com.drewchaseproject.net.Flexx.Media.Libraries
{
    /// <summary>
    /// A Library is a collection of Media Objects and various functions to easily use them.
    /// </summary>
    public class LibraryModel
    {
        /// <summary>
        /// The Root path of the library
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The User Friendly name of the library
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The <seealso cref="LibraryType"/>
        /// </summary>
        public LibraryType Type { get; set; }
        /// <summary>
        /// If <seealso cref="Type"/> is a <seealso cref="LibraryType.Movies"/><br />
        /// This will be populated with all movies listed under the <seealso cref="Path"/>
        /// </summary>
        public MovieListModel Movies { get; private set; }
        /// <summary>
        /// Manifest file that contains all pertinent information 
        /// </summary>
        public ConfigManager Manifest => new ConfigManager(System.IO.Path.Combine(Path, $"{Name}.manifest"));

        /// <summary>
        /// Generates Media Objects based on the libraries path.<br />
        /// <i>Uses <seealso cref="MovieListModel.GenerateListFromLibrary(LibraryModel)"/></i>
        /// </summary>
        public void GenerateLibraryItems()
        {
            if (Type == LibraryType.Movies)
            {
                // Do Movie Shit
                Movies = MovieListModel.GenerateListFromLibrary(this);
            }
            else if (Type == LibraryType.TV)
            {
                // Do Tv Show Shit
            }
        }
    }
}
