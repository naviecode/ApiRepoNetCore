using ShopApi.Service.Models.UserDto;

namespace ShopApi.Service.Abstractions
{
    public interface IUserService
    {
        ResponseActionDto<UserResponse> Add(UserFormFile data);
        ResponseActionDto<UserResponse> Update(UserFormFile data);
        ResponseActionDto<UserResponse> Delete(int id);
        ResponseDataDto<UserResponse> GetAll();
        ResponseDataDto<UserResponse> GetAllByFilter(UserRequest filter);
        ResponseActionDto<UserResponse> GetById(int id);
        ResponseActionDto<UserResponse> Register(UserRegister register);
        ResponseActionDto<UserResponse> ChangePassword(UserRegister register);
    }
}
