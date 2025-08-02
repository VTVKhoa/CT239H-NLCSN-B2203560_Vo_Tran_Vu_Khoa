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
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public ActionResult Login(string Username, string Password)
        {
            User user = db.Users.Find(Username);
            if (user == null || user.Password != Hash.HashString(Password))
            {
                Session["validate"] = "false";
                return RedirectToAction("Login");
            }
            Session["user"] = user;
            Session["Username"] = user.Username;

            if (user is Player)
            {
                Session["role"] = "player";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["role"] = "admin";
                return RedirectToAction("Admin");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password")] User user)
        {
            if (db.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "This user has already existed");
                user.Password = null;
                return View(user);
            }
            if (ModelState.IsValid)
            {
                user.Password = Hash.HashString(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Password", "The password is too short");
                user.Password = null;
                return View(user);
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Username, string Password, HttpPostedFileBase Avatar)
        {
            User user = db.Users.Find(Username);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            if (Password != null && Password.Length < 5 && Password.Length > 0)
            {
                user.Password = null;
                return View(user);
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
            db.SaveChanges();
            if (user.Username == Session["Username"].ToString())
            {
                Session["user"] = user;
            }
            return RedirectToAction("Index");
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            string avatar = user.Avatar;
            db.Users.Remove(user);
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
