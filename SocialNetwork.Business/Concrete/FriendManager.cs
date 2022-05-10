using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Business.Abstract;
using SocialNetwork.DataAccess.Abstract;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Concrete
{
    public class FriendManager : IFriendService
    {
        private readonly IFriendDal _friendDal;

        public FriendManager(IFriendDal friendDal)
        {
            _friendDal = friendDal;
        }

        public bool CheckFriend(int userId, int friendId)
        {
            return _friendDal.CheckFriend(userId, friendId);
        }
    }
}