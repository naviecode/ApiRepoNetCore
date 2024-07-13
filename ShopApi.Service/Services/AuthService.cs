using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.AuthDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopApi.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        public AuthService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IUserTokenRepository userTokensRepository, IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _userTokenRepository = userTokensRepository;
            _unitOfWork = unitOfWork;
        }

        public ResponseActionDto<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = _userRepository.GetAll().Where(x => x.UserName == model.Username && x.Password == model.Password).FirstOrDefault();
            // return null if user not found
            if (user == null) return new ResponseActionDto<AuthenticateResponse>(null, CommonConstants.Error, "Đăng nhập thất bại", "Tài khoản or mật khẩu không đúng");

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            //Lưu refresh token
            //Kiểm tra đã có token dưới db chưa
            var userToken = _userTokenRepository.GetAll().Where(x => x.UserID == user.ID).FirstOrDefault();
            if (userToken != null)
            {
                // Lưu trữ refresh token refresh db
                userToken.ValueToken = refreshToken;
                _userTokenRepository.Update(userToken);
                _unitOfWork.Commit();
            }
            else
            {
                _userTokenRepository.Add(new UserTokens() { UserID = user.ID, NameToken = "Refresh Token", ValueToken = refreshToken, LoginProvider = "Web" });
                _unitOfWork.Commit();
            }
            
            return new ResponseActionDto<AuthenticateResponse>(new AuthenticateResponse(user, token, refreshToken), CommonConstants.Success, "Đăng nhập thành công", "");
        }


        public ResponseActionDto<AuthenticateResponse> RefreshToken(RefreshTokenRequest model)
        {
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal == null)
            {
                return new ResponseActionDto<AuthenticateResponse>(null, CommonConstants.Error, "Token không hợp lệ", "");
            }

            var savedRefreshToken = GetRefreshToken(principal); // Lấy refresh token từ cơ sở dữ liệu hoặc bộ nhớ
            
            if (savedRefreshToken != model.RefreshToken)
            {
                return new ResponseActionDto<AuthenticateResponse>(null, CommonConstants.Error, "Refresh Token không hợp lệ", "");
            }
            var idUser = int.Parse(principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var user = _userRepository.GetAll().Where(x => x.ID == idUser).FirstOrDefault();
            var userToken = _userTokenRepository.GetAll().Where(x => x.UserID == idUser).FirstOrDefault();

            var newAccessToken = generateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Lưu trữ refresh token refresh db
            userToken.ValueToken = newRefreshToken;
            _userTokenRepository.Update(userToken);
            _unitOfWork.Commit();


            return new ResponseActionDto<AuthenticateResponse>(new AuthenticateResponse(user, newAccessToken, newRefreshToken), CommonConstants.Success, "Đăng nhập thành công", "");

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
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
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
        private string GetRefreshToken(ClaimsPrincipal user)
        {
            // Lấy refresh token từ cơ sở dữ liệu hoặc bộ nhớ dựa trên thông tin người dùng
            var idUser = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            var tokenRefresh = _userTokenRepository.GetAll().Where(x=>x.UserID == idUser).FirstOrDefault()?.ValueToken;
            return tokenRefresh;
        }
    }
}
