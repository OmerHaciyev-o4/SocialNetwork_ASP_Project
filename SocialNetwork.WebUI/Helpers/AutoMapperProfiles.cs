using AutoMapper;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.DTOS;

namespace SocialNetwork.WebUI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailDto>();
        }
    }
}