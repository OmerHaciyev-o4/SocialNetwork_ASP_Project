using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public IActionResult AddFriend(int id, int notId)
        {
            _friendService.Add(new Friend()
            {
                UserId = HomeController.User.Id,
                FriendId = id
            });
            _friendService.Add(new Friend()
            {
                UserId = id,
                FriendId = HomeController.User.Id
            });
            _notificationService.Remove(notId);

            return Ok();
        }

        [HttpPost]
        public IActionResult AddNotification(string notificationInJson)
        {
            int i = 0;

            var notification = JsonConvert.DeserializeObject<Notification>(notificationInJson);
            notification.SenderUserId = HomeController.User.Id;
            notification.SendDate = DateTime.Now;
            _notificationService.Add(notification);

            return Redirect("/Home/Index");
        }

        [HttpPost]
        public IActionResult RemoveNotification(int notId)
        {
            _notificationService.Remove(notId);
            
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet]
        public IActionResult GetFriends()
        {
            List<Friend> friends = new List<Friend>();
            List<User> users = new List<User>();
            for (int i = 0; i < friends.Count; i++)
            {
                if (friends[i].UserId == HomeController.User.Id)
                {
                    users.Add(_userService.GetById(friends[i].FriendId));
                }
            }

            return Ok(users);
        }
    }
}