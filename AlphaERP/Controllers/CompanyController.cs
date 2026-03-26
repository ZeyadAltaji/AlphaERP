using AlphaERP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace AlphaERP.Controllers
{
    public class CompanyController : controller
    {
        private List<Company> companies(string UserID)
        {
            if (Session["Companies"] == null)
            {
                List<Company> l = db.Companies.ToList();
                Session["Companies"] = l;
                return l;
            }
            else
            {
                List<Company> l = (List<Company>)Session["Companies"];
                return l;

            }
        }
        public ActionResult Index()
        {
            Session["CompError"] = "";
            List<Company> compan = companies(me.UserID);

            if (compan.Count == 0)
            {
                return RedirectToAction("NotAuthorizedForAllCompanies", "Error");
            }
            if (compan.Count == 1)
            {

                Company company = db.Companies.FirstOrDefault();
                if (company != null)
                {
                    List<UserPermission> permissions = new MDB().UserPermissions.Where(x => x.UserID == me.UserID && x.CompNo == company.comp_num && x.ProgAccess == true).ToList();
                    List<Ord_UsersPermissions> permiss = new MDB().Ord_UsersPermissions.Where(x => x.UserID == me.UserID && x.CompNo == company.comp_num && x.ProgAccess == true).ToList();

                    List<Ord_Programs> mm = permiss.Where(x => x.Ord_Programs != null).Select(x => x.Ord_Programs).ToList();
                    Session["company"] = company;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Index", compan);
        }
        [HttpPost]
        public ActionResult Index(short comp_num)
        {
            string SuperUser = System.Configuration.ConfigurationManager.AppSettings.Get("SuperUser");

            string IP = GetIpAddress();
            OnlineUser online = db.OnlineUsers
                               .Where(x => x.CompNo == comp_num && x.UserID == me.UserID)
                               .FirstOrDefault();
            //if (online != null)
            //{
            //    if (online.Ip != IP && SuperUser.ToLower() != me.UserID.ToLower())
            //    {
            //        Session["CompError"] = Resources.Resource.CompError;
            //        List<Company> compan = companies(me.UserID);

            //        if (compan.Count == 0)
            //        {
            //            return RedirectToAction("NotAuthorizedForAllCompanies", "Error");
            //        }
            //        return View("Index", compan);
            //    }
            //    db.OnlineUsers.Remove(online);
            //    db.SaveChanges();

            //}
            //OnlineUser onlineUser = new OnlineUser();
            //onlineUser.Ip = IP;
            //onlineUser.CompNo = comp_num;
            //onlineUser.Date = DateTime.Now;
            //onlineUser.UserID = me.UserID;
            //db.OnlineUsers.Add(onlineUser);
            //db.SaveChanges();

            List<UserPermission> permissions = new MDB().UserPermissions.Where(x => x.UserID == me.UserID && x.CompNo == comp_num && x.ProgAccess == true).ToList();

            Company company = db.Companies.Where(x => x.comp_num == comp_num).FirstOrDefault();
            if (company != null)
            {
                List<Ord_UsersPermissions> permiss = new MDB().Ord_UsersPermissions.Where(x => x.UserID == me.UserID && x.CompNo == comp_num && x.ProgAccess == true).ToList();

                List<Ord_Programs> mm = permiss.Where(x => x.Ord_Programs != null).Select(x => x.Ord_Programs).ToList();
                Session["company"] = company;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public JsonResult getSRC(short comp_num)
        {

            Company c = companies(me.UserID).Where(x => x.comp_num == comp_num).FirstOrDefault();
            string src = "data:image/png;base64,";
            if (c != null)
            {
                if (c.comp_logo != null)
                {
                    src += Convert.ToBase64String(c.comp_logo);
                }
                else
                {
                    src = "data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTYuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgMTA2LjA1OSAxMDYuMDU5IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCAxMDYuMDU5IDEwNi4wNTk7IiB4bWw6c3BhY2U9InByZXNlcnZlIj4KPGc+Cgk8cGF0aCBkPSJNOTAuNTQ0LDkwLjU0MmMyMC42ODctMjAuNjg1LDIwLjY4NS01NC4zNDIsMC4wMDItNzUuMDI0QzY5Ljg1OC01LjE3MiwzNi4xOTktNS4xNzIsMTUuNTE2LDE1LjUxMyAgIEMtNS4xNzMsMzYuMTk4LTUuMTcxLDY5Ljg1OCwxNS41MTgsOTAuNTQ3QzM2LjE5OSwxMTEuMjMsNjkuODU4LDExMS4yMyw5MC41NDQsOTAuNTQyeiBNMjEuMzAyLDIxLjMgICBjMTcuNDkzLTE3LjQ5Myw0NS45Ni0xNy40OTUsNjMuNDU3LDAuMDAyYzE3LjQ5NCwxNy40OTQsMTcuNDkyLDQ1Ljk2Mi0wLjAwMiw2My40NTVjLTE3LjQ5MywxNy40OTQtNDUuOTYyLDE3LjQ5Ni02My40NTUsMC4wMDIgICBDMy44MDQsNjcuMjYzLDMuODA3LDM4Ljc5NCwyMS4zMDIsMjEuM3ogTTMyLjk2OSw0MC40MzFsMS43NTktMS43NTlsLTEuNzYtMS43NjJjLTEuMjMzLTEuMjMyLTEuMjk2LTMuMjI5LTAuMTQzLTQuNTQ0ICAgYzAuMDI1LTAuMDM1LDAuMDc4LTAuMDk3LDAuMTM3LTAuMTU2YzEuMjc3LTEuMjc4LDMuNDYtMS4yNzEsNC43MTgtMC4wMTJsMS43NiwxLjc1OWwxLjc2LTEuNzYgICBjMS4xOTgtMS4xOTksMy4yNjctMS4yNjIsNC41NDQtMC4xNDVjMC4wNDEsMC4wMjksMC4xMDYsMC4wODYsMC4xNjcsMC4xNDZjMC42MjksMC42MjksMC45NzYsMS40NjUsMC45NzcsMi4zNTQgICBjMCwwLjg5LTAuMzQ2LDEuNzI3LTAuOTc2LDIuMzU1bC0xLjc2LDEuNzYxbDEuNzU4LDEuNzU5YzAuNjMsMC42MjksMC45NzcsMS40NjYsMC45NzcsMi4zNTRjMCwwLjg5MS0wLjM0NiwxLjcyOC0wLjk3NiwyLjM1NyAgIGMtMS4yNTgsMS4yNi0zLjQ1MiwxLjI2MS00LjcxMiwwbC0xLjc1OC0xLjc1OGwtMS43NTgsMS43NThjLTAuNjMxLDAuNjMtMS40NjcsMC45NzctMi4zNTYsMC45NzdjLTAuODksMC0xLjcyNy0wLjM0Ny0yLjM1Ni0wLjk3NyAgIEMzMS42NzEsNDMuODQ0LDMxLjY3MSw0MS43MywzMi45NjksNDAuNDMxeiBNNjEuMDg2LDQwLjQxNWwxLjc1OS0xLjc1OWwtMS43Ni0xLjc2MWMtMS4yMzItMS4yMzMtMS4yOTYtMy4yMjktMC4xNDMtNC41NDQgICBjMC4wMjQtMC4wMzUsMC4wNzgtMC4wOTcsMC4xMzctMC4xNTdjMS4yNzctMS4yNzcsMy40Ni0xLjI3LDQuNzE4LTAuMDExbDEuNzYsMS43NTlsMS43NjEtMS43NiAgIGMxLjE5Ny0xLjE5OSwzLjI2Ny0xLjI2Miw0LjU0NC0wLjE0NmMwLjA0MSwwLjAzLDAuMTA1LDAuMDg2LDAuMTY3LDAuMTQ2YzAuNjI5LDAuNjI5LDAuOTc2LDEuNDY1LDAuOTc3LDIuMzU0ICAgYzAsMC44OTEtMC4zNDYsMS43MjgtMC45NzYsMi4zNTZsLTEuNzYsMS43NmwxLjc1OCwxLjc1OWMwLjYzLDAuNjMsMC45NzcsMS40NjcsMC45NzcsMi4zNTVjMCwwLjg5LTAuMzQ2LDEuNzI3LTAuOTc2LDIuMzU2ICAgYy0xLjI1OCwxLjI2MS0zLjQ1MiwxLjI2Mi00LjcxMiwwbC0xLjc1OS0xLjc1OEw2NS44LDQ1LjEyNGMtMC42MzEsMC42MzEtMS40NjcsMC45NzgtMi4zNTUsMC45NzggICBjLTAuODkxLDAtMS43MjgtMC4zNDctMi4zNTYtMC45NzhDNTkuNzg3LDQzLjgyOSw1OS43ODcsNDEuNzE2LDYxLjA4Niw0MC40MTV6IE0zMC4xNzcsNzMuNjg1VjY5LjU4YzAtMC41NTIsMC40NDgtMSwxLTEgICBjMS4yNjgsMCwzLjgwOS0wLjk3Niw0LjkwNC0yLjUwNGMwLjE4OC0wLjI2MiwwLjQ5MS0wLjQxNywwLjgxMy0wLjQxN2MwLDAsMCwwLDAuMDAxLDBjMC4zMjMsMCwwLjYyNSwwLjE1NiwwLjgxMywwLjQxOSAgIGMxLjExOSwxLjU2NywyLjk0OSwyLjUwMyw0Ljg5NSwyLjUwM2MxLjk0OSwwLDMuNzc5LTAuOTM2LDQuODk2LTIuNTAyYzAuMTg4LTAuMjYzLDAuNDktMC40MTksMC44MTMtMC40MTljMCwwLDAsMCwwLjAwMSwwICAgYzAuMzIyLDAsMC42MjUsMC4xNTUsMC44MTMsMC40MTdjMS4xMjQsMS41NjgsMi45NTcsMi41MDQsNC45MDIsMi41MDRjMS45NDgsMCwzLjc3OS0wLjkzNiw0LjktMi41MDMgICBjMC4zNzUtMC41MjQsMS4yNTEtMC41MjQsMS42MjcsMGMxLjEyMSwxLjU2NywyLjk1MiwyLjUwMyw0Ljg5OCwyLjUwM2MxLjk0OCwwLDMuNzc5LTAuOTM2LDQuOS0yLjUwMyAgIGMwLjE4OC0wLjI2MiwwLjQ4OS0wLjQxOCwwLjgxMi0wLjQxOGwwLDBjMC4zMjIsMCwwLjYyNSwwLjE1NSwwLjgxMywwLjQxN2MxLjA5NSwxLjUyOSwzLjYzNCwyLjUwNCw0Ljg5OSwyLjUwNCAgIGMwLjU1MiwwLDEsMC40NDgsMSwxdjQuMTA0YzAsMC41NTItMC40NDgsMS0xLDFjLTEuNjY2LDAtNC4wOTctMC45MzMtNS42ODgtMi40MDFjLTEuNDkyLDEuNTIxLTMuNTYyLDIuNDAxLTUuNzM5LDIuNDAxICAgYy0yLjE2NCwwLTQuMjIyLTAuODcxLTUuNzEyLTIuMzc1Yy0xLjQ5LDEuNTA0LTMuNTQ4LDIuMzc1LTUuNzEzLDIuMzc1Yy0yLjE2MywwLTQuMjIyLTAuODcxLTUuNzE0LTIuMzc1ICAgYy0xLjQ4OCwxLjUwNC0zLjU0NiwyLjM3NS01LjcxMiwyLjM3NWMtMi4xNzYsMC00LjI0NC0wLjg4LTUuNzM1LTIuNDAxYy0xLjU5MiwxLjQ2OS00LjAyNCwyLjQwMS01LjY5MSwyLjQwMSAgIEMzMC42MjUsNzQuNjg1LDMwLjE3Nyw3NC4yMzcsMzAuMTc3LDczLjY4NXoiIGZpbGw9IiMwMDAwMDAiLz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K";
                }
            }
            else
            {
                src = "data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTYuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgd2lkdGg9IjUxMnB4IiBoZWlnaHQ9IjUxMnB4IiB2aWV3Qm94PSIwIDAgMTA2LjA1OSAxMDYuMDU5IiBzdHlsZT0iZW5hYmxlLWJhY2tncm91bmQ6bmV3IDAgMCAxMDYuMDU5IDEwNi4wNTk7IiB4bWw6c3BhY2U9InByZXNlcnZlIj4KPGc+Cgk8cGF0aCBkPSJNOTAuNTQ0LDkwLjU0MmMyMC42ODctMjAuNjg1LDIwLjY4NS01NC4zNDIsMC4wMDItNzUuMDI0QzY5Ljg1OC01LjE3MiwzNi4xOTktNS4xNzIsMTUuNTE2LDE1LjUxMyAgIEMtNS4xNzMsMzYuMTk4LTUuMTcxLDY5Ljg1OCwxNS41MTgsOTAuNTQ3QzM2LjE5OSwxMTEuMjMsNjkuODU4LDExMS4yMyw5MC41NDQsOTAuNTQyeiBNMjEuMzAyLDIxLjMgICBjMTcuNDkzLTE3LjQ5Myw0NS45Ni0xNy40OTUsNjMuNDU3LDAuMDAyYzE3LjQ5NCwxNy40OTQsMTcuNDkyLDQ1Ljk2Mi0wLjAwMiw2My40NTVjLTE3LjQ5MywxNy40OTQtNDUuOTYyLDE3LjQ5Ni02My40NTUsMC4wMDIgICBDMy44MDQsNjcuMjYzLDMuODA3LDM4Ljc5NCwyMS4zMDIsMjEuM3ogTTMyLjk2OSw0MC40MzFsMS43NTktMS43NTlsLTEuNzYtMS43NjJjLTEuMjMzLTEuMjMyLTEuMjk2LTMuMjI5LTAuMTQzLTQuNTQ0ICAgYzAuMDI1LTAuMDM1LDAuMDc4LTAuMDk3LDAuMTM3LTAuMTU2YzEuMjc3LTEuMjc4LDMuNDYtMS4yNzEsNC43MTgtMC4wMTJsMS43NiwxLjc1OWwxLjc2LTEuNzYgICBjMS4xOTgtMS4xOTksMy4yNjctMS4yNjIsNC41NDQtMC4xNDVjMC4wNDEsMC4wMjksMC4xMDYsMC4wODYsMC4xNjcsMC4xNDZjMC42MjksMC42MjksMC45NzYsMS40NjUsMC45NzcsMi4zNTQgICBjMCwwLjg5LTAuMzQ2LDEuNzI3LTAuOTc2LDIuMzU1bC0xLjc2LDEuNzYxbDEuNzU4LDEuNzU5YzAuNjMsMC42MjksMC45NzcsMS40NjYsMC45NzcsMi4zNTRjMCwwLjg5MS0wLjM0NiwxLjcyOC0wLjk3NiwyLjM1NyAgIGMtMS4yNTgsMS4yNi0zLjQ1MiwxLjI2MS00LjcxMiwwbC0xLjc1OC0xLjc1OGwtMS43NTgsMS43NThjLTAuNjMxLDAuNjMtMS40NjcsMC45NzctMi4zNTYsMC45NzdjLTAuODksMC0xLjcyNy0wLjM0Ny0yLjM1Ni0wLjk3NyAgIEMzMS42NzEsNDMuODQ0LDMxLjY3MSw0MS43MywzMi45NjksNDAuNDMxeiBNNjEuMDg2LDQwLjQxNWwxLjc1OS0xLjc1OWwtMS43Ni0xLjc2MWMtMS4yMzItMS4yMzMtMS4yOTYtMy4yMjktMC4xNDMtNC41NDQgICBjMC4wMjQtMC4wMzUsMC4wNzgtMC4wOTcsMC4xMzctMC4xNTdjMS4yNzctMS4yNzcsMy40Ni0xLjI3LDQuNzE4LTAuMDExbDEuNzYsMS43NTlsMS43NjEtMS43NiAgIGMxLjE5Ny0xLjE5OSwzLjI2Ny0xLjI2Miw0LjU0NC0wLjE0NmMwLjA0MSwwLjAzLDAuMTA1LDAuMDg2LDAuMTY3LDAuMTQ2YzAuNjI5LDAuNjI5LDAuOTc2LDEuNDY1LDAuOTc3LDIuMzU0ICAgYzAsMC44OTEtMC4zNDYsMS43MjgtMC45NzYsMi4zNTZsLTEuNzYsMS43NmwxLjc1OCwxLjc1OWMwLjYzLDAuNjMsMC45NzcsMS40NjcsMC45NzcsMi4zNTVjMCwwLjg5LTAuMzQ2LDEuNzI3LTAuOTc2LDIuMzU2ICAgYy0xLjI1OCwxLjI2MS0zLjQ1MiwxLjI2Mi00LjcxMiwwbC0xLjc1OS0xLjc1OEw2NS44LDQ1LjEyNGMtMC42MzEsMC42MzEtMS40NjcsMC45NzgtMi4zNTUsMC45NzggICBjLTAuODkxLDAtMS43MjgtMC4zNDctMi4zNTYtMC45NzhDNTkuNzg3LDQzLjgyOSw1OS43ODcsNDEuNzE2LDYxLjA4Niw0MC40MTV6IE0zMC4xNzcsNzMuNjg1VjY5LjU4YzAtMC41NTIsMC40NDgtMSwxLTEgICBjMS4yNjgsMCwzLjgwOS0wLjk3Niw0LjkwNC0yLjUwNGMwLjE4OC0wLjI2MiwwLjQ5MS0wLjQxNywwLjgxMy0wLjQxN2MwLDAsMCwwLDAuMDAxLDBjMC4zMjMsMCwwLjYyNSwwLjE1NiwwLjgxMywwLjQxOSAgIGMxLjExOSwxLjU2NywyLjk0OSwyLjUwMyw0Ljg5NSwyLjUwM2MxLjk0OSwwLDMuNzc5LTAuOTM2LDQuODk2LTIuNTAyYzAuMTg4LTAuMjYzLDAuNDktMC40MTksMC44MTMtMC40MTljMCwwLDAsMCwwLjAwMSwwICAgYzAuMzIyLDAsMC42MjUsMC4xNTUsMC44MTMsMC40MTdjMS4xMjQsMS41NjgsMi45NTcsMi41MDQsNC45MDIsMi41MDRjMS45NDgsMCwzLjc3OS0wLjkzNiw0LjktMi41MDMgICBjMC4zNzUtMC41MjQsMS4yNTEtMC41MjQsMS42MjcsMGMxLjEyMSwxLjU2NywyLjk1MiwyLjUwMyw0Ljg5OCwyLjUwM2MxLjk0OCwwLDMuNzc5LTAuOTM2LDQuOS0yLjUwMyAgIGMwLjE4OC0wLjI2MiwwLjQ4OS0wLjQxOCwwLjgxMi0wLjQxOGwwLDBjMC4zMjIsMCwwLjYyNSwwLjE1NSwwLjgxMywwLjQxN2MxLjA5NSwxLjUyOSwzLjYzNCwyLjUwNCw0Ljg5OSwyLjUwNCAgIGMwLjU1MiwwLDEsMC40NDgsMSwxdjQuMTA0YzAsMC41NTItMC40NDgsMS0xLDFjLTEuNjY2LDAtNC4wOTctMC45MzMtNS42ODgtMi40MDFjLTEuNDkyLDEuNTIxLTMuNTYyLDIuNDAxLTUuNzM5LDIuNDAxICAgYy0yLjE2NCwwLTQuMjIyLTAuODcxLTUuNzEyLTIuMzc1Yy0xLjQ5LDEuNTA0LTMuNTQ4LDIuMzc1LTUuNzEzLDIuMzc1Yy0yLjE2MywwLTQuMjIyLTAuODcxLTUuNzE0LTIuMzc1ICAgYy0xLjQ4OCwxLjUwNC0zLjU0NiwyLjM3NS01LjcxMiwyLjM3NWMtMi4xNzYsMC00LjI0NC0wLjg4LTUuNzM1LTIuNDAxYy0xLjU5MiwxLjQ2OS00LjAyNCwyLjQwMS01LjY5MSwyLjQwMSAgIEMzMC42MjUsNzQuNjg1LDMwLjE3Nyw3NC4yMzcsMzAuMTc3LDczLjY4NXoiIGZpbGw9IiMwMDAwMDAiLz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8Zz4KPC9nPgo8L3N2Zz4K";
            }
            return Json(new { SRC = src }, JsonRequestBehavior.AllowGet);

        }
    }
}