using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Data
{
    public class Genre
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public Genre(int _id, string _name)
        {
            ID = _id;
            Name = _name;
        }
    }
}
