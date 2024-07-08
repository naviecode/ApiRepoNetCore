using ShopApi.Service.Models;

namespace ShopApi.Service.Abstractions
{
    public interface IAuthService
    {
        ResponseActionDto<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
}
