using Microsoft.AspNetCore.Mvc;
using System.IO;
using VikingErik.Mvc.ResumingActionResults;
namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class StreamController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview", "Media");
        }
        [Route("/api/streaming/{library}/{movie}/direct/{start}")]
        public IActionResult GetFile(string movie, string library, long start = 0)
        {
            string path = Media.Libraries.LibraryListModel.Singleton.GetByName(library).Movies.GetByName(movie).Path;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //stream.Seek(1024, SeekOrigin.Begin);
            //ResumingFileStreamResult fsr = new ResumingFileStreamResult(stream, "video/mp4");
            return File(stream, "video/mp4");
            //return File(bytes, "video/mp4", path);
        }
    }
}
