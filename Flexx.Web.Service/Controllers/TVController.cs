using Flexx.Media.Libraries;
using Flexx.Media.Libraries.Movies;
using Flexx.Media.Libraries.Series;
using Microsoft.AspNetCore.Mvc;

namespace Flexx.Web.Service.Controllers
{
    public class TVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/api/media/tv/list")]
        public IActionResult SeriesListView()
        {
            return View();
        }

        [Route("/Library/TV/")]
        public IActionResult Shows()
        {
            return View();
        }

        [Route("/Library/TV/{series}")]
        public IActionResult SeriesView(string series)
        {
            var show = LibraryListModel.Singleton.TV.Series.GetByName(series);
            return View(show);
        }
        [Route("/Library/TV/{series}/Season/{season}/")]
        public IActionResult EpisodesView(string series, int season)
        {
            var show = LibraryListModel.Singleton.TV.Series.GetByName(series).Seasons.ToArray()[season - 1];
            return View(show);
        }
        [Route("/Library/TV/{series}/Season/{season}/Episode/{episode}")]
        public IActionResult EpisodeMediaView(string series, int season, int episode)
        {
            var model = LibraryListModel.Singleton.TV.Series.GetByName(series).Seasons.ToArray()[season - 1].GetEpisode(episode);
            return View(model);
        }
        [Route("/Library/TV/{series}/Season/{season}/Episode/{episode}/Watch")]
        public IActionResult Watch(string series, int season, int episode)
        {
            var model = LibraryListModel.Singleton.TV.Series.GetByName(series).Seasons.ToArray()[season - 1].Episodes.ToArray()[episode - 1];
            return View(model);
        }
        [Route("/Library/TV/{series}/Season/{season}/Episode/{episode}/Watch/begin")]
        public IActionResult WatchFromBegining(string series, int season, int episode)
        {
            var model = LibraryListModel.Singleton.TV.Series.GetByName(series).Seasons.ToArray()[season - 1].Episodes.ToArray()[episode - 1];
            model.WatchedDuration = 0;
            return RedirectPermanent($"/Library/TV/{series}/Season/{season}/Episode/{episode}/Watch");
        }
    }
}
