using AlphaERP.Controllers;
using AlphaERP.Filter;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class CrystalReportViewerController : controller
    {
        // GET: CrystalReportViewer
        public ActionResult ReportViewer(string id)
        {
            ReportInformation report = ReportInfoManager.GetReport(id);
            if(report == null)
            {
                return RedirectToAction("Logout", "Account", new { area = string.Empty });
            }
            ViewBag.report = report;
            return View();
        }

      

    }
}