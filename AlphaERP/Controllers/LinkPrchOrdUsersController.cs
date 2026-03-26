using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class LinkPrchOrdUsersController : controller
    {
        // GET: LinkPrchOrdUsers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPurchOrd(int PurchOrd)
        {
            if (PurchOrd == 0)
            {
                return PartialView("LinkPrchOrdUsersRequest");
            }
            else
            {
                return PartialView("LinkPrchOrdUsersItems");
            }
        }

        public JsonResult SaveLinkPrchOrdUsersRequest(List<OrdRequestDF> OrdReqDF)
        {
            foreach (OrdRequestDF item in OrdReqDF)
            {
              List<Ord_RequestDF> OrdDf = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo).ToList();
                List<Ord_LinkPrchOrdUsers> delex = db.Ord_LinkPrchOrdUsers.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo).ToList();
                if(delex != null)
                {
                    db.Ord_LinkPrchOrdUsers.RemoveRange(delex);
                    db.SaveChanges();
                }
                foreach (Ord_RequestDF items in OrdDf)
                {
                    Ord_LinkPrchOrdUsers ex = new Ord_LinkPrchOrdUsers();
                    ex.CompNo = company.comp_num;
                    ex.ReqYear = item.ReqYear;
                    ex.ReqNo = item.ReqNo;
                    ex.ItemNo = items.SubItemNo;
                    ex.UserId = item.UserID;
                    db.Ord_LinkPrchOrdUsers.Add(ex);
                    db.SaveChanges();
                }
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveLinkPrchOrdUsersItems(List<OrdRequestDF> OrdReqDF)
        {
            foreach (OrdRequestDF item in OrdReqDF)
            {
                Ord_LinkPrchOrdUsers ex = db.Ord_LinkPrchOrdUsers.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.ItemNo == item.SubItemNo).FirstOrDefault();
                if (ex != null)
                {
                    ex.UserId = item.UserID;
                    db.SaveChanges();
                }
                else
                {
                    Ord_LinkPrchOrdUsers ex1 = new Ord_LinkPrchOrdUsers();
                    ex1.CompNo = company.comp_num;
                    ex1.ReqYear = item.ReqYear;
                    ex1.ReqNo = item.ReqNo;
                    ex1.ItemNo = item.SubItemNo;
                    ex1.UserId = item.UserID;
                    db.Ord_LinkPrchOrdUsers.Add(ex1);
                    db.SaveChanges();
                }
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}