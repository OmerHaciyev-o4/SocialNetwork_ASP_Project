using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Models
{
    public class ActiveUserViewModel
    {
        public User User { get; set; }
        public bool IsActive { get; set; }
    }
}