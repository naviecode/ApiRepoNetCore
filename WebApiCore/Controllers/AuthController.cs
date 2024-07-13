using Microsoft.AspNetCore.Mvc;
using ShopApi.Service;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.AuthDto;

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
        [HttpPost("refreshToken")]
        public async Task<ResponseActionDto<AuthenticateResponse>> RefreshToken([FromBody]RefreshTokenRequest model)
        {
            var response = _serviceManager.AuthService.RefreshToken(model);
            return response;
        }
    }
}
