using System.ComponentModel.DataAnnotations;

namespace ShopApi.Service.Models.AuthDto
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
