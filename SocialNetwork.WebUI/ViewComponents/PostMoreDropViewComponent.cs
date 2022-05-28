using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace SocialNetwork.WebUI.ViewComponents
{
    public class PostMoreDropViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {
            return View();
        }
    }
}