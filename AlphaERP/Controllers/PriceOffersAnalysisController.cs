using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class PriceOffersAnalysisController : controller
    {
        // GET: PriceOffersAnalysis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PriceOffersAnalysisList()
        {
            return PartialView();
        }
        public ActionResult ChoosePriceOffersAnalysis(int ReqforQuotyear, int ReqforQuotNo)
        {
            ViewBag.ReqforQuotyear = ReqforQuotyear;
            ViewBag.ReqforQuotNo = ReqforQuotNo;
            return PartialView();
        }
        
        public ActionResult DataBestOffer(int ReqforQuotyear, int ReqforQuotNo, int BestOffer)
        {
            List<MRP_Web_OrdCopyInfo> OrdCopy = new List<MRP_Web_OrdCopyInfo>();
            if (BestOffer == 1)
            {
                OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo && x.Qty.Value > 0).OrderByDescending(o => o.Qty).ToList();
            }
            if (BestOffer == 2)
            {
                OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo && x.Qty.Value > 0).OrderBy(o => o.SellPrice).ToList();
            }
            if (BestOffer == 3)
            {
                OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo && x.Qty.Value > 0).OrderBy(o => o.DeliveryDate).ToList();
            }
            return PartialView(OrdCopy);
        }
        public JsonResult NominationBestOffer(int ReqforQuotyear, int ReqforQuotNo,long VendorNo)
        {

            List<MRP_Web_OrdCopyInfo> lord = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear
             && x.ReqforQuotNo == ReqforQuotNo).ToList();
            foreach (MRP_Web_OrdCopyInfo items in lord)
            {
                MRP_Web_OrdCopyInfo ex = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == items.CompNo && x.ReqforQuotyear == items.ReqforQuotyear
                 && x.ReqforQuotNo == items.ReqforQuotNo && x.VendorNo == items.VendorNo && x.ItemNo == items.ItemNo).FirstOrDefault();

                ex.bPriceOffersAnalysis = true;

                if (ex.VendorNo == VendorNo)
                {
                    ex.bNominationBestOffer = true;
                }
                else
                {
                    ex.bNominationBestOffer = false;
                }
                db.SaveChanges();
            }
            
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}