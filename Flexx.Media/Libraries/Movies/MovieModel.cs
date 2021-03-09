using com.drewchaseproject.net.Flexx.Core.Data;
using System.IO;
using System.Net;
using TorrentTitleParser;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies
{
    public class MovieModel
    {
        public string Title { get; private set; }
        public short Year { get; private set; }
        public string Summery { get; }
        public string Path { get; set; }
        public string File => new FileInfo(Path).Name;
        public string Extension => new FileInfo(Path).Extension;
        public bool HasNFO => System.IO.Directory.GetFiles(Path, "*.nfo", System.IO.SearchOption.AllDirectories).Length > 0;
        public string NFOFile => HasNFO ? System.IO.Directory.GetFiles(Path, "*.nfo", System.IO.SearchOption.AllDirectories)[0] : string.Empty;

        public void GenerateDetails()
        {
            Newtonsoft.Json.Linq.JToken obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData()))["results"];
            Title = obj["original_title"].ToString();
            Year = short.TryParse(obj["release_date"].ToString().Split('-')[0].Replace("-", ""), out short _year) ? _year : 0000;
            System.IO.File.Move(Path, System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Title} ({Year}).{Extension}"));
        }

        private string GetJsonResponse(string query)
        {
            return new WebClient().DownloadString($"https://api.themoviedb.org/3/search/movie?api_key={Values.TheMovieDBAPIKey}&query={query.Replace(" ", "%20")}");
        }

        public string GetTitleFromTorrentData()
        {
            return new Torrent(File).Title;
        }
    }
}
