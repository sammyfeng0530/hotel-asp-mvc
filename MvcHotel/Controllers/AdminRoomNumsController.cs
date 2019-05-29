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
    public class AdminRoomNumsController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        [ChildActionOnly]
        public ActionResult _Admin()
        {
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }

        // GET: RoomNums
        //public ActionResult Index()
        //{
        //    var roomNum = db.RoomNum.Include(r => r.RoomInfo);
        //    ViewBag.Rtype = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
        //    return View(roomNum.ToList());
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
            var result = db.RoomNum.SqlQuery("SELECT * From RoomNum");
            if (String.IsNullOrEmpty(Request.Form["Rtype"])&& String.IsNullOrEmpty(searchString))
            {
                result = db.RoomNum.SqlQuery("SELECT * From RoomNum");
            }
            if (!String.IsNullOrEmpty(searchString)&& !String.IsNullOrEmpty(Request.Form["Rtype"]))
            {
                result = db.RoomNum.SqlQuery("SELECT distinct Rno,Rtype,RoomNum.RtypeID From RoomNum,RoomInfo WHERE Rno Like '%" + searchString + "%'");
            }
            if (String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Request.Form["Rtype"]))
            {
                result = db.RoomNum.SqlQuery("SELECT DISTINCT Rno,Rtype,RoomNum.RtypeID From RoomNum,RoomInfo WHERE RoomNum.RtypeID = '" + Request.Form["Rtype"] + "'AND RoomInfo.RtypeID='" + Request.Form["Rtype"]+"'");
            }
            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Request.Form["Rtype"]))
            {
                result = db.RoomNum.SqlQuery("SELECT DISTINCT Rno,Rtype,RoomNum.RtypeID From RoomNum,RoomInfo WHERE RoomNum.RtypeID = '" + Request.Form["Rtype"] + "'AND Rno Like '%" + searchString + "%' AND RoomInfo.RtypeID = '" + Request.Form["Rtype"]+"'");
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        // GET: RoomNums/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomNum roomNum = db.RoomNum.Find(id);
            if (roomNum == null)
            {
                return HttpNotFound();
            }
            return View(roomNum);
        }

        // GET: RoomNums/Create
        public ActionResult Create()
        {
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            return View();
        }

        // POST: RoomNums/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Rno,RtypeID")] RoomNum roomNum)
        {
            if (ModelState.IsValid)
            {
                db.RoomNum.Add(roomNum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", roomNum.RtypeID);
            return View(roomNum);
        }

        // GET: RoomNums/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomNum roomNum = db.RoomNum.Find(id);
            if (roomNum == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rtype = new SelectList(db.RoomInfo, "RtypeID", "Rtype", roomNum.RtypeID);
            return View(roomNum);
        }

        // POST: RoomNums/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Rno,Rtype")] RoomNum roomNum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomNum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rtype = new SelectList(db.RoomInfo, "RtypeID", "Rtype", roomNum.RtypeID);
            return View(roomNum);
        }

        // GET: RoomNums/Delete/5
        public ActionResult Delete(string id)
        {
            RoomNum roomNum = db.RoomNum.Find(id);
            db.RoomNum.Remove(roomNum);
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
                    RoomNum roomNum = db.RoomNum.Find(i);
                    db.RoomNum.Remove(roomNum);
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
        public PartialViewResult _RoomNumInfo(string currentFilter, string searchString, int? page) {

            var result = db.RoomNum.SqlQuery("SELECT * From RoomNum");

            if (!String.IsNullOrEmpty(searchString))
            {
                result = db.RoomNum.SqlQuery("SELECT distinct Rno,Rtype,RoomNum.RtypeID From RoomNum,RoomInfo WHERE Rno Like '%" + searchString + "%'");
            }
            //ViewBag.Rtype = new SelectList(db.RoomInfo, "Rtype", "Rtype");
            if (!String.IsNullOrEmpty(Request.Form["Rtype"]))
            {
                result = db.RoomNum.SqlQuery("SELECT DISTINCT Rno,Rtype,RoomNum.RtypeID From RoomNum,RoomInfo WHERE RoomNum.RtypeID = '" + Request.Form["Rtype"] + "'");
            }
            return PartialView(result.ToList());
        }
    }
}
