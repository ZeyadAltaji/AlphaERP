using AlphaERP.Models;
using System.Web.Mvc;

namespace AlphaERP.Filter
{
    public class AuthorizationFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string UserID = "";
            System.Web.HttpContextBase ctx = filterContext.HttpContext;
            User me = (User)ctx.Session["me"];
            UrlHelper Url = new UrlHelper(filterContext.RequestContext);
            string Action = filterContext.RouteData.Values["action"].ToString();
            string Controller = filterContext.RouteData.Values["Controller"].ToString();
            if (me != null)
            {
                UserID = me.UserID;
            }
            bool Allowed = true;
            // UsersPermission Exist = me.Permissions.Where(x=>x.Menu != null).Where(x => x.Menu.SourceForm == Controller && x.ProgAccess == true).FirstOrDefault();
            // if (Exist != null)
            // {
            //     Allowed = true;
            // }


            // List<SpecialPermission> SPs = new MDB().SpecialPermissions.Where(x => x.UserId == me.Id).ToList();
            // SpecialPermission ExistSp = SPs.Where(x => x.Access == true && x.ProgID == Controller).FirstOrDefault();
            // if (ExistSp != null)
            // {
            //     Allowed = true;
            // }
            // if (!Allowed && Controller == "Home" && Action == "Dashboard")
            // {
            //     SpecialPermission ExistSps = SPs.Where(x => x.Access == true && x.ProgID == Action).FirstOrDefault();
            //     if (ExistSps != null)
            //     {
            //         Allowed = true;
            //     }
            // }
            if (!Allowed)
            {
                filterContext.RouteData.Values["Controller"] = "AuthorizationFailed";
                filterContext.RouteData.Values["action"] = "Index";
                filterContext.Result = new RedirectResult(Url.Action("Index", "AuthorizationFailed"));
                //base.OnResultExecuting(filterContext);
            }

        }
    }
}