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
    public class SlaughterOrderController : controller
    {
        // GET: SlaughterOrder
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SlaughterOrderList()
        {
            return PartialView();
        }
        public ActionResult AddSlaughterOrder()
        {
            return View();
        }
        public ActionResult SlaughterOrderItemsList(int StoreNo)
        {
            ViewBag.StoreNo = StoreNo;
            return PartialView();
        }
        public ActionResult eSlaughterOrderItemsList(int StoreNo, long SlaughterOrderNo)
        {
            ViewBag.StoreNo = StoreNo;
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            return PartialView();
        }
        public ActionResult EditSlaughterOrder(long SlaughterOrderNo)
        {
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            return View();
        }
        public ActionResult ViewSlaughterOrder(long SlaughterOrderNo)
        {
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            return View();
        }
        public JsonResult LoadUnitItem(string ItemNo, int storeNo)
        {
            DataTable Dt = new DataTable();
            DataTable DtBatch = new DataTable();
            string BatchNo = "";
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "InvT_LoadItemUnits";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemCode", System.Data.SqlDbType.VarChar, 20)).Value = ItemNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<InvUnitCode> InvUnitCode = Dt.AsEnumerable().Select(row => new InvUnitCode
            {
                UnitCode = row.Field<string>("UnitCode"),
                VSUnitCode = row.Field<int>("UnitSerial"),
                UnitName = row.Field<string>("UnitName"),
                UnitNameEng = row.Field<string>("UnitNameEng")
            }).ToList();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Invt_StoreBatches";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", System.Data.SqlDbType.Int)).Value = storeNo;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", System.Data.SqlDbType.VarChar, 20)).Value = ItemNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(DtBatch);
                    }
                }
            }

            foreach (DataRow row in DtBatch.Rows)
            {
                BatchNo = row["BatchNo"].ToString();
                break;
            }

            return Json(new { unit = InvUnitCode, Batch = BatchNo }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadItemKits(string ItemNo, int StoreNo, string Batch)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            ViewBag.Batch = Batch;
            return PartialView();
        }
        public ActionResult eLoadItemKits(int Serial, string ItemNo, int StoreNo, string Batch, long SlaughterOrderNo)
        {
            ViewBag.Serial = Serial;
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            ViewBag.Batch = Batch;

            return PartialView();
        }
        public ActionResult LoadItemBatch(string ItemNo, int StoreNo)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            return PartialView();
        }
        public ActionResult eLoadItemBatch(string ItemNo, int StoreNo, long SlaughterOrderNo)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            return PartialView();
        }
        public ActionResult LoadItemBatchhf(string ItemNo, int StoreNo)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            return PartialView();
        }
        public ActionResult eLoadItemBatchhf(string ItemNo, int StoreNo, long SlaughterOrderNo)
        {
            ViewBag.ItemNo = ItemNo;
            ViewBag.StoreNo = StoreNo;
            ViewBag.SlaughterOrderNo = SlaughterOrderNo;
            return PartialView();
        }
        public DataSet LoadStoreBatchs(string ItemNo, int StoreNo, short UnitSerial, int VouType)
        {
            DataSet Dsbatch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = cn;
                    Cmd.CommandText = "InvT_LoadStoreBatchs";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@ItemNo", SqlDbType.VarChar).Value = ItemNo;
                    Cmd.Parameters.Add("@StoreNo", SqlDbType.Int).Value = StoreNo;
                    Cmd.Parameters.Add("@UnitSerial", SqlDbType.TinyInt).Value = UnitSerial;
                    Cmd.Parameters.Add("@VouType", SqlDbType.TinyInt).Value = 1;
                    Cmd.Transaction = transaction;
                    try
                    {
                        DaCmd.SelectCommand = Cmd;
                        DaCmd.Fill(Dsbatch, "BatchsTbl");
                        Dsbatch.Tables["BatchsTbl"].PrimaryKey = new DataColumn[] { Dsbatch.Tables["BatchsTbl"].Columns["ItemNo"], Dsbatch.Tables["BatchsTbl"].Columns["BatchNo"] };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        Dsbatch = new DataSet();
                    }
                }
            }

            return Dsbatch;
        }
        public JsonResult Save_SlaughterOrder(Invt_SlaughterOrderHF SlaughterOrderHF, List<Invt_SlaughterOrderDF> SlaughterOrderDF, List<Invt_OutlaySlaughter> OutlaySlaughter)
        {
            foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            {
                bool ChkStores = ChkStore(item.StoreNo.Value, item.ItemNo);
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
            //foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            //{
            //    DataSet DsBatchs = new DataSet();
            //    DsBatchs = LoadStoreBatchs(item.ItemNo, item.StoreNo.Value, item.UnitSerial.Value,1);
            //    if (DsBatchs.Tables["BatchsTbl"].Rows.Count == 0)
            //    {
            //        if (Language == "ar-JO")
            //        {
            //            return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + " الموجودة في مستودع رقم  " + " " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //    }

            //    DataRow DrBatch1;
            //    object[] findTheseVals = new object[2];
            //    findTheseVals[0] = item.ItemNo;
            //    findTheseVals[1] = item.Batch;
            //    DrBatch1 = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);
            //    if (DrBatch1 == null)
            //    {
            //        if (Language == "ar-JO")
            //        {
            //            return Json(new { error = "الحزمة غير معرفة للمادة " + "  " + item.ItemNo + " الموجودة في مستودع رقم " + " " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            return Json(new { error = "The Batch is not defined for the item " + " " + item.ItemNo + " " + " That Exist In Store No " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //    }
            //}


            Invt_SlaughterOrderHF OrderNo = db.Invt_SlaughterOrderHF.Where(x => x.CompNo == company.comp_num).OrderByDescending(z => z.SlaughterOrderNo).FirstOrDefault();
            long count = 0;
            if (OrderNo != null)
            {
                count = OrderNo.SlaughterOrderNo + 1;

            }
            else
            {
                count = 1;
            }
            Invt_SlaughterOrderHF ex = new Invt_SlaughterOrderHF();
            ex.CompNo = SlaughterOrderHF.CompNo;
            ex.SlaughterOrderNo = count;
            ex.SlaughterOrderDate = SlaughterOrderHF.SlaughterOrderDate;
            ex.StoreNo = SlaughterOrderHF.StoreNo;
            ex.ItemNo = SlaughterOrderHF.ItemNo;
            ex.Batch = SlaughterOrderHF.Batch;
            ex.Qty = SlaughterOrderHF.Qty;
            ex.TUnit = SlaughterOrderHF.TUnit;
            ex.UnitSerial = SlaughterOrderHF.UnitSerial;
            ex.Qty2 = SlaughterOrderHF.Qty2;
            if (SlaughterOrderHF.Notes == null)
            {
                SlaughterOrderHF.Notes = "";
            }
            ex.Notes = SlaughterOrderHF.Notes;
            ex.IsApproval = false;
            db.Invt_SlaughterOrderHF.Add(ex);
            db.SaveChanges();

            short ItemSrl = 1;

            foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            {
                Invt_SlaughterOrderDF ex2 = new Invt_SlaughterOrderDF();
                ex2.CompNo = item.CompNo;
                ex2.SlaughterOrderNo = count;
                ex2.StoreNo = item.StoreNo;
                ex2.ItemNo = item.ItemNo;
                ex2.ItemSrl = ItemSrl;
                ex2.Batch = item.Batch;
                ex2.Qty = item.Qty;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.Qty2 = item.Qty2;
                ex2.CostPercent = item.CostPercent;
                db.Invt_SlaughterOrderDF.Add(ex2);
                db.SaveChanges();
                ItemSrl++;
            }

            foreach (Invt_OutlaySlaughter item in OutlaySlaughter)
            {
                Invt_OutlaySlaughter ex3 = new Invt_OutlaySlaughter();
                ex3.CompNo = item.CompNo;
                ex3.SlaughterOrderNo = count;
                ex3.DocEspCode = item.DocEspCode;
                ex3.DocEspVal = item.DocEspVal;
                db.Invt_OutlaySlaughter.Add(ex3);
                db.SaveChanges();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Edit_SlaughterOrder(Invt_SlaughterOrderHF SlaughterOrderHF, List<Invt_SlaughterOrderDF> SlaughterOrderDF, List<Invt_OutlaySlaughter> OutlaySlaughter)
        {
            foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            {
                bool ChkStores = ChkStore(item.StoreNo.Value, item.ItemNo);
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

            //foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            //{
            //    DataSet DsBatchs = new DataSet();
            //    DsBatchs = LoadStoreBatchs(item.ItemNo, item.StoreNo.Value, item.UnitSerial.Value, 1);
            //    if (DsBatchs.Tables["BatchsTbl"].Rows.Count == 0)
            //    {
            //        if (Language == "ar-JO")
            //        {
            //            return Json(new { error = " لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + " الموجودة في مستودع رقم  " + " " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //    }

            //    DataRow DrBatch1;
            //    object[] findTheseVals = new object[2];
            //    findTheseVals[0] = item.ItemNo;
            //    findTheseVals[1] = item.Batch;
            //    DrBatch1 = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);
            //    if (DrBatch1 == null)
            //    {
            //        if (Language == "ar-JO")
            //        {
            //            return Json(new { error = "الحزمة غير معرفة للمادة " + " " + item.ItemNo + " الموجودة في مستودع رقم  " + " " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            return Json(new { error = "The Batch is not defined for the item " + " " + item.ItemNo + " " + " That Exist In Store No " + item.StoreNo.Value }, JsonRequestBehavior.AllowGet);
            //        }
            //    }
            //}
            Invt_SlaughterOrderHF ex = db.Invt_SlaughterOrderHF.Where(x => x.CompNo == SlaughterOrderHF.CompNo && x.SlaughterOrderNo == SlaughterOrderHF.SlaughterOrderNo).FirstOrDefault();
            ex.SlaughterOrderDate = SlaughterOrderHF.SlaughterOrderDate;
            ex.StoreNo = SlaughterOrderHF.StoreNo;
            ex.ItemNo = SlaughterOrderHF.ItemNo;
            ex.Batch = SlaughterOrderHF.Batch;
            ex.Qty = SlaughterOrderHF.Qty;
            ex.TUnit = SlaughterOrderHF.TUnit;
            ex.UnitSerial = SlaughterOrderHF.UnitSerial;
            ex.Qty2 = SlaughterOrderHF.Qty2;
            if (SlaughterOrderHF.Notes == null)
            {
                SlaughterOrderHF.Notes = "";
            }
            ex.Notes = SlaughterOrderHF.Notes;
            ex.IsApproval = false;
            db.SaveChanges();

            short ItemSrl = 1;

            List<Invt_SlaughterOrderDF> exdel2 = db.Invt_SlaughterOrderDF.Where(x => x.CompNo == SlaughterOrderHF.CompNo && x.SlaughterOrderNo == SlaughterOrderHF.SlaughterOrderNo).ToList();

            db.Invt_SlaughterOrderDF.RemoveRange(exdel2);
            db.SaveChanges();

            foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            {
                Invt_SlaughterOrderDF ex2 = new Invt_SlaughterOrderDF();
                ex2.CompNo = item.CompNo;
                ex2.SlaughterOrderNo = item.SlaughterOrderNo;
                ex2.StoreNo = item.StoreNo;
                ex2.ItemNo = item.ItemNo;
                ex2.ItemSrl = ItemSrl;
                ex2.Batch = item.Batch;
                ex2.Qty = item.Qty;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.Qty2 = item.Qty2;
                ex2.CostPercent = item.CostPercent;
                db.Invt_SlaughterOrderDF.Add(ex2);
                db.SaveChanges();
                ItemSrl++;
            }

            List<Invt_OutlaySlaughter> exdel = db.Invt_OutlaySlaughter.Where(x => x.CompNo == SlaughterOrderHF.CompNo && x.SlaughterOrderNo == SlaughterOrderHF.SlaughterOrderNo).ToList();

            db.Invt_OutlaySlaughter.RemoveRange(exdel);
            db.SaveChanges();

            foreach (Invt_OutlaySlaughter item in OutlaySlaughter)
            {
                Invt_OutlaySlaughter ex3 = new Invt_OutlaySlaughter();
                ex3.CompNo = item.CompNo;
                ex3.SlaughterOrderNo = item.SlaughterOrderNo;
                ex3.DocEspCode = item.DocEspCode;
                ex3.DocEspVal = item.DocEspVal;
                db.Invt_OutlaySlaughter.Add(ex3);
                db.SaveChanges();
            }
            return Json(new { SlaughterOrderNo = SlaughterOrderHF.SlaughterOrderNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_SlaughterOrder(long SlaughterOrderNo)
        {
            List<Invt_OutlaySlaughter> exdel = db.Invt_OutlaySlaughter.Where(x => x.CompNo == company.comp_num && x.SlaughterOrderNo == SlaughterOrderNo).ToList();

            db.Invt_OutlaySlaughter.RemoveRange(exdel);
            db.SaveChanges();

            List<Invt_SlaughterOrderDF> exdel2 = db.Invt_SlaughterOrderDF.Where(x => x.CompNo == company.comp_num && x.SlaughterOrderNo == SlaughterOrderNo).ToList();

            db.Invt_SlaughterOrderDF.RemoveRange(exdel2);
            db.SaveChanges();

            Invt_SlaughterOrderHF ex = db.Invt_SlaughterOrderHF.Where(x => x.CompNo == company.comp_num && x.SlaughterOrderNo == SlaughterOrderNo).FirstOrDefault();


            if (ex != null)
            {
                db.Invt_SlaughterOrderHF.Remove(ex);
                db.SaveChanges();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Posted_SlaughterOrder(long SlaughterOrderNo)
        {
            Invt_SlaughterOrderHF SlaughterOrderHF = db.Invt_SlaughterOrderHF.Where(x => x.CompNo == company.comp_num && x.SlaughterOrderNo == SlaughterOrderNo).FirstOrDefault();
            List<Invt_SlaughterOrderDF> SlaughterOrderDF = db.Invt_SlaughterOrderDF.Where(x => x.CompNo == company.comp_num && x.SlaughterOrderNo == SlaughterOrderNo).ToList();
            int count = 0;
            foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
            {
                if (item.Qty > 0)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب ان تكون كمية استلام اكبر من 0 لمادة واحدة ع الاقل " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The quantity received must be greater than 0 for at least one item " }, JsonRequestBehavior.AllowGet);
                }
            }
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
            int VouhNo = 1;

            int StoreNo = SlaughterOrderDF.Select(z => z.StoreNo.Value).FirstOrDefault();

            InvStoresMF LinkStoreIssue = db.InvStoresMFs.Where(x => x.CompNo == company.comp_num && x.StoreNo == SlaughterOrderHF.StoreNo.Value).FirstOrDefault();
            InvStoresMF LinkStoreReceipt = db.InvStoresMFs.Where(x => x.CompNo == company.comp_num && x.StoreNo == StoreNo).FirstOrDefault();

            GLCOMMF GLCOMMF = db.GLCOMMFs.Where(x => x.GLCOM_COMNUM == company.comp_num).FirstOrDefault();

            int DeptIssue = GLCOMMF.ContractDept.Value;
            long AccIssue = GLCOMMF.RevenueAcc.Value;//LinkStoreIssue.CashAcc;

            int DeptReceipt = GLCOMMF.ContractDept.Value ;
            long AccReceipt = GLCOMMF.RevenueAcc.Value; //LinkStoreReceipt.CashAcc;

            if (DeptIssue == 0 || AccIssue == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب ادخال معلومات الدائرة والحساب للمستودع " + " " + SlaughterOrderHF.StoreNo.Value + " من شاشة معلومات المستودع  " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The department and account information must be entered for the stores " + " " + SlaughterOrderHF.StoreNo.Value + " " + " From the stores information screen " }, JsonRequestBehavior.AllowGet);
                }
            }

            if (DeptReceipt == 0 || AccReceipt == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب ادخال معلومات الدائرة والحساب للمستودع " + " " + StoreNo + " من شاشة معلومات المستودع  " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The department and account information must be entered for the stores " + " " + StoreNo + " " + " From the stores information screen " }, JsonRequestBehavior.AllowGet);
                }
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TmpTbl");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM dbo.InvBatchsMF  WHERE compno = 0", cn);
                DaPR2.Fill(ds2, "TmpBatchs");

                transaction = cn.BeginTransaction();



                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = SlaughterOrderHF.SlaughterOrderDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 11;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = SlaughterOrderHF.SlaughterOrderDate.Value.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = SlaughterOrderHF.StoreNo.Value;

                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DeptIssue;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = AccIssue;

                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = SlaughterOrderHF.SlaughterOrderNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@NetFAmount", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashAcc", SqlDbType.BigInt)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء امر الذبح من الويب" + " - " + SlaughterOrderHF.SlaughterOrderNo;

                    cmd.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = SlaughterOrderHF.SlaughterOrderNo;
                    cmd.Parameters.Add(new SqlParameter("@CustRef", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@KitType", SqlDbType.Int)).Value = 2;
                    cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar)).Value = DateTime.Now.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@ActID", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@ComputerName", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@TSource", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 999999999;
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

                    if (CheckMinusQty(SlaughterOrderHF.StoreNo.Value, transaction, cn) == false)
                    {
                        transaction.Rollback();
                        cn.Dispose();

                        if (Language == "ar-JO")
                        {
                            return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + "موجودة في مستودع رقم" + " " + SlaughterOrderHF.StoreNo }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { error = "Can't execute because of a problem  in quantity of item  That Exist In Store No  " + SlaughterOrderHF.StoreNo }, JsonRequestBehavior.AllowGet);
                        }
                    }


                    DataSet DsBatchs = new DataSet();
                    DsBatchs = LoadStoreBatchs(SlaughterOrderHF.ItemNo, SlaughterOrderHF.StoreNo.Value, SlaughterOrderHF.UnitSerial.Value, transaction, cn);
                    DataRow DrBatch1;
                    object[] findTheseVals = new object[2];
                    findTheseVals[0] = SlaughterOrderHF.ItemNo;
                    findTheseVals[1] = SlaughterOrderHF.Batch;
                    DrBatch1 = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);
                    if (CheckMinusQty(SlaughterOrderHF.StoreNo.Value, transaction, cn) == false)
                    {
                        if (Convert.ToDouble(DrBatch1["QtyOH"]) < SlaughterOrderHF.Qty)
                        {
                            transaction.Rollback();
                            cn.Dispose();

                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + "الموجودة في مستودع رقم" + " " + SlaughterOrderHF.StoreNo }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "Can't execute because of a problem  in quantity of item  That Exist In Store No  " + SlaughterOrderHF.StoreNo }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }


                    DrTmp = ds.Tables["TmpTbl"].NewRow();
                    DrTmp.BeginEdit();
                    DrTmp["compno"] = company.comp_num;
                    DrTmp["VouYear"] = SlaughterOrderHF.SlaughterOrderDate.Value.Year;
                    DrTmp["VouType"] = 11;
                    DrTmp["VouNo"] = VouhNo;
                    DrTmp["StoreNo"] = SlaughterOrderHF.StoreNo;
                    DrTmp["ItemNo"] = SlaughterOrderHF.ItemNo;
                    DrTmp["Batch"] = SlaughterOrderHF.Batch;
                    DrTmp["ItemSer"] = i;
                    DrTmp["VouDate"] = SlaughterOrderHF.SlaughterOrderDate.Value;
                    DrTmp["Qty"] = SlaughterOrderHF.Qty.Value * -1;
                    DrTmp["Qty2"] = SlaughterOrderHF.Qty2.Value * -1;
                    DrTmp["ItemDimension"] = "0*0*0";
                    DrTmp["Bonus"] = 0;
                    DrTmp["TUnit"] = SlaughterOrderHF.TUnit;
                    DrTmp["NetSellValue"] = 0;
                    DrTmp["Discount"] = 0;
                    DrTmp["PerDiscount"] = 0;
                    DrTmp["VouDiscount"] = 0;
                    DrTmp["CurrNo"] = 0;
                    DrTmp["ForUCost"] = 0;
                    DrTmp["ToStoreNo"] = 0;
                    DrTmp["ToBatch"] = 0;
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
                    DrTmp["ProdOrderNo"] = 1;
                    DrTmp["UnitSerial"] = SlaughterOrderHF.UnitSerial;
                    DrTmp["ProdOrderNo"] = 1;
                    DrTmp["ExRate"] = 1;
                    DrTmp["NoteDet"] = "";

                    DrTmp.EndEdit();
                    ds.Tables["TmpTbl"].Rows.Add(DrTmp);

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
                        cmd1.Parameters.Add(new SqlParameter("@ItemDimension", SqlDbType.VarChar, 20, "ItemDimension"));
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
                        cmd1.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Parameters.Add(new SqlParameter("@NoteDet", SqlDbType.VarChar, 200, "NoteDet"));
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
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = SlaughterOrderHF.SlaughterOrderDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = SlaughterOrderHF.SlaughterOrderDate.Value.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = SlaughterOrderDF.Select(x => x.StoreNo.Value).FirstOrDefault();

                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DeptReceipt;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = AccReceipt;

                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = SlaughterOrderHF.SlaughterOrderNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@NetFAmount", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CashAcc", SqlDbType.BigInt)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء امر الذبح من الويب" + " - " + SlaughterOrderHF.SlaughterOrderNo;

                    cmd.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = SlaughterOrderHF.SlaughterOrderNo;
                    cmd.Parameters.Add(new SqlParameter("@CustRef", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@KitRef", SqlDbType.Int)).Value = VouhNo;
                    cmd.Parameters.Add(new SqlParameter("@KitType", SqlDbType.Int)).Value = 2;
                    cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar)).Value = DateTime.Now.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@ActID", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@ComputerName", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@TSource", SqlDbType.SmallInt)).Value = 0;
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
                    i = 1;

                    foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
                    {
                        if (item.Qty > 0)
                        {
                            DataSet DsBatchs = new DataSet();
                            DsBatchs = LoadStoreBatchs(item.ItemNo, item.StoreNo.Value, item.UnitSerial.Value, transaction, cn);
                            DataRow DrBatch;
                            object[] findTheseVals = new object[2];
                            findTheseVals[0] = item.ItemNo;
                            findTheseVals[1] = item.Batch;

                            DrBatch = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);

                            if (DrBatch == null)
                            {
                                DrTmp2 = ds2.Tables["TmpBatchs"].NewRow();
                                DrTmp2["CompNo"] = company.comp_num;
                                DrTmp2["StoreNo"] = item.StoreNo.Value;
                                DrTmp2["ItemNo"] = item.ItemNo;
                                DrTmp2["batchNo"] = item.Batch;
                                DrTmp2["ExpDate"] = DateTime.Now.ToShortDateString();
                                DrTmp2["ManDate"] = DateTime.Now.ToShortDateString();
                                DrTmp2["IsHalt"] = 0;
                                DrTmp2["UnitCost"] = 0;
                                DrTmp2["RefNo"] = "";
                                DrTmp2["RefNo2"] = "";
                                DrTmp2["Location"] = "";
                                DrTmp2["SellPrice"] = 0;
                                DrTmp2["BatchSerial"] = item.UnitSerial.Value;
                                ds2.Tables["TmpBatchs"].Rows.Add(DrTmp2);
                            }
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
                            DaPR2.InsertCommand = cmdBatch;
                            try
                            {
                                DaPR2.Update(ds2, "TmpBatchs");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    foreach (Invt_SlaughterOrderDF item in SlaughterOrderDF)
                    {
                        if (item.Qty > 0)
                        {
                            DrTmp1 = ds1.Tables["TmpTbl"].NewRow();
                            DrTmp1.BeginEdit();
                            DrTmp1["compno"] = company.comp_num;
                            DrTmp1["VouYear"] = SlaughterOrderHF.SlaughterOrderDate.Value.Year;
                            DrTmp1["VouType"] = 1;
                            DrTmp1["VouNo"] = VouhNo;
                            DrTmp1["StoreNo"] = item.StoreNo;
                            DrTmp1["ItemNo"] = item.ItemNo;
                            DrTmp1["Batch"] = item.Batch;
                            DrTmp1["ItemSer"] = i;
                            DrTmp1["VouDate"] = SlaughterOrderHF.SlaughterOrderDate.Value;
                            DrTmp1["Qty"] = item.Qty.Value * -1;
                            DrTmp1["Qty2"] = item.Qty2.Value * -1;
                            DrTmp1["ItemDimension"] = "0*0*0";
                            DrTmp1["Bonus"] = 0;
                            DrTmp1["TUnit"] = item.TUnit;
                            DrTmp1["NetSellValue"] = 0;
                            DrTmp1["Discount"] = 0;
                            DrTmp1["PerDiscount"] = 0;
                            DrTmp1["VouDiscount"] = 0;
                            DrTmp1["CurrNo"] = 0;
                            DrTmp1["ForUCost"] = 0;
                            DrTmp1["ToStoreNo"] = 0;
                            DrTmp1["ToBatch"] = 0;
                            DrTmp1["AccDepNo"] = 0;
                            DrTmp1["AccNo"] = 0;
                            DrTmp1["Item_Tax_Per"] = 0;
                            DrTmp1["Item_Tax_Val"] = 0;
                            DrTmp1["Item_STax_Per"] = 0;
                            DrTmp1["Item_STax_Val"] = 0;
                            DrTmp1["Item_Tax_Type"] = 0;
                            DrTmp1["Item_STax_Type"] = 0;
                            DrTmp1["Item_PTax_Per"] = 0;
                            DrTmp1["Item_PTax_Val"] = 0;
                            DrTmp1["Item_PTax_Type"] = 0;
                            DrTmp1["ProdOrderNo"] = 1;
                            DrTmp1["UnitSerial"] = item.UnitSerial;
                            DrTmp1["ProdOrderNo"] = 1;
                            DrTmp1["ExRate"] = 1;
                            DrTmp1["NoteDet"] = "";
                            if (item.CostPercent == null)
                            {
                                item.CostPercent = 0;
                            }
                            DrTmp1["CommPer"] = item.CostPercent;
                            DrTmp1.EndEdit();
                            ds1.Tables["TmpTbl"].Rows.Add(DrTmp1);
                            i = i + 1;
                        }
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
                        cmd1.Parameters.Add(new SqlParameter("@ItemDimension", SqlDbType.VarChar, 20, "ItemDimension"));
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
                        cmd1.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Parameters.Add(new SqlParameter("@NoteDet", SqlDbType.VarChar, 200, "NoteDet"));
                        cmd1.Parameters.Add(new SqlParameter("@CommPer", SqlDbType.Float, 8, "CommPer"));
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
                }

                transaction.Commit();
                cn.Dispose();
            }

            SlaughterOrderHF.IsApproval = true;
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool CheckMinusQty(int StoreNo, SqlTransaction MyTrans, SqlConnection co)
        {
            bool AllowMinus = false;
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "select AllowMinuseQty from InvStoresMF where  compno=" + company.comp_num + " and StoreNo='" + StoreNo + "'";
                Cmd.CommandType = CommandType.Text;
                Cmd.Transaction = MyTrans;
                try
                {
                    SqlDataReader rdr = Cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            AllowMinus = Convert.ToBoolean(rdr["AllowMinuseQty"]);
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    AllowMinus = false;
                }
            }
            int ChkRCount = 0;

            if (AllowMinus == false)
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = co;
                    Cmd.CommandText = "select InvBatchsMF.* from InvBatchsMF where  compno=" + company.comp_num + " and StoreNo='" + StoreNo + "' and qtyoh<0";
                    Cmd.CommandType = CommandType.Text;
                    Cmd.Transaction = MyTrans;
                    try
                    {
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                ChkRCount = ChkRCount + 1;
                            }
                        }
                        rdr.Close();
                    }
                    catch (Exception ex)
                    {
                        ChkRCount = 0;
                    }
                }
            }
            else
            {
                AllowMinus = true;
            }

            if (ChkRCount != 0)
            {
                AllowMinus = false;
            }
            else
            {
                AllowMinus = true;
            }
            return AllowMinus;
        }
    }
}
