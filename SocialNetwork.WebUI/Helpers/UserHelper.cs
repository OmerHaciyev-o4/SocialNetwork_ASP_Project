using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;

namespace SocialNetwork.WebUI.Helpers
{
    public class UserHelper
    {
        public static User SenderUser { get; set; }
        public static User ReceiverUser { get; set; }
        public static List<User> ActiveUsers { get; set; } = new List<User>();
        public static List<CustomIdentityUser> IdentityActiveUsers { get; set; } = new List<CustomIdentityUser>();
    }
}