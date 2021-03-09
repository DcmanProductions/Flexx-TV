using com.drewchaseproject.net.Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Mvc;
namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("Overview");
        }
        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult MediaView(string movieName)
        {
            MovieModel movie = new MovieModel();
            return View(movie);
        }

    }
}
