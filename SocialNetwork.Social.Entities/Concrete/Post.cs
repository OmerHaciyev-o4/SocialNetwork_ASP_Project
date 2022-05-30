using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Message { get; set; }
        public DateTime DatePost { get; set; }
        public int Rating { get; set; }
    }
}