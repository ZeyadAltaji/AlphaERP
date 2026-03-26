using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class WeightChangeRateController : controller
    {
        // GET: WeightChangeRate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShipmentsList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult AddWeightChangeRate(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return PartialView();
        }
        public ActionResult EditWeightChangeRate(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int Ser)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.srl = Ser;
            return PartialView();
        }
        public ActionResult ViewWeightChangeRate(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int Ser)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.srl = Ser;
            return PartialView();
        }
        public ActionResult DetailsWeightChangeRate(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return PartialView();
        }

        public JsonResult Save_WeightChangeRate(Ord_WeightChangeRate WeightChangeRate)
        {

            Ord_WeightChangeRate ex = new Ord_WeightChangeRate();
            ex.CompNo = WeightChangeRate.CompNo;
            ex.OrderYear = WeightChangeRate.OrderYear;
            ex.OrderNo = WeightChangeRate.OrderNo;
            ex.TawreedNo = WeightChangeRate.TawreedNo;
            ex.ShipSer = WeightChangeRate.ShipSer;
            ex.Ser = WeightChangeRate.Ser;
            ex.WeightRateDate = WeightChangeRate.WeightRateDate;
            ex.WeightRate = WeightChangeRate.WeightRate;
            if (WeightChangeRate.WeightRateNote == null)
            {
                WeightChangeRate.WeightRateNote = "";
            }
            ex.WeightRateNote = WeightChangeRate.WeightRateNote;
            ex.StandardCost = WeightChangeRate.StandardCost;
            db.Ord_WeightChangeRate.Add(ex);
            db.SaveChanges();

            return Json(new { TawreedNo = WeightChangeRate.TawreedNo, OrdNo = WeightChangeRate.OrderNo, OrdYear = WeightChangeRate.OrderYear, ShipSer = WeightChangeRate.ShipSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_WeightChangeRate(Ord_WeightChangeRate WeightChangeRate)
        {
            Ord_WeightChangeRate ex = db.Ord_WeightChangeRate.Where(x => x.CompNo == company.comp_num && x.OrderYear == WeightChangeRate.OrderYear && x.OrderNo == WeightChangeRate.OrderNo && x.TawreedNo == WeightChangeRate.TawreedNo && x.ShipSer == WeightChangeRate.ShipSer && x.Ser == WeightChangeRate.Ser).FirstOrDefault();

            if(ex != null)
            {
                ex.WeightRateDate = WeightChangeRate.WeightRateDate;
                ex.WeightRate = WeightChangeRate.WeightRate;
                if (WeightChangeRate.WeightRateNote == null)
                {
                    WeightChangeRate.WeightRateNote = "";
                }
                ex.WeightRateNote = WeightChangeRate.WeightRateNote;
                ex.StandardCost = WeightChangeRate.StandardCost;

                db.SaveChanges();
            }
            
            return Json(new { TawreedNo = WeightChangeRate.TawreedNo, OrdNo = WeightChangeRate.OrderNo, OrdYear = WeightChangeRate.OrderYear, ShipSer = WeightChangeRate.ShipSer, Ser = WeightChangeRate.Ser, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Del_WeightChangeRate(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int Ser)
        {
            Ord_WeightChangeRate ex = db.Ord_WeightChangeRate.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.Ser == Ser).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_WeightChangeRate.Remove(ex);
                db.SaveChanges();
            }

            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, ShipSer = ShipSer, Ser = Ser, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}