using Flexx.Media.Libraries.Movies.Extras;

namespace Flexx.Media.Libraries.Movies
{
    public class MovieObjectModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Summery { get; set; }
        public short Year { get; set; }
        public string PosterURL { get; set; }
        public string CoverURL { get; set; }
        public string MPAA { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string Resolution { get; set; }
        public bool Watched { get; set; }
        public int WatchedDuration { get; set; }
        public int WatchedPercentage { get; set; }
        public string[] Writers { get; set; }
        public string[] Genres { get; set; }
        public ActorModel[] Actors { get; set; }
    }
}
