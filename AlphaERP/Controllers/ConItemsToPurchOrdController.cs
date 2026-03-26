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
    public class ConItemsToPurchOrdController : controller
    {
        // GET: ConItemsToPurchOrd
        public ActionResult Index()
        {
            List<Ord_RequestDF> lOrdRequestDF = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.bPurchaseOrder == false && x.Ord_RequestHF.ReqStatus == 1).OrderByDescending(o => o.ReqNo).ToList();
            return View(lOrdRequestDF);
        }
        public ActionResult PurchaseRequestList()
        {
            return PartialView();
        }
        public ActionResult ConItemsToPurchOrdList()
        {
            List<Ord_RequestDF> lOrdRequestDF = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.bPurchaseOrder == false && x.Ord_RequestHF.ReqStatus == 1).ToList();
            return View(lOrdRequestDF);
        }
        public ActionResult Vendors(int DeptId)
        {
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("select GLCRBMF.crb_dep as deptid, Vendors.VendorNo as AccId, Vendors.Name as AccDescAr, Vendors.Eng_Name as AccDescEn " +
                                                       "FROM GLCRBMF INNER JOIN Vendors ON Vendors.comp = GLCRBMF.CRB_COMP AND Vendors.VendorNo = GLCRBMF.crb_acc " +
                                                        "WHERE(GLCRBMF.crb_dep = '{0}') AND (GLCRBMF.CRB_COMP = '{1}') AND (Vendors.IsHalt = 0)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public string GetOrderSerial(int PYear, int Scode, SqlTransaction MyTrans, SqlConnection co)
        {
            int OrdSerial =0;
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
        public JsonResult CreatePurchOrder(int DeptId,long VendorNo,int BU, List<Ord_RequestDF> OrdReqDF)
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            DataRow DrTmp;
            int OrdNo = 0;
            string TawreedNo = "0";
            int i = 1;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "POrderDF");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "PReqDF");

                transaction = cn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Ord_AppPreOrderHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
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
                    cmd.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
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
                        var xxx = ex.Message;
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
                    var Items = OrdReqDF.GroupBy(x => x.SubItemNo).ToList();
                    double? TotalQty = 0;
                    foreach (var item in Items)
                    {
                        List<Ord_RequestDF> OrdReqDF1 = OrdReqDF.Where(x => x.SubItemNo == item.Key).ToList();
                        double? Qty = 0;
                        double? Price = 0;
                        double? TotolPrice = 0;
                        foreach (Ord_RequestDF DF in OrdReqDF1)
                        {
                            Qty += DF.Qty;
                            Price += DF.Price;
                        }
                        if (OrdReqDF1.Count > 1)
                        {
                            TotolPrice = (Price / OrdReqDF1.Count);
                        }
                        else
                        {
                            TotolPrice = Price;
                        }
                        TotalQty += Qty * TotolPrice;

                        DrTmp = ds.Tables["POrderDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrdYear"] = DateTime.Now.Year;
                        DrTmp["OrderNo"] = OrdNo;
                        DrTmp["ItemNo"] = item.Key;
                        DrTmp["NSItemNo"] = 0;
                        DrTmp["Price"] = TotolPrice;
                        DrTmp["ExtraPrice"] = 0;
                        DrTmp["Qty"] = Qty;
                        DrTmp["Bonus"] = 0;
                        DrTmp["TotalValue"] = TotolPrice * Qty;
                        DrTmp["Confirmation"] = 1;
                        DrTmp["ConfirmQty"] = Qty;
                        DrTmp["Note"] = "";
                        DrTmp["DiscPer"] = 0;
                        DrTmp["ValueAfterDisc"] = Qty;
                        DrTmp["DiscAmt"] = 0;
                        DrTmp["UnitSerial"] = 1;
                        DrTmp["ItemSr"] = i;

                        DrTmp.EndEdit();
                        ds.Tables["POrderDF"].Rows.Add(DrTmp);
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
                        DaPR.InsertCommand = cmd1;
                        try
                        {
                            DaPR.Update(ds, "POrderDF");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                TawreedNo = GetOrderSerial(DateTime.Now.Year, 6, transaction, cn);

                if (TawreedNo == "0")
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = Resources.Resource.errorPurchaseOrderSerial }, JsonRequestBehavior.AllowGet);
                }

                var Items1 = OrdReqDF.GroupBy(x => x.SubItemNo).ToList();
                double? TotalQty1 = 0;
                foreach (var item in Items1)
                {
                    List<Ord_RequestDF> OrdReqDF1 = OrdReqDF.Where(x => x.SubItemNo == item.Key).ToList();
                    double? Qty = 0;
                    double? Price = 0;
                    double? TotolPrice = 0;
                    foreach (Ord_RequestDF DF in OrdReqDF1)
                    {
                        Qty += DF.Qty;
                        Price += DF.Price;
                    }
                    if (OrdReqDF1.Count > 1)
                    {
                        TotolPrice = (Price / OrdReqDF1.Count);
                    }
                    else
                    {
                        TotolPrice = Price;
                    }
                    TotalQty1 += Qty * TotolPrice;

                    InvItemsMF Mf = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.Key).FirstOrDefault();
                    InvUnitCode U = db.InvUnitCodes.Where(x => x.CompNo == company.comp_num && x.UnitCode == Mf.UnitC1).FirstOrDefault();
                    DrTmp = ds1.Tables["PReqDF"].NewRow();
                    DrTmp.BeginEdit();
                    DrTmp["CompNo"] = company.comp_num;
                    DrTmp["OrderNo"] = OrdNo;
                    DrTmp["OrdYear"] = DateTime.Now.Year;
                    DrTmp["TawreedNo"] = TawreedNo;
                    DrTmp["ItemNo"] = item.Key;
                    DrTmp["ItemSr"] = i;
                    DrTmp["PUnit"] = U.UnitNameEng;
                    DrTmp["PerDiscount"] = 0;
                    DrTmp["Bonus"] = 0;
                    DrTmp["Qty"] = Qty;
                    DrTmp["Price"] = TotolPrice;
                    DrTmp["ItemTaxType"] = true;
                    DrTmp["ItemTaxPer"] = 0;
                    DrTmp["ItemTaxVal"] = 0;
                    DrTmp["RefNo"] = 0;
                    DrTmp["ordamt"] = TotolPrice * Qty;
                    DrTmp["Note"] = "";
                    DrTmp["NoteDtl"] = "";
                    DrTmp["UnitSerial"] = 1;

                    DrTmp.EndEdit();
                    ds1.Tables["PReqDF"].Rows.Add(DrTmp);
                    i = i + 1;
                }

                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = cn;
                    cmd1.CommandText = "Ord_AddOrderHF";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
                    cmd1.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                    cmd1.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = VendorNo;
                    cmd1.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar)).Value = "1";
                    cmd1.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء امر شراء من الويب";
                    cmd1.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = "1";
                    cmd1.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = TotalQty1;
                    cmd1.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "Each";
                    cmd1.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = TotalQty1;
                    cmd1.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = me.UserName;
                    cmd1.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = DeptId;
                    cmd1.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = "1";
                    cmd1.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = "إنشاء امر شراء من الويب";
                    cmd1.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@ShipTerms", SqlDbType.VarChar)).Value = "";
                    cmd1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd1.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = "";
                    cmd1.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = false;
                    cmd1.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = 0;
                    cmd1.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd1.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd1.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(new SqlParameter("@LastCost", SqlDbType.Bit)).Value = true;

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
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    if (Convert.ToInt32(cmd1.Parameters["@ErrNo"].Value) != 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    long LogID = Convert.ToInt64(cmd1.Parameters["@LogID"].Value);

                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = cn;
                        cmd2.CommandText = "Ord_AddOrderDF";
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd2.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
                        cmd2.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                        cmd2.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 8, "PerDiscount"));
                        cmd2.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
                        cmd2.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmd2.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 8, "Price"));
                        cmd2.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
                        cmd2.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                        cmd2.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 8, "ordamt"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemTaxType", SqlDbType.Bit, 1, "ItemTaxType"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 8, "ItemTaxPer"));
                        cmd2.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 8, "ItemTaxVal"));
                        cmd2.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money, 8, "VouDiscount"));
                        cmd2.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                        cmd2.Parameters.Add(new SqlParameter("@NoteDtl", SqlDbType.Text, 300, "NoteDtl"));
                        cmd2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd2.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;

                        cmd2.Transaction = transaction;
                        DaPR1.InsertCommand = cmd2;
                        DaPR1.Update(ds1, "PReqDF");
                    }

                    bool InsertWorkFlow = AddWorkFlowLog(3, DateTime.Now.Year, OrdNo.ToString(), TawreedNo.ToString(), BU, 1, TotalQty1, transaction, cn);
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

            foreach (Ord_RequestDF item in OrdReqDF)
            {
                int year = DateTime.Now.Year;
                Ord_RequestHF ex1 = db.Ord_RequestHF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo).FirstOrDefault();
                ex1.ReqStatus = 4;
                db.SaveChanges();

                Ord_RequestDF ex = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.SubItemNo == item.SubItemNo && x.ItemSr == item.ItemSr).FirstOrDefault();
                ex.PurchaseOrderYear = Convert.ToInt16(year);
                ex.PurchaseOrderNo = OrdNo;
                ex.PurchaseOrdTawreedNo = TawreedNo;
                ex.PurchaseOrdOfferNo = 0;
                ex.bPurchaseOrder = true;
                db.SaveChanges();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLog(int FID, int TrYear, string TrNo, string TawreedNo, int BU, int FStat, double? TrAmnt,  SqlTransaction MyTrans, SqlConnection co)
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
                    string zz = ex.Message;
                    return false;
                }
            }
            return true;
        }
    }
}