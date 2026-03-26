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
    public class InboundsInfoController : controller
    {
        // GET: InboundsInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InboundsInfoList(int OrdYear)
        {
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            List<PurchaseOrderInfo> lOrderInfo = new List<PurchaseOrderInfo>();
            if (CActive.ClientNo == 239)
            {
                if (me.UserID == "Saif.sc" || me.UserID == "admin")
                {
                    lOrderInfo = new MDB().Database.SqlQuery<PurchaseOrderInfo>(string.Format("SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,Vendors.Name,Vendors.Eng_Name,  " +
                    "ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName  " +
                    "FROM Ord_RequestDF INNER JOIN OrderHF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo INNER JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN  Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo  " +
                    "left outer JOIN Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  " +
                    "where (Ord_RequestDF.CompNo = '{0}') AND (Ord_RequestDF.PurchaseOrderYear = '{1}') AND (Ord_RequestDF.bPurchaseOrder = 1) AND (dbo.OrderHF.TawreedNo >= 988) AND (OrderHF.Approved = 1)  GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0)  UNION  " +
                    "SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي '  else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,  " +
                    "Vendors.Name,Vendors.Eng_Name, ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName  FROM OrderHF left outer JOIN  " +
                    "Ord_RequestDF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo left outer JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo left outer JOIN " +
                    "Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  " +
                    "where (OrderHF.CompNo = '{0}') AND (OrderHF.OrdYear = '{1}')  AND (OrderHF.Approved = 1)  GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0) Order by OrderHF.OrdYear,OrderHF.OrderNo desc ,OrderHF.TawreedNo desc   ", company.comp_num, OrdYear)).ToList();
                }
                else
                {
                    lOrderInfo = new MDB().Database.SqlQuery<PurchaseOrderInfo>(string.Format("SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,Vendors.Name,Vendors.Eng_Name,  " +
                    "ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName  " +
                    "FROM Ord_RequestDF INNER JOIN OrderHF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo INNER JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN  Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo  " +
                    "left outer JOIN Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  " +
                    "where (Ord_RequestDF.CompNo = '{0}') AND (Ord_RequestDF.PurchaseOrderYear = '{1}') AND (Ord_RequestDF.bPurchaseOrder = 1) AND (dbo.OrderHF.TawreedNo >= 988) AND (OrderHF.Approved = 1) AND (dbo.OrderHF.OrdUser = '{2}')  GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0)   UNION  " +
                    "SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي '  else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,  " +
                    "Vendors.Name,Vendors.Eng_Name, ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName FROM OrderHF left outer JOIN  " +
                    "Ord_RequestDF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo left outer JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo left outer JOIN " +
                    "Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  " +
                    "where (OrderHF.CompNo = '{0}') AND (OrderHF.OrdYear = '{1}')  AND (OrderHF.Approved = 1) AND (dbo.OrderHF.OrdUser = '{2}')  GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0) Order by OrderHF.OrdYear,OrderHF.OrderNo desc ,OrderHF.TawreedNo desc   ", company.comp_num, OrdYear, me.UserID)).ToList();
                }
            }
            else
            {
                lOrderInfo = new MDB().Database.SqlQuery<PurchaseOrderInfo>(string.Format("SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي ' else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,Vendors.Name,Vendors.Eng_Name,  " +
                    "ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName  " +
                    "FROM Ord_RequestDF INNER JOIN OrderHF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo INNER JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN  Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo  " +
                    "left outer JOIN Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  inner join  " +
                    "OrdPreOrderHF on OrdPreOrderHF.CompNo = OrderHF.CompNo AND OrdPreOrderHF.Year = OrderHF.OrdYear AND OrdPreOrderHF.OrderNo = OrderHF.OrderNo inner join  " +
                    "Alpha_BusinessUnitUsers on Alpha_BusinessUnitUsers.CompNo = OrdPreOrderHF.CompNo and Alpha_BusinessUnitUsers.BusUnitID = OrdPreOrderHF.BusUnitID  and Alpha_BusinessUnitUsers.Permission = 1 AND (dbo.Alpha_BusinessUnitUsers.UserID = '{2}')  " +
                    "where (Ord_RequestDF.CompNo = '{0}') AND (Ord_RequestDF.PurchaseOrderYear = '{1}') AND (Ord_RequestDF.bPurchaseOrder = 1) AND (OrderHF.Approved = 1) GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0)   UNION  " +
                    "SELECT OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo,convert(date,OrderHF.EnterDate) as EnterDate ,OrderHF.VendorNo,OrderHF.RefNo,OrderHF.DlvState as DlvStateCode,case when dbo.OrderHF.DlvState = 0 then  'غير مستلم' else case when dbo.OrderHF.DlvState = 1 then 'مستلم جزئي '  else case when dbo.OrderHF.DlvState = 2 then 'مستلم كلي' else '*' end end end as DlvState, " +
                    "case when dbo.OrderHF.DlvState = 0 then  'Not Rec.' else case when dbo.OrderHF.DlvState = 1 then 'Partially Rec.' else case when dbo.OrderHF.DlvState = 2 then 'Completely Rec' else '*' end end end as DlvState_Eng,  " +
                    "Vendors.Name,Vendors.Eng_Name, ISNULL(Ord_InboundsInfoHF.IsClosed,0) as IsClosed,case when OrderHF.DlvState = 0 then 'lightskyblue' when OrderHF.DlvState = 1 then 'lightskyblue' when OrderHF.DlvState = 2 then 'lightgreen' else 'lightskyblue' end as colorName FROM OrderHF left outer JOIN  " +
                    "Ord_RequestDF on Ord_RequestDF.CompNo = OrderHF.CompNo AND Ord_RequestDF.PurchaseOrderYear = OrderHF.OrdYear AND Ord_RequestDF.PurchaseOrderNo = OrderHF.OrderNo AND Ord_RequestDF.PurchaseOrdTawreedNo = OrderHF.TawreedNo left outer JOIN  " +
                    "Ord_RequestHF on Ord_RequestHF.CompNo = Ord_RequestDF.CompNo AND Ord_RequestHF.ReqYear = Ord_RequestDF.ReqYear AND Ord_RequestHF.ReqNo = Ord_RequestDF.ReqNo INNER JOIN Vendors on Vendors.comp = OrderHF.CompNo and Vendors.VendorNo = OrderHF.VendorNo left outer JOIN " +
                    "Ord_InboundsInfoHF on Ord_InboundsInfoHF.CompNo = OrderHF.CompNo AND Ord_InboundsInfoHF.OrderYear = OrderHF.OrdYear AND Ord_InboundsInfoHF.OrderNo = OrderHF.OrderNo AND Ord_InboundsInfoHF.TawreedNo = OrderHF.TawreedNo  inner join " +
                    "OrdPreOrderHF on OrdPreOrderHF.CompNo = OrderHF.CompNo AND OrdPreOrderHF.Year = OrderHF.OrdYear AND OrdPreOrderHF.OrderNo = OrderHF.OrderNo inner join  " +
                    "Alpha_BusinessUnitUsers on Alpha_BusinessUnitUsers.CompNo = OrdPreOrderHF.CompNo and Alpha_BusinessUnitUsers.BusUnitID = OrdPreOrderHF.BusUnitID and Alpha_BusinessUnitUsers.Permission = 1 AND (dbo.Alpha_BusinessUnitUsers.UserID = '{2}')" +
                    "where (OrderHF.CompNo = '{0}') AND (OrderHF.OrdYear = '{1}') AND (OrderHF.Approved = 1)  GROUP BY OrderHF.OrdYear,OrderHF.OrderNo ,OrderHF.TawreedNo ,convert(date,OrderHF.EnterDate),OrderHF.VendorNo,Vendors.Name,Vendors.Eng_Name,OrderHF.RefNo,OrderHF.DlvState,ISNULL(Ord_InboundsInfoHF.IsClosed,0) Order by OrderHF.OrdYear,OrderHF.OrderNo desc ,OrderHF.TawreedNo desc   ", company.comp_num, OrdYear, me.UserID)).ToList();
            }

                return PartialView(lOrderInfo);
        }
        public ActionResult AddInboundsInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            List<Ord_InboundsInfoHF> lCountSrl = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).OrderByDescending(o => o.InboundSer).ToList();
            List<string> CountSrl = lCountSrl.Select(o => o.InboundSer).ToList();
            string InboundSer = "";
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            if (CountSrl.Count == 0)
            {
                InboundSer = TawreedNo + "/" + 1;
                ViewBag.srl = TawreedNo + 1;

            }
            else
            {
                List<int> x = new List<int>();
                foreach (string item in CountSrl)
                {
                    string srl = item.Replace(TawreedNo + "/", "");
                    x.Add(Convert.ToInt32(srl));
                }

                string srl1 = x.Max().ToString();
                //srl1 = srl1.Replace("/", "");
                //srl1 = srl1.TrimStart();
                int serl = Convert.ToInt32(srl1) + 1;
                InboundSer = TawreedNo + "/" + serl;
                ViewBag.srl = TawreedNo + serl;
            }
            ViewBag.InboundSer = InboundSer.TrimStart();

            return View();
        }
        public ActionResult EditInboundsInfo(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            string srl = InboundSer.Replace(TawreedNo, "");
            srl = srl.Replace("/", TawreedNo);
            ViewBag.srl = srl.TrimStart();
            return View();
        }
        public ActionResult ViewInboundsInfo(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            string srl = InboundSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();
            return View();
        }
        public ActionResult LoadItemBatch(string ItemNo, int StoreNo)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            return PartialView();
        }

        public ActionResult DetailsInboundsInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return PartialView();
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
        public JsonResult Save_InboundsInfo(Ord_InboundsInfoHF InboundsInfoHF, List<Ord_InboundsInfoDF> InboundsInfoDF)
        {
            foreach (Ord_InboundsInfoDF item in InboundsInfoDF)
            {
                bool ChkStores = ChkStore(InboundsInfoHF.InboundStoreNo.Value, item.ItemNo);
                if (ChkStores == false)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "المادة " + item.ItemNo + " غير معرفة في المستودع " }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "Item " + item.ItemNo + " Is not defined in the store" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }


            Ord_InboundsInfoHF ex = new Ord_InboundsInfoHF();
            ex.CompNo = InboundsInfoHF.CompNo;
            ex.OrderYear = InboundsInfoHF.OrderYear;
            ex.OrderNo = InboundsInfoHF.OrderNo;
            ex.TawreedNo = InboundsInfoHF.TawreedNo;
            ex.InboundSer = InboundsInfoHF.InboundSer;
            ex.InboundDate = InboundsInfoHF.InboundDate;
            ex.InboundStoreNo = InboundsInfoHF.InboundStoreNo;
            ex.InboundNotes = InboundsInfoHF.InboundNotes;
            ex.IsApproval = false;
            ex.IsClosed = false;
            ex.ReqStatus = 0;
            ex.RecStatus = 0;

            db.Ord_InboundsInfoHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_InboundsInfoDF item in InboundsInfoDF)
            {
                Ord_InboundsInfoDF ex2 = new Ord_InboundsInfoDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = InboundsInfoHF.OrderYear;
                ex2.OrderNo = InboundsInfoHF.OrderNo;
                ex2.TawreedNo = InboundsInfoHF.TawreedNo;
                ex2.InboundSer = item.InboundSer;
                ex2.ItemSrl = item.ItemSrl;
                ex2.ItemNo = item.ItemNo;
                ex2.batchNo = item.batchNo;
                ex2.Qty = item.Qty;
                ex2.BounsQty = item.BounsQty;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.Qty2 = item.Qty2;
                ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                db.Ord_InboundsInfoDF.Add(ex2);
                db.SaveChanges();
            }
            //using (SqlConnection cn = new SqlConnection(ConnectionString()))
            //{
            //    SqlTransaction transaction;
            //    cn.Open();

            //    transaction = cn.BeginTransaction();

            //    using (SqlCommand Cmd = new SqlCommand())
            //    {
            //        Cmd.Connection = cn;
            //        Cmd.CommandText = "WMS_ASN_Push";
            //        Cmd.CommandType = CommandType.StoredProcedure;
            //        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = InboundsInfoHF.CompNo;
            //        Cmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = InboundsInfoHF.OrderYear;
            //        Cmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = InboundsInfoHF.OrderNo;
            //        Cmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar, 50).Value = InboundsInfoHF.TawreedNo;
            //        Cmd.Parameters.Add("@InboundSer", SqlDbType.VarChar, 50).Value = InboundsInfoHF.InboundSer;
            //        Cmd.Transaction = transaction;
            //        Cmd.CommandTimeout = 999999999;
            //        Cmd.ExecuteNonQuery();
            //    }

            //    transaction.Commit();
            //    cn.Dispose();
            //}

            return Json(new { TawreedNo = InboundsInfoHF.TawreedNo, OrdNo = InboundsInfoHF.OrderNo, OrdYear = InboundsInfoHF.OrderYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_InboundsInfo(Ord_InboundsInfoHF InboundsInfoHF, List<Ord_InboundsInfoDF> InboundsInfoDF)
        {
            foreach (Ord_InboundsInfoDF item in InboundsInfoDF)
            {
                bool ChkStores = ChkStore(InboundsInfoHF.InboundStoreNo.Value, item.ItemNo);
                if (ChkStores == false)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "المادة " + item.ItemNo + "غير معرفة في المستودع" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "Item" + item.ItemNo + "Is not defined in the store" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            Ord_InboundsInfoHF ex = db.Ord_InboundsInfoHF.Where(x => x.CompNo == InboundsInfoHF.CompNo && x.OrderYear == InboundsInfoHF.OrderYear
            && x.OrderNo == InboundsInfoHF.OrderNo && x.TawreedNo == InboundsInfoHF.TawreedNo && x.InboundSer == InboundsInfoHF.InboundSer).FirstOrDefault();

            if (ex != null)
            {
                ex.InboundDate = InboundsInfoHF.InboundDate;
                ex.InboundStoreNo = InboundsInfoHF.InboundStoreNo;
                ex.InboundNotes = InboundsInfoHF.InboundNotes;
                ex.IsApproval = false;
                ex.IsClosed = false;
                db.SaveChanges();
            }

            foreach (Ord_InboundsInfoDF item in InboundsInfoDF)
            {
                Ord_InboundsInfoDF ex2 = db.Ord_InboundsInfoDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == InboundsInfoHF.OrderYear
            && x.OrderNo == InboundsInfoHF.OrderNo && x.TawreedNo == InboundsInfoHF.TawreedNo && x.InboundSer == item.InboundSer
            && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (ex2 != null)
                {
                    ex2.batchNo = item.batchNo;
                    ex2.Qty = item.Qty;
                    ex2.BounsQty = item.BounsQty;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.Qty2 = item.Qty2;
                    ex2.ReqDeliveryDate = item.ReqDeliveryDate;
                    db.SaveChanges();
                }
                else
                {
                    Ord_InboundsInfoDF ex1 = new Ord_InboundsInfoDF();
                    ex1.CompNo = item.CompNo;
                    ex1.OrderYear = InboundsInfoHF.OrderYear;
                    ex1.OrderNo = InboundsInfoHF.OrderNo;
                    ex1.TawreedNo = InboundsInfoHF.TawreedNo;
                    ex1.InboundSer = item.InboundSer;
                    ex1.ItemSrl = item.ItemSrl;
                    ex1.ItemNo = item.ItemNo;
                    ex1.batchNo = item.batchNo;
                    ex1.Qty = item.Qty;
                    ex1.BounsQty = item.BounsQty;
                    ex1.TUnit = item.TUnit;
                    ex1.UnitSerial = item.UnitSerial;
                    ex1.Qty2 = item.Qty2;
                    ex1.ReqDeliveryDate = item.ReqDeliveryDate;
                    db.Ord_InboundsInfoDF.Add(ex1);
                    db.SaveChanges();
                }
            }

            return Json(new { TawreedNo = InboundsInfoHF.TawreedNo, OrdNo = InboundsInfoHF.OrderNo, OrdYear = InboundsInfoHF.OrderYear, InboundSer= InboundsInfoHF.InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_InboundsInfo(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            List<Ord_InboundsInfoDF> exdel = db.Ord_InboundsInfoDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer).ToList();
            db.Ord_InboundsInfoDF.RemoveRange(exdel);
            db.SaveChanges();

            Ord_InboundsInfoHF ex = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo 
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_InboundsInfoHF.Remove(ex);
                db.SaveChanges();
            }

            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, InboundSer = InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Approval_InboundsInfo(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            Ord_InboundsInfoHF ex = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer).FirstOrDefault();

            if (ex != null)
            {
                ex.IsApproval = true;
                db.SaveChanges();
            }

            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, InboundSer = InboundSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Closed_InboundsInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            List<Ord_InboundsInfoHF> ex = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo).ToList();

            foreach (Ord_InboundsInfoHF item in ex)
            {
                Ord_InboundsInfoHF ex1 = db.Ord_InboundsInfoHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
             && x.TawreedNo == item.TawreedNo && x.InboundSer == item.InboundSer).FirstOrDefault();
                if (ex1 != null)
                {
                    ex1.IsClosed = true;
                    db.SaveChanges();
                }
            }

            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}