using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IMessageService
    {
        List<MessageCloud> GetList();
        void Add(MessageCloud messageCloud);
        void Update(MessageCloud messageCloud);
        void Remove(MessageCloud messageCloud);
        MessageCloud GetById(int id);
    }
}