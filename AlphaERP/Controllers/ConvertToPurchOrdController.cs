using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ConvertToPurchOrdController : controller
    {
        int OrdNo = 0;
        long LogID = 0;
        int gLcDept = 0;
        long gLcAcc = 0;
        double? TotalNetAmount = 0;

        // GET: ConvertToPurchOrd
        public ActionResult Index()
        {
            Session["Files"] = null;
            return View();
        }
        public ActionResult GetPurchOrd(int PurchOrd)
        {
            OrdCompMF ordCompMF = new MDB().OrdCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            ClientsActive CActive = new MDB().ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            List<OrdRequestDF> OrdReqtDF = new List<OrdRequestDF>();
            if (PurchOrd == 0)
            {
                if (CActive.ClientNo == 239)
                {
                    if (me.UserID == "Saif.sc")
                    {
                        OrdReqtDF = new MDB().Database.SqlQuery<OrdRequestDF>(string.Format("SELECT Ord_RequestDF.CompNo as CompNo,Ord_RequestDF.ReqYear as ReqYear ,Ord_RequestDF.ReqNo as ReqNo,Ord_RequestDF.ItemSr as ItemSr,Ord_RequestDF.SubItemNo as SubItemNo,Ord_RequestDF.SubTUnit  as SubTUnit,  " +
                        "Ord_RequestDF.SubUnitSerial  as SubUnitSerial,Ord_RequestDF.Qty as Qty,case when Ord_RequestDF.Qty2 IS NULL then 0 else Ord_RequestDF.Qty2 end as Qty2,Ord_RequestDF.Price as Price,Ord_RequestDF.TotalValue as TotalValue,isnull(InvItemsMF.prefvendor,0) as prefvendor,Ord_RequestDF.ReqDeliveryDate as ReqDeliveryDate,Ord_RequestDF.Note as Note " +
                        "from Ord_RequestDF INNER JOIN  Ord_RequestHF ON Ord_RequestDF.CompNo = Ord_RequestHF.CompNo AND Ord_RequestDF.ReqYear = Ord_RequestHF.ReqYear AND Ord_RequestDF.ReqNo = Ord_RequestHF.ReqNo Inner Join " +
                        "InvItemsMF on Ord_RequestDF.CompNo = InvItemsMF.CompNo AND Ord_RequestDF.SubItemNo = InvItemsMF.ItemNo " +
                        "where Ord_RequestDF.CompNo = '{0}' AND bPurchaseOrder = 0 AND (bRequestforQuotation = 0) AND Ord_RequestHF.ReqStatus In(1,4,7) AND (isnull(Ord_RequestDF.IsReject,0) = 0)    Order by InvItemsMF.prefvendor ", company.comp_num)).ToList();
                    }
                    else
                    {
                        if (ordCompMF.LinkReqOrdByUser == false)
                        {
                            OrdReqtDF = new MDB().Database.SqlQuery<OrdRequestDF>(string.Format("SELECT Ord_RequestDF.CompNo as CompNo,Ord_RequestDF.ReqYear as ReqYear ,Ord_RequestDF.ReqNo as ReqNo,Ord_RequestDF.ItemSr as ItemSr,Ord_RequestDF.SubItemNo as SubItemNo,Ord_RequestDF.SubTUnit  as SubTUnit,  " +
                        "Ord_RequestDF.SubUnitSerial  as SubUnitSerial,Ord_RequestDF.Qty as Qty,case when Ord_RequestDF.Qty2 IS NULL then 0 else Ord_RequestDF.Qty2 end as Qty2,Ord_RequestDF.Price as Price,Ord_RequestDF.TotalValue as TotalValue,isnull(InvItemsMF.prefvendor,0) as prefvendor,Ord_RequestDF.ReqDeliveryDate as ReqDeliveryDate,Ord_RequestDF.Note as Note " +
                        "from Ord_RequestDF INNER JOIN  Ord_RequestHF ON Ord_RequestDF.CompNo = Ord_RequestHF.CompNo AND Ord_RequestDF.ReqYear = Ord_RequestHF.ReqYear AND Ord_RequestDF.ReqNo = Ord_RequestHF.ReqNo Inner Join " +
                        "InvItemsMF on Ord_RequestDF.CompNo = InvItemsMF.CompNo AND Ord_RequestDF.SubItemNo = InvItemsMF.ItemNo " +
                        "where Ord_RequestDF.CompNo = '{0}' AND bPurchaseOrder = 0 AND (bRequestforQuotation = 0) AND Ord_RequestHF.ReqStatus In(1,4,7) AND (isnull(Ord_RequestDF.IsReject,0) = 0)    Order by InvItemsMF.prefvendor ", company.comp_num)).ToList();

                        }
                        else
                        {
                            OrdReqtDF = new MDB().Database.SqlQuery<OrdRequestDF>(string.Format("SELECT Ord_RequestDF.CompNo as CompNo,Ord_RequestDF.ReqYear as ReqYear ,Ord_RequestDF.ReqNo as ReqNo,Ord_RequestDF.ItemSr as ItemSr,Ord_RequestDF.SubItemNo as SubItemNo,Ord_RequestDF.SubTUnit  as SubTUnit,  " +
                       "Ord_RequestDF.SubUnitSerial  as SubUnitSerial,Ord_RequestDF.Qty as Qty,case when Ord_RequestDF.Qty2 IS NULL then 0 else Ord_RequestDF.Qty2 end as Qty2,Ord_RequestDF.Price as Price,Ord_RequestDF.TotalValue as TotalValue,isnull(InvItemsMF.prefvendor,0) as prefvendor,Ord_RequestDF.ReqDeliveryDate as ReqDeliveryDate,Ord_RequestDF.Note as Note " +
                       "from Ord_RequestDF INNER JOIN  Ord_RequestHF ON Ord_RequestDF.CompNo = Ord_RequestHF.CompNo AND Ord_RequestDF.ReqYear = Ord_RequestHF.ReqYear AND Ord_RequestDF.ReqNo = Ord_RequestHF.ReqNo Inner Join " +
                       "InvItemsMF on Ord_RequestDF.CompNo = InvItemsMF.CompNo AND Ord_RequestDF.SubItemNo = InvItemsMF.ItemNo Inner Join Ord_LinkPrchOrdUsers on Ord_LinkPrchOrdUsers.CompNo =Ord_RequestDF.CompNo and  Ord_LinkPrchOrdUsers.ReqYear =Ord_RequestDF.ReqYear " +
                       "and  Ord_LinkPrchOrdUsers.ReqNo =Ord_RequestDF.ReqNo and  Ord_LinkPrchOrdUsers.ItemNo =Ord_RequestDF.ItemNo and Ord_LinkPrchOrdUsers.UserId = '{1}'" +
                       "where Ord_RequestDF.CompNo = '{0}' AND bPurchaseOrder = 0 AND (bRequestforQuotation = 0) AND Ord_RequestHF.ReqStatus In(1,4,7)  AND (isnull(Ord_RequestDF.IsReject,0) = 0)    Order by InvItemsMF.prefvendor ", company.comp_num, me.UserID)).ToList();

                        }
                    }
                }
                else
                {
                    if (ordCompMF.LinkReqOrdByUser == false)
                    {
                        OrdReqtDF = new MDB().Database.SqlQuery<OrdRequestDF>(string.Format("SELECT Ord_RequestDF.CompNo as CompNo,Ord_RequestDF.ReqYear as ReqYear ,Ord_RequestDF.ReqNo as ReqNo,Ord_RequestDF.ItemSr as ItemSr,Ord_RequestDF.SubItemNo as SubItemNo,Ord_RequestDF.SubTUnit  as SubTUnit,  " +
                    "Ord_RequestDF.SubUnitSerial  as SubUnitSerial,Ord_RequestDF.Qty as Qty,case when Ord_RequestDF.Qty2 IS NULL then 0 else Ord_RequestDF.Qty2 end as Qty2,Ord_RequestDF.Price as Price,Ord_RequestDF.TotalValue as TotalValue,isnull(InvItemsMF.prefvendor,0) as prefvendor,Ord_RequestDF.ReqDeliveryDate as ReqDeliveryDate,Ord_RequestDF.Note as Note " +
                    "from Ord_RequestDF INNER JOIN  Ord_RequestHF ON Ord_RequestDF.CompNo = Ord_RequestHF.CompNo AND Ord_RequestDF.ReqYear = Ord_RequestHF.ReqYear AND Ord_RequestDF.ReqNo = Ord_RequestHF.ReqNo Inner Join " +
                    "InvItemsMF on Ord_RequestDF.CompNo = InvItemsMF.CompNo AND Ord_RequestDF.SubItemNo = InvItemsMF.ItemNo " +
                    "where Ord_RequestDF.CompNo = '{0}' AND bPurchaseOrder = 0 AND (bRequestforQuotation = 0) AND Ord_RequestHF.ReqStatus In(1,4,7) AND (isnull(Ord_RequestDF.IsReject,0) = 0)    Order by InvItemsMF.prefvendor ", company.comp_num)).ToList();

                    }
                    else
                    {
                        OrdReqtDF = new MDB().Database.SqlQuery<OrdRequestDF>(string.Format("SELECT Ord_RequestDF.CompNo as CompNo,Ord_RequestDF.ReqYear as ReqYear ,Ord_RequestDF.ReqNo as ReqNo,Ord_RequestDF.ItemSr as ItemSr,Ord_RequestDF.SubItemNo as SubItemNo,Ord_RequestDF.SubTUnit  as SubTUnit,  " +
                   "Ord_RequestDF.SubUnitSerial  as SubUnitSerial,Ord_RequestDF.Qty as Qty,case when Ord_RequestDF.Qty2 IS NULL then 0 else Ord_RequestDF.Qty2 end as Qty2,Ord_RequestDF.Price as Price,Ord_RequestDF.TotalValue as TotalValue,isnull(InvItemsMF.prefvendor,0) as prefvendor,Ord_RequestDF.ReqDeliveryDate as ReqDeliveryDate,Ord_RequestDF.Note as Note " +
                   "from Ord_RequestDF INNER JOIN  Ord_RequestHF ON Ord_RequestDF.CompNo = Ord_RequestHF.CompNo AND Ord_RequestDF.ReqYear = Ord_RequestHF.ReqYear AND Ord_RequestDF.ReqNo = Ord_RequestHF.ReqNo Inner Join " +
                   "InvItemsMF on Ord_RequestDF.CompNo = InvItemsMF.CompNo AND Ord_RequestDF.SubItemNo = InvItemsMF.ItemNo Inner Join Ord_LinkPrchOrdUsers on Ord_LinkPrchOrdUsers.CompNo =Ord_RequestDF.CompNo and  Ord_LinkPrchOrdUsers.ReqYear =Ord_RequestDF.ReqYear " +
                   "and  Ord_LinkPrchOrdUsers.ReqNo =Ord_RequestDF.ReqNo and  Ord_LinkPrchOrdUsers.ItemNo =Ord_RequestDF.ItemNo and Ord_LinkPrchOrdUsers.UserId = '{1}'" +
                   "where Ord_RequestDF.CompNo = '{0}' AND bPurchaseOrder = 0 AND (bRequestforQuotation = 0) AND Ord_RequestHF.ReqStatus In(1,4,7) AND (isnull(Ord_RequestDF.IsReject,0) = 0)    Order by InvItemsMF.prefvendor ", company.comp_num, me.UserID)).ToList();
                    }
                }
                return PartialView("ConvToPurchOrdDirectList", OrdReqtDF);
            }
            else
            {
                return PartialView("ConvToPurchOrdInDirectList");
            }
        }
        public ActionResult PurchOrdList()
        {
            return PartialView("PurchOrdList");
        }
        public ActionResult AttachPurchaseRequest(int ReqYear, string ReqNo)
        {
            ViewBag.ReqYear = ReqYear;
            ViewBag.ReqNo = ReqNo;
            return PartialView();
        }
        public ActionResult ReqDeliveryDate(int OrdYear, int OrdNo, string TawreedNo,string itemNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrdNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ItemNo = itemNo;
            return PartialView();
        }
        public ActionResult SuppliersItem(string itemNo)
        {
            ViewBag.ItemNo = itemNo;
            return PartialView();
        }
        public ActionResult PurchOrdInDirect(int ReqYear, string ReqNo)
        {
            ViewBag.ReqYear = ReqYear;
            ViewBag.ReqNo = ReqNo;
            return PartialView("ConvToPurchOrdInDirectList");
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
                            OrdSerial = Convert.ToInt32(rdr["srl_srl"]) +1;
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
        public JsonResult Return_PurchaseRequest(short ReqYear, string ReqNo, string ItemNo, string ReturnReason)
        {
            Ord_RequestDF df = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo && x.SubItemNo == ItemNo).FirstOrDefault();
            //foreach (Ord_RequestDF item in df)
            //{
            //    OrderHF hf = db.OrderHFs.Where(x => x.CompNo == item.CompNo && x.OrdYear == item.PurchaseOrderYear && x.OrderNo == item.PurchaseOrderNo
            //    && x.TawreedNo == item.PurchaseOrdTawreedNo).FirstOrDefault();
            //    if (hf != null)
            //    {
            //        return Json(new { error = Resources.Resource.errorProdOrderCreate }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            df.ReqStatus = 7;
            df.IsReject = true;
            df.RejectReason = ReturnReason;
            db.SaveChanges();

            Ord_RequestHF o = db.Ord_RequestHF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo).FirstOrDefault();
            o.RejectReason = ReturnReason;
            o.ReqStatus = 7;
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public int AddPreOrder(int BU, List<OrdRequestDF> OrdReqDF, DataSet ds, SqlDataAdapter DaPR, SqlTransaction MyTrans, SqlConnection co)
        {
            int i = 1;
            DataRow DrTmp;
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
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

                cmd.Transaction = MyTrans;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -1;
                }

                if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) != 0)
                {
                    if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -12)
                    {
                        return -12;
                    }
                    else
                    {
                        return -1;
                    }
                }

                LogID = Convert.ToInt64(cmd.Parameters["@LogID"].Value);
                OrdNo = Convert.ToInt32(cmd.Parameters["@UsedOrdNo"].Value);
                double? TotalQty = 0;

                if (CActive.ClientNo == 239)
                {
                    var OrderTypes = OrdReqDF.GroupBy(x => x.OrderType).ToList();
                    foreach (var OT in OrderTypes)
                    {
                        if (OT.Key == 1)
                        {
                            var Items = OrdReqDF.Where(x => x.OrderType == 1).GroupBy(x => x.SubItemNo).ToList();
                            foreach (var item in Items)
                            {
                                List<OrdRequestDF> OrdReqDF1 = OrdReqDF.Where(x => x.SubItemNo == item.Key).ToList();
                                InvItemsMF MF = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.Key).FirstOrDefault();
                                double? Qty = 0;
                                double? Qty2 = 0;
                                double? QtyUnit = 0;
                                double? Price = 0;
                                double? TotolPrice = 0;
                                short? SubUnitSerial = 0;
                                string QtyFormat = "";
                                StringBuilder Note = new StringBuilder();

                                foreach (OrdRequestDF DF in OrdReqDF1)
                                {

                                    if (OrdReqDF1.Count > 1)
                                    {
                                        if (DF.SubUnitSerial == 4)
                                        {
                                            Qty += DF.Qty;
                                            Price += DF.Price;
                                        }
                                        else if (DF.SubUnitSerial == 3)
                                        {
                                            QtyUnit = DF.Qty * Convert.ToDouble((MF.Conv4));
                                            Qty += QtyUnit;
                                            Price += DF.Price;
                                        }
                                        else if (DF.SubUnitSerial == 2)
                                        {
                                            QtyUnit = DF.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)));
                                            Qty += QtyUnit;
                                            Price += DF.Price;
                                        }
                                        else if (DF.SubUnitSerial == 1)
                                        {
                                            QtyUnit = DF.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)) * Convert.ToDouble((MF.Conv2)));
                                            Qty += QtyUnit;
                                            Price += DF.Price;
                                        }
                                        Qty2 += DF.Qty2;
                                        SubUnitSerial = 4;
                                    }
                                    else
                                    {
                                        Qty += DF.Qty;
                                        Qty2 += DF.Qty2;
                                        Price += DF.Price;
                                        SubUnitSerial = DF.SubUnitSerial;
                                    }

                                    Note.Append(DF.Note);
                                }


                                if (OrdReqDF1.Count > 1)
                                {
                                    TotolPrice = (Price / OrdReqDF1.Count);
                                }
                                else
                                {
                                    TotolPrice = Price;
                                }
                                QtyFormat = Qty.Value.ToString("0.000");
                                TotalQty += Convert.ToDouble(QtyFormat) * TotolPrice;




                                DrTmp = ds.Tables["POrderDF"].NewRow();
                                DrTmp.BeginEdit();
                                DrTmp["CompNo"] = company.comp_num;
                                DrTmp["OrdYear"] = DateTime.Now.Year;
                                DrTmp["OrderNo"] = OrdNo;
                                DrTmp["ItemNo"] = item.Key;
                                DrTmp["NSItemNo"] = 0;
                                DrTmp["Price"] = TotolPrice;
                                DrTmp["ExtraPrice"] = 0;
                                DrTmp["Qty"] = Qty.Value.ToString("0.000");
                                DrTmp["Qty2"] = Qty2.Value.ToString("0.000");
                                DrTmp["Bonus"] = 0;
                                DrTmp["TotalValue"] = TotolPrice * Convert.ToDouble(QtyFormat);
                                DrTmp["Confirmation"] = 1;
                                DrTmp["ConfirmQty"] = Qty.Value.ToString("0.000");
                                DrTmp["Note"] = Note;
                                DrTmp["DiscPer"] = 0;
                                DrTmp["ValueAfterDisc"] = Qty.Value.ToString("0.000");
                                DrTmp["DiscAmt"] = 0;
                                DrTmp["UnitSerial"] = SubUnitSerial;
                                DrTmp["ItemSr"] = i;

                                DrTmp.EndEdit();
                                ds.Tables["POrderDF"].Rows.Add(DrTmp);
                                i = i + 1;
                            }
                        }
                        else
                        {
                            List<OrdRequestDF> df = OrdReqDF.Where(x => x.OrderType == 2).ToList();
                            foreach (var item in df)
                            {
                                double? Qty = 0;
                                double? Qty2 = 0;
                                double? QtyUnit = 0;
                                double? Price = 0;
                                InvItemsMF MF = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.SubItemNo).FirstOrDefault();
                                if (item.SubUnitSerial == 4)
                                {
                                    Qty = item.Qty;
                                    Price = item.Price;
                                }
                                else if (item.SubUnitSerial == 3)
                                {
                                    QtyUnit = item.Qty * Convert.ToDouble((MF.Conv4));
                                    Qty = QtyUnit;
                                    Price = item.Price;
                                }
                                else if (item.SubUnitSerial == 2)
                                {
                                    QtyUnit = item.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)));
                                    Qty = QtyUnit;
                                    Price += item.Price;
                                }
                                else if (item.SubUnitSerial == 1)
                                {
                                    QtyUnit = item.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)) * Convert.ToDouble((MF.Conv2)));
                                    Qty = QtyUnit;
                                    Price = item.Price;
                                }


                                DrTmp = ds.Tables["POrderDF"].NewRow();
                                DrTmp.BeginEdit();
                                DrTmp["CompNo"] = company.comp_num;
                                DrTmp["OrdYear"] = DateTime.Now.Year;
                                DrTmp["OrderNo"] = OrdNo;
                                DrTmp["ItemNo"] = item.SubItemNo;
                                DrTmp["NSItemNo"] = 0;
                                DrTmp["Price"] = Price;
                                DrTmp["ExtraPrice"] = 0;
                                DrTmp["Qty"] = Qty.Value.ToString("0.000");
                                DrTmp["Qty2"] = Qty2.Value.ToString("0.000");
                                DrTmp["Bonus"] = 0;
                                DrTmp["TotalValue"] = Price * Convert.ToDouble(Qty.Value.ToString("0.000"));
                                DrTmp["Confirmation"] = 1;
                                DrTmp["ConfirmQty"] = Qty.Value.ToString("0.000");
                                DrTmp["Note"] = item.Note;
                                DrTmp["DiscPer"] = 0;
                                DrTmp["ValueAfterDisc"] = Qty.Value.ToString("0.000");
                                DrTmp["DiscAmt"] = 0;
                                DrTmp["UnitSerial"] = item.SubUnitSerial;
                                DrTmp["ItemSr"] = i;

                                DrTmp.EndEdit();
                                ds.Tables["POrderDF"].Rows.Add(DrTmp);
                                i = i + 1;
                            }
                        }
                    }
                }
                else
                {
                    var Items = OrdReqDF.GroupBy(x => x.SubItemNo).ToList();
                    foreach (var item in Items)
                    {
                        List<OrdRequestDF> OrdReqDF1 = OrdReqDF.Where(x => x.SubItemNo == item.Key).ToList();
                        InvItemsMF MF = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.Key).FirstOrDefault();
                        double? Qty = 0;
                        double? Qty2 = 0;
                        double? QtyUnit = 0;
                        double? Price = 0;
                        double? TotolPrice = 0;
                        short? SubUnitSerial = 0;
                        string QtyFormat = "";
                        StringBuilder Note = new StringBuilder();

                        foreach (OrdRequestDF DF in OrdReqDF1)
                        {

                            if (OrdReqDF1.Count > 1)
                            {
                                if (DF.SubUnitSerial == 4)
                                {
                                    Qty += DF.Qty;
                                    Price += DF.Price;
                                }
                                else if (DF.SubUnitSerial == 3)
                                {
                                    QtyUnit = DF.Qty * Convert.ToDouble((MF.Conv4));
                                    Qty += QtyUnit;
                                    Price += DF.Price;
                                }
                                else if (DF.SubUnitSerial == 2)
                                {
                                    QtyUnit = DF.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)));
                                    Qty += QtyUnit;
                                    Price += DF.Price;
                                }
                                else if (DF.SubUnitSerial == 1)
                                {
                                    QtyUnit = DF.Qty * (Convert.ToDouble((MF.Conv4)) * Convert.ToDouble((MF.Conv3)) * Convert.ToDouble((MF.Conv2)));
                                    Qty += QtyUnit;
                                    Price += DF.Price;
                                }
                                Qty2 += DF.Qty2;
                                SubUnitSerial = 4;
                            }
                            else
                            {
                                Qty += DF.Qty;
                                Qty2 += DF.Qty2;
                                Price += DF.Price;
                                SubUnitSerial = DF.SubUnitSerial;
                            }

                            Note.Append(DF.Note);
                        }


                        if (OrdReqDF1.Count > 1)
                        {
                            TotolPrice = (Price / OrdReqDF1.Count);
                        }
                        else
                        {
                            TotolPrice = Price;
                        }
                        QtyFormat = Qty.Value.ToString("0.000");
                        TotalQty += Convert.ToDouble(QtyFormat) * TotolPrice;




                        DrTmp = ds.Tables["POrderDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["OrdYear"] = DateTime.Now.Year;
                        DrTmp["OrderNo"] = OrdNo;
                        DrTmp["ItemNo"] = item.Key;
                        DrTmp["NSItemNo"] = 0;
                        DrTmp["Price"] = TotolPrice;
                        DrTmp["ExtraPrice"] = 0;
                        DrTmp["Qty"] = Qty.Value.ToString("0.000");
                        DrTmp["Qty2"] = Qty2.Value.ToString("0.000");
                        DrTmp["Bonus"] = 0;
                        DrTmp["TotalValue"] = TotolPrice * Convert.ToDouble(QtyFormat);
                        DrTmp["Confirmation"] = 1;
                        DrTmp["ConfirmQty"] = Qty.Value.ToString("0.000");
                        DrTmp["Note"] = Note;
                        DrTmp["DiscPer"] = 0;
                        DrTmp["ValueAfterDisc"] = Qty.Value.ToString("0.000");
                        DrTmp["DiscAmt"] = 0;
                        DrTmp["UnitSerial"] = SubUnitSerial;
                        DrTmp["ItemSr"] = i;

                        DrTmp.EndEdit();
                        ds.Tables["POrderDF"].Rows.Add(DrTmp);
                        i = i + 1;
                    }
                }

                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = co;
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
                    cmd1.Transaction = MyTrans;
                    DaPR.InsertCommand = cmd1;
                    try
                    {
                        DaPR.Update(ds, "POrderDF");
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }
        public int AddOrder(int BU,string TawreedNo, OrdPreOrderHF PreOrderHF, List<OrdPreOrderDF> PreOrderDF, Vendor Vend, int Dept, PayCodes Source, DataSet ds1, SqlDataAdapter DaPR1, DataSet ds2, SqlDataAdapter DaPR2, SqlTransaction MyTrans, SqlConnection co)
        {
            
            DataRow DrTmp1;
            DataRow DrTmp2;
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();

            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_ChkOrderHF";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = DateTime.Now.Year;
                Cmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdNo;
                Cmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                Cmd.Transaction = MyTrans;
                SqlDataReader rdr = Cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        db.OrdPreOrderHFs.Remove(PreOrderHF);
                        db.OrdPreOrderDFs.RemoveRange(PreOrderDF);
                        co.Dispose();
                        return -2;
                    }
                }

                rdr.Close();
            }

            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetCompMF";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Transaction = MyTrans;
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

            double? itemDiscVal = 0;
            double? ItemTaxVal = 0;
            double? totalTaxVal = 0;
            double? itemTotal = 0;
            double? totalAmount = 0;
            double? Total = 0;
            string unitname = "";
            foreach (OrdPreOrderDF item in PreOrderDF)
            {
                InvItemsMF MF = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == item.ItemNo).FirstOrDefault();

                switch (item.UnitSerial)
                {
                    case 1:
                        unitname = MF.UnitC1;
                        break;
                    case 2:
                        unitname = MF.UnitC2;
                        break;
                    case 3:
                        unitname = MF.UnitC3;
                        break;
                    case 4:
                        unitname = MF.UnitC4;
                        break;
                }



                itemTotal = item.Qty * item.Price;
                totalAmount += itemTotal;
                itemDiscVal = (item.Qty * item.Price * Convert.ToDouble(item.DiscPer)) / 100;
                if (CActive.ClientNo == 335)
                {
                    ItemTaxVal = (item.Qty * item.Price - itemDiscVal) * (0 / 100);
                }
                else
                {
                    ItemTaxVal = (item.Qty * item.Price - itemDiscVal) * (MF.STax_Perc / 100);
                }
                totalTaxVal += ItemTaxVal;
                Total = itemTotal - itemDiscVal;
                TotalNetAmount += Total;

                DrTmp1 = ds1.Tables["PReqDF"].NewRow();
                DrTmp1.BeginEdit();
                DrTmp1["CompNo"] = item.CompNo;
                DrTmp1["OrderNo"] = item.OrderNo;
                DrTmp1["OrdYear"] = item.OrdYear;
                DrTmp1["TawreedNo"] = TawreedNo;
                DrTmp1["ItemNo"] = item.ItemNo;
                DrTmp1["ItemSr"] = item.ItemSr;
                DrTmp1["PUnit"] = unitname;
                DrTmp1["PerDiscount"] = item.DiscPer;
                DrTmp1["Bonus"] = item.Bonus;
                DrTmp1["Qty"] = item.Qty;
                DrTmp1["Qty2"] = item.Qty2;
                DrTmp1["Price"] = item.Price;
                DrTmp1["ItemTaxType"] = 1;
                if (CActive.ClientNo == 335)
                {
                    DrTmp1["ItemTaxPer"] = 0;
                }
                else
                {
                    DrTmp1["ItemTaxPer"] = MF.STax_Perc;
                }
                DrTmp1["ItemTaxVal"] = ItemTaxVal;
                DrTmp1["RefNo"] = 0;
                DrTmp1["ordamt"] = itemTotal;
                DrTmp1["VouDiscount"] = 0;
                DrTmp1["Note"] = item.Note;
                DrTmp1["NoteDtl"] = "";
                DrTmp1["UnitSerial"] = item.UnitSerial;
                DrTmp1.EndEdit();
                ds1.Tables["PReqDF"].Rows.Add(DrTmp1);
            }

            TotalNetAmount = TotalNetAmount + totalTaxVal;

            PayCodes ShipMethod = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 sys_minor as sys_minor ,sys_adesc as DescAr,isnull(sys_edesc,'No Desc.') as DescEn from SysCodes where (CompNo = '{0}') AND sys_major='0f'", company.comp_num)).FirstOrDefault();

            PayCodes DlivPlace = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 PlaceCode as Id ,Place as DescAr from OrdDlivPlace where (CompNo = '{0}')", company.comp_num)).FirstOrDefault();



            using (SqlCommand CmdHead = new SqlCommand())
            {
                CmdHead.Connection = co;
                CmdHead.CommandText = "Ord_AddOrderHF";
                CmdHead.CommandType = CommandType.StoredProcedure;
                CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                CmdHead.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = PreOrderHF.Year;
                CmdHead.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = PreOrderHF.OrderNo;
                CmdHead.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;

                CmdHead.Parameters.Add(new SqlParameter("@OfferNo", SqlDbType.Int)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = Vend.VendorNo;
                CmdHead.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                CmdHead.Parameters.Add(new SqlParameter("@ConfDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                CmdHead.Parameters.Add(new SqlParameter("@ApprovalDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();

                if (Vend.Curr == null || Vend.Curr == 0)
                {
                    PayCodes Currency = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 CurrType as Id ,CurrName as DescAr,isnull(CurrNameE,'No Desc.') as DescEn from CurrType")).FirstOrDefault();
                    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = Currency.Id;
                }
                else
                {
                    CmdHead.Parameters.Add(new SqlParameter("@CurType", SqlDbType.Int)).Value = Vend.Curr;
                }
                if (Vend.Pmethod == null || Vend.Pmethod == 0)
                {
                    BusunisUnit PaymentM = new MDB().Database.SqlQuery<BusunisUnit>(string.Format("select top 1 MCode as Id ,[Desc] as DescAr,isnull([EngDesc],'No Desc.') as DescEn from PayMethods where (Comp = '{0}')", company.comp_num)).FirstOrDefault();
                    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = PaymentM.Id;
                }
                else
                {
                    CmdHead.Parameters.Add(new SqlParameter("@PayWay", SqlDbType.Int)).Value = Vend.Pmethod;
                }
                CmdHead.Parameters.Add(new SqlParameter("@ShipWay", SqlDbType.Int)).Value = ShipMethod.sys_minor;
                CmdHead.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = 0;



                CmdHead.Parameters.Add(new SqlParameter("@DlvryPlace", SqlDbType.NVarChar, 20)).Value = DlivPlace.Id;
                CmdHead.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "";
                CmdHead.Parameters.Add(new SqlParameter("@QutationRef", SqlDbType.VarChar)).Value = "";
                CmdHead.Parameters.Add(new SqlParameter("@DlvDays", SqlDbType.Int)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@MainPeriod", SqlDbType.Int)).Value = 0;

                CmdHead.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = totalTaxVal;
                CmdHead.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@TotalAmt", SqlDbType.Float)).Value = totalAmount;
                CmdHead.Parameters.Add(new SqlParameter("@UnitKind", SqlDbType.VarChar)).Value = "Each";
                CmdHead.Parameters.Add(new SqlParameter("@NetAmt", SqlDbType.Float)).Value = TotalNetAmount;

                CmdHead.Parameters.Add(new SqlParameter("@OrderClose", SqlDbType.Bit)).Value = false;
                CmdHead.Parameters.Add(new SqlParameter("@DlvState", SqlDbType.SmallInt, 4)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@LcDept", SqlDbType.Int)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@LcAcc", SqlDbType.BigInt)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@PInCharge", SqlDbType.VarChar)).Value = Vend.Resp_Person;
                CmdHead.Parameters.Add(new SqlParameter("@Vend_Dept", SqlDbType.Int)).Value = Dept;

                CmdHead.Parameters.Add(new SqlParameter("@VouDiscount", SqlDbType.Money)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@PerDiscount", SqlDbType.Float)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.VarChar)).Value = Source.sys_minor;

                CmdHead.Parameters.Add(new SqlParameter("@GenNote", SqlDbType.VarChar)).Value = "";

                CmdHead.Parameters.Add(new SqlParameter("@UseOrderAprv", SqlDbType.Bit)).Value = false;
                CmdHead.Parameters.Add(new SqlParameter("@IsApproved", SqlDbType.Bit)).Value = false;

                CmdHead.Parameters.Add(new SqlParameter("@ShipTerms", SqlDbType.VarChar)).Value = "";
                CmdHead.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                CmdHead.Parameters.Add(new SqlParameter("@ExpShDate", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                CmdHead.Parameters.Add(new SqlParameter("@ETA", SqlDbType.SmallDateTime)).Value = DateTime.Now.ToShortDateString();
                CmdHead.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar)).Value = "";
                if (CActive.ClientNo == 335)
                {
                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = true;
                }
                else if (CActive.ClientNo == 343)
                {
                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = true;
                }
                else
                {
                    CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = false;
                }
                CmdHead.Parameters.Add(new SqlParameter("@FreightExpense", SqlDbType.Money)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@ExtraExpenses", SqlDbType.Money)).Value = 0;

                CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                CmdHead.Parameters.Add(new SqlParameter("@Frmstat", SqlDbType.SmallInt)).Value = 1;
                CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                CmdHead.Parameters.Add(new SqlParameter("@LastCost", SqlDbType.Bit)).Value = 0;
                CmdHead.Parameters.Add(new SqlParameter("@RefNo", SqlDbType.NVarChar)).Value = "";


                CmdHead.Transaction = MyTrans;
                try
                {
                    CmdHead.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return -1;
                }

                var LogID = Convert.ToString(CmdHead.Parameters["@LogID"].Value);

                using (SqlCommand CmdIns = new SqlCommand())
                {
                    CmdIns.Connection = co;
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
                    CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                    CmdIns.Transaction = MyTrans;
                    DaPR1.InsertCommand = CmdIns;
                    try
                    {
                        DaPR1.Update(ds1, "PReqDF");
                    }
                    catch (Exception ex)
                    {
                       
                        return -1;
                    }
                }


                DrTmp2 = ds2.Tables["TblConds"].NewRow();
                DrTmp2.BeginEdit();
                DrTmp2["CompNo"] = company.comp_num;
                DrTmp2["OrderNo"] = OrdNo;
                DrTmp2["OrdYear"] = DateTime.Now.Year;
                DrTmp2["TwareedNo"] = TawreedNo;
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
                ds2.Tables["TblConds"].Rows.Add(DrTmp2);

                using (SqlCommand CmdInsConds = new SqlCommand())
                {
                    CmdInsConds.Connection = co;
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
                    CmdInsConds.Transaction = MyTrans;
                    DaPR2.InsertCommand = CmdInsConds;
                    try
                    {
                        DaPR2.Update(ds2, "TblConds");
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
            return 0;
        }
        public JsonResult CreatePurchOrder(int BU, List<OrdRequestDF> OrdReqDF)
        {
            long VendorNo = OrdReqDF.Select(o => o.prefvendor).FirstOrDefault();
            Vendor Vend = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == VendorNo).FirstOrDefault();
            int Dept = new MDB().Database.SqlQuery<int>(string.Format("select TOP 1 crb_dep as VenderId from GLCRBMF where (CRB_COMP = '{0}') AND (crb_acc = '{1}')", company.comp_num, Vend.VendorNo)).FirstOrDefault();
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            OrdCompMF ordCompMF = db.OrdCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
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

            
            string TawreedNo = "0";
            PayCodes Source = new PayCodes();
            if (CActive.ClientNo == 343)
            {
                Source = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 sys_minor as sys_minor ,sys_adesc as DescAr,isnull(sys_edesc,'No Desc.') as DescEn from SysCodes where (CompNo = '{0}') AND sys_major='91k' AND (sys_minor = '2')", company.comp_num)).FirstOrDefault();
            }
            else
            {
                Source = new MDB().Database.SqlQuery<PayCodes>(string.Format("select top 1 sys_minor as sys_minor ,sys_adesc as DescAr,isnull(sys_edesc,'No Desc.') as DescEn from SysCodes where (CompNo = '{0}') AND sys_major='91k' AND (sys_minor = '1')", company.comp_num)).FirstOrDefault();
            }
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdPreOrderDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "POrderDF");

                transaction = cn.BeginTransaction();

                int InsertPreOrder = AddPreOrder(BU, OrdReqDF, ds, DaPR, transaction, cn);
                if (InsertPreOrder == 0)
                {
                    if (CActive.ClientNo == 239)
                    {
                        if (Source.sys_minor == "1")
                        {
                            TawreedNo = GetOrderSerial(DateTime.Now.Year, 6, transaction, cn);
                        }
                        else
                        {
                            TawreedNo = GetOrderSerial(DateTime.Now.Year, 7, transaction, cn);
                        }
                    }
                    else
                    {
                        TawreedNo = OrdNo.ToString();
                    }
                    if (TawreedNo == "0")
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = Resources.Resource.errorPurchaseOrderSerial }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (InsertPreOrder == -1)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (InsertPreOrder == -12)
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
                transaction.Commit();
                cn.Dispose();
            }

            using (var cn1 = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction1;
                cn1.Open();

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDF  WHERE compno = 0", cn1);
                DaPR1.Fill(ds1, "PReqDF");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrdCondsGuaranty  WHERE compno = 0", cn1);
                DaPR2.Fill(ds2, "TblConds");


                transaction1 = cn1.BeginTransaction();

                OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == company.comp_num && x.Year == DateTime.Now.Year && x.OrderNo == OrdNo).FirstOrDefault();
                List<OrdPreOrderDF> PreOrderDF = db.OrdPreOrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).ToList();


                int InsertOrder = AddOrder(BU, TawreedNo, PreOrderHF, PreOrderDF, Vend, Dept, Source, ds1, DaPR1, ds2, DaPR2, transaction1, cn1);

                if(InsertOrder == 0)
                {
                    transaction1.Commit();
                    cn1.Dispose();
                }
                else
                {
                    if (InsertOrder == -1)
                    {
                        db.OrdPreOrderHFs.Remove(PreOrderHF);
                        db.OrdPreOrderDFs.RemoveRange(PreOrderDF);
                        db.SaveChanges();
                        transaction1.Rollback();
                        cn1.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (InsertOrder == -2)
                    {
                        db.OrdPreOrderHFs.Remove(PreOrderHF);
                        db.OrdPreOrderDFs.RemoveRange(PreOrderDF);
                        db.SaveChanges();
                        transaction1.Rollback();
                        cn1.Dispose();
                        if (Language == "en")
                        {
                            return Json(new { error = "P.O Is Referred To Direct Purchase On Tawreed NO. " + TawreedNo + "  " + "Add Operation Failed" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { error = "طلب الشراء محال للشراء المباشر على رقم توريد " + TawreedNo + "  " + "لم تنجح عملية الاضافة" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }

            using (var cn2 = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction2;
                cn2.Open();

                transaction2 = cn2.BeginTransaction();

                OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == company.comp_num && x.Year == DateTime.Now.Year && x.OrderNo == OrdNo).FirstOrDefault();
                List<OrdPreOrderDF> PreOrderDF = db.OrdPreOrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == DateTime.Now.Year && x.OrderNo == OrdNo).ToList();

                using (SqlCommand CmdChkQty = new SqlCommand())
                {
                    CmdChkQty.Connection = cn2;
                    CmdChkQty.CommandText = "Ord_ChkQuantities";
                    CmdChkQty.CommandType = CommandType.StoredProcedure;
                    CmdChkQty.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    CmdChkQty.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = DateTime.Now.Year;
                    CmdChkQty.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdNo;
                    CmdChkQty.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = "";
                    CmdChkQty.Parameters.Add("@ExtraRecPer", SqlDbType.Int).Value = -2;
                    CmdChkQty.Parameters.Add("@Rows", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                    CmdChkQty.Transaction = transaction2;
                    CmdChkQty.ExecuteNonQuery();

                    if (Convert.ToInt16(CmdChkQty.Parameters["@Rows"].Value) > 0)
                    {
                        
                        OrderHF OrderHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == PreOrderHF.Year && x.OrderNo == PreOrderHF.OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();
                        List<OrderDF> OrderDF = db.OrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrderHF.OrdYear && x.OrderNo == OrderHF.OrderNo && x.TawreedNo == OrderHF.TawreedNo).ToList();

                        db.OrderHFs.Remove(OrderHF);
                        db.OrderDFs.RemoveRange(OrderDF);

                        db.OrdPreOrderHFs.Remove(PreOrderHF);
                        db.OrdPreOrderDFs.RemoveRange(PreOrderDF);
                        db.SaveChanges();

                        transaction2.Rollback();
                        cn2.Dispose();
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

                bool InsertWorkFlow = AddWorkFlowLog(3, BU, DateTime.Now.Year.ToString(), OrdNo.ToString(), TawreedNo, 1, TotalNetAmount, transaction2, cn2);
                if (InsertWorkFlow == false)
                {
                    OrderHF OrderHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == PreOrderHF.Year && x.OrderNo == PreOrderHF.OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();
                    List<OrderDF> OrderDF = db.OrderDFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrderHF.OrdYear && x.OrderNo == OrderHF.OrderNo && x.TawreedNo == OrderHF.TawreedNo).ToList();

                    db.OrderHFs.Remove(OrderHF);
                    db.OrderDFs.RemoveRange(OrderDF);

                    db.OrdPreOrderHFs.Remove(PreOrderHF);
                    db.OrdPreOrderDFs.RemoveRange(PreOrderDF);
                    db.SaveChanges();
                    transaction2.Rollback();
                    cn2.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }

                foreach (OrdRequestDF item in OrdReqDF)
                {
                    int year = DateTime.Now.Year;
                    Ord_RequestHF ex1 = db.Ord_RequestHF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo).FirstOrDefault();
                    ex1.ReqStatus = 4;

                    Ord_LinkRquestDFAndPO exd = new Ord_LinkRquestDFAndPO();
                    exd.CompNo = company.comp_num;
                    exd.ReqYear = item.ReqYear;
                    exd.ReqNo = item.ReqNo;
                    exd.ItemNo = item.SubItemNo;
                    exd.PurchaseOrderYear = Convert.ToInt16(year);
                    exd.PurchaseOrderNo = OrdNo;
                    exd.PurchaseOrdTawreedNo = TawreedNo;
                    exd.Qty = item.Qty;
                    db.Ord_LinkRquestDFAndPO.Add(exd);
                    db.SaveChanges();



                    if (ordCompMF.IsDivisionPurchaseOrder == false)
                    {
                        Ord_RequestDF ex = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.SubItemNo == item.SubItemNo && x.ItemSr == item.ItemSr).FirstOrDefault();
                        ex.PurchaseOrderYear = Convert.ToInt16(year);
                        ex.PurchaseOrderNo = OrdNo;
                        ex.PurchaseOrdTawreedNo = TawreedNo;
                        ex.PurchaseOrdOfferNo = 0;
                        ex.bPurchaseOrder = true;
                    }
                    else
                    {
                        Ord_RequestDF ex = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.SubItemNo == item.SubItemNo && x.ItemSr == item.ItemSr).FirstOrDefault();
                        double ReqestQty = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo && x.SubItemNo == item.SubItemNo && x.ItemSr == item.ItemSr).Sum(o => o.Qty).Value;
                        double qty = db.Ord_LinkRquestDFAndPO.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear && x.ReqNo == item.ReqNo  && x.ItemNo == item.SubItemNo).Sum(o => o.Qty).Value;
                        if (qty == ReqestQty)
                        {
                            ex.PurchaseOrderYear = Convert.ToInt16(year);
                            ex.PurchaseOrderNo = OrdNo;
                            ex.PurchaseOrdTawreedNo = TawreedNo;
                            ex.PurchaseOrdOfferNo = 0;
                            ex.bPurchaseOrder = true;
                        }
                        else
                        {
                            ex.PurchaseOrderYear = Convert.ToInt16(year);
                            ex.PurchaseOrderNo = OrdNo;
                            ex.PurchaseOrdTawreedNo = TawreedNo;
                            ex.PurchaseOrdOfferNo = 0;
                            ex.bPurchaseOrder = false;
                        }
                    }

                    db.SaveChanges();
                }
            }

            return Json(new { vendor = OrdReqDF.Select(o => o.prefvendor).FirstOrDefault(),TawreedNo = TawreedNo, OrdNo = OrdNo, OrdYear = DateTime.Now.Date.Year, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreatePurchaseOrder(int BU, OrderHF OrdHF, List<OrderDF> OrdDF,List<OrderDFShipDate> OrderDFShipDate, OrdCondsGuaranty OrdCondsGuaranty,List<Ord_POAttachments> Ord_POAttachments,List<UploadFile> DocumentArchive)
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

            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();

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

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.OrderDFShipDate  WHERE compno = 0", cn);
                DaPR2.Fill(ds2, "TmpDelv");

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
                    if (CActive.ClientNo == 335)
                    {
                        CmdHead.Parameters.Add(new SqlParameter("@IsLC", SqlDbType.Bit)).Value = true;
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
                        DrTmp["OrderNo"] = item.OrderNo;
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
                        CmdIns.Parameters.Add(new SqlParameter("@ReqYear", SqlDbType.SmallInt, 4, "ReqYear"));
                        CmdIns.Parameters.Add(new SqlParameter("@ReqNo", SqlDbType.VarChar, 10, "ReqNo"));
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
                    if(OrdCondsGuaranty.CondPerc3 == null)
                    {
                        OrdCondsGuaranty.CondPerc3 = 0;
                    }
                    DrTmp1["CondPerc3"] = OrdCondsGuaranty.CondPerc3;
                    if (OrdCondsGuaranty.CondPerc4 == null)
                    {
                        OrdCondsGuaranty.CondPerc4 = 0;
                    }
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


                    using (SqlCommand CmdChkQty = new SqlCommand())
                    {
                        CmdChkQty.Connection = cn;
                        CmdChkQty.CommandText = "Ord_ChkQuantities";
                        CmdChkQty.CommandType = CommandType.StoredProcedure;
                        CmdChkQty.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        CmdChkQty.Parameters.Add("@OrdYear", SqlDbType.SmallInt).Value = OrdHF.OrdYear;
                        CmdChkQty.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdHF.OrderNo;
                        CmdChkQty.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = "";
                        CmdChkQty.Parameters.Add("@ExtraRecPer", SqlDbType.Int).Value = -2;
                        CmdChkQty.Parameters.Add("@Rows", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        CmdChkQty.Transaction = transaction;
                        CmdChkQty.ExecuteNonQuery();

                        if(Convert.ToInt16(CmdChkQty.Parameters["@Rows"].Value) > 0 )
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "en")
                            {
                                return Json(new { error = "Cant exceed approved Qty." }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "لايمكن تجاوز الكمية الموافق عليها "  }, JsonRequestBehavior.AllowGet);
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


                    List<UploadFile> upFile = (List<UploadFile>)Session["Files"];
                    if(upFile != null)
                    {
                        if (upFile.Count != 0)
                        {
                            if(DocumentArchive != null)
                            {
                                int serial = 0;
                                foreach (UploadFile item in DocumentArchive)
                                {
                                    UploadFile fl = upFile.Where(x => x.FileId == item.FileId).FirstOrDefault();
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
                                        if(item.Description == null)
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

                bool InsertWorkFlow = AddWorkFlowLog(3, BU, OrdHF.OrdYear.ToString(), OrdHF.OrderNo.ToString(), OrdHF.TawreedNo,2, OrdHF.NetAmt, transaction, cn); ;
                if (InsertWorkFlow == false)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }
                transaction.Commit();
                cn.Dispose();
            }

            foreach (OrderDF item in OrdDF)
            {
                Ord_LinkRquestDFAndPO ex = db.Ord_LinkRquestDFAndPO.Where(x => x.CompNo == company.comp_num && x.PurchaseOrderYear == item.OrdYear && x.PurchaseOrderNo == item.OrderNo && x.PurchaseOrdTawreedNo == item.TawreedNo && x.ItemNo == item.ItemNo).FirstOrDefault();
                ex.Qty = item.Qty;
                db.SaveChanges();

                OrdCompMF ordcompmf = db.OrdCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
                if(ordcompmf.IsDivisionPurchaseOrder == true)
                {
                    Ord_RequestDF ex1 = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ex.ReqYear && x.ReqNo == ex.ReqNo && x.SubItemNo == item.ItemNo).FirstOrDefault();
                    double ReqestQty = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ex.ReqYear && x.ReqNo == ex.ReqNo && x.SubItemNo == item.ItemNo).Sum(o => o.Qty).Value;
                    double qty = db.Ord_LinkRquestDFAndPO.Where(x => x.CompNo == company.comp_num && x.ReqYear == ex.ReqYear && x.ReqNo == ex.ReqNo && x.ItemNo == item.ItemNo).Sum(o => o.Qty).Value;
                    if (qty == ReqestQty)
                    {
                        ex1.PurchaseOrderYear = item.OrdYear;
                        ex1.PurchaseOrderNo = item.OrderNo;
                        ex1.PurchaseOrdTawreedNo = item.TawreedNo;
                        ex1.PurchaseOrdOfferNo = 0;
                        ex1.bPurchaseOrder = true;
                    }
                    else
                    {
                        ex1.PurchaseOrderYear = item.OrdYear;
                        ex1.PurchaseOrderNo = item.OrderNo;
                        ex1.PurchaseOrdTawreedNo = item.TawreedNo;
                        ex1.PurchaseOrdOfferNo = 0;
                        ex1.bPurchaseOrder = false;
                    }
                }
            }

            return Json(new { vendor = OrdHF.VendorNo, TawreedNo = OrdHF.TawreedNo, OrdNo = OrdHF.OrderNo, OrdYear = OrdHF.OrdYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLog(int FID, int BU, string TrYear, string TrNo, string TawreedNo,  int FStat, double? TrAmnt, SqlTransaction MyTrans, SqlConnection co)
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
         

       public ActionResult AddPurchaseOrder(long vendor, int OrdYear, int OrdNo, string TawreedNo)
        {
            ViewBag.vendor = vendor;
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrdNo;
            ViewBag.TawreedNo = TawreedNo;
            return View();
        }
        public string InsertFile()
        {
            List<UploadFile> lstFile = null;
            if(Session["Files"] == null)
            {
                lstFile = new List<UploadFile>();
            }
            else
            {
                lstFile = (List<UploadFile>)Session["Files"];
            }
            UploadFile SinglFile = new UploadFile();
            var Id = Request.Params["Id"];

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.FileId == Convert.ToInt32(Id));
            }

            if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    if (SinglFile == null)
                    {
                        var _file = (HttpPostedFileBase)Request.Files[file];
                        byte[] FileByte = null;
                        BinaryReader rdr = new BinaryReader(_file.InputStream);
                        FileByte = rdr.ReadBytes((int)_file.ContentLength);
                        SinglFile = new UploadFile();
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
                        byte[] FileByte = null;
                        BinaryReader rdr = new BinaryReader(_file.InputStream);
                        FileByte = rdr.ReadBytes((int)_file.ContentLength);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                    
                }
                Session["Files"] = lstFile;
            }


            return "done";
        }

        public string RemoveFile(int Id)
        {
            List<UploadFile> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFile>();
            }
            else
            {
                lstFile = (List<UploadFile>)Session["Files"];
            }
            UploadFile SinglFile = new UploadFile();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.FileId == Id);
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["Files"] = lstFile;
            }

            return "done";
        }

        public JsonResult CreateRequestforQuotation(List<OrdCopyInfo> OrdCopyInfo, int ReqYear, string ReqNo)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            int ReqforQuotNo =  db.MRP_Web_OrdCopyInfo.Where(x => x.CompNo == company.comp_num).OrderByDescending(o => o.ReqforQuotNo).Select(s => s.ReqforQuotNo).FirstOrDefault();
            ReqforQuotNo = ReqforQuotNo + 1;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.MRP_Web_OrdCopyInfo  WHERE compno = 0", cn);
                DaPR.Fill(ds, "OrdCopyInfo");
                transaction = cn.BeginTransaction();

                foreach (var item in OrdCopyInfo)
                {
                    Ord_RequestDF RequestDF = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear && x.ReqNo == ReqNo && x.SubItemNo == item.ItemNo).FirstOrDefault();
                    DrTmp = ds.Tables["OrdCopyInfo"].NewRow();
                    DrTmp.BeginEdit();
                    DrTmp["CompNo"] = company.comp_num;
                    DrTmp["ReqforQuotyear"] = DateTime.Now.Year;
                    DrTmp["ReqforQuotNo"] = ReqforQuotNo;
                    DrTmp["VendorNo"] = item.VendorNo;
                    DrTmp["ItemNo"] = item.ItemNo;
                    DrTmp["RequiredQty"] = item.RequiredQty;
                    DrTmp["Qty"] = item.RequiredQty;
                    DrTmp["SellPrice"] = RequestDF.Price;
                    DrTmp["Qty2"] = item.Qty2;
                    DrTmp["SellDate"] = DateTime.Now;
                    DrTmp["Notes"] = "إنشاء طلب عرض سعر من الويب";
                    DrTmp["Port"] = item.Port;
                    DrTmp["SendTestSample"] = item.SendTestSample;
                    DrTmp.EndEdit();
                    ds.Tables["OrdCopyInfo"].Rows.Add(DrTmp);
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "MRP_Web_AddOrdCopyInfo";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                    cmd.Parameters.Add(new SqlParameter("@ReqforQuotyear", SqlDbType.Int, 8, "ReqforQuotyear"));
                    cmd.Parameters.Add(new SqlParameter("@ReqforQuotNo", SqlDbType.Int, 8, "ReqforQuotNo"));
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 250, "ItemNo"));
                    cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 20, "VendorNo"));
                    cmd.Parameters.Add(new SqlParameter("@RequiredQty", SqlDbType.Float, 20, "RequiredQty"));
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 20, "Qty"));
                    cmd.Parameters.Add(new SqlParameter("@SellPrice", SqlDbType.Money, 20, "SellPrice"));
                    cmd.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 20, "Qty2"));
                    cmd.Parameters.Add(new SqlParameter("@SellDate", SqlDbType.SmallDateTime, 16, "SellDate"));
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 250, "Notes"));
                    cmd.Parameters.Add(new SqlParameter("@Port", SqlDbType.VarChar, 250, "Port"));
                    cmd.Parameters.Add(new SqlParameter("@SendTestSample", SqlDbType.Bit, 1, "SendTestSample"));
                    cmd.Transaction = transaction;
                    DaPR.InsertCommand = cmd;
                    try
                    {
                        DaPR.Update(ds, "OrdCopyInfo");
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

                List<Ord_RequestDF> df = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == ReqYear
                 && x.ReqNo == ReqNo).ToList();

                foreach (Ord_RequestDF item in df)
                {
                    Ord_RequestDF ex = db.Ord_RequestDF.Where(x => x.CompNo == company.comp_num && x.ReqYear == item.ReqYear
                    && x.ReqNo == item.ReqNo && x.SubItemNo == item.ItemNo).FirstOrDefault();
                    if (ex != null)
                    {
                        ex.RequestforQuotationYear = Convert.ToInt16(DateTime.Now.Year);
                        ex.RequestforQuotationNo = ReqforQuotNo;
                        ex.bRequestforQuotation = true;
                        db.SaveChanges();
                    }
                }
            }

            ClientsActive CA = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            if(CA.ClientNo == 249)
            {
                var Vendors = OrdCopyInfo.GroupBy(x => x.VendorNo).ToList();
                AlphaOrd_Email Email = db.AlphaOrd_Email.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
                foreach (var Vend in Vendors)
                {
                    Vendor v = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == Vend.Key).FirstOrDefault();
                    var fromAddress = new MailAddress(me.Email, "طلب عرض سعر شراء");
                    var toAddress = new MailAddress(v.EMail, "To " + v.Name);
                    var smtp = new SmtpClient
                    {
                        Host = Email.SmtpAddress,
                        Port = Convert.ToInt32(Email.SmtpPort),
                        EnableSsl = Convert.ToBoolean(Email.IsUsingSSL),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, me.UserPWD)
                    };

                    var body = new StringBuilder();
                    body.AppendLine(@"<div dir=""rtl"">");
                    body.AppendLine(string.Format("<p>{0} : {1} </p>", "طلب عرض سعر برقم شراء - ", ReqNo));
                    body.AppendLine("</div>");
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        IsBodyHtml = Email.IsHtmlBody.Value,
                        Subject = "Alpha P.O Quotation",
                        Body = body.ToString(),
                    })
                    {
                        smtp.Send(message);
                    };
                }
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public class OrdCopyInfo
        {
            public short CYear { get; set; }

            public string OrderNo { get; set; }

            public string ItemNo { get; set; }

            public long VendorNo { get; set; }

            public double RequiredQty { get; set; }

            public double Qty2 { get; set; }

            public string Port { get; set; }
            
            public bool SendTestSample { get; set; }
        }
    }
}