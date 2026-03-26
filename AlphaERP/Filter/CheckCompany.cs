using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Filter
{
    public class CheckCompany : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Company"] == null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(Url.Action("Logout", "Account"));

            }
        }
    }
}