using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Social.Entities.Concrete;

namespace SocialNetwork.Business.Abstract
{
    public interface IRoomService
    {
        List<Room> GetAll();
        void Add(Room room);
        void Update(Room room);
        void Remove(Room room);
        Room GetById(int id);
    }
}