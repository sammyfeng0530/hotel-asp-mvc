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
    public class AdminBooksController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        [ChildActionOnly]
        public ActionResult _Admin()
        {
            Admin admin = db.Admin.Find(Session["UserID"]);
            return PartialView(admin);
        }

        // GET: Books
        //public ActionResult Index()
        //{
        //    var book = db.Book.Include(b => b.Customer).Include(b => b.RoomInfo);
        //    return View(book.ToList());
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

            var result = db.Book.SqlQuery("SELECT * From Book");

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(Request.Form["findClass"]))
            {
                switch (Request.Form["findClass"])
                {
                    case "1":
                        result = db.Book.SqlQuery("SELECT CheckinTime,CheckoutTime,Book.Ctel,RtypeID,Bid From Book,Customer WHERE Customer.Cname LIKE '%" + searchString + "%' AND Customer.Ctel = Book.Ctel");
                        break;
                    case "2":
                        result = db.Book.SqlQuery("SELECT * From Book WHERE Ctel LIKE '%" + searchString + "%'");
                        break;
                }
            }
            else {
                if (!String.IsNullOrEmpty(checkinTimeString) && String.IsNullOrEmpty(checkoutTimeString))
                {
                    page = 1;
                    checkinTimeString = currentFilter;
                    result = db.Book.SqlQuery("SELECT * From Book WHERE CheckinTime between '" + checkinTimeString + " 00:00:00' AND '" + checkinTimeString + " 23:59:59'");
                }
                else if (String.IsNullOrEmpty(checkinTimeString) && !String.IsNullOrEmpty(checkoutTimeString))
                {
                    page = 1;
                    checkoutTimeString = currentFilter;
                    result = db.Book.SqlQuery("SELECT * From Book WHERE CheckoutTime between '" + checkoutTimeString + " 00:00:00' AND '" + checkoutTimeString + " 23:59:59'");
                }
                else if (!String.IsNullOrEmpty(checkinTimeString) && !String.IsNullOrEmpty(checkoutTimeString))
                {
                    page = 1;
                    checkinTimeString = currentFilter;
                    result = db.Book.SqlQuery("SELECT * From Book WHERE CheckinTime >= '" + checkinTimeString + " 00:00:00' AND CheckoutTime <= '" + checkoutTimeString + " 23:59:59'");
                }
            }
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));

        }

        //public ActionResult Index(string currentFilter,int? page, string checkinTimeString, string checkoutTimeString)
        //{
        //    ViewBag.CurrentFilter = checkinTimeString;

        //    var result = db.Book.SqlQuery("SELECT * From Book");

        //    if (!String.IsNullOrEmpty(checkinTimeString)&& String.IsNullOrEmpty(checkoutTimeString))
        //    {
        //        page = 1;
        //        checkinTimeString = currentFilter;
        //        result = db.Book.SqlQuery("SELECT * From Book WHERE Convert(varchar, CheckinTime, 120) LIKE '" + checkinTimeString + "%'");
        //    }
        //    else if (String.IsNullOrEmpty(checkinTimeString) && !String.IsNullOrEmpty(checkoutTimeString))
        //    {
        //        page = 1;
        //        checkoutTimeString = currentFilter;
        //        result = db.Book.SqlQuery("SELECT * From Book WHERE Convert(varchar, CheckoutTime, 120) LIKE '" + checkoutTimeString + "%'");
        //    }
        //    else if (String.IsNullOrEmpty(checkinTimeString) && String.IsNullOrEmpty(checkoutTimeString))
        //    {
        //        page = 1;
        //        checkinTimeString = currentFilter;
        //        result = db.Book.SqlQuery("SELECT * From Book WHERE CheckinTime >= '" + checkinTimeString + " 00:00:00' AND CheckoutTime <= '"+ checkoutTimeString+" 00:00:00'");
        //    }
        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);
        //    return View(result.ToPagedList(pageNumber, pageSize));
        //}


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
            ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname");
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype");
            return View();
        }

        // POST: Books/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckinTime,Ctel,CheckoutTime,Bid,RtypeID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname", book.Ctel);
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return View(book);
        }

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
            //ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname", book.Ctel);
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return View(book);
        }

        // POST: Books/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckinTime,Ctel,CheckoutTime,Bid,RtypeID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Ctel = new SelectList(db.Customer, "Ctel", "Cname", book.Ctel);
            ViewBag.RtypeID = new SelectList(db.RoomInfo, "RtypeID", "Rtype", book.RtypeID);
            return View(book);
        }
        public ActionResult Delete(int? id)
        {
            Book book = db.Book.Find(id);
            db.Book.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //// GET: Books/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Book book = db.Book.Find(id);
        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(book);
        //}

        //// POST: Books/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Book book = db.Book.Find(id);
        //    db.Book.Remove(book);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult MultDelete(FormCollection collection)
        {
            string str = collection["checkitem"];// checkitem复选框的名
            string[] strDelete = str.Split(',');
            foreach (var i in strDelete)
            {
                if (i != "false")
                {
                    Book book = db.Book.Find(int.Parse(i));
                    db.Book.Remove(book);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult CheckCtel(string Ctel)
        {
            bool result = true;

                var query = from c in db.Customer//用linq语句 查询实体模型类中T_UserInfo表  
                            where c.Ctel == Ctel
                            select new { ctel = c.Ctel };
                if (query == null || !query.Any())
                {
                    result = false;
                }
                
            return Json(result, JsonRequestBehavior.AllowGet);
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
