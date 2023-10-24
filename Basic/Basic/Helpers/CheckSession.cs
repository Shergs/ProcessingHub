using Basic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Basic.Helpers
{
    public class CheckSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Check if session data is available
            var sessionData = filterContext.HttpContext.Session.GetString("Init");

            if (string.IsNullOrEmpty(sessionData))
            {
                // If the user is authenticated, sign them out
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    var signInManager = filterContext.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                    signInManager.SignOutAsync().Wait();
                }

                // Session data is missing; redirect to login page
                filterContext.Result = new RedirectResult("~/Identity/Account/Login");                
            }
        }
    }
}