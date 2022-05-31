using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Helpers
{
    public static class UserHelper
    {
        public static User SenderUser { get; set; }
        public static User ReceiverUser { get; set; }
        public static List<User> ActiveUsers { get; set; } = new List<User>();
    }
}