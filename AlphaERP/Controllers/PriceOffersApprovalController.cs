using CrystalDecisions.Shared;
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
    public class PriceOffersApprovalController : controller
    {
        // GET: PriceOffersApproval
        public ActionResult Index()
        {
            return View();
        }
        public string CreateReportUniqueId(string path)
        {
            int index1 = path.LastIndexOf('\\');
            int index2 = path.LastIndexOf('.');
            return path.Substring(index1 + 1, index2 - index1 - 1);
        }
        [HttpPost]
        public ActionResult GenerateReport(int ReqforQuotyear, int ReqforQuotNo)
        {
            ReportInformation reportInfo = new ReportInformation();
            DataSet Alpha_ERP_DataSet = new DataSet();
            DataTable d1 = Ord_RptOrdCopyInfo(ReqforQuotyear, ReqforQuotNo);
            d1.TableName = "Ord_RptOrdCopyInfo";
            Alpha_ERP_DataSet.Tables.Add(d1);

            DataTable d2 = Comp_FillImage();
            d2.TableName = "Comp_FillImage";
            Alpha_ERP_DataSet.Tables.Add(d2);

            reportInfo.myDataSet = Alpha_ERP_DataSet;

            reportInfo.ReportName = "اعتماد عروض الاسعار";

            reportInfo.fields = SetParameterField();

            if (Language == "ar-JO")
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptOrdCopyInfo.rpt");
            }
            else
            {
                reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptOrdCopyInfoEng.rpt");
            }

            string reportUniqueName = CreateReportUniqueId(reportInfo.path);
            ReportInfoManager.AddReport(reportUniqueName, reportInfo);

            this.HttpContext.Session["reportInfoID"] = reportInfo.Id;
            return RedirectToAction("ReportViewer", "CrystalReportViewer", new { area = "", id = reportInfo.Id });
        }
        public ParameterFields SetParameterField()
        {
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
            /************************/

            return parameterFields;
        }
        public DataTable Ord_RptOrdCopyInfo(int ReqforQuotyear, int ReqforQuotNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptOrdCopyInfo";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ReqforQuotyear", System.Data.SqlDbType.Int)).Value = ReqforQuotyear;
                    cmd.Parameters.Add(new SqlParameter("@ReqforQuotNo", System.Data.SqlDbType.Int)).Value = ReqforQuotNo;
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
        public ActionResult PriceOffersApprovalList()
        {
            return PartialView();
        }
        public ActionResult ChoosePriceOffersApproval(int ReqforQuotyear, int ReqforQuotNo)
        {
            ViewBag.ReqforQuotyear = ReqforQuotyear;
            ViewBag.ReqforQuotNo = ReqforQuotNo;


            return PartialView();
        }
        public JsonResult SaveBestOffer(int ReqforQuotyear, int ReqforQuotNo, long VendorNo, string Notes)
        {
            List<MRP_Web_OrdCopyInfo> OrdCopyInfo = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo).ToList();
            foreach (MRP_Web_OrdCopyInfo item in OrdCopyInfo)
            {
                MRP_Web_OrdCopyInfo ex1 = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotNo == item.ReqforQuotNo
                && x.VendorNo == item.VendorNo && x.ItemNo == item.ItemNo).FirstOrDefault();

                ex1.Notes = Notes;

                if (ex1.VendorNo == VendorNo)
                {
                    ex1.bBestOffer = true;
                    ex1.bNominationBestOffer = true;
                }
                else
                {
                    ex1.bBestOffer = false;
                    ex1.bNominationBestOffer = false;
                }
                db.SaveChanges();
            }
            return Json(new { ReqforQuotNo = ReqforQuotNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApprovalBestOffer(int ReqforQuotyear, int ReqforQuotNo, long VendorNo,string Notes)
        {
            Vendor Vend = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == VendorNo).FirstOrDefault();
            int Dept = new MDB().Database.SqlQuery<int>(string.Format("select TOP 1 crb_dep as VenderId from GLCRBMF where (CRB_COMP = '{0}') AND (crb_acc = '{1}')", company.comp_num, Vend.VendorNo)).FirstOrDefault();

            if(Dept == 0)
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

            List<MRP_Web_OrdCopyInfo> OrdCopyInfo = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo).ToList();
            foreach (MRP_Web_OrdCopyInfo item in OrdCopyInfo)
            {
                MRP_Web_OrdCopyInfo ex1 = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotNo == item.ReqforQuotNo 
                && x.VendorNo == item.VendorNo && x.ItemNo == item.ItemNo).FirstOrDefault();
                ex1.bPriceOffersApproval = true;
                ex1.Notes = Notes;
                if (ex1.VendorNo == VendorNo)
                {
                    ex1.bBestOffer = true;
                }
                else
                {
                    ex1.bBestOffer = false;
                }
                db.SaveChanges();
            }

            Ord_RequestDF Orddf = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.RequestforQuotationYear == ReqforQuotyear && x.RequestforQuotationNo == ReqforQuotNo).FirstOrDefault();
            Ord_RequestHF OrdHF = db.Ord_RequestHF.Where(x => x.CompNo == Orddf.CompNo && x.ReqYear == Orddf.ReqYear && x.ReqNo == Orddf.ReqNo).FirstOrDefault();


            int OrdNo = 0;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                SqlDataAdapter DaPR = new SqlDataAdapter();
                SqlDataAdapter DaPR1 = new SqlDataAdapter();
                SqlDataAdapter DaPR2 = new SqlDataAdapter();

                DataRow DrTmp;
                DataRow DrTmp1;
                DataRow DrTmp2;

                int i = 1;
                int gLcDept = 0;
                long gLcAcc = 0;

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "POrderDF");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdVOfferCondsGuarnty  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TblConds");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdOfferDF  WHERE compno = 0", cn);
                DaPR2.Fill(ds2, "OrdOfferDF");



               List<MRP_Web_OrdCopyInfo> OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num && x.ReqforQuotyear == ReqforQuotyear && x.ReqforQuotNo == ReqforQuotNo && x.VendorNo == VendorNo && x.bBestOffer == true).ToList();
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
                    cmd.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmd.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = true;
                    cmd.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
                    cmd.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = "إنشاء طلب شراء من الويب";
                    cmd.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@BU", SqlDbType.SmallInt)).Value = OrdHF.AuthBU;
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
                    

                    foreach (MRP_Web_OrdCopyInfo item in OrdCopy)
                    {
                        double? Qty = 0;
                        double? Qty2 = 0;
                        decimal? SellPrice = 0;
                        double Total = 0;

                        Qty = item.Qty.Value;
                        Qty2 = item.Qty2.Value;
                        SellPrice = item.SellPrice.Value;

                        Total = Convert.ToDouble(SellPrice) * item.Qty.Value;

                        DrTmp = ds.Tables["POrderDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrdYear"] = DateTime.Now.Year;
                        DrTmp["OrderNo"] = OrdNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["NSItemNo"] = 0;
                        DrTmp["Price"] = SellPrice;
                        DrTmp["ExtraPrice"] = 0;
                        DrTmp["Qty"] = Qty;
                        DrTmp["Qty2"] = Qty2;
                        DrTmp["Bonus"] = item.Bonus;
                        DrTmp["TotalValue"] = Total;
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

                using (SqlCommand cmdOrdCopy = new SqlCommand())
                {
                    cmdOrdCopy.Connection = cn;
                    cmdOrdCopy.CommandText = "Ord_AddOrdCopyInfo";
                    cmdOrdCopy.CommandType = CommandType.StoredProcedure;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@CYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = VendorNo;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@ReceiptNo", SqlDbType.Int)).Value = OrdNo;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@SellDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "4";
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar)).Value = "Jordan";
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@PayCond", SqlDbType.Int)).Value = 1;
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@RecCond", SqlDbType.VarChar)).Value = "";
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@Accept", SqlDbType.VarChar)).Value = "";
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء طلب عرض سعر من الويب";
                    cmdOrdCopy.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmdOrdCopy.Transaction = transaction;
                    try
                    {
                        cmdOrdCopy.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) != 0)
                    {
                        if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) == 2627)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "en")
                            {
                                return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) == 2627)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "en")
                            {
                                return Json(new { error = "P.O No Dosent Have a Bid ,Operation Failed" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "لم يتم تعريف عطاء لطلب الشراء المدخل لم تنجح العملية" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }



                using (SqlCommand cmdOrdOfferHF = new SqlCommand())
                {
                    Vendor v = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == VendorNo).FirstOrDefault();
                    string VendorOfferNo = OrdNo.ToString() + " - " + "1";
                    cmdOrdOfferHF.Connection = cn;
                    cmdOrdOfferHF.CommandText = "Ord_AddOrdOfferHF";
                    cmdOrdOfferHF.CommandType = CommandType.StoredProcedure;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OrdNo;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = VendorNo;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ValidateDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferCurr", SqlDbType.Int)).Value = OrdCopy.Select(x => x.Curr).FirstOrDefault();
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = OrdCopy.Select(x => x.Pmethod).FirstOrDefault();
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ShipCenter", SqlDbType.Int)).Value = 1;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@DelivDay", SqlDbType.Int)).Value = 0;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = 0;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@VendorOfferNo", SqlDbType.VarChar)).Value = VendorOfferNo;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@LcType", SqlDbType.SmallInt)).Value = 1;
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "ادخال عروض الموردين من الويب";
                    cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmdOrdOfferHF.Transaction = transaction;
                    try
                    {
                        cmdOrdOfferHF.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    foreach (MRP_Web_OrdCopyInfo item in OrdCopy)
                    {
                        InvItemsMF mf = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.ItemNo).FirstOrDefault();
                        DrTmp1 = ds2.Tables["OrdOfferDF"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = company.comp_num;
                        DrTmp1["OrdYear"] = DateTime.Now.Year;
                        DrTmp1["OrderNo"] = OrdNo;
                        DrTmp1["OfferNo"] = OrdNo;
                        DrTmp1["VendorNo"] = VendorNo;
                        DrTmp1["ItemNo"] = item.ItemNo;
                        DrTmp1["PerDiscount"] = 0;
                        DrTmp1["Bonus"] = item.Bonus.Value;
                        DrTmp1["Qty"] = item.Qty.Value;
                        DrTmp1["Qty2"] = item.Qty2.Value;
                        DrTmp1["Price1"] = item.SellPrice.Value;
                        DrTmp1["Price2"] = 0;
                        DrTmp1["Price3"] = 0;
                        DrTmp1["ItemSourceCode"] = 0;
                        DrTmp1["Paper"] = false;
                        DrTmp1["Free_Sample"] = false;
                        DrTmp1["PUnit"] = mf.UnitC4;

                        DrTmp1.EndEdit();
                        ds2.Tables["OrdOfferDF"].Rows.Add(DrTmp1);
                    }
                        

                    using (SqlCommand cmdOrdOfferDF = new SqlCommand())
                    {
                        cmdOrdOfferDF.Connection = cn;
                        cmdOrdOfferDF.CommandText = "Ord_AddOrdOfferDF";
                        cmdOrdOfferDF.CommandType = CommandType.StoredProcedure;
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 8, "VendorNo"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int, 8, "OfferNo"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 16, "PerDiscount"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 8, "Qty2"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price1", SqlDbType.Float, 8, "Price1"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price2", SqlDbType.Float, 8, "Price2"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price3", SqlDbType.Float, 8, "Price3"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ItemSourceCode", SqlDbType.VarChar, 20, "ItemSourceCode"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Paper", SqlDbType.Bit, 1, "Paper"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Free_Sample", SqlDbType.Bit, 1, "Free_Sample"));
                        cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output; ;
                        cmdOrdOfferDF.Transaction = transaction;
                        DaPR2.InsertCommand = cmdOrdOfferDF;
                        try
                        {
                            DaPR2.Update(ds2, "OrdOfferDF");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


                DrTmp2 = ds1.Tables["TblConds"].NewRow();
                DrTmp2.BeginEdit();
                DrTmp2["CompNo"] = company.comp_num;
                DrTmp2["OrdYear"] = DateTime.Now.Year;
                DrTmp2["OrderNo"] = OrdNo;
                DrTmp2["OfferNo"] = OrdNo;
                DrTmp2["VendorNo"] = VendorNo;
                DrTmp2["PayCond1"] = 1;
                DrTmp2["PayCond2"] = 0;
                DrTmp2["PayCond3"] = 0;
                DrTmp2["PayCond4"] = 0;
                DrTmp2["PayCond5"] = 0;
                DrTmp2["CondPerc1"] = 100;
                DrTmp2["CondPerc2"] = 0;
                DrTmp2["CondPerc3"] = 0;
                DrTmp2["CondPerc4"] = 0;
                DrTmp2["CondPerc5"] = 0;
                DrTmp2["Guaranty1"] = 0;
                DrTmp2["Guaranty2"] = 0;
                DrTmp2["Guaranty3"] = 0;
                DrTmp2["Guaranty4"] = 0;
                DrTmp2["Guaranty5"] = 0;
                DrTmp2["GuarantyPerc1"] = 0;
                DrTmp2["GuarantyPerc2"] = 0;
                DrTmp2["GuarantyPerc3"] = 0;
                DrTmp2["GuarantyPerc4"] = 0;
                DrTmp2["GuarantyPerc5"] = 0;
                DrTmp2.EndEdit();
                ds1.Tables["TblConds"].Rows.Add(DrTmp2);

                using (SqlCommand CmdInsConds = new SqlCommand())
                {
                    CmdInsConds.Connection = cn;
                    CmdInsConds.CommandText = "Ord_AddVOfferCondsGuarnty";
                    CmdInsConds.CommandType = CommandType.StoredProcedure;
                    CmdInsConds.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                    CmdInsConds.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                    CmdInsConds.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                    CmdInsConds.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int, 8, "OfferNo"));
                    CmdInsConds.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 8, "VendorNo"));
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

                transaction.Commit();
                cn.Dispose();

                using (var cn1 = new SqlConnection(ConnectionString()))
                {
                    SqlTransaction transaction1;
                    cn1.Open();

                    DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn1);
                    DaPR.Fill(ds, "TmpTbl");

                    DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdCondsGuaranty  WHERE compno = 0", cn1);
                    DaPR1.Fill(ds1, "TblConds");

                    transaction1 = cn1.BeginTransaction();

                    OrdOfferHF OffHF = db.OrdOfferHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).FirstOrDefault();

                    using (SqlCommand Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn1;
                        Cmd.CommandText = "Ord_ChkOrderHF";
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        Cmd.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OffHF.OrdYear;
                        Cmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OffHF.OrderNo;
                        Cmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OffHF.OrderNo;
                        Cmd.Transaction = transaction1;
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                cn.Dispose();
                                cn1.Dispose();
                                if (Language == "en")
                                {
                                    return Json(new { error = "P.O Is Referred To Direct Purchase On Tawreed NO. " + OffHF.OrderNo + "  " + "Add Operation Failed" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "طلب الشراء محال للشراء المباشر على رقم توريد " + OffHF.OrderNo + "  " + "لم تنجح عملية الاضافة" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                        rdr.Close();
                    }

                    using (SqlCommand Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn1;
                        Cmd.CommandText = "Ord_GetCompMF";
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        Cmd.Transaction = transaction1;
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
                    

                    List<OrdOfferDF> OffDF = db.OrdOfferDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).ToList();
                    double? totalTaxVal = 0;
                    double? Total = 0;
                    double? itemTaxPer = 0;
                    double? Per_Tax = 0;
                    double? TotalAmnt = 0;
                    double? TotalNetAmount = 0;
                    double? itemDiscVal = 0;
                    double? totalDiscVal = 0;
                    double? ItemTaxVal = 0;

                    foreach (OrdOfferDF item in OffDF)
                    {
                        InvItemsMF MFItem = db.InvItemsMFs.Where(x => x.ItemNo == item.ItemNo).FirstOrDefault();
                        itemTaxPer = MFItem.STax_Perc;
                        itemDiscVal = item.Qty * item.Price1 * item.PerDiscount / 100;
                        ItemTaxVal = (item.Qty * item.Price1 - itemDiscVal) * (itemTaxPer / 100);

                        Total = (item.Qty * item.Price1) - ((item.Qty * item.Price1) * item.PerDiscount) / 100;
                        totalTaxVal += Total * (itemTaxPer / 100);
                        TotalAmnt += Total;
                        totalDiscVal += itemDiscVal;

                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = item.CompNo;
                        DrTmp["OrderNo"] = item.OrderNo;
                        DrTmp["OrdYear"] = item.OrdYear;
                        DrTmp["TawreedNo"] = item.OrderNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["PUnit"] = item.PUnit;
                        DrTmp["PerDiscount"] = item.PerDiscount;
                        DrTmp["Bonus"] = item.Bonus;
                        DrTmp["Qty"] = item.Qty;
                        DrTmp["Qty2"] = item.Qty2;
                        DrTmp["Price"] = item.Price1;
                        DrTmp["ItemTaxPer"] = itemTaxPer;
                        DrTmp["ItemTaxVal"] = ItemTaxVal;
                        DrTmp["Price2"] = 0;
                        DrTmp["Price3"] = 0;
                        DrTmp["RefNo"] = 0;
                        DrTmp["Paper"] = false;
                        DrTmp["ordamt"] = Total;
                        DrTmp["Note"] = "";
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);

                    }

                    Per_Tax = (totalTaxVal / TotalAmnt) * 100;
                    TotalAmnt = TotalAmnt + totalDiscVal;
                    TotalNetAmount = (TotalAmnt + totalTaxVal) - (totalDiscVal);

                    PayCodes ShipMethod = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 sys_minor as sys_minor ,sys_adesc as DescAr,isnull(sys_edesc,'No Desc.') as DescEn from SysCodes where (CompNo = '{0}') AND sys_major='0f'", company.comp_num)).FirstOrDefault();

                    PayCodes DlivPlace = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 PlaceCode as Id ,Place as DescAr from OrdDlivPlace where (CompNo = '{0}')", company.comp_num)).FirstOrDefault();

                    PayCodes Source = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 sys_minor as sys_minor ,sys_adesc as DescAr,isnull(sys_edesc,'No Desc.') as DescEn from SysCodes where (CompNo = '{0}') AND sys_major='91k'", company.comp_num)).FirstOrDefault();



                    using (SqlCommand CmdHead = new SqlCommand())
                    {
                        CmdHead.Connection = cn1;
                        CmdHead.CommandText = "Ord_AddOrderHF";
                        CmdHead.CommandType = CommandType.StoredProcedure;
                        CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = OffHF.CompNo;
                        CmdHead.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OffHF.OrdYear;
                        CmdHead.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OffHF.OrderNo;
                        CmdHead.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = OffHF.OrderNo;
                        CmdHead.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OffHF.OfferNo;
                        CmdHead.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = OffHF.VendorNo;
                        CmdHead.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                        CmdHead.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                        CmdHead.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                        //if (Vend.Curr == null || Vend.Curr == 0)
                        //{
                        //    PayCodes Currency = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType")).FirstOrDefault();
                        //    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = Currency.Id;

                        //}
                        //else
                        //{
                        //    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = Vend.Curr;
                        //}

                        CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = OrdCopy.Select(x => x.Curr).FirstOrDefault();
                        //if (Vend.Pmethod == null || Vend.Pmethod == 0)
                        //{
                        //    BusunisUnit PaymentM = new MDB().Database.SqlQuery<BusunisUnit>(string.Format("select top 1 MCode as Id ,[Desc] as DescAr,isnull([EngDesc],'No Desc.') as DescEn from PayMethods where (Comp = '{0}')", company.comp_num)).FirstOrDefault();
                        //    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = PaymentM.Id;

                        //}
                        //else
                        //{
                        //    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = Vend.Pmethod;
                        //}

                        CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = OrdCopy.Select(x => x.Pmethod).FirstOrDefault();

                        CmdHead.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = ShipMethod.sys_minor;
                        CmdHead.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = 0;
                        //CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = DlivPlace.Id;
                        CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = OrdCopy.Select(x => x.DeliveryPlace).FirstOrDefault();
                        CmdHead.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "";
                        CmdHead.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = "";
                        CmdHead.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = 0;
                        CmdHead.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = OffHF.MainPeriod;

                        CmdHead.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = totalTaxVal;
                        CmdHead.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = Per_Tax;
                        CmdHead.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = TotalAmnt;
                        CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = TotalNetAmount;
                        CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = false;
                        CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = 0;
                        CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "Each";
                        CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = Vend.Resp_Person;
                        CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = gLcDept;
                        CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = gLcAcc;

                        CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = Source.sys_minor;

                        CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = "";
                        CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                        CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = false;
                        CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = Dept;
                        CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                        CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = 0;
                        CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                        CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = "";
                        CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                        CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                        CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                        CmdHead.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = 0;
                        CmdHead.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = 0;
                        CmdHead.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.NVarChar)).Value = OrdHF.RefNo;

                        CmdHead.Transaction = transaction1;
                        try
                        {
                            CmdHead.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            transaction1.Rollback();
                            cn1.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }

                        var LogID = Convert.ToString(CmdHead.Parameters["@LogID"].Value);

                        using (SqlCommand CmdIns = new SqlCommand())
                        {
                            CmdIns.Connection = cn1;
                            CmdIns.CommandText = "Ord_AddOrderDF";
                            CmdIns.CommandType = CommandType.StoredProcedure;
                            CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
                            CmdIns.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
                            CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
                            CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                            CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                            CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
                            CmdIns.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 9, "ItemTaxPer"));
                            CmdIns.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 9, "ItemTaxVal"));
                            CmdIns.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                            CmdIns.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 9, "ordamt"));
                            CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
                            CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                            CmdIns.Transaction = transaction1;
                            DaPR.InsertCommand = CmdIns;
                            try
                            {
                                DaPR.Update(ds, "TmpTbl");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                transaction1.Rollback();
                                cn1.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        DrTmp1 = ds1.Tables["TblConds"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = OffHF.CompNo;
                        DrTmp1["OrderNo"] = OffHF.OrderNo;
                        DrTmp1["OrdYear"] = OffHF.OrdYear;
                        DrTmp1["TwareedNo"] = OffHF.OrderNo;
                        DrTmp1["PayCond1"] = 1;
                        DrTmp1["PayCond2"] = 0;
                        DrTmp1["PayCond3"] = 0;
                        DrTmp1["PayCond4"] = 0;
                        DrTmp1["PayCond5"] = 0;
                        DrTmp1["CondPerc1"] = 100;
                        DrTmp1["CondPerc2"] = 0;
                        DrTmp1["CondPerc3"] = 0;
                        DrTmp1["CondPerc4"] = 0;
                        DrTmp1["CondPerc5"] = 0;
                        DrTmp1["Guaranty1"] = 0;
                        DrTmp1["Guaranty2"] = 0;
                        DrTmp1["Guaranty3"] = 0;
                        DrTmp1["Guaranty4"] = 0;
                        DrTmp1["Guaranty5"] = 0;
                        DrTmp1["GuarantyPerc1"] = 0;
                        DrTmp1["GuarantyPerc2"] = 0;
                        DrTmp1["GuarantyPerc3"] = 0;
                        DrTmp1["GuarantyPerc4"] = 0;
                        DrTmp1["GuarantyPerc5"] = 0;
                        DrTmp1.EndEdit();
                        ds1.Tables["TblConds"].Rows.Add(DrTmp1);

                        using (SqlCommand CmdInsConds = new SqlCommand())
                        {
                            CmdInsConds.Connection = cn1;
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
                            CmdInsConds.Transaction = transaction1;
                            DaPR1.InsertCommand = CmdInsConds;

                            try
                            {
                                DaPR1.Update(ds1, "TblConds");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                transaction1.Rollback();
                                cn1.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }

                    bool InsertWorkFlow = AddWorkFlowLog(3, Convert.ToInt32(OrdHF.AuthBU), OffHF.OrdYear, OffHF.OrderNo.ToString(), OffHF.OrderNo.ToString(), 1, TotalNetAmount, transaction1, cn1); ;
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        transaction1.Rollback();
                        cn1.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    transaction1.Commit();
                    cn1.Dispose();
                }

                List<Ord_RequestDF> OrdReqDF = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.RequestforQuotationYear == ReqforQuotyear && x.RequestforQuotationNo == ReqforQuotNo).ToList();

                OrdHF.ReqStatus = 4;
                db.SaveChanges();

                foreach (Ord_RequestDF item in OrdReqDF)
                {
                    int year = DateTime.Now.Year;
                    Ord_RequestDF ex = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.SubItemNo == item.SubItemNo && x.ItemSr == item.ItemSr).FirstOrDefault();
                    ex.PurchaseOrderYear = Convert.ToInt16(year);
                    ex.PurchaseOrderNo = OrdNo;
                    ex.PurchaseOrdTawreedNo = OrdNo.ToString();
                    ex.PurchaseOrdOfferNo = OrdNo;
                    ex.bPurchaseOrder = true;
                    db.SaveChanges();
                }
            }
            return Json(new { ReqforQuotNo = ReqforQuotNo, OrdNo = OrdNo, OrdYear = DateTime.Now.Year, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult ApprovalBestOffer(List<MRP_Web_OrdCopyInfo> OrdCopyInfo)
        //{
        //    MRP_Web_OrdCopyInfo ord = OrdCopyInfo.FirstOrDefault();
        //    Ord_RequestDF Orddf = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.RequestforQuotationYear == ord.ReqforQuotyear && x.RequestforQuotationNo == ord.ReqforQuotNo).FirstOrDefault();
        //    Ord_RequestHF OrdHF = db.Ord_RequestHF.Where(x => x.CompNo == Orddf.CompNo && x.ReqYear == Orddf.ReqYear && x.ReqNo == Orddf.ReqNo).FirstOrDefault();
        //    List<OrdCopyInfo> copy = new List<OrdCopyInfo>();

        //    List<MRP_Web_OrdCopyInfo> lord = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == ord.CompNo && x.ReqforQuotyear == ord.ReqforQuotyear
        //     && x.ReqforQuotNo == ord.ReqforQuotNo).ToList();

        //    foreach (MRP_Web_OrdCopyInfo item in lord)
        //    {
        //        MRP_Web_OrdCopyInfo ex = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == item.CompNo && x.ReqforQuotyear == item.ReqforQuotyear
        //         && x.ReqforQuotNo == item.ReqforQuotNo && x.VendorNo == item.VendorNo && x.ItemNo == item.ItemNo).FirstOrDefault();

        //        ex.bPriceOffersApproval = true;

        //        if (OrdCopyInfo.Where(x => x.CompNo == ex.CompNo && x.ReqforQuotyear == ex.ReqforQuotyear
        //        && x.ReqforQuotNo == ex.ReqforQuotNo && x.VendorNo == ex.VendorNo && x.ItemNo == ex.ItemNo).FirstOrDefault() != null)
        //        {
        //            ex.bBestOffer = true;
        //        }
        //        db.SaveChanges();
        //    }

        //    var VenoorNo = OrdCopyInfo.GroupBy(x => x.VendorNo).Select(o => o.Key).ToList();
        //    foreach (var VendNo in VenoorNo)
        //    {
        //        int OrdNo = 0;
        //        using (var cn = new SqlConnection(ConnectionString()))
        //        {
        //            SqlTransaction transaction;
        //            cn.Open();

        //            DataSet ds = new DataSet();
        //            DataSet ds1 = new DataSet();
        //            DataSet ds2 = new DataSet();

        //            SqlDataAdapter DaPR = new SqlDataAdapter();
        //            SqlDataAdapter DaPR1 = new SqlDataAdapter();
        //            SqlDataAdapter DaPR2 = new SqlDataAdapter();

        //            DataRow DrTmp;
        //            DataRow DrTmp1;
        //            DataRow DrTmp2;

        //            DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE compno = 0", cn);
        //            DaPR.Fill(ds, "POrderDF");

        //            DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdVOfferCondsGuarnty  WHERE compno = 0", cn);
        //            DaPR1.Fill(ds1, "TblConds");

        //            DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdOfferDF  WHERE compno = 0", cn);
        //            DaPR2.Fill(ds2, "OrdOfferDF");

        //            transaction = cn.BeginTransaction();

        //            long LogID = 0;


        //            int i = 1;
        //            int gLcDept = 0;
        //            long gLcAcc = 0;

        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.Connection = cn;
        //                cmd.CommandText = "Ord_AppPreOrderHF";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
        //                cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
        //                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
        //                cmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmd.Parameters.Add(new SqlParameter("@OperationDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmd.Parameters.Add(new SqlParameter("@OperationType", SqlDbType.SmallInt)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@OrderPurpose", SqlDbType.Int)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.Int)).Value = 0;
        //                cmd.Parameters.Add(new SqlParameter("@OrdSource", SqlDbType.Int)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@OrderType", SqlDbType.Int)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@OrderCateg", SqlDbType.SmallInt)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
        //                cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = 0;
        //                cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = 0;
        //                cmd.Parameters.Add(new SqlParameter("@BuyerNo", SqlDbType.Int)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@OrdOrg", SqlDbType.Int)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@SiteDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmd.Parameters.Add(new SqlParameter("@Confirmation", SqlDbType.Bit)).Value = true;
        //                cmd.Parameters.Add(new SqlParameter("@Approval", SqlDbType.Bit)).Value = false;
        //                cmd.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
        //                cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = "إنشاء طلب شراء من الويب";
        //                cmd.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = 0;
        //                cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(new SqlParameter("@BU", SqlDbType.SmallInt)).Value = OrdHF.AuthBU;
        //                cmd.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
        //                cmd.Parameters.Add(new SqlParameter("@AgreeYear", SqlDbType.Int)).Value = DateTime.Now.Year;
        //                cmd.Parameters.Add(new SqlParameter("@AgreeNo", SqlDbType.BigInt)).Value = 0;

        //                cmd.Transaction = transaction;
        //                try
        //                {
        //                    cmd.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }

        //                if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) != 0)
        //                {
        //                    if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -12)
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        return Json(new { error = Resources.Resource.errorPurchOrderSerial }, JsonRequestBehavior.AllowGet);
        //                    }
        //                    else
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                    }
        //                }

        //                LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);
        //                OrdNo = Convert.ToInt32(cmd.Parameters["@UsedOrdNo"].Value);
        //            }


        //            foreach (MRP_Web_OrdCopyInfo items in OrdCopyInfo.Where(x => x.VendorNo == VendNo))
        //            {

        //                MRP_Web_OrdCopyInfo OrdCopy = db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == items.CompNo && x.ReqforQuotyear == items.ReqforQuotyear
        //         && x.ReqforQuotNo == items.ReqforQuotNo && x.VendorNo == items.VendorNo && x.ItemNo == items.ItemNo).FirstOrDefault();

        //                double? Qty = 0;
        //                decimal? SellPrice = 0;
        //                double Total = 0;

        //                Qty = OrdCopy.Qty.Value;
        //                SellPrice = OrdCopy.SellPrice.Value;
        //                Total = Convert.ToDouble(SellPrice) * OrdCopy.Qty.Value;

        //                DrTmp = ds.Tables["POrderDF"].NewRow();
        //                DrTmp.BeginEdit();
        //                DrTmp["CompNo"] = company.comp_num;
        //                DrTmp["OrdYear"] = DateTime.Now.Year;
        //                DrTmp["OrderNo"] = OrdNo;
        //                DrTmp["ItemNo"] = OrdCopy.ItemNo;
        //                DrTmp["NSItemNo"] = 0;
        //                DrTmp["Price"] = SellPrice;
        //                DrTmp["ExtraPrice"] = 0;
        //                DrTmp["Qty"] = Qty;
        //                DrTmp["Bonus"] = 0;
        //                DrTmp["TotalValue"] = Total;
        //                DrTmp["Confirmation"] = 1;
        //                DrTmp["ConfirmQty"] = Qty;
        //                DrTmp["Note"] = "";
        //                DrTmp["DiscPer"] = 0;
        //                DrTmp["ValueAfterDisc"] = Qty;
        //                DrTmp["DiscAmt"] = 0;
        //                DrTmp["UnitSerial"] = 1;
        //                DrTmp["ItemSr"] = i;

        //                DrTmp.EndEdit();
        //                ds.Tables["POrderDF"].Rows.Add(DrTmp);

        //                InvItemsMF mf = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == OrdCopy.ItemNo).FirstOrDefault();
        //                DrTmp1 = ds2.Tables["OrdOfferDF"].NewRow();
        //                DrTmp1.BeginEdit();
        //                DrTmp1["CompNo"] = company.comp_num;
        //                DrTmp1["OrdYear"] = DateTime.Now.Year;
        //                DrTmp1["OrderNo"] = OrdNo;
        //                DrTmp1["OfferNo"] = OrdNo;
        //                DrTmp1["VendorNo"] = VendNo;
        //                DrTmp1["ItemNo"] = OrdCopy.ItemNo;
        //                DrTmp1["PerDiscount"] = 0;
        //                DrTmp1["Bonus"] = 0;
        //                DrTmp1["Qty"] = OrdCopy.Qty.Value;
        //                DrTmp1["Price1"] = OrdCopy.SellPrice.Value;
        //                DrTmp1["Price2"] = 0;
        //                DrTmp1["Price3"] = 0;
        //                DrTmp1["ItemSourceCode"] = 0;
        //                DrTmp1["Paper"] = false;
        //                DrTmp1["Free_Sample"] = false;
        //                DrTmp1["PUnit"] = mf.UnitC4;

        //                DrTmp1.EndEdit();
        //                ds2.Tables["OrdOfferDF"].Rows.Add(DrTmp1);
        //            }

        //            using (SqlCommand cmd1 = new SqlCommand())
        //            {
        //                cmd1.Connection = cn;
        //                cmd1.CommandText = "Ord_AppPreOrderDF";
        //                cmd1.CommandType = CommandType.StoredProcedure;
        //                cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
        //                cmd1.Parameters.Add(new SqlParameter("@PYear", SqlDbType.SmallInt, 4, "OrdYear"));
        //                cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
        //                cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
        //                cmd1.Parameters.Add(new SqlParameter("@NSItem", SqlDbType.VarChar, 20, "NSItemNo"));
        //                cmd1.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 16, "Price"));
        //                cmd1.Parameters.Add(new SqlParameter("@ExtraPrice", SqlDbType.Float, 8, "ExtraPrice"));
        //                cmd1.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
        //                cmd1.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
        //                cmd1.Parameters.Add(new SqlParameter("@TotalValue", SqlDbType.Float, 8, "TotalValue"));
        //                cmd1.Parameters.Add(new SqlParameter("@Confirm", SqlDbType.Bit, 1, "Confirmation"));
        //                cmd1.Parameters.Add(new SqlParameter("@ConfirmQty", SqlDbType.Float, 8, "ConfirmQty"));
        //                cmd1.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
        //                cmd1.Parameters.Add(new SqlParameter("@DiscPer", SqlDbType.Money, 8, "DiscPer"));
        //                cmd1.Parameters.Add(new SqlParameter("@ValueAfterDisc", SqlDbType.Float, 16, "ValueAfterDisc"));
        //                cmd1.Parameters.Add(new SqlParameter("@DiscAmt", SqlDbType.Float, 16, "DiscAmt"));
        //                cmd1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
        //                cmd1.Parameters.Add(new SqlParameter("@ItemSr", SqlDbType.SmallInt, 4, "ItemSr"));
        //                cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
        //                cmd1.Transaction = transaction;
        //                DaPR.InsertCommand = cmd1;
        //                try
        //                {
        //                    DaPR.Update(ds, "POrderDF");
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }

        //            using (SqlCommand cmdOrdCopy = new SqlCommand())
        //            {
        //                cmdOrdCopy.Connection = cn;
        //                cmdOrdCopy.CommandText = "Ord_AddOrdCopyInfo";
        //                cmdOrdCopy.CommandType = CommandType.StoredProcedure;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@CYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = VendNo;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@ReceiptNo", SqlDbType.Int)).Value = OrdNo;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@SellDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "4";
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@Country", SqlDbType.VarChar)).Value = "Jordan";
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@PayCond", SqlDbType.Int)).Value = 1;
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@RecCond", SqlDbType.VarChar)).Value = "";
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@Accept", SqlDbType.VarChar)).Value = "";
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء طلب عرض سعر من الويب";
        //                cmdOrdCopy.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
        //                cmdOrdCopy.Transaction = transaction;
        //                try
        //                {
        //                    cmdOrdCopy.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }

        //                if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) != 0)
        //                {
        //                    if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) == 2627)
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        if (Language == "en")
        //                        {
        //                            return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                        else
        //                        {
        //                            return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                    else if (Convert.ToInt32(cmdOrdCopy.Parameters["@ErrNo"].Value) == 2627)
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        if (Language == "en")
        //                        {
        //                            return Json(new { error = "P.O No Dosent Have a Bid ,Operation Failed" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                        else
        //                        {
        //                            return Json(new { error = "لم يتم تعريف عطاء لطلب الشراء المدخل لم تنجح العملية" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }

        //            using (SqlCommand cmdOrdOfferHF = new SqlCommand())
        //            {
        //                Vendor v = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == VendNo).FirstOrDefault();
        //                string VendorOfferNo = OrdNo.ToString() + " - " + "1";
        //                cmdOrdOfferHF.Connection = cn;
        //                cmdOrdOfferHF.CommandText = "Ord_AddOrdOfferHF";
        //                cmdOrdOfferHF.CommandType = CommandType.StoredProcedure;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = DateTime.Now.Date.Year;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrdNo;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OrdNo;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = VendNo;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ValidateDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.Date;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@OfferCurr", SqlDbType.Int)).Value = v.Curr;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = v.Pmethod;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ShipCenter", SqlDbType.Int)).Value = 1;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@DelivDay", SqlDbType.Int)).Value = 0;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = 0;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@VendorOfferNo", SqlDbType.VarChar)).Value = VendorOfferNo;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@LcType", SqlDbType.SmallInt)).Value = 1;
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "ادخال عروض الموردين من الويب";
        //                cmdOrdOfferHF.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
        //                cmdOrdOfferHF.Transaction = transaction;
        //                try
        //                {
        //                    cmdOrdOfferHF.ExecuteNonQuery();
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }



        //                using (SqlCommand cmdOrdOfferDF = new SqlCommand())
        //                {
        //                    cmdOrdOfferDF.Connection = cn;
        //                    cmdOrdOfferDF.CommandText = "Ord_AddOrdOfferDF";
        //                    cmdOrdOfferDF.CommandType = CommandType.StoredProcedure;
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 8, "VendorNo"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int, 8, "OfferNo"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 16, "PerDiscount"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 8, "Bonus"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 8, "Qty"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price1", SqlDbType.Float, 8, "Price1"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price2", SqlDbType.Float, 8, "Price2"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Price3", SqlDbType.Float, 8, "Price3"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ItemSourceCode", SqlDbType.VarChar, 20, "ItemSourceCode"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Paper", SqlDbType.Bit, 1, "Paper"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@Free_Sample", SqlDbType.Bit, 1, "Free_Sample"));
        //                    cmdOrdOfferDF.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output; ;
        //                    cmdOrdOfferDF.Transaction = transaction;
        //                    DaPR2.InsertCommand = cmdOrdOfferDF;
        //                    try
        //                    {
        //                        DaPR2.Update(ds2, "OrdOfferDF");
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                    }
        //                }
        //            }

        //            DrTmp2 = ds1.Tables["TblConds"].NewRow();
        //            DrTmp2.BeginEdit();
        //            DrTmp2["CompNo"] = company.comp_num;
        //            DrTmp2["OrdYear"] = DateTime.Now.Year;
        //            DrTmp2["OrderNo"] = OrdNo;
        //            DrTmp2["OfferNo"] = OrdNo;
        //            DrTmp2["VendorNo"] = VendNo;
        //            DrTmp2["PayCond1"] = 1;
        //            DrTmp2["PayCond2"] = 0;
        //            DrTmp2["PayCond3"] = 0;
        //            DrTmp2["PayCond4"] = 0;
        //            DrTmp2["PayCond5"] = 0;
        //            DrTmp2["CondPerc1"] = 100;
        //            DrTmp2["CondPerc2"] = 0;
        //            DrTmp2["CondPerc3"] = 0;
        //            DrTmp2["CondPerc4"] = 0;
        //            DrTmp2["CondPerc5"] = 0;
        //            DrTmp2["Guaranty1"] = 0;
        //            DrTmp2["Guaranty2"] = 0;
        //            DrTmp2["Guaranty3"] = 0;
        //            DrTmp2["Guaranty4"] = 0;
        //            DrTmp2["Guaranty5"] = 0;
        //            DrTmp2["GuarantyPerc1"] = 0;
        //            DrTmp2["GuarantyPerc2"] = 0;
        //            DrTmp2["GuarantyPerc3"] = 0;
        //            DrTmp2["GuarantyPerc4"] = 0;
        //            DrTmp2["GuarantyPerc5"] = 0;
        //            DrTmp2.EndEdit();
        //            ds1.Tables["TblConds"].Rows.Add(DrTmp2);

        //            using (SqlCommand CmdInsConds = new SqlCommand())
        //            {
        //                CmdInsConds.Connection = cn;
        //                CmdInsConds.CommandText = "Ord_AddVOfferCondsGuarnty";
        //                CmdInsConds.CommandType = CommandType.StoredProcedure;
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int, 8, "OfferNo"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 8, "VendorNo"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Cond1", SqlDbType.Int, 8, "PayCond1"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Cond2", SqlDbType.Int, 8, "PayCond2"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Cond3", SqlDbType.Int, 8, "PayCond3"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Cond4", SqlDbType.Int, 8, "PayCond4"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Cond5", SqlDbType.Int, 8, "PayCond5"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc1", SqlDbType.Float, 9, "CondPerc1"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc2", SqlDbType.Float, 9, "CondPerc2"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc3", SqlDbType.Float, 9, "CondPerc3"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc4", SqlDbType.Float, 9, "CondPerc4"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc5", SqlDbType.Float, 9, "CondPerc5"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty1", SqlDbType.Int, 8, "Guaranty1"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty2", SqlDbType.Int, 8, "Guaranty2"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty3", SqlDbType.Int, 8, "Guaranty3"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty4", SqlDbType.Int, 8, "Guaranty4"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty5", SqlDbType.Int, 8, "Guaranty5"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc1", SqlDbType.Float, 9, "GuarantyPerc1"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc2", SqlDbType.Float, 9, "GuarantyPerc2"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc3", SqlDbType.Float, 9, "GuarantyPerc3"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc4", SqlDbType.Float, 9, "GuarantyPerc4"));
        //                CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc5", SqlDbType.Float, 9, "GuarantyPerc5"));
        //                CmdInsConds.Transaction = transaction;
        //                DaPR1.InsertCommand = CmdInsConds;

        //                try
        //                {
        //                    DaPR1.Update(ds1, "TblConds");
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }

        //            transaction.Commit();
        //            cn.Dispose();

        //            using (var cn1 = new SqlConnection(ConnectionString()))
        //            {
        //                SqlTransaction transaction1;
        //                cn1.Open();

        //                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn1);
        //                DaPR.Fill(ds, "TmpTbl");

        //                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdCondsGuaranty  WHERE compno = 0", cn1);
        //                DaPR1.Fill(ds1, "TblConds");

        //                transaction1 = cn1.BeginTransaction();

        //                OrdOfferHF OffHF = db.OrdOfferHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).FirstOrDefault();

        //                using (SqlCommand Cmd = new SqlCommand())
        //                {
        //                    Cmd.Connection = cn1;
        //                    Cmd.CommandText = "Ord_ChkOrderHF";
        //                    Cmd.CommandType = CommandType.StoredProcedure;
        //                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
        //                    Cmd.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OffHF.OrdYear;
        //                    Cmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OffHF.OrderNo;
        //                    Cmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OffHF.OrderNo;
        //                    Cmd.Transaction = transaction1;
        //                    SqlDataReader rdr = Cmd.ExecuteReader();
        //                    if (rdr.HasRows)
        //                    {
        //                        while (rdr.Read())
        //                        {
        //                            cn.Dispose();
        //                            cn1.Dispose();
        //                            if (Language == "en")
        //                            {
        //                                return Json(new { error = "P.O Is Referred To Direct Purchase On Tawreed NO. " + OffHF.OrderNo + "  " + "Add Operation Failed" }, JsonRequestBehavior.AllowGet);
        //                            }
        //                            else
        //                            {
        //                                return Json(new { error = "طلب الشراء محال للشراء المباشر على رقم توريد " + OffHF.OrderNo + "  " + "لم تنجح عملية الاضافة" }, JsonRequestBehavior.AllowGet);
        //                            }
        //                        }
        //                    }

        //                    rdr.Close();
        //                }

        //                using (SqlCommand Cmd = new SqlCommand())
        //                {
        //                    Cmd.Connection = cn1;
        //                    Cmd.CommandText = "Ord_GetCompMF";
        //                    Cmd.CommandType = CommandType.StoredProcedure;
        //                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
        //                    Cmd.Transaction = transaction1;
        //                    SqlDataReader rdr = Cmd.ExecuteReader();
        //                    if (rdr.HasRows)
        //                    {
        //                        while (rdr.Read())
        //                        {
        //                            gLcDept = Convert.ToInt32(rdr["LcDept"]);
        //                            gLcAcc = Convert.ToInt64(rdr["LcAcc"]);
        //                        }
        //                    }
        //                    rdr.Close();
        //                }
        //                Vendor Vend = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == OffHF.VendorNo).FirstOrDefault();
        //                int Dept = new MDB().Database.SqlQuery<int>(string.Format("select TOP 1 crb_dep as VenderId from GLCRBMF where (CRB_COMP = '{0}') AND (crb_acc = '{1}')", company.comp_num, Vend.VendorNo)).FirstOrDefault();

        //                List<OrdOfferDF> OffDF = db.OrdOfferDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).ToList();
        //                double? totalTaxVal = 0;
        //                double? Total = 0;
        //                double? itemTaxPer = 0;
        //                double? Per_Tax = 0;
        //                double? TotalAmnt = 0;
        //                double? TotalNetAmount = 0;
        //                double? itemDiscVal = 0;
        //                double? totalDiscVal = 0;
        //                double? ItemTaxVal = 0;

        //                foreach (OrdOfferDF item in OffDF)
        //                {
        //                    InvItemsMF MFItem = db.InvItemsMFs.Where(x => x.ItemNo == item.ItemNo).FirstOrDefault();
        //                    itemTaxPer = MFItem.STax_Perc;
        //                    itemDiscVal = item.Qty * item.Price1 * item.PerDiscount / 100;
        //                    ItemTaxVal = (item.Qty * item.Price1 - itemDiscVal) * (itemTaxPer / 100);

        //                    Total = (item.Qty * item.Price1) - ((item.Qty * item.Price1) * item.PerDiscount) / 100;
        //                    totalTaxVal += Total * (itemTaxPer / 100);
        //                    TotalAmnt += Total;
        //                    totalDiscVal += itemDiscVal;

        //                    DrTmp = ds.Tables["TmpTbl"].NewRow();
        //                    DrTmp.BeginEdit();
        //                    DrTmp["CompNo"] = item.CompNo;
        //                    DrTmp["OrderNo"] = item.OrderNo;
        //                    DrTmp["OrdYear"] = item.OrdYear;
        //                    DrTmp["TawreedNo"] = item.OrderNo;
        //                    DrTmp["ItemNo"] = item.ItemNo;
        //                    DrTmp["PUnit"] = item.PUnit;
        //                    DrTmp["PerDiscount"] = item.PerDiscount;
        //                    DrTmp["Bonus"] = item.Bonus;
        //                    DrTmp["Qty"] = item.Qty;
        //                    DrTmp["Price"] = item.Price1;
        //                    DrTmp["ItemTaxPer"] = itemTaxPer;
        //                    DrTmp["ItemTaxVal"] = ItemTaxVal;
        //                    DrTmp["Price2"] = 0;
        //                    DrTmp["Price3"] = 0;
        //                    DrTmp["RefNo"] = 0;
        //                    DrTmp["Paper"] = false;
        //                    DrTmp["ordamt"] = Total;
        //                    DrTmp["Note"] = "";
        //                    DrTmp.EndEdit();
        //                    ds.Tables["TmpTbl"].Rows.Add(DrTmp);

        //                }

        //                Per_Tax = (totalTaxVal / TotalAmnt) * 100;
        //                TotalAmnt = TotalAmnt + totalDiscVal;
        //                TotalNetAmount = (TotalAmnt + totalTaxVal) - (totalDiscVal);

        //                using (SqlCommand CmdHead = new SqlCommand())
        //                {
        //                    CmdHead.Connection = cn1;
        //                    CmdHead.CommandText = "Ord_AddOrderHF";
        //                    CmdHead.CommandType = CommandType.StoredProcedure;
        //                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = OffHF.CompNo;
        //                    CmdHead.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OffHF.OrdYear;
        //                    CmdHead.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OffHF.OrderNo;
        //                    CmdHead.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = OffHF.OrderNo;
        //                    CmdHead.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = OffHF.OfferNo;
        //                    CmdHead.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = OffHF.VendorNo;
        //                    CmdHead.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
        //                    CmdHead.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
        //                    CmdHead.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
        //                    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = OffHF.OfferCurr;
        //                    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = OffHF.PayWay;
        //                    CmdHead.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = OffHF.ShipCenter;
        //                    CmdHead.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = 0;
        //                    CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = "";
        //                    CmdHead.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "";
        //                    CmdHead.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = "";
        //                    CmdHead.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = 0;
        //                    CmdHead.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = OffHF.MainPeriod;

        //                    CmdHead.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = totalTaxVal;
        //                    CmdHead.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = Per_Tax;
        //                    CmdHead.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = TotalAmnt;
        //                    CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = TotalNetAmount;
        //                    CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = false;
        //                    CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = 0;
        //                    CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "Each";
        //                    CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = Vend.Resp_Person;
        //                    CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = gLcDept;
        //                    CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = gLcAcc;
        //                    CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = 1;

        //                    CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = "";
        //                    CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
        //                    CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = false;
        //                    CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
        //                    CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = Dept;
        //                    CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
        //                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = 0;
        //                    CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
        //                    CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = "";
        //                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
        //                    CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
        //                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
        //                    CmdHead.Transaction = transaction1;
        //                    try
        //                    {
        //                        CmdHead.ExecuteNonQuery();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        transaction.Rollback();
        //                        cn.Dispose();
        //                        transaction1.Rollback();
        //                        cn1.Dispose();
        //                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                    }

        //                    var LogIDs = Convert.ToString(CmdHead.Parameters["@LogID"].Value);

        //                    using (SqlCommand CmdIns = new SqlCommand())
        //                    {
        //                        CmdIns.Connection = cn1;
        //                        CmdIns.CommandText = "Ord_AddOrderDF";
        //                        CmdIns.CommandType = CommandType.StoredProcedure;
        //                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 9, "ItemTaxPer"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 9, "ItemTaxVal"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 9, "ordamt"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
        //                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogIDs;
        //                        CmdIns.Transaction = transaction1;
        //                        DaPR.InsertCommand = CmdIns;
        //                        try
        //                        {
        //                            DaPR.Update(ds, "TmpTbl");
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            transaction.Rollback();
        //                            cn.Dispose();
        //                            transaction1.Rollback();
        //                            cn1.Dispose();
        //                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }

        //                    DrTmp1 = ds1.Tables["TblConds"].NewRow();
        //                    DrTmp1.BeginEdit();
        //                    DrTmp1["CompNo"] = OffHF.CompNo;
        //                    DrTmp1["OrderNo"] = OffHF.OrderNo;
        //                    DrTmp1["OrdYear"] = OffHF.OrdYear;
        //                    DrTmp1["TwareedNo"] = OffHF.OrderNo;
        //                    DrTmp1["PayCond1"] = 1;
        //                    DrTmp1["PayCond2"] = 0;
        //                    DrTmp1["PayCond3"] = 0;
        //                    DrTmp1["PayCond4"] = 0;
        //                    DrTmp1["PayCond5"] = 0;
        //                    DrTmp1["CondPerc1"] = 100;
        //                    DrTmp1["CondPerc2"] = 0;
        //                    DrTmp1["CondPerc3"] = 0;
        //                    DrTmp1["CondPerc4"] = 0;
        //                    DrTmp1["CondPerc5"] = 0;
        //                    DrTmp1["Guaranty1"] = 0;
        //                    DrTmp1["Guaranty2"] = 0;
        //                    DrTmp1["Guaranty3"] = 0;
        //                    DrTmp1["Guaranty4"] = 0;
        //                    DrTmp1["Guaranty5"] = 0;
        //                    DrTmp1["GuarantyPerc1"] = 0;
        //                    DrTmp1["GuarantyPerc2"] = 0;
        //                    DrTmp1["GuarantyPerc3"] = 0;
        //                    DrTmp1["GuarantyPerc4"] = 0;
        //                    DrTmp1["GuarantyPerc5"] = 0;
        //                    DrTmp1.EndEdit();
        //                    ds1.Tables["TblConds"].Rows.Add(DrTmp1);

        //                    using (SqlCommand CmdInsConds = new SqlCommand())
        //                    {
        //                        CmdInsConds.Connection = cn1;
        //                        CmdInsConds.CommandText = "Ord_AddCondsGuaranty";
        //                        CmdInsConds.CommandType = CommandType.StoredProcedure;
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 4, "OrdYear"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@TwareedNo", SqlDbType.VarChar, 20, "TwareedNo"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Cond1", SqlDbType.Int, 8, "PayCond1"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Cond2", SqlDbType.Int, 8, "PayCond2"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Cond3", SqlDbType.Int, 8, "PayCond3"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Cond4", SqlDbType.Int, 8, "PayCond4"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Cond5", SqlDbType.Int, 8, "PayCond5"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc1", SqlDbType.Float, 9, "CondPerc1"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc2", SqlDbType.Float, 9, "CondPerc2"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc3", SqlDbType.Float, 9, "CondPerc3"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc4", SqlDbType.Float, 9, "CondPerc4"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@CondPerc5", SqlDbType.Float, 9, "CondPerc5"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty1", SqlDbType.Int, 8, "Guaranty1"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty2", SqlDbType.Int, 8, "Guaranty2"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty3", SqlDbType.Int, 8, "Guaranty3"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty4", SqlDbType.Int, 8, "Guaranty4"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@Guaranty5", SqlDbType.Int, 8, "Guaranty5"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc1", SqlDbType.Float, 9, "GuarantyPerc1"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc2", SqlDbType.Float, 9, "GuarantyPerc2"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc3", SqlDbType.Float, 9, "GuarantyPerc3"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc4", SqlDbType.Float, 9, "GuarantyPerc4"));
        //                        CmdInsConds.Parameters.Add(new SqlParameter("@GuarantyPerc5", SqlDbType.Float, 9, "GuarantyPerc5"));
        //                        CmdInsConds.Transaction = transaction1;
        //                        DaPR1.InsertCommand = CmdInsConds;

        //                        try
        //                        {
        //                            DaPR1.Update(ds1, "TblConds");
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            transaction.Rollback();
        //                            cn.Dispose();
        //                            transaction1.Rollback();
        //                            cn1.Dispose();
        //                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }

        //                }

        //                bool InsertWorkFlow = AddWorkFlowLog(3, Convert.ToInt32(OrdHF.AuthBU), OffHF.OrdYear, OffHF.OrderNo.ToString(), OffHF.OrderNo.ToString(), 1, TotalNetAmount, transaction1, cn1); ;
        //                if (InsertWorkFlow == false)
        //                {
        //                    transaction.Rollback();
        //                    cn.Dispose();
        //                    transaction1.Rollback();
        //                    cn1.Dispose();
        //                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
        //                }
        //                transaction1.Commit();
        //                cn1.Dispose();

        //                OrdCopyInfo e = new OrdCopyInfo();
        //                e.ReqforQuotyear = ord.ReqforQuotyear;
        //                e.ReqforQuotNo = ord.ReqforQuotNo;
        //                e.ReqYear = Convert.ToInt16(DateTime.Now.Year);
        //                e.ReqNo = Convert.ToString(OrdNo);
        //                copy.Add(e);
        //            }
        //        }
        //    }
        //    return Json(new { copy = copy, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult AddPurchaseOrderInDirect(int OrdYear, int OrdNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrdNo;
            return View();
        }
        public JsonResult CreatePurchaseOrderInDirect(int BU, OrderHF OrdHF, List<OrderDF> OrdDF, OrdCondsGuaranty OrdCondsGuaranty)
        {
            int i = 1;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            DataRow DrTmp;
            DataRow DrTmp1;

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
                    CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = OrdHF.NetAmt;
                    if (OrdHF.OrderClose == null)
                    {
                        OrdHF.OrderClose = false;
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = OrdHF.OrderClose;
                    CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = OrdHF.DlvState;
                    CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = OrdHF.UnitKind;
                    CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = OrdHF.PInCharge;
                    CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = gLcDept;
                    CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = gLcAcc;
                    CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = OrdHF.SourceType;
                    if (OrdHF.GenNote == null)
                    {
                        OrdHF.GenNote = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = OrdHF.GenNote;
                    CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                    CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = OrdHF.Approved;
                    CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = OrdHF.Vend_Dept;
                    CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = OrdHF.ExpShDate;
                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = OrdHF.IsLC;
                    CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = OrdHF.ETA;
                    CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = OrdHF.Port;
                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 2;
                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    CmdHead.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = OrdHF.FreightExpense;
                    CmdHead.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = OrdHF.ExtraExpenses;
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
                        DrTmp["OrderNo"] = item.OrderNo;
                        DrTmp["OrdYear"] = item.OrdYear;
                        DrTmp["TawreedNo"] = item.TawreedNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["PUnit"] = item.PUnit;
                        DrTmp["PerDiscount"] = item.PerDiscount;
                        DrTmp["Bonus"] = item.Bonus;
                        DrTmp["Qty"] = item.Qty;
                        DrTmp["Qty2"] = item.Qty2;
                        DrTmp["Price"] = item.Price;
                        DrTmp["ItemTaxPer"] = item.ItemTaxPer;
                        DrTmp["ItemTaxVal"] = item.ItemTaxVal;
                        DrTmp["Price2"] = 0;
                        DrTmp["Price3"] = 0;
                        DrTmp["RefNo"] = item.RefNo;
                        DrTmp["Paper"] = item.Paper;
                        DrTmp["ordamt"] = item.ordamt;
                        DrTmp["Note"] = item.Note;
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
                        CmdIns.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar, 20, "TawreedNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float, 9, "PerDiscount"));
                        CmdIns.Parameters.Add(new SqlParameter("@Bonus", SqlDbType.Float, 9, "Bonus"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                        CmdIns.Parameters.Add(new SqlParameter("@Price", SqlDbType.Float, 9, "Price"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxPer", SqlDbType.Float, 9, "ItemTaxPer"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemTaxVal", SqlDbType.Float, 9, "ItemTaxVal"));
                        CmdIns.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.VarChar, 20, "RefNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@PUnit", SqlDbType.VarChar, 5, "PUnit"));
                        CmdIns.Parameters.Add(new SqlParameter("@ordamt", SqlDbType.Float, 9, "ordamt"));
                        CmdIns.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar, 200, "Note"));
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
            return Json(new {  OrdNo = OrdHF.OrderNo, OrdYear = OrdHF.OrdYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);

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
    }
}