using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.WebUI.Models
{
    public class PostDetailsViewModel
    {
        public User User { get; set; }
        public Post Post { get; set; }
        public List<PostImage> PostImages { get; set; }
    }
}