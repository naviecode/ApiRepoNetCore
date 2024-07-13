using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Service.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor) {
            this._httpContextAccessor = httpContextAccessor;
        
        }

        public string GetUserName()
        {
            var lstItems = _httpContextAccessor.HttpContext.Items;
            UserResponse? user = null;
            foreach (var item in lstItems)
            {
                if (item.Key.ToString() == "User")
                {
                    ResponseActionDto<UserResponse> responseData = (ResponseActionDto<UserResponse>)item.Value;
                    user = responseData.Data ?? null;
                }
            }

            if (user == null)
            {
                return "";
            }
            return user.UserName.ToString();

        }

        public string GetUserRole()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}
