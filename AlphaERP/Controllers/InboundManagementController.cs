using CrystalDecisions.Shared;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class InboundManagementController : controller
    {
        // GET: InboundManagement
        public ActionResult Index()
        {
            Session["FilesInboundManagement"] = null;
            return View();
        }
        public string CreateReportUniqueId(string path)
        {
            int index1 = path.LastIndexOf('\\');
            int index2 = path.LastIndexOf('.');
            return path.Substring(index1 + 1, index2 - index1 - 1);
        }
        [HttpPost]
        public ActionResult GenerateReport(int OrdYear, int OrderNo, string TawreedNo, string InboundGRN,string InboundSer)
        {
            ReportInformation reportInfo = new ReportInformation();
            DataSet Alpha_ERP_DataSet = new DataSet();
            DataTable d1 = Ord_RptOrdReceipts(OrdYear, OrderNo, TawreedNo, InboundGRN, InboundSer);
            d1.TableName = "Ord_RptOrdReceipts";
            Alpha_ERP_DataSet.Tables.Add(d1);

            DataTable d2 = Ord_GetRptCust();
            d2.TableName = "MyTable";
            Alpha_ERP_DataSet.Tables.Add(d2);

            DataTable d3 = Comp_FillImage();
            var img = d3.Rows[0]["comp_logo"];
            d3.TableName = "Comp_FillImage";
            Alpha_ERP_DataSet.Tables.Add(d3);

            reportInfo.myDataSet = Alpha_ERP_DataSet;
            Ord_InboundManagementHF InboundManagementHF = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();
            reportInfo.fields = SetParameterField(d2, InboundManagementHF.InboundGRNNotes);

            reportInfo.ReportName = "محضر استلام بضاعة";
            if (Language == "ar-JO")
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptOrdReceipts.rpt");
            }
            else
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptOrdReceiptsEng.rpt");
            }

            string reportUniqueName = CreateReportUniqueId(reportInfo.path);
            ReportInfoManager.AddReport(reportUniqueName, reportInfo);

            this.HttpContext.Session["reportInfoID"] = reportInfo.Id;
            return RedirectToAction("ReportViewer", "CrystalReportViewer", new { area = "", id = reportInfo.Id });
        }
        public ParameterFields SetParameterField(DataTable d7,string Notes)
        {
            ClientsActive c = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            ParameterFields parameterFields = new ParameterFields();
            /****************************************************/

            /******************Param1*****************/
            ParameterField param1 = new ParameterField();
            param1.Name = "Para1";
            ParameterDiscreteValue discreteVal1 = new ParameterDiscreteValue();
            if (d7.Rows[0]["desc"] == DBNull.Value)
            {
                discreteVal1.Value = "";
            }
            else
            {
                discreteVal1.Value = d7.Rows[0]["desc"];
            }
            param1.CurrentValues.Add(discreteVal1);
            parameterFields.Add(param1);
            /******************Param2*****************/
            ParameterField param2 = new ParameterField();
            param2.Name = "para2";
            ParameterDiscreteValue discreteVal2 = new ParameterDiscreteValue();
            if (d7.Rows[1]["desc"] == DBNull.Value)
            {
                discreteVal2.Value = "";
            }
            else
            {
                discreteVal2.Value = d7.Rows[1]["desc"];
            }
            param2.CurrentValues.Add(discreteVal2);
            parameterFields.Add(param2);
            /******************Param3*****************/
            ParameterField param3 = new ParameterField();
            param3.Name = "para3";
            ParameterDiscreteValue discreteVal3 = new ParameterDiscreteValue();
            if (d7.Rows[2]["desc"] == DBNull.Value)
            {
                discreteVal3.Value = "";
            }
            else
            {
                discreteVal3.Value = d7.Rows[2]["desc"];
            }
            param3.CurrentValues.Add(discreteVal3);
            parameterFields.Add(param3);
            /******************Param4*****************/
            ParameterField param4 = new ParameterField();
            param4.Name = "para4";
            ParameterDiscreteValue discreteVal4 = new ParameterDiscreteValue();
            if (d7.Rows[3]["desc"] == DBNull.Value)
            {
                discreteVal4.Value = "";
            }
            else
            {
                discreteVal4.Value = d7.Rows[3]["desc"];
            }
            param4.CurrentValues.Add(discreteVal4);
            parameterFields.Add(param4);
            /******************Param5*****************/
            ParameterField param5 = new ParameterField();
            param5.Name = "para5";
            ParameterDiscreteValue discreteVal5 = new ParameterDiscreteValue();
            if (d7.Rows[4]["desc"] == DBNull.Value)
            {
                discreteVal5.Value = "";
            }
            else
            {
                discreteVal5.Value = d7.Rows[4]["desc"];
            }
            param5.CurrentValues.Add(discreteVal5);
            parameterFields.Add(param5);
            /******************UserName*****************/
            ParameterField UserName = new ParameterField();
            UserName.Name = "UserName";
            ParameterDiscreteValue discreteVal8 = new ParameterDiscreteValue();
            discreteVal8.Value = me.UserID;
            UserName.CurrentValues.Add(discreteVal8);
            parameterFields.Add(UserName);
            /******************CompName*****************/
            ParameterField CompName = new ParameterField();
            CompName.Name = "CompName";
            ParameterDiscreteValue discreteVal9 = new ParameterDiscreteValue();
            discreteVal9.Value = company.comp_name;
            CompName.CurrentValues.Add(discreteVal9);
            parameterFields.Add(CompName);
            /******************ShowCost*****************/
            ParameterField ShowCost = new ParameterField();
            ShowCost.Name = "ShowCost";
            ParameterDiscreteValue discreteVal10 = new ParameterDiscreteValue();
            discreteVal10.Value = false;
            ShowCost.CurrentValues.Add(discreteVal10);
            parameterFields.Add(ShowCost);
            /******************ShowPoQty*****************/
            ParameterField ShowPoQty = new ParameterField();
            ShowPoQty.Name = "ShowPoQty";
            ParameterDiscreteValue discreteVal11 = new ParameterDiscreteValue();
            discreteVal11.Value = true;
            ShowPoQty.CurrentValues.Add(discreteVal11);
            parameterFields.Add(ShowPoQty);
            /******************gClientNo*****************/
            ParameterField GceClient = new ParameterField();
            GceClient.Name = "GceClient";
            ParameterDiscreteValue discreteValgClientNo = new ParameterDiscreteValue();
            discreteValgClientNo.Value = c.ClientNo;
            GceClient.CurrentValues.Add(discreteValgClientNo);
            parameterFields.Add(GceClient);
            /******************gClientNo*****************/
            ParameterField PNotes = new ParameterField();
            PNotes.Name = "Notes";
            ParameterDiscreteValue discreteValgNotes = new ParameterDiscreteValue();
            if(Notes == null)
            {
                discreteValgNotes.Value = "";
            }else
            {
                discreteValgNotes.Value = Notes;
            }
            PNotes.CurrentValues.Add(discreteValgNotes);
            parameterFields.Add(PNotes);
            /************************/
            return parameterFields;
        }

        public DataTable Ord_RptOrdReceipts(int OrdYear, int OrderNo, string TawreedNo,string InboundGRN, string InboundSer)
        {

            Ord_InboundManagementHF InboundManagementHF = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();

            OrdRecHF ordRecHf = db.OrdRecHFs.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundYear == InboundManagementHF.InboundGRNDate.Value.Year && x.InboundNo == InboundGRN && x.InboundSer == InboundSer).FirstOrDefault();
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptOrdReceipts";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TawreedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@RecNo", System.Data.SqlDbType.Int)).Value = ordRecHf.RecNo;
                    cmd.Parameters.Add(new SqlParameter("@RecYear", System.Data.SqlDbType.SmallInt)).Value = ordRecHf.RecYear;
                    if (Language == "ar-JO")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 2;
                    }

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.SelectCommand = cmd;
                        DA.Fill(dataTable);
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return dataTable;
        }
        public DataTable Ord_GetRptCust()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_GetRptCust";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@RptType", System.Data.SqlDbType.SmallInt)).Value = 4;
                    if (Language == "ar-JO")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 2;
                    }

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.SelectCommand = cmd;
                        DA.Fill(dataTable);
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return dataTable;
        }
        public DataTable Comp_FillImage()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Comp_FillImage";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    if (Language == "ar-JO")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", System.Data.SqlDbType.SmallInt)).Value = 2;
                    }

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.SelectCommand = cmd;
                        DA.Fill(dataTable);
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return dataTable;
        }
        public ActionResult InboundsInfoList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            List<LInboundsInfo> InboundsInfo = new List<LInboundsInfo>();

            InboundsInfo = new MDB().Database.SqlQuery<LInboundsInfo>(string.Format("SELECT Ord_InboundsInfoHF.CompNo, OrderYear as OrdYear,Ord_InboundsInfoHF.OrderNo,Ord_InboundsInfoHF.TawreedNo, InboundSer, InboundDate,Isnull((select Top 1 ReqNo from Ord_RequestDF where Ord_RequestDF.CompNo = Ord_InboundsInfoHF.CompNo   " +
              "And Ord_RequestDF.PurchaseOrderYear = Ord_InboundsInfoHF.OrderYear AND Ord_RequestDF.PurchaseOrderNo = Ord_InboundsInfoHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = Ord_InboundsInfoHF.TawreedNo ),0) as ReqNo,OrderHF.VendorNo,Vendors.Name as VenName,Vendors.Eng_Name as VenName_Eng,  " +
              "Ord_InboundsInfoHF.InboundStoreNo as StoreNo,InvStoresMF.StoreName,InvStoresMF.StoreNameEng,InboundNotes, IsApproval, IsClosed, Ord_InboundsInfoHF.ReqStatus,Isnull(Ord_InboundsInfoHF.RecStatus,0) as RecStatus,  " +
              "case when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 0 then 'غير مستلم' when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 1 then 'مستلم جزئي' else 'مستلم كلي' end as RecStatusDescAr,case when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 0 then 'Not Rec.' when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 1 then 'Partially Rec.' else 'Completely Rec' end as RecStatusDescEn, " +
              "case when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 0 then 'lightskyblue' when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 1 then 'lightskyblue' when Isnull(Ord_InboundsInfoHF.RecStatus,0) = 2 then 'lightgreen' else 'lightskyblue' end as colorName  FROM  Ord_InboundsInfoHF INNER JOIN OrderHF on OrderHF.CompNo = Ord_InboundsInfoHF.CompNo AND OrderHF.OrdYear = Ord_InboundsInfoHF.OrderYear  " +
              "AND OrderHF.OrderNo = Ord_InboundsInfoHF.OrderNo AND OrderHF.TawreedNo = Ord_InboundsInfoHF.TawreedNo inner join Vendors on Vendors.comp = OrderHF.CompNo AND Vendors.VendorNo = OrderHF.VendorNo inner join InvStoresMF on InvStoresMF.CompNo = OrderHF.CompNo AND InvStoresMF.StoreNo = Ord_InboundsInfoHF.InboundStoreNo  " +
              "WHERE (Ord_InboundsInfoHF.CompNo = '{0}') AND (year(Ord_InboundsInfoHF.InboundDate) = '{1}') AND (InboundStoreNo In (select StoreNo from InvStoreUsers where (CompNo = '{0}') and (UserID = '{2}') and (InvStoreUsers.AllPerm = 1))) ", company.comp_num, OrdYear,me.UserID)).ToList();

            return PartialView(InboundsInfo);
        }
        public ActionResult DetailsInboundsManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            return PartialView();
        }
        public ActionResult AttachInboundsManagement(int OrdYear, string OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchInboundsManagement(OrdYear, OrderNo, TawreedNo, InboundSer, InboundGRN, transaction, cn);
            }

            ViewBag.Arch = DSArch;

            return PartialView();
        }
        public ActionResult AddInboundsManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            string CountSrl = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer).Max(o => o.InboundGRN);
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            string srl = InboundSer.Replace(TawreedNo, "");
            srl = srl.Replace("/", "");
            srl = srl.TrimStart();
            ViewBag.srl = TawreedNo + srl;

            if (CountSrl == null)
            {
                ViewBag.InboundGRN = "1";
            }
            else
            {
                int serl = Convert.ToInt32(CountSrl) + 1;
                ViewBag.InboundGRN = serl.ToString();
            }

            return View();
        }
        public DataSet LoadArchInboundsManagement(int OrdYear, string OrderNo, string TawreedNo, string InboundSer, string InboundGRN, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetInboundManagementArchiveInfo";
                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = TawreedNo;
                cmd.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = InboundSer;
                cmd.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = InboundGRN;

                cmd.Transaction = MyTrans;
                try
                {
                    DaCmd.SelectCommand = cmd;
                    DaCmd.Fill(DsArch, "Arch");
                }
                catch (Exception ex)
                {
                    DsArch = new DataSet();
                }
            }
            return DsArch;
        }

        public ActionResult EditInboundsManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            string srl = InboundSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", "");
            srl = srl.TrimStart();
            ViewBag.srl = (TawreedNo + 1) + InboundGRN;

            List<UploadFileInboundManagement> lstFile = null;
            if (Session["FilesInboundManagement"] == null)
            {
                lstFile = new List<UploadFileInboundManagement>();
            }
            else
            {
                lstFile = (List<UploadFileInboundManagement>)Session["FilesInboundManagement"];
            }
            UploadFileInboundManagement SinglFile = new UploadFileInboundManagement();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchInboundsManagement(OrdYear, OrderNo.ToString(), TawreedNo, InboundSer, InboundGRN, transaction, cn);
            }

            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == DrArch["OrderNo"].ToString()
                    && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.InboundSer == DrArch["InboundSer"].ToString() && a.InboundGRN == DrArch["InboundGRN"].ToString() && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileInboundManagement();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = DrArch["OrderNo"].ToString();
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.InboundSer = DrArch["InboundSer"].ToString();
                        SinglFile.InboundGRN = DrArch["InboundGRN"].ToString();
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                        lstFile.Add(SinglFile);
                    }
                    else
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = DrArch["OrderNo"].ToString();
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.InboundSer = DrArch["InboundSer"].ToString();
                        SinglFile.InboundGRN = DrArch["InboundGRN"].ToString();
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesInboundManagement"] = lstFile;
                }
            }

            return View();
        }
        public ActionResult ViewInboundManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            string srl = InboundSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", "");
            srl = srl.TrimStart();
            ViewBag.srl = (TawreedNo + 1) + InboundGRN;
            return View();
        }
        public bool ChkStore(int StoreNo, string ItemNo)
        {
            int bStore = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    SqlTransaction transaction;
                    cn.Open();

                    transaction = cn.BeginTransaction();

                    Cmd.Connection = cn;
                    Cmd.CommandText = "InvT_ChkExistsStore";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@StoreNo", SqlDbType.Int).Value = StoreNo;
                    Cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = ItemNo;
                    Cmd.Transaction = transaction;
                    try
                    {
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                bStore = bStore + 1;
                            }
                        }
                        rdr.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        bStore = 0;
                    }
                }
            }

            if (bStore == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public double GetInboundGRNQty(int OrderYear, int OrderNo, string TawreedNo, string InboundGRN, string InboundSer, string ItemNo, int FrmStats)
        {
            int bGRNQty = 0;
            double InboundGRNQty = 0;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    SqlTransaction transaction;
                    cn.Open();

                    transaction = cn.BeginTransaction();

                    Cmd.Connection = cn;
                    Cmd.CommandText = "Ord_GetInboundGRNQty";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@OrderYear", SqlDbType.Int).Value = OrderYear;
                    Cmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    Cmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    Cmd.Parameters.Add("@InboundSer", SqlDbType.VarChar).Value = InboundSer;
                    Cmd.Parameters.Add("@InboundGRN", SqlDbType.VarChar).Value = InboundGRN;
                    Cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = ItemNo;
                    Cmd.Parameters.Add("@FrmStat", SqlDbType.Int).Value = FrmStats;
                    Cmd.Transaction = transaction;
                    try
                    {
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                bGRNQty = bGRNQty + 1;
                                InboundGRNQty = Convert.ToDouble(rdr["InboundGRNQty"]);
                            }
                        }
                        rdr.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        bGRNQty = 0;
                    }
                }
            }

            if (bGRNQty == 0)
            {
                InboundGRNQty = 0;
            }

            return InboundGRNQty;
        }
        public JsonResult Save_InboundManagement(Ord_InboundManagementHF InboundManagementHF, List<Ord_InboundManagementDF> InboundManagementDF, List<UploadFileInboundManagement> FileArchive)
        {
            foreach (Ord_InboundManagementDF item in InboundManagementDF)
            {
                bool ChkStores = ChkStore(InboundManagementHF.InboundGRNStoreNo.Value, item.ItemNo);
                if (ChkStores == false)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = " المادة " + item.ItemNo + " غير معرفة في المستودع " }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "Item " + item.ItemNo + " Is not defined in the store" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            Ord_UserActions UserAction = db.Ord_UserActions.Where(x => x.CompNo == company.comp_num && x.UserID == me.UserID).FirstOrDefault();

            if(UserAction != null)
            {
                if (UserAction.ChckExtraQtyReceipt == false)
                {
                    Ord_InboundManagementHF ex = new Ord_InboundManagementHF();
                    ex.CompNo = InboundManagementHF.CompNo;
                    ex.OrderYear = InboundManagementHF.OrderYear;
                    ex.OrderNo = InboundManagementHF.OrderNo;
                    ex.TawreedNo = InboundManagementHF.TawreedNo;
                    ex.InboundSer = InboundManagementHF.InboundSer;
                    ex.InboundGRN = InboundManagementHF.InboundGRN;
                    ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                    ex.InboundGRNStoreNo = InboundManagementHF.InboundGRNStoreNo;
                    ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                    ex.InvNo = InboundManagementHF.InvNo;
                    ex.IsApproval = false;
                    ex.IsMarketing = InboundManagementHF.IsMarketing;
                    ex.Istqm = InboundManagementHF.Istqm;
                    ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                    ex.IsProduction = InboundManagementHF.IsProduction;
                    ex.IsApprovalQty = true;
                    db.Ord_InboundManagementHF.Add(ex);
                    db.SaveChanges();

                    foreach (Ord_InboundManagementDF item in InboundManagementDF)
                    {
                        Ord_InboundManagementDF ex2 = new Ord_InboundManagementDF();
                        ex2.CompNo = item.CompNo;
                        ex2.OrderYear = InboundManagementHF.OrderYear;
                        ex2.OrderNo = InboundManagementHF.OrderNo;
                        ex2.TawreedNo = InboundManagementHF.TawreedNo;
                        ex2.InboundSer = item.InboundSer;
                        ex2.InboundGRN = item.InboundGRN;
                        ex2.ItemSrl = item.ItemSrl;
                        ex2.ItemNo = item.ItemNo;
                        ex2.batchNo = item.batchNo;
                        ex2.ManfDate = item.ManfDate;
                        ex2.EndDate = item.EndDate;
                        ex2.Qty = item.Qty;
                        ex2.BounsQty = item.BounsQty;
                        ex2.TUnit = item.TUnit;
                        ex2.UnitSerial = item.UnitSerial;
                        ex2.Qty2 = item.Qty2;
                        ex2.InboundGRNQty = item.InboundGRNQty;
                        ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                        ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                        ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                        ex2.InboundGRNQty2 = item.InboundGRNQty2;
                        ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                        db.Ord_InboundManagementDF.Add(ex2);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                Ord_InboundManagementHF ex = new Ord_InboundManagementHF();
                ex.CompNo = InboundManagementHF.CompNo;
                ex.OrderYear = InboundManagementHF.OrderYear;
                ex.OrderNo = InboundManagementHF.OrderNo;
                ex.TawreedNo = InboundManagementHF.TawreedNo;
                ex.InboundSer = InboundManagementHF.InboundSer;
                ex.InboundGRN = InboundManagementHF.InboundGRN;
                ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                ex.InboundGRNStoreNo = InboundManagementHF.InboundGRNStoreNo;
                ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                ex.InvNo = InboundManagementHF.InvNo;
                ex.IsApproval = false;
                ex.IsMarketing = InboundManagementHF.IsMarketing;
                ex.Istqm = InboundManagementHF.Istqm;
                ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                ex.IsProduction = InboundManagementHF.IsProduction;
                ex.IsApprovalQty = true;
                db.Ord_InboundManagementHF.Add(ex);
                db.SaveChanges();

                foreach (Ord_InboundManagementDF item in InboundManagementDF)
                {
                    Ord_InboundManagementDF ex2 = new Ord_InboundManagementDF();
                    ex2.CompNo = item.CompNo;
                    ex2.OrderYear = InboundManagementHF.OrderYear;
                    ex2.OrderNo = InboundManagementHF.OrderNo;
                    ex2.TawreedNo = InboundManagementHF.TawreedNo;
                    ex2.InboundSer = item.InboundSer;
                    ex2.InboundGRN = item.InboundGRN;
                    ex2.ItemSrl = item.ItemSrl;
                    ex2.ItemNo = item.ItemNo;
                    ex2.batchNo = item.batchNo;
                    ex2.ManfDate = item.ManfDate;
                    ex2.EndDate = item.EndDate;
                    ex2.Qty = item.Qty;
                    ex2.BounsQty = item.BounsQty;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.Qty2 = item.Qty2;
                    ex2.InboundGRNQty = item.InboundGRNQty;
                    ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                    ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                    ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                    ex2.InboundGRNQty2 = item.InboundGRNQty2;
                    ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                    db.Ord_InboundManagementDF.Add(ex2);
                    db.SaveChanges();
                }
            }
            

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                List<UploadFileInboundManagement> upFile = (List<UploadFileInboundManagement>)Session["FilesInboundManagement"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if (FileArchive != null)
                        {
                            foreach (UploadFileInboundManagement item in FileArchive)
                            {
                                UploadFileInboundManagement fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
                                && x.TawreedNo == item.TawreedNo && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN && x.FileId == item.FileId).FirstOrDefault();

                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddInboundManagementArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = item.OrderNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = item.InboundSer;
                                    CmdFiles.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = item.InboundGRN;

                                    if (item.Description == null)
                                    {
                                        item.Description = "";
                                    }
                                    CmdFiles.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = item.Description;
                                    CmdFiles.Parameters.Add("@DateUploded", SqlDbType.SmallDateTime).Value = item.DateUploded;
                                    CmdFiles.Parameters.Add("@FileName", SqlDbType.VarChar).Value = fl.FileName;
                                    CmdFiles.Parameters.Add("@FileSize", SqlDbType.Int, 8).Value = fl.FileSize;
                                    CmdFiles.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = item.ContentType;
                                    CmdFiles.Parameters.Add("@ArchiveData", SqlDbType.Image).Value = fl.File;
                                    CmdFiles.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                                    CmdFiles.Transaction = transaction;
                                    try
                                    {
                                        CmdFiles.ExecuteNonQuery();
                                    }
                                    catch (Exception exc)
                                    {
                                        transaction.Rollback();
                                        cn.Dispose();
                                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            if(UserAction != null)
            {
                if (UserAction.ChckExtraQtyReceipt == true)
                {
                    bool bsave = false;
                    foreach (Ord_InboundManagementDF InboundManagementitem in InboundManagementDF)
                    {
                        double TotalQty = 0;
                        double InboundTotalQty = 0;
                        double InboundGRNQty = 0;

                        double Qty = 0;
                        TotalQty = db.OrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.ItemNo == InboundManagementitem.ItemNo).Sum(o => o.Qty).Value;
                        InboundTotalQty = InboundManagementitem.InboundGRNQty.Value;
                        InboundGRNQty = GetInboundGRNQty(InboundManagementHF.OrderYear, InboundManagementHF.OrderNo, InboundManagementHF.TawreedNo, InboundManagementHF.InboundGRN, InboundManagementHF.InboundSer, InboundManagementitem.ItemNo, 1);
                        InboundTotalQty = InboundGRNQty + InboundTotalQty;
                        Qty = (UserAction.ExtraReceivedPer.Value * TotalQty) / 100;
                        TotalQty = Qty + TotalQty;
                        if (InboundTotalQty > TotalQty)
                        {
                            if (bsave == false)
                            {
                                Ord_InboundManagementHF ex = new Ord_InboundManagementHF();
                                ex.CompNo = InboundManagementHF.CompNo;
                                ex.OrderYear = InboundManagementHF.OrderYear;
                                ex.OrderNo = InboundManagementHF.OrderNo;
                                ex.TawreedNo = InboundManagementHF.TawreedNo;
                                ex.InboundSer = InboundManagementHF.InboundSer;
                                ex.InboundGRN = InboundManagementHF.InboundGRN;
                                ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                                ex.InboundGRNStoreNo = InboundManagementHF.InboundGRNStoreNo;
                                ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                                ex.InvNo = InboundManagementHF.InvNo;
                                ex.IsApproval = false;
                                ex.IsMarketing = InboundManagementHF.IsMarketing;
                                ex.Istqm = InboundManagementHF.Istqm;
                                ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                                ex.IsProduction = InboundManagementHF.IsProduction;
                                ex.IsApprovalQty = false;
                                db.Ord_InboundManagementHF.Add(ex);
                                db.SaveChanges();
                                bsave = true;
                            }
                            else
                            {
                                Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                                && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

                                if (ex != null)
                                {
                                    ex.IsApprovalQty = false;
                                    db.SaveChanges();
                                }
                            }

                            List<Ord_InboundManagementHF> lex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                                && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo).ToList();

                            foreach (Ord_InboundManagementHF InboundManagitem in lex)
                            {
                                InboundManagitem.IsApprovalQty = false;
                                db.SaveChanges();
                            }

                            Ord_InboundManagementDF ex2 = new Ord_InboundManagementDF();
                            ex2.CompNo = InboundManagementitem.CompNo;
                            ex2.OrderYear = InboundManagementHF.OrderYear;
                            ex2.OrderNo = InboundManagementHF.OrderNo;
                            ex2.TawreedNo = InboundManagementHF.TawreedNo;
                            ex2.InboundSer = InboundManagementitem.InboundSer;
                            ex2.InboundGRN = InboundManagementitem.InboundGRN;
                            ex2.ItemSrl = InboundManagementitem.ItemSrl;
                            ex2.ItemNo = InboundManagementitem.ItemNo;
                            ex2.batchNo = InboundManagementitem.batchNo;
                            ex2.ManfDate = InboundManagementitem.ManfDate;
                            ex2.EndDate = InboundManagementitem.EndDate;
                            ex2.Qty = InboundManagementitem.Qty;
                            ex2.BounsQty = InboundManagementitem.BounsQty;
                            ex2.TUnit = InboundManagementitem.TUnit;
                            ex2.UnitSerial = InboundManagementitem.UnitSerial;
                            ex2.Qty2 = InboundManagementitem.Qty2;
                            ex2.InboundGRNQty = InboundManagementitem.InboundGRNQty;
                            ex2.InboundGRNBounsQty = InboundManagementitem.InboundGRNBounsQty;
                            ex2.InboundGRNTUnit = InboundManagementitem.InboundGRNTUnit;
                            ex2.InboundGRNUnitSerial = InboundManagementitem.InboundGRNUnitSerial;
                            ex2.InboundGRNQty2 = InboundManagementitem.InboundGRNQty2;
                            ex2.ReqDeliveryDate = InboundManagementitem.ReqDeliveryDate;
                            db.Ord_InboundManagementDF.Add(ex2);
                            db.SaveChanges();

                            using (SqlConnection cn = new SqlConnection(ConnectionString()))
                            {
                                SqlTransaction transaction;
                                cn.Open();

                                transaction = cn.BeginTransaction();

                                string WFTxt = "";
                                if (Language == "ar-JO")
                                {
                                    WFTxt = " تعديل كمية المادة  " + InboundManagementitem.ItemNo + "  - " + TotalQty + " من رقم طلب الشراء  " + InboundManagementHF.OrderNo + " ورقم امر الشراء  " + InboundManagementHF.TawreedNo + " من قبل المستخدم " + me.UserID;
                                }
                                else
                                {
                                    WFTxt = "Edit the quantity of ItemNo " + InboundManagementitem.ItemNo + "  - " + TotalQty + " From the PR.No.  " + InboundManagementHF.OrderNo + " PO.No.  " + InboundManagementHF.TawreedNo + " by the User : " + me.UserID;
                                }

                                OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == company.comp_num && x.Year == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo).FirstOrDefault();
                                bool InsertWorkFlow = AddWorkFlowLog(26, PreOrderHF.BusUnitID.Value, InboundManagementHF.OrderYear, InboundManagementHF.OrderNo.ToString(), InboundManagementHF.TawreedNo, 1, TotalQty, WFTxt, transaction, cn);
                                if (InsertWorkFlow == false)
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                                transaction.Commit();
                                cn.Dispose();
                            }
                        }
                        else
                        {
                            if (bsave == false)
                            {
                                Ord_InboundManagementHF ex = new Ord_InboundManagementHF();
                                ex.CompNo = InboundManagementHF.CompNo;
                                ex.OrderYear = InboundManagementHF.OrderYear;
                                ex.OrderNo = InboundManagementHF.OrderNo;
                                ex.TawreedNo = InboundManagementHF.TawreedNo;
                                ex.InboundSer = InboundManagementHF.InboundSer;
                                ex.InboundGRN = InboundManagementHF.InboundGRN;
                                ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                                ex.InboundGRNStoreNo = InboundManagementHF.InboundGRNStoreNo;
                                ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                                ex.InvNo = InboundManagementHF.InvNo;
                                ex.IsApproval = false;
                                ex.IsMarketing = InboundManagementHF.IsMarketing;
                                ex.Istqm = InboundManagementHF.Istqm;
                                ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                                ex.IsProduction = InboundManagementHF.IsProduction;
                                ex.IsApprovalQty = true;
                                db.Ord_InboundManagementHF.Add(ex);
                                db.SaveChanges();
                                bsave = true;
                            }

                            Ord_InboundManagementDF ex2 = new Ord_InboundManagementDF();
                            ex2.CompNo = InboundManagementitem.CompNo;
                            ex2.OrderYear = InboundManagementHF.OrderYear;
                            ex2.OrderNo = InboundManagementHF.OrderNo;
                            ex2.TawreedNo = InboundManagementHF.TawreedNo;
                            ex2.InboundSer = InboundManagementitem.InboundSer;
                            ex2.InboundGRN = InboundManagementitem.InboundGRN;
                            ex2.ItemSrl = InboundManagementitem.ItemSrl;
                            ex2.ItemNo = InboundManagementitem.ItemNo;
                            ex2.batchNo = InboundManagementitem.batchNo;
                            ex2.ManfDate = InboundManagementitem.ManfDate;
                            ex2.EndDate = InboundManagementitem.EndDate;
                            ex2.Qty = InboundManagementitem.Qty;
                            ex2.BounsQty = InboundManagementitem.BounsQty;
                            ex2.TUnit = InboundManagementitem.TUnit;
                            ex2.UnitSerial = InboundManagementitem.UnitSerial;
                            ex2.Qty2 = InboundManagementitem.Qty2;
                            ex2.InboundGRNQty = InboundManagementitem.InboundGRNQty;
                            ex2.InboundGRNBounsQty = InboundManagementitem.InboundGRNBounsQty;
                            ex2.InboundGRNTUnit = InboundManagementitem.InboundGRNTUnit;
                            ex2.InboundGRNUnitSerial = InboundManagementitem.InboundGRNUnitSerial;
                            ex2.InboundGRNQty2 = InboundManagementitem.InboundGRNQty2;
                            ex2.ReqDeliveryDate = InboundManagementitem.ReqDeliveryDate;
                            db.Ord_InboundManagementDF.Add(ex2);
                            db.SaveChanges();
                        }
                    }
                }
            }
            

            Ord_InboundsInfoHF InboundsInfoHF = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
            && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer).FirstOrDefault();


            double TotalInboundQty = db.Ord_InboundsInfoDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
            && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer).Sum(o => o.Qty).Value;
            double TotalRecQty = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
            && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer).Sum(o => o.InboundGRNQty).Value;

            if(TotalRecQty >= TotalInboundQty)
            {
                InboundsInfoHF.RecStatus = 2;
            }
            else
            {
                InboundsInfoHF.RecStatus = 1;
            }
            db.SaveChanges();
            return Json(new { TawreedNo = InboundManagementHF.TawreedNo, OrdNo = InboundManagementHF.OrderNo, OrdYear = InboundManagementHF.OrderYear, InboundSer = InboundManagementHF.InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLog(int FID, int BU, int TrYear, string TrNo, string TawreedNo, int FStat, double? TrAmnt, string TrDesc, SqlTransaction MyTrans, SqlConnection co)
        {
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Alpha_AddWorkFlowLog";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = FID;
                Cmd.Parameters.Add("@BU", SqlDbType.SmallInt).Value = BU;
                Cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 10).Value = TrYear;
                Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 10).Value = TrNo;
                Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = TawreedNo;
                Cmd.Parameters.Add("@TrAmount", SqlDbType.Money).Value = TrAmnt;
                Cmd.Parameters.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
                Cmd.CommandTimeout = 999999999;
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public JsonResult Edit_InboundManagement(Ord_InboundManagementHF InboundManagementHF, List<Ord_InboundManagementDF> InboundManagementDF, List<UploadFileInboundManagement> FileArchive)
        {
            foreach (Ord_InboundManagementDF item in InboundManagementDF)
            {
                bool ChkStores = ChkStore(InboundManagementHF.InboundGRNStoreNo.Value, item.ItemNo);
                if (ChkStores == false)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = " المادة " + item.ItemNo + " غير معرفة في المستودع " }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "Item " + item.ItemNo + " Is not defined in the store" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            Ord_InboundManagementHF exDet = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
            && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

            if (exDet.IsApproval == true)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا يمكن التعديل تم ترحيل الحركة من قبل" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "It cannot be modified. The movement has already been posted" }, JsonRequestBehavior.AllowGet);
                }
            }


            Ord_UserActions UserAction = db.Ord_UserActions.Where(x => x.CompNo == company.comp_num && x.UserID == me.UserID).FirstOrDefault();

            if(UserAction != null)
            {
                if (UserAction.ChckExtraQtyReceipt == false)
                {
                    Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                 && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

                    if (ex != null)
                    {
                        ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                        ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                        ex.InvNo = InboundManagementHF.InvNo;
                        ex.IsApproval = false;
                        ex.IsMarketing = InboundManagementHF.IsMarketing;
                        ex.Istqm = InboundManagementHF.Istqm;
                        ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                        ex.IsProduction = InboundManagementHF.IsProduction;
                        ex.IsApprovalQty = true;
                        db.SaveChanges();
                    }

                    foreach (Ord_InboundManagementDF item in InboundManagementDF)
                    {
                        Ord_InboundManagementDF ex2 = db.Ord_InboundManagementDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN
                    && x.ItemNo == item.ItemNo).FirstOrDefault();
                        if (ex2 != null)
                        {
                            ex2.batchNo = item.batchNo;
                            ex2.ManfDate = item.ManfDate;
                            ex2.EndDate = item.EndDate;
                            ex2.InboundGRNQty = item.InboundGRNQty;
                            ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                            ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                            ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                            ex2.InboundGRNQty2 = item.InboundGRNQty2;
                            ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                            db.SaveChanges();
                        }
                        else
                        {
                            Ord_InboundManagementDF ex1 = new Ord_InboundManagementDF();
                            ex1.CompNo = item.CompNo;
                            ex1.OrderYear = InboundManagementHF.OrderYear;
                            ex1.OrderNo = InboundManagementHF.OrderNo;
                            ex1.TawreedNo = InboundManagementHF.TawreedNo;
                            ex1.InboundSer = item.InboundSer;
                            ex1.ItemSrl = item.ItemSrl;
                            ex1.ItemNo = item.ItemNo;
                            ex1.batchNo = item.batchNo;
                            ex1.ManfDate = item.ManfDate;
                            ex1.EndDate = item.EndDate;
                            ex1.Qty = item.Qty;
                            ex1.BounsQty = item.BounsQty;
                            ex1.TUnit = item.TUnit;
                            ex1.UnitSerial = item.UnitSerial;
                            ex1.Qty2 = item.Qty2;
                            ex1.InboundGRNQty = item.InboundGRNQty;
                            ex1.InboundGRNBounsQty = item.InboundGRNBounsQty;
                            ex1.InboundGRNTUnit = item.InboundGRNTUnit;
                            ex1.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                            ex1.InboundGRNQty2 = item.InboundGRNQty2;
                            ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                            db.Ord_InboundManagementDF.Add(ex1);
                            db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

                if (ex != null)
                {
                    ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                    ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                    ex.InvNo = InboundManagementHF.InvNo;
                    ex.IsApproval = false;
                    ex.IsMarketing = InboundManagementHF.IsMarketing;
                    ex.Istqm = InboundManagementHF.Istqm;
                    ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                    ex.IsProduction = InboundManagementHF.IsProduction;
                    ex.IsApprovalQty = true;
                    db.SaveChanges();
                }

                foreach (Ord_InboundManagementDF item in InboundManagementDF)
                {
                    Ord_InboundManagementDF ex2 = db.Ord_InboundManagementDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo  && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN
                && x.ItemNo == item.ItemNo).FirstOrDefault();
                    if (ex2 != null)
                    {
                        ex2.batchNo = item.batchNo;
                        ex2.ManfDate = item.ManfDate;
                        ex2.EndDate = item.EndDate;
                        ex2.InboundGRNQty = item.InboundGRNQty;
                        ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                        ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                        ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                        ex2.InboundGRNQty2 = item.InboundGRNQty2;
                        ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                        db.SaveChanges();
                    }
                    else
                    {
                        Ord_InboundManagementDF ex1 = new Ord_InboundManagementDF();
                        ex1.CompNo = item.CompNo;
                        ex1.OrderYear = InboundManagementHF.OrderYear;
                        ex1.OrderNo = InboundManagementHF.OrderNo;
                        ex1.TawreedNo = InboundManagementHF.TawreedNo;
                        ex1.InboundSer = item.InboundSer;
                        ex1.ItemSrl = item.ItemSrl;
                        ex1.ItemNo = item.ItemNo;
                        ex1.batchNo = item.batchNo;
                        ex1.ManfDate = item.ManfDate;
                        ex1.EndDate = item.EndDate;
                        ex1.Qty = item.Qty;
                        ex1.BounsQty = item.BounsQty;
                        ex1.TUnit = item.TUnit;
                        ex1.UnitSerial = item.UnitSerial;
                        ex1.Qty2 = item.Qty2;
                        ex1.InboundGRNQty = item.InboundGRNQty;
                        ex1.InboundGRNBounsQty = item.InboundGRNBounsQty;
                        ex1.InboundGRNTUnit = item.InboundGRNTUnit;
                        ex1.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                        ex1.InboundGRNQty2 = item.InboundGRNQty2;
                        ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                        db.Ord_InboundManagementDF.Add(ex1);
                        db.SaveChanges();
                    }
                }
            }
            
            if(UserAction != null)
            {
                if (UserAction.ChckExtraQtyReceipt == true)
                {
                    foreach (Ord_InboundManagementDF InboundManagementitem in InboundManagementDF)
                    {
                        double TotalQty = db.OrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.ItemNo == InboundManagementitem.ItemNo).Sum(o => o.Qty).Value;
                        double InboundTotalQty = InboundManagementitem.InboundGRNQty.Value;
                        double InboundGRNQty = GetInboundGRNQty(InboundManagementHF.OrderYear, InboundManagementHF.OrderNo, InboundManagementHF.TawreedNo, InboundManagementHF.InboundGRN, InboundManagementHF.InboundSer, InboundManagementitem.ItemNo, 2);
                        InboundTotalQty = InboundGRNQty + InboundTotalQty;
                        double Qty = (UserAction.ExtraReceivedPer.Value * TotalQty) / 100;
                        TotalQty = Qty + TotalQty;
                        if (InboundTotalQty > TotalQty)
                        {
                            Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                     && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

                            if (ex != null)
                            {
                                ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                                ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                                ex.InvNo = InboundManagementHF.InvNo;
                                ex.IsApproval = false;
                                ex.IsMarketing = InboundManagementHF.IsMarketing;
                                ex.Istqm = InboundManagementHF.Istqm;
                                ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                                ex.IsProduction = InboundManagementHF.IsProduction;
                                ex.IsApprovalQty = false;
                                db.SaveChanges();
                            }

                            foreach (Ord_InboundManagementDF item in InboundManagementDF)
                            {
                                Ord_InboundManagementDF ex2 = db.Ord_InboundManagementDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN
                            && x.ItemNo == item.ItemNo).FirstOrDefault();
                                if (ex2 != null)
                                {
                                    ex2.batchNo = item.batchNo;
                                    ex2.ManfDate = item.ManfDate;
                                    ex2.EndDate = item.EndDate;
                                    ex2.InboundGRNQty = item.InboundGRNQty;
                                    ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                                    ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                                    ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                                    ex2.InboundGRNQty2 = item.InboundGRNQty2;
                                    ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    Ord_InboundManagementDF ex1 = new Ord_InboundManagementDF();
                                    ex1.CompNo = item.CompNo;
                                    ex1.OrderYear = InboundManagementHF.OrderYear;
                                    ex1.OrderNo = InboundManagementHF.OrderNo;
                                    ex1.TawreedNo = InboundManagementHF.TawreedNo;
                                    ex1.InboundSer = item.InboundSer;
                                    ex1.ItemSrl = item.ItemSrl;
                                    ex1.ItemNo = item.ItemNo;
                                    ex1.batchNo = item.batchNo;
                                    ex1.ManfDate = item.ManfDate;
                                    ex1.EndDate = item.EndDate;
                                    ex1.Qty = item.Qty;
                                    ex1.BounsQty = item.BounsQty;
                                    ex1.TUnit = item.TUnit;
                                    ex1.UnitSerial = item.UnitSerial;
                                    ex1.Qty2 = item.Qty2;
                                    ex1.InboundGRNQty = item.InboundGRNQty;
                                    ex1.InboundGRNBounsQty = item.InboundGRNBounsQty;
                                    ex1.InboundGRNTUnit = item.InboundGRNTUnit;
                                    ex1.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                                    ex1.InboundGRNQty2 = item.InboundGRNQty2;
                                    ex1.ReqDeliveryDate = item.ReqDeliveryDate;
                                    db.Ord_InboundManagementDF.Add(ex1);
                                    db.SaveChanges();
                                }
                            }
                            using (SqlConnection cn = new SqlConnection(ConnectionString()))
                            {
                                SqlTransaction transaction;
                                cn.Open();

                                transaction = cn.BeginTransaction();
                                string WFTxt = "";
                                if (Language == "ar-JO")
                                {
                                    WFTxt = " تعديل كمية المادة  " + InboundManagementitem.ItemNo + "  - " + TotalQty + " من رقم طلب الشراء  " + InboundManagementHF.OrderNo + " ورقم امر الشراء  " + InboundManagementHF.TawreedNo + " من قبل المستخدم " + me.UserID;
                                }
                                else
                                {
                                    WFTxt = "Edit the quantity of ItemNo " + InboundManagementitem.ItemNo + "  - " + TotalQty + " From the PR.No.  " + InboundManagementHF.OrderNo + " PO.No.  " + InboundManagementHF.TawreedNo + " by the User : " + me.UserID;
                                }
                                OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == company.comp_num && x.Year == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo).FirstOrDefault();

                                bool InsertWorkFlow = AddWorkFlowLog(26, PreOrderHF.BusUnitID.Value, InboundManagementHF.OrderYear, InboundManagementHF.OrderNo.ToString(), InboundManagementHF.TawreedNo, 2, TotalQty, WFTxt, transaction, cn);
                                if (InsertWorkFlow == false)
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                                transaction.Commit();
                                cn.Dispose();
                            }
                        }
                        else
                        {
                            Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrderYear == InboundManagementHF.OrderYear
                           && x.OrderNo == InboundManagementHF.OrderNo && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer && x.InboundGRN == InboundManagementHF.InboundGRN).FirstOrDefault();

                            if (ex != null)
                            {
                                ex.InboundGRNDate = InboundManagementHF.InboundGRNDate;
                                ex.InboundGRNNotes = InboundManagementHF.InboundGRNNotes;
                                ex.InvNo = InboundManagementHF.InvNo;
                                ex.IsApproval = false;
                                ex.IsMarketing = InboundManagementHF.IsMarketing;
                                ex.Istqm = InboundManagementHF.Istqm;
                                ex.IsQualityControl = InboundManagementHF.IsQualityControl;
                                ex.IsProduction = InboundManagementHF.IsProduction;
                                ex.IsApprovalQty = true;
                                db.SaveChanges();
                            }

                            foreach (Ord_InboundManagementDF item in InboundManagementDF)
                            {
                                Ord_InboundManagementDF ex2 = db.Ord_InboundManagementDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN
                            && x.ItemNo == item.ItemNo).FirstOrDefault();
                                if (ex2 != null)
                                {
                                    ex2.batchNo = item.batchNo;
                                    ex2.ManfDate = item.ManfDate;
                                    ex2.EndDate = item.EndDate;
                                    ex2.InboundGRNQty = item.InboundGRNQty;
                                    ex2.InboundGRNBounsQty = item.InboundGRNBounsQty;
                                    ex2.InboundGRNTUnit = item.InboundGRNTUnit;
                                    ex2.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                                    ex2.InboundGRNQty2 = item.InboundGRNQty2;
                                    ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    Ord_InboundManagementDF ex1 = new Ord_InboundManagementDF();
                                    ex1.CompNo = item.CompNo;
                                    ex1.OrderYear = InboundManagementHF.OrderYear;
                                    ex1.OrderNo = InboundManagementHF.OrderNo;
                                    ex1.TawreedNo = InboundManagementHF.TawreedNo;
                                    ex1.InboundSer = item.InboundSer;
                                    ex1.ItemSrl = item.ItemSrl;
                                    ex1.ItemNo = item.ItemNo;
                                    ex1.batchNo = item.batchNo;
                                    ex1.ManfDate = item.ManfDate;
                                    ex1.EndDate = item.EndDate;
                                    ex1.Qty = item.Qty;
                                    ex1.BounsQty = item.BounsQty;
                                    ex1.TUnit = item.TUnit;
                                    ex1.UnitSerial = item.UnitSerial;
                                    ex1.Qty2 = item.Qty2;
                                    ex1.InboundGRNQty = item.InboundGRNQty;
                                    ex1.InboundGRNBounsQty = item.InboundGRNBounsQty;
                                    ex1.InboundGRNTUnit = item.InboundGRNTUnit;
                                    ex1.InboundGRNUnitSerial = item.InboundGRNUnitSerial;
                                    ex1.InboundGRNQty2 = item.InboundGRNQty2;
                                    ex1.ReqDeliveryDate = item.ReqDeliveryDate;
                                    db.Ord_InboundManagementDF.Add(ex1);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                List<UploadFileInboundManagement> upFile = (List<UploadFileInboundManagement>)Session["FilesInboundManagement"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        using (SqlCommand DelCmd = new SqlCommand())
                        {
                            DelCmd.Connection = cn;
                            DelCmd.CommandText = "Ord_Web_DelInboundManagementArchive";
                            DelCmd.CommandType = CommandType.StoredProcedure;
                            DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                            DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = InboundManagementHF.OrderYear;
                            DelCmd.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = InboundManagementHF.OrderNo;
                            DelCmd.Parameters.Add("@TawreedNo", SqlDbType.NVarChar).Value = InboundManagementHF.TawreedNo;
                            DelCmd.Parameters.Add("@InboundSer", SqlDbType.NVarChar).Value = InboundManagementHF.InboundSer;
                            DelCmd.Parameters.Add("@InboundGRN", SqlDbType.NVarChar).Value = InboundManagementHF.InboundGRN;

                            DelCmd.Transaction = transaction;
                            DelCmd.ExecuteNonQuery();
                        }

                        if (FileArchive != null)
                        {

                            foreach (UploadFileInboundManagement item in FileArchive)
                            {
                                UploadFileInboundManagement fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
                                && x.TawreedNo == item.TawreedNo && x.InboundSer == item.InboundSer && x.InboundGRN == item.InboundGRN && x.FileId == item.FileId).FirstOrDefault();

                                if (fl != null)
                                {
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddInboundManagementArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = item.OrderNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = item.TawreedNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = item.InboundSer;
                                        CmdFiles.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = item.InboundGRN;

                                        if (item.Description == null)
                                        {
                                            item.Description = "";
                                        }
                                        CmdFiles.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = item.Description;
                                        CmdFiles.Parameters.Add("@DateUploded", SqlDbType.SmallDateTime).Value = item.DateUploded;
                                        CmdFiles.Parameters.Add("@FileName", SqlDbType.VarChar).Value = fl.FileName;
                                        CmdFiles.Parameters.Add("@FileSize", SqlDbType.Int, 8).Value = fl.FileSize;
                                        CmdFiles.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = item.ContentType;
                                        CmdFiles.Parameters.Add("@ArchiveData", SqlDbType.Image).Value = fl.File;
                                        CmdFiles.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                                        CmdFiles.Transaction = transaction;
                                        try
                                        {
                                            CmdFiles.ExecuteNonQuery();
                                        }
                                        catch (Exception exc)
                                        {
                                            transaction.Rollback();
                                            cn.Dispose();
                                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            Ord_InboundsInfoHF InboundsInfoHF = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
            && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer).FirstOrDefault();


            double TotalInboundQty = db.Ord_InboundsInfoDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo &&  x.InboundSer == InboundManagementHF.InboundSer).Sum(o => o.Qty).Value;
            double TotalRecQty = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == InboundManagementHF.OrderYear && x.OrderNo == InboundManagementHF.OrderNo
                       && x.TawreedNo == InboundManagementHF.TawreedNo && x.InboundSer == InboundManagementHF.InboundSer).Sum(o => o.InboundGRNQty).Value;

            if (TotalRecQty >= TotalInboundQty)
            {
                InboundsInfoHF.RecStatus = 2;
            }
            else
            {
                InboundsInfoHF.RecStatus = 1;
            }
            db.SaveChanges();

            return Json(new { TawreedNo = InboundManagementHF.TawreedNo, OrdNo = InboundManagementHF.OrderNo, OrdYear = InboundManagementHF.OrderYear, InboundSer = InboundManagementHF.InboundSer, InboundGRN = InboundManagementHF.InboundGRN, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_InboundManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            Ord_InboundManagementHF exDet = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();

            if (exDet.IsApproval == true)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا يمكن الحذف تم ترحيل الحركة من قبل" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "It cannot be Delete. The movement has already been posted" }, JsonRequestBehavior.AllowGet);
                }
            }

            List<Ord_InboundManagementDF> exdel = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
                       && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).ToList();
            db.Ord_InboundManagementDF.RemoveRange(exdel);
            db.SaveChanges();

            Ord_InboundManagementHF ex = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_InboundManagementHF.Remove(ex);
                db.SaveChanges();
            }

            Ord_InboundsInfoHF InboundsInfoHF = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == ex.OrderYear && x.OrderNo == ex.OrderNo
            && x.TawreedNo == ex.TawreedNo && x.InboundSer == ex.InboundSer).FirstOrDefault();

            List<Ord_InboundManagementDF> InboundManagementDF = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == ex.InboundSer).ToList();
            double TotalRecQty = 0;
            double TotalInboundQty = db.Ord_InboundsInfoDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == ex.InboundSer).Sum(o => o.Qty).Value;
            if(InboundManagementDF.Count > 0)
            {
                TotalRecQty = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == ex.InboundSer).Sum(o => o.InboundGRNQty).Value;
            }

            if (TotalRecQty == 0)
            {
                InboundsInfoHF.RecStatus = 0;
            }
            else
            {
                if (TotalRecQty >= TotalInboundQty)
                {
                    InboundsInfoHF.RecStatus = 2;
                }
                else
                {
                    InboundsInfoHF.RecStatus = 1;
                }
            }
            db.SaveChanges();

            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, InboundSer = InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public double GetLastExRate(int CCode, DateTime VDate)
        {
            double CurrRatio = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    SqlTransaction transaction;
                    cn.Open();

                    transaction = cn.BeginTransaction();

                    Cmd.Connection = cn;
                    Cmd.CommandText = "Gln_GetCurrExchRatio";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@CurrCode", SqlDbType.SmallInt).Value = CCode;
                    Cmd.Parameters.Add("@VDate", SqlDbType.SmallDateTime).Value = VDate;
                    Cmd.Transaction = transaction;
                    try
                    {
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                CurrRatio = Convert.ToDouble(rdr["CurrRatio"]);
                            }
                        }
                        rdr.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        CurrRatio = 0;
                    }
                }
            }

            return CurrRatio;
        }
        public DataSet LoadStoreBatchs(string ItemNo, int StoreNo, short UnitSerial, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet Dsbatch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "InvT_LoadStoreBatchs";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = ItemNo;
                Cmd.Parameters.Add("@StoreNo", SqlDbType.Int).Value = StoreNo;
                Cmd.Parameters.Add("@UnitSerial", SqlDbType.TinyInt).Value = UnitSerial;
                Cmd.Parameters.Add("@VouType", SqlDbType.TinyInt).Value = 1;
                Cmd.Transaction = MyTrans;
                try
                {
                    DaCmd.SelectCommand = Cmd;
                    DaCmd.Fill(Dsbatch, "BatchsTbl");
                    Dsbatch.Tables["BatchsTbl"].PrimaryKey = new DataColumn[] { Dsbatch.Tables["BatchsTbl"].Columns["ItemNo"], Dsbatch.Tables["BatchsTbl"].Columns["BatchNo"] };
                }
                catch (Exception ex)
                {
                    Dsbatch = new DataSet();
                }
            }

            return Dsbatch;
        }
        public JsonResult Posted_InboundManagement(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            Ord_InboundManagementHF InboundManagementHF = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();

            if(InboundManagementHF.IsApproval == true)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "تم ترحيل الحركة من قبل" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The movement has been moved by" }, JsonRequestBehavior.AllowGet);
                }
            }



            List<Ord_InboundManagementDF> InboundManagementDF = db.Ord_InboundManagementDF.Where(x => x.CompNo == company.comp_num 
            && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo
            && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).ToList();

            foreach (Ord_InboundManagementDF item in InboundManagementDF)
            {
                if(item.InboundGRNQty <= 0)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "يجب إدخال معلومات محضر الاستلام" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "The receipt information must be entered" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }


            OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == InboundManagementHF.CompNo && x.Year == OrdYear && x.OrderNo == OrderNo).FirstOrDefault();
            OrderHF hf = db.OrderHFs.Where(x => x.CompNo == InboundManagementHF.CompNo && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();
            Vendor Vend = new Vendor();
            int Dept = 0;
            int gLcDept = 0;
            long gLcAcc = 0;

            if (hf.IsLC == false)
            {
                Vend = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == hf.VendorNo).FirstOrDefault();
                Dept = new MDB().Database.SqlQuery<int>(string.Format("select TOP 1 crb_dep as VenderId from GLCRBMF where (CRB_COMP = '{0}') AND (crb_acc = '{1}')", company.comp_num, Vend.VendorNo)).FirstOrDefault();
                if (Dept == 0)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "الحساب غير مربوط مع الدائرة" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "Account is not linked with the department" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                OrdCompMF MF = db.OrdCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();

                ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
                if(CActive.ClientNo == 239)
                {
                    gLcDept = MF.LcDept.Value;
                    gLcAcc = MF.LcAcc.Value;
                }
                else
                {
                    gLcDept = hf.LcDept.Value;
                    gLcAcc = hf.LcAccNo.Value;
                }
                
                

                if (gLcDept == 0)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "يجب إدخال حساب الطلبيه" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "The Order Account Must Be Entered" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (gLcAcc == 0)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "يجب إدخال حساب الطلبيه" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "The Order Account Must Be Entered" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            


            long NewRecNo = 0;
            long VouhNo = 0;
            double OrdConvRate = 0;
            OrdConvRate = GetLastExRate(hf.CurType.Value, DateTime.Now);
             
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet DsTmpOrg = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds4 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            SqlDataAdapter DaPR3 = new SqlDataAdapter();
            SqlDataAdapter DaPR4 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp2;
            DataRow DrTmp3;


            List<OrderDF> dfs = db.OrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).ToList();

            int i = 1;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdRecDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");


                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TmpTbl");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.Invt_ReservedStockDF  WHERE compno = 0", cn);
                DaPR2.Fill(DsTmpOrg, "TmpTbl");
                //DsTmpOrg.Tables["TmpTbl"].PrimaryKey = new DataColumn[] { DsTmpOrg.Tables["TmpTbl"].Columns["ItemNo"], DsTmpOrg.Tables["TmpTbl"].Columns["BatchNo"], DsTmpOrg.Tables["TmpTbl"].Columns["ItemSr"] };

                DaPR3.SelectCommand = new SqlCommand("SELECT * FROM dbo.InvBatchsMF  WHERE compno = 0", cn);
                DaPR3.Fill(ds2, "TmpBatchs");


                transaction = cn.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Ord_AddOrdRecHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@RecYear", SqlDbType.SmallInt)).Value = InboundManagementHF.InboundGRNDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@RecDate", SqlDbType.SmallDateTime)).Value = InboundManagementHF.InboundGRNDate.Value;
                    if(InboundManagementHF.InvNo == null)
                    {
                        InboundManagementHF.InvNo = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@InvNo", SqlDbType.VarChar)).Value = InboundManagementHF.InvNo;
                    cmd.Parameters.Add(new SqlParameter("@InvDate", SqlDbType.SmallDateTime)).Value = InboundManagementHF.InboundGRNDate.Value;

                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = InboundManagementHF.InboundGRNStoreNo;
                    cmd.Parameters.Add(new SqlParameter("@PoApplicant", SqlDbType.Int)).Value = PreOrderHF.OrdOrg;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء محضر استلام من الويب برقم  " + " - " + InboundGRN;

                    cmd.Parameters.Add(new SqlParameter("@FileNo", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@CurrCode", SqlDbType.SmallInt)).Value = hf.CurType;
                    cmd.Parameters.Add(new SqlParameter("@ConvRate", SqlDbType.Float)).Value = OrdConvRate;
                    cmd.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@NewRecNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@InvSR", SqlDbType.SmallInt)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@InboundYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@InboundNo", SqlDbType.NVarChar)).Value = InboundGRN;
                    cmd.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = InboundSer;

                    cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;

                    cmd.Transaction = transaction;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    NewRecNo = Convert.ToInt64(cmd.Parameters["@NewRecNo"].Value);
                    long LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);

                    foreach (Ord_InboundManagementDF item in InboundManagementDF)
                    {
                        OrderDF df = dfs.Where(x => x.ItemNo == item.ItemNo).FirstOrDefault();
                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrderNo"] = OrderNo;
                        DrTmp["OrderYear"] = OrdYear;
                        DrTmp["TawreedNo"] = TawreedNo;
                        DrTmp["RecYear"] = InboundManagementHF.InboundGRNDate.Value.Year;
                        DrTmp["RecNo"] = NewRecNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["ItemSr"] = item.ItemSrl;
                        DrTmp["InvQty"] = item.InboundGRNQty;
                        DrTmp["InvQty2"] = item.InboundGRNQty2;
                        DrTmp["InvBonusQty"] = item.InboundGRNBounsQty;
                        DrTmp["NetAmount"] = item.InboundGRNQty * (df.ordamt / item.InboundGRNQty);
                        DrTmp["Item_Tax_Val"] = df.ordamt * (df.ItemTaxPer / 100);
                        DrTmp["Item_Tax_Per"] = df.ItemTaxPer;
                        DrTmp["ExpDate"] = item.EndDate.Value;
                        DrTmp["ManDate"] = item.ManfDate.Value;
                        DrTmp["Batch"] = item.batchNo;
                        DrTmp["Price"] = 0;
                        DrTmp["Note"] = "";
                        DrTmp["PUnit"] = item.InboundGRNTUnit;
                        DrTmp["UnitSerial"] = item.InboundGRNUnitSerial;
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                        i = i + 1;
                    }
                    using (SqlCommand cmd1 = new SqlCommand())
                    {
                        cmd1.Connection = cn;
                        cmd1.CommandText = "Ord_AddOrdRecDF";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd1.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "OrderYear"));
                        cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 4, "OrderNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd1.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                        cmd1.Parameters.Add(new SqlParameter("@RecYear", SqlDbType.SmallInt, 4, "RecYear"));
                        cmd1.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int, 4, "RecNo"));

                        cmd1.Parameters.Add(new SqlParameter("@InvQty", SqlDbType.Float, 8, "InvQty"));
                        cmd1.Parameters.Add(new SqlParameter("@InvQty2", SqlDbType.Float, 8, "InvQty2"));
                        cmd1.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float, 8, "NetAmount"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 8, "Item_Tax_Val"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 8, "Item_Tax_Per"));
                        cmd1.Parameters.Add(new SqlParameter("@InvBonusQty", SqlDbType.Float, 8, "InvBonusQty"));

                        cmd1.Parameters.Add(new SqlParameter("@Expdate", SqlDbType.SmallDateTime, 8, "Expdate"));
                        cmd1.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 8, "ManDate"));
                        cmd1.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "Batch"));
                        cmd1.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd1.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 8, "Price"));
                        cmd1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd1.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));

                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Transaction = transaction;
                        DaPR.InsertCommand = cmd1;
                        try
                        {
                            DaPR.Update(ds, "TmpTbl");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    using (SqlCommand CmdCond = new SqlCommand())
                    {
                        CmdCond.Connection = cn;
                        CmdCond.CommandText = "Ord_UpdDlvState";
                        CmdCond.CommandType = CommandType.StoredProcedure;
                        CmdCond.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        CmdCond.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                        CmdCond.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                        CmdCond.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                        CmdCond.Transaction = transaction;
                        try
                        {
                            CmdCond.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


                DataSet DSArch = new DataSet();
                DSArch = LoadArch(OrdYear, OrderNo.ToString(), TawreedNo, InboundManagementHF.InboundSer, InboundManagementHF.InboundGRN, transaction, cn);
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    using (SqlCommand cmdArch = new SqlCommand())
                    {
                        cmdArch.Connection = cn;
                        cmdArch.CommandText = "Ord_AddArchiveInfo";
                        cmdArch.CommandType = CommandType.StoredProcedure;
                        cmdArch.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        cmdArch.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                        cmdArch.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                        cmdArch.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = TawreedNo;
                        cmdArch.Parameters.Add(new SqlParameter("@RecYear", SqlDbType.SmallInt)).Value = InboundManagementHF.InboundGRNDate.Value.Year;
                        cmdArch.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = NewRecNo;
                        cmdArch.Parameters.Add(new SqlParameter("@VouType", SqlDbType.Int)).Value = 4;
                        cmdArch.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                        cmdArch.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = DrArch["Description"];
                        cmdArch.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar)).Value = DrArch["FileName"];
                        cmdArch.Parameters.Add(new SqlParameter("@DateUploded", SqlDbType.SmallDateTime)).Value = DrArch["DataUpload"];
                        cmdArch.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.Int)).Value = DrArch["FileSize"];
                        cmdArch.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.VarChar)).Value = DrArch["ContentType"];
                        cmdArch.Parameters.Add(new SqlParameter("@ArchiveData", SqlDbType.Image, 2147483647)).Value = DrArch["ArchiveData"];
                        cmdArch.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        cmdArch.Transaction = transaction;
                        try
                        {
                            cmdArch.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = InboundManagementHF.InboundGRNDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = InboundManagementHF.InboundGRNDate;
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = InboundManagementHF.InboundGRNStoreNo;
                    if (hf.IsLC == false)
                    {
                        cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = Dept;
                        cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = hf.VendorNo;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = gLcDept;
                        cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = gLcAcc;
                    }
                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = InboundManagementHF.InvNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = OrdConvRate;
                    cmd.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = hf.CurType;
                    cmd.Parameters.Add(new SqlParameter("@NetFAmount", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashAcc", SqlDbType.BigInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = NewRecNo;
                    cmd.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء سند استلام من رقم طلب شراء" + " - " + OrderNo + " و رقم امر شراء " + " - " + TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@POTawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecYear", SqlDbType.SmallInt)).Value = OrdYear;

                    cmd.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    cmd.Transaction = transaction;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) != 0)
                    {
                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -10)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "يجب تعريف تسلسل للسند حتى تتم عملية التخزين للسند" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "You Must Define Voucher Serial To Continue With The Save Operation" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -20)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "السند موجود في الملف التاريخي" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "The Voucher No Exists In Historical File" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -70)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "السند موجود في ملف اليومية" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "The Voucher No Exists In Daily File" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    long LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);
                    VouhNo = Convert.ToInt32(cmd.Parameters["@UsedVouNo"].Value);


                    using (SqlCommand CmdOrdeRec = new SqlCommand())
                    {
                        CmdOrdeRec.Connection = cn;
                        CmdOrdeRec.CommandText = "Ord_UpdateOrderRec";
                        CmdOrdeRec.CommandType = CommandType.StoredProcedure;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = NewRecNo;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@PurchRecYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                        CmdOrdeRec.Transaction = transaction;
                        try
                        {
                            CmdOrdeRec.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    i = 1;
                    double UnitCost = 0;
                    double netAmount = 0;

                    foreach (Ord_InboundManagementDF item in InboundManagementDF)
                    {
                        DataSet DsBatchs = new DataSet();
                        DsBatchs = LoadStoreBatchs(item.ItemNo, InboundManagementHF.InboundGRNStoreNo.Value, item.InboundGRNUnitSerial.Value, transaction, cn);
                        DataRow DrBatch;
                        object[] findTheseVals = new object[2];
                        findTheseVals[0] = item.ItemNo;
                        findTheseVals[1] = item.batchNo;

                        DrBatch = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);

                        if (DrBatch == null)
                        {
                            DrTmp3 = ds2.Tables["TmpBatchs"].NewRow();
                            DrTmp3["CompNo"] = company.comp_num;
                            DrTmp3["StoreNo"] = InboundManagementHF.InboundGRNStoreNo.Value;
                            DrTmp3["ItemNo"] = item.ItemNo;
                            DrTmp3["batchNo"] = item.batchNo;
                            DrTmp3["ExpDate"] = item.EndDate.Value.ToShortDateString();
                            DrTmp3["ManDate"] = item.ManfDate.Value.ToShortDateString();
                            DrTmp3["IsHalt"] = 0;
                            DrTmp3["UnitCost"] = 0;
                            DrTmp3["RefNo"] = "";
                            DrTmp3["RefNo2"] = "";
                            DrTmp3["Location"] = "";
                            DrTmp3["SellPrice"] = 0;
                            DrTmp3["BatchSerial"] = item.InboundGRNUnitSerial.Value;
                            ds2.Tables["TmpBatchs"].Rows.Add(DrTmp3);
                        }
                    }
                    if (ds2.Tables["TmpBatchs"].Rows.Count != 0)
                    {
                        using (SqlCommand cmdBatch = new SqlCommand())
                        {
                            cmdBatch.Connection = cn;
                            cmdBatch.CommandText = "Invt_AddBatchs";
                            cmdBatch.CommandType = CommandType.StoredProcedure;
                            cmdBatch.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            cmdBatch.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 9, "StoreNo"));
                            cmdBatch.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                            cmdBatch.Parameters.Add(new SqlParameter("@batchNo", SqlDbType.VarChar, 20, "batchNo"));
                            cmdBatch.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.SmallDateTime, 8, "ExpDate"));
                            cmdBatch.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 8, "ManDate"));

                            cmdBatch.Parameters.Add(new SqlParameter("@IsHalt", SqlDbType.Bit, 8, "IsHalt"));
                            cmdBatch.Parameters.Add(new SqlParameter("@UnitCost", SqlDbType.Float, 8, "UnitCost"));
                            cmdBatch.Parameters.Add(new SqlParameter("@HBatch", SqlDbType.Bit)).Value = 0;

                            cmdBatch.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 70, "RefNo"));
                            cmdBatch.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar, 70, "RefNo2"));
                            cmdBatch.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar, 70, "Location"));
                            cmdBatch.Parameters.Add(new SqlParameter("@SellPrice", SqlDbType.Money, 8, "SellPrice"));
                            cmdBatch.Parameters.Add(new SqlParameter("@BatchSerial", SqlDbType.SmallInt, 4, "BatchSerial"));
                            cmdBatch.Transaction = transaction;
                            DaPR3.InsertCommand = cmdBatch;
                            try
                            {
                                DaPR3.Update(ds2, "TmpBatchs");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    foreach (Ord_InboundManagementDF item in InboundManagementDF)
                    {
                        OrderDF df1 = dfs.Where(x => x.ItemNo == item.ItemNo).FirstOrDefault();
                        
                        double ItemTaxVal = Convert.ToDouble(df1.ItemTaxVal / df1.Qty.Value) * item.InboundGRNQty.Value;
                        DrTmp1 = ds1.Tables["TmpTbl"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["compno"] = company.comp_num;
                        DrTmp1["VouYear"] = InboundManagementHF.InboundGRNDate.Value.Year;
                        DrTmp1["VouType"] = 1;
                        DrTmp1["VouNo"] = VouhNo;
                        DrTmp1["StoreNo"] = InboundManagementHF.InboundGRNStoreNo;
                        DrTmp1["ItemNo"] = item.ItemNo;
                        UnitCost = Convert.ToDouble(((df1.Price.Value * item.InboundGRNQty.Value) - ItemTaxVal) / item.InboundGRNQty.Value);
                        DrTmp1["Batch"] = item.batchNo;
                        DrTmp1["ItemSer"] = i;
                        DrTmp1["VouDate"] = InboundManagementHF.InboundGRNDate;
                        DrTmp1["Qty"] = item.InboundGRNQty;
                        DrTmp1["Qty2"] = item.InboundGRNQty2;
                        DrTmp1["ItemDimension"] = "0*0*0";
                        DrTmp1["Bonus"] = item.InboundGRNBounsQty;
                        DrTmp1["TUnit"] = item.InboundGRNTUnit;
                        DrTmp1["NetSellValue"] = (item.InboundGRNQty * df1.Price.Value ) * OrdConvRate;
                        netAmount = Convert.ToDouble(netAmount + (df1.Price.Value *  item.InboundGRNQty) * (OrdConvRate) + (ItemTaxVal));
                        DrTmp1["Discount"] = 0;
                        DrTmp1["PerDiscount"] = 0;
                        DrTmp1["VouDiscount"] = 0;
                        DrTmp1["CurrNo"] = hf.CurType;
                        if(OrdConvRate != 1)
                        {
                            DrTmp1["ForUCost"] = ((df1.Price.Value * item.InboundGRNQty) - (ItemTaxVal)) / item.InboundGRNQty;
                        }
                        else
                        {
                            DrTmp1["ForUCost"] = 0;
                        }
                        DrTmp1["ToStoreNo"] = 0;
                        DrTmp1["ToBatch"] = 0;
                        DrTmp1["AccDepNo"] = 0;
                        DrTmp1["AccNo"] = 0;
                        DrTmp1["Item_Tax_Per"] = df1.ItemTaxPer;
                        DrTmp1["Item_Tax_Val"] = ItemTaxVal;
                        DrTmp1["Item_STax_Per"] = 0;
                        DrTmp1["Item_STax_Val"] = 0;
                        DrTmp1["Item_Tax_Type"] = 1;
                        DrTmp1["Item_STax_Type"] = 0;
                        DrTmp1["Item_PTax_Per"] = 0;
                        DrTmp1["Item_PTax_Val"] = 0;
                        DrTmp1["Item_PTax_Type"] = 0;

                        DrTmp1["ProdOrderNo"] = 0;
                        DrTmp1["ExRate"] = OrdConvRate;
                        DrTmp1["UnitSerial"] = item.InboundGRNUnitSerial;
                        DrTmp1.EndEdit();
                        ds1.Tables["TmpTbl"].Rows.Add(DrTmp1);
                        i = i + 1;
                    }

                    using (SqlCommand cmd1 = new SqlCommand())
                    {
                        cmd1.Connection = cn;
                        cmd1.CommandText = "InvT_AddInvDailyDF";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd1.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt, 4, "VouYear"));
                        cmd1.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt, 4, "VouType"));
                        cmd1.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int, 9, "VouNo"));
                        cmd1.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 9, "StoreNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd1.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "Batch"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 4, "ItemSer"));
                        cmd1.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime, 8, "VouDate"));
                        cmd1.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd1.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 8, "Qty2"));
                        cmd1.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
                        cmd1.Parameters.Add(new SqlParameter("@TUnit", SqlDbType.VarChar, 5, "TUnit"));
                        cmd1.Parameters.Add(new SqlParameter("@NetSellValue", SqlDbType.Float, 8, "NetSellValue"));
                        cmd1.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Float, 8, "Discount"));
                        cmd1.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 8, "PerDiscount"));
                        cmd1.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Float, 8, "VouDiscount"));
                        cmd1.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt, 4, "CurrNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ForUCost", SqlDbType.Float, 8, "ForUCost"));
                        cmd1.Parameters.Add(new SqlParameter("@ToStoreNo", SqlDbType.Int, 8, "ToStoreNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ToBatch", SqlDbType.VarChar, 20, "ToBatch"));
                        cmd1.Parameters.Add(new SqlParameter("@AccDepNo", SqlDbType.Int, 9, "AccDepNo"));
                        cmd1.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt, 9, "AccNo"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_Tax_Per", SqlDbType.Float, 8, "Item_Tax_Per"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_Tax_Val", SqlDbType.Float, 8, "Item_Tax_Val"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_STax_Per", SqlDbType.Float, 8, "Item_STax_Per"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_STax_Val", SqlDbType.Float, 8, "Item_STax_Val"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_Tax_Type", SqlDbType.Float, 8, "Item_Tax_Type"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_STax_Type", SqlDbType.Float, 8, "Item_STax_Type"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_PTax_Per", SqlDbType.Float, 8, "Item_PTax_Per"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_PTax_Val", SqlDbType.Float, 8, "Item_PTax_Val"));
                        cmd1.Parameters.Add(new SqlParameter("@Item_PTax_Type", SqlDbType.Float, 8, "Item_PTax_Type"));
                        cmd1.Parameters.Add(new SqlParameter("@ProdOrderNo", SqlDbType.Int, 9, "ProdOrderNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ExRate", SqlDbType.Float, 8, "ExRate"));
                        cmd1.Parameters.Add(new SqlParameter("@UnitCodeSr", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Transaction = transaction;
                        DaPR1.InsertCommand = cmd1;
                        try
                        {
                            DaPR1.Update(ds1, "TmpTbl");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    using (SqlCommand CmdOrdeRec = new SqlCommand())
                    {
                        CmdOrdeRec.Connection = cn;
                        CmdOrdeRec.CommandText = "Ord_UpdateOrderHF";
                        CmdOrdeRec.CommandType = CommandType.StoredProcedure;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                        CmdOrdeRec.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;

                        CmdOrdeRec.Transaction = transaction;
                        try
                        {
                            CmdOrdeRec.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
                if(InboundManagementHF.IsMarketing == true || InboundManagementHF.IsQualityControl == true || InboundManagementHF.Istqm == true)
                {
                    long VouNoRes;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "InvT_AddInvReserveHF";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                        cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 21;
                        cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;
                        cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = InboundManagementHF.InboundGRNStoreNo;
                        cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DBNull.Value;
                        cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = DBNull.Value;
                        cmd.Parameters.Add(new SqlParameter("@ReservedYear", SqlDbType.SmallInt)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@ReservedRef", SqlDbType.Int)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                        cmd.Parameters.Add(new SqlParameter("@SalesOrderYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                        cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "";

                        cmd.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.Int)).Value = DBNull.Value;
                        cmd.Parameters.Add(new SqlParameter("@ReservedType", SqlDbType.SmallInt)).Value = 1;
                        cmd.Parameters.Add(new SqlParameter("@ToStoreNo", SqlDbType.Int)).Value = InboundManagementHF.InboundGRNStoreNo;

                        cmd.Parameters.Add(new SqlParameter("@InvTransType", SqlDbType.SmallInt)).Value = 2;

                        cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;

                        cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                        cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                        cmd.Parameters.Add(new SqlParameter("@OrdRecYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                        cmd.Parameters.Add(new SqlParameter("@OrdRecNo", SqlDbType.Int)).Value = NewRecNo;

                        cmd.Transaction = transaction;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }

                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) != 0)
                        {
                            if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -10)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                if (Language == "ar-JO")
                                {
                                    return Json(new { error = "يجب تعريف تسلسل للسند حتى تتم عملية التخزين للسند" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "You Must Define Voucher Serial To Continue With The Save Operation" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -20)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                if (Language == "ar-JO")
                                {
                                    return Json(new { error = "السند موجود في الملف التاريخي" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "The Voucher No Exists In Historical File" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -70)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                if (Language == "ar-JO")
                                {
                                    return Json(new { error = "السند موجود في ملف اليومية" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "The Voucher No Exists In Daily File" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        long LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);
                        VouNoRes = Convert.ToInt32(cmd.Parameters["@UsedVouNo"].Value);

                        i = 1;

                        foreach (Ord_InboundManagementDF item in InboundManagementDF)
                        {
                            DrTmp2 = DsTmpOrg.Tables["TmpTbl"].NewRow();
                            DrTmp2.BeginEdit();
                            DrTmp2["compno"] = company.comp_num;
                            DrTmp2["VouYear"] = DateTime.Now.Year;
                            DrTmp2["VouType"] = 21;
                            DrTmp2["VouNo"] = VouNoRes;
                            DrTmp2["StoreNo"] = InboundManagementHF.InboundGRNStoreNo;
                            DrTmp2["ItemNo"] = item.ItemNo;
                            DrTmp2["BatchNo"] = item.batchNo;
                            DrTmp2["ItemSer"] = i;
                            DrTmp2["VouDate"] = DateTime.Now;
                            DrTmp2["ResUpToDate"] = DateTime.Now;
                            DrTmp2["ResNotes"] = "FG Reserve";
                            DrTmp2["TransferToIssue"] = false;

                            DrTmp2["ResQty"] = item.InboundGRNQty * -1;
                            DrTmp2["ResQty2"] = item.InboundGRNQty2 * -1;
                            DrTmp2["ItemStatus"] = 1;
                            DrTmp2["UnitSerial"] = item.InboundGRNUnitSerial;
                            DrTmp2.EndEdit();
                            DsTmpOrg.Tables["TmpTbl"].Rows.Add(DrTmp2);
                            i = i + 1;
                        }

                        using (SqlCommand cmd1 = new SqlCommand())
                        {
                            cmd1.Connection = cn;
                            cmd1.CommandText = "InvT_AddInvReserveDF";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            cmd1.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt, 4, "VouYear"));
                            cmd1.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt, 4, "VouType"));
                            cmd1.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int, 9, "VouNo"));
                            cmd1.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 9, "StoreNo"));
                            cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                            cmd1.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "BatchNo"));
                            cmd1.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 4, "ItemSer"));
                            cmd1.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime, 8, "VouDate"));
                            cmd1.Parameters.Add(new SqlParameter("@ResQty", SqlDbType.Float, 8, "ResQty"));
                            cmd1.Parameters.Add(new SqlParameter("@ResQty2", SqlDbType.Float, 8, "ResQty2"));
                            cmd1.Parameters.Add(new SqlParameter("@ResUpToDate", SqlDbType.SmallDateTime, 8, "ResUpToDate"));
                            cmd1.Parameters.Add(new SqlParameter("@ResNotes", SqlDbType.VarChar, 225, "ResNotes"));
                            cmd1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                            cmd1.Parameters.Add(new SqlParameter("@TransferToIssue", SqlDbType.Bit, 1, "TransferToIssue"));

                            cmd1.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.BigInt)).Value = 1;
                            cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                            cmd1.Parameters.Add(new SqlParameter("@Tunit", SqlDbType.VarChar)).Value = "";
                            cmd1.Parameters.Add(new SqlParameter("@UnitCodeSr", SqlDbType.SmallInt, 2, "UnitSerial"));
                            cmd1.Parameters.Add(new SqlParameter("@ItemStatus", SqlDbType.SmallInt, 2, "ItemStatus"));

                            cmd1.Transaction = transaction;
                            DaPR2.InsertCommand = cmd1;
                            try
                            {
                                DaPR2.Update(DsTmpOrg, "TmpTbl");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }



                using (SqlCommand CmdOrdeRec = new SqlCommand())
                {
                    CmdOrdeRec.Connection = cn;
                    CmdOrdeRec.CommandText = "Ord_UpdateInboundInfoHF";
                    CmdOrdeRec.CommandType = CommandType.StoredProcedure;
                    CmdOrdeRec.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdOrdeRec.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                    CmdOrdeRec.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                    CmdOrdeRec.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                    CmdOrdeRec.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.VarChar)).Value = InboundSer;
                    CmdOrdeRec.Transaction = transaction;
                    try
                    {
                        CmdOrdeRec.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }


            Ord_InboundsInfoHF exInboundsInfoHF = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer).FirstOrDefault();

            if (exInboundsInfoHF != null)
            {
                exInboundsInfoHF.IsApproval = true;
                db.SaveChanges();
            }

            InboundManagementHF.IsApproval = true;
            if (InboundManagementHF.IsMarketing == true || InboundManagementHF.IsQualityControl == true || InboundManagementHF.Istqm == true)
            {
                InboundManagementHF.IsReleaseOrder = true;
                InboundManagementHF.IsReserveFreeReleaseOrder = false;
            }
            else
            {
                InboundManagementHF.IsReleaseOrder = false;
                InboundManagementHF.IsReserveFreeReleaseOrder = true;
            }
            db.SaveChanges();
            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, InboundSer = InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public DataSet LoadArch(int OrdYear, string OrderNo, string TawreedNo, string InboundSer, string InboundGRN, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetInboundManagementArchInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = TawreedNo;
                Cmd.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = InboundSer;
                Cmd.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = InboundGRN;
                Cmd.Transaction = MyTrans;
                try
                {
                    DaCmd.SelectCommand = Cmd;
                    DaCmd.Fill(DsArch, "Arch");
                }
                catch (Exception ex)
                {
                    DsArch = new DataSet();
                }
            }
            return DsArch;
        }
        public string InsertFile()
        {
            List<UploadFileInboundManagement> lstFile = null;
            if (Session["FilesInboundManagement"] == null)
            {
                lstFile = new List<UploadFileInboundManagement>();
            }
            else
            {
                lstFile = (List<UploadFileInboundManagement>)Session["FilesInboundManagement"];
            }
            UploadFileInboundManagement SinglFile = new UploadFileInboundManagement();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var InboundSer = Request.Params["InboundSer"];
            var InboundGRN = Request.Params["InboundGRN"];



            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == OrderNo
                && a.TawreedNo == TawreedNo && a.InboundSer == InboundSer && a.InboundGRN == InboundGRN && a.FileId == Convert.ToInt32(Id));
            }
            byte[] FileByte = null;

            if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    if (SinglFile == null)
                    {
                        var _file = (HttpPostedFileBase)Request.Files[file];
                        BinaryReader rdr = new BinaryReader(_file.InputStream);
                        FileByte = rdr.ReadBytes((int)_file.ContentLength);
                        SinglFile = new UploadFileInboundManagement();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = OrderNo;
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.InboundSer = InboundSer;
                        SinglFile.InboundGRN = InboundGRN;
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                        lstFile.Add(SinglFile);
                    }
                    else
                    {
                        var _file = (HttpPostedFileBase)Request.Files[file];
                        BinaryReader rdr = new BinaryReader(_file.InputStream);
                        FileByte = rdr.ReadBytes((int)_file.ContentLength);
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = OrderNo;
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.InboundSer = InboundSer;
                        SinglFile.InboundGRN = InboundGRN;
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["FilesInboundManagement"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, string OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            List<UploadFileInboundManagement> lstFile = null;
            if (Session["FilesInboundManagement"] == null)
            {
                lstFile = new List<UploadFileInboundManagement>();
            }
            else
            {
                lstFile = (List<UploadFileInboundManagement>)Session["FilesInboundManagement"];
            }
            UploadFileInboundManagement SinglFile = new UploadFileInboundManagement();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == OrderNo
                && a.TawreedNo == TawreedNo && a.InboundSer == InboundSer && a.InboundGRN == InboundGRN && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesInboundManagement"] = lstFile;
            }

            return "done";
        }
    }
}