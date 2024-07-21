using ShopApi.Service.Models.Role;

namespace ShopApi.Service.Abstractions
{
    public interface IRoleService
    {
        ResponseActionDto<RoleResponse> Add(RoleInput input);
        ResponseActionDto<RoleResponse> Update(RoleInput input);
        ResponseActionDto<RoleResponse> Delete(int id);
        ResponseDataDto<RoleResponse> GetAll();
        ResponseDataDto<RoleResponse> GetAllByFilter(RoleRequest filter);
        ResponseDataDto<RoleResponse> GetRoleCombobox();
        ResponseActionDto<RoleResponse> GetById(int id);
    }
}
