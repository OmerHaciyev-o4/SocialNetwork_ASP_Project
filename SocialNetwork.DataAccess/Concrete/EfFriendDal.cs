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
                var friend = 
                    from f in context.Friends
                    where f.UserId == userId && f.FriendId == friendId
                        select f;

                return friend != null;
            }
        }
    }
}