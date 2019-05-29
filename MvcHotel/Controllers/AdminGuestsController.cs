using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHotel.Models;
using PagedList;

namespace MvcHotel.Controllers
{
    public class AdminGuestsController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        [ChildActionOnly]
        public ActionResult _Admin()
        {
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }

        // GET: Guests
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            
            var result = db.Guest.SqlQuery("SELECT * From Guest");

            if (!String.IsNullOrEmpty(searchString)&&!String.IsNullOrEmpty(Request.Form["findClass"]))
            {
                //result = db.Guest.SqlQuery("SELECT * From Guest WHERE " + Request.Form["findClass"] + " Like '%" + searchString + "%'");
                switch (Request.Form["findClass"])
                {
                    case "1":
                        result = db.Guest.SqlQuery("SELECT * From Guest WHERE Gid LIKE '%" + searchString + "%'");
                        break;
                    case "2":
                        result = db.Guest.SqlQuery("SELECT * From Guest WHERE Gname LIKE '%" + searchString + "%'");
                        break;
                    case "3":
                        result = db.Guest.SqlQuery("SELECT * From Guest WHERE Gtel LIKE '%" + searchString + "%'");
                        break;
                }

                //result = db.Guest.SqlQuery("SELECT * From Guest WHERE Gtel LIKE '%" + searchString + "%'");
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));

        }
        //public ActionResult Index(string searchString)
        //{
        //    var guest = from g in db.Guest select g;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        ////var result = db.Guest.SqlQuery("SELECT * From Guest WHERE Gtel = '" + searchString + "'");
        //        ////return View(result);
        //        //result = db.Guest.SqlQuery("SELECT * From Guest WHERE " + Request.Form["findClass"] + " Like '%" + searchString + "%'");

        //        switch (Request.Form["findClass"])
        //        {
        //            case "1":
        //                guest = guest.Where(g => g.Gid.Contains(searchString));
        //                break;
        //            case "2":
        //                guest = guest.Where(g => g.Gname.Contains(searchString));
        //                break;
        //            case "3":
        //                guest = guest.Where(g => g.Gtel.Contains(searchString));
        //                break;
        //            default:
        //                guest = guest.Where(g => g.Gid.Contains(searchString));
        //                break;
        //        }
        //    }
        //    return View(guest.ToPagedList(pageNumber, pageSize));
        //}
        // GET: Guests/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guest.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // GET: Guests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gname,Gsex,Gid,Gtel")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Guest.Add(guest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guest);
        }

        // GET: Guests/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guest.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gname,Gsex,Gid,Gtel")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public ActionResult Delete(string id)
        {
            //Guest Guest = db.Guest.Find(id);
            //var bookID = db.CheckIn.Where(a => a.Ctel.Equals(Guest.Ctel)).ToString();
            Guest guest = db.Guest.Find(id);
            db.CheckIn.RemoveRange(db.CheckIn.Where(c => c.Gid == id));
            db.Guest.Remove(guest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult MultDelete(FormCollection collection)
        {
            string str = collection["checkitem"];// checkitem复选框的名
            string[] strDelete = str.Split(',');
            foreach (var i in strDelete)
            {
                if (i != "false")
                {
                    Guest guest = db.Guest.Find(i);
                    db.CheckIn.RemoveRange(db.CheckIn.Where(c => c.Gid == i));
                    db.Guest.Remove(guest);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        // POST: Guests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    Guest guest = db.Guest.Find(id);
        //    db.Guest.Remove(guest);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
