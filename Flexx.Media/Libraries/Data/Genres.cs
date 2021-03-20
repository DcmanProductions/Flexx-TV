using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class Genres
    {

        public static Genre GetByID(int id)
        {
            Genre temp = null;
            list.ForEach(item =>
            {
                if (item.ID.Equals(id)) temp = item;
            });
            
            return temp??new Genre(0, "UNKOWN");
        }

        public static List<Genre> GetByID(params int[] id)
        {
            List<Genre> temp = new List<Genre>();

            list.ForEach(item =>
            {
                if (item.ID.Equals(id)) temp.Add(item);
            });
            return temp;
        }

        private static List<Genre> list
        {
            get
            {
                List<Genre> tmp = new List<Genre>();
                tmp.Add(new Genre(28, "Action"));
                tmp.Add(new Genre(12, "Adventure"));
                tmp.Add(new Genre(16, "Animation"));
                tmp.Add(new Genre(35, "Comedy"));
                tmp.Add(new Genre(80, "Crime"));
                tmp.Add(new Genre(99, "Documentary"));
                tmp.Add(new Genre(18, "Drama"));
                tmp.Add(new Genre(10751, "Family"));
                tmp.Add(new Genre(14, "Fantasy"));
                tmp.Add(new Genre(36, "History"));
                tmp.Add(new Genre(27, "Horror"));
                tmp.Add(new Genre(10402, "Music"));
                tmp.Add(new Genre(9648, "Mystery"));
                tmp.Add(new Genre(10749, "Romance"));
                tmp.Add(new Genre(878, "Science Fiction"));
                tmp.Add(new Genre(10770, "TV Movie"));
                tmp.Add(new Genre(53, "Thriller"));
                tmp.Add(new Genre(10752, "War"));
                tmp.Add(new Genre(37, "Western"));
                return tmp;
            }
        }
    }
}
