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
    public class MovieModel : MediaModel
    {
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
        /// A List of all crew members
        /// </summary>
        public new CastMembers FullCrew
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

    }
}
