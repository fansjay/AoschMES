using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aosch.MES.Web.Controllers
{
    public class CNCController : Controller
    {
        // GET: CNC
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLogsJson()
        {
            return null;
        }
    }
}