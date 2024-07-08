using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Model.Models;

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
        public async Task<ResponseDataDto<User>> Gets()
        {
            var response = _serviceManager.UserService.GetAll();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<User>> GetById(int id)
        {
            var response = _serviceManager.UserService.GetById(id);
            return response;
        }
        [HttpPost("create")]
        public async Task<ResponseActionDto<User>> Create([FromForm] UserFormFile data)
        {
            var response = _serviceManager.UserService.Add(data);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<User>> Update([FromForm] UserFormFile data)
        {
            var response = _serviceManager.UserService.Update(data);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<User>> Delete(int id)
        {
            var response = _serviceManager.UserService.Delete(id);
            return response;
        }
    }
}
