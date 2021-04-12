using ChaseLabs.CLConfiguration.List;
using Flexx.Core.Data;
using Flexx.Media.Libraries.Movies.Extras;
using System.Net;

namespace Flexx.Media.Libraries.Movies
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
        public static new MovieModel LoadFromSMD(ConfigManager smd, LibraryModel library)
        {
            if (smd.GetConfigByKey("File") == null)
            {
                return null;
            }

            MovieModel movie = new MovieModel() { Path = smd.GetConfigByKey("File").Value, Library = library };
            movie.GenerateDetails();
            return movie;
        }

    }
}
