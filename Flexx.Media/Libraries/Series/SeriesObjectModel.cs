using Flexx.Media.Libraries.Movies.Extras;

namespace Flexx.Media.Libraries.Series
{
    public class SeriesObjectModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Summery { get; set; }
        public short Year { get; set; }
        public string PosterURL { get; set; }
        public string CoverURL { get; set; }
        public string Language { get; set; }
        public bool Watched { get; set; }
        public string[] Writers { get; set; }
        public string[] Genres { get; set; }
        public ActorModel[] Actors { get; set; }
    }
}
