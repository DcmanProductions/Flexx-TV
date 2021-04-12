using Flexx.Core.Data;
using Flexx.Media.Libraries.Data;
using System;

namespace Flexx.Media.Libraries.Series
{
    public class Episode
    {
        public int Number { get; set; }
        public string Path { get; set; }
        public LibraryModel Library { get; set; }
        public string Title { get; set; }
        public string Summery { get; set; }
        public string PosterURL { get; set; }
        public int WatchPercentage => (int)Math.Ceiling(((float)WatchedDuration / Information.Seconds) * 100);
        public bool Watched
        {
            get
            {
                ChaseLabs.CLConfiguration.Object.Config item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                if (item == null)
                {
                    Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched", false);
                }

                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                return item.ParseBoolean();
            }
            set
            {
                ChaseLabs.CLConfiguration.Object.Config item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                if (item == null)
                {
                    Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched", false);
                }

                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched");
                item.Value = value.ToString();
            }
        }
        public int WatchedDuration
        {
            get
            {
                ChaseLabs.CLConfiguration.Object.Config item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                if (item == null)
                {
                    Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched_Duration", 0);
                }

                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                return item.ParseInt();
            }
            set
            {
                ChaseLabs.CLConfiguration.Object.Config item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                if (item == null)
                {
                    Values.Singleton.UserProfile.Add($"{Title}_S{Season.Number}E{Number}_Watched_Duration", 0);
                }

                item = Values.Singleton.UserProfile.GetConfigByKey($"{Title}_S{Season.Number}E{Number}_Watched_Duration");
                item.Value = value.ToString();
            }
        }
        public SeriesModel Series { get; private set; }

        public EpisodeObjectModel objectModel => new()
        {
            Number = Number,
            Name = Title,
            Summery = Summery,
            Watched = Watched,
            PosterURL = PosterURL,
            Duration = Information.Duration,
            Resolution = Information.Resolution.Display,
            WatchedDuration = WatchedDuration,
            WatchedPercentage = WatchPercentage,
        };

        public Season Season { get; private set; }
        public VideoInformation Information { get; private set; }

        public Episode(string path, int episode, SeriesModel show, Season season, LibraryModel library = null)
        {
            Path = path;
            Number = episode;
            Library = library ?? LibraryListModel.Singleton.TV;
            Series = show;
            Season = season;
            string response = "";

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                response = client.DownloadString($"https://api.themoviedb.org/3/tv/{show.TMDBID}/season/{season.Number}/episode/{episode}?api_key={Values.TheMovieDBAPIKey}");
            }
            if (!string.IsNullOrWhiteSpace(response))
            {
                Newtonsoft.Json.Linq.JObject obj = JSON.ParseJson(response);
                Title = obj["name"].ToString();
                Summery = obj["overview"].ToString();
                PosterURL = $"http://image.tmdb.org/t/p/original{obj["still_path"]}";
            }
            Information = new(Path);
        }
    }

    public class TempEpisode : System.Collections.Generic.List<Episode>
    {
        public new void Add(Episode item)
        {
            base.Add(item);
            for (int i = 0; i < Count; i++)
            {
                SeriesModel show = this[i].Series;
                Season season = this[i].Season;
                if (!show.SeasonExists(season)) show.Seasons.Add(season);
                for (int j = 0; j < show.Seasons.Count; j++)
                {
                    if (show.Seasons[j].Number == season.Number && show.Seasons[j].Series.Title.ToLower().Equals(season.Series.Title.ToLower()))
                    {
                        show.Seasons[j].Episodes.Add(this[i]);
                    }
                }
            }
        }
    }


}
