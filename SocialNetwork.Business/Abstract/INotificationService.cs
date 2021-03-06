using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface INotificationService
    {
        List<Notification> GetList();
        void Add(Notification notification);
        void Update(Notification notification);
        void Remove(int id);
        //Notification GetById(int id);
    }
}