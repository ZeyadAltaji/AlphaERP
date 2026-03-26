using AlphaERP.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlphaERP.Reports.CrystalViewer
{
    public partial class PrintReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string reportId = Request.QueryString["id"];
            UrlHelper urlHelp = new UrlHelper(HttpContext.Current.Request.RequestContext);
            ReportInformation report = ReportInfoManager.GetReport(reportId);
            if (report == null)
            {
                Response.Redirect(urlHelp.Action("Logout", "Account"));
                return;
            }
            ReportDocument reportDocument = new ReportDocument();
            ReportInformation ReportInfo = ReportInfoManager.GetReport(reportId);
            DataSet Alpha_ERP_DataSet = ReportInfo.myDataSet;
            reportDocument.Load(ReportInfo.path);
            ParameterFields fields = ReportInfo.fields;
            reportDocument.SetDataSource(Alpha_ERP_DataSet);
            if (fields.Count != 0)
            {
                foreach (ParameterField item in fields)
                {
                    var value = (ParameterDiscreteValue)item.CurrentValues[0];
                    reportDocument.SetParameterValue(item.Name, value.Value);
                }
            }

            reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ExportedReport");
        }
    }
}