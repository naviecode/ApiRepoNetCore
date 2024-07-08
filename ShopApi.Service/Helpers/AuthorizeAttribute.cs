using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopApi.Model.Models;
using ShopApi.Service.Models;

namespace ShopApi.Service.Helpers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var lstItems = filterContext.HttpContext.Items;
            User? user = null;
            foreach (var item in lstItems)
            {
                if(item.Key.ToString() == "User")
                {
                    ResponseActionDto<User> responseData =  (ResponseActionDto<User>)item.Value;
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
