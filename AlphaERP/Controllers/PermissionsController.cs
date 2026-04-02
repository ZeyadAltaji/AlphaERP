using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlphaERP.Models;

namespace AlphaERP.Controllers
{
    public class PermissionsController : controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPermissions(string UserID)
        {
            List<UserPermission> permissions = db.UserPermissions.Where(x => x.CompNo == company.comp_num && x.ModuleID == 10 && x.UserID == UserID).ToList();
            return Json(new { permissions }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetPermissions(string UserID, List<UserPermission> permissions)
        {
            List<UserPermission> Mypermissions = db.UserPermissions.Where(x => x.CompNo == company.comp_num && x.ModuleID == 10 && x.UserID == UserID).ToList();
            if (Mypermissions != null)
            {
                db.UserPermissions.RemoveRange(Mypermissions);
                db.SaveChanges();
            }
            db.UserPermissions.AddRange(permissions);
            db.SaveChanges();
            if(UserID == me.UserID)
            {
                permissions = new MDB().UserPermissions.Where(x => x.UserID == me.UserID && x.CompNo == company.comp_num).ToList();
                Session["permissions"] = permissions;
            }
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}