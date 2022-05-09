using System.Collections.Generic;

namespace SocialNetwork.WebUI.DTOS
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string BgImageURL { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string TownOrCity { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public int Follow { get; set; }
        public int Following { get; set; }
        public bool IsFriend { get; set; }
    }
}