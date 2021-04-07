using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Flexx.Web.Service.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview", "Library");
        }
        public IActionResult Overview()
        {
            return View();
        }


    }
}
