using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Models.Role
{
    public class RoleRequest
    {
        public string? Name { get; set; }
        public bool? Status { get; set; }
    }
}
