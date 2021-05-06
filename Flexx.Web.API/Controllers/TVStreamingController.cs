using Flexx.Core.Data;
using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Series;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Flexx.Web.API.Controllers
{
    [ApiController]
    [Route("/api/streaming/tv")]
    public class TVStreamingController : ControllerBase
    {
        /// <summary>
        /// Gets all shows loaded by Flexx
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        public IActionResult GetAllShows()
        {
            LibraryListModel.Singleton.TV.Series.SortByName();
            object result;
            if (LibraryListModel.Singleton.TV == null)
            {
                result = new
                {
                    items = "No Shows Found"
                };
            }
            else
            {
                result = new
                {
                    items = LibraryListModel.Singleton.TV.Series.GetSeriesListAsJsonObject()
                };
            }

            return new JsonResult(result);
        }

        /// <summary>
        /// Gets all shows loaded by Flexx with additional user information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("{user}/get")]
        public IActionResult GetAllShows(string user)
        {
            LibraryListModel.Singleton.TV.Series.SortByName();
            Values.Singleton.LoggedInUser = user;
            object result;
            if (LibraryListModel.Singleton.TV == null)
            {
                result = new
                {
                    items = "No Shows Found"
                };
            }
            else
            {
                result = new
                {
                    items = LibraryListModel.Singleton.TV.Series.GetSeriesListAsJsonObject()
                };
            }

            return new JsonResult(result);
        }


        /// <summary>
        /// Gets individual show information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public IActionResult GetShow(int id)
        {
            object result = new
            {
                items = "No Show Found"
            }; ;
            if (LibraryListModel.Singleton.TV != null)
            {
                SeriesObjectModel model = LibraryListModel.Singleton.TV.Series.GetByID(id).objectModel;
                if (model != null)
                {
                    result = new
                    {
                        items = model
                    };
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Gets individual show information and user specific data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("{user}/get/{id}")]
        public IActionResult GetShow(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            object result = new
            {
                items = "No Show Found"
            };
            if (LibraryListModel.Singleton.TV != null)
            {
                SeriesObjectModel model = LibraryListModel.Singleton.TV.Series.GetByID(id).objectModel;
                if (model != null)
                {
                    result = new
                    {
                        items = model
                    };
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Gets a list of all seasons in a show
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet("{user}/get/{id}/seasons")]
        public IActionResult GetSeasons(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            LibraryListModel.Singleton.TV.Series.GetByID(id).SortSeasonByName();
            object result = new
            {
                items = "No Show Found"
            };
            if (LibraryListModel.Singleton.TV != null)
            {
                result = new
                {
                    items = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonsObjectModel()
                };
            }
            return new JsonResult(result);
        }
        /// <summary>
        /// Gets episodes in a given season
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="season"></param>
        /// <returns></returns>
        [HttpGet("{user}/get/{id}/{season}")]
        public IActionResult GetEpisodes(int id, string user, int season)
        {
            Values.Singleton.LoggedInUser = user;
            LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).SortEpisodeByName();
            object result = new
            {
                items = "No Season Found"
            };
            if (LibraryListModel.Singleton.TV != null)
            {
                result = new
                {
                    items = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisodesObjectModel()
                };
            }
            return new JsonResult(result);
        }
        /// <summary>
        /// Gets specific episode data with user information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/get/{id}/{season}/{episode}")]
        public IActionResult GetEpisode(int id, string user, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            object result = new
            {
                items = "No Episode Found"
            };
            if (LibraryListModel.Singleton.TV != null)
            {
                result = new
                {
                    items = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode).objectModel
                };
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Gets the media file for specified episode and creates an output stream.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/{id}/{season}/{episode}/video")]
        public IActionResult GetFile(int id, string user, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            Episode mediaFile;
            try
            {
                mediaFile = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode);
            }
            catch
            {
                mediaFile = null;
            }
            if (mediaFile == null)
            {
                return new NotFoundResult();
            }

            string path = mediaFile.Path;
            FileStream stream = new(path, FileMode.Open, FileAccess.Read);
            FileStreamResult file = File(stream, "video/mp4", true);
            return file;
        }
        /// <summary>
        /// Sets the watch duration in seconds for a given user and episode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="seconds"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/{id}/{season}/{episode}/save/duration/{seconds}")]
        public IActionResult SetWatchedDuration(int id, string user, int seconds, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            Episode mediaFile;
            try
            {
                mediaFile = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode);
            }
            catch
            {
                mediaFile = null;
            }
            if (mediaFile == null)
            {
                return new NotFoundResult();
            }

            mediaFile.WatchedDuration = seconds;
            return Ok();
        }
        /// <summary>
        /// Sets rather an episode has been watched or not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="watched"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/{id}/{season}/{episode}/save/watched/{watched}")]
        public IActionResult SetWatched(int id, string user, bool watched, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            Episode mediaFile;
            try
            {
                mediaFile = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode);
            }
            catch
            {
                mediaFile = null;
            }
            if (mediaFile == null)
            {
                return new NotFoundResult();
            }

            mediaFile.Watched = watched;
            mediaFile.WatchedDuration = 0;
            return Ok();
        }
        /// <summary>
        /// Gets the watch duration in seconds for a specified episode and user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/{id}/{season}/{episode}/get/duration/")]
        public IActionResult GetWatchedDuration(int id, string user, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            Episode mediaFile;
            try
            {
                mediaFile = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode);
            }
            catch
            {
                mediaFile = null;
            }
            if (mediaFile == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(new
            {
                mediaFile.WatchedDuration
            });
        }
        /// <summary>
        /// Gets rather specified episode has been watched or not by a given user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="season"></param>
        /// <param name="episode"></param>
        /// <returns></returns>
        [HttpGet("{user}/{id}/{season}/{episode}/get/watched/")]
        public IActionResult GetWatched(int id, string user, int season, int episode)
        {
            Values.Singleton.LoggedInUser = user;
            Episode mediaFile;
            try
            {
                mediaFile = LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).GetEpisode(episode);
            }
            catch
            {
                mediaFile = null;
            }
            if (mediaFile == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(new
            {
                mediaFile.Watched
            });
        }

    }
}
