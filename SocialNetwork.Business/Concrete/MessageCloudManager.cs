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
    public class MessageCloudManager : IMessageService
    {
        private IMessageCloudDal _messageCloudDal;

        public MessageCloudManager(IMessageCloudDal messageCloudDal)
        {
            _messageCloudDal = messageCloudDal;
        }

        public List<MessageCloud> GetList() => _messageCloudDal.GetList();

        public void Add(MessageCloud messageCloud)
        {
            _messageCloudDal.Add(messageCloud);
        }

        public void Update(MessageCloud messageCloud)
        {
            _messageCloudDal.Update(messageCloud);
        }

        public void Remove(MessageCloud messageCloud)
        {
            _messageCloudDal.Delete(messageCloud);
        }

        public MessageCloud GetById(int id) => _messageCloudDal.GetList().FirstOrDefault(x => x.Id == id);
    }
}