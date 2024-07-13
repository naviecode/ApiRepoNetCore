using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApi.Service.Models.UserDto;

namespace ShopApi.Service.Helpers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var lstItems = filterContext.HttpContext.Items;
            UserResponse? user = null;
            foreach (var item in lstItems)
            {
                if(item.Key.ToString() == "User")
                {
                    ResponseActionDto<UserResponse> responseData =  (ResponseActionDto<UserResponse>)item.Value;
                    user = responseData.Data ?? null;
                  
                }
            }

            if (user == null)
            {
                filterContext.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
