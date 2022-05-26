using SocialNetwork.Social.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Abstract
{
    public interface IFriendService
    {
        bool CheckFriend(int userId, int friendId);

        List<Friend> GetAll();
        void Add(Friend friend);
        void Update(Friend friend);
        void Remove(Friend friend);
        Friend GetById(int id);
    }
}