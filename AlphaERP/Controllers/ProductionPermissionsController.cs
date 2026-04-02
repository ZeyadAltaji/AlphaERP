using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ProductionPermissionsController : controller
    {
        // GET: ProductionPermissions
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPermissions(string UserID)
        {
            var company = (Company)Session["company"];
            var permissions = db.ProductionOrdersPermissions.Where(x => x.UserID == UserID && x.CompNo == company.comp_num).ToList();
            return Json(new { permissions = permissions }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetPermissions(string UserID, List<ProductionOrdersPermission> permissions)
        {
            var company = (Company)Session["company"];
            var oldPermissions = db.ProductionOrdersPermissions.Where(x => x.UserID == UserID && x.CompNo == company.comp_num).ToList();
            db.ProductionOrdersPermissions.RemoveRange(oldPermissions);
            if (permissions != null)
            {
                db.ProductionOrdersPermissions.AddRange(permissions);
            }
            db.SaveChanges();
            
            if (UserID == me.UserID)
            {
                var prodPerms = new MDB().ProductionOrdersPermissions.Where(x => x.UserID == me.UserID && x.CompNo == company.comp_num).ToList();
                Session["ProdOrdersPermissions"] = prodPerms;
            }
            
            return Json(new { ok = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}
