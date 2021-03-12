using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies
{
    /// <summary>
    /// A List of Movies.<br />
    /// <i>Usually from a library, but doesn't have to be.</i>
    /// </summary>
    public class MovieListModel : List<MovieModel>
    {
        /// <summary>
        /// Gets the <seealso cref="MovieModel"/> Based on the Movie Name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MovieModel GetByName(string name)
        {
            MovieModel model = null;
            ForEach(e => { if (e.Title.ToLower().Equals(name.ToLower())) { model = e; } });
            if (model == null)
            {
                throw new NullReferenceException($"No Movie Named {name} exists. Make sure that its been loaded!");
            }

            return model;
        }

        /// <summary>
        /// Creates a <seealso cref="MovieListModel"/> based on <seealso cref="LibraryModel"/> object.
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        public static MovieListModel GenerateListFromLibrary(LibraryModel library)
        {
            string[] MediaExtensions = new string[] { "mpegg", "mpeg", "mp4", "mkv", "m4a", "m4v", "f4v", "f4a", "m4b", "m4r", "f4b", "mov", "3gp", "3gp2", "3g2", "3gpp", "3gpp2", "ogg", "oga", "ogv", "ogx", "wmv", "wma", "flv", "avi" };
            MovieListModel model = new MovieListModel();
            Directory.GetFiles(library.Path, "*", SearchOption.AllDirectories).Where(item => MediaExtensions.Any(item.ToLower().EndsWith)).ToList().ForEach(item =>
             {
                 MovieModel value = new MovieModel() { Path = item, Library = library };
                 value.GenerateDetails();
                 model.Add(value);
             });
            return model;
        }
    }
}
