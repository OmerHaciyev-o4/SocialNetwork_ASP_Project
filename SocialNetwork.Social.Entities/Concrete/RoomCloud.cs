using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class RoomCloud : IEntity
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int MyId { get; set; }
        public int FriendId { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}