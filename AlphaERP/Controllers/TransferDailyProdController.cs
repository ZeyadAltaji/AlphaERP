using DocumentFormat.OpenXml.Wordprocessing;
using AlphaERP.Models;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class TransferDailyProdController : controller
    {
        string error = "";
        string fincode = "";
        long gConsVouNo;
        DataSet TmpDs = new DataSet();
        SqlDataAdapter DaPR = new SqlDataAdapter();
        DataRow DrTmp;
        // GET: TransferDailyProd
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TransDailyProd(int year)
        {
            bool? TransByOrder = db.ProdCost_CompPara.Where(x => x.CompNo == company.comp_num).FirstOrDefault().TransByOrder;
            if(TransByOrder == null)
            {
                TransByOrder = false;
            }
            IList<TransferDailyProd> Trans_DailyProd;
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_LoadPostedProdReportToTransfer";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    if(TransByOrder == false)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            if(TransByOrder == false)
            {
                Trans_DailyProd = Dt.AsEnumerable().Select(row => new TransferDailyProd
                {
                    TranByOrder = TransByOrder,
                    Transyear = year,
                    TransferFlag = row.Field<bool>("TransferFlag"),
                    PostFlag = row.Field<bool>("PostFlag"),
                    ProdPrepYear = row.Field<short>("ProdPrepYear"),
                    ProdRecYear = row.Field<short>("ProdRecYear"),
                    ConsVouYear = row.Field<short>("ConsVouYear"),
                    ConsVouNo = row.Field<int>("ConsVouNo"),
                    ProdPrepNo = row.Field<int>("ProdPrepNo"),
                    ProdRecNo = row.Field<int>("ProdRecNo"),
                    FinCode = row.Field<string>("FinCode"),
                    ItemDesc = row.Field<string>("ItemDesc"),
                    Batch = row.Field<string>("Batch"),
                    ProdYrNo = row.Field<string>("ProdYrNo"),
                    Prod_Qty = row.Field<decimal>("Prod_Qty"),
                    TotPreviousQty = row.Field<decimal>("TotPreviousQty"),
                    ConsQty = row.Field<decimal>("ConsQty"),
                    ToStoreNo = row.Field<int>("ToStoreNo"),
                    ToStoreDesc = row.Field<string>("ToStoreDesc"),
                    FromStoreNo = row.Field<int>("FromStoreNo"),
                    FromStoreDesc = row.Field<string>("FromStoreDesc"),
                    Notes = row.Field<string>("Notes"),
                    ItemSer = row.Field<int>("ItemSer"),
                    ExpDate = row.Field<DateTime>("ExpDate"),
                    ManDate = row.Field<DateTime>("ManDate"),
                    UnitSerial = Convert.ToByte(row.Field<int>("UnitSerial")),
                }).ToList();
            }
            else
            {
                Trans_DailyProd = Dt.AsEnumerable().Select(row => new TransferDailyProd
                {
                    TranByOrder = TransByOrder,
                    Transyear = year,
                    TransferFlag = row.Field<bool>("TransferFlag"),
                    ProdPrepYear = row.Field<short>("ProdPrepYear"),
                    ProdPrepNo = row.Field<int>("ProdPrepNo"),
                    FinCode = row.Field<string>("FinCode"),
                    ItemDesc = row.Field<string>("ItemDesc"),
                    TotReciptQty = row.Field<decimal>("TotReciptQty"),
                    TotpreviosConsQty = row.Field<decimal>("TotpreviosConsQty"),
                    ConsQty = row.Field<decimal>("ConsQty"),
                    FromStoreNo =Convert.ToInt32(row.Field<Int16>("FromStoreNo")),
                    FromStoreDesc = row.Field<string>("FromStoreDesc"),
                    UnitSerial =Convert.ToByte(row.Field<Int16>("UnitSerial")),
                }).ToList();
            }

            return PartialView(Trans_DailyProd);
        }

        public JsonResult TransferDailyProd(List<TransferDailyProd> lTransfDailyProd)
        {
            bool? TransByOrder = db.ProdCost_CompPara.Where(x => x.CompNo == company.comp_num).FirstOrDefault().TransByOrder;
            if(TransByOrder == null)
            {
                TransByOrder = false;
            }
            int? BufferStore = 0;
            bool? UseBufferStore = false;
            bool? IsSalesmanToStore = false;
            int MaxTrans;
            DataSet DsG = new DataSet();
            DsG.Tables.Add("TmpTblCons");
            DsG.Tables["TmpTblCons"].Columns.Add("ProdPrepYear");
            DsG.Tables["TmpTblCons"].Columns.Add("ProdPrepNo");
            DsG.Tables["TmpTblCons"].Columns.Add("FromStoreNo");
            DsG.Tables["TmpTblCons"].Columns.Add("FinCode");
            DsG.Tables["TmpTblCons"].Columns.Add("Prod_Qty");
            DsG.Tables["TmpTblCons"].Columns.Add("TotPreviousQty");
            DsG.Tables["TmpTblCons"].Columns.Add("ConsQty");
            DsG.Tables["TmpTblCons"].Columns.Add("UnitSerial");
            DsG.Tables["TmpTblCons"].Columns.Add("UnitCode");
            DsG.Tables["TmpTblCons"].Columns.Add("ConsVouYear");
            DsG.Tables["TmpTblCons"].Columns.Add("ConsVouNo");
            DsG.Tables["TmpTblCons"].Columns.Add("ToStoreNo");
            DsG.Tables["TmpTblCons"].Columns.Add("ToOrgStoreNo");
            DsG.Tables["TmpTblCons"].Columns.Add("UseBufferStore");
            DsG.Tables["TmpTblCons"].Columns.Add("Notes");
            DsG.Tables["TmpTblCons"].Columns.Add("Batch");
            object[] findThisItemH = new object[5];

            DsG.Tables["TmpTblCons"].PrimaryKey = new DataColumn[] { DsG.Tables["TmpTblCons"].Columns["ProdPrepYear"], DsG.Tables["TmpTblCons"].Columns["ProdPrepNo"], DsG.Tables["TmpTblCons"].Columns["FromStoreNo"], DsG.Tables["TmpTblCons"].Columns["FinCode"], DsG.Tables["TmpTblCons"].Columns["Batch"] };

           
            if (TransByOrder == false)
            {
                foreach (TransferDailyProd item in lTransfDailyProd)
                {
                    bool cPostProd = checkPostProd(item.ProdPrepYear, item.ProdPrepNo, item.ItemSer, 2);
                    if (cPostProd == true)
                    {
                        return Json(new { error = Resources.Resource.errorSelectAgain }, JsonRequestBehavior.AllowGet);
                    }

                    findThisItemH[0] = item.ProdPrepYear;
                    findThisItemH[1] = item.ProdPrepNo;
                    findThisItemH[2] = item.FromStoreNo;
                    findThisItemH[3] = item.FinCode;
                    findThisItemH[4] = item.Batch;

                    DataRow Drn = DsG.Tables["TmpTblCons"].Rows.Find(findThisItemH);
                    if (Drn == null)
                    {
                        DataRow Dr7 = DsG.Tables["TmpTblCons"].NewRow();
                        Dr7["ProdPrepYear"] = item.ProdPrepYear;
                        Dr7["ProdPrepNo"] = item.ProdPrepNo;
                        Dr7["FromStoreNo"] = item.FromStoreNo;
                        Dr7["FinCode"] = item.FinCode;
                        Dr7["Batch"] = item.Batch;
                        Dr7["UnitSerial"] = item.UnitSerial;
                        Dr7["UnitCode"] = GetItemUnitBySerial(item.FinCode, Convert.ToInt32(item.UnitSerial));
                        Dr7["ConsQty"] = item.ConsQty;
                        Dr7["ToOrgStoreNo"] = item.ToStoreNo;

                        BufferStore = 0;

                        DataRow DrStoreInfo = GetStoreInfo(item.FromStoreNo);
                        DataRow DrToStoreInfo;
                        DrToStoreInfo = GetStoreInfo(item.ToStoreNo);

                        if (DrStoreInfo != null)
                        {
                            UseBufferStore = (bool?)DrStoreInfo["UseBufferStore"];
                            BufferStore = (int?)DrStoreInfo["BufferStore"];
                            if (UseBufferStore.Value == null)
                            {
                                UseBufferStore = false;
                            }
                            if (BufferStore.Value == null)
                            {
                                BufferStore = 0;
                            }
                        }
                        if (DrToStoreInfo != null)
                        {
                            IsSalesmanToStore = (bool?)DrStoreInfo["Employee"];
                            if (IsSalesmanToStore.Value == null)
                            {
                                IsSalesmanToStore = false;
                            }
                        }

                        Dr7["UseBufferStore"] = UseBufferStore;
                        if (UseBufferStore == true)
                        {
                            Dr7["ToStoreNo"] = BufferStore;

                        }
                        else
                        {
                            Dr7["ToStoreNo"] = item.ToStoreNo;
                        }
                        if (item.Notes == null)
                        {
                            item.Notes = "";
                        }
                        Dr7["Notes"] = item.Notes;
                        DsG.Tables["TmpTblCons"].Rows.Add(Dr7);
                    }
                    else
                    {
                        Drn["ConsQty"] = Convert.ToDecimal(Drn["ConsQty"]) + item.ConsQty;
                    }
                }
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select invdailydf.*,cast(0 as bigint) as logid from invdailydf where compno=0", cn);
                DaPR.Fill(TmpDs, "TmpTbl");
                TmpDs.Tables["TmpTbl"].PrimaryKey = new DataColumn[] { TmpDs.Tables["TmpTbl"].Columns["VouType"], TmpDs.Tables["TmpTbl"].Columns["ItemNo"], TmpDs.Tables["TmpTbl"].Columns["Batch"], TmpDs.Tables["TmpTbl"].Columns["TUnit"] };


                transaction = cn.BeginTransaction();

                DataTable DtGroup1 = new DataTable();
                DataRow DrGroup1;
                DataView dv;
                DataTable TmpDtTestQ = new DataTable();

                DtGroup1.Columns.Add("ProdPrepYr", typeof(int));
                DtGroup1.Columns.Add("ProdPrepNo", typeof(int));
                DtGroup1.Columns.Add("ConsVouYear", typeof(int));
                DtGroup1.Columns.Add("ConsVouNo", typeof(int));

                DtGroup1.PrimaryKey = new DataColumn[] { DtGroup1.Columns["ProdPrepYr"], DtGroup1.Columns["ProdPrepNo"] };

                foreach (DataRow DR in DsG.Tables["TmpTblCons"].Rows)
                {
                    object[] FindYear = new object[2];
                    FindYear[0] = DR["ProdPrepYear"];
                    FindYear[1] = DR["ProdPrepNo"];
                    DrGroup1 = DtGroup1.Rows.Find(FindYear);
                    if (DrGroup1 == null)
                    {
                        DrGroup1 = DtGroup1.NewRow();
                        DrGroup1.BeginEdit();
                        DrGroup1["ProdPrepYr"] = DR["ProdPrepYear"];
                        DrGroup1["ProdPrepNo"] = DR["ProdPrepNo"];
                        DrGroup1["ConsVouYear"] = DR["ConsVouYear"];
                        DrGroup1["ConsVouNo"] = DR["ConsVouNo"];
                        DrGroup1.EndEdit();
                        DtGroup1.Rows.Add(DrGroup1);
                    }
                }
                foreach (DataRow Dr in DtGroup1.Rows)
                {
                    dv = DsG.Tables["TmpTblCons"].DefaultView;
                    dv.RowFilter = "ProdPrepYear = " + Dr["ProdPrepYr"] + " AND ProdPrepNo = " + Dr["ProdPrepNo"];
                    TmpDtTestQ = dv.ToTable();

                    bool WritConsVoc = WriteConsVoucher(Dr, TmpDtTestQ, lTransfDailyProd, transaction, cn);
                    if (WritConsVoc == false)
                    {
                        return Json(new { error = error }, JsonRequestBehavior.AllowGet);
                    }
                    if (TransByOrder == false)
                    {
                        foreach (TransferDailyProd item in lTransfDailyProd)
                        {
                            if (item.PostFlag == true)
                            {
                                if (item.ProdPrepYear == Convert.ToInt16(Dr["ProdPrepYr"]) && (item.ProdPrepNo == Convert.ToInt32(Dr["ProdPrepNo"])))
                                {
                                    item.ConsVouYear = Convert.ToInt16(Dr["ConsVouYear"]);
                                    item.ConsVouNo = Convert.ToInt32(Dr["ConsVouNo"]);
                                }
                            }
                        }
                    }
                    if (TransByOrder == false)
                    {
                        double QtyCons1;
                        MaxTrans = GetMaxTrans(2);
                        foreach (TransferDailyProd item in lTransfDailyProd)
                        {
                            decimal ConsQty = item.ConsQty;
                            decimal TotPreviousQty = item.TotPreviousQty;
                            decimal Prod_Qty = item.Prod_Qty;
                            decimal Total = ConsQty + TotPreviousQty;
                            using (SqlCommand Cmd2 = new SqlCommand())
                            {
                                Cmd2.Connection = cn;
                                Cmd2.CommandType = CommandType.StoredProcedure;
                                Cmd2.CommandText = "ProdCost_TransferDailyProduction";
                                Cmd2.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                                Cmd2.Parameters.Add("@ProdPrepYr", SqlDbType.SmallInt).Value = item.ProdPrepYear;
                                Cmd2.Parameters.Add("@ProdPrepNo", SqlDbType.Int).Value = item.ProdPrepNo;
                                Cmd2.Parameters.Add("@ItemNo", SqlDbType.VarChar, 20).Value = item.FinCode;
                                Cmd2.Parameters.Add("@ItemSerl", SqlDbType.Int).Value = item.ItemSer;
                                Cmd2.Parameters.Add("@ProdRecYear", SqlDbType.SmallInt).Value = item.ProdRecYear;
                                Cmd2.Parameters.Add("@ProdRecNo", SqlDbType.Int).Value = item.ProdRecNo;
                                Cmd2.Parameters.Add("@ConsVouYear", SqlDbType.SmallInt).Value = item.ConsVouYear;
                                Cmd2.Parameters.Add("@ConsVouNo", SqlDbType.Int).Value = item.ConsVouNo;
                                Cmd2.Parameters.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                                Cmd2.Parameters.Add("@TransNoOut", SqlDbType.Int).Value = MaxTrans;
                                if (Total == Prod_Qty)
                                {
                                    Cmd2.Parameters.Add("@TransferFlag", SqlDbType.Bit).Value = 1;
                                }
                                else
                                {
                                    Cmd2.Parameters.Add("@TransferFlag", SqlDbType.Bit).Value = 0;
                                }
                                Cmd2.Parameters.Add("@ConsQty", SqlDbType.Money).Value = item.ConsQty;

                                Cmd2.Transaction = transaction;
                                try
                                {
                                    Cmd2.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                                QtyCons1 = Convert.ToDouble(item.ConsQty);
                            }
                        }
                    }
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool checkPostProd(int ReportYear,int ReportNo,int ItemSer,int TransType)
        {
            bool? checkPostProd = false;
            bool PostProd = false;
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (TransType == 1)
                    {
                        cmd.CommandText = "SELECT PostFlag FROM  ProdCost_DailyProductionD WHERE (CompNo = @CompNo) AND (ReportYear = @ReportYear) AND (ReportNo = @ReportNo) AND (ItemSer = @ItemSer)";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT TransferFlag FROM ProdCost_DailyProductionD WHERE (CompNo = @CompNo) AND (ReportYear = @ReportYear) AND (ReportNo = @ReportNo) AND (ItemSer = @ItemSer)";
                    }
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ReportYear", System.Data.SqlDbType.SmallInt)).Value = ReportYear;
                    cmd.Parameters.Add(new SqlParameter("@ReportNo", System.Data.SqlDbType.Int)).Value = ReportNo;
                    cmd.Parameters.Add(new SqlParameter("@ItemSer", System.Data.SqlDbType.Int)).Value = ItemSer;
                    checkPostProd = (bool?)cmd.ExecuteScalar();
                }
            }
            if(checkPostProd == null)
            {
                PostProd = false;
            }
            else
            {
                PostProd = checkPostProd.Value;
            }
            return PostProd;
        }
        public string GetItemUnitBySerial(string ItemNo,int UnitSerial)
        {
            string UnitCode = "";
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "SELECT CASE WHEN @UnitSerial = 1 THEN UnitC1 ELSE CASE WHEN @UnitSerial = 2 THEN UnitC2 ELSE CASE WHEN @UnitSerial = 3 THEN UnitC3 ELSE UnitC4 END END END AS UnitCode FROM InvItemsMF WHERE (CompNo = @CompNo) AND (ItemNo = @ItemNo)";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", System.Data.SqlDbType.SmallInt)).Value = ItemNo;
                    cmd.Parameters.Add(new SqlParameter("@UnitSerial", System.Data.SqlDbType.Int)).Value = UnitSerial;
                    UnitCode = (string)cmd.ExecuteScalar();
                }
            }
            return UnitCode;
        }
        public DataRow GetStoreInfo(int StoreNo)
        {
            DataRow Dr;
            using (DataTable Dt = new DataTable())
            {
                using (var cn1 = new SqlConnection(ConnectionString()))
                {
                    cn1.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn1;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Invt_GetStoresMF";
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                        cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.SmallInt, 2)).Value = StoreNo;
                        using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                        {
                            DA.Fill(Dt);
                        }
                    }
                }
                Dr = Dt.Rows[0];
            }
            return Dr;
        }
        public bool WriteConsVoucher(DataRow Dr1, DataTable Dt, List<TransferDailyProd> lTransfDailyProd, SqlTransaction tran, SqlConnection cn)
        {
            bool? TransByOrder = db.ProdCost_CompPara.Where(x => x.CompNo == company.comp_num).FirstOrDefault().TransByOrder;
            bool ConsVoc = false;
            SqlCommand Cmd12 = new SqlCommand();
            SqlCommand Cmd5 = new SqlCommand();
            DataRow dr = Dt.Rows[0];
            DataRow DrStoreInfo = GetStoreInfo(Convert.ToInt32(dr["FromStoreNo"]));
            DataRow DrToStoreInfo;
            DrToStoreInfo = GetStoreInfo(Convert.ToInt32(dr["ToStoreNo"]));
            bool? UseBufferStore = false;
            int? BufferStore = 0;
            int i = 1;
            if (DrToStoreInfo != null)
            {
                UseBufferStore = (bool?)DrToStoreInfo["UseBufferStore"];
                if (UseBufferStore.Value == null)
                {
                    UseBufferStore = false;
                }
                UseBufferStore = Convert.ToBoolean(dr["UseBufferStore"]);

                BufferStore = Convert.ToInt32(dr["ToOrgStoreNo"]);
                if (UseBufferStore.Value == null)
                {
                    UseBufferStore = false;
                }
                if (BufferStore.Value == null)
                {
                    BufferStore = 0;
                }
            }
            bool chkVochBatch;
            chkVochBatch = ChkVouBatchs(Convert.ToInt64(BufferStore), lTransfDailyProd, tran, cn);
            if (chkVochBatch == false)
            {
                return ConsVoc = false;
            }
            chkVochBatch = ChkVouBatchs(Convert.ToInt64(dr["FromStoreNo"]), lTransfDailyProd, tran, cn);
            if (chkVochBatch == false)
            {
                return ConsVoc = false;
            }
            chkVochBatch = ChkVouBatchs(Convert.ToInt64(dr["ToStoreNo"]), lTransfDailyProd, tran, cn);
            if (chkVochBatch == false)
            {
                return ConsVoc = false;
            }

            using (Cmd12 = new SqlCommand())
            {
                Cmd12.Connection = cn;
                Cmd12.CommandType = CommandType.StoredProcedure;
                Cmd12.CommandText = "InvT_AddInvdailyHF";
                Cmd12.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd12.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = lTransfDailyProd.FirstOrDefault().TransDate.Year;
                Cmd12.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 12;
                Cmd12.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = lTransfDailyProd.FirstOrDefault().TransDate;
                Cmd12.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = dr["FromStoreNo"];
                Cmd12.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = dr["ToStoreNo"];
                Cmd12.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                Cmd12.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = 1;
                Cmd12.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                Cmd12.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@NetFAmount", SqlDbType.Float)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CashAcc", SqlDbType.BigInt)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = Dr1["ProdPrepYr"];
                Cmd12.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = Dr1["ProdPrepNo"];
                Cmd12.Parameters.Add(new SqlParameter("@ProdBatchNo", SqlDbType.Int)).Value = 1;
                Cmd12.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                if(lTransfDailyProd.FirstOrDefault().Note == null)
                {
                    lTransfDailyProd.FirstOrDefault().Note = "";
                }
                Cmd12.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 300)).Value = lTransfDailyProd.FirstOrDefault().Note;
                Cmd12.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar, 10)).Value = lTransfDailyProd.FirstOrDefault().TransDate;
                Cmd12.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar, 25)).Value = 0;
                Cmd12.Parameters.Add(new SqlParameter("@ComputerName", SqlDbType.VarChar, 50)).Value = 0;
                if (UseBufferStore == true)
                {
                    Cmd12.Parameters.Add(new SqlParameter("@TargetStoreNo", SqlDbType.Int)).Value = BufferStore;
                }
                else
                {
                    Cmd12.Parameters.Add(new SqlParameter("@TargetStoreNo", SqlDbType.Int)).Value = 0;
                }
                Cmd12.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                Cmd12.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                Cmd12.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                Cmd12.Transaction = tran;
                try
                {
                    Cmd12.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return ConsVoc = false;
                }

                short ErrNo = Convert.ToInt16(Cmd12.Parameters["@ErrNo"].Value);
                int UsedVouNo = Convert.ToInt32(Cmd12.Parameters["@UsedVouNo"].Value);
                long LogID = Convert.ToInt64(Cmd12.Parameters["@LogID"].Value);

                if (Convert.ToInt16(Cmd12.Parameters["@ErrNo"].Value) != 0)
                {
                    ConsVoc = false;
                }

                if (Convert.ToInt32(Cmd12.Parameters["@UsedVouNo"].Value) != 0)
                {
                    gConsVouNo = Convert.ToInt32(Cmd12.Parameters["@UsedVouNo"].Value);
                }

                using (Cmd5 = new SqlCommand())
                {
                    Cmd5.Connection = cn;
                    Cmd5.CommandType = CommandType.StoredProcedure;
                    Cmd5.CommandText = "InvT_AddInvdailyHF";
                    Cmd5.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    Cmd5.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = lTransfDailyProd.FirstOrDefault().TransDate.Year;
                    Cmd5.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = gConsVouNo;
                    Cmd5.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 5;
                    Cmd5.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = lTransfDailyProd.FirstOrDefault().TransDate;
                    Cmd5.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = dr["ToStoreNo"];
                    Cmd5.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = dr["FromStoreNo"];
                    Cmd5.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                    Cmd5.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = 1;
                    Cmd5.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    Cmd5.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@NetFAmount", SqlDbType.Float)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CashAcc", SqlDbType.BigInt)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = Dr1["ProdPrepYr"];
                    Cmd5.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = Dr1["ProdPrepNo"];
                    Cmd5.Parameters.Add(new SqlParameter("@ProdBatchNo", SqlDbType.Int)).Value = 1;
                    Cmd5.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    if (lTransfDailyProd.FirstOrDefault().Note == null)
                    {
                        lTransfDailyProd.FirstOrDefault().Note = "";
                    }
                    Cmd5.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 300)).Value = lTransfDailyProd.FirstOrDefault().Note;
                    Cmd5.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar, 10)).Value = lTransfDailyProd.FirstOrDefault().TransDate;
                    Cmd5.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar, 25)).Value = 0;
                    Cmd5.Parameters.Add(new SqlParameter("@ComputerName", SqlDbType.VarChar, 50)).Value = 0;
                    if (UseBufferStore == true)
                    {
                        Cmd5.Parameters.Add(new SqlParameter("@TargetStoreNo", SqlDbType.Int)).Value = BufferStore;
                    }
                    else
                    {
                        Cmd5.Parameters.Add(new SqlParameter("@TargetStoreNo", SqlDbType.Int)).Value = 0;
                    }
                    Cmd5.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    Cmd5.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    Cmd5.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    Cmd5.Transaction = tran;
                    try
                    {
                        Cmd5.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return ConsVoc = false;
                    }
                    long LogID5 = Convert.ToInt64(Cmd5.Parameters["@LogID"].Value);

                    foreach (DataRow dr12 in Dt.Rows)
                    {
                        DrTmp = TmpDs.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["compno"] = company.comp_num;
                        DrTmp["VouYear"] = lTransfDailyProd.FirstOrDefault().TransDate.Year;
                        DrTmp["VouType"] = 12;
                        DrTmp["VouNo"] = gConsVouNo;
                        DrTmp["StoreNo"] = dr12["FromStoreNo"];
                        DrTmp["ItemNo"] = dr12["FinCode"];
                        DrTmp["Batch"] = dr12["Batch"];
                        DrTmp["ItemSer"] = i;
                        DrTmp["VouDate"] = lTransfDailyProd.FirstOrDefault().TransDate;
                        DrTmp["Qty"] = Convert.ToDecimal(dr12["ConsQty"]) * -1;
                        DrTmp["ItemDimension"] = "0*0*0";
                        DrTmp["Qty2"] = 0;
                        DrTmp["Bonus"] = 0;
                        DrTmp["TUnit"] = dr12["UnitCode"];
                        DrTmp["NetSellValue"] = 0;
                        DrTmp["Discount"] = 0;
                        DrTmp["PerDiscount"] = 0;
                        DrTmp["VouDiscount"] = 0;
                        DrTmp["CurrNo"] = 0;
                        DrTmp["ForUCost"] = 0;
                        DrTmp["ToStoreNo"] = dr12["ToStoreNo"];
                        DrTmp["ToBatch"] = dr12["batch"];
                        DrTmp["AccDepNo"] = 0;
                        DrTmp["AccNo"] = 0;
                        DrTmp["Item_Tax_Per"] = 0;
                        DrTmp["Item_Tax_Val"] = 0;
                        DrTmp["Item_STax_Per"] = 0;
                        DrTmp["Item_STax_Val"] = 0;
                        DrTmp["Item_Tax_Type"] = 0;
                        DrTmp["Item_STax_Type"] = 0;
                        DrTmp["Item_PTax_Per"] = 0;
                        DrTmp["Item_PTax_Val"] = 0;
                        DrTmp["Item_PTax_Type"] = 0;
                        DrTmp["ProdOrderNo"] = 0;
                        DrTmp["UnitSerial"] = dr12["UnitSerial"];
                        DrTmp["ExRate"] = 1;
                        DrTmp["logid"] = LogID;
                        DrTmp.EndEdit();
                        TmpDs.Tables["TmpTbl"].Rows.Add(DrTmp);
                        DrTmp = TmpDs.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["compno"] = company.comp_num;
                        DrTmp["VouYear"] = lTransfDailyProd.FirstOrDefault().TransDate.Year;
                        DrTmp["VouType"] = 5;
                        DrTmp["VouNo"] = gConsVouNo;
                        DrTmp["StoreNo"] = dr12["ToStoreNo"];
                        DrTmp["ItemNo"] = dr12["FinCode"];
                        DrTmp["Batch"] = dr12["Batch"];
                        DrTmp["ItemSer"] = i;
                        DrTmp["VouDate"] = lTransfDailyProd.FirstOrDefault().TransDate;
                        DrTmp["Qty"] = Convert.ToDecimal(dr12["ConsQty"]);
                        DrTmp["ItemDimension"] = "0*0*0";
                        DrTmp["Qty2"] = 0;
                        DrTmp["Bonus"] = 0;
                        DrTmp["TUnit"] = dr12["UnitCode"];
                        DrTmp["NetSellValue"] = 0;
                        DrTmp["Discount"] = 0;
                        DrTmp["PerDiscount"] = 0;
                        DrTmp["VouDiscount"] = 0;
                        DrTmp["CurrNo"] = 0;
                        DrTmp["ForUCost"] = 0;
                        DrTmp["ToStoreNo"] = dr12["FromStoreNo"];
                        DrTmp["ToBatch"] = dr12["batch"];
                        DrTmp["AccDepNo"] = 0;
                        DrTmp["AccNo"] = 0;
                        DrTmp["Item_Tax_Per"] = 0;
                        DrTmp["Item_Tax_Val"] = 0;
                        DrTmp["Item_STax_Per"] = 0;
                        DrTmp["Item_STax_Val"] = 0;
                        DrTmp["Item_Tax_Type"] = 0;
                        DrTmp["Item_STax_Type"] = 0;
                        DrTmp["Item_PTax_Per"] = 0;
                        DrTmp["Item_PTax_Val"] = 0;
                        DrTmp["Item_PTax_Type"] = 0;
                        DrTmp["ProdOrderNo"] = 0;
                        DrTmp["UnitSerial"] = dr12["UnitSerial"];
                        DrTmp["ExRate"] = 1;
                        DrTmp["logid"] = LogID5;
                        DrTmp.EndEdit();
                        TmpDs.Tables["TmpTbl"].Rows.Add(DrTmp);
                        i = i + 1;
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "InvT_AddInvDailyDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt, 4, "VouYear"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt, 4, "VouType"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int, 9, "VouNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 4, "StoreNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "Batch"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 4, "ItemSer"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime, 10, "VouDate"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemDimension", SqlDbType.VarChar, 20, "ItemDimension"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                        CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
                        CmdIns.Parameters.Add(new SqlParameter("@TUnit", SqlDbType.VarChar, 5, "TUnit"));
                        CmdIns.Parameters.Add(new SqlParameter("@NetSellValue", SqlDbType.Float, 9, "NetSellValue"));
                        CmdIns.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Float, 9, "Discount"));
                        CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Float, 9, "VouDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt, 4, "CurrNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ForUCost", SqlDbType.Float, 9, "ForUCost"));
                        CmdIns.Parameters.Add(new SqlParameter("@ToStoreNo", SqlDbType.Int, 4, "ToStoreNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ToBatch", SqlDbType.VarChar, 20, "ToBatch"));
                        CmdIns.Parameters.Add(new SqlParameter("@AccDepNo", SqlDbType.Int, 4, "AccDepNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt, 8, "AccNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_Tax_Per", SqlDbType.Float, 9, "Item_Tax_Per"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_Tax_Val", SqlDbType.Float, 9, "Item_Tax_Val"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_STax_Per", SqlDbType.Float, 9, "Item_STax_Per"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_STax_Val", SqlDbType.Float, 9, "Item_STax_Val"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_Tax_Type", SqlDbType.Bit, 1, "Item_Tax_Type"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_STax_Type", SqlDbType.Bit, 1, "Item_STax_Type"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_PTax_Per", SqlDbType.Float, 9, "Item_PTax_Per"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_PTax_Val", SqlDbType.Float, 9, "Item_PTax_Val"));
                        CmdIns.Parameters.Add(new SqlParameter("@Item_PTax_Type", SqlDbType.Bit, 1, "Item_PTax_Type"));
                        CmdIns.Parameters.Add(new SqlParameter("@ProdOrderNo", SqlDbType.Int, 8, "ProdOrderNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ExRate", SqlDbType.Float, 9, "ExRate"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitCodeSr", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt, 2)).Value = 1;
                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt, 8, "LogID"));
                        CmdIns.Transaction = tran;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "TmpTbl");

                        bool TransferConf = AddDelTransferConfirmation(lTransfDailyProd.FirstOrDefault().TransDate.Year, Convert.ToInt32(gConsVouNo), Convert.ToBoolean(UseBufferStore), tran, cn);
                        bool ChMinusQty = CheckMinusQty(Convert.ToInt32(dr["FromStoreNo"]), tran, cn);
                        if (ChMinusQty == false)
                        {
                            error = "لا يمكن تخزين بسب وجود مواد بالسالب في هذا المستودع";
                            return ConsVoc = false;
                        }
                        if (TransByOrder == false || TransByOrder == null)
                        {
                            Dr1.BeginEdit();
                            Dr1["ConsVouYear"] = DateTime.Now.Year;
                            Dr1["ConsVouNo"] = gConsVouNo;
                            Dr1.EndEdit();
                        }
                        ConsVoc = true;
                    }
                }
            }
            
            return ConsVoc;
        }
        public bool ChkVouBatchs(long ToStoreNo, List<TransferDailyProd> lTransfDailyProd, SqlTransaction tran, SqlConnection cn)
        {
            bool ChkBatchE = false;
            SqlCommand CmdIns = new SqlCommand();
            foreach (TransferDailyProd item in lTransfDailyProd)
            {
                fincode = "";
                ChkBatchE = false;
                if (ChkStore(Convert.ToInt32(ToStoreNo), item.FinCode, tran, cn))
                {
                    ChkBatchE = Chkbatch(Convert.ToInt32(ToStoreNo), item.FinCode, item.Batch, tran, cn);
                    if (ChkBatchE == false)
                    {
                        using (CmdIns = new SqlCommand())
                        {
                            CmdIns.Connection = cn;
                            CmdIns.CommandType = CommandType.StoredProcedure;
                            CmdIns.CommandText = "Invt_AddBatchs";
                            CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = company.comp_num;
                            CmdIns.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 4)).Value = ToStoreNo;
                            CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = item.FinCode;
                            CmdIns.Parameters.Add(new SqlParameter("@batchNo", SqlDbType.VarChar, 16)).Value = item.Batch;
                            CmdIns.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.SmallDateTime, 10)).Value = item.ExpDate;
                            CmdIns.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 10)).Value = item.ManDate;
                            CmdIns.Parameters.Add(new SqlParameter("@IsHalt", SqlDbType.Bit, 1)).Value = 0;
                            CmdIns.Parameters.Add(new SqlParameter("@UnitCost", SqlDbType.Float, 8)).Value = 0;
                            CmdIns.Parameters.Add(new SqlParameter("@HBatch", SqlDbType.Bit, 1)).Value = 1;
                            CmdIns.Transaction = tran;
                            CmdIns.ExecuteNonQuery();
                            ChkBatchE = true;
                        }
                    }
                }
                else
                {
                    fincode = item.FinCode;
                }
            }
            return ChkBatchE;
        }
        public bool ChkStore(int StoreNo,string ItemNo, SqlTransaction tran, SqlConnection cn)
        {
            bool ChkSto = false;
            SqlCommand Cmd = new SqlCommand();
            using (Cmd = new SqlCommand())
            {
                Cmd.Connection = cn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "InvT_ChkExistsStore_New";
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.SmallInt, 2)).Value = StoreNo;
                Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = ItemNo;
                Cmd.Parameters.Add(new SqlParameter("@IsLinked", SqlDbType.Bit)).Direction = ParameterDirection.Output;
                Cmd.Transaction = tran;
                Cmd.ExecuteNonQuery();
            }
            ChkSto = (bool)Cmd.Parameters["@IsLinked"].Value;
            return ChkSto;
        }
        public bool Chkbatch(int StoreNo,string ItemNo,string BatchNo, SqlTransaction tran, SqlConnection cn)
        {
            bool Chkbatch = false;
            SqlCommand Cmd = new SqlCommand();
            using (Cmd = new SqlCommand())
            {
                Cmd.Connection = cn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "InvT_ChkExistsBatch_New";
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 8)).Value = StoreNo;
                Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = ItemNo;
                Cmd.Parameters.Add(new SqlParameter("@batch", SqlDbType.VarChar, 20)).Value = BatchNo;
                Cmd.Parameters.Add(new SqlParameter("@IsFound", SqlDbType.Bit)).Direction = ParameterDirection.Output;
                Cmd.Transaction = tran;
                Cmd.ExecuteNonQuery();
            }
            Chkbatch = (bool)Cmd.Parameters["@IsFound"].Value;
            return Chkbatch;

        }
        public bool AddDelTransferConfirmation(int VouYear,int VouNo,bool UseBufferStore, SqlTransaction tran, SqlConnection cn)
        {
            using (SqlCommand CmdIns = new SqlCommand())
            {
                CmdIns.Connection = cn;
                CmdIns.CommandText = "InvT_AddDelTransferConfirmation";
                CmdIns.CommandType = CommandType.StoredProcedure;
                CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                CmdIns.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = VouYear;
                CmdIns.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouNo;
                CmdIns.Parameters.Add(new SqlParameter("@UseBufferStore", SqlDbType.SmallInt)).Value = UseBufferStore;
                CmdIns.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                CmdIns.Transaction = tran;
                CmdIns.ExecuteNonQuery();
            }
            return true;
        }
        public bool CheckMinusQty(int StoreNo, SqlTransaction tran, SqlConnection cn)
        {
            int ChkRCount = 0;
            bool? AllowMinus = false;
            using (SqlCommand CmdIns = new SqlCommand())
            {
                CmdIns.Connection = cn;
                CmdIns.CommandText = "select AllowMinuseQty from InvStoresMF where compno=" + company.comp_num + " and StoreNo=" + StoreNo ;
                CmdIns.CommandType = CommandType.Text;
                CmdIns.Transaction = tran;
                AllowMinus = (bool?)CmdIns.ExecuteScalar();
                if(AllowMinus.Value == null)
                {
                    AllowMinus = false;
                }
            }

            if (AllowMinus == false)
            {
                using (SqlCommand CmdIns = new SqlCommand())
                {
                    CmdIns.Connection = cn;
                    CmdIns.CommandText = "select InvBatchsMF.* from InvBatchsMF where compno=" + company.comp_num + " and StoreNo=" + StoreNo +" and qtyoh<0";
                    CmdIns.CommandType = CommandType.Text;
                    CmdIns.Transaction = tran;
                    SqlDataReader CmdDr;
                    CmdDr = CmdIns.ExecuteReader();
                    while (CmdDr.Read())
                    {
                       ChkRCount = ChkRCount + 1;
                    }
                    CmdDr.Close();
                }
            }

            if (ChkRCount != 0)
            {
                AllowMinus = false;
            }
            else
            {
                AllowMinus = true;
            }
            return AllowMinus.Value;
        }
        public int GetMaxTrans(int TransType)
        {
            int? MaxTrans = 0;
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (TransType == 1)
                    {
                        cmd.CommandText = "SELECT  isnull(MAX(TransNoIn),0)+1 FROM ProdCost_DailyProductionD WHERE (CompNo = @CompNo)";
                    }
                    else
                    {
                        cmd.CommandText = "SELECT  isnull(MAX(TransNoOut),0)+1 FROM ProdCost_DailyProductionD WHERE (CompNo = @CompNo)";
                    }
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    MaxTrans = (int?)cmd.ExecuteScalar();
                    if(MaxTrans == null)
                    {
                        MaxTrans = 0;
                    }
                }
            }
            return MaxTrans.Value;
        }
    }
}