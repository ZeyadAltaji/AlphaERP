using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class LinkPurchOrdersAccController : controller
    {
        // GET: LinkPurchOrdersAcc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ePurchaseRequest(int ReqYear,string ReqNo)
        {
            ViewBag.ReqYear = ReqYear;
            ViewBag.ReqNo = ReqNo;
            return PartialView();
        }
        public ActionResult Account(int DeptId)
        {
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, glactmf.acc_num as AccId, glactmf.acc_desc as AccDescAr, glactmf.acc_edesc as AccDescEn " +
                                                       "FROM glactmf INNER JOIN GLCRBMF ON glactmf.acc_comp = GLCRBMF.CRB_COMP AND glactmf.acc_num = GLCRBMF.crb_acc " +
                                                        "WHERE(GLCRBMF.crb_dep = '{0}') AND (glactmf.acc_comp = '{1}') AND (glactmf.acc_report = 2)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public ActionResult PurchaseRequestList()
        {
            return PartialView();
        }
        public JsonResult EditPurchaseRequest(int ReqYear,string ReqNo , int DeptNo, long AccNo)
        {
            Ord_RequestHF ex = db.Ord_RequestHF.Where(x =>
            x.CompNo == company.comp_num && x.ReqYear == ReqYear
            && x.ReqNo == ReqNo).FirstOrDefault();
            if (ex != null)
            {
                ex.DeptNo = DeptNo;
                ex.AccNo = AccNo;
                db.SaveChanges();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}