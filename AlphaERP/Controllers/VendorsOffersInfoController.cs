using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class VendorsOffersInfoController : controller
    {
        // GET: VendorsOffersInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VendorsOffersInfoList()
        {
            return PartialView();
        }

        public ActionResult EnterVendorOffer(int ReqforQuotyear, int ReqforQuotNo)
        {
            List<MRP_Web_OrdCopyInfo> OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo).ToList();
            return PartialView(OrdCopy);
        }
        public JsonResult Save_VendorsOffersInfo(List<MRP_Web_OrdCopyInfo> OrdCopyInfo)
        {
            foreach (MRP_Web_OrdCopyInfo item in OrdCopyInfo)
            {
                MRP_Web_OrdCopyInfo info = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotNo == item.ReqforQuotNo 
                && x.VendorNo == item.VendorNo && x.ItemNo == item.ItemNo).FirstOrDefault();

                info.Curr = item.Curr;
                info.Pmethod = item.Pmethod;
                info.DeliveryPlace = item.DeliveryPlace;
                info.CountryOfOrigin = item.CountryOfOrigin;
                info.ShippingPort = item.ShippingPort;
                info.Qty = item.Qty;
                info.Qty2 = item.Qty2;
                info.Bonus = item.Bonus;
                info.SellPrice = item.SellPrice;
                info.DeliveryDate = item.DeliveryDate;
                info.bVendorsOffers = item.bVendorsOffers;
                db.SaveChanges();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}