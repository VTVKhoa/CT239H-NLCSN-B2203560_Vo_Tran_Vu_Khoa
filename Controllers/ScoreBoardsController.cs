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
    public class ScoreBoardsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: ScoreBoards
        public ActionResult Index()
        {
            var records = db.Records.Include(s => s.User);
            return View(records.OrderByDescending(r => r.Score).ToList());
        }

        // GET: ScoreBoards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreBoard scoreBoard = db.Records.Find(id);
            if (scoreBoard == null)
            {
                return HttpNotFound();
            }
            return View(scoreBoard);
        }

        // GET: ScoreBoards/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Users, "Username", "Password");
            return View();
        }

        // POST: ScoreBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Score,Username")] ScoreBoard scoreBoard)
        {
            if (ModelState.IsValid)
            {
                db.Records.Add(scoreBoard);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Username = new SelectList(db.Users, "Username", "Password", scoreBoard.Username);
            return RedirectToAction("Index", "Home");
        }

        // GET: ScoreBoards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreBoard scoreBoard = db.Records.Find(id);
            if (scoreBoard == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", scoreBoard.Username);
            return View(scoreBoard);
        }

        // POST: ScoreBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Score,Username")] ScoreBoard scoreBoard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoreBoard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Users, "Username", "Password", scoreBoard.Username);
            return View(scoreBoard);
        }

        // GET: ScoreBoards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreBoard scoreBoard = db.Records.Find(id);
            if (scoreBoard == null)
            {
                return HttpNotFound();
            }
            return View(scoreBoard);
        }

        // POST: ScoreBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreBoard scoreBoard = db.Records.Find(id);
            db.Records.Remove(scoreBoard);
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
