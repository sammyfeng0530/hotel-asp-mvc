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
    public class UserCustomersController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();

        // GET: Customers
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                Customer customer = db.Customer.Find(Session["UserID"]);
                return View(customer);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
            //if (Session["UserID"] != null)
            //{
            //    Customer customer = db.Customer.Find(Session["UserID"]);
            //    return View(customer);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cname,Csex,Ctel,Cpwd,Cpic")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Response.Write("<script>alert('注册失败')</script>");
            return RedirectToAction("Index","Home");
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cname,Csex,Ctel,Cpwd,Cpic")] Customer customer)
        {
            //if (ModelState.IsValid)
            //{
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            //}
            //return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public PartialViewResult _Editpart(string id)
        {
            Customer customer = db.Customer.Find(Session["UserID"]);

            return PartialView("_Editpart", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Editpart(Customer customer)
        {
            //if (ModelState.IsValid)
            //{
            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            //}
            //return View(admin);
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
