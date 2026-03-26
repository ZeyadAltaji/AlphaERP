using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ShipmentsControlController : controller
    {
        // GET: ShipmentsControl
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShipmentsControlList(DateTime Date)
        {
            ViewBag.Date = Date.ToString("yyyy-MM-dd");
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_Web_GetShipmentsControl";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.SmallDateTime)).Value = Date;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<ShipmentsControl> lShipmentsControl = Dt.AsEnumerable().Select(row => new ShipmentsControl
            {
                OrdYear = row.Field<short>("OrderYear"),
                OrderNo = row.Field<int>("OrderNo"),
                TawreedNo = row.Field<string>("TawreedNo"),
                ShipSer = row.Field<string>("ShipSer"),
                RecDate = row.Field<DateTime>("RecDate"),
                PurchOrdQty = row.Field<double>("PurchOrdQty"),
                PurchOrdQty2 = row.Field<double>("PurchOrdQty2"),
                PortQty = row.Field<double>("PortQty"),
                PortQty2 = row.Field<double>("PortQty2"),
                ReceiptQty = row.Field<double>("ReceiptQty"),
                ReceiptQty2 = row.Field<double>("ReceiptQty2"),
                WriteOffQty = row.Field<double>("WriteOffQty"),
                WriteOffQty2 = row.Field<double>("WriteOffQty2"),
                SalesQty = row.Field<double>("SalesQty"),
                SalesQty2 = row.Field<double>("SalesQty2"),
                TotalAdjustedWeight = row.Field<double>("TotalAdjustedWeight"),
                QtyOH = row.Field<decimal>("QtyOH"),
                QtyOH2 = row.Field<decimal>("QtyOH2"),

                TotalIssues = row.Field<double>("TotalIssues"),
                TotalIssuesTreatments = row.Field<double>("TotalIssuesTreatments"),
                IssuesQty = row.Field<double>("IssuesQty"),
                IssuesTreatmentsQty = row.Field<double>("IssuesTreatmentsQty"),
                Total = row.Field<double>("Total"),
            }).ToList();

            return PartialView(lShipmentsControl);

        }
    }
}