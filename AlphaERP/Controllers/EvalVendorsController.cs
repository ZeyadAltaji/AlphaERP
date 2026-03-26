using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class EvalVendorsController : controller
    {
        // GET: EvalVendors
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EvalVendorsList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult AddEvalVendors()
        {
            return View();
        }
        public ActionResult EditEvalVendors(int OrdYear, int OrderNo, string TawreedNo, long EvalVendId)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.EvalVendId = EvalVendId;

            return View();
        }
        public ActionResult ViewEvalVendors(int OrdYear, int OrderNo, string TawreedNo, long EvalVendId)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.EvalVendId = EvalVendId;

            return View();
        }
        public ActionResult PR(int OrdYear)
        {
            List<PROrder> PR = new MDB().Database.SqlQuery<PROrder>(string.Format("SELECT dbo.OrdPreOrderHF.OrderNo,  isnull(dbo.OrderHF.TawreedNo,'*')as TawreedNo,dbo.OrdPreOrderHF.OrderDate,dbo.OrdReqPO.PDesc as BuyerName , dbo.OrdReqPO.EngDesc AS BuyerName_Eng, " +
                "case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng," +
                "Vendors.VendorNo,isnull(dbo.Vendors.name,'*')as VenName,isnull(dbo.Vendors.Eng_name,'*')as VenName_Eng  " +
               "FROM dbo.OrdPreOrderHF INNER JOIN dbo.COMPANY ON dbo.OrdPreOrderHF.CompNo = dbo.COMPANY.comp_num INNER JOIN dbo.OrdPurchPurpose ON dbo.OrdPreOrderHF.CompNo = dbo.OrdPurchPurpose.CompNo AND dbo.OrdPreOrderHF.OrderPerpose = dbo.OrdPurchPurpose.PCode INNER JOIN dbo.OrdReqPO ON dbo.OrdPreOrderHF.CompNo = dbo.OrdReqPO.CompNo AND dbo.OrdPreOrderHF.OrdOrg = dbo.OrdReqPO.PCode  " +
               "LEFT OUTER JOIN dbo.OrderHF ON dbo.OrdPreOrderHF.CompNo = dbo.OrderHF.CompNo AND dbo.OrdPreOrderHF.[Year] = dbo.OrderHF.OrdYear AND dbo.OrdPreOrderHF.OrderNo = dbo.OrderHF.OrderNo LEFT  JOIN dbo.Vendors ON dbo.OrderHF.VendorNo = dbo.Vendors.VendorNo AND dbo.OrderHF.CompNo = dbo.Vendors.comp  " +
               "WHERE(OrdPreOrderHF.Confirmation = 1) AND (dbo.OrdPreOrderHF.CompNo = '{0}') AND (dbo.OrdPreOrderHF.[Year] = '{1}') AND (Vendors.Evaluated = 1)",  company.comp_num, OrdYear)).ToList();

            return PartialView(PR);
        }
        public JsonResult Save_EvalVendors(Ord_Web_EvalVenodrsHF EvalVenodrsHF, List<Ord_Web_EvalVenodrsDF> EvalVenodrsDF)
        {
            long EvalVendId = db.Ord_Web_EvalVenodrsHF.Where(x => x.CompNo == company.comp_num).OrderByDescending(o => o.EvalVendId).Select(z => z.EvalVendId).FirstOrDefault();
            EvalVendId = EvalVendId + 1;

            Ord_Web_EvalVenodrsHF ex = new Ord_Web_EvalVenodrsHF();
            ex.CompNo = EvalVenodrsHF.CompNo;
            ex.Orderyear = EvalVenodrsHF.Orderyear;
            ex.OrderNo = EvalVenodrsHF.OrderNo;
            ex.TawreedNo = EvalVenodrsHF.TawreedNo;
            ex.EvalVendId = EvalVendId;
            ex.VendorNo = EvalVenodrsHF.VendorNo;
            ex.doc = EvalVenodrsHF.doc;
            ex.recdate = EvalVenodrsHF.recdate;
            ex.realdate = EvalVenodrsHF.realdate;
            ex.catdate = EvalVenodrsHF.catdate;
            db.Ord_Web_EvalVenodrsHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_Web_EvalVenodrsDF item in EvalVenodrsDF)
            {
                Ord_Web_EvalVenodrsDF ex2 = new Ord_Web_EvalVenodrsDF();
                ex2.CompNo = item.CompNo;
                ex2.EvalVendId = EvalVendId;
                ex2.CodeId = item.CodeId;
                ex2.Marks = item.Marks;
                db.Ord_Web_EvalVenodrsDF.Add(ex2);
                db.SaveChanges();
            }

            return Json(new {  Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Edit_EvalVendors(Ord_Web_EvalVenodrsHF EvalVenodrsHF, List<Ord_Web_EvalVenodrsDF> EvalVenodrsDF)
        {
            Ord_Web_EvalVenodrsHF ex = db.Ord_Web_EvalVenodrsHF.Where(x => x.CompNo == company.comp_num && x.Orderyear == EvalVenodrsHF.Orderyear && x.OrderNo == EvalVenodrsHF.OrderNo
            && x.TawreedNo == EvalVenodrsHF.TawreedNo && x.EvalVendId == EvalVenodrsHF.EvalVendId).FirstOrDefault();

            ex.doc = EvalVenodrsHF.doc;
            ex.recdate = EvalVenodrsHF.recdate;
            ex.realdate = EvalVenodrsHF.realdate;
            ex.catdate = EvalVenodrsHF.catdate;
            db.SaveChanges();

            foreach (Ord_Web_EvalVenodrsDF item in EvalVenodrsDF)
            {
                Ord_Web_EvalVenodrsDF exdet = db.Ord_Web_EvalVenodrsDF.Where(x => x.CompNo == company.comp_num  && x.EvalVendId == EvalVenodrsHF.EvalVendId
                && x.CodeId == item.CodeId).FirstOrDefault();
                if(exdet != null)
                {
                    exdet.Marks = item.Marks;
                    db.SaveChanges();
                }
                else
                {
                    Ord_Web_EvalVenodrsDF ex2 = new Ord_Web_EvalVenodrsDF();
                    ex2.CompNo = item.CompNo;
                    ex2.EvalVendId = item.EvalVendId;
                    ex2.CodeId = item.CodeId;
                    ex2.Marks = item.Marks;
                    db.Ord_Web_EvalVenodrsDF.Add(ex2);
                    db.SaveChanges();
                }
            }

            return Json(new { TawreedNo = EvalVenodrsHF.TawreedNo, OrdNo = EvalVenodrsHF.OrderNo, OrdYear = EvalVenodrsHF.Orderyear, EvalVendId = EvalVenodrsHF.EvalVendId, Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Del_EvalVendors(int OrdYear, int OrderNo, string TawreedNo, long EvalVendId)
        {
            List<Ord_Web_EvalVenodrsDF> ex1 = db.Ord_Web_EvalVenodrsDF.Where(x => x.CompNo == company.comp_num && x.EvalVendId == EvalVendId).ToList();

            db.Ord_Web_EvalVenodrsDF.RemoveRange(ex1);
            db.SaveChanges();

            Ord_Web_EvalVenodrsHF ex = db.Ord_Web_EvalVenodrsHF.Where(x => x.CompNo == company.comp_num && x.Orderyear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.EvalVendId == EvalVendId).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_Web_EvalVenodrsHF.Remove(ex);
                db.SaveChanges();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

    }
}