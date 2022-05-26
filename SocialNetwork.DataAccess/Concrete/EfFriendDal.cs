using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Core.DataAccess.EntityFramework;
using SocialNetwork.DataAccess.Abstract;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.DataAccess.Concrete
{
    public class EfFriendDal : EfEntityRepositoryBase<Friend, SocialContext>, IFriendDal
    {
        public bool CheckFriend(int userId, int friendId)
        {
            using (var context = new SocialContext())
            {
                if (!context.Friends.Any())
                    return false;

                var friend = context.Set<Friend>().FirstOrDefault(uf => uf.UserId == userId && uf.FriendId == friendId);

                return friend != null;
            }
        }
    }
}