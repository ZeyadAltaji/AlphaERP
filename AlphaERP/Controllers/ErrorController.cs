using System.Web.Mvc;
namespace AlphaERP.Controllers
{
    public class ErrorController : controller
    {
        // GET: Error
        public ActionResult StoppedUser()
        {
            return View();
        }
        public ActionResult NotAuthorizedForAllCompanies()
        {
            Session["me"] = null;
            return View();
        }
        public ActionResult EmptyReport()
        {
            return View();
        }
    }
}
