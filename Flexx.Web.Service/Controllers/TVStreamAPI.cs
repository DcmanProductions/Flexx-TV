using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Flexx.Web.Service.Controllers
{
    public class TVStreamAPI : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Overview", "Library");
        }
        [Route("/api/streaming/tv/{show}/{season}/{episode}/direct")]
        public IActionResult GetFile(string show, int season, int episode)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.TV.Series.GetByName(show).Seasons.ToArray()[season-1].Episodes.ToArray()[episode-1];
            string path = mediaFile.Path;
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            FileStreamResult file = File(stream, "video/mp4");
            file.EnableRangeProcessing = true;
            return file;
        }
        [Route("/api/streaming/tv/{show}/{season}/{episode}/save/duration/{seconds}")]
        public IActionResult SetMediaDuration(string show, int season, int episode, int seconds)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.TV.Series.GetByName(show).Seasons.ToArray()[season - 1].Episodes.ToArray()[episode - 1];
            mediaFile.WatchedDuration = seconds;
            return Ok();
        }
        [Route("/api/streaming/tv/{show}/{season}/{episode}/save/watched/{watched}")]
        public IActionResult SetMediaDuration(string show, int season, int episode, bool watched)
        {
            var mediaFile = Media.Libraries.LibraryListModel.Singleton.TV.Series.GetByName(show).Seasons.ToArray()[season - 1].Episodes.ToArray()[episode - 1];
            mediaFile.Watched = watched;
            mediaFile.WatchedDuration = 0;
            return Ok();
        }
    }
}
