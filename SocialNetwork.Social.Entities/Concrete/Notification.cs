using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class Notification : IEntity
    {
        public int Id { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiveUserId { get; set; }

        public DateTime SendDate { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}