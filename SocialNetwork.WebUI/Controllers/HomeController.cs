using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Badges()
        {
            return View();
        }
        public IActionResult Stories()
        {
            return View();
        }
        public IActionResult Videos()
        {
            return View();
        }
        public IActionResult Group()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult AccountInformation()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult Notification()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SearchResult()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchResult(string data)
        {
            return RedirectToAction("SearchResult");
        }
        public IActionResult GroupView()
        {
            return View();
        }
        public IActionResult LiveConversation()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
    }
}