using ChaseLabs.CLConfiguration.List;
using com.drewchaseproject.net.Flexx.Core.Data;
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
        public string GenerateDetails()
        {
            ConfigManager smd = new ConfigManager(System.IO.Path.Combine(Directory.GetParent(Path).FullName, $"{Title}.smb"));
            if (HasSMD)
            {
                TMDBID = smd.GetConfigByKey("TMDBID").ParseInt();
                Title = smd.GetConfigByKey("Title").Value;
                Year = short.Parse(smd.GetConfigByKey("Year").Value);
                Summery = smd.GetConfigByKey("Summery").Value;
            }
            else
            {
                SetDataBasedOnJSONResponse();
                Organize();

                // Adds items to the SMD File
                smd.Add("TMDBID", TMDBID.ToString());
                smd.Add("Title", Title);
                smd.Add("Year", Year.ToString());
                smd.Add("Summery", Summery);
            }
            return smd.PATH;
        }

        /// <summary>
        /// Checks if the media file is in the correct directory,<br />
        /// If not it will move it to the correct directory.<br />
        /// Will also rename file to proper naming convension.
        /// </summary>
        public void Organize()
        {
            string formattedName = $"{Title} ({Year})";
            string formattedName_Ext = $"{formattedName}.{Extension}";
            string newPath = Directory.GetParent(Path).FullName.Equals(formattedName) ? Path : System.IO.Path.Combine(Library.Path, formattedName, formattedName_Ext);
            System.IO.File.Move(Path, System.IO.Path.Combine(Directory.GetParent(Path).FullName, formattedName_Ext));
        }
        /// <summary>
        /// Will convert file name from the torrent name to perfered media formatting
        /// </summary>
        /// <returns></returns>
        public string GetTitleFromTorrentData()
        {
            return new Torrent(File).Title;
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
            Newtonsoft.Json.Linq.JToken obj = JSON.ParseJson(GetJsonResponse(GetTitleFromTorrentData()))["results"];
            Title = obj["original_title"].ToString();
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
