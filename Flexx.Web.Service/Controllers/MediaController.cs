using com.drewchaseproject.net.Flexx.Media.Libraries;
using com.drewchaseproject.net.Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Mvc;

namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview");
        }
        public IActionResult Overview()
        {
            return View();
        }
        public IActionResult Movies()
        {
            return View();
        }
        [Route("/Media/Movies/{movieName}")]
        public IActionResult MediaView(string movieName)
        {
            MovieModel movie = LibraryListModel.Singleton.GetByName("movies").Movies.GetByName(movieName);
            return View(movie);
        }
        [Route("/Media/Movies/{movieName}/Trailer")]
        public IActionResult Trailer(string movieName)
        {
            MovieModel movie = LibraryListModel.Singleton.GetByName("movies").Movies.GetByName(movieName);
            return View(movie);
        }
        [Route("/Media/Movies/{movieName}/Watch")]
        public IActionResult Watch(string movieName)
        {
            MovieModel movie = LibraryListModel.Singleton.GetByName("movies").Movies.GetByName(movieName);
            return View(movie);
        }

    }
}
