using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Business.Abstract;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Helpers;

namespace SocialNetwork.WebUI.Hubs
{
    public class RegisterUserChatHub : Hub
    {
        private IUserService _userService;
        private UserManager<CustomIdentityUser> _userManager;
        private IHttpContextAccessor _contextAccessor;


        public RegisterUserChatHub(IHttpContextAccessor contextAccessor, IUserService userService, UserManager<CustomIdentityUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userService = userService;
            _userManager = userManager;
        }


        public override async Task OnConnectedAsync()
        {
            var identityUser = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var user = _userService.GetByUsername(identityUser.UserName);
            UserHelper.ActiveUsers.Add(user);
            string info = user.Username + " connected successfully";
            await Clients.All.SendAsync("Connect", info);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var userRemoved = UserHelper.ActiveUsers.SingleOrDefault(u => u.Username == user.UserName);
            if (userRemoved != null)
            {
                UserHelper.ActiveUsers.RemoveAll(u => u.Id == userRemoved.Id);
                string info = user.UserName + " disconnected";
                await Clients.Others.SendAsync("Disconnect", info);
            }
        }
    }
}