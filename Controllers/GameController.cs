using A23017_Cloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A23017_Cloud.Controllers
{
    public class GameController : Controller
    {
        private WoD word;
        private DBContext db = new DBContext();
        public ActionResult Index(int tries)
        {
            int index = new Random().Next(db.WoDs.Count());
            word = db.WoDs
                     .OrderBy(w => w.ID)
                     .Skip(index)
                     .FirstOrDefault();
            ViewBag.Tries = tries;

            return View(word);
        }
    }
}