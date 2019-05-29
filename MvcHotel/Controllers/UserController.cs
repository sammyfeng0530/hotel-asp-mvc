using MvcHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcHotel.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private JustHotelPluscEntities db = new JustHotelPluscEntities();
        // GET: User
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return Content("<script>alert('登录失败');history.go(-1);</script>");
            }
        }

        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer user)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Customer.Where(a => a.Ctel.Equals(user.Ctel) && a.Cpwd.Equals(user.Cpwd)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserID"] = obj.Ctel.ToString();
                    Session["UserName"] = obj.Cname.ToString();
                    ViewBag.username = obj.Cname.ToString();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login data is incorrect!");
            }
            return Content("<script>alert('登录失败，请检查用户名或密码');history.go(-1);</script>"); 
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult _RoomInfo()
        {
            return PartialView(db.RoomTypeInfo.ToList());
        }
    }
}