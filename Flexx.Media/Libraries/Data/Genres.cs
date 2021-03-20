using System.Collections.Generic;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class Genres
    {

        public static Genre GetByID(int id)
        {
            Genre temp = null;
            list.ForEach(item =>
            {
                if (item.ID.Equals(id))
                {
                    temp = item;
                }
            });

            return temp ?? new Genre(0, "UNKOWN");
        }

        public static List<Genre> GetByID(params int[] id)
        {
            List<Genre> temp = new List<Genre>();

            list.ForEach(item =>
            {
                if (item.ID.Equals(id))
                {
                    temp.Add(item);
                }
            });
            return temp;
        }

        private static List<Genre> list
        {
            get
            {
                List<Genre> tmp = new List<Genre>
                {
                    new Genre(28, "Action"),
                    new Genre(12, "Adventure"),
                    new Genre(16, "Animation"),
                    new Genre(35, "Comedy"),
                    new Genre(80, "Crime"),
                    new Genre(99, "Documentary"),
                    new Genre(18, "Drama"),
                    new Genre(10751, "Family"),
                    new Genre(14, "Fantasy"),
                    new Genre(36, "History"),
                    new Genre(27, "Horror"),
                    new Genre(10402, "Music"),
                    new Genre(9648, "Mystery"),
                    new Genre(10749, "Romance"),
                    new Genre(878, "Science Fiction"),
                    new Genre(10770, "TV Movie"),
                    new Genre(53, "Thriller"),
                    new Genre(10752, "War"),
                    new Genre(37, "Western")
                };
                return tmp;
            }
        }
    }
}
