using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.DataAccess;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.DataAccess.Abstract
{
    public interface IFriendDal : IEntityRepository<Friend>
    {
        bool CheckFriend(int id, int friendId);
    }
}