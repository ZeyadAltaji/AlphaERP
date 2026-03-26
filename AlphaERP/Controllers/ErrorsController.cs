using System.Web.Mvc;
namespace AlphaERP.Controllers
{
    public sealed class ErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            ActionResult result;
            object model = Request.Url.PathAndQuery;
            if (!Request.IsAjaxRequest())
            {
                result = View(model);
            }
            else
            {
                result = View("_NotFound", model);
            }

            return result;
        }
        public ActionResult NotFoundError()
        {
            return View();
        }
        public ActionResult EmptyReport()
        {
            return View();
        }
    }
}
