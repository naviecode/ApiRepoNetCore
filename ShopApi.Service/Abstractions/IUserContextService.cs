using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Abstractions
{
    public interface IUserContextService
    {
        string GetUserName();
        string GetUserRole();
    }
}
