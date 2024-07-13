using Microsoft.AspNetCore.Http;

namespace ShopApi.Service.Models.UserDto
{
    public class UserFormFile
    {
        public string UserData { get; set; }
        public IFormFile? File { get; set; }
    }
}
