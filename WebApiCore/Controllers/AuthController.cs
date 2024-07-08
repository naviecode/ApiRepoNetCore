using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IServiceManager _serviceManager;
        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost("login")]
        public async Task<ResponseActionDto<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
        {
            var response = _serviceManager.AuthService.Authenticate(model);
            return response;
        }
    }
}
