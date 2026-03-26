using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlphaERP.Models;

namespace AlphaERP.Controllers
{
    public class AnnualProductionPlanController : controller
    {
        public ActionResult Index()
        {
            YearPlanDetlsH h = db.YearPlanDetlsH.Where(x => x.PlanYear == DateTime.Now.Year).FirstOrDefault();
            if(h == null)
            {
                YearPlanDetlsH nh = new YearPlanDetlsH();
                nh.CompNo = company.comp_num;
                nh.Notes = "";
                nh.PlanDate = DateTime.Now;
                nh.PUpdate = false;
                nh.PlanYear = (short)DateTime.Now.Year;
                db.YearPlanDetlsH.Add(nh);
                db.SaveChanges();
                h = nh;
            }
            return View(h);
        }
        public ActionResult List(short Year)
        {
            YearPlanDetlsH h = db.YearPlanDetlsH.Where(x => x.PlanYear == Year).FirstOrDefault();
            if (h == null)
            {
                YearPlanDetlsH nh = new YearPlanDetlsH();
                nh.CompNo = company.comp_num;
                nh.Notes = "";
                nh.PlanDate = DateTime.Now;
                nh.PlanYear = Year;
                nh.PUpdate = false;
                db.YearPlanDetlsH.Add(nh);
                db.SaveChanges();
                h = nh;
            }
            return View(h);
        }
        public JsonResult Action(List<YearPlanDetlsD> Dts, short Year)
        {
            List<YearPlanDetlsD> exdts = db.YearPlanDetlsD.Where(x => x.PlanYear == Year).ToList();
            if(exdts != null)
            {
                db.YearPlanDetlsD.RemoveRange(exdts);
                db.SaveChanges();
            }

            if(Dts != null)
            {
               
                    foreach (var d in Dts)
                    {
                        d.CompNo = company.comp_num;
                        d.PlanYear = Year;
                    }
                    db.YearPlanDetlsD.AddRange(Dts);
                    db.SaveChanges();
                
            }
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }


    }
}