using ChaseLabs.CLConfiguration.List;
using Flexx.Core.Data;
using Flexx.Media.Libraries.Data;
using Flexx.Media.Libraries.Movies;
using Flexx.Media.Libraries.Movies.Extras;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using TorrentTitleParser;

namespace Flexx.Media.Libraries
{
    public abstract class MediaModel
    {
        #region Fields
        #region Public
        /// <summary>
        /// Movie Title
        /// </summary>
        public string Title { get; protected set; }
        /// <summary>
        /// Movie Release Year
        /// </summary>
        public short Year { get; protected set; }
        /// <summary>
        /// The Movie Database ID
        /// </summary>
        public int TMDBID { get; protected set; }
        /// <summary>
        /// The Language of the Original Movie<br />
        /// <b>EX:</b> <i>English</i><br />
        /// <b>EX 2:</b> <i>Italian</i>
        /// </summary>
        public string Language { get; protected set; }
        /// <summary>
        /// The Movie summery or synopsis.
        /// </summary>
        public string Summery { get; protected set; }
        public bool Watched
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title))
                    return false;
                if (Values.Singleton.UserProfile.GetConfigByKey($"Watched {Title}") == null) Watched = false;
                return Values.Singleton.UserProfile.GetConfigByKey($"Watched {Title}").ParseBoolean();
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    if (Values.Singleton.UserProfile.GetConfigByKey($"Watched {Title}") == null)
                        Values.Singleton.UserProfile.Add($"Watched {Title}", value.ToString());
                    else
                        Values.Singleton.UserProfile.GetConfigByKey($"Watched {Title}").Value = value.ToString();
                }
            }
        }
        public int WatchedDuration
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title))
                    return 0;
                if (Values.Singleton.UserProfile.GetConfigByKey($"WatchedDuration {Title}") == null)
                    WatchedDuration = 0;
                return Values.Singleton.UserProfile.GetConfigByKey($"WatchedDuration {Title}").ParseInt();
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    if (Values.Singleton.UserProfile.GetConfigByKey($"WatchedDuration {Title}") == null)
                        Values.Singleton.UserProfile.Add($"WatchedDuration {Title}", value.ToString());
                    else
                        Values.Singleton.UserProfile.GetConfigByKey($"WatchedDuration {Title}").Value = value.ToString();
                }
            }
        }

        public float WatchPercentage => (float)WatchedDuration / MediaDetails.Seconds;

        /// <summary>
        /// 
        /// </summary>
        public VideoInformation MediaDetails { get; protected set; }
        public string MPAARating { get; protected set; }
        /// <summary>
        /// A Direct URL to the Poster Image
        /// </summary>
        public string PosterURL { get; protected set; }
        public List<string> Genres { get; protected set; }
        /// <summary>
        /// A Direct URL to the Cover Image
        /// </summary>
        public string CoverURL { get; protected set; }
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
        public virtual bool HasSMD => System.IO.File.Exists(System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Values.GetDirectoryFriendlyString(Title + "")}.smb"));
        /// <summary>
        /// Standard Media Data File Path.<br />
        /// Returns <seealso cref="GenerateDetails()"/>
        /// </summary>
        public virtual string SMDFile => GenerateDetails();
        /// <summary>
        /// A List of all crew members
        /// </summary>
        public virtual CastMembers FullCrew
        {
            get
            {
                if (_fullCrew == null && !Title.Equals(File))
                {
                    _fullCrew = new CastMembers((MediaModel)this);
                }

                return _fullCrew;
            }
        }
        /// <summary>
        /// A List of all Actors
        /// </summary>
        public virtual CastMembers Actors
        {
            get
            {
                if (_actors == null && FullCrew != null)
                {
                    _actors = FullCrew.GetCrewByDepartment("Acting");
                }

                return _actors;
            }
        }
        /// <summary>
        /// A List of all Writers
        /// </summary>
        public virtual CastMembers Writers
        {

            get
            {
                if (_writers == null && FullCrew != null)
                {
                    _writers = FullCrew.GetCrewByDepartment("Writing");
                }

                return _writers;
            }
        }
        /// <summary>
        /// A List of all Directors
        /// </summary>
        public virtual CastMembers Directors
        {

            get
            {
                if (_directors == null && FullCrew != null)
                {
                    _directors = FullCrew.GetCrewByDepartment("Directing");
                }

                return _directors;
            }
        }
        #endregion
        #region Private
        protected CastMembers _fullCrew;
        protected CastMembers _actors;
        protected CastMembers _writers;
        protected CastMembers _directors;
        private ConfigManager _smd => new ConfigManager(SMDPath);
        protected virtual string SMDPath => System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Values.GetDirectoryFriendlyString(Title + "")}.smb");
        protected virtual string PosterPath
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
        protected virtual string CoverPath
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
        public virtual string GenerateDetails(bool force = false, ConfigManager _smd = null)
        {
            System.Console.WriteLine($"Generating Details for {Path}");
            ConfigManager smd;

            if (force)
            {
                smd = CreateSMD();
            }
            else
            {
                if (_smd != null)
                    smd = RetrieveSMD(_smd);
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
            }
            InitWatcher();
            MediaDetails = new VideoInformation(Path);
            return smd.PATH;
        }
        public static MediaModel LoadFromSMD(ConfigManager smd, LibraryModel library)
        {
            return null;
        }
        /// <summary>
        /// Checks if the media file is in the correct directory,<br />
        /// If not it will move it to the correct directory.<br />
        /// Will also rename file to proper naming convension.
        /// </summary>
        public virtual void Organize()
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
            Path = args.FullPath;
            GenerateDetails(true);
        }
        protected virtual ConfigManager RetrieveSMD(ConfigManager _smd = null)
        {
            ConfigManager smd = _smd ?? new ConfigManager(SMDPath);
            TMDBID = smd.GetConfigByKey("TMDBID").ParseInt();
            Title = smd.GetConfigByKey("Title").Value;
            Language = smd.GetConfigByKey("Language").Value;
            Year = short.Parse(smd.GetConfigByKey("Year").Value);
            Summery = smd.GetConfigByKey("Summery").Value;
            CoverURL = smd.GetConfigByKey("Cover").Value;
            PosterURL = smd.GetConfigByKey("Poster").Value;
            return smd;
        }
        protected virtual ConfigManager CreateSMD()
        {
            SetDataBasedOnJSONResponse();
            //Organize();
            ConfigManager smd = new(SMDPath);
            // Adds items to the SMD File
            smd.Add("TMDBID", TMDBID.ToString());
            if (Title != null)
                smd.Add("Title", Title);
            if (Language != null)
                smd.Add("Language", Language);
            smd.Add("Year", Year.ToString());
            if (Summery != null)
                smd.Add("Summery", Summery);
            if (CoverURL != null)
                smd.Add("Cover", CoverURL);
            if (PosterURL != null)
                smd.Add("Poster", PosterURL);
            smd.Add("Watched", false.ToString());
            smd.Add("WatchedDuration", 0.ToString());
            smd.Add("File", Path);
            DownloadAssets();
            return smd;
        }
        /// <summary>
        /// Will convert file name from the torrent name to perfered media formatting
        /// </summary>
        /// <returns></returns>
        protected virtual void GetTitleFromTorrentData()
        {
            var torrent = new Torrent(File);
            Title = torrent.Title;
            Year = (short)torrent.Year;
        }

        private void DownloadAssets()
        {
            if (Title.ToLower().Equals(File.ToLower())) return;
            //DownloadPoster();
            //DownloadCover();
        }

        private void DownloadPoster()
        {
            if (string.IsNullOrWhiteSpace(PosterURL))
            {
                GenerateDetails(true);
                return;
            }

            new WebClient().DownloadFile(PosterURL, System.IO.Path.Combine(Directory.GetParent(Path).FullName, "Poster.jpg"));
        }
        private void DownloadCover()
        {
            if (string.IsNullOrWhiteSpace(CoverURL))
            {
                GenerateDetails(true);
                return;
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
                GetTitleFromTorrentData();
                obj = JSON.ParseJson(GetJsonResponse(Title, Year.ToString()))["results"][0];
            }
            catch
            {
                obj = null;
            }
            if (obj == null)
            {
                Title = File;
                Summery = "No Information Found about this Media!";
                CoverURL = "";
                PosterURL = "";
                TMDBID = 0;
                Year = 0000;
            }
            else
            {
                Language = Languages.GetByAbbreviation(obj["original_language"].ToString()).Name;
                Summery = obj["overview"].ToString();
                CoverURL = $"http://image.tmdb.org/t/p/original{obj["backdrop_path"]}";
                PosterURL = $"http://image.tmdb.org/t/p/original{obj["poster_path"]}";
                TMDBID = int.Parse(obj["id"].ToString());
                if (Library.Type.Equals(Values.LibraryType.Movies))
                {

                    Title = obj["original_title"].ToString();
                    Year = (short)(short.TryParse(obj["release_date"].ToString().Split('-')[0].Replace("-", ""), out short _year) ? _year : 0000);
                }
                else
                {
                    Title = obj["original_name"].ToString();
                    Year = (short)(short.TryParse(obj["first_air_date"].ToString().Split('-')[0].Replace("-", ""), out short _year) ? _year : 0000);
                }
                obj["genre_ids"].ToList().ForEach(item =>
                {
                    string name = Data.Genres.GetByID(int.Parse(item + "")).Name;
                    if (Genres == null)
                    {
                        Genres = new List<string>();
                    }

                    Genres.Add(name);
                });
                MPAARating = "UNKNOWN";
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        if (Library.Type.Equals(Values.LibraryType.Movies))
                        {
                            string response = client.DownloadString($"https://api.themoviedb.org/3/movie/{TMDBID}/release_dates?api_key={Values.TheMovieDBAPIKey}");
                            obj = JSON.ParseJson(response)["results"];
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
                        else
                        {
                            string response = client.DownloadString($"https://api.themoviedb.org/3/tv/{TMDBID}/content_ratings?api_key={Values.TheMovieDBAPIKey}");
                            obj = JSON.ParseJson(response)["results"][0];
                            MPAARating = obj["rating"].ToString();
                        }
                    }
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// Gets the JSON Response from The Movie Database using the Query
        /// </summary>
        /// <param name="name">Search Name</param>
        /// <param name="year">Search Year</param>
        /// <returns></returns>
        private string GetJsonResponse(string name, string year)
        {
            return new WebClient().DownloadString($"https://api.themoviedb.org/3/search/{(Library.Type.Equals(Values.LibraryType.Movies) ? "movie" : "tv")}?api_key={Values.TheMovieDBAPIKey}&query={name.Replace(" ", "%20")}{((Library.Type.Equals(Values.LibraryType.Movies) && !string.IsNullOrWhiteSpace(year)) ? "&year=" + year : "")}");
        }
        #endregion
        #endregion
    }
}
