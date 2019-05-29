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
    public class AdminRoomInfoesController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        [ChildActionOnly]
        public ActionResult _Admin()
        {
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }

        // GET: RoomInfoes
        //public ActionResult Index()
        //{
        //    return View(db.RoomInfo.ToList());
        //}

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
            ViewBag.Rtype = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            var result = db.RoomInfo.SqlQuery("SELECT * From RoomInfo");
            if (!String.IsNullOrEmpty(Request.Form["Rtype"]) && String.IsNullOrEmpty(searchString))
            {
                result = db.RoomInfo.SqlQuery("SELECT * From RoomInfo WHERE RtypeID='" + Request.Form["Rtype"]+"'");
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));

        }

        // GET: RoomInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfo.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        // GET: RoomInfoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomInfoes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rtype,Rfloor,Rlive,Rwindow,Rcost,Rreservation,Rpic")] RoomInfo roomInfo)
        {
            if (ModelState.IsValid)
            {
                db.RoomInfo.Add(roomInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomInfo);
        }

        // GET: RoomInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomInfo roomInfo = db.RoomInfo.Find(id);
            if (roomInfo == null)
            {
                return HttpNotFound();
            }
            return View(roomInfo);
        }

        // POST: RoomInfoes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rtype,Rfloor,Rlive,Rwindow,Rcost,Rreservation,Rpic")] RoomInfo roomInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomInfo);
        }

        // GET: RoomInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            RoomInfo roomInfo = db.RoomInfo.Find(id);
            db.RoomNum.RemoveRange(db.RoomNum.Where(c => c.RtypeID == id));
            db.RoomInfo.Remove(roomInfo);
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
                    RoomInfo roomInfo = db.RoomInfo.Find(int.Parse(i));
                    db.RoomNum.RemoveRange(db.RoomNum.Where(c => c.RtypeID == roomInfo.RtypeID));
                    db.RoomInfo.Remove(roomInfo);
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
