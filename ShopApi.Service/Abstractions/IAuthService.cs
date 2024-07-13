using ShopApi.Service.Models.AuthDto;

namespace ShopApi.Service.Abstractions
{
    public interface IAuthService
    {
        ResponseActionDto<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        ResponseActionDto<AuthenticateResponse> RefreshToken(RefreshTokenRequest model);
    }
}
