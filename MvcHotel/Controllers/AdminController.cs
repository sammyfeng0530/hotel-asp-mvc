using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHotel.Models;

namespace MvcHotel.Controllers
{
    public class AdminController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(Admin objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Admin.Where(a => a.Aid.Equals(objUser.Aid) && a.Apwd.Equals(objUser.Apwd)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.Aid.ToString();
                    Session["UserName"] = obj.Aname.ToString();
                    ViewBag.username = obj.Aname.ToString();
                    return RedirectToAction("Index");
                }
            }
            return Content("<script>alert('登录失败，请检查用户名或密码');history.go(-1);</script>");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("AdminIndex", "Home");
        }
        // GET: User
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                ViewBag.username = Session["UserName"];
                //var myInfo = db.Admin.SqlQuery("SELECT * From Admin WHERE Aid='" + Session["UserID"] + "'");
                Admin admin = db.Admin.Find(Session["UserID"]);
                return View(admin);
            }
            else
            {
                return Content("<script>alert('登录失败');history.go(-1);</script>");
            }
        }
        [ChildActionOnly]
        public ActionResult _Admin()
        {
            //var myInfo = db.Admin.SqlQuery("SELECT * From Admin WHERE Aid='" + Session["UserID"] + "'");
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }
        [HttpGet]
        public PartialViewResult _Editpart(string id)
        {
            Admin admin = db.Admin.Find(Session["UserID"]);

            return PartialView("_Editpart", admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Editpart(Admin admin)
        {
            //if (ModelState.IsValid)
            //{
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(admin);
        }
        // GET: Admins
        //public ActionResult Index()
        //{
        //    return View(db.Admin.ToList());
        //}

        //// GET: Admins/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Admin admin = db.Admin.Find(id);
        //    if (admin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(admin);
        //}

        //// GET: Admins/Create
        public PartialViewResult _AddAdmin()
        {
            return PartialView();
        }
        //public ActionResult Create()
        //{
        //    return PartialView();
        //}

        //// POST: Admins/Create
        //// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        //// 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Aid,Apwd,Aname,Atel,Apic")] Admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Admin.Add(admin);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(admin);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _AddAdmin([Bind(Include = "Aid,Apwd,Aname,Atel,Apic")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        //// GET: Admins/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Admin admin = db.Admin.Find(id);
        //    if (admin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(admin);
        //}

        //// POST: Admins/Edit/5
        //// 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        //// 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Aid,Apwd,Aname,Atel,Apic")] Admin admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(admin).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(admin);
        //}

        //// GET: Admins/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Admin admin = db.Admin.Find(id);
        //    if (admin == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(admin);
        //}

        //// POST: Admins/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Admin admin = db.Admin.Find(id);
        //    db.Admin.Remove(admin);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [ChildActionOnly]
        public ActionResult _Adminlist() {
            return PartialView(db.Admin.ToList());
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
