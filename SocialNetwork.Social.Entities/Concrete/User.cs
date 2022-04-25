using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string TwonOrCity { get; set; }
        public int PostCode { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Follow { get; set; }
        public int Following { get; set; }

    }
}