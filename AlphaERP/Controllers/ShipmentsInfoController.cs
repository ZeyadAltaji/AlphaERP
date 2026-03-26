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
    public class ShipmentsInfoController : controller
    {
        // GET: ShipmentsInfo
        public ActionResult Index()
        {
            Session["FilesShipmentsInfo"] = null;
            return View();
        }
        public ActionResult PurchaseOrderInfoList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult ChooseShipmentsInfo(int OrdYear, int OrderNo,string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;

            return PartialView();
        }
        public ActionResult DetailsShipmentsInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return View();
        }
        public ActionResult AddShipments(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return View();
        }
        public string InsertFile()
        {
            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesShipmentsInfo"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesShipmentsInfo"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var ShipSer = Request.Params["ShipSer"];


            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.ShipSer == ShipSer && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFileOrdRec();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.ShipSer = ShipSer;
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
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["FilesShipmentsInfo"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesShipmentsInfo"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesShipmentsInfo"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == OrderNo && a.TawreedNo == TawreedNo && a.ShipSer == ShipSer && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesShipmentsInfo"] = lstFile;
            }
            return "done";
        }
        public JsonResult Save_ShipmentsInfo(Ord_OrderShippingHF OrderShippingHF, List<Ord_OrderShippingDF> OrderShippingDF, List<UploadFileOrdRec> FileArchive)
        {
            Ord_OrderShippingHF ex1 = db.Ord_OrderShippingHF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer).FirstOrDefault();

            if (ex1 == null)
            {
                Ord_OrderShippingHF ex = new Ord_OrderShippingHF();
                ex.CompNo = OrderShippingHF.CompNo;
                ex.OrderYear = OrderShippingHF.OrderYear;
                ex.OrderNo = OrderShippingHF.OrderNo;
                ex.TawreedNo = OrderShippingHF.TawreedNo;
                ex.ShipSer = OrderShippingHF.ShipSer;
                ex.ShippingDate = OrderShippingHF.ShippingDate;
                ex.ArrivalDate = OrderShippingHF.ArrivalDate;
                ex.ShippingPolicyNo = OrderShippingHF.ShippingPolicyNo;
                ex.ClearanceInvNo = OrderShippingHF.ClearanceInvNo;
                ex.ClearanceCompany = OrderShippingHF.ClearanceCompany;
                ex.Transporter = OrderShippingHF.Transporter;
                ex.TransportInvNo = OrderShippingHF.TransportInvNo;
                ex.ShippingNotes = OrderShippingHF.ShippingNotes;
                db.Ord_OrderShippingHF.Add(ex);
                db.SaveChanges();
            }

            foreach (Ord_OrderShippingDF item in OrderShippingDF)
            {
                Ord_OrderShippingDF OrdShipDF = db.Ord_OrderShippingDF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer && x.ItemNo == item.ItemNo).FirstOrDefault();
                if(OrdShipDF == null)
                {
                    Ord_OrderShippingDF ex2 = new Ord_OrderShippingDF();
                    ex2.CompNo = item.CompNo;
                    ex2.OrderYear = item.OrderYear;
                    ex2.OrderNo = item.OrderNo;
                    ex2.TawreedNo = item.TawreedNo;
                    ex2.ShipSer = item.ShipSer;
                    ex2.ItemNo = item.ItemNo;
                    ex2.ShippingQty = item.ShippingQty;
                    ex2.Qty2 = item.Qty2;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.ETA = item.ETA;
                    ex2.Marketing = item.Marketing;
                    ex2.tqm = item.tqm;
                    ex2.QualityControl = item.QualityControl;
                    ex2.SupplierInvoiceNo = item.SupplierInvoiceNo;
                    ex2.Amount = item.Amount;
                    ex2.CurrCode = item.CurrCode;
                    ex2.ShippingDate = item.ShippingDate;
                    ex2.JFDA = item.JFDA;
                    ex2.SampleDate = item.SampleDate;
                    ex2.JFDAApprovalDate = item.JFDAApprovalDate;
                    ex2.Declaration = item.Declaration;
                    ex2.ActualArrivalDate = item.ActualArrivalDate;
                    ex2.AWBBL = item.AWBBL;
                    ex2.Transporter = item.Transporter;
                    ex2.TransporterInvoice = item.TransporterInvoice;

                    db.Ord_OrderShippingDF.Add(ex2);
                    db.SaveChanges();
                }
            }


            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                List<UploadFileOrdRec> upFile = (List<UploadFileOrdRec>)Session["FilesShipmentsInfo"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if (FileArchive != null)
                        {
                            foreach (UploadFileOrdRec item in FileArchive)
                            {
                                UploadFileOrdRec fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.FileId == item.FileId).FirstOrDefault();

                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddOrdShipmentsInfoArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.NVarChar)).Value = item.ShipSer;

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


            return Json(new {  TawreedNo = OrderShippingHF.TawreedNo, OrdNo = OrderShippingHF.OrderNo, OrdYear = OrderShippingHF.OrderYear, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_ShipmentsInfo(Ord_OrderShippingHF OrderShippingHF, List<Ord_OrderShippingDF> OrderShippingDF, List<UploadFileOrdRec> FileArchive)
        {
            Ord_OrderShippingHF ex1 = db.Ord_OrderShippingHF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer).FirstOrDefault();

            if (ex1 != null)
            {
                ex1.ShippingDate = OrderShippingHF.ShippingDate;
                ex1.ArrivalDate = OrderShippingHF.ArrivalDate;
                ex1.ShippingPolicyNo = OrderShippingHF.ShippingPolicyNo;
                ex1.ClearanceInvNo = OrderShippingHF.ClearanceInvNo;
                ex1.ClearanceCompany = OrderShippingHF.ClearanceCompany;
                ex1.Transporter = OrderShippingHF.Transporter;
                ex1.TransportInvNo = OrderShippingHF.TransportInvNo;
                ex1.ShippingNotes = OrderShippingHF.ShippingNotes;
                db.SaveChanges();
            }
            List<Ord_OrderShippingDF> ShippingDF = db.Ord_OrderShippingDF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer).ToList();
            
            foreach (Ord_OrderShippingDF item1 in ShippingDF)
            {
                Ord_OrderShippingDF OrdShipDF = db.Ord_OrderShippingDF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer && x.ItemNo == item1.ItemNo).FirstOrDefault();
                if (OrdShipDF != null)
                {
                    db.Ord_OrderShippingDF.Remove(OrdShipDF);
                    db.SaveChanges();
                }
            }

            foreach (Ord_OrderShippingDF item in OrderShippingDF)
            {
                Ord_OrderShippingDF OrdShipDF = db.Ord_OrderShippingDF.Where(x => x.CompNo == OrderShippingHF.CompNo && x.OrderYear == OrderShippingHF.OrderYear
            && x.OrderNo == OrderShippingHF.OrderNo && x.TawreedNo == OrderShippingHF.TawreedNo && x.ShipSer == OrderShippingHF.ShipSer && x.ItemNo == item.ItemNo).FirstOrDefault();
                if (OrdShipDF == null)
                {
                    Ord_OrderShippingDF ex2 = new Ord_OrderShippingDF();
                    ex2.CompNo = item.CompNo;
                    ex2.OrderYear = item.OrderYear;
                    ex2.OrderNo = item.OrderNo;
                    ex2.TawreedNo = item.TawreedNo;
                    ex2.ShipSer = item.ShipSer;
                    ex2.ItemNo = item.ItemNo;
                    ex2.ShippingQty = item.ShippingQty;
                    ex2.Qty2 = item.Qty2;
                    ex2.TUnit = item.TUnit;
                    ex2.UnitSerial = item.UnitSerial;
                    ex2.ETA = item.ETA;
                    ex2.Marketing = item.Marketing;
                    ex2.tqm = item.tqm;
                    ex2.QualityControl = item.QualityControl;
                    ex2.SupplierInvoiceNo = item.SupplierInvoiceNo;
                    ex2.Amount = item.Amount;
                    ex2.CurrCode = item.CurrCode;
                    ex2.ShippingDate = item.ShippingDate;
                    ex2.JFDA = item.JFDA;
                    ex2.SampleDate = item.SampleDate;
                    ex2.JFDAApprovalDate = item.JFDAApprovalDate;
                    ex2.Declaration = item.Declaration;
                    ex2.ActualArrivalDate = item.ActualArrivalDate;
                    ex2.AWBBL = item.AWBBL;
                    ex2.Transporter = item.Transporter;
                    ex2.TransporterInvoice = item.TransporterInvoice;
                    db.Ord_OrderShippingDF.Add(ex2);
                    db.SaveChanges();
                }
            }


            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();



                List<UploadFileOrdRec> upFile = (List<UploadFileOrdRec>)Session["FilesShipmentsInfo"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        using (SqlCommand DelCmd = new SqlCommand())
                        {
                            DelCmd.Connection = cn;
                            DelCmd.CommandText = "Ord_Web_DelShipmentsInfoArchiveInfo";
                            DelCmd.CommandType = CommandType.StoredProcedure;
                            DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                            DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrderShippingHF.OrderYear;
                            DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderShippingHF.OrderNo;
                            DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrderShippingHF.TawreedNo;
                            DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = OrderShippingHF.ShipSer;
                            DelCmd.Transaction = transaction;
                            DelCmd.ExecuteNonQuery();
                        }

                        if (FileArchive != null)
                        {

                            foreach (UploadFileOrdRec item in FileArchive)
                            {
                                UploadFileOrdRec fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.FileId == item.FileId).FirstOrDefault();

                                if (fl != null)
                                {
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddOrdShipmentsInfoArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
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
                }

                transaction.Commit();
                cn.Dispose();

            }

            return Json(new { TawreedNo = OrderShippingHF.TawreedNo, OrdNo = OrderShippingHF.OrderNo, OrdYear = OrderShippingHF.OrderYear, ShipSer = OrderShippingHF.ShipSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_ShipmentsInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            List<Ord_OrderShippingDF> ex1 = db.Ord_OrderShippingDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer).ToList();

            db.Ord_OrderShippingDF.RemoveRange(ex1);
            db.SaveChanges();

            Ord_OrderShippingHF ex = db.Ord_OrderShippingHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_OrderShippingHF.Remove(ex);
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
                    DelCmd.CommandText = "Ord_Web_DelShipmentsInfoArchiveInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdYear;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = ShipSer;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShipmentsView(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return View();
        }
        public ActionResult eShipmentsInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();

            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesShipmentsInfo"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesShipmentsInfo"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();
            DataSet DSArch = new DataSet();

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdShipmentsInfo(OrdYear, OrderNo, TawreedNo, ShipSer, transaction, cn);
            }
            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.ShipSer == DrArch["ShipSer"].ToString() && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileOrdRec();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = Convert.ToString(DrArch["TawreedNo"]);
                        SinglFile.ShipSer = Convert.ToString(DrArch["ShipSer"]);
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
                        SinglFile.TawreedNo = Convert.ToString(DrArch["TawreedNo"]);
                        SinglFile.ShipSer = Convert.ToString(DrArch["ShipSer"]);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesShipmentsInfo"] = lstFile;
                }
            }

            return View();
        }
        public DataSet LoadArchOrdShipmentsInfo(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetOrdShipmentsInfoArchiveInfo";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = TawreedNo;
                cmd.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.NVarChar)).Value = ShipSer;
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
        public JsonResult LoadUnitItem(string ItemNo, string srl)
        {
            DataTable Dt = new DataTable();
            decimal QtyOH = 0;
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
                    cmd.CommandText = "InvT_GetItemQtyOH";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", System.Data.SqlDbType.VarChar, 20)).Value = ItemNo;
                    QtyOH = (decimal)cmd.ExecuteScalar();
                }
            }


            return Json(new { unit = InvUnitCode, Qty = QtyOH,Id = srl }, JsonRequestBehavior.AllowGet);
        }
    }
}