using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IRoomCloudService
    {
        List<RoomCloud> GetAll();
        void Add(RoomCloud roomCloud);
        void Update(RoomCloud roomCloud);
        void Remove(RoomCloud roomCloud);
        RoomCloud GetById(int id);
    }
}