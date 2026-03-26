using AlphaERP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class SettingsController : controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult LogOut(string UserID, short CompNo)
        {
            OnlineUser del = db.OnlineUsers
                .Where(x => x.UserID == UserID && x.CompNo == CompNo).FirstOrDefault();
            db.OnlineUsers.Remove(del);
            db.SaveChanges();
            return Json(new { });
        }
        public JsonResult LoadOnline(string UserID)
        {
            List<OnlineUser> List = db.OnlineUsers.Where(x => x.UserID == UserID).ToList();
            return Json(new { List = List }, JsonRequestBehavior.AllowGet);
        }
    }
}