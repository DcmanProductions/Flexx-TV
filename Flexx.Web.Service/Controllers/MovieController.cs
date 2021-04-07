using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Flexx.Web.Service.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Movies");
        }

        [Route("/Library/Movies/")]
        public IActionResult Movies()
        {
            return View();
        }
        [Route("/api/media/movies/list")]
        public IActionResult MovieListView()
        {
            return View();
        }
        [Route("/Library/Movies/{movieName}")]
        public IActionResult MediaView(string movieName)
        {
            MediaModel movie = LibraryListModel.Singleton.Movies.Movies.GetByName(movieName);
            return View(movie);
        }
        [Route("/Library/Movies/{movieName}/Trailer")]
        public IActionResult Trailer(string movieName)
        {
            MediaModel movie = LibraryListModel.Singleton.Movies.Movies.GetByName(movieName);
            return View(movie);
        }
        [Route("/Library/Movies/{movieName}/Watch")]
        public IActionResult Watch(string movieName)
        {
            MediaModel movie = LibraryListModel.Singleton.Movies.Movies.GetByName(movieName);
            return View(movie);
        }

        [Route("/Library/Movies/{movieName}/Watch/begin")]
        public IActionResult WatchFromBegining(string movieName)
        {
            MediaModel movie = LibraryListModel.Singleton.Movies.Movies.GetByName(movieName);
            movie.WatchedDuration = 0;
            return RedirectPermanent($"/Library/Movies/{movieName}/Watch");
        }
    }
}
