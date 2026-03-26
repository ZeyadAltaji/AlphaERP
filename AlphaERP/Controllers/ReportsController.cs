using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Report.Web;
using System;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ReportsController : controller
    {
        
        public ActionResult Index(short CompNo, short PlanYear, int PlanNo, string MRT)
        {
            ViewBag.CompNo = CompNo;
            ViewBag.PlanYear = PlanYear;
            ViewBag.PlanNo = PlanNo;
            ViewBag.MRT = MRT;
            return PartialView();
        }

        public ActionResult GetReport()
        {
            string MRT = (TempData["MRT"]).ToString();
            short CompNo = Convert.ToInt16((TempData["CompNo"]).ToString());
            short PlanYear = Convert.ToInt16((TempData["PlanYear"]).ToString());
            int PlanNo = Convert.ToInt32((TempData["PlanNo"]).ToString());
            StiRequestParams requestParams = StiMvcViewer.GetRequestParams();
            Stimulsoft.Base.StiLicense.LoadFromFile(Server.MapPath("~/license.key"));
            StiReport report = new StiReport();
            string packedReport = System.IO.File.ReadAllText(Server.MapPath("~/Views/Reports/" + MRT + ".mrt"));
            report.LoadFromString(packedReport);
            report.Dictionary.Databases.Clear();
            report.Dictionary.Databases.Add(new Stimulsoft.Report.Dictionary.StiSqlDatabase("MS SQL", db.Database.Connection.ConnectionString));
            report.Dictionary.Variables.Clear();
            report.Dictionary.Variables.Add("CompNo", CompNo);
            report.Dictionary.Variables.Add("PlanYear", PlanYear);
            report.Dictionary.Variables.Add("PlanNo", PlanNo);
            return StiMvcViewer.GetReportResult(report);
        }

        public ActionResult PrintReport()
        {
            StiReport report = StiMvcViewer.GetReportObject();

            // Some actions with report when printing

            return StiMvcViewer.PrintReportResult(report);
        }

        public ActionResult ExportReport()
        {
            StiReport report = StiMvcViewer.GetReportObject();
            StiRequestParams parameters = StiMvcViewer.GetRequestParams();

            if (parameters.ExportFormat == StiExportFormat.Pdf)
            {
                // Some actions with report when exporting to PDF
            }

            return StiMvcViewer.ExportReportResult(report);
        }

        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }

        public ActionResult EmailContract()
        {
            // SenderEmail sender = db.SenderEmails.FirstOrDefault();
            //   StiEmailOptions options = StiMvcViewer.GetEmailOptions();

            //  List<SenderEmailCC> ccs = db.SenderEmailCCs.Where(x => x.Contract == true).ToList();
            //  foreach (SenderEmailCC cc in ccs)
            //  {
            //      options.CC.Add(cc.Email);
            //  }
            //  options.AddressFrom = sender.AddressFrom;
            //  options.Host = sender.Host;
            //  options.Port = sender.Port;
            //  options.UserName = sender.UserName;
            //  options.Password = sender.Password;
            //  options.EnableSsl = sender.EnableSsl;
            //  return StiMvcViewer.EmailReportResult(options);
            return StiMvcViewer.EmailReportResult(null);
        }

    }
}