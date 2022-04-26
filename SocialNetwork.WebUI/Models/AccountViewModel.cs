using SocialNetwork.Social.Entities.Concrete;
using Microsoft.AspNetCore.Http;


namespace SocialNetwork.WebUI.Models
{
    public class AccountViewModel
    {
        public User User { get; set; }
        public IFormFile File { get; set; }
    }
}