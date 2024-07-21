using AutoMapper;
using ShopApi.Model.Models;

namespace ShopApi.Service.Models.UserDto
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<User, UserResponse>();
            CreateMap<User, UserUpdate>();
            CreateMap<User, UserCreate>();
            CreateMap<UserUpdate, User>();
            CreateMap<UserCreate, User>();
            CreateMap<UserResponse, User>();
            CreateMap<User, User>().ForMember(x => x.ID, opt => opt.Ignore()); ;
        }
    }
}
