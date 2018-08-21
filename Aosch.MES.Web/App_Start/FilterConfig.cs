using Aosch.MES.Web.Controllers;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            filters.Add(new ApplicationHandleErrorAttribute());//自定义错误
        }
    }
}
