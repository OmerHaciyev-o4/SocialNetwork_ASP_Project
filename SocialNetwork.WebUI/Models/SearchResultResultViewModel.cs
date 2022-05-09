using System.Collections.Generic;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.DTOS;

namespace SocialNetwork.WebUI.Models
{
    public class SearchResultResultViewModel
    {
        public List<UserForDetailDto> Users { get; set; }
        //public List<Post> Posts { get; set; }
    }
}