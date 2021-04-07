using Microsoft.AspNetCore.Mvc;
using System.IO;
namespace Flexx.Web.Service.Controllers
{
    public class StreamController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview", "Library");
        }
        [Route("/api/streaming/{library}/{movie}/direct")]
        public IActionResult GetFile(string movie, string library)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.Movies.Movies.GetByName(movie);
            string path = mediaFile.Path;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            FileStreamResult file = File(stream, "video/mp4");
            file.EnableRangeProcessing = true;
            return file;
        }
        [Route("/api/streaming/{library}/{movie}/save/duration/{seconds}")]
        public IActionResult SetMediaDuration(string movie, string library, int seconds)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.Movies.Movies.GetByName(movie);
            mediaFile.WatchedDuration = seconds;
            return Ok();
        }
        [Route("/api/streaming/{library}/{movie}/save/watched/{watched}")]
        public IActionResult SetMediaDuration(string movie, string library, bool watched)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.Movies.Movies.GetByName(movie);
            mediaFile.Watched = watched;
            mediaFile.WatchedDuration = 0;
            return Ok();
        }
    }
}
