using Flexx.Core.Data;
using Flexx.Media.Libraries.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flexx.Media.Libraries.Series
{
    public class Episode
    {
        public int Number { get; private set; }
        public string Path { get; private set; }
        public LibraryModel Library { get; private set; }
        public string Title { get; private set; }
        public string Summery { get; private set; }
        public string PosterURL { get; private set; }
        public float WatchPercentage => (float)WatchedDuration / Information.Seconds;
        public bool Watched
        {
            get
            {
                var item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                if (item == null) Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched", false);
                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                return item.ParseBoolean();
            }
            set
            {
                var item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                if (item == null) Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched", false);
                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                item.Value = value.ToString();
            }
        }
        public int WatchedDuration
        {
            get
            {
                var item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                if (item == null) Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched_Duration", 0);
                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                return item.ParseInt();
            }
            set
            {
                var item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                if (item == null) Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched_Duration", 0);
                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                item.Value = value.ToString();
            }
        }
        public SeriesModel Series { get; private set; }
        public Season Season { get; private set; }
        public VideoInformation Information { get; private set; }

        public Episode(string path, int episode, LibraryModel library, SeriesModel show, Season season)
        {
            Path = path;
            Number = episode;
            Library = library;
            Series = show;
            Season = season;
            string response = "";

            using (var client = new System.Net.WebClient())
            {
                response = client.DownloadString($"https://api.themoviedb.org/3/tv/{show.TMDBID}/season/{season.Number}/episode/{episode}?api_key={Values.TheMovieDBAPIKey}");
            }
            if (!string.IsNullOrWhiteSpace(response))
            {
                var obj = JSON.ParseJson(response);
                Title = obj["name"].ToString();
                Summery = obj["overview"].ToString();
                PosterURL = $"http://image.tmdb.org/t/p/original{obj["still_path"]}";
            }
            Information = new(Path);
        }
    }
}
