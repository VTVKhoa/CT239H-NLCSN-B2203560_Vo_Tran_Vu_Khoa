using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A23017_Cloud.Models;

namespace A23017_Cloud.Controllers
{
    public class PlayersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Players
        public ActionResult Index()
        {
            return View(db.Players.ToList());
        }

        // GET: Players/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Point")] Player player)
        {
            if(db.Users.Any(u => u.Username == player.Username))
            {
                ModelState.AddModelError("Username", "This user has already existed");
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Username, string Password, string point, HttpPostedFileBase Avatar)
        {
            Player player = new Player();

            if (ModelState.IsValid)
            {
                player.Username = Username;
                player.Password = Password;
                if (Avatar != null)
                {
                    string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                    string path = Server.MapPath("~/Assets/img/avatar/" + fileName);

                    string date = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                    string extention = Path.GetExtension(path);
                    string newFileName = Path.GetFileNameWithoutExtension(path).ToString() + "_" + date + extention;

                    Console.WriteLine(newFileName);

                    path = Server.MapPath("~/Assets/img/avatar/" + newFileName);

                    Avatar.SaveAs(path);
                    player.Avatar = newFileName;
                }
                Session["user"] = player;
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = player.Username });
            }
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
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
