using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Helpers;
using SocialNetwork.WebUI.Models;
using System.Security.Claims;

namespace SocialNetwork.WebUI.Controllers
{
    public class DatabaseController : Controller
    {
        private IUserService _userService;
        private IFriendService _friendService;
        private INotificationService _notificationService;
        private IPostService _postService;
        private IPostImageService _postImageService;
        private UserManager<CustomIdentityUser> _userManager;
        private Cloudinary _cloudinary;
        public static HttpContext Context { get; set; }

        public DatabaseController(IUserService userService, IFriendService friendService, INotificationService notificationService, IPostService postService, IPostImageService postImageService, UserManager<CustomIdentityUser> userManager)
        {
            _userService = userService;
            _friendService = friendService;
            _notificationService = notificationService;
            _postService = postService;
            _postImageService = postImageService;

            var myAccount = new Account(apiKey: "392371254347452", apiSecret: "7qwJgIJnuMdrYhtOVgj4TxQ2yNQ", cloud: "social-network-web");
            _cloudinary = new Cloudinary(myAccount);
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            //notifications.Sort((x, y) => DateTime.Compare(x.SendDate, y.SendDate));

            var notifications = _notificationService.GetList();
            notifications = notifications.OrderByDescending(n => n.SendDate).ToList();

            List<NotificationViewModel> notificationViewModels = new List<NotificationViewModel>();

            foreach (var notification in notifications)
            {
                var senderUser = _userService.GetById(notification.SenderUserId);
                notificationViewModels.Add(new NotificationViewModel()
                {
                    SenderUser = senderUser,
                    Notification = notification
                });
            }

            return Ok(notificationViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            if (identityUser != null)
            {
                var user = _userService.GetByUsername(identityUser.UserName);
                return Ok(user);
            }

            return Ok();
        }
        
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var user = _userService.GetByUsername(identityUser.UserName);

            var friends = _friendService.GetAll();
            var friendUsers = new List<User>();

            foreach (var friend in friends)
            {
                if (friend.UserId == user.Id)
                    friendUsers.Add(_userService.GetById(friend.FriendId));
            }
            return Ok(friendUsers);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var currentUser = _userService.GetByUsername(identityUser.UserName);
            var posts = _postService.GetAll();
            var friends = _friendService.GetAll();

            var friendsForUsers = new List<User>();
            foreach (var friend in friends)
            {
                if (friend.UserId == currentUser.Id)
                {
                    friendsForUsers.Add(_userService.GetById(friend.FriendId));
                }
            }

            var friendsForPost = new List<PostDetailsViewModel>();
            foreach (var post in posts)
            {
                foreach (var friendsForUser in friendsForUsers)
                {
                    if (post.IdUser == friendsForUser.Id)
                    {
                        var imagesForPost = _postImageService.GetAll(post.Id);

                        friendsForPost.Add(new PostDetailsViewModel()
                        {
                            User = friendsForUser,
                            Post = post,
                            PostImages = imagesForPost
                        });
                        break;
                    }
                    else if (post.IdUser == currentUser.Id)
                    {
                        var imagesForPost = _postImageService.GetAll(post.Id);

                        friendsForPost.Add(new PostDetailsViewModel()
                        {
                            User = currentUser,
                            Post = post,
                            PostImages = imagesForPost
                        });
                        break;
                    }
                }
            }

            friendsForPost = friendsForPost.OrderByDescending(pd => pd.Post.DatePost).ToList();

            return Ok(friendsForPost);





            //List<User> friends = getFriends();

            //List<UserPostViewModel> posts = new List<UserPostViewModel>();
            //for (int i = 0; i < friends.Count; i++)
            //{
            //    var friendPosts = _postService.GetAll(friends[i].Id);
            //    //friendPosts = friendPosts.OrderByDescending(p => p.DatePost).ToList();
            //    if (friendPosts.Count > 0)
            //    {
            //        var friendPostsVM = new UserPostViewModel() { User = friends[i] };
            //        for (int j = 0; j < friendPosts.Count; j++)
            //        {
            //            var id = friendPosts[j].Id;
            //            var postImages = _postImageService.GetAll(id);
            //            friendPostsVM.Posts.Add(new PostDetailsViewModel()
            //            {
            //                Post = friendPosts[j],
            //                PostImages = postImages
            //            });
            //        }
            //        posts.Add(friendPostsVM);
            //    }
                
            //}

            //var myPosts = _postService.GetAll(HomeController.User.Id);
            //if (myPosts.Count > 0)
            //{
            //    var myPostsVM = new UserPostViewModel() { User = HomeController.User };
            //    for (int i = 0; i < myPosts.Count; i++)
            //    {
            //        var id = myPosts[i].Id;
            //        var postImages = _postImageService.GetAll(id);
            //        myPostsVM.Posts.Add(new PostDetailsViewModel()
            //        {
            //            Post = myPosts[i],
            //            PostImages = postImages
            //        });
            //    }

            //    posts.Add(myPostsVM);
            //}

            ////foreach (var item in posts)
            ////{
            ////    item.Posts = item.Posts.OrderByDescending(x => x.Post.DatePost).ToList();
            ////}
            //return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveUsers()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var currentUser = _userService.GetByUsername(identityUser.UserName);
            var activeUsers = UserHelper.ActiveUsers;
            var activeUsersForAPI = new List<ActiveUserViewModel>();

            var friends = _friendService.GetAll();
            var friendsUsers = new List<User>();
            foreach (var friend in friends)
            {
                if (friend.UserId == currentUser.Id)
                    friendsUsers.Add(_userService.GetById(friend.FriendId));
            }

            foreach (var friend in friendsUsers)
            {
                var activeUserForAPI = new ActiveUserViewModel() { User = friend };
                foreach (var activeUser in activeUsers)
                {
                    if (friend.Id == activeUser.Id)
                    {
                        activeUserForAPI.IsActive = true;
                        break;
                    }
                    else
                    {
                        activeUserForAPI.IsActive = false;
                    }
                }
                activeUsersForAPI.Add(activeUserForAPI);
            }

            activeUsersForAPI = activeUsersForAPI.OrderByDescending(au => au.IsActive).ToList();
            return Ok(activeUsersForAPI);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessageClouds(int id)
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var currentUser = _userService.GetByUsername(identityUser.UserName);
            var friendUser = _userService.GetById(id);

            //var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            //var currentUser = _userService.GetByUsername(identityUser.UserName);
            //var friendUser = _userService.GetById(id);

            //var clouds = _messageService.GetList();
            //var roomCloudVM = new MessageCloudViewModel()
            //{
            //    MyUser = currentUser,
            //    FriendUser = friendUser
            //};

            //foreach (var cloud in clouds)
            //{
            //    if (cloud.MyId == currentUser.Id &&
            //        cloud.FriendId == friendUser.Id)
            //    {
            //        roomCloudVM.Clouds.Add(cloud);
            //    }
            //    else if (cloud.MyId == friendUser.Id &&
            //             cloud.FriendId == currentUser.Id)
            //    {
            //        if (!cloud.Seen)
            //        {
            //            cloud.Seen = true;
            //            cloud.SendDate = DateTime.Now;
            //            _messageService.Update(cloud);
            //        }
            //        roomCloudVM.Clouds.Add(cloud);
            //    }
            //}
            //roomCloudVM.Clouds = roomCloudVM.Clouds.OrderByDescending(c => c.SendDate).ToList();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserAppMode()
        {
            var identityUser = await _userManager.GetUserAsync(HttpContext.User);
            var customUser = _userService.GetByUsername(identityUser.UserName);

            if (customUser.IsDarkMode)
                customUser.IsDarkMode = false;
            else if (!customUser.IsDarkMode)
                customUser.IsDarkMode = true;

            HomeController.User = customUser;
            _userService.Update(customUser);

            return Ok(customUser.IsDarkMode);
        }


        [HttpPost]
        public IActionResult AddNotification(string notificationInJson)
        {
            Thread.Sleep(1000);

            var postedNotification = JsonConvert.DeserializeObject<Notification>(notificationInJson);
            postedNotification.SendDate = DateTime.Now;

            var notifications = _notificationService.GetList();
            var verificationAnswer = false;
            foreach (var notification in notifications)
            {
                if (notification.SenderUserId == postedNotification.SenderUserId && 
                    notification.ReceiveUserId == postedNotification.ReceiveUserId && 
                    notification.Title == "Friend Request")
                {
                    verificationAnswer = true;
                    break;
                }
            }

            if (!verificationAnswer)
            {
                _notificationService.Add(postedNotification);
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult RemoveNotification(int notId)
        {
            Thread.Sleep(1000);

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
            var posts = _postService.GetAll();
            var tempPosts = new List<Post>();
            foreach (var post1 in posts)
            {
                if (post1.IdUser == HomeController.User.Id)
                {
                    tempPosts.Add(post1);
                }
            }

            posts = tempPosts.ToArray().ToList();
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

        [HttpPost]
        public IActionResult Follow(int senderId, int myId, int notId)
        {
            var meToFriend = new Friend()
            {
                UserId = myId,
                FriendId = senderId
            };

            var friendToMe = new Friend()
            {
                UserId = senderId,
                FriendId = myId
            };

            _friendService.Add(meToFriend);
            _friendService.Add(friendToMe);
            _notificationService.Remove(notId);

            return Ok();
        }

        [HttpPost]
        public IActionResult UnFollow(int userId, int friendId)
        {
            var friends = _friendService.GetAll();
            Friend meToFriend = null;
            Friend friendToMe = null;
            foreach (var friend in friends)
            {
                if (friend.UserId == userId &&
                    friend.FriendId == friendId)
                {
                    meToFriend = friend;
                }
                else if (friend.UserId == friendId &&
                         friend.FriendId == userId)
                {
                    friendToMe = friend;
                }
            }

            if (meToFriend != null && friendToMe != null)
            {
                _friendService.Remove(meToFriend);
                _friendService.Remove(friendToMe);

                //TODO: Room and RoomClouds delete
                return Ok();
            }

            return BadRequest();
        }

        private List<User> getFriends()
        {
            var user = _userService.GetByUsername(HomeController.User.Username);

            var friends = _friendService.GetAll();
            var friendUsers = new List<User>();

            foreach (var friend in friends)
            {
                if (friend.UserId == user.Id)
                    friendUsers.Add(_userService.GetById(friend.FriendId));
            }

            return friendUsers;
        }
    }
}