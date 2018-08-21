using Aosch.MES.Common;
using Aosch.MES.Model;
using Aosch.MES.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class LoginController : Controller
    {
       
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DoLogin()
        {
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPwd))
            {
                return new RedirectResult("/Login/Index");
            }
            //new MailSender().SendMail();

            var account = new AccountService().Login(userName, userPwd);
            if (account == null)
            {
                LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"),LogType.Warning, $"用户名:{userName}的用户登录失败!原因：用户名密码不正确！\n");
                return Json(new { Status = "FAIL"});
            }
            else
            {
                LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"), LogType.Info, $"用户名:{userName}的用户登录成功！\n");
                
                return Json(new {LoginAccount=account,Status="OK"});
            }
            
        }
    }
}