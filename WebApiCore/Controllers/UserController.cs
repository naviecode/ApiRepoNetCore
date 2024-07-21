using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Model.Models;
using ShopApi.Service.Models.UserDto;
using ShopApi.Service;
using ShopApi.Service.Models.Role;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IServiceManager _serviceManager;
        public UserController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("getAll")]
        public async Task<ResponseDataDto<UserResponse>> Gets()
        {
            var response = _serviceManager.UserService.GetAll();
            return response;
        }
        [HttpPost("getAllFilter")]
        public async Task<ResponseDataDto<UserResponse>> GetAllFilter(UserRequest filter)
        {
            var response = _serviceManager.UserService.GetAllByFilter(filter);
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<UserResponse>> GetById(int id)
        {
            var response = _serviceManager.UserService.GetById(id);
            return response;
        }
        [HttpPost("create")]
        public async Task<ResponseActionDto<UserResponse>> Create([FromForm] UserFormFile data)
        {
            var response = _serviceManager.UserService.Add(data);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<UserResponse>> Update([FromForm] UserFormFile data)
        {
            var response = _serviceManager.UserService.Update(data);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<UserResponse>> Delete(int id)
        {
            var response = _serviceManager.UserService.Delete(id);
            return response;
        }
        [HttpPost("register")]
        public async Task<ResponseActionDto<UserResponse>> Register(UserRegister data)
        {
            var response = _serviceManager.UserService.Register(data);
            return response;
        }
        [HttpPost("changePassword")]
        public async Task<ResponseActionDto<UserResponse>> ChangePassword(UserRegister data)
        {
            var response = _serviceManager.UserService.ChangePassword(data);
            return response;
        }
    }
}
