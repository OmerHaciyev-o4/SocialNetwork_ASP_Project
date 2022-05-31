using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Business.Abstract;

namespace SocialNetwork.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private IUserService _userService;
        private IHttpContextAccessor _contextAccessor;


        public ChatHub(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }


    }
}