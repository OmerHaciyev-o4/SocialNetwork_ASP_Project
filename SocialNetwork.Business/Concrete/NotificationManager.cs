using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Business.Abstract;
using SocialNetwork.DataAccess.Abstract;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;


        public NotificationManager(INotificationDal notificationDal)
        {
            _notificationDal = notificationDal;
        }


        public void Add(Notification notification)
        {
            _notificationDal.Add(notification);
        }

        //public Notification GetById(int id) => _notificationDal.GetList().FirstOrDefault(not => not.Id == id);

        public List<Notification> GetList(int id) => _notificationDal.GetList().Where(not => not.ReceiveUserId == id).ToList();

        public void Remove(int id)
        {
            _notificationDal.Delete(_notificationDal.GetList().FirstOrDefault(not => not.Id == id));
        }

        public void Update(Notification notification)
        {
            _notificationDal.Update(notification);
        }
    }
}