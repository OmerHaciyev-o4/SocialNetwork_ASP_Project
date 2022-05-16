using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.WebUI.Controllers
{
    public class DatabaseController : Controller
    {
        private IUserService _userService;
        private IFriendService _friendService;
        private INotificationService _notificationService;

        public DatabaseController(IUserService userService, IFriendService friendService, INotificationService notificationService)
        {
            _userService = userService;
            _friendService = friendService;
            _notificationService = notificationService;
        }
        
        [HttpGet]
        public IActionResult GetNotification()
        {
            var user = JsonConvert.SerializeObject(HomeController.User);

            return Ok(HomeController.User);
        }
    }
}