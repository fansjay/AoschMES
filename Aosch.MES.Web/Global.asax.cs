using Aosch.MES.Web.Controllers;
using log4net;
using log4net.Config;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Aosch.MES.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {

            //log4net日志配置
            XmlConfigurator.Configure();

            //string LogFilePath = Server.MapPath("/Log/");

            //程序一启动的时候开启一线程【用来把队列中存储的错误存入日志文件】      
            CancellationToken cancellationToken = new CancellationToken();
            TaskFactory taskFactory = new TaskFactory(cancellationToken);
            Action action = () => {
                while (true)
                {

                    if (ApplicationHandleErrorAttribute.QueueExceptions.Count > 0) //队列里有错误日志
                    {
                        //File.AppendAllText(a + LogFileName, $"\n{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")} \t {LogType.Error}--->{}\n" );
                        logger.Error(ApplicationHandleErrorAttribute.QueueExceptions.Dequeue().Message);
                    }
                    else
                    {
                        Thread.Sleep(5000);//没有错误 休息省CPU资源
                    }
                }
            };
            taskFactory.StartNew(action);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
