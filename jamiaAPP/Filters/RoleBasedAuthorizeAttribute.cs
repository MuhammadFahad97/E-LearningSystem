using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace jamiaAPP.Filters
{
    public class RoleBasedAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (httpContext.Session["UserName"] != null && httpContext.Session["UserID"] != null) {

                if (httpContext.Session["UserRole"].ToString() == "Admin") {
                    return true;

                }
            }
            
            //var rd = httpContext.Request.RequestContext.RouteData;
            //var id = rd.Values["id"] as string;
           
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if ((filterContext.HttpContext.Session["UserName"] != null || filterContext.HttpContext.Session["UserID"] != null) && (filterContext.HttpContext.Session["UserRole"].ToString()=="Student"))
            {
                base.HandleUnauthorizedRequest(filterContext);

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Student", action = "Index" }));
            }
            else {

                base.HandleUnauthorizedRequest(filterContext);

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }
        }

    }
}