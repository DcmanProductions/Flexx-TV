using ChaseLabs.CLConfiguration.List;
using com.drewchaseproject.net.Flexx.Core.Data;
using com.drewchaseproject.net.Flexx.Media.Libraries.Data;
using com.drewchaseproject.net.Flexx.Media.Libraries.Movies.Extras;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TorrentTitleParser;

namespace com.drewchaseproject.net.Flexx.Media.Libraries.Movies
{
    /// <summary>
    /// A Movie Object
    /// </summary>
    public class MovieModel
    {
        #region Fields
        #region Public
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
        public string Language { get; private set; }
        /// <summary>
        /// The Movie summery or synopsis.
        /// </summary>
        public string Summery { get; private set; }
        public VideoInformation MediaDetails { get; private set; }
        public string MPAARating { get; private set; }
        /// <summary>
        /// A Direct URL to the Poster Image
        /// </summary>
        public string PosterURL { get; private set; }
        public List<string> Genres { get; private set; }
        /// <summary>
        /// A Direct URL to the Cover Image
        /// </summary>
        public string CoverURL { get; private set; }
        /// <summary>
        /// Gets the Youtube Trailer URL <seealso cref="TrailerVideoID"/>
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string EmbededYoutubeTrailer => $"https://youtube.com/watch?v={TrailerVideoID}";
        /// <summary>
        /// Returns the direct Video URL based on <seealso cref="EmbededYoutubeTrailer"/><br />
        /// Also See <seealso cref="Trailer"/> for More information.
        /// </summary>
        public string DirectVideoTrailer => new Trailer(this).URL;
        /// <summary>
        /// Gets the Trailer ID based on The Movie Database ID
        /// </summary>
        public string TrailerVideoID => JSON.ParseJson(new WebClient().DownloadString($"https://api.themoviedb.org/3/movie/{TMDBID}/videos?api_key={Values.TheMovieDBAPIKey}"))["results"][0]["key"].ToString();
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
        public bool HasSMD => System.IO.File.Exists(System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Values.GetDirectoryFriendlyString(Title + "")}.smb"));
        /// <summary>
        /// Standard Media Data File Path.<br />
        /// Returns <seealso cref="GenerateDetails()"/>
        /// </summary>
        public string SMDFile => GenerateDetails();
        /// <summary>
        /// A List of all crew members
        /// </summary>
        public CastMembers FullCrew
        {
            get
            {
                if (_fullCrew == null)
                {
                    _fullCrew = new CastMembers(this);
                }

                return _fullCrew;
            }
        }
        /// <summary>
        /// A List of all Actors
        /// </summary>
        public CastMembers Actors
        {
            get
            {
                if (_actors == null)
                {
                    _actors = FullCrew.GetCrewByDepartment("Acting");
                }

                return _actors;
            }
        }
        /// <summary>
        /// A List of all Writers
        /// </summary>
        public CastMembers Writers
        {

            get
            {
                if (_writers == null)
                {
                    _writers = FullCrew.GetCrewByDepartment("Writing");
                }

                return _writers;
            }
        }
        /// <summary>
        /// A List of all Directors
        /// </summary>
        public CastMembers Directors
        {

            get
            {
                if (_directors == null)
                {
                    _directors = FullCrew.GetCrewByDepartment("Directing");
                }

                return _directors;
            }
        }
        #endregion
        #region Private
        private CastMembers _fullCrew;
        private CastMembers _actors;
        private CastMembers _writers;
        private CastMembers _directors;
        private string SMDPath => System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Values.GetDirectoryFriendlyString(Title + "")}.smb");
        private string PosterPath
        {
            get
            {
                string path = System.IO.Path.Combine(Path, "Poster.jpg");
                if (!System.IO.File.Exists(path))
                {
                    DownloadAssets();
                }

                return path;
            }
        }
        private string CoverPath
        {
            get
            {
                string path = System.IO.Path.Combine(Path, "Cover.jpg");
                if (!System.IO.File.Exists(path))
                {
                    DownloadAssets();
                }

                return path;
            }
        }
        #endregion
        #endregion
        #region Functions
        #region Public
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
            InitWatcher();
            MediaDetails = new VideoInformation(Path);
            return smd.PATH;
        }

        /// <summary>
        /// Checks if the media file is in the correct directory,<br />
        /// If not it will move it to the correct directory.<br />
        /// Will also rename file to proper naming convension.
        /// </summary>
        public void Organize()
        {
            string formattedName = Values.GetDirectoryFriendlyString($"{Title} ({Year})");
            string formattedName_Ext = $"{formattedName}{Extension}";
            string movieFolder = System.IO.Path.Combine(Library.Path, formattedName);
            string newPath = Directory.GetParent(Path).FullName.Equals(formattedName) ? Path : System.IO.Path.Combine(movieFolder, formattedName_Ext);
            if (!Directory.Exists(movieFolder))
            {
                Directory.CreateDirectory(movieFolder);
            }

            if (!Path.Equals(newPath))
            {
                System.IO.File.Move(Path, newPath, true);
                Path = newPath;
            }
        }
        #endregion
        #region Private
        private void InitWatcher()
        {
            FileSystemWatcher fsw = new FileSystemWatcher
            {
                Path = Directory.GetParent(Path).FullName,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };
            fsw.Renamed += Refresh;
            fsw.EnableRaisingEvents = true;
        }

        private void Refresh(object Sender, RenamedEventArgs args)
        {
            System.Console.WriteLine("HELLO FROM RENAMED");
            Path = args.FullPath;
            GenerateDetails(true);
        }
        private ConfigManager RetrieveSMD()
        {
            ConfigManager smd = new ConfigManager(SMDPath);
            TMDBID = smd.GetConfigByKey("TMDBID").ParseInt();
            Title = smd.GetConfigByKey("Title").Value;
            Language = smd.GetConfigByKey("Language").Value;
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
            ConfigManager smd = new ConfigManager(SMDPath);
            // Adds items to the SMD File
            smd.Add("TMDBID", TMDBID.ToString());
            smd.Add("Title", Title);
            smd.Add("Language", Language);
            smd.Add("Year", Year.ToString());
            smd.Add("Summery", Summery);
            smd.Add("Cover", CoverURL);
            smd.Add("Poster", PosterURL);
            DownloadAssets();
            return smd;
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
            //DownloadPoster();
            //DownloadCover();
        }

        private void DownloadPoster()
        {
            if (string.IsNullOrWhiteSpace(PosterURL))
            {
                GenerateDetails(true);
            }

            new WebClient().DownloadFile(PosterURL, System.IO.Path.Combine(Directory.GetParent(Path).FullName, "Poster.jpg"));
        }
        private void DownloadCover()
        {
            if (string.IsNullOrWhiteSpace(CoverURL))
            {
                GenerateDetails(true);
            }

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
            JToken obj = null;
            try
            {
                obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData()))["results"][0];
            }
            catch
            {
                try
                {

                    obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData(false)))["results"][0];
                }
                catch
                {
                    obj = null;
                }
            }
            if (obj == null)
            {
                Title = File;
                Summery = "No Information Found about this Movie!";
                CoverURL = "";
                PosterURL = "";
                TMDBID = 0;
                Year = 0000;
            }
            else
            {
                Title = obj["original_title"].ToString();
                Language = Languages.GetByAbbreviation(obj["original_language"].ToString()).Name;
                Summery = obj["overview"].ToString();
                CoverURL = $"http://image.tmdb.org/t/p/original{obj["backdrop_path"]}";
                PosterURL = $"http://image.tmdb.org/t/p/original{obj["poster_path"]}";
                TMDBID = int.Parse(obj["id"].ToString());
                Year = (short)(short.TryParse(obj["release_date"].ToString().Split('-')[0].Replace("-", ""), out short _year) ? _year : 0000);

                obj["genre_ids"].ToList().ForEach(item =>
                {
                    string name = Data.Genres.GetByID(int.Parse(item + "")).Name;
                    if (Genres == null)
                    {
                        Genres = new List<string>();
                    }

                    Genres.Add(name);
                });

                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString($"https://api.themoviedb.org/3/movie/{TMDBID}/release_dates?api_key={Values.TheMovieDBAPIKey}");
                    obj = JSON.ParseJson(response)["results"];
                    MPAARating = "UNKNOWN";
                    foreach (JToken child in obj.Children().ToList())
                    {
                        try
                        {
                            if (child["iso_3166_1"].ToString().ToLower().Equals("us"))
                            {
                                MPAARating = child["release_dates"][0]["certification"].ToString();
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
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
        #endregion
        #endregion
    }
}
