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
    public class PurchaseOrderController : controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            Session["FilesPO"] = null;
            return View();
        }
        public ActionResult PurchaseOrderList(int OrdYear)
        {
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            List<LPROrder> PR = new List<LPROrder>();
            if(CActive.ClientNo == 239)
            {
                if (me.UserID == "Saif.sc")
                {
                    PR = new MDB().Database.SqlQuery<LPROrder>(string.Format("SELECT dbo.OrderHF.OrdYear as OrdYear,dbo.OrderHF.OrderNo as OrderNo, isnull(dbo.OrderHF.TawreedNo,'*')as TawreedNo,OrderHF.EnterDate as OrderDate,Vendors.VendorNo,isnull(dbo.Vendors.name,'*')as VenName,isnull(dbo.Vendors.Eng_name,'*')as VenName_Eng ,  " +
              "case when OrderClose = 1 then 'مغلق' else 'غير مغلق' end as OrderCloseDescAr,case when OrderClose = 1 then 'Closed' else 'Un Closed' end as OrderCloseDescEn, " +
              "case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
              "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng," +
              "isnull((select top 1 BULvl as BULvl from Alpha_WorkFlowLog where (CompNo = OrderHF.CompNo) AND (FID = 3) AND (K1 = OrderHF.OrdYear) AND (K2 = OrderHF.OrderNo) AND (K3 = OrderHF.TawreedNo) order by BULvl desc),1) as LevelApprv, " +
              "OrderHF.Approved,case when OrderHF.Approved = 1 then 'موافق عليها' else 'غير موافق عليها' end as ApprovedDescAr,case when OrderHF.Approved = 1 then 'Approved' else 'Un Non Approved Order' end as ApprovedDescEn, " +
              "Isnull((select top 1 ISNULL(ReqNo,0) as ReqNo from Ord_RequestDF where (CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), 0) as ReqNo,  " +
             "Isnull((select top 1 ISNULL(RefNo,'') as ReqNo from Ord_RequestHF Inner join Ord_RequestDF on (Ord_RequestHF.CompNo = Ord_RequestDF.CompNo) AND (Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear) AND (Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo) where (Ord_RequestHF.CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), '') as RefNo,isnull(OrderHF.IsExport,0) as IsExport  " +
              "FROM dbo.OrderHF LEFT  JOIN dbo.Vendors ON dbo.OrderHF.VendorNo = dbo.Vendors.VendorNo AND dbo.OrderHF.CompNo = dbo.Vendors.comp  " +
              "WHERE (dbo.OrderHF.CompNo = '{0}') AND (dbo.OrderHF.OrdYear = '{1}')  ", company.comp_num, OrdYear)).ToList();
                }
                else
                {
                    PR = new MDB().Database.SqlQuery<LPROrder>(string.Format("SELECT dbo.OrderHF.OrdYear as OrdYear,dbo.OrderHF.OrderNo as OrderNo, isnull(dbo.OrderHF.TawreedNo,'*')as TawreedNo,OrderHF.EnterDate as OrderDate,Vendors.VendorNo,isnull(dbo.Vendors.name,'*')as VenName,isnull(dbo.Vendors.Eng_name,'*')as VenName_Eng ,  " +
             "case when OrderClose = 1 then 'مغلق' else 'غير مغلق' end as OrderCloseDescAr,case when OrderClose = 1 then 'Closed' else 'Un Closed' end as OrderCloseDescEn, " +
             "case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
             "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng," +
             "isnull((select top 1 BULvl as BULvl from Alpha_WorkFlowLog where (CompNo = OrderHF.CompNo) AND (FID = 3) AND (K1 = OrderHF.OrdYear) AND (K2 = OrderHF.OrderNo) AND (K3 = OrderHF.TawreedNo) order by BULvl desc),1) as LevelApprv, " +
             "OrderHF.Approved,case when OrderHF.Approved = 1 then 'موافق عليها' else 'غير موافق عليها' end as ApprovedDescAr,case when OrderHF.Approved = 1 then 'Approved' else 'Un Non Approved Order' end as ApprovedDescEn, " +
             "Isnull((select top 1 ISNULL(ReqNo,0) as ReqNo from Ord_RequestDF where (CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), 0) as ReqNo,  " +
             "Isnull((select top 1 ISNULL(RefNo,'') as ReqNo from Ord_RequestHF Inner join Ord_RequestDF on (Ord_RequestHF.CompNo = Ord_RequestDF.CompNo) AND (Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear) AND (Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo) where (Ord_RequestHF.CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), '') as RefNo ,isnull(OrderHF.IsExport,0) as IsExport  " +
             "FROM dbo.OrderHF LEFT  JOIN dbo.Vendors ON dbo.OrderHF.VendorNo = dbo.Vendors.VendorNo AND dbo.OrderHF.CompNo = dbo.Vendors.comp  " +
             "WHERE (dbo.OrderHF.CompNo = '{0}') AND (dbo.OrderHF.OrdYear = '{1}') AND (OrderHF.OrdUser = '{2}') ", company.comp_num, OrdYear, me.UserID)).ToList();
                }
            }
            else
            {
                PR = new MDB().Database.SqlQuery<LPROrder>(string.Format("SELECT dbo.OrderHF.OrdYear as OrdYear,dbo.OrderHF.OrderNo as OrderNo, isnull(dbo.OrderHF.TawreedNo,'*')as TawreedNo,OrderHF.EnterDate as OrderDate,Vendors.VendorNo,isnull(dbo.Vendors.name,'*')as VenName,isnull(dbo.Vendors.Eng_name,'*')as VenName_Eng ,  " +
              "case when OrderClose = 1 then 'مغلق' else 'غير مغلق' end as OrderCloseDescAr,case when OrderClose = 1 then 'Closed' else 'Un Closed' end as OrderCloseDescEn, " +
              "case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
              "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng," +
              "isnull((select top 1 BULvl as BULvl from Alpha_WorkFlowLog where (CompNo = OrderHF.CompNo) AND (FID = 3) AND (K1 = OrderHF.OrdYear) AND (K2 = OrderHF.OrderNo) AND (K3 = OrderHF.TawreedNo) order by BULvl desc),1) as LevelApprv, " +
              "OrderHF.Approved,case when OrderHF.Approved = 1 then 'موافق عليها' else 'غير موافق عليها' end as ApprovedDescAr,case when OrderHF.Approved = 1 then 'Approved' else 'Un Non Approved Order' end as ApprovedDescEn, " +
              "isnull((select top 1 ISNULL(ReqNo,0) as ReqNo from Ord_RequestDF where (CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), 0) as ReqNo , " +
              "isnull((select top 1 ISNULL(RefNo,'') as ReqNo from Ord_RequestHF Inner join Ord_RequestDF on (Ord_RequestHF.CompNo = Ord_RequestDF.CompNo) AND (Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear) AND (Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo) where (Ord_RequestHF.CompNo = OrderHF.CompNo) AND (PurchaseOrderYear = OrderHF.OrdYear) AND (PurchaseOrderNo = OrderHF.OrderNo) AND(PurchaseOrdTawreedNo = OrderHF.TawreedNo)), '') as RefNo ,isnull(OrderHF.IsExport,0) as IsExport " +
              "FROM dbo.OrderHF LEFT  JOIN dbo.Vendors ON dbo.OrderHF.VendorNo = dbo.Vendors.VendorNo AND dbo.OrderHF.CompNo = dbo.Vendors.comp  " +
              "WHERE (dbo.OrderHF.CompNo = '{0}') AND (dbo.OrderHF.OrdYear = '{1}')", company.comp_num, OrdYear)).ToList();
            }

            ViewBag.OrdYear = OrdYear;
            return PartialView(PR);
        }
        public ActionResult AddPurchaseOrder(int OrdYear)
        {
            string TawreedNo = "0";
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                TawreedNo = GetOrderSerial(OrdYear, 6, transaction, cn);
                if (TawreedNo == "0")
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = Resources.Resource.errorPurchaseOrderSerial }, JsonRequestBehavior.AllowGet);

                }
            }
            ViewBag.OrdYear = OrdYear;
            ViewBag.TawreedNo = TawreedNo;
            return View();
        }
        public string Serial(int PYear, int Scode)
        {
            string TawreedNo = "0";
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                if(Scode == 1)
                {
                    TawreedNo = GetOrderSerial(PYear, 6, transaction, cn);
                }
                else
                {
                    TawreedNo = GetOrderSerial(PYear, 7, transaction, cn);
                }
                if (TawreedNo == "0")
                {
                    transaction.Rollback();
                    cn.Dispose();
                }
            }
            return Convert.ToString(TawreedNo);
        }

        public string GetOrderSerial(int PYear, int Scode, SqlTransaction MyTrans, SqlConnection co)
        {
            int OrdSerial = 0;
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetOrdSerials";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@PYear", SqlDbType.SmallInt).Value = PYear;
                Cmd.Parameters.Add("@SCode", SqlDbType.Int).Value = Scode;
                Cmd.Transaction = MyTrans;
                try
                {
                    SqlDataReader rdr = Cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            OrdSerial = Convert.ToInt32(rdr["srl_srl"]) + +1;
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    string zz = ex.Message;
                    OrdSerial = 0;
                }
            }
            return Convert.ToString(OrdSerial);
        }

        [HttpGet]
        public ActionResult GetItemRevisions(string itemNo)
        {
            var revisions = db.Prod_RevisionSetup
                .Where(x => x.CompNo == company.comp_num && x.ItemNo == itemNo &&  x.PurchasingFlag == true).ToList();
                
            return PartialView("_RevisionsModal", revisions);
        }

        public ActionResult PurchaseOrderItems(List<InvItemsMF> items, int srl)
        {
            ViewBag.Serial = srl;
            return PartialView(items);
        }
        public ActionResult ePurchaseOrderItems(List<InvItemsMF> items, int srl, string Id)
        {
            ViewBag.Serial = srl;
            ViewBag.Id = Id;

            return PartialView(items);
        }
        public ActionResult EditPurchaseOrder( int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;


            List<UploadFilePO> lstFile = null;
            if (Session["FilesPO"] == null)
            {
                lstFile = new List<UploadFilePO>();
            }
            else
            {
                lstFile = (List<UploadFilePO>)Session["FilesPO"];
            }
            UploadFilePO SinglFile = new UploadFilePO();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdPO(OrdYear, OrderNo, TawreedNo, transaction, cn);
            }

            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["Ord_Year"]) && a.OrderNo == Convert.ToInt32(DrArch["Ord_No"]) && a.TawreedNo == Convert.ToString(DrArch["TawreedNo"]) && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["FileData"];

                        SinglFile = new UploadFilePO();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["Ord_Year"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["Ord_No"]);
                        SinglFile.TawreedNo = Convert.ToString(DrArch["TawreedNo"]);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                        lstFile.Add(SinglFile);
                    }
                    else
                    {
                        byte[] FileByte = (byte[])DrArch["FileData"];
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["Ord_Year"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["Ord_No"]);
                        SinglFile.TawreedNo = Convert.ToString(DrArch["TawreedNo"]);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesPO"] = lstFile;
                }
            }

            return View();
        }
        public DataSet LoadArchOrdPO(int OrdYear, int OrdNo, string TawreedNo, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetAttachmentFiles";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.BigInt)).Value = OrdNo;
                cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
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
        public JsonResult LoadUnitCost(string itemNo, int Unitserial, int srl)
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
            return Json(new { QtyOH = QtyOH, ResQty = ResQty, srl = srl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDeptName(int DeptNo)
        {
            string DeptName = "";
            GLN_UsersDept UsersDept = db.GLN_UsersDept.Where(x => x.CompNo == company.comp_num && x.DeptID == DeptNo && x.UserID == me.UserID && x.Permission == true).FirstOrDefault();
            if(UsersDept == null)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا تملك الصلاحية لهذه الدائرة",DeptName = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "You dont have permissions For this department", DeptName = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            GLDEPMF dept = new MDB().GLDEPMFs.Where(x => x.DEP_COMP == company.comp_num && x.DEP_NUM == DeptNo).FirstOrDefault();

            if (Language == "ar-JO")
            {
                DeptName = dept.DEP_NAME;
            }
            else
            {
                DeptName = dept.DEP_ENAME;
            }
           
            return Json(new { DeptName = DeptName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Account(int DeptId)
        {
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, Vendors.VendorNo as AccId, Vendors.[Name] as AccDescAr, Vendors.Eng_Name as AccDescEn " +
                                                     "FROM Vendors INNER JOIN GLCRBMF ON Vendors.comp = GLCRBMF.CRB_COMP AND Vendors.VendorNo = GLCRBMF.crb_acc " +
                                                      "WHERE(GLCRBMF.crb_dep = '{0}') AND (comp = '{1}') AND (IsHalt = 0)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public ActionResult eAccount(int DeptId,int Id)
        {
            ViewBag.Id = Id;
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, Vendors.VendorNo as AccId, Vendors.[Name] as AccDescAr, Vendors.Eng_Name as AccDescEn " +
                                                     "FROM Vendors INNER JOIN GLCRBMF ON Vendors.comp = GLCRBMF.CRB_COMP AND Vendors.VendorNo = GLCRBMF.crb_acc " +
                                                      "WHERE(GLCRBMF.crb_dep = '{0}') AND (comp = '{1}') AND (IsHalt = 0)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public JsonResult GetVendorName(int DeptNo, long AccNo)
        {
            
            Acc ex = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, Vendors.VendorNo as AccId, Vendors.[Name] as AccDescAr, Vendors.Eng_Name as AccDescEn " +
                                                       "FROM Vendors INNER JOIN GLCRBMF ON Vendors.comp = GLCRBMF.CRB_COMP AND Vendors.VendorNo = GLCRBMF.crb_acc " +
                                                        "WHERE(GLCRBMF.crb_dep = '{0}') AND (comp = '{1}') AND (Vendors.VendorNo = '{2}') AND (IsHalt = 0)", DeptNo, company.comp_num, AccNo)).FirstOrDefault();


            string VendorName = "";
            if (ex == null)
            {
                return Json(new { VendorName = VendorName }, JsonRequestBehavior.AllowGet);
            }


            if (Language == "ar-JO")
            {
                VendorName = ex.AccDescAr;
            }
            else
            {
                VendorName = ex.AccDescEn;
            }
            Vendor ven = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == ex.AccId).FirstOrDefault();
            return Json(new { VendorName = VendorName, VendAdrees = ven.Address, PaymentType = ven.Pmethod, Curr = ven.Curr }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreatePurchaseOrder(int BU, OrderHF OrdHF, List<OrderDF> OrdDF, OrdCondsGuaranty OrdCondsGuaranty, List<Ord_POAttachments> Ord_POAttachments, List<UploadFilePO> DocumentArchive)
        {
            int i = 1;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            SqlDataAdapter DaPR3 = new SqlDataAdapter();
            SqlDataAdapter DaPR4 = new SqlDataAdapter();
            SqlDataAdapter DaPR5 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp3;
            DataRow DrTmp5;

            int OrdNo = 0;
            int gLcDept = 0;
            long gLcAcc = 0;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR5.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE compno = 0", cn);
                DaPR5.Fill(ds5, "POrderDF");

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdCondsGuaranty  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TblConds");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDFShipDate  WHERE compno = 0", cn);
                DaPR2.Fill(ds2, "TmpDelv");

                DaPR3.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ord_POAttachments  WHERE compno = 0", cn);
                DaPR3.Fill(ds3, "Attachments");

                DaPR4.SelectCommand = new SqlCommand("SELECT * FROM DBArchives.dbo.Ord_T_AttachmentFiles  WHERE compno = 0", cn);
                DaPR4.Fill(ds4, "Archive");

                transaction = cn.BeginTransaction();



                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Ord_AppPreOrderHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdHF.OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd.Parameters.Add(new SqlParameter("@OperationDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd.Parameters.Add(new SqlParameter("@OperationType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@OrderPurpose", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@OrdSource", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@OrderCateg", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@BuyerNo", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@OrdOrg", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = BU;
                    cmd.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = true;
                    cmd.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = true;
                    cmd.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = "إنشاء طلب شراء من الويب";
                    cmd.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@BU", SqlDbType.SmallInt)).Value = BU;
                    cmd.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@AgreeYear", SqlDbType.Int)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@AgreeNo", SqlDbType.BigInt)).Value = 0;

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
                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -12)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = Resources.Resource.errorPurchOrderSerial }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    long LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);
                    OrdNo = Convert.ToInt32(cmd.Parameters["@UsedOrdNo"].Value);
                    foreach (OrderDF item in OrdDF)
                    {
                        InvItemsMF MF = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.ItemNo).FirstOrDefault();

                        DrTmp5 = ds5.Tables["POrderDF"].NewRow();
                        DrTmp5.BeginEdit();
                        DrTmp5["CompNo"] = company.comp_num;
                        DrTmp5["OrdYear"] = OrdHF.OrdYear;
                        DrTmp5["OrderNo"] = OrdNo;
                        DrTmp5["ItemNo"] = item.ItemNo;
                        DrTmp5["NSItemNo"] = 0;
                        DrTmp5["Price"] = item.Price;
                        DrTmp5["ExtraPrice"] = 0;
                        DrTmp5["Qty"] = item.Qty.Value.ToString("0.000");
                        DrTmp5["Qty2"] = item.Qty2.Value.ToString("0.000");
                        DrTmp5["Bonus"] = item.Bonus.Value.ToString("0.000");
                        DrTmp5["TotalValue"] = Convert.ToDouble(item.Qty.Value.ToString("0.000")) * Convert.ToDouble(item.Price);
                        DrTmp5["Confirmation"] = 1;
                        DrTmp5["ConfirmQty"] = item.Qty.Value.ToString("0.000");
                        DrTmp5["Note"] = "";
                        DrTmp5["DiscPer"] = item.PerDiscount;
                        DrTmp5["ValueAfterDisc"] = item.ordamt;
                        DrTmp5["DiscAmt"] = (item.VouDiscount * (Convert.ToDouble(item.Qty.Value.ToString("0.000")) * Convert.ToDouble(item.Price))) / 100;
                        DrTmp5["UnitSerial"] = item.UnitSerial;
                        DrTmp5["ItemSr"] = i;

                        DrTmp5.EndEdit();
                        ds5.Tables["POrderDF"].Rows.Add(DrTmp5);
                        i = i + 1;
                    }

                    using (SqlCommand cmd1 = new SqlCommand())
                    {
                        cmd1.Connection = cn;
                        cmd1.CommandText = "Ord_AppPreOrderDF";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd1.Parameters.Add(new SqlParameter("@PYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd1.Parameters.Add(new SqlParameter("@NSItem", SqlDbType.VarChar, 20, "NSItemNo"));
                        cmd1.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 16, "Price"));
                        cmd1.Parameters.Add(new SqlParameter("@ExtraPrice", SqlDbType.Float, 8, "ExtraPrice"));
                        cmd1.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd1.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 8, "Qty2"));
                        cmd1.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
                        cmd1.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Float, 8, "TotalValue"));
                        cmd1.Parameters.Add(new SqlParameter("@Confirm", SqlDbType.Bit, 1, "Confirmation"));
                        cmd1.Parameters.Add(new SqlParameter("@ConfirmQty", SqlDbType.Float, 8, "ConfirmQty"));
                        cmd1.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd1.Parameters.Add(new SqlParameter("@DiscPer", SqlDbType.Money, 8, "DiscPer"));
                        cmd1.Parameters.Add(new SqlParameter("@ValueAfterDisc", SqlDbType.Float, 16, "ValueAfterDisc"));
                        cmd1.Parameters.Add(new SqlParameter("@DiscAmt", SqlDbType.Float, 16, "DiscAmt"));
                        cmd1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Transaction = transaction;
                        DaPR5.InsertCommand = cmd1;
                        try
                        {
                            DaPR5.Update(ds5, "POrderDF");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();


                using (SqlCommand CmdHead = new SqlCommand())
                {
                    CmdHead.Connection = cn;
                    CmdHead.CommandText = "Ord_AddOrderHF";
                    CmdHead.CommandType = CommandType.StoredProcedure;
                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = OrdHF.CompNo;
                    CmdHead.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdHF.OrdYear;
                    CmdHead.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
                    CmdHead.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = OrdHF.TawreedNo;

                    CmdHead.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OrdHF.OfferNo;
                    CmdHead.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = OrdHF.VendorNo;
                    CmdHead.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = OrdHF.EnterDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = OrdHF.ConfDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = OrdHF.ApprovalDate;

                    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = OrdHF.CurType;
                    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = OrdHF.PayWay;
                    CmdHead.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = OrdHF.ShipWay;
                    CmdHead.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = OrdHF.StoreNo;

                    CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = OrdHF.DlvryPlace;
                    if (OrdHF.Notes == null)
                    {
                        OrdHF.Notes = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = OrdHF.Notes;
                    if (OrdHF.QutationRef == null)
                    {
                        OrdHF.QutationRef = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = OrdHF.QutationRef;
                    if (OrdHF.DlvDays == null)
                    {
                        OrdHF.DlvDays = 0;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = OrdHF.DlvDays;
                    if (OrdHF.MainPeriod == null)
                    {
                        OrdHF.MainPeriod = 0;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = OrdHF.MainPeriod;

                    CmdHead.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = OrdHF.Vou_Tax;
                    CmdHead.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = OrdHF.Per_Tax;
                    CmdHead.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = OrdHF.TotalAmt;
                    CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = OrdHF.UnitKind;
                    CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = OrdHF.NetAmt;
                    if (OrdHF.OrderClose == null)
                    {
                        OrdHF.OrderClose = false;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = OrdHF.OrderClose;
                    CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = OrdHF.DlvState;
                    CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = 0;
                    if (OrdHF.PInCharge == null)
                    {
                        OrdHF.PInCharge = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = OrdHF.PInCharge;
                    CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = OrdHF.Vend_Dept;

                    CmdHead.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money)).Value = OrdHF.VouDiscount;
                    CmdHead.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float)).Value = OrdHF.PerDiscount;
                    CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = OrdHF.SourceType;
                    if (OrdHF.GenNote == null)
                    {
                        OrdHF.GenNote = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = OrdHF.GenNote;

                    CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                    CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = OrdHF.Approved;
                    if (OrdHF.ShipTerms == null)
                    {
                        OrdHF.ShipTerms = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@ShipTerms", SqlDbType.VarChar)).Value = OrdHF.ShipTerms;
                    CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = OrdHF.ExpShDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = OrdHF.ETA;
                    CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = OrdHF.Port;
                    if(CActive.ClientNo == 335)
                    {
                        CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value =true;
                    }
                    else if (CActive.ClientNo == 343)
                    {
                        CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = true;
                    }
                    else
                    {
                        CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = OrdHF.IsLC;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = OrdHF.FreightExpense;
                    CmdHead.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = OrdHF.ExtraExpenses;

                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@LastCost", SqlDbType.Bit)).Value = OrdHF.LastCost;
                    CmdHead.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.NVarChar)).Value = OrdHF.RefNo;

                    CmdHead.Transaction = transaction;
                    try
                    {
                        CmdHead.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    var LogID = Convert.ToString(CmdHead.Parameters["@LogID"].Value);
                    foreach (OrderDF item in OrdDF)
                    {
                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["OrderNo"] = OrdNo;
                        DrTmp["OrdYear"] = item.OrdYear;
                        DrTmp["TawreedNo"] = item.TawreedNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["ItemSr"] = item.ItemSr;
                        DrTmp["PUnit"] = item.PUnit;
                        DrTmp["PerDiscount"] = item.PerDiscount;
                        DrTmp["Bonus"] = item.Bonus;
                        DrTmp["Qty"] = item.Qty;
                        DrTmp["Qty2"] = item.Qty2;
                        DrTmp["Price"] = item.Price;
                        DrTmp["ItemTaxType"] = 1;
                        DrTmp["ItemTaxPer"] = item.ItemTaxPer;
                        DrTmp["ItemTaxVal"] = item.ItemTaxVal;
                        DrTmp["RefNo"] = item.RefNo;
                        DrTmp["ordamt"] = item.ordamt;
                        DrTmp["VouDiscount"] = item.VouDiscount;
                        DrTmp["Note"] = item.Note;
                        DrTmp["NoteDtl"] = item.NoteDtl;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["ReqDeliveryDate"] = item.ReqDeliveryDate;
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "Ord_AddOrderDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        CmdIns.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        CmdIns.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                        CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
                        CmdIns.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                        CmdIns.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 9, "ordamt"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxType", SqlDbType.Bit, 1, "ItemTaxType"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 9, "ItemTaxPer"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 9, "ItemTaxVal"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money, 9, "VouDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        CmdIns.Parameters.Add(new SqlParameter("@NoteDtl", SqlDbType.Text, 300, "NoteDtl"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqDeliveryDate", SqlDbType.SmallDateTime, 20, "ReqDeliveryDate"));
                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;

                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
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


                    if(OrdCondsGuaranty != null)
                    {
                        DrTmp1 = ds1.Tables["TblConds"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = OrdCondsGuaranty.CompNo;
                        DrTmp1["OrderNo"] = OrdNo;
                        DrTmp1["OrdYear"] = OrdCondsGuaranty.OrdYear;
                        DrTmp1["TwareedNo"] = OrdCondsGuaranty.TwareedNo;
                        if (OrdCondsGuaranty.PayCond1 == null)
                        {
                            OrdCondsGuaranty.PayCond1 = 0;
                        }
                        if (OrdCondsGuaranty.PayCond1 == 0)
                        {
                            DrTmp1["PayCond1"] = 1;
                        }
                        else
                        {
                            DrTmp1["PayCond1"] = OrdCondsGuaranty.PayCond1;
                        }
                        if (OrdCondsGuaranty.PayCond2 == null)
                        {
                            OrdCondsGuaranty.PayCond2 = 0;
                        }
                        DrTmp1["PayCond2"] = OrdCondsGuaranty.PayCond2;
                        if (OrdCondsGuaranty.PayCond3 == null)
                        {
                            OrdCondsGuaranty.PayCond3 = 0;
                        }

                        DrTmp1["PayCond3"] = OrdCondsGuaranty.PayCond3;
                        if (OrdCondsGuaranty.PayCond4 == null)
                        {
                            OrdCondsGuaranty.PayCond4 = 0;
                        }
                        DrTmp1["PayCond4"] = OrdCondsGuaranty.PayCond4;
                        if (OrdCondsGuaranty.PayCond5 == null)
                        {
                            OrdCondsGuaranty.PayCond5 = 0;
                        }
                        DrTmp1["PayCond5"] = OrdCondsGuaranty.PayCond5;
                        if (OrdCondsGuaranty.CondPerc1 == 0)
                        {
                            DrTmp1["CondPerc1"] = 100;
                        }
                        else
                        {
                            DrTmp1["CondPerc1"] = OrdCondsGuaranty.CondPerc1;
                        }
                        DrTmp1["CondPerc2"] = OrdCondsGuaranty.CondPerc2;
                        DrTmp1["CondPerc3"] = OrdCondsGuaranty.CondPerc3;
                        DrTmp1["CondPerc4"] = OrdCondsGuaranty.CondPerc4;
                        DrTmp1["CondPerc5"] = OrdCondsGuaranty.CondPerc5;

                        if (OrdCondsGuaranty.Guaranty1 == null)
                        {
                            OrdCondsGuaranty.Guaranty1 = 0;
                        }
                        DrTmp1["Guaranty1"] = OrdCondsGuaranty.Guaranty1;
                        if (OrdCondsGuaranty.Guaranty2 == null)
                        {
                            OrdCondsGuaranty.Guaranty2 = 0;
                        }
                        DrTmp1["Guaranty2"] = OrdCondsGuaranty.Guaranty2;
                        if (OrdCondsGuaranty.Guaranty3 == null)
                        {
                            OrdCondsGuaranty.Guaranty3 = 0;
                        }
                        DrTmp1["Guaranty3"] = OrdCondsGuaranty.Guaranty3;
                        if (OrdCondsGuaranty.Guaranty4 == null)
                        {
                            OrdCondsGuaranty.Guaranty4 = 0;
                        }
                        DrTmp1["Guaranty4"] = OrdCondsGuaranty.Guaranty4;
                        if (OrdCondsGuaranty.Guaranty5 == null)
                        {
                            OrdCondsGuaranty.Guaranty5 = 0;
                        }
                        DrTmp1["Guaranty5"] = OrdCondsGuaranty.Guaranty5;
                        DrTmp1["GuarantyPerc1"] = OrdCondsGuaranty.GuarantyPerc1;
                        DrTmp1["GuarantyPerc2"] = OrdCondsGuaranty.GuarantyPerc2;
                        DrTmp1["GuarantyPerc3"] = OrdCondsGuaranty.GuarantyPerc3;
                        DrTmp1["GuarantyPerc4"] = OrdCondsGuaranty.GuarantyPerc4;
                        DrTmp1["GuarantyPerc5"] = OrdCondsGuaranty.GuarantyPerc5;
                        DrTmp1.EndEdit();
                        ds1.Tables["TblConds"].Rows.Add(DrTmp1);

                        using (SqlCommand CmdInsConds = new SqlCommand())
                        {
                            CmdInsConds.Connection = cn;
                            CmdInsConds.CommandText = "Ord_AddCondsGuaranty";
                            CmdInsConds.CommandType = CommandType.StoredProcedure;
                            CmdInsConds.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@TwareedNo", SqlDbType.VarChar, 20, "TwareedNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond1", SqlDbType.Int, 8, "PayCond1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond2", SqlDbType.Int, 8, "PayCond2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond3", SqlDbType.Int, 8, "PayCond3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond4", SqlDbType.Int, 8, "PayCond4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond5", SqlDbType.Int, 8, "PayCond5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc1", SqlDbType.Float, 9, "CondPerc1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc2", SqlDbType.Float, 9, "CondPerc2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc3", SqlDbType.Float, 9, "CondPerc3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc4", SqlDbType.Float, 9, "CondPerc4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc5", SqlDbType.Float, 9, "CondPerc5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty1", SqlDbType.Int, 8, "Guaranty1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty2", SqlDbType.Int, 8, "Guaranty2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty3", SqlDbType.Int, 8, "Guaranty3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty4", SqlDbType.Int, 8, "Guaranty4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty5", SqlDbType.Int, 8, "Guaranty5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc1", SqlDbType.Float, 9, "GuarantyPerc1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc2", SqlDbType.Float, 9, "GuarantyPerc2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc3", SqlDbType.Float, 9, "GuarantyPerc3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc4", SqlDbType.Float, 9, "GuarantyPerc4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc5", SqlDbType.Float, 9, "GuarantyPerc5"));
                            CmdInsConds.Transaction = transaction;
                            DaPR1.InsertCommand = CmdInsConds;

                            try
                            {
                                DaPR1.Update(ds1, "TblConds");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    


                    using (SqlCommand CmdChkQty = new SqlCommand())
                    {
                        CmdChkQty.Connection = cn;
                        CmdChkQty.CommandText = "Ord_ChkQuantities";
                        CmdChkQty.CommandType = CommandType.StoredProcedure;
                        CmdChkQty.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        CmdChkQty.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                        CmdChkQty.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdNo;
                        CmdChkQty.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = "";
                        CmdChkQty.Parameters.Add("@ExtraRecPer", SqlDbType.Int).Value = -1;
                        CmdChkQty.Parameters.Add("@Rows", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        CmdChkQty.Transaction = transaction;
                        CmdChkQty.ExecuteNonQuery();

                        if (Convert.ToInt16(CmdChkQty.Parameters["@Rows"].Value) > 0)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "en")
                            {
                                return Json(new { error = "Cant exceed approved Qty." }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "لايمكن تجاوز الكمية الموافق عليها " }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    using (SqlCommand CmdDelAttachments = new SqlCommand())
                    {
                        CmdDelAttachments.Connection = cn;
                        CmdDelAttachments.CommandText = "Ord_DelAttachmentsInfo";
                        CmdDelAttachments.CommandType = CommandType.StoredProcedure;
                        CmdDelAttachments.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        CmdDelAttachments.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                        CmdDelAttachments.Parameters.Add("@OrderNo", SqlDbType.BigInt).Value = OrdNo;
                        CmdDelAttachments.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdHF.TawreedNo;
                        CmdDelAttachments.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        CmdDelAttachments.Transaction = transaction;
                        CmdDelAttachments.ExecuteNonQuery();
                    }


                    if (Ord_POAttachments != null)
                    {
                        foreach (Ord_POAttachments item in Ord_POAttachments)
                        {
                            DrTmp3 = ds3.Tables["Attachments"].NewRow();
                            DrTmp3.BeginEdit();
                            DrTmp3["CompNo"] = item.CompNo;
                            DrTmp3["OrdNo"] = OrdNo;
                            DrTmp3["OrdYear"] = item.OrdYear;
                            DrTmp3["TawreedNo"] = item.TawreedNo;
                            DrTmp3["Description"] = item.Description;
                            DrTmp3["DescriptionEng"] = item.DescriptionEng;
                            DrTmp3.EndEdit();
                            ds3.Tables["Attachments"].Rows.Add(DrTmp3);
                        }

                        using (SqlCommand CmdAttach = new SqlCommand())
                        {
                            CmdAttach.Connection = cn;
                            CmdAttach.CommandText = "Ord_AddAttachmentsInfo";
                            CmdAttach.CommandType = CommandType.StoredProcedure;
                            CmdAttach.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.BigInt, 8, "OrdNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                            CmdAttach.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 300, "Description"));
                            CmdAttach.Parameters.Add(new SqlParameter("@DescriptionEng", SqlDbType.VarChar, 300, "DescriptionEng"));
                            CmdAttach.Transaction = transaction;
                            DaPR3.InsertCommand = CmdAttach;
                            try
                            {
                                DaPR3.Update(ds3, "Attachments");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }


                    List<UploadFilePO> upFile = (List<UploadFilePO>)Session["FilesPO"];
                    if (upFile != null)
                    {
                        if (upFile.Count != 0)
                        {
                            if (DocumentArchive != null)
                            {
                                int serial = 0;
                                foreach (UploadFilePO item in DocumentArchive)
                                {
                                    UploadFilePO fl = upFile.Where(x => x.OrderYear == OrdHF.OrdYear && x.OrderNo == 0 && x.TawreedNo == OrdHF.TawreedNo && x.FileId == item.FileId).FirstOrDefault();
                                    serial += 1;
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Insert Into DBArchives.dbo.Ord_T_AttachmentFiles " +
                                                     " (CompNo, Ord_Year, Ord_Type, Ord_No, Serial, TawreedNo, InvSr, Description, DateUploded, FileName, FileSize, ContentType, FileData) " +
                                                     " VALUES " +
                                                     "(@CompNo, @Ord_Year, @Ord_Type, @Ord_No, @Serial, @TawreedNo, @InvSr, @Description, @DateUploded, @FileName, @FileSize, @ContentType, @FileData)";


                                        CmdFiles.Parameters.Add("@CompNo", SqlDbType.SmallInt, 4).Value = company.comp_num;
                                        CmdFiles.Parameters.Add("@Ord_Year", SqlDbType.SmallInt, 4).Value = OrdHF.OrdYear;
                                        CmdFiles.Parameters.Add("@Ord_Type", SqlDbType.SmallInt, 4).Value = 1;
                                        CmdFiles.Parameters.Add("@Ord_No", SqlDbType.Int, 8).Value = OrdNo;
                                        CmdFiles.Parameters.Add("@Serial", SqlDbType.BigInt, 20).Value = serial;
                                        CmdFiles.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = OrdHF.TawreedNo;
                                        CmdFiles.Parameters.Add("@InvSr", SqlDbType.SmallInt, 4).Value = 0;
                                        if (item.Description == null)
                                        {
                                            item.Description = "";
                                        }
                                        CmdFiles.Parameters.Add("@Description", SqlDbType.VarChar).Value = item.Description;
                                        CmdFiles.Parameters.Add("@DateUploded", SqlDbType.SmallDateTime).Value = item.DateUploded;
                                        CmdFiles.Parameters.Add("@FileName", SqlDbType.VarChar).Value = fl.FileName;
                                        CmdFiles.Parameters.Add("@FileSize", SqlDbType.Int, 8).Value = fl.FileSize;
                                        CmdFiles.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = item.ContentType;
                                        CmdFiles.Parameters.Add("@FileData", SqlDbType.Image).Value = fl.File;
                                        CmdFiles.Transaction = transaction;
                                        try
                                        {
                                            CmdFiles.ExecuteNonQuery();
                                            fl.OrderNo = OrdNo;
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
                        }
                    }
                }

                bool InsertWorkFlow = AddWorkFlowLog(3, BU, OrdHF.OrdYear, OrdNo.ToString(), OrdHF.TawreedNo, 1, OrdHF.NetAmt, transaction, cn); ;
                if (InsertWorkFlow == false)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { OrdYear = OrdHF.OrdYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit_PurchaseOrder(int BU, OrderHF OrdHF, List<OrderDF> OrdDF,  OrdCondsGuaranty OrdCondsGuaranty, List<Ord_POAttachments> Ord_POAttachments, List<UploadFilePO> DocumentArchive)
        {
            int i = 1;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
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
            DataRow DrTmp4;


            int gLcDept = 0;
            long gLcAcc = 0;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdCondsGuaranty  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TblConds");

                DaPR3.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ord_POAttachments  WHERE compno = 0", cn);
                DaPR3.Fill(ds3, "Attachments");

                DaPR4.SelectCommand = new SqlCommand("SELECT * FROM DBArchives.dbo.Ord_T_AttachmentFiles  WHERE compno = 0", cn);
                DaPR4.Fill(ds4, "Archive");

                transaction = cn.BeginTransaction();

                using (SqlCommand CmdDel = new SqlCommand())
                {
                    CmdDel.Connection = cn;
                    CmdDel.CommandText = "Ord_DelOrder";
                    CmdDel.CommandType = CommandType.StoredProcedure;
                    CmdDel.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    CmdDel.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                    CmdDel.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdHF.OrderNo;
                    CmdDel.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdHF.TawreedNo;
                    CmdDel.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdDel.Parameters.Add("@Frmstat", SqlDbType.SmallInt).Value = 2;
                    CmdDel.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    CmdDel.Transaction = transaction;
                    CmdDel.ExecuteNonQuery();
                }

                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = cn;
                    Cmd.CommandText = "Ord_GetCompMF";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Transaction = transaction;
                    SqlDataReader rdr = Cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            gLcDept = Convert.ToInt32(rdr["LcDept"]);
                            gLcAcc = Convert.ToInt64(rdr["LcAcc"]);
                        }
                    }
                    rdr.Close();
                }

                using (SqlCommand CmdHead = new SqlCommand())
                {
                    CmdHead.Connection = cn;
                    CmdHead.CommandText = "Ord_AddOrderHF";
                    CmdHead.CommandType = CommandType.StoredProcedure;
                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = OrdHF.CompNo;
                    CmdHead.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdHF.OrdYear;
                    CmdHead.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdHF.OrderNo;
                    CmdHead.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = OrdHF.TawreedNo;

                    CmdHead.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OrdHF.OfferNo;
                    CmdHead.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = OrdHF.VendorNo;
                    CmdHead.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = OrdHF.EnterDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = OrdHF.ConfDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = OrdHF.ApprovalDate;

                    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = OrdHF.CurType;
                    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = OrdHF.PayWay;
                    CmdHead.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = OrdHF.ShipWay;
                    CmdHead.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = OrdHF.StoreNo;

                    CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = OrdHF.DlvryPlace;
                    if (OrdHF.Notes == null)
                    {
                        OrdHF.Notes = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = OrdHF.Notes;
                    if (OrdHF.QutationRef == null)
                    {
                        OrdHF.QutationRef = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = OrdHF.QutationRef;
                    CmdHead.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = OrdHF.DlvDays;
                    CmdHead.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = OrdHF.MainPeriod;

                    CmdHead.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = OrdHF.Vou_Tax;
                    CmdHead.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = OrdHF.Per_Tax;
                    CmdHead.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = OrdHF.TotalAmt;
                    CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = OrdHF.UnitKind;
                    CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = OrdHF.NetAmt;
                    if (OrdHF.OrderClose == null)
                    {
                        OrdHF.OrderClose = false;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = OrdHF.OrderClose;
                    CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = OrdHF.DlvState;
                    CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = 0;
                    if (OrdHF.PInCharge == null)
                    {
                        OrdHF.PInCharge = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = OrdHF.PInCharge;
                    CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = OrdHF.Vend_Dept;

                    CmdHead.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money)).Value = OrdHF.VouDiscount;
                    CmdHead.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float)).Value = OrdHF.PerDiscount;
                    CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = OrdHF.SourceType;
                    if (OrdHF.GenNote == null)
                    {
                        OrdHF.GenNote = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = OrdHF.GenNote;

                    CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                    CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = OrdHF.Approved;
                    if (OrdHF.ShipTerms == null)
                    {
                        OrdHF.ShipTerms = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@ShipTerms", SqlDbType.VarChar)).Value = OrdHF.ShipTerms;
                    CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = OrdHF.ExpShDate;
                    CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = OrdHF.ETA;
                    CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = OrdHF.Port;
                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = OrdHF.IsLC;

                    CmdHead.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = OrdHF.FreightExpense;
                    CmdHead.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = OrdHF.ExtraExpenses;

                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 2;
                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@LastCost", SqlDbType.Bit)).Value = OrdHF.LastCost;
                    CmdHead.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.NVarChar)).Value = OrdHF.RefNo;

                    CmdHead.Transaction = transaction;
                    try
                    {
                        CmdHead.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    var LogID = Convert.ToString(CmdHead.Parameters["@LogID"].Value);

                    i = 1;
                    foreach (OrderDF item in OrdDF)
                    {
                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["OrderNo"] = item.OrderNo;
                        DrTmp["OrdYear"] = item.OrdYear;
                        DrTmp["TawreedNo"] = item.TawreedNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["ItemSr"] = i;
                        DrTmp["PUnit"] = item.PUnit;
                        DrTmp["PerDiscount"] = item.PerDiscount;
                        DrTmp["Bonus"] = item.Bonus;
                        DrTmp["Qty"] = item.Qty;
                        DrTmp["Qty2"] = item.Qty2;
                        DrTmp["Price"] = item.Price;
                        DrTmp["ItemTaxType"] = 1;
                        DrTmp["ItemTaxPer"] = item.ItemTaxPer;
                        DrTmp["ItemTaxVal"] = item.ItemTaxVal;
                        DrTmp["RefNo"] = item.RefNo;
                        DrTmp["ordamt"] = item.ordamt;
                        DrTmp["VouDiscount"] = item.VouDiscount;
                        DrTmp["Note"] = item.Note;
                        DrTmp["NoteDtl"] = item.NoteDtl;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["ReqDeliveryDate"] = item.ReqDeliveryDate;
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                        i++;
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "Ord_AddOrderDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        CmdIns.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        CmdIns.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                        CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
                        CmdIns.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                        CmdIns.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 9, "ordamt"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxType", SqlDbType.Bit, 1, "ItemTaxType"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 9, "ItemTaxPer"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 9, "ItemTaxVal"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money, 9, "VouDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        CmdIns.Parameters.Add(new SqlParameter("@NoteDtl", SqlDbType.Text, 300, "NoteDtl"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqDeliveryDate", SqlDbType.SmallDateTime, 20, "ReqDeliveryDate"));
                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;

                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
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

                    if(OrdCondsGuaranty != null)
                    {
                        DrTmp1 = ds1.Tables["TblConds"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = OrdCondsGuaranty.CompNo;
                        DrTmp1["OrderNo"] = OrdCondsGuaranty.OrderNo;
                        DrTmp1["OrdYear"] = OrdCondsGuaranty.OrdYear;
                        DrTmp1["TwareedNo"] = OrdCondsGuaranty.TwareedNo;
                        if (OrdCondsGuaranty.PayCond1 == null)
                        {
                            OrdCondsGuaranty.PayCond1 = 0;
                        }
                        if (OrdCondsGuaranty.PayCond1 == 0)
                        {
                            DrTmp1["PayCond1"] = 1;
                        }
                        else
                        {
                            DrTmp1["PayCond1"] = OrdCondsGuaranty.PayCond1;
                        }
                        if (OrdCondsGuaranty.PayCond2 == null)
                        {
                            OrdCondsGuaranty.PayCond2 = 0;
                        }
                        DrTmp1["PayCond2"] = OrdCondsGuaranty.PayCond2;
                        if (OrdCondsGuaranty.PayCond3 == null)
                        {
                            OrdCondsGuaranty.PayCond3 = 0;
                        }

                        DrTmp1["PayCond3"] = OrdCondsGuaranty.PayCond3;
                        if (OrdCondsGuaranty.PayCond4 == null)
                        {
                            OrdCondsGuaranty.PayCond4 = 0;
                        }
                        DrTmp1["PayCond4"] = OrdCondsGuaranty.PayCond4;
                        if (OrdCondsGuaranty.PayCond5 == null)
                        {
                            OrdCondsGuaranty.PayCond5 = 0;
                        }
                        DrTmp1["PayCond5"] = OrdCondsGuaranty.PayCond5;
                        if (OrdCondsGuaranty.CondPerc1 == 0)
                        {
                            DrTmp1["CondPerc1"] = 100;
                        }
                        else
                        {
                            DrTmp1["CondPerc1"] = OrdCondsGuaranty.CondPerc1;
                        }
                        DrTmp1["CondPerc2"] = OrdCondsGuaranty.CondPerc2;
                        DrTmp1["CondPerc3"] = OrdCondsGuaranty.CondPerc3;
                        DrTmp1["CondPerc4"] = OrdCondsGuaranty.CondPerc4;
                        DrTmp1["CondPerc5"] = OrdCondsGuaranty.CondPerc5;

                        if (OrdCondsGuaranty.Guaranty1 == null)
                        {
                            OrdCondsGuaranty.Guaranty1 = 0;
                        }
                        DrTmp1["Guaranty1"] = OrdCondsGuaranty.Guaranty1;
                        if (OrdCondsGuaranty.Guaranty2 == null)
                        {
                            OrdCondsGuaranty.Guaranty2 = 0;
                        }
                        DrTmp1["Guaranty2"] = OrdCondsGuaranty.Guaranty2;
                        if (OrdCondsGuaranty.Guaranty3 == null)
                        {
                            OrdCondsGuaranty.Guaranty3 = 0;
                        }
                        DrTmp1["Guaranty3"] = OrdCondsGuaranty.Guaranty3;
                        if (OrdCondsGuaranty.Guaranty4 == null)
                        {
                            OrdCondsGuaranty.Guaranty4 = 0;
                        }
                        DrTmp1["Guaranty4"] = OrdCondsGuaranty.Guaranty4;
                        if (OrdCondsGuaranty.Guaranty5 == null)
                        {
                            OrdCondsGuaranty.Guaranty5 = 0;
                        }
                        DrTmp1["Guaranty5"] = OrdCondsGuaranty.Guaranty5;
                        DrTmp1["GuarantyPerc1"] = OrdCondsGuaranty.GuarantyPerc1;
                        DrTmp1["GuarantyPerc2"] = OrdCondsGuaranty.GuarantyPerc2;
                        DrTmp1["GuarantyPerc3"] = OrdCondsGuaranty.GuarantyPerc3;
                        DrTmp1["GuarantyPerc4"] = OrdCondsGuaranty.GuarantyPerc4;
                        DrTmp1["GuarantyPerc5"] = OrdCondsGuaranty.GuarantyPerc5;
                        DrTmp1.EndEdit();
                        ds1.Tables["TblConds"].Rows.Add(DrTmp1);

                        using (SqlCommand CmdInsConds = new SqlCommand())
                        {
                            CmdInsConds.Connection = cn;
                            CmdInsConds.CommandText = "Ord_AddCondsGuaranty";
                            CmdInsConds.CommandType = CommandType.StoredProcedure;
                            CmdInsConds.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@TwareedNo", SqlDbType.VarChar, 20, "TwareedNo"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond1", SqlDbType.Int, 8, "PayCond1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond2", SqlDbType.Int, 8, "PayCond2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond3", SqlDbType.Int, 8, "PayCond3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond4", SqlDbType.Int, 8, "PayCond4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Cond5", SqlDbType.Int, 8, "PayCond5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc1", SqlDbType.Float, 9, "CondPerc1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc2", SqlDbType.Float, 9, "CondPerc2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc3", SqlDbType.Float, 9, "CondPerc3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc4", SqlDbType.Float, 9, "CondPerc4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc5", SqlDbType.Float, 9, "CondPerc5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty1", SqlDbType.Int, 8, "Guaranty1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty2", SqlDbType.Int, 8, "Guaranty2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty3", SqlDbType.Int, 8, "Guaranty3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty4", SqlDbType.Int, 8, "Guaranty4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty5", SqlDbType.Int, 8, "Guaranty5"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc1", SqlDbType.Float, 9, "GuarantyPerc1"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc2", SqlDbType.Float, 9, "GuarantyPerc2"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc3", SqlDbType.Float, 9, "GuarantyPerc3"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc4", SqlDbType.Float, 9, "GuarantyPerc4"));
                            CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc5", SqlDbType.Float, 9, "GuarantyPerc5"));
                            CmdInsConds.Transaction = transaction;
                            DaPR1.InsertCommand = CmdInsConds;

                            try
                            {
                                DaPR1.Update(ds1, "TblConds");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    


                    //using (SqlCommand CmdChkQty = new SqlCommand())
                    //{
                    //    CmdChkQty.Connection = cn;
                    //    CmdChkQty.CommandText = "Ord_ChkQuantities";
                    //    CmdChkQty.CommandType = CommandType.StoredProcedure;
                    //    CmdChkQty.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    //    CmdChkQty.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                    //    CmdChkQty.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdHF.OrderNo;
                    //    CmdChkQty.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdHF.TawreedNo;
                    //    CmdChkQty.Parameters.Add("@ExtraRecPer", SqlDbType.Int).Value = -1;
                    //    CmdChkQty.Parameters.Add("@Rows", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    //    CmdChkQty.Transaction = transaction;
                    //    CmdChkQty.ExecuteNonQuery();

                    //    if (Convert.ToInt16(CmdChkQty.Parameters["@Rows"].Value) > 0)
                    //    {
                    //        transaction.Rollback();
                    //        cn.Dispose();
                    //        if (Language == "en")
                    //        {
                    //            return Json(new { error = "Cant exceed approved Qty." }, JsonRequestBehavior.AllowGet);
                    //        }
                    //        else
                    //        {
                    //            return Json(new { error = "لايمكن تجاوز الكمية الموافق عليها " }, JsonRequestBehavior.AllowGet);
                    //        }
                    //    }
                    //}

                    using (SqlCommand CmdDelAttachments = new SqlCommand())
                    {
                        CmdDelAttachments.Connection = cn;
                        CmdDelAttachments.CommandText = "Ord_DelAttachmentsInfo";
                        CmdDelAttachments.CommandType = CommandType.StoredProcedure;
                        CmdDelAttachments.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        CmdDelAttachments.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                        CmdDelAttachments.Parameters.Add("@OrderNo", SqlDbType.BigInt).Value = OrdHF.OrderNo;
                        CmdDelAttachments.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdHF.TawreedNo;
                        CmdDelAttachments.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        CmdDelAttachments.Transaction = transaction;
                        CmdDelAttachments.ExecuteNonQuery();
                    }


                    if (Ord_POAttachments != null)
                    {
                        foreach (Ord_POAttachments item in Ord_POAttachments)
                        {
                            DrTmp3 = ds3.Tables["Attachments"].NewRow();
                            DrTmp3.BeginEdit();
                            DrTmp3["CompNo"] = item.CompNo;
                            DrTmp3["OrdNo"] = item.OrdNo;
                            DrTmp3["OrdYear"] = item.OrdYear;
                            DrTmp3["TawreedNo"] = item.TawreedNo;
                            DrTmp3["Description"] = item.Description;
                            DrTmp3["DescriptionEng"] = item.DescriptionEng;
                            DrTmp3.EndEdit();
                            ds3.Tables["Attachments"].Rows.Add(DrTmp3);
                        }

                        using (SqlCommand CmdAttach = new SqlCommand())
                        {
                            CmdAttach.Connection = cn;
                            CmdAttach.CommandText = "Ord_AddAttachmentsInfo";
                            CmdAttach.CommandType = CommandType.StoredProcedure;
                            CmdAttach.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.BigInt, 8, "OrdNo"));
                            CmdAttach.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                            CmdAttach.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 300, "Description"));
                            CmdAttach.Parameters.Add(new SqlParameter("@DescriptionEng", SqlDbType.VarChar, 300, "DescriptionEng"));
                            CmdAttach.Transaction = transaction;
                            DaPR3.InsertCommand = CmdAttach;
                            try
                            {
                                DaPR3.Update(ds3, "Attachments");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }


                    List<UploadFilePO> upFile = (List<UploadFilePO>)Session["FilesPO"];
                    if (upFile != null)
                    {
                        if (upFile.Count != 0)
                        {
                            if (DocumentArchive != null)
                            {
                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Delete FROM DBArchives.dbo.Ord_T_AttachmentFiles " +
                                    "where CompNo=@CompNo and Ord_Year=@Ord_Year and  Ord_Type=@Ord_Type And Ord_No=@Ord_No And TawreedNo = @TawreedNo";

                                    CmdFiles.Parameters.Add("@CompNo", SqlDbType.SmallInt, 4).Value = company.comp_num;
                                    CmdFiles.Parameters.Add("@Ord_Year", SqlDbType.SmallInt, 4).Value = OrdHF.OrdYear;
                                    CmdFiles.Parameters.Add("@Ord_Type", SqlDbType.SmallInt, 4).Value = 1;
                                    CmdFiles.Parameters.Add("@Ord_No", SqlDbType.Int, 8).Value = OrdHF.OrderNo;
                                    CmdFiles.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = OrdHF.TawreedNo;
                                    CmdFiles.Transaction = transaction;
                                    try
                                    {
                                        CmdFiles.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        cn.Dispose();
                                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                int serial = 0;
                                foreach (UploadFilePO item in DocumentArchive)
                                {
                                    UploadFilePO fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.FileId == item.FileId).FirstOrDefault();
                                    serial += 1;
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Insert Into DBArchives.dbo.Ord_T_AttachmentFiles " +
                                                     " (CompNo, Ord_Year, Ord_Type, Ord_No, Serial, TawreedNo, InvSr, Description, DateUploded, FileName, FileSize, ContentType, FileData) " +
                                                     " VALUES " +
                                                     "(@CompNo, @Ord_Year, @Ord_Type, @Ord_No, @Serial, @TawreedNo, @InvSr, @Description, @DateUploded, @FileName, @FileSize, @ContentType, @FileData)";


                                        CmdFiles.Parameters.Add("@CompNo", SqlDbType.SmallInt, 4).Value = company.comp_num;
                                        CmdFiles.Parameters.Add("@Ord_Year", SqlDbType.SmallInt, 4).Value = OrdHF.OrdYear;
                                        CmdFiles.Parameters.Add("@Ord_Type", SqlDbType.SmallInt, 4).Value = 1;
                                        CmdFiles.Parameters.Add("@Ord_No", SqlDbType.Int, 8).Value = OrdHF.OrderNo;
                                        CmdFiles.Parameters.Add("@Serial", SqlDbType.BigInt, 20).Value = serial;
                                        CmdFiles.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = OrdHF.TawreedNo;
                                        CmdFiles.Parameters.Add("@InvSr", SqlDbType.SmallInt, 4).Value = 0;
                                        if (item.Description == null)
                                        {
                                            item.Description = "";
                                        }
                                        CmdFiles.Parameters.Add("@Description", SqlDbType.VarChar).Value = item.Description;
                                        CmdFiles.Parameters.Add("@DateUploded", SqlDbType.SmallDateTime).Value = item.DateUploded;
                                        CmdFiles.Parameters.Add("@FileName", SqlDbType.VarChar).Value = fl.FileName;
                                        CmdFiles.Parameters.Add("@FileSize", SqlDbType.Int, 8).Value = fl.FileSize;
                                        CmdFiles.Parameters.Add("@ContentType", SqlDbType.VarChar).Value = item.ContentType;
                                        CmdFiles.Parameters.Add("@FileData", SqlDbType.Image).Value = fl.File;
                                        CmdFiles.Transaction = transaction;
                                        try
                                        {
                                            CmdFiles.ExecuteNonQuery();
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
                        }
                    }
                }

                bool InsertWorkFlow = AddWorkFlowLog(3, BU, OrdHF.OrdYear, OrdHF.OrderNo.ToString(), OrdHF.TawreedNo, 2, OrdHF.NetAmt, transaction, cn); ;
                if (InsertWorkFlow == false)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { TawreedNo = OrdHF.TawreedNo, OrdNo = OrdHF.OrderNo, OrdYear = OrdHF.OrdYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_PurchaseOrder(int OrdYear, int OrderNo, string TawreedNo)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                transaction = cn.BeginTransaction();

                using (SqlCommand CmdDel = new SqlCommand())
                {
                    CmdDel.Connection = cn;
                    CmdDel.CommandText = "Ord_DelOrder";
                    CmdDel.CommandType = CommandType.StoredProcedure;
                    CmdDel.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    CmdDel.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdYear;
                    CmdDel.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    CmdDel.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    CmdDel.Parameters.Add("@ErrNo", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    CmdDel.Parameters.Add("@Frmstat", SqlDbType.SmallInt).Value = 3;
                    CmdDel.Parameters.Add("@UserID", SqlDbType.VarChar).Value = me.UserID;
                    CmdDel.Transaction = transaction;
                    try
                    {
                        CmdDel.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }

                using (SqlCommand CmdDel = new SqlCommand())
                {
                    CmdDel.Connection = cn;
                    CmdDel.CommandText = "DELETE FROM OrdLCDetails WHERE(CompNo = @CompNo) AND (OrdYear = @OrdYear) AND (OrdNo = @OrdNo) AND (TawreedNo = @TawreedNo) ";
                    CmdDel.CommandType = CommandType.Text;
                    CmdDel.Parameters.Add("@CompNo", SqlDbType.SmallInt, 4).Value = company.comp_num;
                    CmdDel.Parameters.Add("@OrdNo", SqlDbType.Int, 8).Value = OrderNo;
                    CmdDel.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = TawreedNo;
                    CmdDel.Parameters.Add("@OrdYear", SqlDbType.SmallInt, 4).Value = OrdYear;
                    CmdDel.Transaction = transaction;
                    try
                    {
                        CmdDel.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ord_DelAttachmentsInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = TawreedNo;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int, 8).Value = OrderNo;
                    DelCmd.Parameters.Add("@OrdYear", SqlDbType.SmallInt, 4).Value = OrdYear;
                    DelCmd.Parameters.Add("@ErrNo", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                using (SqlCommand CmdFiles = new SqlCommand())
                {
                    CmdFiles.Connection = cn;
                    CmdFiles.CommandText = "Delete FROM DBArchives.dbo.Ord_T_AttachmentFiles " +
                    "where CompNo=@CompNo and Ord_Year=@Ord_Year and  Ord_Type=@Ord_Type And Ord_No=@Ord_No And TawreedNo = @TawreedNo";

                    CmdFiles.Parameters.Add("@CompNo", SqlDbType.SmallInt, 4).Value = company.comp_num;
                    CmdFiles.Parameters.Add("@Ord_Year", SqlDbType.SmallInt, 4).Value = OrdYear;
                    CmdFiles.Parameters.Add("@Ord_Type", SqlDbType.SmallInt, 4).Value = 1;
                    CmdFiles.Parameters.Add("@Ord_No", SqlDbType.Int, 8).Value = OrderNo;
                    CmdFiles.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 20).Value = TawreedNo;
                    CmdFiles.Transaction = transaction;
                    try
                    {
                        CmdFiles.ExecuteNonQuery();
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
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStutusAction(bool StutusAction, int OrdYear, int OrderNo, string TawreedNo)
        {
            OrderHF hf = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();
            hf.IsExport = StutusAction;
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public bool AddWorkFlowLog(int FID, int BU, int TrYear, string TrNo, string TawreedNo, int FStat, double? TrAmnt, SqlTransaction MyTrans, SqlConnection co)
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
                Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
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

        public string InsertFile()
        {
            List<UploadFilePO> lstFile = null;
            if (Session["FilesPO"] == null)
            {
                lstFile = new List<UploadFilePO>();
            }
            else
            {
                lstFile = (List<UploadFilePO>)Session["FilesPO"];
            }
            UploadFilePO SinglFile = new UploadFilePO();
            var Id = Request.Params["Id"];
            var Ordyear = Request.Params["Ordyear"];
            var OrdNo = Request.Params["OrdNo"];
            var TawreedNo = Request.Params["TawreedNo"];

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt32(Ordyear) && a.OrderNo == Convert.ToInt32(OrdNo) && a.TawreedNo == Convert.ToString(TawreedNo) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFilePO();
                        SinglFile.OrderYear = Convert.ToInt32(Ordyear);
                        SinglFile.OrderNo = Convert.ToInt32(OrdNo);
                        SinglFile.TawreedNo = Convert.ToString(TawreedNo);
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
                        SinglFile.OrderYear = Convert.ToInt32(Ordyear);
                        SinglFile.OrderNo = Convert.ToInt32(OrdNo);
                        SinglFile.TawreedNo = Convert.ToString(TawreedNo);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }

                }
                Session["FilesPO"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int Ordyear, int OrdNo, string TawreedNo)
        {
            List<UploadFilePO> lstFile = null;
            if (Session["FilesPO"] == null)
            {
                lstFile = new List<UploadFilePO>();
            }
            else
            {
                lstFile = (List<UploadFilePO>)Session["FilesPO"];
            }
            UploadFilePO SinglFile = new UploadFilePO();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt32(Ordyear) && a.OrderNo == Convert.ToInt32(OrdNo) && a.TawreedNo == Convert.ToString(TawreedNo) && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesPO"] = lstFile;
            }

            return "done";
        }


    }
}