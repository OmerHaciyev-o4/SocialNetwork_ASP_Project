using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class Room : IEntity
    {
        public int Id { get; set; }
        public int MId { get; set; }
        public int FId { get; set; }
    }
}