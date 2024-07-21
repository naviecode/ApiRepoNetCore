using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models.UserDto
{
    public class UserRegister
    {
        public int Id { get; set; }
        public string? PassWord { get; set; }
        public string? NewPassword { get; set; }
    }
}
