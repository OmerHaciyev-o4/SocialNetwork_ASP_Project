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
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<User> GetAll() => _userDal.GetList();

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public void Update(User user)
        {
            _userDal.Update(user);
        }

        public void Remove(User user)
        {
            _userDal.Delete(user);
        }

        public User GetById(int id) => _userDal.Get(u => u.Id == id);
        public User GetByUsername(string username) => _userDal.Get(u => u.Username == username);
    }
}