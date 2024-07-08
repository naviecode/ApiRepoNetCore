using ShopApi.Model.Models;
using ShopApi.Service.Models;

namespace ShopApi.Service.Abstractions
{
    public interface IUserService
    {
        ResponseActionDto<User> Add(UserFormFile data);
        ResponseActionDto<User> Update(UserFormFile data);
        ResponseActionDto<User> Delete(int id);
        ResponseDataDto<User>GetAll();
        ResponseActionDto<User> GetById(int id);
    }
}
