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
    public class RoomCloudManager : IRoomCloudService
    {
        private IRoomCloudDal _roomCloudDal;


        public RoomCloudManager(IRoomCloudDal roomCloudDal)
        {
            _roomCloudDal = roomCloudDal;
        }


        public List<RoomCloud> GetAll() => _roomCloudDal.GetList();

        public void Add(RoomCloud roomCloud)
        {
            _roomCloudDal.Add(roomCloud);
        }

        public void Update(RoomCloud roomCloud)
        {
            _roomCloudDal.Update(roomCloud);
        }

        public void Remove(RoomCloud roomCloud)
        {
            _roomCloudDal.Delete(roomCloud);
        }

        public RoomCloud GetById(int id) => _roomCloudDal.GetList().FirstOrDefault(rc => rc.Id == id);
    }
}