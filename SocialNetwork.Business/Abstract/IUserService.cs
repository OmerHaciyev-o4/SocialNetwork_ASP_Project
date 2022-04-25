using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IUserService
    {
        List<User> GetAll();
        void Add(User user);
        void Update(User user);
        void Remove(User user);
        User GetById(int id);
        User GetByUsername(string username);
    }
}