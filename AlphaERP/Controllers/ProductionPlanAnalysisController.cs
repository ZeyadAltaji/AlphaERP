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
    public class ProductionPlanAnalysisController : controller
    {
        List<CreatePlanPRNo> lCountAdd = new List<CreatePlanPRNo>();

        // GET: ProductionPlanAnalysis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ActivateProductionPlain(int year)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "MRPW_GetActivateProdPlanAnalysis";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", System.Data.SqlDbType.Int)).Value = year;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<MRP_ProductionPlain> lMRP_ProductionPlain = Dt.AsEnumerable().Select(row => new MRP_ProductionPlain
            {
                PlanYear = row.Field<short>("PlanYear"),
                PlanNo = row.Field<int>("PlanNo"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate"),
                PlanNotes = row.Field<string>("PlanNotes")
            }).ToList();
            return PartialView(lMRP_ProductionPlain);
        }
        public ActionResult CalcActivatePlan(short Planyear, int PlanNo)
        {
            MRP_GeneralPlanInfoH PlanH = db.MRP_GeneralPlanInfoH.Where(x => x.PlanYear == Planyear && x.PlanNo == PlanNo).FirstOrDefault();
            int daysDiff = ((TimeSpan)(PlanH.EndDate - PlanH.StartDate)).Days;
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "MRP_MRPCalculation";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = Planyear;
                    cmd.Parameters.Add(new SqlParameter("@PlanNo", SqlDbType.Int)).Value = PlanNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<MRP_Calculation> lMRP_Calculation = Dt.AsEnumerable().Select(row => new MRP_Calculation
            {
                LeadTime = row.Field<int>("LeadTime"),
                StockCoverage = row.Field<int>("stockCoverage"),
                ReOrderPoint = row.Field<short>("ReOrderPoint"),
                MOQ = row.Field<decimal>("MOQ"),
                OrderMulti = row.Field<long>("OrderMulti"),

                PlanYear = Planyear,
                PlanNo = PlanNo,
                Days = daysDiff,
                StartDate = PlanH.StartDate,
                EndDate = PlanH.EndDate,
                PlanNote = PlanH.PlanNotes,

                ItemNo = row.Field<string>("ItemNo"),
                ItemDesc = row.Field<string>("ItemDesc"),
                UnitDesc = row.Field<string>("Un1Desc"),
                TotalReqQty = row.Field<decimal>("TotalReqQty"),
                TotalPrvQty = row.Field<decimal>("TotalPrvQty"),
                QtyOH = row.Field<decimal>("QtyOH"),
                QtyOHCDays = row.Field<int>("QtyOHCDays"),
                QtyInOrder = row.Field<decimal>("QtyInOrder"),
                QtyInOrderCDays = row.Field<int>("QtyInOrderCDays"),
                TotalAvialbeQty = row.Field<decimal>("TotalAvialbeQty"),
                StockCDays = row.Field<int>("StockCDays"),
                OrderStat = row.Field<bool>("OrderStat"),
                QtyShoratge = row.Field<decimal>("QtyShoratge"),
                NoOforders = row.Field<decimal>("NoOforders"),
                QtyToOrder = row.Field<decimal>("QtyToOrder"),
                Prefvendor = row.Field<long>("Prefvendor"),
                SupplierName = row.Field<string>("name"),
                PlanPRNo = row.Field<string>("PlanPRNo"),
            }).ToList();
            return PartialView(lMRP_Calculation);
        }
        public JsonResult CreatePurchaseOrders(List<CreatePlanPRNo> lPlanPRNo)
        {
            if (lPlanPRNo.FirstOrDefault().GroupItemBySupplier == true)
            {
                var Vender = lPlanPRNo.GroupBy(x => x.venderNo).ToList();
                lCountAdd = new List<CreatePlanPRNo>();
                foreach (var V in Vender)
                {
                    List<CreatePlanPRNo> dCreatePlanPRNo = new List<CreatePlanPRNo>();
                    List<CreatePlanPRNo> lPRNo = lPlanPRNo.Where(x => x.venderNo == V.Key).ToList();
                    foreach (CreatePlanPRNo item1 in lPRNo)
                    {
                        if (item1.QtyToOrder != 0)
                        {
                            CreatePlanPRNo drPlanPRNo = new CreatePlanPRNo();
                            drPlanPRNo.PlanYear = lPlanPRNo.FirstOrDefault().PlanYear;
                            drPlanPRNo.PlanNo = lPlanPRNo.FirstOrDefault().PlanNo;
                            drPlanPRNo.ItemNo = item1.ItemNo;
                            drPlanPRNo.QtyToOrder = item1.QtyToOrder;
                            drPlanPRNo.venderNo = item1.venderNo;
                            dCreatePlanPRNo.Add(drPlanPRNo);
                            lCountAdd.Add(drPlanPRNo);
                        }
                    }
                    if (dCreatePlanPRNo.Count != 0)
                    {
                        bool GroupItemBySupplier = InsertGroupItemBySupplier(dCreatePlanPRNo);
                        if (GroupItemBySupplier == false)
                        {
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (lCountAdd.Count == 0)
                {
                    return Json(new { error = "لم يتم ادخال الكمية المقترحة للطلب" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (lPlanPRNo.FirstOrDefault().PurchaseOrderForEachItem == true)
            {
                lCountAdd = new List<CreatePlanPRNo>();
                foreach (CreatePlanPRNo item in lPlanPRNo)
                {
                    if (item.QtyToOrder != 0)
                    {
                        bool PurchaseOrderForEachItem = InsertPurchaseOrderForEachItem(item);
                        if (PurchaseOrderForEachItem == false)
                        {
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                        lCountAdd.Add(item);
                    }
                }
                if (lCountAdd.Count == 0)
                {
                    return Json(new { error = "لم يتم ادخال الكمية المقترحة للطلب" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (lPlanPRNo.FirstOrDefault().OnePurchaseOrderForAllItems == true)
            {
                lCountAdd = new List<CreatePlanPRNo>();
                bool OnePurchaseOrderForAllItems = InsertOnePurchaseOrderForAllItems(lPlanPRNo);
                if (OnePurchaseOrderForAllItems == false)
                {
                    if (lCountAdd.Count == 0)
                    {
                        return Json(new { error = "لم يتم ادخال الكمية المقترحة للطلب" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool InsertGroupItemBySupplier(List<CreatePlanPRNo> lPlanPRNo)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            int i = 1;
            int PRNo;
            string query = string.Format("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0");
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "PReqDF");
                ds.Tables["PReqDF"].PrimaryKey = new DataColumn[] { ds.Tables["PReqDF"].Columns["PReqDF"] };

                foreach (CreatePlanPRNo item in lPlanPRNo)
                {
                    if (item.QtyToOrder != 0)
                    {
                        DrTmp = ds.Tables["PReqDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrdYear"] = DateTime.Now.Year;
                        DrTmp["OrderNo"] = 0;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["ItemSr"] = i;
                        DrTmp["NSItemNo"] = 0;
                        DrTmp["Price"] = 0;
                        DrTmp["ExtraPrice"] = 0;
                        DrTmp["Qty"] = Math.Ceiling(Math.Abs(item.QtyToOrder));
                        DrTmp["Bonus"] = 0;
                        DrTmp["TotalValue"] = 0;
                        DrTmp["Confirmation"] = 0;
                        DrTmp["ConfirmQty"] = Math.Ceiling(Math.Abs(item.QtyToOrder));
                        DrTmp["Note"] = "Purchase Requestion For Plan No. " + item.PlanNo;
                        DrTmp["DiscPer"] = 0;
                        DrTmp["ValueAfterDisc"] = 0;
                        DrTmp["DiscAmt"] = 0;
                        DrTmp["UnitSerial"] = 1;

                        DrTmp.EndEdit();
                        ds.Tables["PReqDF"].Rows.Add(DrTmp);
                        i = i + 1;
                    }
                }
                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = cn;
                    cmd1.CommandText = "Ord_AppPreOrderHF";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationType", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderPurpose", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = lPlanPRNo.FirstOrDefault().PlanYear + " - " + lPlanPRNo.FirstOrDefault().PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@OrdSource", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderCateg", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd1.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = lPlanPRNo.FirstOrDefault().venderNo;
                    cmd1.Parameters.Add(new SqlParameter("@BuyerNo", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrdOrg", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = lPlanPRNo.FirstOrDefault().PlanYear;
                    cmd1.Parameters.Add(new SqlParameter("@PlanNo", SqlDbType.Int)).Value = lPlanPRNo.FirstOrDefault().PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@LogId", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    transaction = cn.BeginTransaction();
                    cmd1.Transaction = transaction;
                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    if (Convert.ToInt32(cmd1.Parameters["@ErrNo"].Value) != 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    PRNo = Convert.ToInt32(cmd1.Parameters["@UsedOrdNo"].Value);

                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = cn;
                        cmd2.CommandText = "Ord_AppPreOrderDF";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd2.Parameters.Add(new SqlParameter("@PYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8)).Value = PRNo;
                        cmd2.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@NSItem", SqlDbType.VarChar, 20, "NSItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 8, "Price"));
                        cmd2.Parameters.Add(new SqlParameter("@ExtraPrice", SqlDbType.Float, 8, "ExtraPrice"));
                        cmd2.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd2.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Money, 8, "Bonus"));
                        cmd2.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Money, 8, "TotalValue"));
                        cmd2.Parameters.Add(new SqlParameter("@Confirm", SqlDbType.Bit, 1, "Confirmation"));
                        cmd2.Parameters.Add(new SqlParameter("@ConfirmQty", SqlDbType.Float, 8, "ConfirmQty"));
                        cmd2.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscPer", SqlDbType.Money, 8, "DiscPer"));
                        cmd2.Parameters.Add(new SqlParameter("@ValueAfterDisc", SqlDbType.Money, 8, "ValueAfterDisc"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscAmt", SqlDbType.Money, 8, "DiscAmt"));
                        cmd2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd2.Transaction = transaction;
                        DaPR.InsertCommand = cmd2;
                        DaPR.Update(ds, "PReqDF");
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }
            return true;
        }
        public bool InsertPurchaseOrderForEachItem(CreatePlanPRNo lPlanPRNo)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            int i = 1;
            int PRNo;
            string query = string.Format("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0");
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "PReqDF");
                ds.Tables["PReqDF"].PrimaryKey = new DataColumn[] { ds.Tables["PReqDF"].Columns["PReqDF"] };

                DrTmp = ds.Tables["PReqDF"].NewRow();
                DrTmp.BeginEdit();
                DrTmp["CompNo"] = company.comp_num;
                DrTmp["OrdYear"] = DateTime.Now.Year;
                DrTmp["OrderNo"] = 0;
                DrTmp["ItemNo"] = lPlanPRNo.ItemNo;
                DrTmp["ItemSr"] = i;
                DrTmp["NSItemNo"] = 0;
                DrTmp["Price"] = 0;
                DrTmp["ExtraPrice"] = 0;
                DrTmp["Qty"] = Math.Ceiling(Math.Abs(lPlanPRNo.QtyToOrder));
                DrTmp["Bonus"] = 0;
                DrTmp["TotalValue"] = 0;
                DrTmp["Confirmation"] = 0;
                DrTmp["ConfirmQty"] = Math.Ceiling(Math.Abs(lPlanPRNo.QtyToOrder));
                DrTmp["Note"] = "Purchase Requestion For Plan No. " + lPlanPRNo.PlanNo;
                DrTmp["DiscPer"] = 0;
                DrTmp["ValueAfterDisc"] = 0;
                DrTmp["DiscAmt"] = 0;
                DrTmp["UnitSerial"] = 1;

                DrTmp.EndEdit();
                ds.Tables["PReqDF"].Rows.Add(DrTmp);

                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = cn;
                    cmd1.CommandText = "Ord_AppPreOrderHF";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationType", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderPurpose", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = lPlanPRNo.PlanYear + " - " + lPlanPRNo.PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@OrdSource", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderCateg", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd1.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = lPlanPRNo.venderNo;
                    cmd1.Parameters.Add(new SqlParameter("@BuyerNo", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrdOrg", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = lPlanPRNo.PlanYear;
                    cmd1.Parameters.Add(new SqlParameter("@PlanNo", SqlDbType.Int)).Value = lPlanPRNo.PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@LogId", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    transaction = cn.BeginTransaction();
                    cmd1.Transaction = transaction;
                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    if (Convert.ToInt32(cmd1.Parameters["@ErrNo"].Value) != 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    PRNo = Convert.ToInt32(cmd1.Parameters["@UsedOrdNo"].Value);

                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = cn;
                        cmd2.CommandText = "Ord_AppPreOrderDF";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd2.Parameters.Add(new SqlParameter("@PYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8)).Value = PRNo;
                        cmd2.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@NSItem", SqlDbType.VarChar, 20, "NSItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 8, "Price"));
                        cmd2.Parameters.Add(new SqlParameter("@ExtraPrice", SqlDbType.Float, 8, "ExtraPrice"));
                        cmd2.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd2.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Money, 8, "Bonus"));
                        cmd2.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Money, 8, "TotalValue"));
                        cmd2.Parameters.Add(new SqlParameter("@Confirm", SqlDbType.Bit, 1, "Confirmation"));
                        cmd2.Parameters.Add(new SqlParameter("@ConfirmQty", SqlDbType.Float, 8, "ConfirmQty"));
                        cmd2.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscPer", SqlDbType.Money, 8, "DiscPer"));
                        cmd2.Parameters.Add(new SqlParameter("@ValueAfterDisc", SqlDbType.Money, 8, "ValueAfterDisc"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscAmt", SqlDbType.Money, 8, "DiscAmt"));
                        cmd2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd2.Transaction = transaction;
                        DaPR.InsertCommand = cmd2;
                        DaPR.Update(ds, "PReqDF");
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }
            return true;
        }
        public bool InsertOnePurchaseOrderForAllItems(List<CreatePlanPRNo> lPlanPRNo)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            int i = 1;
            int PRNo;
            string query = string.Format("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0");
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "PReqDF");
                ds.Tables["PReqDF"].PrimaryKey = new DataColumn[] { ds.Tables["PReqDF"].Columns["PReqDF"] };

                foreach (CreatePlanPRNo item in lPlanPRNo)
                {
                    if (item.QtyToOrder != 0)
                    {
                        DrTmp = ds.Tables["PReqDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrdYear"] = DateTime.Now.Year;
                        DrTmp["OrderNo"] = 0;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["ItemSr"] = i;
                        DrTmp["NSItemNo"] = 0;
                        DrTmp["Price"] = 0;
                        DrTmp["ExtraPrice"] = 0;
                        DrTmp["Qty"] = Math.Ceiling(Math.Abs(item.QtyToOrder));
                        DrTmp["Bonus"] = 0;
                        DrTmp["TotalValue"] = 0;
                        DrTmp["Confirmation"] = 0;
                        DrTmp["ConfirmQty"] = Math.Ceiling(Math.Abs(item.QtyToOrder));
                        DrTmp["Note"] = "Purchase Requestion For Plan No. " + item.PlanNo;
                        DrTmp["DiscPer"] = 0;
                        DrTmp["ValueAfterDisc"] = 0;
                        DrTmp["DiscAmt"] = 0;
                        DrTmp["UnitSerial"] = 1;

                        DrTmp.EndEdit();
                        ds.Tables["PReqDF"].Rows.Add(DrTmp);
                        lCountAdd.Add(item);
                        i = i + 1;
                    }
                }
                if (ds.Tables["PReqDF"].Rows.Count == 0)
                {
                    return false;
                }

                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = cn;
                    cmd1.CommandText = "Ord_AppPreOrderHF";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@OperationType", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderPurpose", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = lPlanPRNo.FirstOrDefault().PlanYear + " - " + lPlanPRNo.FirstOrDefault().PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@OrdSource", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderCateg", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd1.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@BuyerNo", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@OrdOrg", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = lPlanPRNo.FirstOrDefault().PlanYear;
                    cmd1.Parameters.Add(new SqlParameter("@PlanNo", SqlDbType.Int)).Value = lPlanPRNo.FirstOrDefault().PlanNo;
                    cmd1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@LogId", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    transaction = cn.BeginTransaction();
                    cmd1.Transaction = transaction;
                    try
                    {
                        cmd1.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    if (Convert.ToInt32(cmd1.Parameters["@ErrNo"].Value) != 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return false;
                    }

                    PRNo = Convert.ToInt32(cmd1.Parameters["@UsedOrdNo"].Value);

                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = cn;
                        cmd2.CommandText = "Ord_AppPreOrderDF";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd2.Parameters.Add(new SqlParameter("@PYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8)).Value = PRNo;
                        cmd2.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@NSItem", SqlDbType.VarChar, 20, "NSItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 8, "Price"));
                        cmd2.Parameters.Add(new SqlParameter("@ExtraPrice", SqlDbType.Float, 8, "ExtraPrice"));
                        cmd2.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd2.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Money, 8, "Bonus"));
                        cmd2.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Money, 8, "TotalValue"));
                        cmd2.Parameters.Add(new SqlParameter("@Confirm", SqlDbType.Bit, 1, "Confirmation"));
                        cmd2.Parameters.Add(new SqlParameter("@ConfirmQty", SqlDbType.Float, 8, "ConfirmQty"));
                        cmd2.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscPer", SqlDbType.Money, 8, "DiscPer"));
                        cmd2.Parameters.Add(new SqlParameter("@ValueAfterDisc", SqlDbType.Money, 8, "ValueAfterDisc"));
                        cmd2.Parameters.Add(new SqlParameter("@DiscAmt", SqlDbType.Money, 8, "DiscAmt"));
                        cmd2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd2.Transaction = transaction;
                        DaPR.InsertCommand = cmd2;
                        DaPR.Update(ds, "PReqDF");
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }
            return true;
        }
        public class CreatePlanPRNo
        {
            public short PlanYear { get; set; }
            public int PlanNo { get; set; }
            public string ItemNo { get; set; }
            public decimal QtyToOrder { get; set; }
            public long venderNo { get; set; }
            public bool GroupItemBySupplier { get; set; }
            public bool PurchaseOrderForEachItem { get; set; }
            public bool OnePurchaseOrderForAllItems { get; set; }

        }
    }
}