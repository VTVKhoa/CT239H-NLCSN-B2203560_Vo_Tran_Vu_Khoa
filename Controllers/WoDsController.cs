using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A23017_Cloud.Models;

namespace A23017_Cloud.Controllers
{
    public class WoDsController : Controller
    {
        private DBContext db = new DBContext();
        public static int Added = 0;

        // GET: WoDs
        public ActionResult Index(string search = "", string icon = "fa-sort-down")
        {
            search = search.Trim();
            search = search.ToLower();
            List<WoD> words = db.WoDs.Where(w => w.Word.Contains(search)).OrderBy(w => w.Word).ToList();
            return View(words);
        }

        // GET: WoDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WoD woD = db.WoDs.Find(id);
            if (woD == null)
            {
                return HttpNotFound();
            }
            return View(woD);
        }

        public ActionResult getWordCount()
        {
            int wordCount = db.WoDs.Count();
            return Json(new { count = wordCount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult increaseAdded()
        {
            Added++;
            return Json(new { added = Added }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAdded()
        {
            return Json(new { added = Added }, JsonRequestBehavior.AllowGet);
        }

        // GET: WoDs/Create
        public ActionResult Create()
        {
            ViewBag.Count = db.WoDs.Count();
            ViewBag.Added = 0;
            return View();
        }

        // POST: WoDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Word,Phonetic,Definition")] WoD woD)
        {
            if (db.WoDs.Any(w => w.Word == woD.Word))
            {
                ModelState.AddModelError("Word", "This word has already exist");
                return View("Create");
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Word", "This word was inncorrectly input, please check again");
                return View("Create");
            }

            ViewBag.Word = woD.Word;

            woD.Word = woD.Word.ToLower();
            db.WoDs.Add(woD);
            db.SaveChanges();

            ViewBag.Count = db.WoDs.Count();
            ViewBag.Added = (ViewBag.Added ?? 0) + 1;

            return RedirectToAction("Create");
        }

        public ActionResult CreateManual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateManual([Bind(Include = "ID,Word,Phonetic,Definition")] WoD woD)
        {
            woD.Word = woD.Word.ToLower();
            if (db.WoDs.Any(w => w.Word == woD.Word))
            {
                ModelState.AddModelError("Word", "This word has already exist");
                return View("CreateManual");
            }
            if(!ModelState.IsValid || woD.Phonetic.Equals("//"))
            {
                ModelState.AddModelError("Word", "Error in input, please check again");
                return View("CreateManual");
            }

            db.WoDs.Add(woD);
            db.SaveChanges();

            return RedirectToAction("CreateManual");
        }

        // GET: WoDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WoD woD = db.WoDs.Find(id);
            if (woD == null)
            {
                return HttpNotFound();
            }
            return View(woD);
        }

        // POST: WoDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Word,Phonetic,Definition")] WoD woD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(woD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(woD);
        }

        // GET: WoDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WoD woD = db.WoDs.Find(id);
            if (woD == null)
            {
                return HttpNotFound();
            }
            return View(woD);
        }

        // POST: WoDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WoD woD = db.WoDs.Find(id);
            db.WoDs.Remove(woD);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
