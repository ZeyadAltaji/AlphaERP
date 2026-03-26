using Microsoft.AspNet.SignalR;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using AlphaERP.Hubs;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
namespace AlphaERP.Controllers
{
    public class WFController : controller
    {

        long AccNo = 0;
        int DeptNo = 0;

        public int notify(decimal TID, string BUUser, short? FID, string FDescAr, short? CompNo, string K1, string K2, string K3, string K4, string TrDesc)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.notify(TID, BUUser, FID, FDescAr, CompNo, K1, K2, K3, K4, TrDesc);
            return 0;
        }

        public ActionResult Functions(string userid)
        {
            List<Alpha_WorkFlowFunctions> functions = db.Alpha_WorkFlowFunctions.ToList();
            foreach (var f in functions)
            {
                f.SourceForm = c(userid, f.FID).ToString();
            }
            functions = functions.Where(x => x.SourceForm != "0").OrderByDescending(x => x.SourceForm).ToList();
            foreach (var x in functions)
            {
                short FID = x.FID;
                short COUNT = Convert.ToInt16(x.SourceForm);
                x.SourceForm = FID.ToString();
                x.FID = COUNT;
            }
            functions = functions.OrderByDescending(x => x.FID).ToList();
            return PartialView(functions);
        }

        private int c(string userid, int FID)
        {
            int r = 0;

            {
                string sql = string.Format("select count(*) count from Alpha_WorkFlowLog where BUUser = '{0}' and FID = '{1}' and RAction = 'cr' and BUUserAction is null", userid, FID);
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    r = (int)rdr["count"];
                    rdr.Close();
                }

            }
            return r;
        }

        public ActionResult GroupDetails(string userid, int FID)
        {
            string sql = string.Format("select * from Alpha_WorkFlowLog where BUUser = '{0}' and FID = '{1}' and RAction = 'cr' and BUUserAction is null", userid, FID);

            List<Alpha_WorkFlowLog> logs = db.Alpha_WorkFlowLog.SqlQuery(sql).ToList();
            return PartialView(logs);
        }


        string ServerName = System.Configuration.ConfigurationManager.AppSettings.Get("Server").Replace(@"\\", @"\");

        string DataBase_ = System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");

        public ActionResult GetWF(int TID)
        {
            short PlanYear; int PlanNo; int ReqNo; short CompNo;
            Alpha_WorkFlowLog log = db.Alpha_WorkFlowLog.Where(x => x.TID == TID).FirstOrDefault();
            BusunisUnit bu = db.Database.SqlQuery<BusunisUnit>(string.Format("SELECT BusUnitID AS Id, BusUnitDescAr AS DescAr, BusUnitDescEng AS DescEn, CompNo FROM Alpha_BusinessUnitDef WHERE (CompNo = {0}) AND (BusUnitID = {1})", company.comp_num, log.BU)).FirstOrDefault();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            PlanYear = Convert.ToInt16(log.K1);
            ReqNo = Convert.ToInt32(log.K2);
            CompNo = (short)log.CompNo.Value;
            PlanNo = db.Database.SqlQuery<int>(string.Format("SELECT PlanNo FROM MRP_ActivatePlan WHERE CompNo = {0} AND ReqYear = {1} and ReqNo = {2}", company.comp_num, PlanYear, ReqNo)).FirstOrDefault();
            MRP_GeneralPlanInfoH H = db.MRP_GeneralPlanInfoH.Where(x => x.CompNo == CompNo && x.PlanNo == PlanNo && x.PlanYear == PlanYear).FirstOrDefault();
            ViewBag.BU = bu;
            if (H == null)
            {
                H = new MRP_GeneralPlanInfoH();
                H.StartDate = DateTime.Now;
                H.EndDate = DateTime.Now;
            }
            Session["Log_"] = log;
            return View(H);
        }

        public JsonResult WF(string UserAction, int TID, string UserNotes)
        {
            SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = new SqlConnection(ConnectionString());
            {
                cmd.Connection = cn;
                cn.Open();
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                cmd.CommandText = "Alpha_ManageWorkFlowLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserAction", SqlDbType.VarChar)).Value = UserAction;
                cmd.Parameters.Add(new SqlParameter("@TrID", SqlDbType.Int)).Value = TID;
                cmd.Parameters.Add(new SqlParameter("@UserNotes", SqlDbType.VarChar)).Value = UserNotes;
                cmd.Parameters.Add(new SqlParameter("@FinalApprove", SqlDbType.SmallInt)).Direction = System.Data.ParameterDirection.Output;
                transaction = cn.BeginTransaction();
                cmd.Transaction = transaction;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
                }

                int ErrNo = Convert.ToInt32(cmd.Parameters["@FinalApprove"].Value);

                if (ErrNo == 0)
                {
                    transaction.Commit();
                    cn.Dispose();
                }
                else
                {
                    transaction.Rollback();
                    cn.Dispose();
                }
            }
            return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
        }

        private string WriteOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string WriteOrderTextRet = default(string);
            WriteOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    WriteOrderTextRet = string.Format("{0}:   {1}         ", IIf(Lang == 1, "سنة أمر الاتلاف", ""), Dr["OrdYear"]);
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم أمر الاتلاف", ""), Dr["OrdNo"], "<br/>");
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ أمر الاتلاف", ""), Conversions.ToDate(Dr["OrdDate"]).ToShortDateString(), "<br/>");
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم المستودع", ""), Dr["StoreNo"], "<br/>");
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وصف المستودع", ""), Dr["StoreName"], "<br/>");
                    WriteOrderTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "رقم الوثيقة", ""), Dr["DocNo"]);
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"], "<br/>");
                    WriteOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Notes"], "<br/>");
                }

                Dr.Close();

                WriteOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return WriteOrderTextRet;
        }

        private string ProdOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string ProdOrderTextRet = default(string);
            ProdOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    ProdOrderTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", ""), Dr["prepare_year"]);
                    ProdOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الأمر", ""), Dr["prepare_code"]);
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", ""), Conversions.ToDate(Dr["prepare_date"]).ToShortDateString(), "<br/>");
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BusUnitDesc"], "<br/>");
                    // ProdOrderText &= String.Format("{0}:   {1}{2}{2}", IIf(gLang = 1, "الدائرة", LangFile.GetString("الدائرة")), "12", vbCrLf) ' Dr("DeptName"), vbCrLf)
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الحساب", ""), Dr["acc_desc"], "<br/>");
                    // ProdOrderText &= String.Format("{0}:   {1}{2}", IIf(gLang = 1, "القيمة", LangFile.GetString("القيمة")), Dr("Amount"), vbCrLf)
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "السبب", ""), Dr["ExCostReason"], "<br/>");
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Note"], "<br/>");
                    ProdOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "المادة الجاهزة", ""), Dr["ItemDesc"]);
                    ProdOrderTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "الكمية", ""), Dr["qty_prepare"], "<br/>");
                }

                Dr.Close();
                ProdOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return ProdOrderTextRet;
        }

        private string TmpInvoiceText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string TmpInvoiceTextRet = default(string);
            TmpInvoiceTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    TmpInvoiceTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", ""), Dr["VouYear"]);
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الفاتورة", ""), Dr["VouNo"]);
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", ""), Conversions.ToDate(Dr["VouDate"]).ToShortDateString(), "<br/>");

                    // TmpInvoiceText &= String.Format("{0}:   {1}{2}{2}", IIf(gLang = 1, "وحدة العمل", LangFile.GetString("وحدة العمل")), Dr("BUnitDesc"), vbCrLf)

                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "العميل", ""), Dr["CustName"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المندوب", ""), Dr["SalesmanName"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "القيمة", ""), (double)(Dr["Total"]));
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "الخصم", ""), (double)(Dr["Discount"]));
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الضريبة", ""), (double)(Dr["Tax"]), "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الإجمالي", ""), (double)(Dr["NetAmount"]), "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "طريقة الدفع", ""), Dr["PayMethodDesc"]);
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Note"], "<br/>");
                    AccNo = Conversions.ToLong(Dr["CustNo"]);
                }

                Dr.Close();
                double ChqBal = GetCustChqBalance(CompNo, AccNo);
                double GlBal = GetCustGLBalance(CompNo, AccNo, DeptNo);
                double Limit = GetCustLimit(CompNo, AccNo, DeptNo);
                double OrderAvg = GetCustomerOrderAvg(CompNo, AccNo);
                TmpInvoiceTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "معدل طلبات العميل", ""), OrderAvg, "<br/>");
                TmpInvoiceTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "الحد المدين", ""), Limit, "<br/>");
                TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رصيد المحاسبة", ""), GlBal);
                TmpInvoiceTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "رصيد الشيكات", ""), ChqBal, "<br/>");
                TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), GlBal + ChqBal, "<br/>");

                var GetDate1 = DateAndTime.DateAdd(DateInterval.Month, -1, DateAndTime.Now.Date);
                var GetDate2 = DateAndTime.DateAdd(DateInterval.Month, -2, DateAndTime.Now.Date);
                DateTime Date1 = Conversions.ToDate(string.Format("{0}-{1}-{2}", GetDate1.Year, GetDate1.Month, DateTime.DaysInMonth(GetDate1.Year, GetDate1.Month)));
                DateTime Date2 = Conversions.ToDate(string.Format("{0}-{1}-{2}", GetDate2.Year, GetDate2.Month, DateTime.DaysInMonth(GetDate2.Year, GetDate2.Month)));
                using (var Cmd = new SqlCommand())
                {

                    {
                        Cmd.Connection = conn;
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }
                        Cmd.CommandText = "SalesOrder_LstCustInfo";
                        Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        Cmd.Parameters.Add(new SqlParameter("@TillDate", SqlDbType.SmallDateTime)).Value = DateAndTime.Now.Date.ToShortDateString();
                        Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = AccNo;
                        Cmd.Parameters.Add(new SqlParameter("@gLang", SqlDbType.SmallInt)).Value = Lang;
                        Cmd.Parameters.Add(new SqlParameter("@DateByMonth1", SqlDbType.SmallDateTime)).Value = Date1;
                        Cmd.Parameters.Add(new SqlParameter("@DateByMonth2", SqlDbType.SmallDateTime)).Value = Date2;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Dr = Cmd.ExecuteReader();

                    }
                }

                if (Dr.Read())
                {
                    TmpInvoiceTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السقف", ""), Dr["Dr_Lim"]);
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), Dr["Balance"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "قيمة التجاوز", ""), Dr["ExceedValue"]);
                    TmpInvoiceTextRet += string.Format("{0}:   {1} %{2}{2}", IIf(Lang == 1, "نسبة التجاوز", ""), Math.Round((double)Dr["ExceedPerc"], 2), "<br/>");
                    TmpInvoiceTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رصيد الفواتير المستحق", ""), Dr["DueAmntToDay"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "رصيد الفواتير حتى تاريخ", ""), Date1.ToShortDateString(), Dr["DueAmntDate1"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "رصيد الفواتير حتى تاريخ", ""), Date2.ToShortDateString(), Dr["DueAmntDate2"], "<br/>");
                    TmpInvoiceTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "قيمة التجاوز لإستحقاق الشيك", ""), "حتى تاريخه", Dr["DueChq"], "<br/>");
                }

                Dr.Close();
                TmpInvoiceTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return TmpInvoiceTextRet;
        }

        private double GetCustomerOrderAvg(int CompNo, long CustNo)
        {
            double GetCustomerOrderAvgRet = default(double);
            SqlCommand Cmd;
            SqlDataReader DrCust;
            int DrCount = 0;

            {
                Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SalesOrder_GetCustomerOrderAvg";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustNo;
                DrCust = Cmd.ExecuteReader();

            }
            while (DrCust.Read())
            {
                DrCount = DrCount + 1;
                GetCustomerOrderAvgRet = Conversions.ToDouble(IIf(Information.IsDBNull(DrCust["OrderAvg"]), 0, DrCust["OrderAvg"]));
            }

            DrCust.Close();
            return GetCustomerOrderAvgRet;
        }

        private double GetCustLimit(int CompNo, long CustNo, int DepNo)
        {
            double GetCustLimitRet = default(double);
            SqlCommand Cmd;
            SqlDataReader DrCust;
            int DrCount = 0;

            {
                Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SalesOrder_GetCustInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustNo;
                Cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = DeptNo;
                DrCust = Cmd.ExecuteReader();

            }
            while (DrCust.Read())
            {
                DrCount = DrCount + 1;
                GetCustLimitRet = Conversions.ToDouble(IIf(Information.IsDBNull(DrCust["Dr_lim"]), 0, DrCust["Dr_lim"]));
            }

            DrCust.Close();
            return GetCustLimitRet;
        }

        private double GetCustGLBalance(int CompNo, long CustAcc, int DepNo)
        {
            double GetCustGLBalanceRet = default(double);
            SqlDataReader CmdRd;
            double GlAmtVoh = 0;

            {
                var Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "InvT_GetCustGLBal";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustAcc;
                Cmd.Parameters.Add(new SqlParameter("@DepNo", SqlDbType.Int)).Value = DeptNo;
                CmdRd = Cmd.ExecuteReader();

            }
            while (CmdRd.Read())
                GlAmtVoh = Conversions.ToDouble(CmdRd["TotGlBalVoh"]);
            CmdRd.Close();
            GetCustGLBalanceRet = GlAmtVoh;
            return GetCustGLBalanceRet;
        }

        private double GetCustChqBalance(int CompNo, long CustAcc)
        {
            double GetCustChqBalanceRet = default(double);
            SqlDataReader CmdRd;
            double ChqsAmt = 0;
            var Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "InvT_GetCustChqs";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustAcc;
                CmdRd = Cmd.ExecuteReader();

            }
            while (CmdRd.Read())
                ChqsAmt = Conversions.ToDouble(CmdRd["TotChqs"]);
            CmdRd.Close();
            GetCustChqBalanceRet = ChqsAmt;
            return GetCustChqBalanceRet;
        }

        private string ReturnOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string ReturnOrderTextRet = default(string);
            ReturnOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    ReturnOrderTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", ""), Dr["TransYear"]);
                    ReturnOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الأمر", ""), Dr["TransNo"]);
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", ""), Conversions.ToDate(Dr["TransDate"]).ToShortDateString(), "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "نقدي/ذمم", ""), Dr["CrCaDesc"]);
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "نوع الوثيقة", ""), Dr["DocName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"]);
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المستودع", ""), Dr["StoreName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الدائرة", ""), Dr["DeptName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "العميل", ""), Dr["AccName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المندوب", ""), Dr["SalesmanName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "طريقة الدفع", ""), Dr["PayMethodName"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "سبب الإرجاع", ""), Dr["ReturnReasonDesc"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "القيمة", ""), Dr["Amount"], "<br/>");
                    ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Notes"], "<br/>");
                    if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(Dr["NeedAlert"], true, false)))
                    {
                        ReturnOrderTextRet += string.Format("**************** {0} ****************{1}{1}", IIf(Lang == 1, "يرجى مراجعة التفاصيل", ""), "<br/>");
                    }

                    AccNo = Conversions.ToLong(Dr["AccNo"]);
                    DeptNo = Conversions.ToInteger(Dr["DeptNo"]);
                }

                Dr.Close();
                double Bal = GetAccBalance(CompNo, DeptNo, AccNo);
                ReturnOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), Bal, "<br/>");
                ReturnOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return ReturnOrderTextRet;
        }

        public double GetAccBalance(int CompNo, int DeptNo, long AccNo)
        {
            double res = 0;

            {
                var Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SELECT     Round(ISNULL(Gln_GetOpenBal_1.AccBal,0),CurrType.fra) " + "FROM         CurrType INNER JOIN " + "glactmf ON CurrType.CurrType = glactmf.acc_curr INNER JOIN " + "dbo.Gln_GetOpenBal(@CompNo, @DeptNo, @DeptNo, @AccNo, @AccNo, '', @ToDate, 'Sys') AS Gln_GetOpenBal_1 ON glactmf.acc_comp = Gln_GetOpenBal_1.CompNo AND " + "glactmf.acc_num = Gln_GetOpenBal_1.AccNo ";
                Cmd.CommandType = CommandType.Text;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = CompNo;
                Cmd.Parameters.Add("@DeptNo", SqlDbType.Int).Value = DeptNo;
                Cmd.Parameters.Add("@AccNo", SqlDbType.BigInt).Value = AccNo;
                Cmd.Parameters.Add("@ToDate", SqlDbType.SmallDateTime).Value = DateAndTime.Now.Date.ToShortDateString();
                res = Conversions.ToDouble(IIf(Information.IsDBNull(Cmd.ExecuteScalar()), 0, Cmd.ExecuteScalar()));

            }
            return res;
        }

        private string PaymentOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string PaymentOrderTextRet = default(string);
            PaymentOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    PaymentOrderTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", ""), Dr["TransYear"]);
                    PaymentOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الأمر", ""), Dr["TransNo"]);
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", ""), Conversions.ToDate(Dr["TransDate"]).ToShortDateString(), "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"], "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الدائرة", ""), Dr["DeptName"], "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الحساب", ""), Dr["AccName"], "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "القيمة", ""), Dr["Amount"], "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "البيان", ""), Dr["Remark"], "<br/>");
                    PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Note"], "<br/>");
                    AccNo = Conversions.ToLong(Dr["AccNo"]);
                    DeptNo = Conversions.ToInteger(Dr["DeptNo"]);
                }

                Dr.Close();
                double Bal = GetAccBalance(CompNo, DeptNo, AccNo);
                PaymentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), Bal, "<br/>");
                PaymentOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return PaymentOrderTextRet;
        }

        private string ConsignmentOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string ConsignmentOrderTextRet = default(string);
            ConsignmentOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    AccNo = Conversions.ToLong(Dr["CashAcc"]);
                    ConsignmentOrderTextRet = string.Format("{0}:   {1}         ", IIf(Lang == 1, "سنة أمر التحويل", ""), Dr["TransferYear"]);
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم أمر التحويل", ""), Dr["TransferNo"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ أمر التحويل", ""), Conversions.ToDate(Dr["TransferDate"]).ToShortDateString(), "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "من مستودع", ""), Dr["FromStore"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وصف المستودع", ""), Dr["FromStoreDesc"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الى مستودع", ""), Dr["ToStore"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وصف المستودع", ""), Dr["StoreName"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["TransNotes"], "<br/>");
                    ConsignmentOrderTextRet += string.Format("{0}:   {1}       ", IIf(Lang == 1, "قيمة الطلب", ""), Dr["OrderAmount"]);
                }

                Dr.Close();
                double GlBal = GetCustGLBalance(CompNo, AccNo, DeptNo);
                ConsignmentOrderTextRet += string.Format("{0}:   {1}", IIf(Lang == 1, "رصيد المحاسبة", ""), GlBal);

                ConsignmentOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return ConsignmentOrderTextRet;
        }

        private string ProductionText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string ProductionTextRet = default(string);
            ProductionTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    ProductionTextRet = string.Format("{0}:   {1}         ", IIf(Lang == 1, "رقم أمر فك الحجز والصرف", ""), Dr["TrnID"]);
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ أمر فك الحجز والصرف", ""), Conversions.ToDate(Dr["OrderDate"]).ToShortDateString(), "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "سنة الخطة", ""), Dr["PlanYear"]);
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم الخطة", ""), Dr["PlanNo"], "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "سنة أمر التجهيز", ""), Dr["OrderYear"]);
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم أمر التجهيز", ""), Dr["OrderNo"], "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ أمر التجهيز", ""), Conversions.ToDate(Dr["prepare_date"]).ToShortDateString(), "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رمز المادة", ""), Dr["ItemNo"], "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وصف المادة", ""), Dr["ItemDesc"], "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الكمية", ""), Dr["qty_prepare"], "<br/>");
                    ProductionTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"]);
                    ProductionTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المستخدم", ""), Dr["UserID"], "<br/>");
                }

                Dr.Close();

                ProductionTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return ProductionTextRet;
        }

        private string IssueOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string IssueOrderTextRet = default(string);
            IssueOrderTextRet = "";
            try
            {

                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    IssueOrderTextRet = string.Format("{0}:   {1}         ", IIf(Lang == 1, "سنة أمر الصرف", ""), Dr["OrdYear"]);
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم أمر الصرف", ""), Dr["OrdNo"], "<br/>");
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ أمر الصرف", ""), Conversions.ToDate(Dr["OrdDate"]).ToShortDateString(), "<br/>");
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم المستودع", ""), Dr["StoreNo"], "<br/>");
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وصف المستودع", ""), Dr["StoreName"], "<br/>");
                    IssueOrderTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "رقم الوثيقة", ""), Dr["DocNo"]);
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"], "<br/>");
                    IssueOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Notes"], "<br/>");
                }

                Dr.Close();

                IssueOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return IssueOrderTextRet;
        }

        private string QuotationText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string QuotationTextRet = default(string);
            QuotationTextRet = "";
            try
            {
                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    QuotationTextRet = string.Format("{0}:   {1}         ", IIf(Lang == 1, "السنة", "Year"), Dr["QuotYear"]);
                    QuotationTextRet += string.Format("{0}:   {1}         ", IIf(Lang == 1, "رقم التسعير", "Pricing No."), Dr["QuotNo"]);
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رقم التسعير الفرعي", "Sub Pricing No."), Dr["QuotSubNo"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "التاريخ", "Date"), Dr["QuotDate"]);
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "نوع التسعير", "Pricing type"), Dr["QuotTypeDesc"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", "Business unit"), Dr["BUnitDesc"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "العميل", "Client"), Dr["CustName"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المندوب", "Delegate"), Dr["SalesmanName"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "الرقم المرجعي", "Reference No"), Dr["QuotRefNo"]);
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تاريخ المرجع", "Reference date"), Dr["RefDate"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "موضوع التسعير", "Pricing subject"), Dr["Subject"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ النهائي للتسليم", "Final delivery date"), Dr["DeliveryDate"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "مدة  التسعير", "Pricing period"), Dr["QuotPeriod"]);
                    QuotationTextRet += string.Format("{0}:   {1}  ", IIf(Lang == 1, "موعد التسليم من", "Delivery date"), Dr["DeliveryFrom"]);
                    QuotationTextRet += string.Format("{0}:   {1}  {2}{3}{3}", IIf(Lang == 1, "إلى", "To"), Dr["DeliveryTo"], IIf(Lang == 1, "أيام", "days"), "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", "Notes"), Dr["Note"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}       ", IIf(Lang == 1, "المجموع", "Total"), Dr["Total"]);
                    QuotationTextRet += string.Format("{0}:   {1}      ", IIf(Lang == 1, "الخصم", "Discount"), Dr["Discount"]);
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الضريبة", "Tax"), Dr["Tax"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الإجمالي", "Net amount"), Dr["NetAmount"], "<br/>");
                    AccNo = Conversions.ToLong(Dr["Customer"]);
                }

                Dr.Close();
                using (var Cmd = new SqlCommand())
                {

                    {
                        Cmd.Connection = conn;
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }
                        Cmd.CommandText = "Quot_GetCustomerFeedback";
                        Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        Cmd.Parameters.Add(new SqlParameter("@QuotYear", SqlDbType.SmallInt)).Value = k1;
                        Cmd.Parameters.Add(new SqlParameter("@QuotNo", SqlDbType.Int)).Value = k2;
                        Cmd.Parameters.Add(new SqlParameter("@QuotSubNo", SqlDbType.Int)).Value = k3;
                        Cmd.Parameters.Add(new SqlParameter("@Lang", SqlDbType.SmallInt)).Value = Lang;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Dr = Cmd.ExecuteReader();

                    }
                }

                if (Dr.Read())
                {
                    QuotationTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "حالة التسعير", "Pricing status"), Dr["StatusDesc"], "<br/>");
                    QuotationTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رد العميل", "Client reply"), Dr["FeedBack"], "<br/>");
                }

                Dr.Close();
                QuotationTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {
                // 
            }

            return QuotationTextRet;
        }

        private string POText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {

            string POTextRet = default(string);
            POTextRet = "";
            try
            {
                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                var VendorNo = default(long);
                var VendDeptNo = default(int);
                string VendorName = "";
                if (Dr.Read())
                {
                    POTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", "Year"), Dr["Year"]);
                    POTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الطلب", "Order No"), Dr["OrderNo"]);
                    POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "أمر الشراء", "Purchase Order"), Dr["TawreedNo"], "<br/>");
                    POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", "Date"), Dr["OrderDate"], "<br/>");
                    POTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "نوع الطلب", "Order type"), Dr["TypeDesc"]);
                    POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الجهة الطالبة", "Order party"), Dr["PDesc"], "<br/>");
                    POTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "المستخدم", "User"), Dr["UserID"]);
                    POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "قيمة الطلب", "Order amount"), Dr["NetAmount"], "<br/>");
                    POTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                    AccNo = Conversions.ToLong(Dr["AccNo"]);
                    DeptNo = Conversions.ToInteger(Dr["DeptNo"]);
                    VendorNo = Conversions.ToLong(Dr["VendorNo"]);
                    VendDeptNo = Conversions.ToInteger(Dr["Vend_Dept"]);
                    VendorName = Conversions.ToString(Dr["VendorName"]);
                }

                Dr.Close();
                double Budget = GetAccBudget(CompNo, DeptNo, AccNo, Conversions.ToInteger(k1), DateAndTime.Now.Date.Month);
                double Balance = GetAccBal(CompNo, DeptNo, AccNo);
                POTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "المخصص", "Allocated"), Budget);
                POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", "Balance"), Balance, "<br/>");
                POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المتبقي", "Left"), IIf(Budget < Balance, 0, Budget - Balance), "<br/>");
                POTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                POTextRet += string.Format("{0}:   {1}   {2}{3}{3}", IIf(Lang == 1, "المورد", "Supplier"), VendorNo, VendorName, "<br/>");
                POTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", "Balance"), GetAccBal(CompNo, VendDeptNo, VendorNo), "<br/>");
                POTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {
                //
            }

            return POTextRet;
        }

        private string PRText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string PRTextRet = default(string);
            PRTextRet = "";
            try
            {
                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);
                if (Dr.Read())
                {
                    PRTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", "Year"), Dr["Year"]);
                    PRTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الطلب", "Order No"), Dr["OrderNo"]);
                    PRTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", "Date"), Dr["OrderDate"], "<br/>");
                    PRTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "نوع الطلب", "Order type"), Dr["TypeDesc"]);
                    PRTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الجهة الطالبة", "Requesting party"), Dr["PDesc"], "<br/>");
                    PRTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "المستخدم", "User"), Dr["UserID"]);
                    PRTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "قيمة الطلب", "Order amount"), Dr["NetAmount"], "<br/>");
                    PRTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                    AccNo = Conversions.ToLong(Dr["AccNo"]);
                    DeptNo = Conversions.ToInteger(Dr["DeptNo"]);
                }

                Dr.Close();
                double Budget = GetAccBudget(CompNo, DeptNo, AccNo, Conversions.ToInteger(k1), DateAndTime.Now.Date.Month);
                double Balance = GetAccBal(CompNo, DeptNo, AccNo);
                PRTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "المخصص", "Allocated"), Budget);
                PRTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", "Balance"), Balance, "<br/>");
                PRTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المتبقي", "Left"), IIf(Budget < Balance, 0, Budget - Balance), "<br/>");
                PRTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return PRTextRet;
        }

        public double GetAccBal(int CompNo, int DeptNo, long AccNo)
        {
            double GetAccBalRet = default(double);
            var Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "Select IsNull(Gln_GetOpenBal.AccBal,0) as AccBal From Gln_GetOpenBal(@CompNo, @DeptNo, @DeptNo, @AccNo, @AccNo, '200405', @ToDate, 'sys')";
                Cmd.CommandType = CommandType.Text;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = DeptNo;
                Cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = AccNo;
                Cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.SmallDateTime)).Value = DateAndTime.Now.Date.ToShortDateString();
                GetAccBalRet = Conversions.ToDouble(Cmd.ExecuteScalar());

            }
            return GetAccBalRet;
        }

        public double GetAccBudget(int CompNo, int DeptNo, long AccNo, int BYear, int BMonth)
        {
            double GetAccBudgetRet = default(double);
            var Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "Select IsNull(GLN_GetBudgetTillMonth.Budget,0) as Budget From GLN_GetBudgetTillMonth(@CompNo,@DeptNo, @DeptNo, @AccNo, @AccNo, @BYear, @ToMonth)";
                Cmd.CommandType = CommandType.Text;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = DeptNo;
                Cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = AccNo;
                Cmd.Parameters.Add(new SqlParameter("@BYear", SqlDbType.SmallInt)).Value = BYear;
                Cmd.Parameters.Add(new SqlParameter("@ToMonth", SqlDbType.SmallInt)).Value = BMonth;
                GetAccBudgetRet = Conversions.ToDouble(Cmd.ExecuteScalar());

            }
            return GetAccBudgetRet;
        }

        private string SalesOrderText(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string SalesOrderTextRet = default(string);
            SalesOrderTextRet = "";
            try
            {
                SqlDataReader Dr = ShowDetails(FID, TID, CompNo, k1, k2, k3, k4, Lang);

                if (Dr.Read())
                {
                    SalesOrderTextRet = string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السنة", ""), Dr["OrderYear"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رقم الطلب", ""), Dr["OrderNo"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", ""), Conversions.ToDate(Dr["OrderDate"]).ToShortDateString(), "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "نوع الطلب", ""), Dr["TypeDesc"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "وحدة العمل", ""), Dr["BUnitDesc"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "العميل", ""), Dr["CustName"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "المندوب", ""), Dr["SalesmanName"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "القيمة", ""), Dr["Total"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "الخصم", ""), Dr["Discount"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الضريبة", ""), Dr["Tax"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الإجمالي", ""), Dr["NetAmount"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "طريقة الدفع", ""), Dr["PayMethodDesc"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "تفاصيل طريقة الدفع", ""), Dr["PayTermsDet"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الحجز", ""), Dr["ReserveItems"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", ""), Dr["Comments"], "<br/>");
                    AccNo = Conversions.ToLong(Dr["CustNo"]);
                    DeptNo = Conversions.ToInteger(Dr["DeptNo"]);
                }

                Dr.Close();
                double ChqBal = GetCustChqBalanceByDept(AccNo, DeptNo, CompNo);
                double GlBal = GetCustGLBalance(CompNo, AccNo, DeptNo);
                double Limit = GetCustLimit(CompNo, AccNo, DeptNo);
                double OrderAvg = GetCustomerOrderAvg(CompNo, AccNo);
                double Orders = GetCustOrders(AccNo, DeptNo, CompNo);
                SalesOrderTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "معدل طلبات العميل", ""), OrderAvg, "<br/>");
                SalesOrderTextRet += string.Format("{0}:   {1}                                      ", IIf(Lang == 1, "الحد المدين", ""), Limit);
                SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "طلبات البيع", ""), Orders, "<br/>");
                SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "رصيد المحاسبة", ""), GlBal);
                SalesOrderTextRet += string.Format("{0}:   {1}{2}", IIf(Lang == 1, "رصيد الشيكات", ""), ChqBal, "<br/>");
                SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), GlBal + ChqBal, "<br/>");

                var GetDate1 = DateAndTime.DateAdd(DateInterval.Month, -1, DateAndTime.Now.Date);
                var GetDate2 = DateAndTime.DateAdd(DateInterval.Month, -2, DateAndTime.Now.Date);
                DateTime Date1 = Conversions.ToDate(string.Format("{0}-{1}-{2}", GetDate1.Year, GetDate1.Month, DateTime.DaysInMonth(GetDate1.Year, GetDate1.Month)));
                DateTime Date2 = Conversions.ToDate(string.Format("{0}-{1}-{2}", GetDate2.Year, GetDate2.Month, DateTime.DaysInMonth(GetDate2.Year, GetDate2.Month)));
                using (var Cmd = new SqlCommand())
                {

                    {
                        Cmd.Connection = conn;
                        if (conn.State == ConnectionState.Closed) { conn.Open(); }
                        Cmd.CommandText = "SalesOrder_LstCustInfo";
                        Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        Cmd.Parameters.Add(new SqlParameter("@TillDate", SqlDbType.SmallDateTime)).Value = DateAndTime.Now.Date.ToShortDateString();
                        Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = AccNo;
                        Cmd.Parameters.Add(new SqlParameter("@gLang", SqlDbType.SmallInt)).Value = Lang;
                        Cmd.Parameters.Add(new SqlParameter("@DateByMonth1", SqlDbType.SmallDateTime)).Value = Date1;
                        Cmd.Parameters.Add(new SqlParameter("@DateByMonth2", SqlDbType.SmallDateTime)).Value = Date2;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Dr = Cmd.ExecuteReader();

                    }
                }

                if (Dr.Read())
                {
                    SalesOrderTextRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "السقف", ""), Dr["Dr_Lim"]);
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "الرصيد", ""), Dr["Balance"], "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}                   ", IIf(Lang == 1, "قيمة التجاوز", ""), Dr["ExceedValue"]);
                    SalesOrderTextRet += string.Format("{0}:   {1} %{2}{2}", IIf(Lang == 1, "نسبة التجاوز", ""), Convert.ToDouble(Dr["ExceedPerc"]).ToString("#0.00"), "<br/>");
                    SalesOrderTextRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "رصيد الفواتير المستحق", ""), Dr["DueAmntToDay"], "<br/>");
                    SalesOrderTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "رصيد الفواتير حتى تاريخ", ""), Date1.ToShortDateString(), Dr["DueAmntDate1"], "<br/>");
                    SalesOrderTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "رصيد الفواتير حتى تاريخ", ""), Date2.ToShortDateString(), Dr["DueAmntDate2"], "<br/>");
                    SalesOrderTextRet += string.Format("{0} {1}:   {2}{3}{3}", IIf(Lang == 1, "قيمة التجاوز لإستحقاق الشيك", ""), "حتى تاريخه", Dr["DueChq"], "<br/>");
                }

                Dr.Close();
                SalesOrderTextRet += ShowNotes(FID, TID, CompNo, k1, k2, k3, k4, Lang);
            }
            catch (Exception ex)
            {

            }

            return SalesOrderTextRet;
        }
        SqlConnection conn = new SqlConnection(string.Format("user id='admin';password='GceSoft01042000';initial catalog={1};data source={0};Connect Timeout=30;Persist Security Info=false;Integrated Security=false", System.Configuration.ConfigurationManager.AppSettings.Get("Server").Replace(@"\\", @"\"), System.Configuration.ConfigurationManager.AppSettings.Get("DataBase")));
        public SqlDataReader ShowDetails(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            SqlDataReader Dr;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            cmd.CommandText = "Alpha_ShowDetailsWFLog";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = CompNo;
            cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = FID;
            cmd.Parameters.Add("@K1", SqlDbType.VarChar, 20).Value = k1;
            cmd.Parameters.Add("@K2", SqlDbType.VarChar, 20).Value = k2;
            cmd.Parameters.Add("@K3", SqlDbType.VarChar, 20).Value = k3;
            cmd.Parameters.Add("@K4", SqlDbType.VarChar, 20).Value = k4;
            cmd.Parameters.Add("@Lang", SqlDbType.TinyInt).Value = Lang;
            cmd.CommandType = CommandType.StoredProcedure;
            Dr = cmd.ExecuteReader();
            return Dr;
        }

        private Object IIf(Boolean Expression, Object TruePart, Object FalsePart)
        {
            if (Expression)
            {
                return TruePart;
            }
            else
            {
                return FalsePart;
            }
        }

        private double GetCustChqBalanceByDept(long CustAcc, long DeptNo, short CompNo)
        {
            double GetCustChqBalanceByDeptRet = default(double);
            SqlDataReader CmdRd;
            double ChqsAmt = 0;

            {
                var Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "InvT_GetCustChqsByDept";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = DeptNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustAcc;
                CmdRd = Cmd.ExecuteReader();

            }
            while (CmdRd.Read())
                ChqsAmt = Conversions.ToDouble(CmdRd["TotChqs"]);
            CmdRd.Close();
            GetCustChqBalanceByDeptRet = ChqsAmt;
            return GetCustChqBalanceByDeptRet;
        }

        private double GetCustGLBalance(long CustAcc, int DepNo, short CompNo)
        {
            double GetCustGLBalanceRet = default(double);
            SqlDataReader CmdRd;
            double GlAmtVoh = 0;
            var Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "InvT_GetCustGLBal";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustAcc;
                Cmd.Parameters.Add(new SqlParameter("@DepNo", SqlDbType.Int)).Value = DepNo;
                CmdRd = Cmd.ExecuteReader();

            }
            while (CmdRd.Read())
                GlAmtVoh = Conversions.ToDouble(CmdRd["TotGlBalVoh"]);
            CmdRd.Close();
            GetCustGLBalanceRet = GlAmtVoh;
            return GetCustGLBalanceRet;
        }

        private double GetCustLimit(long CustNo, int DepNo, short CompNo)
        {
            double GetCustLimitRet = default(double);
            SqlCommand Cmd;
            SqlDataReader DrCust;
            int DrCount = 0;

            {
                Cmd = new SqlCommand();
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SalesOrder_GetCustInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustNo;
                Cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = DepNo;
                DrCust = Cmd.ExecuteReader();

            }
            while (DrCust.Read())
            {
                DrCount = DrCount + 1;
                GetCustLimitRet = Conversions.ToDouble(IIf(Information.IsDBNull(DrCust["Dr_lim"]), 0, DrCust["Dr_lim"]));
            }

            DrCust.Close();
            return GetCustLimitRet;
        }

        private string ShowNotes(int FID, int TID, short CompNo, string k1, string k2, string k3, string k4, short Lang)
        {
            string ShowNotesRet = default(string);
            ShowNotesRet = "";
            SqlDataReader Dr;
            using (var Cmd = new SqlCommand())
            {

                {
                    Cmd.Connection = conn;
                    if (conn.State == ConnectionState.Closed) { conn.Open(); }
                    Cmd.CommandText = "Alpha_ShowNotesWFLog";
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = CompNo;
                    Cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = FID;
                    Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 20).Value = k1;
                    Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 20).Value = k2;
                    Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 20).Value = k3;
                    Cmd.Parameters.Add("@K4", SqlDbType.VarChar, 20).Value = k4;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Dr = Cmd.ExecuteReader();

                }
            }

            while (Dr.Read())
            {
                ShowNotesRet += string.Format("-------------------------------------------------------{0}{0}", "<br/>");
                ShowNotesRet += string.Format("{0}:   {1}           ", IIf(Lang == 1, "المستخدم", "User"), Dr["BUUser"]);
                ShowNotesRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "التاريخ", "Date"), Dr["ActionDate"], "<br/>");
                ShowNotesRet += string.Format("{0}:   {1}{2}{2}", IIf(Lang == 1, "ملاحظات", "Notes"), Dr["UserNotes"], "<br/>");
            }

            Dr.Close();
            return ShowNotesRet;
        }

        private double GetCustomerOrderAvg(long CustNo, short CompNo)
        {
            double GetCustomerOrderAvgRet = default(double);
            SqlCommand Cmd;
            SqlDataReader DrCust;
            int DrCount = 0;
            Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SalesOrder_GetCustomerOrderAvg";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = CustNo;
                DrCust = Cmd.ExecuteReader();

            }
            while (DrCust.Read())
            {
                DrCount = DrCount + 1;
                GetCustomerOrderAvgRet = Conversions.ToDouble(IIf(Information.IsDBNull(DrCust["OrderAvg"]), 0, DrCust["OrderAvg"]));
            }

            DrCust.Close();
            return GetCustomerOrderAvgRet;
        }

        private double GetCustOrders(long AccNo, int DeptNo, short CompNo)
        {
            double xx = 0;
            var Cmd = new SqlCommand();

            {
                Cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                Cmd.CommandText = "SalesOrder_SP_GetCustOrders";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = CompNo;
                Cmd.Parameters.Add("@DeptNo", SqlDbType.Int).Value = DeptNo;
                Cmd.Parameters.Add("@CustNo", SqlDbType.BigInt).Value = AccNo;
                xx = Conversions.ToDouble(IIf(Information.IsDBNull(Cmd.ExecuteScalar()), 0, Cmd.ExecuteScalar()));

            }
            return xx;
        }

    }
}