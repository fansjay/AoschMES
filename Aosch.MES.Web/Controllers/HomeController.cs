using Aosch.MES.Service;
using log4net;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    [LoginFilterAcctribute]
    public class HomeController : Controller
    {

        ILog logger = LogManager.GetLogger("errorMsg");
        public ActionResult Index()
        {
            var accounts= new AccountService().LoadEntities(a => a.ID > 0);
            ViewBag.CurrentAccount = accounts;
            return View();
        }
        public ActionResult GetNavigationsFansjay(string Username)
        {
            var list = new Aosch.MES.Service.RoleService().GetNavigation(Username);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Home()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}