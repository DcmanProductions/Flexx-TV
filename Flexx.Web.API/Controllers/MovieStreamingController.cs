using Flexx.Core.Data;
using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Flexx.Web.API.Controllers
{
    [ApiController]
    [Route("/api/streaming/movies")]
    public class MovieStreamingController : ControllerBase
    {
        [HttpGet("get")]
        public IActionResult GetAllMovies()
        {
            object result;
            if (LibraryListModel.Singleton.Movies == null)
                result = new
                {
                    items = "No Movies Found"
                };
            else
                result = new
                {
                    items = LibraryListModel.Singleton.Movies.Movies.GetMovieListAsJsonObject()
                };
            return new JsonResult(result);
        }

        [HttpGet("{user}/get")]
        public IActionResult GetAllMovies(string user)
        {
            Values.Singleton.LoggedInUser = user;
            object result;
            if (LibraryListModel.Singleton.Movies == null)
                result = new
                {
                    items = "No Movies Found"
                };
            else
                result = new
                {
                    items = LibraryListModel.Singleton.Movies.Movies.GetMovieListAsJsonObject()
                };
            return new JsonResult(result);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetMovie(int id)
        {
            object result = new
            {
                items = "No Movie Found"
            }; ;
            if (LibraryListModel.Singleton.Movies != null)
            {
                MovieObjectModel model = LibraryListModel.Singleton.Movies.Movies.GetMovieAsJsonObject(id);
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
        public IActionResult GetMovie(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            object result = new
            {
                items = "No Movie Found"
            }; ;
            if (LibraryListModel.Singleton.Movies != null)
            {
                MovieObjectModel model = LibraryListModel.Singleton.Movies.Movies.GetMovieAsJsonObject(id);
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


        [Route("/api/streaming/{user}/{id}/video")]
        public IActionResult GetFile(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            var mediaFile = LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult();
            string path = mediaFile.Path;
            FileStream stream = new(path, FileMode.Open, FileAccess.Read);
            FileStreamResult file = File(stream, "video/mp4");
            file.EnableRangeProcessing = true;
            return file;
        }
        [Route("/api/streaming/{id}/trailer")]
        public IActionResult GetTrailer(int id)
        {
            MovieModel mediaFile = (MovieModel)LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult();
            return RedirectPermanent(mediaFile.DirectVideoTrailer);
        }
        [Route("/api/streaming/{user}/{id}/save/duration/{seconds}")]
        public IActionResult SetWatchedDuration(int id, string user, int seconds)
        {
            Values.Singleton.LoggedInUser = user;
            var mediaFile = LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult();
            mediaFile.WatchedDuration = seconds;
            return Ok();
        }
        [Route("/api/streaming/{user}/{id}/save/watched/{watched}")]
        public IActionResult SetWatched(int id, string user, bool watched)
        {
            Values.Singleton.LoggedInUser = user;
            var mediaFile = LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult();
            mediaFile.Watched = watched;
            mediaFile.WatchedDuration = 0;
            return Ok();
        }
        [Route("/api/streaming/{user}/{id}/get/duration/")]
        public IActionResult GetWatchedDuration(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            var mediaFile = LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult(); 
            return new JsonResult(new
            {
                mediaFile.WatchedDuration
            });
        }
        [Route("/api/streaming/{user}/{id}/get/watched/")]
        public IActionResult GetWatched(int id, string user)
        {
            Values.Singleton.LoggedInUser = user;
            var mediaFile = LibraryListModel.Singleton.Movies.Movies.GetByID(id);
            if (mediaFile == null) return new NotFoundResult();
            return new JsonResult(new
            {
                mediaFile.Watched
            });
        }

    }
}
