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
    public class RoomManager : IRoomService
    {
        private IRoomDal _roomDal;


        public RoomManager(IRoomDal roomDal)
        {
            _roomDal = roomDal;
        }


        public List<Room> GetAll() => _roomDal.GetList();

        public void Add(Room room)
        {
            _roomDal.Add(room);
        }

        public void Update(Room room)
        {
            _roomDal.Update(room);
        }

        public void Remove(Room room)
        {
            _roomDal.Delete(room);
        }

        public Room GetById(int id) => _roomDal.GetList().FirstOrDefault(r => r.Id == id);
    }
}