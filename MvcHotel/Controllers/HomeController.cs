using MvcHotel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHotel.Controllers
{
    public class HomeController : Controller
    {
        private JustHotelPluscEntities db = new JustHotelPluscEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult About()
        {
            //if (Request["findkey"] != null)
            //{
            //    string findkey = Request["findkey"];
            //    var room = db.RoomInfo.SqlQuery("SELECT * From RoomInfo WHERE Rtype Like '%" + findkey + "%'");
            //    return View(room);
            //}
            //else {
            //    return View(db.RoomInfo.ToList());
            //}
            return View();
        }
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
        public ActionResult _popup()
        {
            return PartialView();
        }
        public ActionResult CheckCtel(string Ctel)
        {
            bool result = false;

            var query = from c in db.Customer//用linq语句 查询实体模型类中T_UserInfo表  
                        where c.Ctel == Ctel
                        select new { ctel = c.Ctel };
            if (query == null || !query.Any())
            {
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(Customer user)
        //{

        //        var obj = db.Customer.Where(a => a.Ctel.Equals(user.Ctel) && a.Cpwd.Equals(user.Cpwd)).FirstOrDefault();
        //        if (obj != null)
        //        {
        //            Session["UserID"] = obj.Ctel.ToString();
        //            Session["UserName"] = obj.Cname.ToString();
        //            ViewBag.username = obj.Cname.ToString();
        //            return RedirectToAction("Index", "User");
        //        }


        //    return View(user);
        //}
    }
}