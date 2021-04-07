using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TorrentTitleParser;
using System.Collections.Generic;
using Flexx.Core.Data;
using Newtonsoft.Json.Linq;

namespace Flexx.Media.Libraries.Series
{
    public class Season
    {
        public short Number { get; private set; }
        public List<Episode> Episodes { get; private set; }
        public string SeasonPath { get; private set; }
        public string PosterURL { get; private set; }
        public SeriesModel Series { get; private set; }
        public string Name => $"Season {(Number >= 10 ? Number.ToString() : $"0{Number}")}";

        public Season(SeriesModel Series, short Number, string SeasonPath)
        {
            Episodes = new List<Episode>();
            this.Series = Series;
            this.Number = Number;
            this.SeasonPath = SeasonPath;
            GetAssets();
        }

        private void GetAssets()
        {
            using (var client = new System.Net.WebClient())
            {
                string response = client.DownloadString($"https://api.themoviedb.org/3/tv/{Series.TMDBID}/season/{Number}?api_key={Values.TheMovieDBAPIKey}");
                JToken obj = JSON.ParseJson(response);
                PosterURL = $"http://image.tmdb.org/t/p/original{obj["poster_path"]}";
            }
        }
        public Episode GetEpisode(int number)
        {
            for (int i = 0; i < Episodes.Count; i++)
            {
                if (Episodes[i].Number == number) return Episodes[i];
            }
            return null;
        }
        public void SortEpisodeByName()
        {
            bool success = false;
            var list = Episodes.OrderBy(o =>
            {
                try
                {
                    return o.Number;
                }
                catch { success = false; return 0; }
            }).ToList();
            if (success)
            {
                Episodes.Clear();
                Episodes.AddRange(list);
            }
        }

    }
}
