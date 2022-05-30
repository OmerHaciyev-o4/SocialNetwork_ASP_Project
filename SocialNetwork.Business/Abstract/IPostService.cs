using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IPostService
    {
        List<Post> GetAll(int id = -1);
        void Add(Post post);
        void Update(Post post);
        void Remove(Post post);
        Post GetById(int id);
    }
}