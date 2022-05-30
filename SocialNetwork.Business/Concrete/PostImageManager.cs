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
    public class PostImageManager : IPostImageService
    {
        private IPostImageDal _postImageDal;


        public PostImageManager(IPostImageDal postImageDal)
        {
            _postImageDal = postImageDal;
        }


        public List<PostImage> GetAll(int id = -1)
        {
            if (id != -1)
                return _postImageDal.GetList().Where(pi => pi.PostId == id).ToList();
            else
                return _postImageDal.GetList();
        }


        public void Add(PostImage postImage)
        {
            _postImageDal.Add(postImage);
        }

        public void Update(PostImage postImage)
        {
            _postImageDal.Update(postImage);
        }

        public void Remove(PostImage postImage)
        {
            _postImageDal.Delete(postImage);
        }

        public PostImage GetById(int id)=> _postImageDal.GetList().FirstOrDefault(pi => pi.Id == id);
    }
}