using Flexx.Core.Data;
using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Series;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Flexx.Web.API.Controllers
{
    [ApiController]
    [Route("/api/streaming/tv")]
    public class TVStreamingController : ControllerBase
    {
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
        [HttpGet("{user}/get/{id}/{season}")]
        public IActionResult GetEpisodes(int id, string user, int season)
        {
            Values.Singleton.LoggedInUser = user;
            LibraryListModel.Singleton.TV.Series.GetByID(id).GetSeasonByNumber(season).SortEpisodeByName();
            object result = new
            {
                items = "No Show Found"
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
