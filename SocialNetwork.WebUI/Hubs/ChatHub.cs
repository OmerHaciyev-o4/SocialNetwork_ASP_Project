using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        public static HttpContext Context { get; set; }

        private UserManager<CustomIdentityUser> _userManager;
        private IUserService _userService;
        private IMessageService _messageService;
        private IHttpContextAccessor httpContextAccessor;

        public ChatHub(IUserService userService, IMessageService messageService, IHttpContextAccessor httpContextAccessor, UserManager<CustomIdentityUser> userManager)
        {
            _userService = userService;
            _messageService = messageService;
            this.httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task SendMessage(string userId, string messageContent, string messageJSON)
        {
            var identityUser = await _userManager.GetUserAsync(Context.User);
            var currentUser = _userService.GetByUsername(identityUser.UserName);
            var friendUser = _userService.GetById(Convert.ToInt32(userId));
            var identityUserF = _userManager.Users.FirstOrDefault(u => u.UserName == friendUser.Username);
            MessageCloud message = null;
            try
            {
                if (messageJSON != null)
                    message = JsonConvert.DeserializeObject<MessageCloud>(messageJSON);
            }
            catch (Exception){}

            if (message == null)
            {
                MessageCloud cloud = new MessageCloud()
                {
                    MyId = currentUser.Id,
                    FriendId = friendUser.Id,
                    Message = messageContent,
                    SendDate = DateTime.Now,
                    Seen = false,
                    SeenDate = DateTime.Now
                };

                _messageService.Add(cloud);

                await Clients.User(identityUserF.Id).SendAsync("ReceiveMessage", friendUser, cloud);
            }
            else if (message != null)
            {
                message.Seen = true;
                message.SeenDate = DateTime.Now;
                _messageService.Update(message);

                await Clients.User(identityUserF.Id).SendAsync("UpdateMessage", friendUser.Id);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(Context.User);
            var currentUser = _userService.GetByUsername(user.UserName);
            UserHelper.ActiveUsers.Add(currentUser);
            string info = "connected successfully";
            await Clients.User(user.Id).SendAsync("Connect", info);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            var userRemoved = UserHelper.ActiveUsers.FirstOrDefault(ac => ac.Username == user.UserName);
            if (userRemoved != null)
            {
                UserHelper.ActiveUsers.Remove(userRemoved);
                string info = "disconnected";
                await Clients.User(user.Id).SendAsync("Disconnect", info);
            }
        }
    }
}