using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Models
{
    public class UserPostViewModel
    {
        public User User { get; set; }
        public List<PostDetailsViewModel> Posts { get; set; }

        public UserPostViewModel()
        {
            Posts = new List<PostDetailsViewModel>();
        }
    }
}