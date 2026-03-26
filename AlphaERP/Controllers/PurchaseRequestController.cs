using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlphaERP.Models;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.Data.SqlClient;
using System.Data;
using CrystalDecisions.Shared;

namespace AlphaERP.Controllers
{
    public class PurchaseRequestController : controller
    {
        public ActionResult Index()
        {
            Session["FilesPR"] = null;
            return View();
        }
        public string CreateReportUniqueId(string path)
        {
            int index1 = path.LastIndexOf('\\');
            int index2 = path.LastIndexOf('.');
            return path.Substring(index1 + 1, index2 - index1 - 1);
        }
        [HttpPost]
        public ActionResult ReportPurchaseRequest(int ReqYear, string ReqNo)
        {
            ReportInformation reportInfo = new ReportInformation();
            DataSet Alpha_ERP_DataSet = new DataSet();
            DataTable d1 = Ord_RptRequestHF(ReqYear, ReqNo);
            d1.TableName = "Ord_RptRequestHF";
            Alpha_ERP_DataSet.Tables.Add(d1);

            DataTable d2 = Ord_RptRequestDF(ReqYear, ReqNo);
            d2.TableName = "Ord_RptRequestDF";
            Alpha_ERP_DataSet.Tables.Add(d2);

            DataTable d3 = Comp_FillImage();
            d3.TableName = "Comp_FillImage";
            Alpha_ERP_DataSet.Tables.Add(d3);

            reportInfo.myDataSet = Alpha_ERP_DataSet;
            reportInfo.ReportName = "طلب شراء";
            reportInfo.fields = SetParameterField();

            if (Language == "ar-JO")
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptPRInfo.rpt");
            }
            else
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptPRInfoEng.rpt");
            }

            string reportUniqueName = CreateReportUniqueId(reportInfo.path);
            ReportInfoManager.AddReport(reportUniqueName, reportInfo);

