using Aosch.MES.Common;
using Aosch.MES.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Aosch.MES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            //程序一启动的时候开启一线程【用来把队列中存储的错误存入日志文件】
            string LogFilePath = Server.MapPath("/Log/");
            string LogFileName = $"{DateTime.Now.ToString("yyyyMMdd")}.log";
            CancellationToken cancellationToken = new CancellationToken();
            TaskFactory taskFactory = new TaskFactory(cancellationToken);
            Action<object> action = a => {
                while (true)
                {

                    if (ApplicationHandleErrorAttribute.QueueExceptions.Count > 0) //队列里有错误日志
                    {
                        File.AppendAllText(a + LogFileName, $"\n{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")} \t {LogType.Error}--->{ApplicationHandleErrorAttribute.QueueExceptions.Dequeue().Message}\n" );
                    }
                    else
                    {
                        Thread.Sleep(5000);//没有错误 休息省CPU资源
                    }
                }
            };
            taskFactory.StartNew(action, LogFilePath);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);





        }
    }
}
