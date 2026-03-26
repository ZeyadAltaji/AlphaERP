using Owin;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace AlphaERP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            RouteData rd = new RouteData();
            rd.DataTokens["area"] = ""; // In case controller is in another area
            rd.Values["controller"] = "Errors";
            rd.Values["action"] = "NotFoundError";


            var app = (HttpApplication)source;
            var uriObject = app.Context.Request.Url;
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 404)
            {
                Response.Clear();
                RouteData rd = new RouteData();
                rd.DataTokens["area"] = ""; // In case controller is in another area
                rd.Values["controller"] = "Errors";
                rd.Values["action"] = "NotFoundError";
                IController c = new Controllers.ErrorsController();
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
        }
    }
}
