using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Models
{
    public class NotificationViewModel
    {
        public User SenderUser { get; set; }
        public Notification Notification { get; set; }
    }
}