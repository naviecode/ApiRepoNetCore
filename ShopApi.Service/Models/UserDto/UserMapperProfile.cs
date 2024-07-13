using AutoMapper;
using ShopApi.Model.Models;

namespace ShopApi.Service.Models.UserDto
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserResponse, User>();
        }
    }
}
