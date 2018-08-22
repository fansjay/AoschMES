using Aosch.MES.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aosch.MES.Common
{
    public static class CookieHelper
    {
        public static Account GetCurrentAccount()
        {
            //取出Cookie对象
            HttpCookie userInfoCookie = System.Web.HttpContext.Current.Request.Cookies.Get("CurrentAccount");

            //从Cookie对象中取出Json串
            string strUserInfo = HttpUtility.UrlDecode(userInfoCookie.Value, Encoding.GetEncoding("UTF-8"));

            //Json串反序列化为实体
            var CurrentAccount= JsonConvert.DeserializeObject<Account>(strUserInfo);
            return CurrentAccount;
        }
    }
}
