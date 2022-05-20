using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;

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
        public IActionResult GetNotification(string data = null)
        {
            var notifications = _notificationService.GetList(HomeController.User.Id);
            if (!string.IsNullOrEmpty(data))
            {
                notifications.Sort((x, y) => DateTime.Compare(x.SendDate, y.SendDate));
            }
            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult AddNotification(string notificationInJson)
        {
            int i = 0;

            var notification = JsonConvert.DeserializeObject<Notification>(notificationInJson);
            notification.SenderUserId = HomeController.User.Id;
            notification.SendDate = DateTime.Now;
            _notificationService.Add(notification);

            //TODO: model with new notification send
            //TODO: new not. write database.
            return Ok();
        }
    }
}