using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class LoginFilterAcctribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect("/Login/Index");
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var memberValidation = httpContext.Request.Cookies["Username"];
            var CurrentAccount = httpContext.Request.Cookies["CurrentAccount"];
            if (memberValidation!=null && CurrentAccount != null)
            {
                return true;
            }
            return false;
        }

    }
}