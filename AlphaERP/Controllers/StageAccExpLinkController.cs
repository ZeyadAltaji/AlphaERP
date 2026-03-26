using AlphaERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class StageAccExpLinkController : controller
    {
        // GET: StageAccExpLink
        public ActionResult Index()
        {
            List<ProdCost_StageAccExpLink> StageAccExpLink = db.ProdCost_StageAccExpLink.Where(x => x.CompNo == company.comp_num).OrderByDescending(o => o.StageCode).ToList();
            return View(StageAccExpLink);
        }
        public ActionResult Account(int DeptId)
        {
            List<Acc> BU = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, glactmf.acc_num as AccId, glactmf.acc_desc as AccDescAr, glactmf.acc_edesc as AccDescEn " +
                                                       "FROM glactmf INNER JOIN GLCRBMF ON glactmf.acc_comp = GLCRBMF.CRB_COMP AND glactmf.acc_num = GLCRBMF.crb_acc " +
                                                        "WHERE(GLCRBMF.crb_dep = '{0}') AND (glactmf.acc_comp = '{1}')", DeptId, company.comp_num)).ToList();

            return PartialView(BU);
        }
        public ActionResult StageAccExpLinkList()
        {
            List<ProdCost_StageAccExpLink> StageAccExpLink = db.ProdCost_StageAccExpLink.Where(x => x.CompNo == company.comp_num).OrderByDescending(o => o.StageCode).ToList();
            return PartialView(StageAccExpLink);
        }
        public ActionResult eStageAccExpLink(int stagecode, int DeptId, long AccNo)
        {
            List<ProdCost_StageAccExpLink> StageAccExpLink = db.ProdCost_StageAccExpLink.Where(x => x.CompNo == company.comp_num
         && x.StageCode == stagecode && x.CloseDept == DeptId && x.CloseAcc == AccNo).ToList();

            return PartialView(StageAccExpLink);
        }
        public JsonResult CheckIsExixts(int Stagecode,int Deptcode,long Acccode)
        {
            List<ProdCost_StageAccExpLink> StageAccExpLink = db.ProdCost_StageAccExpLink.Where(x => x.CompNo == company.comp_num
            && x.StageCode == Stagecode && x.CloseDept == Deptcode && x.CloseAcc == Acccode).ToList();

            if(StageAccExpLink.Count != 0)
            {
                return Json(new { d = "IsExixts" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { d = "NotIsExixts" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Save_StageAccExpLink(List<ProdCost_StageAccExpLink> StageAccExpLink)
        {
            db.ProdCost_StageAccExpLink.AddRange(StageAccExpLink);
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_StageAccExpLink(List<ProdCost_StageAccExpLink> StageAccExpLink)
        {
            ProdCost_StageAccExpLink ProdCost_Stage = StageAccExpLink.FirstOrDefault();
            List<ProdCost_StageAccExpLink> ex = db.ProdCost_StageAccExpLink.Where(x =>
            x.CompNo == ProdCost_Stage.CompNo && x.StageCode == ProdCost_Stage.StageCode
            && x.CloseDept == ProdCost_Stage.CloseDept && x.CloseAcc == ProdCost_Stage.CloseAcc).ToList();
            if(ex.Count != 0)
            {
                db.ProdCost_StageAccExpLink.RemoveRange(ex);
                db.SaveChanges();
            }
         
            db.ProdCost_StageAccExpLink.AddRange(StageAccExpLink);
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete_StageAccExpLink(int stagecode, int DeptId, long AccNo)
        {
            List<ProdCost_StageAccExpLink> ex = db.ProdCost_StageAccExpLink.Where(x =>
            x.CompNo == company.comp_num && x.StageCode == stagecode
            && x.CloseDept == DeptId && x.CloseAcc == AccNo).ToList();
            if (ex.Count != 0)
            {
                db.ProdCost_StageAccExpLink.RemoveRange(ex);
                db.SaveChanges();
            }
            
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
} 