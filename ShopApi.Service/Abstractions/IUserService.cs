using ShopApi.Service.Models.UserDto;

namespace ShopApi.Service.Abstractions
{
    public interface IUserService
    {
        ResponseActionDto<UserResponse> Add(UserFormFile data);
        ResponseActionDto<UserResponse> Update(UserFormFile data);
        ResponseActionDto<UserResponse> Delete(int id);
        ResponseDataDto<UserResponse> GetAll();
        ResponseActionDto<UserResponse> GetById(int id);
    }
}
