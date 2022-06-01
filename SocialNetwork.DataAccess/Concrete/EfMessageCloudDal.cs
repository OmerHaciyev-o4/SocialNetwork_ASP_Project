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
    public class EfMessageCloudDal : EfEntityRepositoryBase<MessageCloud, SocialContext>, IMessageCloudDal
    {
    }
}