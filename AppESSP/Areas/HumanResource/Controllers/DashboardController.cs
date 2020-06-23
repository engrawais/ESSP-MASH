using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppESSP.Areas.HumanResource.Controllers
{
    public class DashboardController : Controller
    {
        // GET: HumanResource/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}