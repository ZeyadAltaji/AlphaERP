using AlphaERP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ParametersController : controller
    {
        
        public ActionResult Index()
        {
            var parameters = db.ProdCost_Parameters
                .Where(x => x.CompNo == company.comp_num)
                .ToList();

            ViewBag.lParam = parameters;

            return View();
        }

        public ActionResult List()
        {
            var parametersHF = db.ProdCost_ParametersHF
                .Where(x => x.CompNo == company.comp_num)
                .ToList();

            return PartialView(parametersHF);
        }

        public ActionResult Details(short parmID)
        {
            ViewBag.ParmID = parmID;

            var details = db.ProdCost_Parameters
                .Where(x => x.CompNo == company.comp_num && x.ParmID == parmID)
                .ToList();

            return View(details);
        }

        public JsonResult AddNewDetail(ProdCost_Parameter detail)
        {
            if (detail.ParmCode == 0)
            {
                short nextParmCode = 1;

                var lastParameter = db.ProdCost_Parameters
                    .Where(x => x.CompNo == company.comp_num && x.ParmID == detail.ParmID)
                    .OrderByDescending(x => x.ParmCode)
                    .FirstOrDefault();

                if (lastParameter != null)
                    nextParmCode += lastParameter.ParmCode;

                detail.CompNo = company.comp_num;
                detail.ParmCode = nextParmCode;
                detail.Groups = detail.ParmID == 5 ? detail.Groups : (short?)0;

                db.ProdCost_Parameters.Add(detail);
                db.SaveChanges();
            }
            else
            {
                var existingParameter = db.ProdCost_Parameters
                    .FirstOrDefault(x => x.CompNo == company.comp_num &&
                                         x.ParmID == detail.ParmID &&
                                         x.ParmCode == detail.ParmCode);

                if (existingParameter != null)
                {
                    existingParameter.EngDesc = detail.EngDesc;
                    existingParameter.LocalDesc = detail.LocalDesc;
                    existingParameter.Groups = detail.ParmID == 5 ? detail.Groups : (short?)0;

                    db.SaveChanges();
                }
            }

            return Json(new { detail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditParam(ProdCost_Parameter detail)
        {
            var model = db.ProdCost_Parameters
                .FirstOrDefault(x => x.CompNo == company.comp_num &&
                                     x.ParmID == detail.ParmID &&
                                     x.ParmCode == detail.ParmCode);

            var parameters = db.ProdCost_Parameters
                .Where(x => x.CompNo == company.comp_num)
                .ToList();

            ViewBag.lParam = parameters;

            return PartialView(model);
        }

        public JsonResult DeleteDetail(ProdCost_Parameter detail)
        {
            var entity = db.ProdCost_Parameters
                .FirstOrDefault(x => x.ParmID == detail.ParmID && x.ParmCode == detail.ParmCode);

            if (entity != null)
            {
                db.ProdCost_Parameters.Remove(entity);
                db.SaveChanges();
            }

            return Json(new { ok = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
        }
    }

}