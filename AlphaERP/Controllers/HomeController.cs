using AlphaERP.Filter;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AlphaERP.Controllers
{
    public class HomeController : controller
    {

        private class MenuItem
        {
            public int prog { get; set; }
            public int count { get; set; }

        }

        [HttpPost]
        public ActionResult ChangePassword(string old, string np, string npr)
        {
            if (old != me.UserPWD)
            {
                return Json(new { error = Resources.Resource.CurrentPasswordWrong }, JsonRequestBehavior.AllowGet);
            }
            if (np != npr)
            {
                return Json(new { error = Resources.Resource.PasswordsNotMatch }, JsonRequestBehavior.AllowGet);
            }

            User u = db.Users.Where(x => x.UserID == me.UserID).FirstOrDefault();
            u.UserPWD = np;

            db.SaveChanges();
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizationFilter]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Index()
        {





            //string PrivateKey = System.Configuration.ConfigurationManager.AppSettings.Get("PrivateKey");
            //string MacAddress = db.Database.SqlQuery<string>("declare @t table (i uniqueidentifier default newsequentialid(), m as cast(i as char(36))) insert into @t default values;select substring(m,25,2) + '-' + substring(m,27,2) + '-' + substring(m,29,2) + '-' + substring(m,31,2) + '-' + substring(m,33,2) + '-' + substring(m,35,2) as UserName FROM @t").FirstOrDefault();
            //string hashedData = ComputeSha256Hash(MacAddress);
            //if (hashedData == PrivateKey)
            //{


            return View();
            //}
            //else
            //{
            //    Session.Clear();
            //    return View("~/Views/Account/Illegal.cshtml");
            //}


        }

        public ActionResult Style()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetTheme(string theme)
        {
            HttpCookie Theme = new HttpCookie("Theme");
            Theme["Theme"] = theme;
            Session["Theme"] = theme;
            Theme.Expires = DateTime.Now.AddYears(10);
            Response.Cookies.Add(Theme);
            return Json("Ok");
        }

        [HttpGet]
        public JsonResult GetNotifications()
        {

            return Json(new
            {
                FeedBackList = 0
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Badges()
        {
            List<MenuItem> l = new List<MenuItem>();



            /*int CompaniesCount = db.Companies.Count();
            int DiffCount = db.Company_FinParametersDiff.Count();
            int PeriodsCount = db.Fin_FinancialPeriods.Where(x => x.ComID == Comp.ComID).Count();
            int SegmentsCount = db.Fin_FinancialSegments.Where(x => x.ComID == Comp.ComID).Count();
            InsertMenuItem(11210, CompaniesCount, l);
            InsertMenuItem(11204, DiffCount, l);
            InsertMenuItem(11211, PeriodsCount, l);
            InsertMenuItem(11205, SegmentsCount, l);*/
            return Json(l = l, JsonRequestBehavior.AllowGet);
        }

        private void InsertMenuItem(int Prog, int count, List<MenuItem> l)
        {
            MenuItem l1 = new MenuItem
            {
                prog = Prog,
                count = count
            };
            l.Add(l1);
        }

        [HttpGet]
        public ActionResult TotalNotifications()
        {
            return View();
        }




    }

}
