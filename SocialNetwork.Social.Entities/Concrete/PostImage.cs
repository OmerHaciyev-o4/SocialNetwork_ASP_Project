using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.Entities;

namespace SocialNetwork.Social.Entities.Concrete
{
    public class PostImage : IEntity
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public string PostImageURL { get; set; }
        public string PostImageType { get; set; }
    }
}