            this.HttpContext.Session["reportInfoID"] = reportInfo.Id;
            return RedirectToAction("ReportViewer", "CrystalReportViewer", new { area = "", id = reportInfo.Id });
        }
        public ParameterFields SetParameterField()
        {
            ClientsActive CActive =db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            InvCompMF compMF = db.InvCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            ParameterFields parameterFields = new ParameterFields();
            /****************************************************/

            /******************UserName*****************/
            ParameterField UserName = new ParameterField();
            UserName.Name = "UserName";
            ParameterDiscreteValue discreteVal8 = new ParameterDiscreteValue();
            discreteVal8.Value = me.UserID;
            UserName.CurrentValues.Add(discreteVal8);
            parameterFields.Add(UserName);
            /******************UseQty*****************/
            ParameterField UseQty = new ParameterField();
            UseQty.Name = "UseQty";
            ParameterDiscreteValue discreteVal9 = new ParameterDiscreteValue();
            if (compMF.UseQty2 == true)
            {
                discreteVal9.Value = true;
            }
            else
            {
                discreteVal9.Value = false;
            }
            UseQty.CurrentValues.Add(discreteVal9);
            parameterFields.Add(UseQty);
            /******************CActive*****************/
            ParameterField ClientActive = new ParameterField();
            ClientActive.Name = "CActive";
            ParameterDiscreteValue discreteValClientActive = new ParameterDiscreteValue();
            discreteValClientActive.Value = CActive.ClientNo;
            ClientActive.CurrentValues.Add(discreteValClientActive);
            parameterFields.Add(ClientActive);
            /************************/
            return parameterFields;
        }
        public DataTable Ord_RptRequestHF(int OrdYear, string OrderNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptRequestHF";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.Int)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.NVarChar)).Value = OrderNo;
                    if (Language == "ar-JO")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", System.Data.SqlDbType.SmallInt)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", System.Data.SqlDbType.SmallInt)).Value = 2;
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
        public DataTable Ord_RptRequestDF(int OrdYear, string OrderNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptRequestDF";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.Int)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.NVarChar)).Value = OrderNo;
                    if (Language == "ar-JO")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", System.Data.SqlDbType.SmallInt)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", System.Data.SqlDbType.SmallInt)).Value = 2;
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
        public ActionResult AddPurchaseRequest()
        {
            List<PayCodes> Currency = new MDB().Database.SqlQuery<PayCodes>("select CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType").ToList();
            List<PayCodes> PrdType = new MDB().Database.SqlQuery<PayCodes>(string.Format("select MinCode as sys_minor ,CodeDesc as DescAr,isnull(CodeDescEng,'No Desc.') as DescEn from InvCodes where (CompNo = '{0}') AND (MajCode = 'PrdType') AND MinCode <> 0", company.comp_num)).ToList();
            List<PayCodes> Purpose = new MDB().Database.SqlQuery<PayCodes>(string.Format("select PCode as Id ,Purp_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from OrdPurchPurpose where (CompNo = '{0}')", company.comp_num)).ToList();

            ViewBag.Currency = Currency;
            ViewBag.PrdType = PrdType;
            ViewBag.Purpose = Purpose;

            return View();
        }
        public ActionResult AddPurchaseRequestKasih()
        {
            List<PayCodes> Currency = new MDB().Database.SqlQuery<PayCodes>("select CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType").ToList();
            List<PayCodes> PrdType = new MDB().Database.SqlQuery<PayCodes>(string.Format("select MinCode as sys_minor ,CodeDesc as DescAr,isnull(CodeDescEng,'No Desc.') as DescEn from InvCodes where (CompNo = '{0}') AND (MajCode = 'PrdType') AND MinCode <> 0", company.comp_num)).ToList();
            List<PayCodes> Purpose = new MDB().Database.SqlQuery<PayCodes>(string.Format("select PCode as Id ,Purp_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from OrdPurchPurpose where (CompNo = '{0}')", company.comp_num)).ToList();

            ViewBag.Currency = Currency;
            ViewBag.PrdType = PrdType;
            ViewBag.Purpose = Purpose;
            return View("~/Views/PurchaseRequest/Kasih/AddPurchaseRequestKasih.cshtml");
        }
        public ActionResult EditPurchaseRequestKasih(string ReqNo, int ReqYear)
        {
            ViewBag.ReqNo = ReqNo;
            ViewBag.ReqYear = ReqYear;

            List<UploadFilePR> lstFile = null;
            if (Session["FilesPR"] == null)
            {
                lstFile = new List<UploadFilePR>();
            }
            else
            {
                lstFile = (List<UploadFilePR>)Session["FilesPR"];
            }
            UploadFilePR SinglFile = new UploadFilePR();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdReceipt(ReqYear, ReqNo, transaction, cn);
            }

            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == DrArch["OrderNo"].ToString() && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFilePR();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = DrArch["OrderNo"].ToString();
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
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesPR"] = lstFile;
                }
            }

            List<BusunisUnit> BU = new MDB().Database.SqlQuery<BusunisUnit>(string.Format("SELECT  Alpha_BusinessUnitDef.BusUnitID as Id, BusUnitDescAr as DescAr,BusUnitDescEng as DescEn  " +
              "from Alpha_BusinessUnitDef " +
              "where Alpha_BusinessUnitDef.CompNo = '{0}' ", company.comp_num)).ToList();


            List<PayCodes> Currency = new MDB().Database.SqlQuery<PayCodes>("select CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType").ToList();
            List<PayCodes> PrdType = new MDB().Database.SqlQuery<PayCodes>(string.Format("select MinCode as sys_minor ,CodeDesc as DescAr,isnull(CodeDescEng,'No Desc.') as DescEn from InvCodes where (CompNo = '{0}') AND (MajCode = 'PrdType') AND MinCode <> 0", company.comp_num)).ToList();
            List<PayCodes> Purpose = new MDB().Database.SqlQuery<PayCodes>(string.Format("select PCode as Id ,Purp_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from OrdPurchPurpose where (CompNo = '{0}')", company.comp_num)).ToList();

            ViewBag.Currency = Currency;
            ViewBag.PrdType = PrdType;
            ViewBag.Purpose = Purpose;
            ViewBag.BusunisUnit = BU;
            ViewBag.Arch = DSArch.Tables["Arch"];
            return View("~/Views/PurchaseRequest/Kasih/EditPurchaseRequestKasih.cshtml");
        }
        public ActionResult EditPurchaseRequest(string ReqNo, int ReqYear)
        {
            ViewBag.ReqNo = ReqNo;
            ViewBag.ReqYear = ReqYear;

            List<UploadFilePR> lstFile = null;
            if (Session["FilesPR"] == null)
            {
                lstFile = new List<UploadFilePR>();
            }
            else
            {
                lstFile = (List<UploadFilePR>)Session["FilesPR"];
            }
            UploadFilePR SinglFile = new UploadFilePR();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdReceipt(ReqYear, ReqNo, transaction, cn);
            }

            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == DrArch["OrderNo"].ToString() && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFilePR();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = DrArch["OrderNo"].ToString();
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
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesPR"] = lstFile;
                }
            }

            List<BusunisUnit> BU = new MDB().Database.SqlQuery<BusunisUnit>(string.Format("SELECT  Alpha_BusinessUnitDef.BusUnitID as Id, BusUnitDescAr as DescAr,BusUnitDescEng as DescEn  " +
              "from Alpha_BusinessUnitDef " +
              "where Alpha_BusinessUnitDef.CompNo = '{0}' ", company.comp_num)).ToList();

            List<PayCodes> Currency = new MDB().Database.SqlQuery<PayCodes>("select CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType").ToList();
            List<PayCodes> PrdType = new MDB().Database.SqlQuery<PayCodes>(string.Format("select MinCode as sys_minor ,CodeDesc as DescAr,isnull(CodeDescEng,'No Desc.') as DescEn from InvCodes where (CompNo = '{0}') AND (MajCode = 'PrdType') AND MinCode <> 0", company.comp_num)).ToList();
            List<PayCodes> Purpose = new MDB().Database.SqlQuery<PayCodes>(string.Format("select PCode as Id ,Purp_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from OrdPurchPurpose where (CompNo = '{0}')", company.comp_num)).ToList();

            ViewBag.Currency = Currency;
            ViewBag.PrdType = PrdType;
            ViewBag.Purpose = Purpose;

            ViewBag.BusunisUnit = BU;
            ViewBag.Arch = DSArch.Tables["Arch"];
            return View();
        }
        public ActionResult ViewPurchaseRequest(string ReqNo,int ReqYear)
        {
            ViewBag.ReqNo = ReqNo;
            ViewBag.ReqYear = ReqYear;


            List<BusunisUnit> BU = new MDB().Database.SqlQuery<BusunisUnit>(string.Format("SELECT  Alpha_BusinessUnitDef.BusUnitID as Id, BusUnitDescAr as DescAr,BusUnitDescEng as DescEn  " +
        "from Alpha_BusinessUnitDef " +
        "where Alpha_BusinessUnitDef.CompNo = '{0}' ", company.comp_num)).ToList();

            List<PayCodes> Currency = new MDB().Database.SqlQuery<PayCodes>("select CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType").ToList();
            List<PayCodes> PrdType = new MDB().Database.SqlQuery<PayCodes>(string.Format("select MinCode as sys_minor ,CodeDesc as DescAr,isnull(CodeDescEng,'No Desc.') as DescEn from InvCodes where (CompNo = '{0}') AND (MajCode = 'PrdType') AND MinCode <> 0", company.comp_num)).ToList();
            List<PayCodes> Purpose = new MDB().Database.SqlQuery<PayCodes>(string.Format("select PCode as Id ,Purp_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from OrdPurchPurpose where (CompNo = '{0}')", company.comp_num)).ToList();
            ViewBag.Currency = Currency;
            ViewBag.PrdType = PrdType;
            ViewBag.Purpose = Purpose;

            ViewBag.BusunisUnit = BU;

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdReceipt(ReqYear, ReqNo, transaction, cn);
            }

            ViewBag.Arch = DSArch.Tables["Arch"];

            return View();
        }
        public ActionResult PurchaseRequestList(int ReqStatus)
        {
            List<OrdRequest> OrdRequestHF = new MDB().Database.SqlQuery<OrdRequest>(string.Format("exec Ord_GetPurchaseRequestList {0},{1},'{2}'", company.comp_num,ReqStatus,me.UserID)).ToList();

            return PartialView(OrdRequestHF);
        }
        public ActionResult PurchaseReqItemDiv()
        {
            return PartialView();
        }
        public ActionResult PurchaseReqItemDivKasih()
        {
            return PartialView("~/Views/PurchaseRequest/Kasih/PurchaseReqItemDivKasih.cshtml");
        }
        public ActionResult PurchaseRequestItems(List<InvItemsMF> items, int srl)
        {
            ViewBag.Serial = srl;
            return PartialView(items);
        }
        public ActionResult ePurchaseReqItemDiv(List<Ord_RequestDF> OrdRequestDF)
        {
            return PartialView(OrdRequestDF);
        }
        public ActionResult ePurchaseReqItemDivKasih(List<Ord_RequestDF> OrdRequestDF)
        {
            return PartialView("~/Views/PurchaseRequest/Kasih/ePurchaseReqItemDivKasih.cshtml", OrdRequestDF);
        }
        public ActionResult ePurchaseRequestItems(List<InvItemsMF> items, int srl)
        {
            ViewBag.Serial = srl;
            return PartialView(items);
        }
        public JsonResult LoadUnitCost(string itemNo, int Unitserial,int srl)
        {
            DataTable Dt = new DataTable();
            double QtyOH = 0;
            double ResQty = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_GetItemUnitCost";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", System.Data.SqlDbType.VarChar, 20)).Value = itemNo;
                    cmd.Parameters.Add(new SqlParameter("@UnitSerial", System.Data.SqlDbType.SmallInt, 4)).Value = Unitserial;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            foreach (DataRow row in Dt.Rows)
            {
                QtyOH = Convert.ToDouble(row["AvlQty"]);
                ResQty = Convert.ToDouble(row["ResQty"]);
            }

            return Json(new { QtyOH = QtyOH, ResQty = ResQty, srl= srl }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Save_PurchaseRequest(Ord_RequestHF OrdReqHF, List<Ord_RequestDF> OrdReqDF, List<UploadFilePR> FileArchive)
        {
            
            int i = 1;
            DataSet TmpDs = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                try
                {
                    DaPR.SelectCommand = new SqlCommand("select Ord_RequestDF.* from Ord_RequestDF where CompNo=0", cn);
                    DaPR.Fill(TmpDs, "tmp");
                }
                catch(Exception ex)
                {
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }

                transaction = cn.BeginTransaction();


                using (SqlCommand CmdHead = new SqlCommand())
                {
                    CmdHead.Connection = cn;
                    CmdHead.CommandText = "Ord_AddOrdRequestHF";
                    CmdHead.CommandType = CommandType.StoredProcedure;
                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = OrdReqHF.CompNo;
                    CmdHead.Parameters.Add(new SqlParameter("@ReqYear", SqlDbType.SmallInt, 4)).Value = OrdReqHF.ReqDate.Value.Year;
                    CmdHead.Parameters.Add(new SqlParameter("@ReqNo", SqlDbType.VarChar, 10)).Value = OrdReqHF.ReqNo;
                    CmdHead.Parameters.Add(new SqlParameter("@ReqDate", SqlDbType.DateTime, 20)).Value = OrdReqHF.ReqDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ReqDeliveryDate", SqlDbType.SmallDateTime, 20)).Value = OrdReqHF.ReqDeliveryDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ReqBU", SqlDbType.SmallInt,4)).Value = OrdReqHF.ReqBU;
                    CmdHead.Parameters.Add(new SqlParameter("@AuthBU", SqlDbType.SmallInt, 4)).Value = OrdReqHF.AuthBU;
                    CmdHead.Parameters.Add(new SqlParameter("@UserIDApprovel", SqlDbType.VarChar, 10)).Value = OrdReqHF.UserIDApprovel;
                    CmdHead.Parameters.Add(new SqlParameter("@OrderPriority", SqlDbType.SmallInt, 4)).Value = OrdReqHF.OrderPriority;

                    if (OrdReqHF.Note == null)
                    {
                        OrdReqHF.Note = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 5000)).Value = OrdReqHF.Note;
                    CmdHead.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt, 4)).Value = OrdReqHF.CurrNo;
                    if (OrdReqHF.RefNo == null)
                    {
                        OrdReqHF.RefNo = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.NVarChar, 200)).Value = OrdReqHF.RefNo;
                    CmdHead.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int, 9)).Value = OrdReqHF.OrderType;
                    CmdHead.Parameters.Add(new SqlParameter("@Purpose", SqlDbType.Int, 9)).Value = OrdReqHF.Purpose;

                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.VarChar,10)).Direction = ParameterDirection.Output;
                    CmdHead.Transaction = transaction;
                    try
                    {
                        CmdHead.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    var LogID = Convert.ToString(CmdHead.Parameters["@LogID"].Value);
                    double Total = 0;
                    foreach (Ord_RequestDF item in OrdReqDF)
                    {
                        DrTmp = TmpDs.Tables["tmp"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["ReqYear"] = OrdReqHF.ReqDate.Value.Year;
                        DrTmp["ReqNo"] = LogID;
                        DrTmp["ItemSr"] = i;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["TUnit"] = item.TUnit;
                        DrTmp["Qty"] = item.Qty;
                        DrTmp["Qty2"] = item.Qty2;
                        DrTmp["Price"] = item.Price;
                        DrTmp["TotalValue"] = item.TotalValue;
                        Total = Total + item.TotalValue.Value;
                        DrTmp["Note"] = item.Note;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["ReqDeliveryDate"] = item.ReqDeliveryDate;
                        DrTmp["QtyStock"] = item.QtyStock;
                        DrTmp.EndEdit();
                        TmpDs.Tables["tmp"].Rows.Add(DrTmp);
                        i = i + 1;
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "Ord_AddOrdRequestDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqYear", SqlDbType.SmallInt, 4, "ReqYear"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqNo", SqlDbType.VarChar, 10, "ReqNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 20, "ItemSr"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@TUnit", SqlDbType.VarChar, 5, "TUnit"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                        CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
                        CmdIns.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Float, 9, "TotalValue"));
                        CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqDeliveryDate", SqlDbType.SmallDateTime, 20, "ReqDeliveryDate"));
                        CmdIns.Parameters.Add(new SqlParameter("@QtyStock", SqlDbType.Money, 9, "QtyStock"));
                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "tmp");
                    }

                    List<UploadFilePR> upFile = (List<UploadFilePR>)Session["FilesPR"];
                    if (upFile != null)
                    {
                        if (upFile.Count != 0)
                        {
                            if (FileArchive != null)
                            {
                                foreach (UploadFilePR item in FileArchive)
                                {
                                    UploadFilePR fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.FileId == item.FileId).FirstOrDefault();

                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddOrdPRArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdReqHF.ReqDate.Value.Year;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = LogID;
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
                                            fl.OrderYear = Convert.ToInt16(OrdReqHF.ReqDate.Value.Year);
                                            fl.OrderNo = LogID;
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

                    string TrDesc = "Referance ( " + OrdReqHF.ReqDate.Value.Year + "-" + LogID + "--)       Amount:  (" + Total + ")   User:(" + me.UserID + ")";
                    bool InsertWorkFlow = AddWorkFlowLog(25, OrdReqHF.ReqDate.Value.Year, 1, LogID, OrdReqHF.AuthBU.Value , 1, Total, TrDesc, OrdReqHF.UserIDApprovel, transaction, cn);
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_PurchaseRequest(Ord_RequestHF OrdReqHF, List<Ord_RequestDF> OrdReqDF, List<UploadFilePR> FileArchive)
        {
            Ord_RequestHF ex = db.Ord_RequestHF.Where(x => x.CompNo == company.comp_num && x.ReqYear == OrdReqHF.ReqYear && x.ReqNo == OrdReqHF.ReqNo).FirstOrDefault();
            if(ex != null)
            {
                ex.ReqDate = OrdReqHF.ReqDate;
                ex.ReqStatus = 5;
                ex.ReqDeliveryDate = OrdReqHF.ReqDeliveryDate;
                ex.ReqBU = OrdReqHF.ReqBU;
                ex.AuthBU = OrdReqHF.AuthBU;
                ex.OrderPriority = OrdReqHF.OrderPriority;
                ex.Note = OrdReqHF.Note;
                ex.CurrNo = OrdReqHF.CurrNo;
                ex.RefNo = OrdReqHF.RefNo;
                if(OrdReqHF.OrderType != null)
                {
                    ex.OrderType = OrdReqHF.OrderType;
                }
                if (OrdReqHF.Purpose != null)
                {
                    ex.Purpose = OrdReqHF.Purpose;
                }
                ex.ReqStatusPO = 0;
                db.SaveChanges();
            }
            double Total = 0;
            List<Ord_RequestDF> RequestDF = db.Ord_RequestDF.Where(x => x.CompNo == OrdReqHF.CompNo && x.ReqYear == OrdReqHF.ReqYear && x.ReqNo == OrdReqHF.ReqNo).ToList();
            if(RequestDF.Count != 0)
            {
                db.Ord_RequestDF.RemoveRange(RequestDF);
                db.SaveChanges();
            }

            short i = 1;
            foreach (Ord_RequestDF item in OrdReqDF)
            {

                Ord_RequestDF ex1 = new Ord_RequestDF();
                ex1.CompNo = item.CompNo;
                ex1.ReqYear = OrdReqHF.ReqYear;
                ex1.ReqNo = OrdReqHF.ReqNo;
                ex1.ItemNo = item.ItemNo;
                ex1.ItemSr = i;
                ex1.TUnit = item.TUnit;
                ex1.UnitSerial = item.UnitSerial;
                ex1.SubItemNo = item.ItemNo;
                ex1.SubTUnit = item.TUnit;
                ex1.SubUnitSerial = item.UnitSerial;
                ex1.Qty = item.Qty;
                ex1.Qty2 = item.Qty2;
                ex1.Price = item.Price;
                ex1.TotalValue = item.TotalValue;
                Total = Total + item.TotalValue.Value;
                ex1.ReqDeliveryDate = item.ReqDeliveryDate;
                ex1.QtyStock = item.QtyStock;
                ex1.Note = item.Note;
                ex1.bPurchaseOrder = false;
                ex1.PurchaseOrderYear = 0;
                ex1.PurchaseOrderNo = 0;
                ex1.PurchaseOrdTawreedNo = "0";
                ex1.PurchaseOrdOfferNo = 0;
                ex1.bRequestforQuotation = false;
                ex1.RequestforQuotationYear = 0;
                ex1.RequestforQuotationNo = 0;
                db.Ord_RequestDF.Add(ex1);
                db.SaveChanges();
                i++;
            }
            

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                List<UploadFilePR> upFile = (List<UploadFilePR>)Session["FilesPR"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        using (SqlCommand DelCmd = new SqlCommand())
                        {
                            DelCmd.Connection = cn;
                            DelCmd.CommandText = "Ord_Web_DelOrdPRArchiveInfo";
                            DelCmd.CommandType = CommandType.StoredProcedure;
                            DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                            DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdReqHF.ReqYear;
                            DelCmd.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = OrdReqHF.ReqNo;
                            DelCmd.Transaction = transaction;
                            DelCmd.ExecuteNonQuery();
                        }

                        if (FileArchive != null)
                        {

                            foreach (UploadFilePR item in FileArchive)
                            {
                                UploadFilePR fl = upFile.Where(x => x.OrderYear == OrdReqHF.ReqYear && x.OrderNo == OrdReqHF.ReqNo && x.FileId == item.FileId).FirstOrDefault();

                                if (fl != null)
                                {
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddOrdPRArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.Int)).Value = OrdReqHF.ReqYear;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = OrdReqHF.ReqNo;
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

                string TrDesc = "Referance ( " + OrdReqHF.ReqYear + "-" + OrdReqHF.ReqNo + "--)       Amount:  (" + Total + ")   User:(" + me.UserID + ")";
                bool InsertWorkFlow = AddWorkFlowLog(25, OrdReqHF.ReqYear, 1, OrdReqHF.ReqNo, OrdReqHF.AuthBU.Value, 2, Total, TrDesc, OrdReqHF.UserIDApprovel, transaction, cn);
                if (InsertWorkFlow == false)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }

                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { ReqYear = OrdReqHF.ReqYear, ReqNo = OrdReqHF.ReqNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public DataSet LoadArchOrdReceipt(int OrdYear, string OrderNo, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetOrdPRArchiveInfo";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.NVarChar)).Value = OrderNo;
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
        public JsonResult Cancel_PurchaseRequest(short ReqYear, string ReqNo,string RejectReason)
        {
            List<Ord_RequestDF> df = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo).ToList();
            foreach (Ord_RequestDF item in df)
            {
                OrderHF hf = db.OrderHFs.Where(x => x.CompNo == item.CompNo && x.OrdYear == item.PurchaseOrderYear && x.OrderNo == item.PurchaseOrderNo
                && x.TawreedNo == item.PurchaseOrdTawreedNo && x.OfferNo == item.PurchaseOrdOfferNo).FirstOrDefault();
                if (hf != null)
                {
                    return Json(new { error = Resources.Resource.errorProdOrderCreate }, JsonRequestBehavior.AllowGet);
                }
            }
            
            Ord_RequestHF o = db.Ord_RequestHF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo).FirstOrDefault();
            if (o.ReqStatus == 0)
            {
                using (var cn = new SqlConnection(ConnectionString()))
                {
                    SqlTransaction transaction;
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    using (SqlCommand CmdDel = new SqlCommand())
                    {
                        CmdDel.Connection = cn;
                        CmdDel.CommandText = "Ord_DelOrdRequestHF";
                        CmdDel.CommandType = CommandType.StoredProcedure;
                        CmdDel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = company.comp_num;
                        CmdDel.Parameters.Add(new SqlParameter("@ReqYear", SqlDbType.SmallInt, 4)).Value = ReqYear;
                        CmdDel.Parameters.Add(new SqlParameter("@ReqNo", SqlDbType.VarChar, 10)).Value = ReqNo;
                        CmdDel.Transaction = transaction;
                        try
                        {
                            CmdDel.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            var xxx = ex.Message;
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    using (SqlCommand DelCmd = new SqlCommand())
                    {
                        DelCmd.Connection = cn;
                        DelCmd.CommandText = "Ord_Web_DelOrdPRArchiveInfo";
                        DelCmd.CommandType = CommandType.StoredProcedure;
                        DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        DelCmd.Parameters.Add("@OrderYear", SqlDbType.Int).Value = ReqYear;
                        DelCmd.Parameters.Add("@OrderNo", SqlDbType.NVarChar).Value = ReqNo;
                        DelCmd.Transaction = transaction;
                        DelCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    cn.Dispose();
                }
            }
            else
            {
                using (var cn = new SqlConnection(ConnectionString()))
                {
                    o.RejectReason = RejectReason;
                    o.ReqStatus = 3;
                    db.SaveChanges();
                    SqlTransaction transaction;
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    double Total = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo).Sum(z => z.TotalValue).Value;

                    string TrDesc = "Cancel Purchase Request Reference ( " + ReqYear + "-" + ReqNo + "--)       Amount:  (" + Total + ")   User:(" + me.UserID + ")";

                    using (SqlCommand Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn;
                        Cmd.CommandText = "Alpha_AddWorkFlowLog";
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        Cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = 25;
                        Cmd.Parameters.Add("@BU", SqlDbType.SmallInt).Value = o.AuthBU.Value;
                        Cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                        Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 10).Value = ReqYear;
                        Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 10).Value = ReqNo;
                        Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = 2;
                        Cmd.Parameters.Add("@TrAmount", SqlDbType.Money).Value = Total;
                        Cmd.Parameters.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                        Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = 1;
                        Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        Cmd.Transaction = transaction;
                        try
                        {
                            Cmd.ExecuteNonQuery();
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
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLogKasih(int FID, int TrYear, int TrType, string TrNo, short BU, int FStat, double TrAmnt, string TrDesc,string UserIDApprovel, SqlTransaction MyTrans, SqlConnection co)
        {
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Alpha_AddWorkFlowLog2";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = FID;
                Cmd.Parameters.Add("@BU", SqlDbType.SmallInt).Value = BU;
                Cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 10).Value = TrYear;
                Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 10).Value = TrNo;
                Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = TrType;
                Cmd.Parameters.Add("@TrAmount", SqlDbType.Money).Value = TrAmnt;
                Cmd.Parameters.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                Cmd.Parameters.Add("@AlertUserID", SqlDbType.VarChar, 8).Value = UserIDApprovel;
                Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
                Cmd.CommandTimeout = 999999999;
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string zz = ex.Message;
                    return false;
                }
            }
            return true;
        }
        public bool AddWorkFlowLog(int FID, int TrYear, int TrType, string TrNo, short BU, int FStat, double TrAmnt, string TrDesc, string UserIDApprovel, SqlTransaction MyTrans, SqlConnection co)
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
                Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = TrType;
                Cmd.Parameters.Add("@TrAmount", SqlDbType.Money).Value = TrAmnt;
                Cmd.Parameters.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                Cmd.Parameters.Add("@AlertUserID", SqlDbType.VarChar, 8).Value = UserIDApprovel;
                Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
                Cmd.CommandTimeout = 999999999;
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string zz = ex.Message;
                    return false;
                }
            }
            return true;
        }

        public string InsertFile()
        {
            List<UploadFilePR> lstFile = null;
            if (Session["FilesPR"] == null)
            {
                lstFile = new List<UploadFilePR>();
            }
            else
            {
                lstFile = (List<UploadFilePR>)Session["FilesPR"];
            }
            UploadFilePR SinglFile = new UploadFilePR();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];


            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == OrderNo && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFilePR();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = OrderNo;
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
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["FilesPR"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, string OrderNo)
        {
            List<UploadFilePR> lstFile = null;
            if (Session["FilesPR"] == null)
            {
                lstFile = new List<UploadFilePR>();
            }
            else
            {
                lstFile = (List<UploadFilePR>)Session["FilesPR"];
            }
            UploadFilePR SinglFile = new UploadFilePR();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == OrderNo &&  a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesPR"] = lstFile;
            }

            return "done";
        }
    }
}