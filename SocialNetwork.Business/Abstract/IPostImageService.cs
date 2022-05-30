using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IPostImageService
    {
        List<PostImage> GetAll(int id = -1);
        void Add(PostImage postImage);
        void Update(PostImage postImage);
        void Remove(PostImage postImage);
        PostImage GetById(int id);
    }
}