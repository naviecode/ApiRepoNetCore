using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Service.Models;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace ShopApi.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public AuthService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public ResponseActionDto<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = _userRepository.GetAll().Where(x => x.UserName == model.Username && x.Password == model.Password).FirstOrDefault();
            // return null if user not found
            if (user == null) return new ResponseActionDto<AuthenticateResponse>(null, CommonConstants.Error, "Đăng nhập thất bại", "Tài khoản or mật khẩu không đúng");

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new ResponseActionDto<AuthenticateResponse>(new AuthenticateResponse(user, token), CommonConstants.Success, "Đăng nhập thành công", "");
        }


        // helper methods
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim("id", user.ID.ToString()),
                new Claim(ClaimTypes.Name, "quangson"),
                new Claim(ClaimTypes.MobilePhone, "0123456789"),
                new Claim(ClaimTypes.Role, "admin")
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
