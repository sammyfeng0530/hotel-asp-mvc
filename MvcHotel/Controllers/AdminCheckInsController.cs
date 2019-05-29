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
    public class AdminCheckInsController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        [ChildActionOnly]
        public ActionResult _Admin()
        {
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }
        // GET: AdminCheckIns
        //public ActionResult Index()
        //{
        //    var checkIn = db.CheckIn.Include(c => c.Guest).Include(c => c.RoomNum);
        //    return View(checkIn.ToList());
        //}

        public ActionResult Index(string currentFilter, string searchString, int? page, string checkinTimeString, string checkoutTimeString)
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

            var result = db.CheckIn.SqlQuery("SELECT * From CheckIn");

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Request.Form["findClass"]))
            {
                switch (Request.Form["findClass"])
                {
                    case "1":
                        result = db.CheckIn.SqlQuery("SELECT * From CheckIn,Guest WHERE Guest.Gname LIKE '%" + searchString + "%' AND CheckIn.Gid = Guest.Gid");
                        break;
                    case "2":
                        result = db.CheckIn.SqlQuery("SELECT * From CheckIn WHERE Rno LIKE '%" + searchString + "%'");
                        break;
                }
            }
            if (!String.IsNullOrEmpty(checkinTimeString) && String.IsNullOrEmpty(checkoutTimeString))
            {
                page = 1;
                checkinTimeString = currentFilter;
                result = db.CheckIn.SqlQuery("SELECT * From CheckIn WHERE CheckinTime between '" + checkinTimeString + " 00:00:00' AND '" + checkinTimeString + " 23:59:59'");
            }
            else if (String.IsNullOrEmpty(checkinTimeString) && !String.IsNullOrEmpty(checkoutTimeString))
            {
                page = 1;
                checkoutTimeString = currentFilter;
                result = db.CheckIn.SqlQuery("SELECT * From CheckIn WHERE CheckoutTime between '" + checkoutTimeString + " 00:00:00' AND '" + checkoutTimeString + " 23:59:59'");
            }
            else if (!String.IsNullOrEmpty(checkinTimeString) && !String.IsNullOrEmpty(checkoutTimeString))
            {
                page = 1;
                checkinTimeString = currentFilter;
                result = db.CheckIn.SqlQuery("SELECT * From CheckIn WHERE CheckinTime >= '" + checkinTimeString + " 00:00:00' AND CheckoutTime <= '" + checkoutTimeString + " 23:59:59'");
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));

        }

        // GET: AdminCheckIns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkIn = db.CheckIn.Find(id);
            if (checkIn == null)
            {
                return HttpNotFound();
            }
            return View(checkIn);
        }

        // GET: AdminCheckIns/Create
        public ActionResult Create()
        {
            ViewBag.Gid = new SelectList(db.Guest, "Gid", "Gname");
            ViewBag.Rno = new SelectList(db.RoomNum, "Rno", "Rno");
            return View();
        }

        // POST: AdminCheckIns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Gid,Rno,CheckinTime,CheckoutTime,Cash,CheID")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                db.CheckIn.Add(checkIn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Gid = new SelectList(db.Guest, "Gid", "Gname", checkIn.Gid);
            ViewBag.Rno = new SelectList(db.RoomNum, "Rno", "Rno", checkIn.Rno);
            //var obj = db.RoomInfo.Where(a => a.RoomNum.Equals(checkIn.RoomNum)).FirstOrDefault();
            //ViewBag.RtypeID = obj;
            return View(checkIn);
        }

        // GET: AdminCheckIns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckIn checkIn = db.CheckIn.Find(id);
            if (checkIn == null)
            {
                return HttpNotFound();
            }
            ViewBag.Gid = new SelectList(db.Guest, "Gid", "Gname", checkIn.Gid);
            ViewBag.Rno = new SelectList(db.RoomNum, "Rno", "Rno", checkIn.Rno);
            return View(checkIn);
        }

        // POST: AdminCheckIns/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Gid,Rno,CheckinTime,CheckoutTime,Cash,CheID")] CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkIn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Gid = new SelectList(db.Guest, "Gid", "Gname", checkIn.Gid);
            ViewBag.Rno = new SelectList(db.RoomNum, "Rno", "Rno", checkIn.Rno);
            return View(checkIn);
        }

        // GET: AdminCheckIns/Delete/5
        public ActionResult Delete(int? id)
        {
            CheckIn checkIn = db.CheckIn.Find(id);
            db.CheckIn.Remove(checkIn);
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
                    CheckIn checkIn = db.CheckIn.Find(int.Parse(i));
                    db.CheckIn.Remove(checkIn);
                    db.SaveChanges();
                }
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
