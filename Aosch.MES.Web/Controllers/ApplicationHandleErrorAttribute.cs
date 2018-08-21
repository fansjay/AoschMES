using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{

    public class ApplicationHandleErrorAttribute:HandleErrorAttribute
    {

        public static Queue<Exception> QueueExceptions = new Queue<Exception>();
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //出错加入错误队列
            QueueExceptions.Enqueue(filterContext.Exception);

            //抛出异常
            filterContext.HttpContext.Response.Redirect("/Error/Index.html");

        }
    }
}