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
    public class DailyProductionController : controller
    {
        // GET: DailyProduction
        public ActionResult Index()
        {
            List<ProdCost_DailyProductionH> DailyProdHf = db.ProdCost_DailyProductionH.Where(x => x.CompNo == company.comp_num && x.ReportYear == DateTime.Now.Year).OrderByDescending(x => x.ReportYear).ToList();
            return View(DailyProdHf);
        }
        public ActionResult DailyProdList(int Year)
        {
            List<ProdCost_DailyProductionH> DailyProdHf = db.ProdCost_DailyProductionH.Where(x => x.CompNo == company.comp_num && x.ReportYear == Year).OrderByDescending(x => x.ReportYear).ToList();
            return PartialView(DailyProdHf);
        }
        public ActionResult ProdOrder(int year)
        {
            string Condition = " where (prod_prepare_info.comp_no = '" + company.comp_num + "') AND (prod_prepare_info.prepare_year = '" + year + "') AND (prod_prepare_info.SusFlag = 0) ";
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Search";
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Condition", SqlDbType.VarChar, 2000)).Value = Condition;
                    cmd.Parameters.Add(new SqlParameter("@ProgID", SqlDbType.SmallInt)).Value = 201;

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<ProdOrders> Prod_Orders = Dt.AsEnumerable().Select(row => new ProdOrders
            {
                prepare_year = row.Field<short>("prepare_year"),
                prepare_code = row.Field<int>("prepare_code"),
                prepare_date = row.Field<DateTime>("prepare_date"),
                item_no = row.Field<string>("item_no"),
                ItemDesc = row.Field<string>("ItemDesc"),
                ItemDesc_Ara = row.Field<string>("ItemDesc_Ara"),
                qty_prepare = Convert.ToDecimal(row.Field<float>("qty_prepare")),
                StoreNo = row.Field<int?>("StoreNo"),
                StoreName = row.Field<string>("StoreName"),
                stage_code = row.Field<string>("stage_code"),
                UnitSerial = row.Field<byte?>("UnitSerial"),
                TUnit = row.Field<string>("TUnit"),
            }).ToList();

            return PartialView(Prod_Orders);
        }
        public ActionResult eDailyProd(short CompNo, int ReportYear, int ReportNo)
        {
            ProdCost_DailyProductionH DailyProdH = db.ProdCost_DailyProductionH.Where(x => x.CompNo == CompNo
                                   && x.ReportYear == ReportYear && x.ReportNo == ReportNo).FirstOrDefault();
            return PartialView(DailyProdH);
        }
        public ActionResult ItemProdOrder(ProdOrders ItemOrder)
        {
            IList<ProdOrders> Prod_Orders;
            int i = 2;
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderPacking";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = ItemOrder.prepare_year;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 2)).Value = ItemOrder.prepare_code;
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
            Prod_Orders = Dt.AsEnumerable().Select(row => new ProdOrders
            {
                item_no = row.Field<string>("FinProd"),
                ItemDesc = row.Field<string>("ItemDesc"),
                Qty = row.Field<double>("Qty"),
                UnitSerial = 4,
                TUnit = row.Field<string>("TUnit"),
                serial = i++
            }).ToList();

            
            ProdOrders prod = new ProdOrders();
            prod.item_no = ItemOrder.item_no;
            prod.ItemDesc = ItemOrder.ItemDesc;
            if(Prod_Orders.Count != 0)
            {
                prod.Qty = 0;
            }
            else
            {
                prod.Qty = ItemOrder.Qty;
            }
            prod.UnitSerial = ItemOrder.UnitSerial;
            prod.TUnit = ItemOrder.TUnit;
            prod.serial = 1;
            Prod_Orders.Add(prod);

            return PartialView(Prod_Orders.OrderBy(o => o.serial));
        }
        public JsonResult Save_DailyProduction(ProdCost_DailyProductionH DailyProductionH,List<ProdCost_DailyProductionD> DailyProductionDF)
        {
            DataSet TmpDs = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT  * FROM dbo.ProdCost_DailyProductionD WHERE CompNo = 0", cn);
                DaPR.Fill(TmpDs, "DailyProd");
               

                using (SqlCommand CmdH = new SqlCommand())
                {
                    CmdH.Connection = cn;
                    CmdH.CommandText = "ProdCost_AddDailyProductionH";
                    CmdH.CommandType = CommandType.StoredProcedure;
                    CmdH.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = DailyProductionH.CompNo;
                    CmdH.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = DailyProductionH.ReportYear;
                    CmdH.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int)).Value = DailyProductionH.ReportNo;
                    CmdH.Parameters.Add(new SqlParameter("@Prod_Date", SqlDbType.SmallDateTime)).Value = DailyProductionH.Prod_Date;
                    CmdH.Parameters.Add(new SqlParameter("@ShiftNo", SqlDbType.Int)).Value = DailyProductionH.ShiftNo;
                    CmdH.Parameters.Add(new SqlParameter("@Prod_stage", SqlDbType.Int)).Value = DailyProductionH.Prod_stage;
                    CmdH.Parameters.Add(new SqlParameter("@MachineNo", SqlDbType.Int)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@UsedSrNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    transaction = cn.BeginTransaction();
                    CmdH.Transaction = transaction;
                    try
                    {
                        CmdH.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    int ReportNo = Convert.ToInt32(CmdH.Parameters["@UsedSrNo"].Value);
                    if (ReportNo == -10)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = Resources.Resource.errorDailyProduction }, JsonRequestBehavior.AllowGet);
                    }
                    int i = 1;
                    foreach (ProdCost_DailyProductionD item in DailyProductionDF)
                    {
                        DrTmp = TmpDs.Tables["DailyProd"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["ReportYear"] = item.ReportYear;
                        DrTmp["ReportNo"] = ReportNo;
                        DrTmp["ItemSer"] = i;
                        DrTmp["ProdPrepYear"] = item.ProdPrepYear;
                        DrTmp["ProdPrepNo"] = item.ProdPrepNo;
                        DrTmp["FinCode"] = item.FinCode;
                        DrTmp["Prod_Qty"] = item.Prod_Qty;
                        DrTmp["Tunit"] = item.TUnit;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["UserID"] = me.UserID;
                        DrTmp.EndEdit();
                        TmpDs.Tables["DailyProd"].Rows.Add(DrTmp);
                        i = i + 1;
                    }
                    using (SqlCommand CmdDIns = new SqlCommand())
                    {
                        CmdDIns.Connection = cn;
                        CmdDIns.CommandText = "ProdCost_AddDailyProductionD";
                        CmdDIns.CommandType = CommandType.StoredProcedure;
                        CmdDIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = DailyProductionH.CompNo;
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportYear", SqlDbType.SmallInt, 4)).Value = DailyProductionH.ReportYear;
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int, 8)).Value = ReportNo;
                        CmdDIns.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.Int, 8, "ItemSer"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt, 4, "ProdPrepYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepNo", SqlDbType.Int, 8, "ProdPrepNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@FinCode", SqlDbType.VarChar, 20, "FinCode"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Prod_Qty", SqlDbType.Money, 8, "Prod_Qty"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8, "UserID"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 16, "Batch")).Value = "1";
                        CmdDIns.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 10, "ManDate")).Value = DailyProductionH.Prod_Date;
                        CmdDIns.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.SmallDateTime, 10, "ExpDate")).Value = DailyProductionH.Prod_Date;
                        CmdDIns.Parameters.Add(new SqlParameter("@ConsQty", SqlDbType.Money, 8, "ConsQty"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Tunit", SqlDbType.VarChar, 5, "Tunit"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransType", SqlDbType.SmallInt, 4)).Value = 1;
                        CmdDIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdDIns;
                        DaPR.Update(TmpDs, "DailyProd");
                    }
                
                    transaction.Commit();
                    cn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_DailyProduction(ProdCost_DailyProductionH DailyProductionH, List<ProdCost_DailyProductionD> DailyProductionDF)
        {
            DataSet TmpDs = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT  * FROM dbo.ProdCost_DailyProductionD WHERE CompNo = 0", cn);
                DaPR.Fill(TmpDs, "DailyProd");

                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "ProdCost_DelDailyProduction";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = DailyProductionH.CompNo;
                    cmddel.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt, 2)).Value = DailyProductionH.ReportYear;
                    cmddel.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int, 8)).Value = DailyProductionH.ReportNo;
                    cmddel.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = me.UserID;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }

                using (SqlCommand CmdH = new SqlCommand())
                {
                    CmdH.Connection = cn;
                    CmdH.CommandText = "ProdCost_AddDailyProductionH";
                    CmdH.CommandType = CommandType.StoredProcedure;
                    CmdH.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = DailyProductionH.CompNo;
                    CmdH.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = DailyProductionH.ReportYear;
                    CmdH.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int)).Value = DailyProductionH.ReportNo;
                    CmdH.Parameters.Add(new SqlParameter("@Prod_Date", SqlDbType.SmallDateTime)).Value = DailyProductionH.Prod_Date;
                    CmdH.Parameters.Add(new SqlParameter("@ShiftNo", SqlDbType.Int)).Value = DailyProductionH.ShiftNo;
                    CmdH.Parameters.Add(new SqlParameter("@Prod_stage", SqlDbType.Int)).Value = DailyProductionH.Prod_stage;
                    CmdH.Parameters.Add(new SqlParameter("@MachineNo", SqlDbType.Int)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 2;
                    CmdH.Parameters.Add(new SqlParameter("@UsedSrNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    CmdH.Transaction = transaction;
                    try
                    {
                        CmdH.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    int ReportNo = Convert.ToInt32(CmdH.Parameters["@UsedSrNo"].Value);
                    if (ReportNo == -10)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = Resources.Resource.errorDailyProduction }, JsonRequestBehavior.AllowGet);
                    }
                    int i = 1;
                    foreach (ProdCost_DailyProductionD item in DailyProductionDF)
                    {
                        DrTmp = TmpDs.Tables["DailyProd"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["ReportYear"] = item.ReportYear;
                        DrTmp["ReportNo"] = item.ReportNo;
                        DrTmp["ItemSer"] = i;
                        DrTmp["ProdPrepYear"] = item.ProdPrepYear;
                        DrTmp["ProdPrepNo"] = item.ProdPrepNo;
                        DrTmp["FinCode"] = item.FinCode;
                        DrTmp["Prod_Qty"] = item.Prod_Qty;
                        DrTmp["Tunit"] = item.TUnit;
                        if (item.PostFlag != null)
                        {
                            DrTmp["PostFlag"] = item.PostFlag;
                        }
                        if (item.ProdRecYear != null)
                        {
                            DrTmp["ProdRecYear"] = item.ProdRecYear;
                        }
                        if (item.ProdRecNo != null)
                        {
                            DrTmp["ProdRecNo"] = item.ProdRecNo;
                        }
                        if (item.TransferFlag != null)
                        {
                            DrTmp["TransferFlag"] = item.TransferFlag;
                        }
                        if (item.ConsVouYear != null)
                        {
                            DrTmp["ConsVouYear"] = item.ConsVouYear;
                        }
                        if (item.ConsVouNo != null)
                        {
                            DrTmp["ConsVouNo"] = item.ConsVouNo;
                        }
                        if (item.TransNoIn != null)
                        {
                            DrTmp["TransNoIn"] = item.TransNoIn;
                        }
                        if (item.TransNoOut != null)
                        {
                            DrTmp["TransNoOut"] = item.TransNoOut;
                        }
                        if (item.TransDateIn != null)
                        {
                            DrTmp["TransDateIn"] = item.TransDateIn;
                        }
                        if (item.TransDateOut != null)
                        {
                            DrTmp["TransDateOut"] = item.TransDateOut;
                        }
                        if (item.TransUserIn != null)
                        {
                            DrTmp["TransUserIn"] = item.TransUserIn;
                        }
                        if (item.TransUserOut != null)
                        {
                            DrTmp["TransUserOut"] = item.TransUserOut;
                        }
                        if (item.Batch != null)
                        {
                            DrTmp["Batch"] = item.Batch;
                        }
                        if (item.ManDate != null)
                        {
                            DrTmp["ManDate"] = item.ManDate;
                        }
                        if (item.ExpDate != null)
                        {
                            DrTmp["ExpDate"] = item.ExpDate;
                        }
                        if (item.ConsQty != null)
                        {
                            DrTmp["ConsQty"] = item.ConsQty;
                        }
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["UserID"] = me.UserID;
                        DrTmp.EndEdit();
                        TmpDs.Tables["DailyProd"].Rows.Add(DrTmp);
                        i = i + 1;
                    }
                    using (SqlCommand CmdDIns = new SqlCommand())
                    {
                        CmdDIns.Connection = cn;
                        CmdDIns.CommandText = "ProdCost_AddDailyProductionD";
                        CmdDIns.CommandType = CommandType.StoredProcedure;
                        CmdDIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = DailyProductionH.CompNo;
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportYear", SqlDbType.SmallInt, 4)).Value = DailyProductionH.ReportYear;
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int, 8)).Value = DailyProductionH.ReportNo;
                        CmdDIns.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.Int, 8, "ItemSer"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt, 4, "ProdPrepYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepNo", SqlDbType.Int, 8, "ProdPrepNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@FinCode", SqlDbType.VarChar, 20, "FinCode"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Prod_Qty", SqlDbType.Money, 8, "Prod_Qty"));
                        CmdDIns.Parameters.Add(new SqlParameter("@PostFlag", SqlDbType.Bit, 1, "PostFlag"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdRecYear", SqlDbType.SmallInt, 4, "ProdRecYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdRecNo", SqlDbType.Int, 8, "ProdRecNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransferFlag", SqlDbType.Bit, 1, "TransferFlag"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ConsVouYear", SqlDbType.SmallInt, 4, "ConsVouYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ConsVouNo", SqlDbType.Int, 8, "ConsVouNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransNoIn", SqlDbType.Int, 8, "TransNoIn"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransNoOut", SqlDbType.Int, 8, "TransNoOut"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransDateIn", SqlDbType.SmallDateTime, 10, "TransDateIn"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransDateOut", SqlDbType.SmallDateTime, 10, "TransDateOut"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransUserIn", SqlDbType.VarChar, 8, "TransUserIn"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransUserOut", SqlDbType.VarChar, 8, "TransUserOut"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8, "UserID"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 16, "Batch"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 10, "ManDate"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.SmallDateTime, 10, "ExpDate"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ConsQty", SqlDbType.Money, 8, "ConsQty"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Tunit", SqlDbType.VarChar, 5, "Tunit"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransType", SqlDbType.SmallInt, 4)).Value = 2;
                        CmdDIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdDIns;
                        DaPR.Update(TmpDs, "DailyProd");
                    }

                    transaction.Commit();
                    cn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete_DailyProduction(short comp_no, int ReportYear, int ReportNo)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "ProdCost_DelDailyProduction";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = comp_no;
                    cmddel.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt, 2)).Value = ReportYear;
                    cmddel.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int, 8)).Value = ReportNo;
                    cmddel.Parameters.Add(new SqlParameter("@TransType", SqlDbType.SmallInt, 4)).Value = 3;
                    cmddel.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = me.UserID;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }
                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public class ProdOrders
        {
            public short prepare_year { get; set; }
            public int prepare_code { get; set; }
            public DateTime prepare_date { get; set; }
            public string item_no { get; set; }
            public string ItemDesc { get; set; }
            public string ItemDesc_Ara { get; set; }
            public decimal qty_prepare { get; set; }
            public int? StoreNo { get; set; }
            public string StoreName { get; set; }
            public string stage_code { get; set; }
            public bool ClosingStat { get; set; }
            public byte? UnitSerial { get; set; }
            public string TUnit { get; set; }
            public double Qty { get; set; }
            public int serial { get; set; }

        }
    }
}