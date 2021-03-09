using System;
using System.Collections.Generic;
using System.Text;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies
{
    public class MovieListModel : List<MovieModel>
    {
        public MovieModel GetByName(string name)
        {
            MovieModel model = null;
            ForEach(e => { if (e.Title.ToLower().Equals(name.ToLower())) model = e; });
            if (model == null) throw new NullReferenceException($"No Movie Named {name} exists. Make sure that its been loaded!");
            return model;
        }

        public void init()
        {
        }
    }
}
