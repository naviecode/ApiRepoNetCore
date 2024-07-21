using Microsoft.AspNetCore.Mvc;
using ShopApi.Service;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Service.Models.ProductCategoryDto;
using ShopApi.Service.Models.Role;

namespace WebApiCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : Controller
    {
        private IServiceManager _serviceManager;
        public RoleController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("getAll")]
        public async Task<ResponseDataDto<RoleResponse>> Gets()
        {
            var response = _serviceManager.RoleService.GetAll();
            return response;
        }
        [HttpPost("getAllFilter")]
        public async Task<ResponseDataDto<RoleResponse>> GetAllFilter(RoleRequest filter)
        {
            var response = _serviceManager.RoleService.GetAllByFilter(filter);
            return response;
        }
        [HttpGet("getCombobox")]
        public async Task<ResponseDataDto<RoleResponse>> GetCombobox()
        {
            var response = _serviceManager.RoleService.GetRoleCombobox();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<RoleResponse>> GetById(int id)
        {
            var response = _serviceManager.RoleService.GetById(id);
            return response;
        }
        [HttpPost("create")]
        public async Task<ResponseActionDto<RoleResponse>> Create(RoleInput data)
        {
            var response = _serviceManager.RoleService.Add(data);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<RoleResponse>> Update(RoleInput data)
        {
            var response = _serviceManager.RoleService.Update(data);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<RoleResponse>> Delete(int id)
        {
            var response = _serviceManager.RoleService.Delete(id);
            return response;
        }
    }
}
