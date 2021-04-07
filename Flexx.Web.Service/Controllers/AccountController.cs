using Microsoft.AspNetCore.Mvc;

namespace Flexx.Web.Service.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Profile");
        }
        [Route("/Account/Profile")]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
