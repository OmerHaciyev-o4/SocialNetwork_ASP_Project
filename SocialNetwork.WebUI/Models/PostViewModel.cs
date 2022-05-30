using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SocialNetwork.WebUI.Models
{
    public class PostViewModel
    {
        public int SharedUserId { get; set; }
        public string PostMessage { get; set; }
        public List<string> DataList { get; set; }
    }
}