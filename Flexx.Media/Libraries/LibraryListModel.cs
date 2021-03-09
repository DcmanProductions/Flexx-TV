using com.drewchaseproject.net.Flexx.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace com.drewchaseproject.net.Flexx.Media.Libraries
{
    public class LibraryListModel : List<LibraryModel>
    {
        /// <summary>
        /// Gets the Library from the Library Name.<br />
        /// Throws <see cref="LibraryNotFoundException"/> if no library is found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LibraryModel GetByName(string name)
        {
            foreach (var model in this)
            {
                if (model.Name.ToLower().Equals(name.ToLower())) return model;
            }
            throw new LibraryNotFoundException($"{name} was not found as a loaded library. Maybe it needs to be loaded or its missing.");
        }

        public LibraryModel CreateLibrary(string Name, string Path)
        {
            LibraryModel model = new LibraryModel() { Name = Name, Path = Path };
            Add(model);
            return model;
        }
    }
}
