using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SocialNetwork.WebUI.Controllers;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.ViewComponents
{
    public class PostViewViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {
            return View();
        }
    }
}