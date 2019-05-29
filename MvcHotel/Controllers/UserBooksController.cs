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
    public class UserBooksController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        // GET: Books
        public ActionResult Index()
        {
            var Rn = db.Book.SqlQuery("SELECT * From Book WHERE Ctel='" + Session["UserID"] + "'ORDER BY Bid DESC");
            //var book = db.Book.Include(b => b.Customer).Include(b => b.RoomInfo);
            return View(Rn);
        }
        public ActionResult _myBooks()
        {
            var Rn = db.Book.SqlQuery("SELECT * From Book WHERE Ctel='" + Session["UserID"] + "'ORDER BY Bid DESC");
            //var book = db.Book.Include(b => b.Customer).Include(b => b.RoomInfo);
            return PartialView(Rn);
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            //ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname");
            ViewBag.Ctel = Session["UserID"];
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            return View();
        }

        // POST: Books/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {

            //book.Ctel = Session["UserID"].ToString();
            book.Ctel = Request["Ctel"].ToString();
            if (ModelState.IsValid)
            {
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index", "UserCustomers");
            }

            ViewBag.Ctel = Session["UserID"].ToString();
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            //return View(book);
            return RedirectToAction("Index", "UserCustomers");
        }
        // GET: Books/Create
        public ActionResult Createc()
        {
            //ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname");
            ViewBag.Ctel = Session["UserID"];
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            if (Request["findkey"] != null)
            {
                string findkey = Request["findkey"];
                var room = db.RoomInfo.SqlQuery("SELECT * From RoomInfo WHERE Rtype='" + findkey + "'");
                return View(room);
            }
            else
            {
                return View(db.RoomInfo.ToList());
            }
        }

        // POST: Books/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Createc(Book book)
        {
            book.Ctel = Session["UserID"].ToString();

            if (ModelState.IsValid)
            {
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ctel = Session["UserID"].ToString();
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return View(book);
        }
        [ChildActionOnly]
        public ActionResult _BookCreate()
        {
            //ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname");
            ViewBag.Ctel = Session["UserID"];
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CheckinTime,Ctel,CheckoutTime,Rtype,Bid")] Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Book.Add(book);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname", book.Ctel);
        //    ViewBag.Rtype = new SelectList(db.RoomInfo, "Rtype", "Rpic", book.Rtype);
        //    return View(book);
        //}

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ctel = Session["UserID"].ToString();
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return View(book);
        }

        // POST: Books/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckinTime,Ctel,CheckoutTime,Rtype,Bid")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ctel = Session["UserID"].ToString();
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return RedirectToAction("Index", "UserCustomers");
            //return View(book);
        }

        // GET: Books/Delete/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Book book = db.Book.Find(id);
            //if (book == null)
            //{
            //    return HttpNotFound();
            //}
            db.Book.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index","UserCustomers");
        }

        // POST: Books/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int? id)
        //{
        //    Book book = db.Book.Find(id);
        //    db.Book.Remove(book);
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [ChildActionOnly]
        public ActionResult _RoomInfo()
        {
            if (Request["findkey"] != null)
            {
                string findkey = Request["findkey"];
                var room = db.RoomInfo.SqlQuery("SELECT * From RoomInfo WHERE Rtype Like '%" + findkey + "%'");
                return PartialView(room);
            }
            else
            {
                return PartialView(db.RoomInfo.ToList());
            }
        }
        [ChildActionOnly]
        public ActionResult _myBooksDetail() {
            return PartialView(db.RoomInfo.ToList());
        }
    }
}
