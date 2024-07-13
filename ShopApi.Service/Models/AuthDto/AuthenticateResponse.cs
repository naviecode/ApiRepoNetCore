using ShopApi.Model.Models;

namespace ShopApi.Service.Models.AuthDto
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }


        public AuthenticateResponse(User user, string token, string refreshToken)
        {
            Id = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.UserName;
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
