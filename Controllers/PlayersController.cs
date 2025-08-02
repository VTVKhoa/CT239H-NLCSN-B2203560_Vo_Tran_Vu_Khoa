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
using A23017_Cloud.Utils;

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
            if (db.Users.Any(u => u.Username == player.Username))
            {
                ModelState.AddModelError("Username", "This user has already existed");
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                player.Password = Hash.HashString(player.Password);
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Password", "The password is too short");
                player.Password = null;
                return View(player);
            }
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
        public ActionResult Edit(string Username, string Password, HttpPostedFileBase Avatar)
        {
            Player user = (Player)Session["user"];
            if (Password != null && Password.Length < 5 && Password.Length > 0)
            {
                return RedirectToAction("Edit", new { id = Session["Username"] });
            }
            else if (Password != null)
            {
                user.Password = Hash.HashString(Password);
            }
            if (Avatar != null)
            {
                if (Avatar.ContentType != "image/png" && Avatar.ContentType != "image/jpeg")
                {
                    return View(user);
                }
                if (Avatar.ContentLength > 5242880)
                {
                    return View(user);
                }
                FileHandler fileHandler = new FileHandler();
                string tenFileMoi = fileHandler.Save(Avatar, "Assets/img/avatar");
                if (user.Avatar != null && user.Avatar != "blank.jpg")
                {
                    fileHandler.Delete(user.Avatar, "Assets/img/avatar");
                }
                user.Avatar = tenFileMoi;
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = user.Username });
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
            if (player == null)
            {
                return RedirectToAction("Index");
            }
            string avatar = player.Avatar;
            db.Players.Remove(player);
            db.SaveChanges();
            if (avatar != "blank.jpg")
            {
                FileHandler fileHandler = new FileHandler();
                fileHandler.Delete(avatar, "Assets/img/avatar");
            }
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
