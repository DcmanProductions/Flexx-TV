using Microsoft.AspNetCore.Mvc;

namespace com.drewchaseproject.net.Flexx.Web.Service.Controllers
{
    public class WelcomeController : Controller
    {
        [Route("/")]
        public IActionResult Welcome()
        {
            return View();
        }
        //[Route("/Accounts/Login")]
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[Route("/Accounts/SignUp")]
        //public IActionResult SignUp()
        //{
        //    return View();
        //}
    }
}
