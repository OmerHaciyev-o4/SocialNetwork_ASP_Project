using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.Controllers
{
    public class DatabaseController : Controller
    {
        private IUserService _userService;
        private IFriendService _friendService;
        private INotificationService _notificationService;
        private IPostService _postService;
        private IPostImageService _postImageService;
        private Cloudinary _cloudinary;

        public DatabaseController(IUserService userService, IFriendService friendService, INotificationService notificationService, IPostService postService, IPostImageService postImageService)
        {
            _userService = userService;
            _friendService = friendService;
            _notificationService = notificationService;
            _postService = postService;
            _postImageService = postImageService;

            var myAccount = new Account(apiKey: "392371254347452", apiSecret: "7qwJgIJnuMdrYhtOVgj4TxQ2yNQ", cloud: "social-network-web");
            _cloudinary = new Cloudinary(myAccount);
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            var notifications = _notificationService.GetList(HomeController.User.Id);
            notifications.Sort((x, y) => DateTime.Compare(x.SendDate, y.SendDate));
            return Ok(notifications);
        }
        
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet]
        public IActionResult GetFriends()
        {
            return Ok(getFriends());
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            List<User> friends = getFriends();

            List<UserPostViewModel> posts = new List<UserPostViewModel>();
            for (int i = 0; i < friends.Count; i++)
            {
                var friendPosts = _postService.GetAll(friends[i].Id);
                if (friendPosts.Count > 0)
                {
                    var friendPostsVM = new UserPostViewModel() { User = friends[i] };
                    for (int j = 0; j < friendPosts.Count; j++)
                    {
                        var id = friendPosts[j].Id;
                        var postImages = _postImageService.GetAll(id);
                        friendPostsVM.Posts.Add(new PostDetailsViewModel()
                        {
                            Post = friendPosts[j],
                            PostImages = postImages
                        });
                    }
                    posts.Add(friendPostsVM);
                }
            }

            var myPosts = _postService.GetAll(HomeController.User.Id);
            if (myPosts.Count > 0)
            {
                var myPostsVM = new UserPostViewModel() { User = HomeController.User };
                for (int i = 0; i < myPosts.Count; i++)
                {
                    var id = myPosts[i].Id;
                    var postImages = _postImageService.GetAll(id);
                    myPostsVM.Posts.Add(new PostDetailsViewModel()
                    {
                        Post = myPosts[i],
                        PostImages = postImages
                    });
                }

                posts.Add(myPostsVM);
            }

            return Ok(posts);
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

        [HttpPost]
        public IActionResult NewPost(PostViewModel model)
        {
            if (string.IsNullOrEmpty(model.PostMessage))
            {
                if (model.DataList == null || model.DataList.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var post = new Post()
            {
                IdUser = HomeController.User.Id,
                Message = model.PostMessage,
                DatePost = DateTime.Now
            };

            _postService.Add(post);
            Thread.Sleep(800);
            var posts = _postService.GetAll(HomeController.User.Id);
            posts.Sort((x, y) => DateTime.Compare(x.DatePost, y.DatePost));

            for (int i = 0; i < model.DataList.Count; i++)
            {
                var spitedData = model.DataList[i].Split('~');
                var postImage = new PostImage()
                {
                    PostId = posts[^1].Id,
                    PostImageURL = spitedData[0],
                    PostImageType = spitedData[1]
                };
                _postImageService.Add(postImage);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UpdateRating(string data)
        {
            var post = _postService.GetById(Convert.ToInt32(data.Split('~')[0]));

            if (data.Split('~')[1] == "add")
                post.Rating++;
            else if (data.Split('~')[1] == "delete")
                post.Rating--;

            _postService.Update(post);
            return Ok();
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


        private List<User> getFriends()
        {
            List<Friend> friends = _friendService.GetAll();
            List<User> users = new List<User>();
            for (int i = 0; i < friends.Count; i++)
            {
                if (friends[i].UserId == HomeController.User.Id)
                {
                    users.Add(_userService.GetById(friends[i].FriendId));
                }
            }

            return users;
        }
    }
}