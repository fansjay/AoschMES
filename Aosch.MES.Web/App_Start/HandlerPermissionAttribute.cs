using Aosch.MES.Common;
using Aosch.MES.Service;
using System.Linq;
using System.Web.Mvc;

namespace Aosch.MES.Web.App_Start
{

    //public class HandlerAuthorizeAttribute : ActionFilterAttribute

    /// <summary>
    /// 权限验证
    /// </summary>
    public class HandlerPermissionAttribute:ActionFilterAttribute
    {
        /// <summary>
        /// 是否验证权限
        /// </summary>
        public bool IgnoreCheck { set; get; } 

        public HandlerPermissionAttribute(bool ignore = true)
        {
            this.IgnoreCheck = ignore;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            //验证是当前用户角色名称是否是系统管理员 以上权限
            var roleid = CookieHelper.GetCurrentAccount().RoleID;
            var rolelevel = new RoleService().LoadEntities(r => r.ID == roleid).FirstOrDefault().RoleLevel;
            if (rolelevel > (int)RoleLevel.公司管理)//系统管理员以上都没有权限
            {
                //StringBuilder sbScript = new StringBuilder();
                //sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>");
                //filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                //filterContext.Result = new JavaScriptResult() {Script= "alert('很抱歉！您的权限不足，访问被拒绝！')" }; 
                filterContext.Result = new JsonResult() { Data = new { Status = "Error", Message = "权限不足,访问被拒绝!" } };
                return;
            }

        }

    }
}