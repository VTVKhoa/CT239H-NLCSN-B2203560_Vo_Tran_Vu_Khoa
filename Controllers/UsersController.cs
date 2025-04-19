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
            if (user == null || user.Password != Password)
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
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
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
            User user = new User();

            if (ModelState.IsValid)
            {
                user.Username = Username;
                user.Password = Password;
                if (Avatar != null)
                {
                    string fileName = System.IO.Path.GetFileName(Avatar.FileName);
                    string path = Server.MapPath("~/Assets/img/avatar/" + fileName);

                    string date = DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
                    string extention = Path.GetExtension(path);
                    string newFileName = Path.GetFileNameWithoutExtension(path).ToString() + "_" + date + extention;

                    path = Server.MapPath("~/Assets/img/avatar/" + newFileName);

                    Avatar.SaveAs(path);
                    user.Avatar = newFileName;

                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
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
            db.Users.Remove(user);
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
