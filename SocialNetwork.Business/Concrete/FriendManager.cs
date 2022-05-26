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


        public FriendManager(IFriendDal friendDal) { _friendDal = friendDal; }



        public void Add(Friend friend) { _friendDal.Add(friend); }

        public bool CheckFriend(int userId, int friendId) => _friendDal.CheckFriend(userId, friendId);

        public List<Friend> GetAll() => _friendDal.GetList();

        public Friend GetById(int id) => _friendDal.GetList().FirstOrDefault(f => f.FriendId == id);

        public void Remove(Friend friend) { _friendDal.Delete(friend); }

        public void Update(Friend friend) { _friendDal.Update(friend); }
    }
}