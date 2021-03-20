using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class StreamController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview", "Media");
        }
        [Route("/api/streaming/{library}/{movie}/direct")]
        public IActionResult GetFile(string movie, string library)
        {
            string path = Media.Libraries.LibraryListModel.Singleton.GetByName(library).Movies.GetByName(movie).Path;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var file = File(stream, "video/mp4");
            file.EnableRangeProcessing = true;
            return file;
        }
    }
}
