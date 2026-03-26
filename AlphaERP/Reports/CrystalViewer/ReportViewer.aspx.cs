using AlphaERP.Controllers;
using AlphaERP.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AlphaERP.Reports.CrystalViewer
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        string reportId = "";
        ReportDocument reportDocument = new ReportDocument();
        protected void Page_Init(object sender, EventArgs e)
        {

            UrlHelper urlHelp = new UrlHelper(HttpContext.Current.Request.RequestContext);
            try
            {
                reportId = (dynamic)HttpContext.Current.Session["reportInfoID"];
                ReportInformation ReportInfo = ReportInfoManager.GetReport(reportId);
                if(ReportInfo == null)
                {
                    Response.Redirect(urlHelp.Action("Logout", "Account"));
                    return;
                }
                DataSet Alpha_ERP_DataSet = ReportInfo.myDataSet;
                reportDocument.Load(ReportInfo.path);
                ParameterFields fields = ReportInfo.fields;
                reportDocument.SetDataSource(Alpha_ERP_DataSet);
                CrystalReportViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
               
                if (fields.Count != 0)
                {
                    foreach (ParameterField item in fields)
                    {
                        var  value = (ParameterDiscreteValue)item.CurrentValues[0];
                        reportDocument.SetParameterValue(item.Name, value.Value);
                    }
                }

                if (Alpha_ERP_DataSet.Tables[0].Rows.Count == 0)
                {Response.Redirect("EmptyReport.aspx");}

                else
                {
                    ReportInfo.ReportSource = reportDocument;
                    CrystalReportViewer.ReportSource = reportDocument;
                }

                /************************/



            }
            catch (Exception ex )
            {
                string exMessage = ex.Message;
                if (ex.Message == "Invalid report file path.")
                {
                    Response.Redirect(urlHelp.Action("Logout", "Account"));
                }
                else if (ex.Message == "Thread was being aborted.")
                {
                    Response.Redirect(urlHelp.Action("EmptyReport", "Error"));

                }
                else
                {
                    Response.Redirect(urlHelp.Action("ErrorMessage", "Account", new { errorMsg = exMessage }));
                }
            }
          
            /**********/
       
         

        }
        protected void Page_Unload(object sender, EventArgs e) {
            reportDocument.Close();
            reportDocument.Dispose();
        }
        protected void Print_Click(object sender, ImageClickEventArgs e)
        {
            string url = "PrintReport.aspx?id=" + reportId;
            Response.Write("<script>");
            Response.Write(" window.open('" + url + "', '_blank')");
            Response.Write("</script>");

        }
    }
}