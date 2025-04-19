using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A23017_Cloud.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.test = ViewBag.Username;
            return View();
        }

        public ActionResult Music()
        {
            return View();
        }
    }
}