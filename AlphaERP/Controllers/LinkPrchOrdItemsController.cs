using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class LinkPrchOrdItemsController : controller
    {
        // GET: LinkPrchOrdItems
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PurchaseRequestItems(int ReqYear,string ReqNo)
        {
            List<Ord_RequestDF> lOrdRequestDF = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo).ToList();
            return PartialView(lOrdRequestDF);
        }
        public JsonResult EditItemsPurchaseRequest(List<Ord_RequestDF> OrdReqDF)
        {
            foreach (Ord_RequestDF item in OrdReqDF)
            {
                Ord_RequestDF ex = db.Ord_RequestDF.Where(x =>
            x.CompNo == company.comp_num && x.ReqYear == item.ReqYear
            && x.ReqNo == item.ReqNo && x.ItemSr == item.ItemSr && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (ex != null)
                {
                    ex.SubItemNo = item.SubItemNo;
                    ex.SubTUnit = item.SubTUnit;
                    ex.SubUnitSerial = item.SubUnitSerial;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException exp)
                    {
                        foreach (var entityValidationErrors in exp.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                Console.WriteLine("Property: " + validationError.PropertyName);
                                Console.WriteLine("Error: " + validationError.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}