using com.drewchaseproject.net.Flexx.Core.Exceptions;
using System.Collections.Generic;

namespace com.drewchaseproject.net.Flexx.Media.Libraries
{
    /// <summary>
    /// A List of all <seealso cref="LibraryModel"/> currently loaded in FLEXX
    /// </summary>
    public class LibraryListModel : List<LibraryModel>
    {
        #region Singleton
        private static LibraryListModel _singleton;
        public static LibraryListModel Singleton
        {
            get
            {
                if (_singleton == null) _singleton = new LibraryListModel();
                return _singleton;
            }
        }
        #endregion

        /// <summary>
        /// Gets the Library from the Library Name.<br />
        /// Throws <see cref="LibraryNotFoundException"/> if no library is found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LibraryModel GetByName(string name)
        {
            foreach (LibraryModel model in this)
            {
                if (model.Name.ToLower().Equals(name.ToLower()))
                {
                    return model;
                }
            }
            throw new LibraryNotFoundException($"{name} was not found as a loaded library. Maybe it needs to be loaded or its missing.");
        }

        /// <summary>
        /// Creates a new Library
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public LibraryModel CreateLibrary(string Name, string Path)
        {
            LibraryModel model = new LibraryModel() { Name = Name, Path = Path, Type = Core.Data.Values.LibraryType.Movies };
            Add(model);
            model.GenerateLibraryItems();
            return model;
        }
    }
}
