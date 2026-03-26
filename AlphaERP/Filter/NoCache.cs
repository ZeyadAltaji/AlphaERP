using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Filter
{
    public  class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();
            var Url = new UrlHelper(filterContext.RequestContext);
            string originAction = filterContext.RouteData.Values["action"].ToString();
          
                if (HttpContext.Current.Session["UserInfo"] != null && HttpContext.Current.Session["Company"] != null)
                {
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Home"));
                }
                else if (HttpContext.Current.Session["UserInfo"] != null && HttpContext.Current.Session["Company"] == null)
                {
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Company", new { area = string.Empty }));

                }

            //base.OnResultExecuting(filterContext);
        }
    }
}