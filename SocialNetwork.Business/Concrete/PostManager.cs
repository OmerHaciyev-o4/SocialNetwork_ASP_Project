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
    public class PostManager : IPostService
    {
        private IPostDal _postDal;


        public PostManager(IPostDal postDal)
        {
            _postDal = postDal;
        }


        public List<Post> GetAll(int id = -1)
        {
            if (id != -1)
                return _postDal.GetList().Where(p => p.IdUser == id).ToList();
            else
                return _postDal.GetList();
        }

        public void Add(Post post)
        {
            _postDal.Add(post);
        }

        public void Update(Post post)
        {
            _postDal.Update(post);
        }

        public void Remove(Post post)
        {
            _postDal.Delete(post);
        }

        public Post GetById(int id) => _postDal.GetList().FirstOrDefault(p => p.Id == id);
    }
}