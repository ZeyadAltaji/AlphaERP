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
    public class VoucIssueController : controller
    {
        // GET: VoucIssue
        public ActionResult Index()
        {
            Session["Files"] = null;
            return View();
        }

        #region PaymentOrd
        public ActionResult VocPaymentOrdList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            
            return PartialView("~/Views/VoucIssue/PaymentOrd/VocPaymentOrdList.cshtml");
        }
        public ActionResult ChoosePaymentOrderRec(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return PartialView("~/Views/VoucIssue/PaymentOrd/ChoosePaymentOrderRec.cshtml");
        }
        public ActionResult DetailsPaymentOrderRec(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return PartialView("~/Views/VoucIssue/PaymentOrd/DetailsPaymentOrderRec.cshtml");
        }
        public ActionResult PaymentOrdItemsList(int StoreNo, int srl)
        {
            ViewBag.StoreNo = StoreNo;
            ViewBag.srl = srl;

            return PartialView("~/Views/VoucIssue/PaymentOrd/PaymentOrdItemsList.cshtml");
        }
        public ActionResult ePaymentOrdItemsList(int StoreNo, int srl)
        {
            ViewBag.StoreNo = StoreNo;
            ViewBag.srl = srl;
            return PartialView("~/Views/VoucIssue/PaymentOrd/ePaymentOrdItemsList.cshtml");
        }
        public ActionResult CPaymentOrdItemsList(int StoreNo, int srl)
        {
            ViewBag.StoreNo = StoreNo;
            ViewBag.srl = srl;
            return PartialView("~/Views/VoucIssue/PaymentOrd/ePaymentOrdItemsList.cshtml");
        }
        public ActionResult ChoosePaymentOrdItems(List<InvItemsMF> items, int srl, int id)
        {
            ViewBag.Serial = srl;
            ViewBag.id = id;

            return PartialView("~/Views/VoucIssue/PaymentOrd/ChoosePaymentOrdItems.cshtml", items);
        }
        public ActionResult eChoosePaymentOrdItems(List<InvItemsMF> items, int srl, int id)
        {
            ViewBag.Serial = srl;
            ViewBag.id = id;
            return PartialView("~/Views/VoucIssue/PaymentOrd/eChoosePaymentOrdItems.cshtml", items);
        }
        public ActionResult CChoosePaymentOrdItems(List<InvItemsMF> items, int srl, int id)
        {
            ViewBag.Serial = srl;
            ViewBag.id = id;
            return PartialView("~/Views/VoucIssue/PaymentOrd/eChoosePaymentOrdItems.cshtml", items);
        }
        public ActionResult AddPaymentOrder(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return View("~/Views/VoucIssue/PaymentOrd/AddPaymentOrder.cshtml");
        }
        public ActionResult EditPaymentOrder(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.VouNo = VouNo;
            ViewBag.VouType = VouType;

            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArch(OrdYear, OrderNo, TawreedNo, VouNo, VouType, transaction, cn);

            }
            int i = 1;
            if(DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.VouNo == Convert.ToInt32(DrArch["VouNo"]) && a.VouType == 11 && a.FileId == i);
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileVoch();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(11);
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
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(11);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i = i + 1;
                    Session["Files"] = lstFile;
                }

            }

            return View("~/Views/VoucIssue/PaymentOrd/EditPaymentOrder.cshtml");
        }

        public ActionResult CopyVoucIssue(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.VouNo = VouNo;
            ViewBag.VouType = VouType;

            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArch(OrdYear, OrderNo, TawreedNo, VouNo, VouType, transaction, cn);

            }
            int i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.VouNo == Convert.ToInt32(DrArch["VouNo"]) && a.VouType == 11 && a.FileId == i);
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileVoch();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(11);
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
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(11);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i = i + 1;
                    Session["Files"] = lstFile;
                }
            }

            return View("~/Views/VoucIssue/PaymentOrd/CopyVoucIssue.cshtml");
        }
        public ActionResult ViewPaymentOrder(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.VouNo = VouNo;
            ViewBag.VouType = VouType;
            return View("~/Views/VoucIssue/PaymentOrd/ViewPaymentOrder.cshtml");
        }
        public JsonResult Save_PaymentOrder(Ord_VoucIssueHF OrdVoucIssueHF, List<Ord_VoucIssueDF> OrdVoucIssueDF, List<UploadFileVoch> FileArchive)
        {
            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                bool ChkStores = ChkStore(OrdVoucIssueHF.StoreNo.Value, item.ItemNo);
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

            int VouNo = GetOrderSerial(OrdVoucIssueHF.OrderYear, OrdVoucIssueHF.StoreNo.Value, OrdVoucIssueHF.DocType.Value, OrdVoucIssueHF.VouType);
            if (VouNo == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا يمكنك الاستمرار يجب تعريف تسلسل امر صرف أولا" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "You Cant Proceed ,You Should Define a Serial For Issue Order" }, JsonRequestBehavior.AllowGet);
                }
            }

            Ord_VoucIssueHF ex = new Ord_VoucIssueHF();
            ex.CompNo = OrdVoucIssueHF.CompNo;
            ex.OrderYear = OrdVoucIssueHF.OrderYear;
            ex.OrderNo = OrdVoucIssueHF.OrderNo;
            ex.TawreedNo = OrdVoucIssueHF.TawreedNo;
            ex.VouType = OrdVoucIssueHF.VouType;
            ex.VouNo = VouNo;
            ex.ShipSer = OrdVoucIssueHF.ShipSer;
            ex.DocType = OrdVoucIssueHF.DocType;
            ex.StoreNo = OrdVoucIssueHF.StoreNo;
            ex.VouDate = OrdVoucIssueHF.VouDate;
            ex.Notes = OrdVoucIssueHF.Notes;
            ex.IsApproval = false;
            ex.VouTypeNew = 0;
            ex.VouYear = OrdVoucIssueHF.VouDate.Value.Year;
            ex.VouNoNew = 0;
            db.Ord_VoucIssueHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                Ord_VoucIssueDF ex2 = new Ord_VoucIssueDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = item.OrderYear;
                ex2.OrderNo = item.OrderNo;
                ex2.TawreedNo = item.TawreedNo;
                ex2.VouType = item.VouType;
                ex2.VouNo = VouNo;
                ex2.ItemNo = item.ItemNo;
                ex2.ShipSer = item.ShipSer;
                ex2.Qty = item.Qty;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.Qty2 = item.Qty2;
                db.Ord_VoucIssueDF.Add(ex2);
                db.SaveChanges();
            }
            List<UploadFileVoch> upFile = (List<UploadFileVoch>)Session["Files"];

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if(FileArchive != null)
                        {
                            foreach (UploadFileVoch item in FileArchive)
                            {
                                UploadFileVoch fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.FileId == item.FileId).FirstOrDefault();

                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddVoucIssueArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = OrdVoucIssueHF.VouType;
                                    CmdFiles.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouNo;
                                    if (OrdVoucIssueHF.ShipSer == null)
                                    {
                                        OrdVoucIssueHF.ShipSer = "";
                                    }
                                    CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = OrdVoucIssueHF.ShipSer;
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
                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { TawreedNo = OrdVoucIssueHF.TawreedNo, OrderNo = OrdVoucIssueHF.OrderNo, OrdYear = OrdVoucIssueHF.OrderYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_PaymentOrder(Ord_VoucIssueHF OrdVoucIssueHF, List<Ord_VoucIssueDF> OrdVoucIssueDF, List<UploadFileVoch> FileArchive)
        {
            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                bool ChkStores = ChkStore(OrdVoucIssueHF.StoreNo.Value, item.ItemNo);
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

            Ord_VoucIssueHF ex = db.Ord_VoucIssueHF.Where(x => x.CompNo == OrdVoucIssueHF.CompNo && x.OrderYear == OrdVoucIssueHF.OrderYear && x.OrderNo == OrdVoucIssueHF.OrderNo
           && x.TawreedNo == OrdVoucIssueHF.TawreedNo && x.VouType == OrdVoucIssueHF.VouType && x.VouNo == OrdVoucIssueHF.VouNo).FirstOrDefault();
            if (ex != null)
            {
                ex.ShipSer = OrdVoucIssueHF.ShipSer;
                ex.DocType = OrdVoucIssueHF.DocType;
                ex.StoreNo = OrdVoucIssueHF.StoreNo;
                ex.VouDate = OrdVoucIssueHF.VouDate;
                ex.Notes = OrdVoucIssueHF.Notes;
                ex.VouTypeNew = 0;
                ex.VouYear = OrdVoucIssueHF.VouDate.Value.Year;
                ex.VouNoNew = 0;
                ex.IsApproval = false;
                db.SaveChanges();
            }

            List<Ord_VoucIssueDF> IssueDF = db.Ord_VoucIssueDF.Where(x => x.CompNo == OrdVoucIssueHF.CompNo && x.OrderYear == OrdVoucIssueHF.OrderYear && x.OrderNo == OrdVoucIssueHF.OrderNo
               && x.TawreedNo == OrdVoucIssueHF.TawreedNo && x.VouType == OrdVoucIssueHF.VouType && x.VouNo == OrdVoucIssueHF.VouNo).ToList();

            if (IssueDF.Count != OrdVoucIssueDF.Count)
            {
                db.Ord_VoucIssueDF.RemoveRange(IssueDF);
                db.SaveChanges();
            }

            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                Ord_VoucIssueDF ex2 = db.Ord_VoucIssueDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
               && x.TawreedNo == item.TawreedNo && x.VouType == item.VouType && x.VouNo == item.VouNo && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (ex2 != null)
                {
                    ex2.Qty = item.Qty;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.Qty2 = item.Qty2;
                    db.SaveChanges();
                }
                else
                {
                    Ord_VoucIssueDF ex3 = new Ord_VoucIssueDF();
                    ex3.CompNo = item.CompNo;
                    ex3.OrderYear = item.OrderYear;
                    ex3.OrderNo = item.OrderNo;
                    ex3.TawreedNo = item.TawreedNo;
                    ex3.VouType = item.VouType;
                    ex3.VouNo = item.VouNo;
                    ex3.ItemNo = item.ItemNo;
                    ex3.ShipSer = item.ShipSer;
                    ex3.Qty = item.Qty;
                    ex3.TUnit = item.TUnit;
                    ex3.UnitSerial = item.UnitSerial;
                    ex3.Qty2 = item.Qty2;
                    db.Ord_VoucIssueDF.Add(ex3);
                    db.SaveChanges();
                }

            }
            List<UploadFileVoch> upFile = (List<UploadFileVoch>)Session["Files"];

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();


                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if (FileArchive.Count != 0)
                        {
                            using (SqlCommand DelCmd = new SqlCommand())
                            {
                                DelCmd.Connection = cn;
                                DelCmd.CommandText = "Ord_Web_DelVoucIssueArchiveInfo";
                                DelCmd.CommandType = CommandType.StoredProcedure;
                                DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                                DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdVoucIssueHF.OrderYear;
                                DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdVoucIssueHF.OrderNo;
                                DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdVoucIssueHF.TawreedNo;
                                DelCmd.Parameters.Add("@VouType", SqlDbType.SmallInt).Value = OrdVoucIssueHF.VouType;
                                DelCmd.Parameters.Add("@VouNo", SqlDbType.Int).Value = OrdVoucIssueHF.VouNo;
                                DelCmd.Transaction = transaction;
                                DelCmd.ExecuteNonQuery();
                            }
                        }

                        foreach (UploadFileVoch item in FileArchive)
                        {
                            UploadFileVoch fl = upFile.Find(a => a.OrderYear == item.OrderYear && a.OrderNo == item.OrderNo && a.TawreedNo == item.TawreedNo && a.VouNo == item.VouNo && a.VouType ==11 && a.FileId == Convert.ToInt32(item.FileId));

                            using (SqlCommand CmdFiles = new SqlCommand())
                            {
                                CmdFiles.Connection = cn;
                                CmdFiles.CommandText = "Ord_AddVoucIssueArchiveInfo";
                                CmdFiles.CommandType = CommandType.StoredProcedure;
                                CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                CmdFiles.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = OrdVoucIssueHF.VouType;
                                CmdFiles.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = OrdVoucIssueHF.VouNo;
                                if (OrdVoucIssueHF.ShipSer == null)
                                {
                                    OrdVoucIssueHF.ShipSer = "";
                                }
                                CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = OrdVoucIssueHF.ShipSer;
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
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { TawreedNo = OrdVoucIssueHF.TawreedNo, OrderNo = OrdVoucIssueHF.OrderNo, OrdYear = OrdVoucIssueHF.OrderYear, VouNo = OrdVoucIssueHF.VouNo, VouType = OrdVoucIssueHF.VouType, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_PaymentOrder(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType)
        {
            List<Ord_VoucIssueDF> ex1 = db.Ord_VoucIssueDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.VouType == VouType && x.VouNo == VouNo).ToList();

            db.Ord_VoucIssueDF.RemoveRange(ex1);
            db.SaveChanges();

            Ord_VoucIssueHF ex = db.Ord_VoucIssueHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.VouType == VouType && x.VouNo == VouNo).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_VoucIssueHF.Remove(ex);
                db.SaveChanges();
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ord_Web_DelVoucIssueArchiveInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdYear;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    DelCmd.Parameters.Add("@VouType", SqlDbType.SmallInt).Value = VouType;
                    DelCmd.Parameters.Add("@VouNo", SqlDbType.Int).Value = VouNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Posted_PaymentOrder(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType)
        {
            List<Ord_VoucIssueDF> VoucIssueDF = db.Ord_VoucIssueDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.VouType == VouType && x.VouNo == VouNo).ToList();

            Ord_VoucIssueHF VoucIssueHF = db.Ord_VoucIssueHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.VouType == VouType && x.VouNo == VouNo).FirstOrDefault();

            string IP = GetIpAddress();
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;

            int i = 1;
            int VouhNo = 1;

            double? Qty = 0;
            double? QtyUnit = 0;
            double? NetAmount = 0;

            InvStoresMF LinkStoreIssue = db.InvStoresMFs.Where(x => x.CompNo == company.comp_num && x.StoreNo == VoucIssueHF.StoreNo.Value).FirstOrDefault();

            OrderHF hf = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();


            //int DeptIssue = LinkStoreIssue.CashDep;
            //long AccIssue = LinkStoreIssue.CashAcc;

            int DeptIssue = new MDB().Database.SqlQuery<int>(string.Format("select TOP 1 crb_dep  from GLCRBMF where (CRB_COMP = '{0}') AND (crb_dep = '{1}') AND (crb_acc = '{2}')", company.comp_num, 99, hf.LcAccNo.Value)).FirstOrDefault();
            if (DeptIssue == 0)
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

            long AccIssue = hf.LcAccNo.Value;


            if (DeptIssue == 0 || AccIssue == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب ادخال معلومات الدائرة والحساب للمستودع " + " " + VoucIssueHF.StoreNo.Value + " من شاشة معلومات المستودع  " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The department and account information must be entered for the stores " + " " + VoucIssueHF.StoreNo.Value + " " + " From the stores information screen " }, JsonRequestBehavior.AllowGet);
                }
            }


            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");
                ds.Tables["TmpTbl"].PrimaryKey = new DataColumn[] { ds.Tables["TmpTbl"].Columns["ItemNo"], ds.Tables["TmpTbl"].Columns["ItemSer"] };


                transaction = cn.BeginTransaction();

               


                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = VoucIssueHF.VouDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 11;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = VoucIssueHF.VouDate.Value.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = VoucIssueHF.StoreNo.Value;

                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DeptIssue;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = AccIssue;
                    
                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = VoucIssueHF.DocType.Value;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = VoucIssueHF.VouNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = NetAmount;

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

                    cmd.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = VoucIssueHF.OrderYear;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = VoucIssueHF.OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@POTawreedNo", SqlDbType.VarChar)).Value = VoucIssueHF.TawreedNo;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء سند صرف من الويب" + " - " + VouNo.ToString() + " - " + " لرقم طلب شراء  " + VoucIssueHF.OrderNo + " - " + " وامر شراء برقم" + VoucIssueHF.TawreedNo;

                    cmd.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = VoucIssueHF.VouNo;
                    cmd.Parameters.Add(new SqlParameter("@CustRef", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar)).Value = DateTime.Now.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@ActID", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar)).Value = IP;
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

                    int count = 1;
                    int srl;
                    foreach (Ord_VoucIssueDF item in VoucIssueDF)
                    {
                        count = 1;
                        srl = 0;
                        Qty = item.Qty;
                        if (Qty > 0)
                        {
                            DataSet DsBatchs = new DataSet();
                            DsBatchs = LoadStoreBatchs(item.ItemNo, VoucIssueHF.StoreNo.Value, item.UnitSerial.Value, transaction, cn);
                            if (DsBatchs.Tables["BatchsTbl"].Rows.Count == 0)
                            {
                                transaction.Rollback();
                                cn.Dispose();

                                if (Language == "ar-JO")
                                {
                                    return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                }
                            }


                            foreach (DataRow DrBatch in DsBatchs.Tables["BatchsTbl"].Rows)
                            {
                                if (Qty == 0)
                                {
                                    break;
                                }
                                if (DsBatchs.Tables["BatchsTbl"].Rows.Count == count)
                                {
                                    if (Convert.ToBoolean(DrBatch["ishalt"]) != true && Convert.ToDouble(DrBatch["QtyOH"]) != 0)
                                    {
                                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                                        DrTmp.BeginEdit();
                                        DrTmp["compno"] = company.comp_num;
                                        DrTmp["VouYear"] = VoucIssueHF.VouDate.Value.Year;
                                        DrTmp["VouType"] = 11;
                                        DrTmp["VouNo"] = VouhNo;
                                        DrTmp["StoreNo"] = VoucIssueHF.StoreNo;
                                        DrTmp["ItemNo"] = item.ItemNo;
                                        DrTmp["Batch"] = DrBatch["BatchNo"];
                                        DrTmp["ItemSer"] = i;
                                        DrTmp["VouDate"] = VoucIssueHF.VouDate.Value;
                                        if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                        {
                                            if (Math.Abs(Convert.ToDouble(DrBatch["QtyOH"])) >= Qty.Value)
                                            {
                                                DrTmp["Qty"] = Qty.Value * -1;
                                                Qty = 0;
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                cn.Dispose();

                                                if (Language == "ar-JO")
                                                {
                                                    return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                                else
                                                {
                                                    return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            cn.Dispose();

                                            if (Language == "ar-JO")
                                            {
                                                return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                            }
                                        }

                                        if (item.Qty2 != 0)
                                        {
                                            DrTmp["Qty2"] = item.Qty2 * -1;
                                        }
                                        else
                                        {
                                            DrTmp["Qty2"] = 0;
                                        }
                                        DrTmp["ItemDimension"] = "0*0*0";
                                        DrTmp["Bonus"] = 0;
                                        DrTmp["TUnit"] = item.TUnit;
                                        QtyUnit = item.Qty * Convert.ToDouble(DrBatch["UnitCost"]);
                                        NetAmount += QtyUnit;
                                        DrTmp["NetSellValue"] = QtyUnit * -1;
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
                                        DrTmp["UnitSerial"] = item.UnitSerial;
                                        DrTmp["ProdOrderNo"] = 1;
                                        DrTmp["NoteDet"] = "";
                                        DrTmp.EndEdit();
                                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                                        i = i + 1;
                                        continue;
                                    }
                                    else
                                    {
                                        DataRow DrBatch1;
                                        object[] findTheseVals = new object[2];
                                        findTheseVals[0] = item.ItemNo;
                                        findTheseVals[1] = srl;
                                        DrBatch1 = ds.Tables["TmpTbl"].Rows.Find(findTheseVals);
                                        if (DrBatch1 != null)
                                        {
                                            DrTmp = DrBatch1;
                                            if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                            {
                                                DrTmp["Qty"] = (Qty.Value - Convert.ToDouble(DrBatch1["Qty"])) * -1;
                                                Qty = 0;
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                cn.Dispose();

                                                if (Language == "ar-JO")
                                                {
                                                    return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                                else
                                                {
                                                    return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                            {
                                                DrTmp = ds.Tables["TmpTbl"].NewRow();
                                                DrTmp.BeginEdit();
                                                DrTmp["compno"] = company.comp_num;
                                                DrTmp["VouYear"] = VoucIssueHF.VouDate.Value.Year;
                                                DrTmp["VouType"] = 11;
                                                DrTmp["VouNo"] = VouhNo;
                                                DrTmp["StoreNo"] = VoucIssueHF.StoreNo;
                                                DrTmp["ItemNo"] = item.ItemNo;
                                                DrTmp["Batch"] = DrBatch["BatchNo"];
                                                DrTmp["ItemSer"] = i;
                                                DrTmp["VouDate"] = VoucIssueHF.VouDate.Value;
                                                DrTmp["Qty"] = Qty.Value * -1;
                                                Qty = 0;
                                                if (item.Qty2 != 0)
                                                {
                                                    DrTmp["Qty2"] = item.Qty2 * -1;
                                                }
                                                else
                                                {
                                                    DrTmp["Qty2"] = 0;
                                                }
                                                DrTmp["ItemDimension"] = "0*0*0";
                                                DrTmp["Bonus"] = 0;
                                                DrTmp["TUnit"] = item.TUnit;
                                                QtyUnit = item.Qty * Convert.ToDouble(DrBatch["UnitCost"]);
                                                NetAmount += QtyUnit;
                                                DrTmp["NetSellValue"] = QtyUnit * -1;
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
                                                DrTmp["UnitSerial"] = item.UnitSerial;
                                                DrTmp["ProdOrderNo"] = 1;
                                                DrTmp["NoteDet"] = "";
                                                DrTmp.EndEdit();
                                                ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                                                i = i + 1;
                                                continue;
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                cn.Dispose();

                                                if (Language == "ar-JO")
                                                {
                                                    return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                                else
                                                {
                                                    return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                        }

                                    }
                                }
                                if (Convert.ToBoolean(DrBatch["ishalt"]) != true && Convert.ToDouble(DrBatch["QtyOH"]) != 0)
                                {
                                    DrTmp = ds.Tables["TmpTbl"].NewRow();
                                    DrTmp.BeginEdit();
                                    DrTmp["compno"] = company.comp_num;
                                    DrTmp["VouYear"] = VoucIssueHF.VouDate.Value.Year;
                                    DrTmp["VouType"] = 11;
                                    DrTmp["VouNo"] = VouhNo;
                                    DrTmp["StoreNo"] = VoucIssueHF.StoreNo;
                                    DrTmp["ItemNo"] = item.ItemNo;
                                    DrTmp["Batch"] = DrBatch["BatchNo"];
                                    DrTmp["ItemSer"] = i;
                                    DrTmp["VouDate"] = VoucIssueHF.VouDate.Value.ToShortDateString();
                                    if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                    {
                                        if (Math.Abs(Convert.ToDouble(DrBatch["QtyOH"])) >= Qty.Value)
                                        {
                                            DrTmp["Qty"] = Qty.Value * -1;
                                            Qty = 0;
                                        }
                                        else
                                        {
                                            Qty = Qty.Value - Convert.ToDouble(DrBatch["QtyOH"]);
                                            DrTmp["Qty"] = Convert.ToDouble(DrBatch["QtyOH"]) * -1;
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        cn.Dispose();

                                        if (Language == "ar-JO")
                                        {
                                            return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    if (item.Qty2 != 0)
                                    {
                                        DrTmp["Qty2"] = item.Qty2 * -1;
                                    }
                                    else
                                    {
                                        DrTmp["Qty2"] = 0;
                                    }
                                    DrTmp["ItemDimension"] = "0*0*0";
                                    DrTmp["Bonus"] = 0;
                                    DrTmp["TUnit"] = item.TUnit;
                                    QtyUnit = item.Qty * Convert.ToDouble(DrBatch["UnitCost"]);
                                    NetAmount += QtyUnit;
                                    DrTmp["NetSellValue"] = QtyUnit * -1;
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
                                    DrTmp["UnitSerial"] = item.UnitSerial;
                                    DrTmp["ProdOrderNo"] = 1;
                                    DrTmp["NoteDet"] = "";
                                    DrTmp.EndEdit();
                                    ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                                    srl = i;
                                    i = i + 1;
                                }
                                count++;
                            }
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

                    DataSet DSArch = new DataSet();
                    DSArch = LoadArch(OrdYear, OrderNo, TawreedNo, VouNo, VouType, transaction, cn);
                    foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                    {
                        using (SqlCommand cmdArch = new SqlCommand())
                        {
                            cmdArch.Connection = cn;
                            cmdArch.CommandText = "Invt_AddArchiveInfo";
                            cmdArch.CommandType = CommandType.StoredProcedure;
                            cmdArch.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            cmdArch.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = VoucIssueHF.VouDate.Value.Year;
                            cmdArch.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 11;
                            cmdArch.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouhNo;
                            cmdArch.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                            cmdArch.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = DrArch["Description"];
                            cmdArch.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar)).Value = DrArch["FileName"];
                            cmdArch.Parameters.Add(new SqlParameter("@DateUploded", SqlDbType.SmallDateTime)).Value = DrArch["DataUpload"];
                            cmdArch.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.Int)).Value = DrArch["FileSize"];
                            cmdArch.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.VarChar)).Value = DrArch["ContentType"];
                            cmdArch.Parameters.Add(new SqlParameter("@ArchiveData", SqlDbType.Image, 2147483647)).Value = DrArch["ArchiveData"];
                            cmdArch.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmdArch.Transaction = transaction;
                            try
                            {
                                cmdArch.ExecuteNonQuery();
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

                transaction.Commit();
                cn.Dispose();
            }

            VoucIssueHF.VouTypeNew = 11;
            VoucIssueHF.VouYear = VoucIssueHF.VouDate.Value.Year;
            VoucIssueHF.VouNoNew = VouhNo;
            VoucIssueHF.IsApproval = true;
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public string InsertFile()
        {
            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var VouNo = Request.Params["VouNo"];
            var VouType = Request.Params["VouType"];


            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.VouNo == Convert.ToInt32(VouNo) && a.VouType == Convert.ToInt32(VouType) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFileVoch();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.VouNo = Convert.ToInt32(VouNo);
                        SinglFile.VouType = Convert.ToInt32(VouType);
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
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.VouNo = Convert.ToInt32(VouNo);
                        SinglFile.VouType = Convert.ToInt32(VouType);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["Files"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, int OrderNo, string TawreedNo,int VouNo,int VouType)
        {
            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.VouNo == Convert.ToInt32(VouNo) && a.VouType == Convert.ToInt32(VouType) && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["Files"] = lstFile;
            }

            return "done";
        }
        #endregion

        #region WriteOff
        public ActionResult VocWriteOffList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView("~/Views/VoucIssue/WriteOff/VocWriteOffList.cshtml");
        }
        public ActionResult ChooseWriteOffRec(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return PartialView("~/Views/VoucIssue/WriteOff/ChooseWriteOffRec.cshtml");
        }
        public ActionResult DetailsWriteOffRec(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();
            return PartialView("~/Views/VoucIssue/WriteOff/DetailsWriteOffRec.cshtml");
        }
        public ActionResult ChooseWriteOffOrdItems(List<InvItemsMF> items, int srl, int id)
        {
            ViewBag.Serial = srl;
            ViewBag.id = id;
            return PartialView("~/Views/VoucIssue/WriteOff/ChooseWriteOffOrdItems.cshtml", items);
        }
        public ActionResult eChooseWriteOffOrdItems(List<InvItemsMF> items, int srl, int id)
        {
            ViewBag.Serial = srl;
            ViewBag.id = id;
            return PartialView("~/Views/VoucIssue/WriteOff/eChooseWriteOffOrdItems.cshtml", items);
        }
        public ActionResult AddWriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();
            return View("~/Views/VoucIssue/WriteOff/AddWriteOff.cshtml");
        }
        public ActionResult EditWriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.VouNo = VouNo;
            ViewBag.VouType = VouType;

            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchWriteOff(OrdYear, OrderNo, TawreedNo, ShipSer, VouNo, VouType, transaction, cn);

            }
            int i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.ShipSer == DrArch["ShipSer"].ToString() && a.VouNo == Convert.ToInt32(DrArch["VouNo"]) && a.VouType == 13 && a.FileId == i);
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileVoch();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.ShipSer = DrArch["ShipSer"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(13);
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
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.ShipSer = DrArch["ShipSer"].ToString();
                        SinglFile.VouNo = Convert.ToInt32(DrArch["VouNo"]);
                        SinglFile.VouType = Convert.ToInt32(13);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i = i + 1;
                    Session["Files"] = lstFile;
                }
            }

            return View("~/Views/VoucIssue/WriteOff/EditWriteOff.cshtml");
        }
        public DataSet LoadArchWriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetWriteOffArchInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@OrderYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                Cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@TawreedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
                Cmd.Parameters.Add(new SqlParameter("@ShipSer", System.Data.SqlDbType.VarChar)).Value = ShipSer;
                Cmd.Parameters.Add(new SqlParameter("@VouType", System.Data.SqlDbType.SmallInt)).Value = VouType;
                Cmd.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouNo;
                Cmd.Transaction = MyTrans;
                try
                {
                    DaCmd.SelectCommand = Cmd;
                    DaCmd.Fill(DsArch, "Arch");
                }
                catch (Exception ex)
                {
                    DsArch = new DataSet();
                }
            }
            return DsArch;
        }

        public ActionResult ViewWriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.VouNo = VouNo;
            ViewBag.VouType = VouType;
            return View("~/Views/VoucIssue/WriteOff/ViewWriteOff.cshtml");
        }
        public JsonResult Save_WriteOff(Ord_VoucIssueHF OrdVoucIssueHF, List<Ord_VoucIssueDF> OrdVoucIssueDF, List<UploadFileVoch> FileArchive)
        {
            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                bool ChkStores = ChkStore(OrdVoucIssueHF.StoreNo.Value, item.ItemNo);
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

            int VouNo = GetOrderSerial(OrdVoucIssueHF.OrderYear, OrdVoucIssueHF.StoreNo.Value, OrdVoucIssueHF.DocType.Value, OrdVoucIssueHF.VouType);
            if (VouNo == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا يمكنك الاستمرار يجب تعريف تسلسل امر اتلاف أولا" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "You Cant Proceed ,You Should Define a Serial For WriteOff Order" }, JsonRequestBehavior.AllowGet);
                }
            }

            Ord_VoucIssueHF ex = new Ord_VoucIssueHF();
            ex.CompNo = OrdVoucIssueHF.CompNo;
            ex.OrderYear = OrdVoucIssueHF.OrderYear;
            ex.OrderNo = OrdVoucIssueHF.OrderNo;
            ex.TawreedNo = OrdVoucIssueHF.TawreedNo;
            ex.VouType = OrdVoucIssueHF.VouType;
            ex.VouNo = VouNo;
            ex.ShipSer = OrdVoucIssueHF.ShipSer;
            ex.DocType = OrdVoucIssueHF.DocType;
            ex.StoreNo = OrdVoucIssueHF.StoreNo;
            ex.VouDate = OrdVoucIssueHF.VouDate;
            ex.Notes = OrdVoucIssueHF.Notes;
            ex.IsApproval = false;
            ex.VouTypeNew =0;
            ex.VouYear = OrdVoucIssueHF.VouDate.Value.Year;
            ex.VouNoNew = 0;
            db.Ord_VoucIssueHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                Ord_VoucIssueDF ex2 = new Ord_VoucIssueDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = item.OrderYear;
                ex2.OrderNo = item.OrderNo;
                ex2.TawreedNo = item.TawreedNo;
                ex2.VouType = item.VouType;
                ex2.VouNo = VouNo;
                ex2.ItemNo = item.ItemNo;
                ex2.ShipSer = item.ShipSer;
                ex2.Qty = item.Qty;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.Qty2 = item.Qty2;
                db.Ord_VoucIssueDF.Add(ex2);
                db.SaveChanges();
            }

            List<UploadFileVoch> upFile = (List<UploadFileVoch>)Session["Files"];

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if (FileArchive != null)
                        {
                            foreach (UploadFileVoch item in FileArchive)
                            {
                                UploadFileVoch fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.VouNo == item.VouNo && x.VouType == item.VouType && x.FileId == item.FileId).FirstOrDefault();

                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddVoucIssueArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = item.VouType;
                                    CmdFiles.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouNo;
                                    if (OrdVoucIssueHF.ShipSer == null)
                                    {
                                        OrdVoucIssueHF.ShipSer = "";
                                    }
                                    CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = item.ShipSer;
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
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { TawreedNo = OrdVoucIssueHF.TawreedNo, OrderNo = OrdVoucIssueHF.OrderNo, OrdYear = OrdVoucIssueHF.OrderYear, ShipSer = OrdVoucIssueHF.ShipSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_WriteOff(Ord_VoucIssueHF OrdVoucIssueHF, List<Ord_VoucIssueDF> OrdVoucIssueDF, List<UploadFileVoch> FileArchive)
        {
            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                bool ChkStores = ChkStore(OrdVoucIssueHF.StoreNo.Value, item.ItemNo);
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

            Ord_VoucIssueHF ex = db.Ord_VoucIssueHF.Where(x => x.CompNo == OrdVoucIssueHF.CompNo && x.OrderYear == OrdVoucIssueHF.OrderYear && x.OrderNo == OrdVoucIssueHF.OrderNo
           && x.TawreedNo == OrdVoucIssueHF.TawreedNo && x.ShipSer == OrdVoucIssueHF.ShipSer && x.VouType == OrdVoucIssueHF.VouType && x.VouNo == OrdVoucIssueHF.VouNo).FirstOrDefault();
            if (ex != null)
            {
                ex.DocType = OrdVoucIssueHF.DocType;
                ex.StoreNo = OrdVoucIssueHF.StoreNo;
                ex.VouDate = OrdVoucIssueHF.VouDate;
                ex.Notes = OrdVoucIssueHF.Notes;
                ex.IsApproval = false;
                ex.VouTypeNew = 0;
                ex.VouYear = OrdVoucIssueHF.VouDate.Value.Year;
                ex.VouNoNew = 0;
                db.SaveChanges();
            }

            List<Ord_VoucIssueDF> IssueDF = db.Ord_VoucIssueDF.Where(x => x.CompNo == OrdVoucIssueHF.CompNo && x.OrderYear == OrdVoucIssueHF.OrderYear && x.OrderNo == OrdVoucIssueHF.OrderNo
              && x.TawreedNo == OrdVoucIssueHF.TawreedNo && x.ShipSer == OrdVoucIssueHF.ShipSer && x.VouType == OrdVoucIssueHF.VouType && x.VouNo == OrdVoucIssueHF.VouNo).ToList();

            if (IssueDF.Count != OrdVoucIssueDF.Count)
            {
                db.Ord_VoucIssueDF.RemoveRange(IssueDF);
                db.SaveChanges();
            }

            foreach (Ord_VoucIssueDF item in OrdVoucIssueDF)
            {
                Ord_VoucIssueDF ex2 = db.Ord_VoucIssueDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
               && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.VouType == item.VouType && x.VouNo == item.VouNo && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (ex2 != null)
                {
                    ex2.Qty = item.Qty;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.Qty2 = item.Qty2;
                    db.SaveChanges();
                }
                else
                {
                    Ord_VoucIssueDF ex3 = new Ord_VoucIssueDF();
                    ex3.CompNo = item.CompNo;
                    ex3.OrderYear = item.OrderYear;
                    ex3.OrderNo = item.OrderNo;
                    ex3.TawreedNo = item.TawreedNo;
                    ex3.VouType = item.VouType;
                    ex3.VouNo = item.VouNo;
                    ex3.ItemNo = item.ItemNo;
                    ex3.ShipSer = item.ShipSer;
                    ex3.Qty = item.Qty;
                    ex3.TUnit = item.TUnit;
                    ex3.UnitSerial = item.UnitSerial;
                    ex3.Qty2 = item.Qty2;
                    db.Ord_VoucIssueDF.Add(ex3);
                    db.SaveChanges();
                }

            }
            List<UploadFileVoch> upFile = (List<UploadFileVoch>)Session["Files"];

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();


                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if (FileArchive.Count != 0)
                        {
                            using (SqlCommand DelCmd = new SqlCommand())
                            {
                                DelCmd.Connection = cn;
                                DelCmd.CommandText = "Ord_Web_DelWriteOffArchInfo";
                                DelCmd.CommandType = CommandType.StoredProcedure;
                                DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                                DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdVoucIssueHF.OrderYear;
                                DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdVoucIssueHF.OrderNo;
                                DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdVoucIssueHF.TawreedNo;
                                DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = OrdVoucIssueHF.ShipSer;
                                DelCmd.Parameters.Add("@VouType", SqlDbType.SmallInt).Value = OrdVoucIssueHF.VouType;
                                DelCmd.Parameters.Add("@VouNo", SqlDbType.Int).Value = OrdVoucIssueHF.VouNo;
                                DelCmd.Transaction = transaction;
                                DelCmd.ExecuteNonQuery();
                            }
                        }

                        foreach (UploadFileVoch item in FileArchive)
                        {
                            UploadFileVoch fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.VouNo == item.VouNo && x.VouType == item.VouType && x.FileId == item.FileId).FirstOrDefault();

                            using (SqlCommand CmdFiles = new SqlCommand())
                            {
                                CmdFiles.Connection = cn;
                                CmdFiles.CommandText = "Ord_AddVoucIssueArchiveInfo";
                                CmdFiles.CommandType = CommandType.StoredProcedure;
                                CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                CmdFiles.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = item.VouType;
                                CmdFiles.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = item.VouNo;
                                if (OrdVoucIssueHF.ShipSer == null)
                                {
                                    OrdVoucIssueHF.ShipSer = "";
                                }
                                CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = item.ShipSer;
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
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { TawreedNo = OrdVoucIssueHF.TawreedNo, OrderNo = OrdVoucIssueHF.OrderNo, OrdYear = OrdVoucIssueHF.OrderYear, ShipSer = OrdVoucIssueHF.ShipSer, VouNo = OrdVoucIssueHF.VouNo, VouType = OrdVoucIssueHF.VouType, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_WriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType)
        {
            List<Ord_VoucIssueDF> ex1 = db.Ord_VoucIssueDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.VouType == VouType && x.VouNo == VouNo).ToList();

            db.Ord_VoucIssueDF.RemoveRange(ex1);
            db.SaveChanges();

            Ord_VoucIssueHF ex = db.Ord_VoucIssueHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.VouType == VouType && x.VouNo == VouNo).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_VoucIssueHF.Remove(ex);
                db.SaveChanges();
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ord_Web_DelWriteOffArchInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdYear;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = ShipSer;
                    DelCmd.Parameters.Add("@VouType", SqlDbType.SmallInt).Value = VouType;
                    DelCmd.Parameters.Add("@VouNo", SqlDbType.Int).Value = VouNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Posted_WriteOff(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType)
        {
            List<Ord_VoucIssueDF> VoucIssueDF = db.Ord_VoucIssueDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.VouType == VouType && x.VouNo == VouNo).ToList();

            Ord_VoucIssueHF VoucIssueHF = db.Ord_VoucIssueHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.VouType == VouType && x.VouNo == VouNo).FirstOrDefault();

            string IP = GetIpAddress();
            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;

            int i = 1;
            int VouhNo = 1;

            double? Qty = 0;
            double? QtyUnit = 0;
            double? NetAmount = 0;



            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");


                transaction = cn.BeginTransaction();


                


                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = VoucIssueHF.VouDate.Value.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 13;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = VoucIssueHF.VouDate.Value.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = VoucIssueHF.StoreNo.Value;
                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = VoucIssueHF.DocType.Value;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = VoucIssueHF.VouNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = NetAmount;

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

                    cmd.Parameters.Add(new SqlParameter("@PurchOrderYear", SqlDbType.SmallInt)).Value = VoucIssueHF.OrderYear;
                    cmd.Parameters.Add(new SqlParameter("@PurchOrderNo", SqlDbType.Int)).Value = VoucIssueHF.OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@POTawreedNo", SqlDbType.VarChar)).Value = VoucIssueHF.TawreedNo;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepBatchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PurchRecNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@IsLc", SqlDbType.Bit)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustPurOrderYear", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PriceList", SqlDbType.Int)).Value = 0;

                    cmd.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء سند اتلاف من الويب" + " - " + VouNo.ToString() + " - " + " لرقم طلب شراء  " + VoucIssueHF.OrderNo + " - " + " وامر شراء برقم" + VoucIssueHF.TawreedNo;


                    cmd.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = VoucIssueHF.VouNo;
                    cmd.Parameters.Add(new SqlParameter("@CustRef", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar)).Value = DateTime.Now.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@ActID", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@IpAddress", SqlDbType.VarChar)).Value = IP;
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

                    foreach (Ord_VoucIssueDF item in VoucIssueDF)
                    {
                        Qty = item.Qty;
                        if (Qty > 0)
                        {
                            DataSet DsBatchs = new DataSet();
                            DsBatchs = LoadStoreBatchs(item.ItemNo, VoucIssueHF.StoreNo.Value, item.UnitSerial.Value, transaction, cn);
                            if (DsBatchs.Tables["BatchsTbl"].Rows.Count == 0)
                            {
                                transaction.Rollback();
                                cn.Dispose();

                                if (Language == "ar-JO")
                                {
                                    return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + item.ItemNo + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { error = "Can't execute because of a problem  in quantity of item " + " " + item.ItemNo + " " + " That Exist In Store No " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                }
                            }

                            DataRow DrBatch1;
                            object[] findTheseVals = new object[2];
                            findTheseVals[0] = item.ItemNo;
                            findTheseVals[1] = item.ShipSer;
                            DrBatch1 = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);

                            if (DrBatch1 != null)
                            {
                                if (Convert.ToBoolean(DrBatch1["ishalt"]) != true && Convert.ToDouble(DrBatch1["QtyOH"]) != 0)
                                {
                                    DrTmp = ds.Tables["TmpTbl"].NewRow();
                                    DrTmp.BeginEdit();
                                    DrTmp["compno"] = company.comp_num;
                                    DrTmp["VouYear"] = VoucIssueHF.VouDate.Value.Year;
                                    DrTmp["VouType"] = 13;
                                    DrTmp["VouNo"] = VouhNo;
                                    DrTmp["StoreNo"] = VoucIssueHF.StoreNo;
                                    DrTmp["ItemNo"] = item.ItemNo;
                                    DrTmp["Batch"] = DrBatch1["BatchNo"];
                                    DrTmp["ItemSer"] = i;
                                    DrTmp["VouDate"] = VoucIssueHF.VouDate.Value.ToShortDateString();
                                    if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                    {
                                        if (Math.Abs(Convert.ToDouble(DrBatch1["QtyOH"])) >= Qty.Value)
                                        {
                                            DrTmp["Qty"] = Qty.Value * -1;
                                            Qty = 0;
                                        }
                                        else
                                        {
                                            Qty = Qty.Value - Convert.ToDouble(DrBatch1["QtyOH"]);
                                            DrTmp["Qty"] = Convert.ToDouble(DrBatch1["QtyOH"]) * -1;
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        cn.Dispose();

                                        if (Language == "ar-JO")
                                        {
                                            return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            return Json(new { error = "Can't execute because of a problem  in quantity of item  That Exist In Store No  " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    if (item.Qty2 != 0)
                                    {
                                        DrTmp["Qty2"] = item.Qty2 * -1;
                                    }
                                    else
                                    {
                                        DrTmp["Qty2"] = 0;
                                    }
                                    DrTmp["ItemDimension"] = "0*0*0";
                                    DrTmp["Bonus"] = 0;
                                    DrTmp["TUnit"] = item.TUnit;
                                    QtyUnit = item.Qty * Convert.ToDouble(DrBatch1["UnitCost"]);
                                    NetAmount += QtyUnit;
                                    DrTmp["NetSellValue"] = QtyUnit * -1;
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
                                    DrTmp["UnitSerial"] = item.UnitSerial;
                                    DrTmp["ProdOrderNo"] = 1;
                                    DrTmp["NoteDet"] = "";
                                    DrTmp.EndEdit();
                                    ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                                    i = i + 1;
                                }
                            }
                            foreach (DataRow DrBatch in DsBatchs.Tables["BatchsTbl"].Rows)
                            {
                                if (Qty == 0)
                                {
                                    break;
                                }
                                if (Convert.ToBoolean(DrBatch["ishalt"]) != true && Convert.ToDouble(DrBatch["QtyOH"]) != 0)
                                {
                                    DrTmp = ds.Tables["TmpTbl"].NewRow();
                                    DrTmp.BeginEdit();
                                    DrTmp["compno"] = company.comp_num;
                                    DrTmp["VouYear"] = VoucIssueHF.VouDate.Value.Year;
                                    DrTmp["VouType"] = 13;
                                    DrTmp["VouNo"] = VouhNo;
                                    DrTmp["StoreNo"] = VoucIssueHF.StoreNo;
                                    DrTmp["ItemNo"] = item.ItemNo;
                                    DrTmp["Batch"] = DrBatch["BatchNo"];
                                    DrTmp["ItemSer"] = i;
                                    DrTmp["VouDate"] = VoucIssueHF.VouDate.Value.ToShortDateString();
                                    if (CheckMinusQty(VoucIssueHF.StoreNo.Value, transaction, cn) == true)
                                    {
                                        if (Math.Abs(Convert.ToDouble(DrBatch["QtyOH"])) >= Qty.Value)
                                        {
                                            DrTmp["Qty"] = Qty.Value * -1;
                                            Qty = 0;
                                        }
                                        else
                                        {
                                            Qty = Qty.Value - Convert.ToDouble(DrBatch["QtyOH"]);
                                            DrTmp["Qty"] = Convert.ToDouble(DrBatch["QtyOH"]) * -1;
                                        }
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        cn.Dispose();

                                        if (Language == "ar-JO")
                                        {
                                            return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + "الموجودة في مستودع رقم" + " " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            return Json(new { error = "Can't execute because of a problem  in quantity of item  That Exist In Store No  " + VoucIssueHF.StoreNo }, JsonRequestBehavior.AllowGet);
                                        }
                                    }

                                    if (item.Qty2 != 0)
                                    {
                                        DrTmp["Qty2"] = item.Qty2 * -1;
                                    }
                                    else
                                    {
                                        DrTmp["Qty2"] = 0;
                                    }
                                    DrTmp["ItemDimension"] = "0*0*0";
                                    DrTmp["Bonus"] = 0;
                                    DrTmp["TUnit"] = item.TUnit;
                                    QtyUnit = item.Qty * Convert.ToDouble(DrBatch["UnitCost"]);
                                    NetAmount += QtyUnit;
                                    DrTmp["NetSellValue"] = QtyUnit * -1;
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
                                    DrTmp["UnitSerial"] = item.UnitSerial;
                                    DrTmp["ProdOrderNo"] = 1;
                                    DrTmp["NoteDet"] = "";
                                    DrTmp.EndEdit();
                                    ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                                    i = i + 1;
                                }
                            }
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

                    DataSet DSArch = new DataSet();
                    DSArch = LoadArchWriteOff(OrdYear, OrderNo, TawreedNo,ShipSer, VouNo, VouType, transaction, cn);
                    foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                    {
                        using (SqlCommand cmdArch = new SqlCommand())
                        {
                            cmdArch.Connection = cn;
                            cmdArch.CommandText = "Invt_AddArchiveInfo";
                            cmdArch.CommandType = CommandType.StoredProcedure;
                            cmdArch.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            cmdArch.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = VoucIssueHF.VouDate.Value.Year;
                            cmdArch.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 13;
                            cmdArch.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouhNo;
                            cmdArch.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                            cmdArch.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar)).Value = DrArch["Description"];
                            cmdArch.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar)).Value = DrArch["FileName"];
                            cmdArch.Parameters.Add(new SqlParameter("@DateUploded", SqlDbType.SmallDateTime)).Value = DrArch["DataUpload"];
                            cmdArch.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.Int)).Value = DrArch["FileSize"];
                            cmdArch.Parameters.Add(new SqlParameter("@ContentType", SqlDbType.VarChar)).Value = DrArch["ContentType"];
                            cmdArch.Parameters.Add(new SqlParameter("@ArchiveData", SqlDbType.Image, 2147483647)).Value = DrArch["ArchiveData"];
                            cmdArch.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmdArch.Transaction = transaction;
                            try
                            {
                                cmdArch.ExecuteNonQuery();
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

                transaction.Commit();
                cn.Dispose();
            }

            VoucIssueHF.VouTypeNew = 13;
            VoucIssueHF.VouYear = VoucIssueHF.VouDate.Value.Year;
            VoucIssueHF.VouNoNew = VouhNo;
            VoucIssueHF.IsApproval = true;
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public string InsertFileWriteOff()
        {
            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var ShipSer = Request.Params["ShipSer"];
            var VouNo = Request.Params["VouNo"];
            var VouType = Request.Params["VouType"];

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.ShipSer == ShipSer && a.VouNo == Convert.ToInt32(VouNo) && a.VouType == Convert.ToInt32(VouType) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFileVoch();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.ShipSer = ShipSer;
                        SinglFile.VouNo = Convert.ToInt32(VouNo);
                        SinglFile.VouType = Convert.ToInt32(VouType);
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
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.ShipSer = ShipSer;
                        SinglFile.VouNo = Convert.ToInt32(VouNo);
                        SinglFile.VouType = Convert.ToInt32(VouType);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["Files"] = lstFile;
            }
            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFileWriteOff(int Id, int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int VouNo, int VouType)
        {
            List<UploadFileVoch> lstFile = null;
            if (Session["Files"] == null)
            {
                lstFile = new List<UploadFileVoch>();
            }
            else
            {
                lstFile = (List<UploadFileVoch>)Session["Files"];
            }
            UploadFileVoch SinglFile = new UploadFileVoch();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.VouNo == Convert.ToInt32(VouNo) && a.VouType == Convert.ToInt32(VouType) && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["Files"] = lstFile;
            }

            return "done";
        }
        #endregion

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

            if(bStore == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public int GetOrderSerial(int PYear, int StoreNo,int DocType, int VouType)
        {
            int OrdSerial = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    SqlTransaction transaction;
                    cn.Open();

                    transaction = cn.BeginTransaction();

                    Cmd.Connection = cn;
                    Cmd.CommandText = "Sales_GetVouSer";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@SalesTrType", SqlDbType.SmallInt).Value = VouType;
                    Cmd.Parameters.Add("@BookYear", SqlDbType.SmallInt).Value = PYear;
                    Cmd.Parameters.Add("@StoreNo", SqlDbType.Int).Value = StoreNo;
                    Cmd.Parameters.Add("@CaCr", SqlDbType.SmallInt).Value = 3;
                    Cmd.Parameters.Add("@DocType", SqlDbType.SmallInt).Value = DocType;
                    Cmd.Parameters.Add(new SqlParameter("@CurrSr", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    Cmd.Transaction = transaction;
                    try
                    {
                        Cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return OrdSerial = 0;
                    }


                    if (Convert.ToInt32(Cmd.Parameters["@CurrSr"].Value) == -10)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return OrdSerial = 0;
                    }

                    OrdSerial = Convert.ToInt32(Cmd.Parameters["@CurrSr"].Value);
                    transaction.Commit();
                }
            }

            return OrdSerial;
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
                Cmd.Parameters.Add("@VouType", SqlDbType.TinyInt).Value = 11;
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

            if(ChkRCount != 0)
            {
                AllowMinus = false;
            }
            else
            {
                AllowMinus = true;
            }
            return AllowMinus;
        }
        public DataSet LoadArch(int OrdYear, int OrderNo, string TawreedNo, int VouNo, int VouType, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetVoucIssueArchInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@OrderYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                Cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@TawreedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
                Cmd.Parameters.Add(new SqlParameter("@VouType", System.Data.SqlDbType.SmallInt)).Value = VouType;
                Cmd.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int)).Value = VouNo;
                Cmd.Transaction = MyTrans;
                try
                {
                    DaCmd.SelectCommand = Cmd;
                    DaCmd.Fill(DsArch, "Arch");
                }
                catch (Exception ex)
                {
                    DsArch = new DataSet();
                }
            }
            return DsArch;
        }
        
    }
}