namespace Flexx.Media.Libraries.Series
{
    public class SeasonObjectModel
    {
        public int Number { get; set; }
        public string Name => $"Season {(Number > 9 ? Number.ToString() : $"0{Number}")}";
        public string PosterURL { get; set; }
        public bool Watched { get; set; }
    }
}
