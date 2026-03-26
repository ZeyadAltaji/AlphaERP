using AlphaERP.Models;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class StagesQcInProcessController : controller
    {
        private DataSet DsQCTest = new DataSet();
        public ActionResult Index() 
        {


            DataSet d = LoadQcHFInPross();
            return View(d);
        }

        public ActionResult List()
        {
            DataSet d = LoadQcHFInPross();
            return View(d);
        }

        public DataSet LoadQcHFInPross()
        {
            int LstOption = 3;
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsOrders = new DataSet();
            var DaOrders = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_WebLists";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            Cmd.Parameters.Add(new SqlParameter("@LstOption", SqlDbType.SmallInt, 2)).Value = LstOption;
            DaOrders.SelectCommand = Cmd;
            DsOrders.Tables.Clear();
            DsOrders.Dispose();
            DaOrders.Fill(DsOrders, "Orders");
            cn.Close();
            return DsOrders;
        }

        public ActionResult LoadQcPros(long RefNo,short orderyear, int orderno)
        {
            ViewBag.RefNo = RefNo;
            ViewBag.orderyear = orderyear;
            ViewBag.orderno = orderno;
            DataSet d = LoadQcDFInPross(RefNo);
            return View(d);
        }

        public DataSet LoadQcDFInPross(long RefNo)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsQCStages = new DataSet();
            var DaQCStages = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_GetQCDFInPross";
            // Cmd.Parameters.Add(New SqlClient.SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = gCompNo
            Cmd.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
            DaQCStages.SelectCommand = Cmd;
            DsQCTest.Tables.Clear();
            DsQCStages.Tables.Clear();
            DsQCStages.Dispose();
            DaQCStages.Fill(DsQCStages, "QCPross");
            cn.Close();
            return DsQCStages;
        }
        //public JsonResult LoadTestDetQcDf(long RefNo, long QCTrNo)
        //{
        //    SqlConnection cn = new SqlConnection(ConnectionString());
        //    cn.Open();
        //    var DaQCTest = new SqlDataAdapter();
        //    DsQCTest = new DataSet();
        //    var DaCmd = new SqlDataAdapter();
        //    var Cmd = new SqlCommand();
        //    Cmd.Connection = cn;
        //    Cmd.CommandType = CommandType.StoredProcedure;
        //    Cmd.CommandText = "ProdCost_GetTestQCDF";
        //    Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
        //    Cmd.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
        //    Cmd.Parameters.Add(new SqlParameter("@QCTrNo", SqlDbType.BigInt)).Value = QCTrNo;
        //    DaQCTest.SelectCommand = Cmd;
        //    DsQCTest.Tables.Clear();
        //    DsQCTest.Dispose();
        //    DaQCTest.Fill(DsQCTest, "QCTest");
          
        //    DataTable Dt = new DataTable();
        //    using (var Da = new SqlDataAdapter())
        //    {
        //        Da.SelectCommand = Cmd;
        //        Da.Fill(Dt);
        //    }
        //    List<Test> tests = new List<Test>();
        //    foreach (DataRow Dr in DsQCTest.Tables["QCTest"].Rows)
        //    {
        //        Test t = new Test();
        //        t.TestNo = Convert.ToInt32(Dr["TestNo"].ToString());
        //        t.TestDesc = Dr["TestDesc"].ToString();
        //        t.ValFrom = Convert.ToDecimal(Dr["ValFrom"].ToString());
        //        t.ValTo = Convert.ToDecimal(Dr["ValTo"].ToString());
        //        t.QCVal = Convert.ToDecimal(Dr["QCVal"].ToString());
        //        t.Notes = Dr["Notes"].ToString();
        //        tests.Add(t);
        //    }


        //    string Notes = Conversions.ToString(Interaction.IIf(Information.IsDBNull(Dt.Rows[0]["GeneralQcNotes"]), "", Dt.Rows[0]["GeneralQcNotes"]));
        //    bool FixTest = Conversions.ToBoolean(Dt.Rows[0]["QCClosed"]);
        //    bool OptSuccess = Conversions.ToBoolean(Dt.Rows[0]["QCPassed"]);

        //    return Json(new { tests, Notes, FixTest, OptSuccess }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult LoadTestDetQcDf(long RefNo, long QCTrNo, short orderyear, int orderno)
        {
            List<LoadTestDetQcDfView> ListTestDF = db.Database.SqlQuery<LoadTestDetQcDfView>("exec ProdCost_GetTestQCDF {0},{1},{2}", company.comp_num, RefNo, QCTrNo).ToList();
            ProdCost_QCInspHF getTestDetQcHF = db.ProdCost_QCInspHF.Where(x => x.CompNo == company.comp_num && x.RefNo == RefNo &&x.OrderNo == orderyear && x.OrderYear == orderno ).FirstOrDefault();
            ViewBag.getTestDetQcHF = getTestDetQcHF;

            ViewBag.orderyear = orderyear;
            ViewBag.orderno = orderno;
            ViewBag.RefNo = RefNo;
            return PartialView(ListTestDF);
        }
        public  ActionResult LoadViewTestDetQCDf(long RefNo,long QCTrNo)
        {
            List<LoadTestDetQcDfView> ListTestDF = db.Database.SqlQuery<LoadTestDetQcDfView>("exec ProdCost_GetTestQCDF {0},{1},{2}", company.comp_num, RefNo, QCTrNo).ToList();

            ProdCost_QCInspHF getTestDetQcHF = db.ProdCost_QCInspHF.Where(x => x.CompNo == company.comp_num && x.RefNo == RefNo).FirstOrDefault();
            ViewBag.getTestDetQcHF = getTestDetQcHF;


            return PartialView(ListTestDF);
        }

        public JsonResult Close(long RefNo, bool QCPassed, string Notes,bool? QCClosed, long QCTrNo)
        {

            SqlConnection cn = Cn();
            cn.Open();
            using (SqlCommand Cmd = new SqlCommand())
            { 
                Cmd.Connection = cn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "ProdCost_UpdQcInspHf";
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
                Cmd.Parameters.Add(new SqlParameter("@QCPassed", SqlDbType.Bit)).Value = QCPassed;
                Cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 250)).Value = Notes;
                Cmd.ExecuteNonQuery();
            }
            using (SqlCommand CmdUPdate = new SqlCommand())
            {
                CmdUPdate.Connection = cn;
                CmdUPdate.CommandType = CommandType.Text;
                CmdUPdate.CommandText = "update ProdCost_QCInspDF set QCClosed=1 where RefNo =@RefNo and QCTrNo=@QCTrNo";
                CmdUPdate.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
                CmdUPdate.Parameters.Add(new SqlParameter("@QCTrNo", SqlDbType.BigInt)).Value = QCTrNo;
                CmdUPdate.ExecuteNonQuery();

            }
            cn.Close();
            return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long RefNo, long QCTrNo)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd1 = new SqlCommand();
            Cmd1.Connection = cn;
            Cmd1.CommandText = "ProdCost_DelQCTestInProcess";
            Cmd1.CommandType = CommandType.StoredProcedure;
            Cmd1.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
            Cmd1.Parameters.Add(new SqlParameter("@QCTrNo", SqlDbType.BigInt)).Value = QCTrNo;
            Cmd1.ExecuteNonQuery();
            return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
        }
        //public DataTable Dt = new DataTable();
        public DataTable TmpDt;
        private int MaxQCTrNo1;
        public class Test
        {
            public int TestNo { get; set; }
            public string TestDesc { get; set; }
            public decimal ValFrom { get; set; }
            public decimal ValTo { get; set; }
            public decimal QCVal { get; set; }
            public string Notes { get; set; }
            public bool? QCPassed { get; set; }

        }
        public JsonResult SaveFile(List<Test> tests, short OrderYear, int OrderNo, int TransType, long RefNo, long QCTrNo, string Notes)
        {
            string TrDesc;
            try
            {
                SqlConnection cn = Cn();
                cn.Open();
                var Cmd1 = new SqlCommand();

                TmpDt = new DataTable();
                TmpDt.Columns.Add(new DataColumn("TestNo"));
                TmpDt.Columns.Add(new DataColumn("ValFrom"));
                TmpDt.Columns.Add(new DataColumn("ValTo"));
                TmpDt.Columns.Add(new DataColumn("QCVal"));
                TmpDt.Columns.Add(new DataColumn("Notes"));
                TmpDt.Columns.Add(new DataColumn("QCPassed"));

                foreach (var t in tests)
                {
                    var TmpDr = TmpDt.NewRow();
                    TmpDr["TestNo"] = t.TestNo;
                    TmpDr["ValFrom"] = t.ValFrom;
                    TmpDr["ValTo"] = t.ValTo;
                    TmpDr["QCVal"] = t.QCVal;
                    TmpDr["Notes"] = t.Notes;
                    TmpDr["QCPassed"] = t.QCPassed;
                    if (Convert.ToBoolean(Operators.ConditionalCompareObjectEqual(t.TestNo, 1, false) & gGceClient == 251))
                    {
                        var Cmd = new SqlCommand();
                        Cmd.Connection = cn;
                        Cmd.CommandType = CommandType.Text;
                        Cmd.CommandText = "update prod_prepare_info set Actual_SG=" + t.QCVal + " where comp_no=" + company.comp_num + " and prepare_year=" + OrderYear + " and prepare_code=" + OrderNo + "";
                        Cmd.ExecuteNonQuery();
                    }
                    TmpDt.Rows.Add(TmpDr);

                }

                SqlTransaction myTrans;
                myTrans = cn.BeginTransaction();
                if (TransType == 2)
                {
                    Cmd1.Connection = cn;
                    Cmd1.CommandText = "ProdCost_DelQCTestInProcess";
                    Cmd1.CommandType = CommandType.StoredProcedure;
                    Cmd1.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
                    Cmd1.Parameters.Add(new SqlParameter("@QCTrNo", SqlDbType.BigInt)).Value = QCTrNo;
                    Cmd1.Transaction = myTrans;
                    Cmd1.ExecuteNonQuery();
                }
                else
                {
                    MaxQCTrNo1 = GetMaxQCTrNo(myTrans, cn, RefNo);
                }

                using (var Da = new SqlDataAdapter())
                {
                    using (var Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.CommandText = "ProdCost_AddQCInProcess";
                        Cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = me.UserID;
                        Cmd.Parameters.Add("@RefNo", SqlDbType.BigInt).Value = RefNo;
                        Cmd.Parameters.Add("@QCTrNo", SqlDbType.BigInt).Value = Interaction.IIf(TransType == 1, MaxQCTrNo1, QCTrNo);
                        Cmd.Parameters.Add("@TestNo", SqlDbType.SmallInt, 4, "TestNo");
                        Cmd.Parameters.Add("@ValFrom", SqlDbType.Money, 8, "ValFrom");
                        Cmd.Parameters.Add("@ValTo", SqlDbType.Money, 8, "ValTo");
                        Cmd.Parameters.Add("@QCVal", SqlDbType.Money, 8, "QCVal");
                        Cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 100, "Notes");
                        Cmd.Parameters.Add("@GeneralQcNotes", SqlDbType.VarChar, 100).Value = Notes;
                        Cmd.Parameters.Add("@QCPassed", SqlDbType.Bit, 1, "QCPassed");
                        //Cmd.Parameters.Add("@QCClosed", SqlDbType.Bit, 1).Value = FixTest;
                        Cmd.Transaction = myTrans;
                        Da.InsertCommand = Cmd;
                    }

                    Da.Update(TmpDt);
                }
                //if (FixTest == true)
                //{
                //    if ((string)Session["language"] == "ar-JO")
                //    {
                //        TrDesc = Conversions.ToString(Conversions.ToString("<Êã ÇÛáÇÞ ÇáÝÍÕ ÇáÎÇÕ ÈÇáãÑÍáÉ  >   " + "ProdCost.SysVar.gStageDesc" + "  <ÈäÊíÌÉ > " + Interaction.IIf(OptSuccess == true, " äÇÌÍ", " ÛíÑ äÇÌÍ") + " <ãä ÇãÑ ÊÌåíÒ ÑÞã >   ") + "ProdCost.SysVar.gWorkOrderNo" + "   <ááãÇÏÉ ÇáÌÇåÒÉ>   " + "ProdCost.PInterface.gItemDesc");
                //    }
                //    else
                //    {
                //        TrDesc = Conversions.ToString(Conversions.ToString("The inspection of production stage:  " + "ProdCost.SysVar.gStageDesc" + "  are Closed With final Result" + Interaction.IIf(OptSuccess == true, "Passed", "Not Passed") + "   For Production Order No: ") + "ProdCost.SysVar.gWorkOrderNo" + "  Production Item: " + "ProdCost.PInterface.gItemDesc");
                //    }
                //    AddWorkFlowLog(cn, 98, OrderYear, OrderNo, Conversions.ToInteger(Interaction.IIf(TransType == 1, MaxQCTrNo1, QCTrNo)), 1, 1, 0.1, TrDesc, myTrans);
                //}
                myTrans.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ok = 4 }, JsonRequestBehavior.AllowGet);
        }
        public int GetMaxQCTrNo(SqlTransaction MyTrans, SqlConnection cn, long RefNo)
        {
            int GetMaxQCTrNoRet = 0;
            var Cmd = new SqlCommand();
            SqlDataReader DRead;
            if (!Information.IsNothing(MyTrans))
            {
                Cmd.Transaction = MyTrans;
            }
            Cmd.Connection = cn;
            Cmd.CommandText = "SELECT isnull(MAX(QCTrNo),0)+1  as MaxQCTrNo FROM ProdCost_QCInspDF WHERE (RefNo = @RefNo)";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.BigInt)).Value = RefNo;
            DRead = Cmd.ExecuteReader();
            while (DRead.Read())
            {
                if (!Information.IsNothing(MyTrans))
                {
                    GetMaxQCTrNoRet = Conversions.ToInteger(DRead["MaxQCTrNo"]);
                }
                else
                {
                    GetMaxQCTrNoRet = Conversions.ToInteger(DRead["MaxQCTrNo"]) - 1;
                }
            }
            DRead.Close();
            return GetMaxQCTrNoRet;
        }
    }
}