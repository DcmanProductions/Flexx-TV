using Microsoft.AspNetCore.Mvc;

namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class WelcomeController : Controller
    {
        [Route("Welcome")]
        [Route("/")]
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
