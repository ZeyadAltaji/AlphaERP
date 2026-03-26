using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class PortInfoController : controller
    {
        // GET: PortInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PortInfoList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult ChoosePortInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;

            return PartialView();
        }
        public ActionResult DetailsPortInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;

            return PartialView();
        }
        public ActionResult GetPortInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();

            Ord_PortInfoHF ex1 = db.Ord_PortInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer).FirstOrDefault();

            if (ex1 == null)
            {
                return View("AddPortInfo");
            }
            else
            {
                return View("ePortInfo");
            }
        }
        public ActionResult ViewPortInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return View();
        }

        public JsonResult Save_PortInfo(Ord_PortInfoHF PortInfoHF, List<Ord_PortInfoDF> PortInfoDF)
        {
            Ord_PortInfoHF ex1 = db.Ord_PortInfoHF.Where(x => x.CompNo == PortInfoHF.CompNo && x.OrderYear == PortInfoHF.OrderYear
            && x.OrderNo == PortInfoHF.OrderNo && x.TawreedNo == PortInfoHF.TawreedNo && x.ShipSer == PortInfoHF.ShipSer).FirstOrDefault();

            if (ex1 == null)
            {
                Ord_PortInfoHF ex = new Ord_PortInfoHF();
                ex.CompNo = PortInfoHF.CompNo;
                ex.OrderYear = PortInfoHF.OrderYear;
                ex.OrderNo = PortInfoHF.OrderNo;
                ex.TawreedNo = PortInfoHF.TawreedNo;
                ex.ShipSer = PortInfoHF.ShipSer;
                ex.VendorNo = PortInfoHF.VendorNo;
                ex.ArrivalDate = PortInfoHF.ArrivalDate;
                ex.PortNotes = PortInfoHF.PortNotes;
                ex.IsApproval = false;
                db.Ord_PortInfoHF.Add(ex);
                db.SaveChanges();
            }
            else
            {
                ex1.ArrivalDate = PortInfoHF.ArrivalDate;
                ex1.PortNotes = PortInfoHF.PortNotes;
                ex1.IsApproval = false;
                db.SaveChanges();
            }

            foreach (Ord_PortInfoDF item in PortInfoDF)
            {
                Ord_PortInfoDF ex = db.Ord_PortInfoDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == item.OrderYear
            && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (ex == null)
                {
                    Ord_PortInfoDF ex2 = new Ord_PortInfoDF();
                    ex2.CompNo = item.CompNo;
                    ex2.OrderYear = item.OrderYear;
                    ex2.OrderNo = item.OrderNo;
                    ex2.TawreedNo = item.TawreedNo;
                    ex2.ShipSer = item.ShipSer;
                    ex2.ItemNo = item.ItemNo;
                    ex2.ShippingQty = item.ShippingQty;
                    ex2.ShippingTUnit = item.ShippingTUnit;
                    ex2.ShippingUnitSerial = item.ShippingUnitSerial;
                    ex2.ShippingQty2 = item.ShippingQty2;
                    ex2.PortQty = item.PortQty;
                    ex2.PortTUnit = item.PortTUnit;
                    ex2.PortUnitSerial = item.PortUnitSerial;
                    ex2.PortQty2 = item.PortQty2;
                    db.Ord_PortInfoDF.Add(ex2);
                    db.SaveChanges();
                }
                else
                {
                    ex.ShippingQty = item.ShippingQty;
                    ex.ShippingTUnit = item.ShippingTUnit;
                    ex.ShippingUnitSerial = item.ShippingUnitSerial;
                    ex.ShippingQty2 = item.ShippingQty2;
                    ex.PortQty = item.PortQty;
                    ex.PortTUnit = item.PortTUnit;
                    ex.PortUnitSerial = item.PortUnitSerial;
                    ex.PortQty2 = item.PortQty2;
                    db.SaveChanges();
                }
            }

            return Json(new { TawreedNo = PortInfoHF.TawreedNo, OrdNo = PortInfoHF.OrderNo, OrdYear = PortInfoHF.OrderYear, ShipSer = PortInfoHF.ShipSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovalPortInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            Ord_PortInfoHF ex1 = db.Ord_PortInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer).FirstOrDefault();

            if (ex1 != null)
            {
                ex1.IsApproval = true;
                db.SaveChanges();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}