using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AlphaERP.Filter;
using AlphaERP.Models;

namespace AlphaERP.Controllers
{
    public class MenuController : controller
    {

        [AuthorizationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateOrder(List<Menu> ids)
        {
            if (ids != null)
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE AlphaERP_Menu");
                foreach (var item in ids)
                {
                    item.ModuleID = 10;
                }
                db.Menus.AddRange(ids);
                db.SaveChanges();
            }
            return null;
        }

        [HttpPost]
        public JsonResult DeleteMe(int ProgId)
        {
            Menu del = db.Menus.Where(x => x.ProgID == ProgId).FirstOrDefault();
            if (del != null)
            {
                db.Menus.Remove(del);
                db.SaveChanges();
            }
            List<UserPermission> ps = db.UserPermissions.Where(x => x.ProgID == ProgId).ToList();
            if (ps != null)
            {
                db.UserPermissions.RemoveRange(ps);
                db.SaveChanges();
            }
            return Json("Deleted");
        }

        [HttpPost]
        public JsonResult GetLastMENU()
        {
            int last = 10000;
            last *= 10000 + 1;
            Menu menu = db.Menus.OrderByDescending(x => x.ProgID).FirstOrDefault();
            if (menu != null)
            {
                last = menu.ProgID + 1;
            }
            return Json(new { last = last }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddToMenu(Menu Menu)
        {
            Menu.ModuleID = 10;
            if (Menu.ParentID == null) { Menu.ParentID = 0; }
            if (Menu.ParentID == 909710)
            {
                //    Menu.SourceForm = "1";
                Menu.ParentID = 0;
            }
            Menu.sort = "99999";
            Menu exmenu = db.Menus.Where(x => x.ProgID == Menu.ProgID).FirstOrDefault();
            if (exmenu != null)
            {
                db.Menus.Remove(exmenu);
                db.SaveChanges();
            }

            UserPermission exUsersPermissions = db.UserPermissions.Where(x => x.ProgID == Menu.ProgID && x.ModuleID == 10).FirstOrDefault();
            if (exUsersPermissions != null)
            {
                db.UserPermissions.Remove(exUsersPermissions);
                db.SaveChanges();
            }

            UserPermission p = new UserPermission
            {
                ProgID = Menu.ProgID,
                UserID = me.UserID,
                ProgAccess = true,
                ProgMod = true,
                ProgDel = true,
                ProgAdd = true,
                ModuleID = 10,
                CompNo = company.comp_num
            };
            db.Menus.Add(Menu);

            db.UserPermissions.Add(p);
            db.SaveChanges();
            User m = db.Users.Where(x => x.UserID == me.UserID).FirstOrDefault();
            Session["me"] = m;

            return Json("ok");
        }

        public JsonResult export()
        {
            DataTable r = _menu();
            DataTable p = _UsersPermissions();
            List<Menu> l = db.Menus.ToList();
            List<UserPermission> l2 = db.UserPermissions.ToList();
            foreach (Menu e in l)
            {
                r.Rows.Add(e.ProgID, e.ProgArName, e.ProgEnName, e.SourceForm, e.Icon, e.ParentID, e.sort);
            }
            r.WriteXml(Server.MapPath("/export/Menu.xml"));
            foreach (UserPermission e in l2)
            {
                p.Rows.Add(e.UserID, e.ProgID, e.ProgAccess, e.ProgAdd, e.ProgMod, e.ProgDel);
            }
            p.WriteXml(Server.MapPath("/export/UsersPermissions.xml"));





            /*
            var listOfFieldNames = typeof(Menu).GetProperties().Select(f => f.Name).ToList();

            foreach (var item in listOfFieldNames)
            {
               var ff = item.ToString();
            }
            */
















            return Json("ok");
        }

        public void exportmenu(List<Menu> menus)
        {
            Menu a1 = new Menu();
            Menu a2 = new Menu();
            DataTable d = new DataTable
            {
                TableName = "Menu"
            };
            List<PropertyInfo> g = typeof(Menu).GetProperties().ToList();
            foreach (PropertyInfo p in g)
            {
                //  a1.p = a2.p;
            }
        }

        private DataTable _menu()
        {
            DataTable d = new DataTable
            {
                TableName = "Menu"
            };
            d.Columns.Add("ProgID");
            d.Columns.Add("ProgArName");
            d.Columns.Add("ProgEnName");
            d.Columns.Add("SourceForm");
            d.Columns.Add("Icon");
            d.Columns.Add("ParentID");
            d.Columns.Add("sort");
            return d;
        }

        private DataTable _UsersPermissions()
        {
            DataTable d = new DataTable
            {
                TableName = "UsersPermissions"
            };
            d.Columns.Add("CompNo");
            d.Columns.Add("UserID");
            d.Columns.Add("ProgID");
            d.Columns.Add("ProgAccess");
            d.Columns.Add("ProgAdd");
            d.Columns.Add("ProgMod");
            d.Columns.Add("ProgDel");
            return d;
        }

        public ActionResult ReloadMenu()
        {
            return PartialView("~/Views/Shared/Menu.cshtml");
        }

    }
}