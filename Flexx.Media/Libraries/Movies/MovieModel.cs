using ChaseLabs.CLConfiguration.List;
using com.drewchaseproject.net.Flexx.Core.Data;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using TorrentTitleParser;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies
{
    /// <summary>
    /// A Movie Object
    /// </summary>
    public class MovieModel
    {
        /// <summary>
        /// Movie Title
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Movie Release Year
        /// </summary>
        public short Year { get; private set; }
        /// <summary>
        /// The Movie Database ID
        /// </summary>
        public int TMDBID { get; private set; }
        /// <summary>
        /// The Movie summery or synopsis.
        /// </summary>
        public string Summery { get; private set; }
        public string PosterURL { get; private set; }
        public string CoverURL { get; private set; }
        public string PosterPath
        {
            get
            {
                string path = System.IO.Path.Combine(Path, "Poster.jpg");
                if (!System.IO.File.Exists(path)) DownloadAssets();
                return path;
            }
        }
        public string CoverPath
        {
            get
            {
                string path = System.IO.Path.Combine(Path, "Cover.jpg");
                if (!System.IO.File.Exists(path)) DownloadAssets();
                return path;
            }
        }
        /// <summary>
        /// The Path to the Media File
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The File name without extension.
        /// </summary>
        public string File => new FileInfo(Path).Name;
        /// <summary>
        /// The File extension of the file.
        /// </summary>
        public string Extension => new FileInfo(Path).Extension;
        /// <summary>
        /// The <seealso cref="LibraryModel"/> where this Movie is held.
        /// </summary>
        public LibraryModel Library { get; set; }
        /// <summary>
        /// Checks if <seealso cref="SMDFile"/> Exists
        /// </summary>
        public bool HasSMD => System.IO.File.Exists($"{Title}.smd");
        /// <summary>
        /// Standard Media Data File Path.<br />
        /// Returns <seealso cref="GenerateDetails()"/>
        /// </summary>
        public string SMDFile => GenerateDetails();

        /// <summary>
        /// <b>If SMD File doesn't exist:</b>
        /// <list type="bullet">
        /// <item>
        /// <i>This will gather required information from the filename and The Movie Database and will generate a SMD File (Standard Media Data)</i>
        /// </item>
        /// </list>
        /// <b>Otherwise:</b>
        /// <list type="bullet">
        /// <item>
        /// <i>This will Get information from the existing <seealso cref="SMDFile"/></i>
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>Path to the <seealso cref="SMDFile"/> File</returns>
        public string GenerateDetails(bool force = false)
        {
            ConfigManager smd;
            if (force)
            {
                smd = CreateSMD();
            }
            else
            {
                if (HasSMD)
                {
                    smd = RetrieveSMD();
                }
                else
                {
                    smd = CreateSMD();
                }
            }
            return smd.PATH;
        }

        private ConfigManager RetrieveSMD()
        {
            ConfigManager smd = new ConfigManager(System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Title}.smb"));
            TMDBID = smd.GetConfigByKey("TMDBID").ParseInt();
            Title = smd.GetConfigByKey("Title").Value;
            Year = short.Parse(smd.GetConfigByKey("Year").Value);
            Summery = smd.GetConfigByKey("Summery").Value;
            CoverURL = smd.GetConfigByKey("Cover").Value;
            PosterURL = smd.GetConfigByKey("Poster").Value;
            return smd;
        }
        private ConfigManager CreateSMD()
        {
            SetDataBasedOnJSONResponse();
            Organize();
            ConfigManager smd = new ConfigManager(System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Title}.smb"));
            // Adds items to the SMD File
            smd.Add("TMDBID", TMDBID.ToString());
            smd.Add("Title", Title);
            smd.Add("Year", Year.ToString());
            smd.Add("Summery", Summery);
            smd.Add("Cover", CoverURL);
            smd.Add("Poster", PosterURL);
            DownloadAssets();
            return smd;
        }

        /// <summary>
        /// Checks if the media file is in the correct directory,<br />
        /// If not it will move it to the correct directory.<br />
        /// Will also rename file to proper naming convension.
        /// </summary>
        public void Organize()
        {
            string formattedName = $"{Title} ({Year})";
            string formattedName_Ext = $"{formattedName}{Extension}";
            string movieFolder = System.IO.Path.Combine(Library.Path, formattedName);
            string newPath = Directory.GetParent(Path).FullName.Equals(formattedName) ? Path : System.IO.Path.Combine(movieFolder, formattedName_Ext);
            if (!Directory.Exists(movieFolder))
                Directory.CreateDirectory(movieFolder);
            if (!Path.Equals(newPath))
            {
                System.IO.File.Move(Path, newPath, true);
                Path = newPath;
            }
        }
        /// <summary>
        /// Will convert file name from the torrent name to perfered media formatting
        /// </summary>
        /// <returns></returns>
        private string GetTitleFromTorrentData(bool withYear = true)
        {
            return new Torrent(File).Title + (withYear ? $"({ new Torrent(File).Year})" : "");
        }

        private void DownloadAssets()
        {
            DownloadPoster();
            DownloadCover();
        }

        private void DownloadPoster()
        {
            if (string.IsNullOrWhiteSpace(PosterURL)) GenerateDetails(true);
            new WebClient().DownloadFile(PosterURL, System.IO.Path.Combine(Directory.GetParent(Path).FullName, "Poster.jpg"));
        }
        private void DownloadCover()
        {
            if (string.IsNullOrWhiteSpace(CoverURL)) GenerateDetails(true);
            new WebClient().DownloadFile(CoverURL, System.IO.Path.Combine(Directory.GetParent(Path).FullName, "Cover.jpg"));
        }

        /// <summary>
        /// Will set the<br />
        /// <list type="bullet">
        /// <item><seealso cref="Title"/></item>
        /// <item><seealso cref="TMDBID"/></item>
        /// <item><seealso cref="Year"/></item>
        /// </list>
        /// Based on the response from <seealso cref="GetJsonResponse(string)"/>
        /// 
        /// </summary>
        private void SetDataBasedOnJSONResponse()
        {
            JToken obj;
            try
            {
                obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData()))["results"][0];
            }
            catch
            {
                obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData(false)))["results"][0];
            }
            Title = obj["original_title"].ToString();
            Summery = obj["overview"].ToString();
            CoverURL = $"http://image.tmdb.org/t/p/original{obj["backdrop_path"].ToString()}";
            PosterURL = $"http://image.tmdb.org/t/p/original{obj["poster_path"].ToString()}";
            TMDBID = int.Parse(obj["id"].ToString());
            Year = short.TryParse(obj["release_date"].ToString().Split('-')[0].Replace("-", ""), out short _year) ? _year : 0000;
        }


        /// <summary>
        /// Gets the JSON Response from The Movie Database using the Query
        /// </summary>
        /// <param name="query">Search Name</param>
        /// <returns></returns>
        private string GetJsonResponse(string query)
        {
            return new WebClient().DownloadString($"https://api.themoviedb.org/3/search/movie?api_key={Values.TheMovieDBAPIKey}&query={query.Replace(" ", "%20")}");
        }

    }
}
