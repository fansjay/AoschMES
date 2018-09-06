using Aosch.MES.Common;
using Aosch.MES.Service;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class LoginController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

            var account = new AccountService().Login(userName, EncryptUtil.Base64(userPwd));
            if (account == null)
            {
                //LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"),LogType.Warning, $"用户名:{userName}的用户登录失败!原因：用户名密码不正确！\n");
                logger.Error($"用户名:{userName}的用户登录失败!原因：用户名密码不正确!");
                return Json(new { Status = "FAIL"});
            }
            else
            {
                //LoggerHelper.Log(Server.MapPath($"/Log/{DateTime.Now.ToString("yyyyMMdd")}.log"), LogType.Info, $"用户名:{userName}的用户登录成功！\n");
                logger.Info($"用户名:{userName}的用户登录成功!");
                //Session["CurrentAccount"] = account;  session容易失效


                //创建cookie对象
                HttpCookie CurrentAccountCookie = new HttpCookie("CurrentAccount");

                //将序列化之后的Json串以UTF-8编码，再存入Cookie
                CurrentAccountCookie.Value = HttpUtility.UrlEncode(JsonConvert.SerializeObject(account), Encoding.GetEncoding("UTF-8"));

                //将cookie写入到客户端
                System.Web.HttpContext.Current.Response.SetCookie(CurrentAccountCookie);

                //设置cookie保存时间
                CurrentAccountCookie.Expires = DateTime.Now.AddDays(2);
                return Json(new {LoginAccount=account,Status="OK"});
            }
            
        }
    }
}