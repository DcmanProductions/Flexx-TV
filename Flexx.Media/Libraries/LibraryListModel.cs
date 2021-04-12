using System.Collections.Generic;

namespace Flexx.Media.Libraries
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
                if (_singleton == null)
                {
                    _singleton = new LibraryListModel();
                }

                return _singleton;
            }
        }
        #endregion

        public LibraryModel Movies { get; private set; }
        public LibraryModel TV { get; private set; }

        /// <summary>
        /// Creates a new Library
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public LibraryModel CreateLibrary(Core.Data.Values.LibraryType type, string Path)
        {
            LibraryModel model = new() { Path = Path, Type = type };
            Add(model);
            model.GenerateLibraryItems();
            if (type == Core.Data.Values.LibraryType.Movies)
            {
                Movies = model;
            }

            if (type == Core.Data.Values.LibraryType.TV)
            {
                TV = model;
            }

            return model;
        }
    }
}
