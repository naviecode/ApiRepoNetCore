using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApi.Service.Models.UserDto;
using System.Security.Claims;

namespace ShopApi.Service.Helpers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var lstItems = filterContext.HttpContext.Items;
            int? userId = null;
            foreach (var item in lstItems)
            {
                if(item.Key.ToString() == "User")
                {
                    userId =  int.Parse(item.Value.ToString());                  
                }
            }

            if (userId == null)
            {
                filterContext.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            if (string.IsNullOrEmpty(Roles))
            {
                return;
            }
            var rolesArray = Roles.Split(',');

            var userRoles = filterContext.HttpContext.User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!rolesArray.Any(role => userRoles.Contains(role)))
            {
                filterContext.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
            }

        }
    }
}
