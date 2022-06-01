using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Models
{
    public class MessageCloudViewModel
    {
        public User MyUser { get; set; }
        public User FriendUser { get; set; }
        public List<MessageCloud> Clouds { get; set; } = new List<MessageCloud>();
    }
}