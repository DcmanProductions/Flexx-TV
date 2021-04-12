namespace Flexx.Media.Libraries.Series
{
    public class EpisodeObjectModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Summery { get; set; }
        public string PosterURL { get; set; }
        public string Duration { get; set; }
        public string Resolution { get; set; }
        public bool Watched { get; set; }
        public int WatchedDuration { get; set; }
        public int WatchedPercentage { get; set; }
    }
}
