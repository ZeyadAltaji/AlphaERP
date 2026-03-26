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
    public class ReleaseOrdersController : controller
    {
        // GET: ReleaseOrders
        public ActionResult Index()
        {
            Session["FilesReleaseOrd"] = null;
            return View();
        }
        public ActionResult ReleaseOrdersList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult ChooseReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            return PartialView();
        }
        public ActionResult DetailsReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            return PartialView();
        }

        public ActionResult AddReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN,int StoreNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            ViewBag.StoreNo = StoreNo;
            return View();

        }

        public ActionResult EditReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            ViewBag.ReleaseOrdId = ReleaseOrdId;

            List<UploadFileReleaseOrders> lstFile = null;
            if (Session["FilesReleaseOrd"] == null)
            {
                lstFile = new List<UploadFileReleaseOrders>();
            }
            else
            {
                lstFile = (List<UploadFileReleaseOrders>)Session["FilesReleaseOrd"];
            }
            UploadFileReleaseOrders SinglFile = new UploadFileReleaseOrders();
            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchReleaseOrder(OrdYear, OrderNo, TawreedNo, InboundSer, InboundGRN, ReleaseOrdId, transaction, cn);
            }

            short i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) &&
                    a.TawreedNo == DrArch["TawreedNo"].ToString() && a.InboundSer == DrArch["InboundSer"].ToString() && a.InboundGRN == DrArch["InboundGRN"].ToString()
                    && a.ReleaseOrdId == Convert.ToInt32(DrArch["ReleaseOrdId"])  && a.FileId == Convert.ToInt32(i));

                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileReleaseOrders();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.InboundSer = DrArch["InboundSer"].ToString();
                        SinglFile.InboundGRN = DrArch["InboundGRN"].ToString();
                        SinglFile.ReleaseOrdId = Convert.ToInt32(DrArch["ReleaseOrdId"]);
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
                        SinglFile.InboundSer = DrArch["InboundSer"].ToString();
                        SinglFile.InboundGRN = DrArch["InboundGRN"].ToString();
                        SinglFile.ReleaseOrdId = Convert.ToInt32(DrArch["ReleaseOrdId"]);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i++;
                    Session["FilesReleaseOrd"] = lstFile;
                }
            }


            return View();
        }
        public DataSet LoadArchReleaseOrder(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetOrdReleaseOrdersArchiveInfo";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = TawreedNo;
                cmd.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = InboundSer;
                cmd.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = InboundGRN;
                cmd.Parameters.Add(new SqlParameter("@ReleaseOrdId", SqlDbType.Int)).Value = ReleaseOrdId;
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
        public ActionResult ViewReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.InboundSer = InboundSer;
            ViewBag.InboundGRN = InboundGRN;
            ViewBag.ReleaseOrdId = ReleaseOrdId;
            return View();
        }
        public string InsertFile()
        {
            List<UploadFileReleaseOrders> lstFile = null;
            if (Session["FilesReleaseOrd"] == null)
            {
                lstFile = new List<UploadFileReleaseOrders>();
            }
            else
            {
                lstFile = (List<UploadFileReleaseOrders>)Session["FilesReleaseOrd"];
            }
            UploadFileReleaseOrders SinglFile = new UploadFileReleaseOrders();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var InboundSer = Request.Params["InboundSer"];
            var InboundGRN = Request.Params["InboundGRN"];
            var ReleaseOrdId = Request.Params["ReleaseOrdId"];

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == Convert.ToInt32(OrderNo)
                && a.TawreedNo == TawreedNo && a.InboundSer == InboundSer && a.InboundGRN == InboundGRN && a.ReleaseOrdId == Convert.ToInt32(ReleaseOrdId) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFileReleaseOrders();
                        SinglFile.OrderYear = Convert.ToInt16(OrderYear);
                        SinglFile.OrderNo = Convert.ToInt32(OrderNo);
                        SinglFile.TawreedNo = TawreedNo;
                        SinglFile.InboundSer = InboundSer;
                        SinglFile.InboundGRN = InboundGRN;
                        SinglFile.ReleaseOrdId = Convert.ToInt32(ReleaseOrdId);
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
                        SinglFile.InboundSer = InboundSer;
                        SinglFile.InboundGRN = InboundGRN;
                        SinglFile.ReleaseOrdId = Convert.ToInt32(ReleaseOrdId);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["FilesReleaseOrd"] = lstFile;
            }
            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId)
        {
            List<UploadFileReleaseOrders> lstFile = null;
            if (Session["FilesReleaseOrd"] == null)
            {
                lstFile = new List<UploadFileReleaseOrders>();
            }
            else
            {
                lstFile = (List<UploadFileReleaseOrders>)Session["FilesReleaseOrd"];
            }
            UploadFileReleaseOrders SinglFile = new UploadFileReleaseOrders();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == Convert.ToInt32(OrderNo)
               && a.TawreedNo == TawreedNo && a.InboundSer == InboundSer && a.InboundGRN == InboundGRN && a.ReleaseOrdId == Convert.ToInt32(ReleaseOrdId) && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesReleaseOrd"] = lstFile;
            }

            return "done";
        }

        public JsonResult Save_ReleaseOrders(Ord_ReleaseOrdersHF ReleaseOrdersHF, List<Ord_ReleaseOrdersDF> ReleaseOrdersDF, List<UploadFileReleaseOrders> FileArchive)
        {
            int Id = db.Ord_ReleaseOrdersHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == ReleaseOrdersHF.OrderYear && x.OrderNo == ReleaseOrdersHF.OrderNo
            && x.TawreedNo == ReleaseOrdersHF.TawreedNo && x.InboundSer == ReleaseOrdersHF.InboundSer && x.InboundGRN == ReleaseOrdersHF.InboundGRN).OrderByDescending(o => o.ReleaseOrdId).Select(z => z.ReleaseOrdId).FirstOrDefault();
            Id = Id + 1;
            Ord_ReleaseOrdersHF ex = new Ord_ReleaseOrdersHF();
            ex.CompNo = ReleaseOrdersHF.CompNo;
            ex.OrderYear = ReleaseOrdersHF.OrderYear;
            ex.OrderNo = ReleaseOrdersHF.OrderNo;
            ex.TawreedNo = ReleaseOrdersHF.TawreedNo;
            ex.InboundSer = ReleaseOrdersHF.InboundSer;
            ex.InboundGRN = ReleaseOrdersHF.InboundGRN;
            ex.ReleaseOrdId = Id;
            ex.DocType = ReleaseOrdersHF.DocType;
            ex.StoreNo = ReleaseOrdersHF.StoreNo;
            ex.Notes = ReleaseOrdersHF.Notes;
            ex.IsApproval = false;
            db.Ord_ReleaseOrdersHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_ReleaseOrdersDF item in ReleaseOrdersDF)
            {
                Ord_ReleaseOrdersDF ex2 = new Ord_ReleaseOrdersDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = ReleaseOrdersHF.OrderYear;
                ex2.OrderNo = ReleaseOrdersHF.OrderNo;
                ex2.TawreedNo = ReleaseOrdersHF.TawreedNo;
                ex2.InboundSer = ReleaseOrdersHF.InboundSer;
                ex2.InboundGRN = ReleaseOrdersHF.InboundGRN;
                ex2.ReleaseOrdId = Id;
                ex2.ItemSrl = item.ItemSrl;
                ex2.ItemNo = item.ItemNo;
                ex2.batchNo = item.batchNo;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.ReservedQty = item.ReservedQty;
                ex2.FreeQty = item.FreeQty;
                db.Ord_ReleaseOrdersDF.Add(ex2);
                db.SaveChanges();
            }
            List<UploadFileReleaseOrders> upFile = (List<UploadFileReleaseOrders>)Session["FilesReleaseOrd"];

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
                            foreach (UploadFileReleaseOrders item in FileArchive)
                            {
                                UploadFileReleaseOrders fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
                                && x.TawreedNo == item.TawreedNo && x.InboundGRN == item.InboundGRN && x.InboundSer == item.InboundSer && x.ReleaseOrdId == item.ReleaseOrdId && x.FileId == item.FileId).FirstOrDefault();

                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddOrdReleaseOrdersArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;

                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = item.InboundSer;
                                    CmdFiles.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = item.InboundGRN;
                                    CmdFiles.Parameters.Add(new SqlParameter("@ReleaseOrdId", SqlDbType.Int)).Value = Id;

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
                                        fl.ReleaseOrdId = Id;
                                    }
                                    catch (Exception exc)
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
                    }
                }
            }

            return Json(new { TawreedNo = ReleaseOrdersHF.TawreedNo, OrdNo = ReleaseOrdersHF.OrderNo, OrdYear = ReleaseOrdersHF.OrderYear, InboundSer = ReleaseOrdersHF.InboundSer, InboundGRN = ReleaseOrdersHF.InboundGRN, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit_ReleaseOrders(Ord_ReleaseOrdersHF ReleaseOrdersHF, List<Ord_ReleaseOrdersDF> ReleaseOrdersDF, List<UploadFileReleaseOrders> FileArchive)
        {
            Ord_ReleaseOrdersHF ex = db.Ord_ReleaseOrdersHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == ReleaseOrdersHF.OrderYear && x.OrderNo == ReleaseOrdersHF.OrderNo
            && x.TawreedNo == ReleaseOrdersHF.TawreedNo && x.InboundSer == ReleaseOrdersHF.InboundSer && x.InboundGRN == ReleaseOrdersHF.InboundGRN && x.ReleaseOrdId == ReleaseOrdersHF.ReleaseOrdId).FirstOrDefault();

            if (ex != null)
            {
                ex.DocType = ReleaseOrdersHF.DocType;
                ex.StoreNo = ReleaseOrdersHF.StoreNo;
                ex.Notes = ReleaseOrdersHF.Notes;
                ex.IsApproval = false;
                db.SaveChanges();
            }

            List<Ord_ReleaseOrdersDF> exDel = db.Ord_ReleaseOrdersDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == ReleaseOrdersHF.OrderYear && x.OrderNo == ReleaseOrdersHF.OrderNo
            && x.TawreedNo == ReleaseOrdersHF.TawreedNo && x.InboundSer == ReleaseOrdersHF.InboundSer && x.InboundGRN == ReleaseOrdersHF.InboundGRN && x.ReleaseOrdId == ReleaseOrdersHF.ReleaseOrdId).ToList();

            if(exDel != null)
            {
                db.Ord_ReleaseOrdersDF.RemoveRange(exDel);
                db.SaveChanges();
            }

            foreach (Ord_ReleaseOrdersDF item in ReleaseOrdersDF)
            {
                Ord_ReleaseOrdersDF ex2 = new Ord_ReleaseOrdersDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = ReleaseOrdersHF.OrderYear;
                ex2.OrderNo = ReleaseOrdersHF.OrderNo;
                ex2.TawreedNo = ReleaseOrdersHF.TawreedNo;
                ex2.InboundSer = ReleaseOrdersHF.InboundSer;
                ex2.InboundGRN = ReleaseOrdersHF.InboundGRN;
                ex2.ReleaseOrdId = ReleaseOrdersHF.ReleaseOrdId;
                ex2.ItemSrl = item.ItemSrl;
                ex2.ItemNo = item.ItemNo;
                ex2.batchNo = item.batchNo;
                ex2.TUnit = item.TUnit;
                ex2.UnitSerial = item.UnitSerial;
                ex2.ReservedQty = item.ReservedQty;
                ex2.FreeQty = item.FreeQty;
                db.Ord_ReleaseOrdersDF.Add(ex2);
                db.SaveChanges();
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                List<UploadFileReleaseOrders> upFile = (List<UploadFileReleaseOrders>)Session["FilesReleaseOrd"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        using (SqlCommand DelCmd = new SqlCommand())
                        {
                            DelCmd.Connection = cn;
                            DelCmd.CommandText = "Ord_Web_DelOrdReleaseOrderArchiveInfo";
                            DelCmd.CommandType = CommandType.StoredProcedure;
                            DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                            DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = ReleaseOrdersHF.OrderYear;
                            DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = ReleaseOrdersHF.OrderNo;
                            DelCmd.Parameters.Add("@TawreedNo", SqlDbType.NVarChar).Value = ReleaseOrdersHF.TawreedNo;
                            DelCmd.Parameters.Add("@InboundSer", SqlDbType.NVarChar).Value = ReleaseOrdersHF.InboundSer;
                            DelCmd.Parameters.Add("@InboundGRN", SqlDbType.NVarChar).Value = ReleaseOrdersHF.InboundGRN;
                            DelCmd.Parameters.Add("@ReleaseOrdId", SqlDbType.Int).Value = ReleaseOrdersHF.ReleaseOrdId;

                            DelCmd.Transaction = transaction;
                            DelCmd.ExecuteNonQuery();
                        }

                        if (FileArchive != null)
                        {

                            foreach (UploadFileReleaseOrders item in FileArchive)
                            {
                                
                                UploadFileReleaseOrders fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo
                                && x.TawreedNo == item.TawreedNo && x.InboundGRN == item.InboundGRN && x.InboundSer == item.InboundSer && x.ReleaseOrdId == item.ReleaseOrdId && x.FileId == item.FileId).FirstOrDefault();


                                if (fl != null)
                                {
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddOrdReleaseOrdersArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;

                                        CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.NVarChar)).Value = item.TawreedNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@InboundSer", SqlDbType.NVarChar)).Value = item.InboundSer;
                                        CmdFiles.Parameters.Add(new SqlParameter("@InboundGRN", SqlDbType.NVarChar)).Value = item.InboundGRN;
                                        CmdFiles.Parameters.Add(new SqlParameter("@ReleaseOrdId", SqlDbType.Int)).Value = item.ReleaseOrdId;

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


            return Json(new { TawreedNo = ReleaseOrdersHF.TawreedNo, OrdNo = ReleaseOrdersHF.OrderNo, OrdYear = ReleaseOrdersHF.OrderYear, InboundSer = ReleaseOrdersHF.InboundSer, InboundGRN = ReleaseOrdersHF.InboundGRN, ReleaseOrdId = ReleaseOrdersHF.ReleaseOrdId, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Del_ReleaseOrders(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId)
        {
            
            List<Ord_ReleaseOrdersDF> exDel = db.Ord_ReleaseOrdersDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN && x.ReleaseOrdId == ReleaseOrdId).ToList();

            if (exDel != null)
            {
                db.Ord_ReleaseOrdersDF.RemoveRange(exDel);
                db.SaveChanges();
            }

            Ord_ReleaseOrdersHF ex = db.Ord_ReleaseOrdersHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
            && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN && x.ReleaseOrdId == ReleaseOrdId).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_ReleaseOrdersHF.Remove(ex);
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
                    DelCmd.CommandText = "Ord_Web_DelOrdReleaseOrderArchiveInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdYear;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.NVarChar).Value = TawreedNo;
                    DelCmd.Parameters.Add("@InboundSer", SqlDbType.NVarChar).Value = InboundSer;
                    DelCmd.Parameters.Add("@InboundGRN", SqlDbType.NVarChar).Value = InboundGRN;
                    DelCmd.Parameters.Add("@ReleaseOrdId", SqlDbType.Int).Value = ReleaseOrdId;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Free_ReleaseOrder(int OrdYear, int OrderNo, string TawreedNo, string InboundSer, string InboundGRN, int ReleaseOrdId)
        {
            Ord_ReleaseOrdersHF ReleaseOrdersHF = db.Ord_ReleaseOrdersHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN && x.ReleaseOrdId == ReleaseOrdId).FirstOrDefault();

            

            List<Ord_ReleaseOrdersDF> ReleaseOrdersDF = db.Ord_ReleaseOrdersDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN && x.ReleaseOrdId == ReleaseOrdId).ToList();

            OrdPreOrderHF PreOrderHF = db.OrdPreOrderHFs.Where(x => x.CompNo == company.comp_num && x.Year == OrdYear && x.OrderNo == OrderNo).FirstOrDefault();
            OrderHF ordhf = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();

            Ord_InboundManagementHF exInboundManagementHF = new MDB().Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();


            OrdRecHF recHF = db.OrdRecHFs.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.InboundYear == exInboundManagementHF.InboundGRNDate.Value.Year && x.InboundNo == InboundGRN && x.InboundSer == InboundSer).FirstOrDefault();

            DataSet ds = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;

            int OrdNo = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.Invt_ReservedStockDF  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");

                transaction = cn.BeginTransaction();
                long VouNoRes;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvReserveHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 22;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = ReleaseOrdersHF.StoreNo;
                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@ReservedYear", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ReservedRef", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;

                    cmd.Parameters.Add(new SqlParameter("@SalesOrderYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "";

                    cmd.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@LogId", SqlDbType.BigInt)).Direction = ParameterDirection.Output;

                    cmd.Parameters.Add(new SqlParameter("@CashDep", SqlDbType.Int)).Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@ReservedType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@ToStoreNo", SqlDbType.Int)).Value = ReleaseOrdersHF.StoreNo;

                    cmd.Parameters.Add(new SqlParameter("@InvTransType", SqlDbType.SmallInt)).Value = 2;

                    cmd.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;

                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@OrdRecYear", SqlDbType.SmallInt)).Value = recHF.RecYear;
                    cmd.Parameters.Add(new SqlParameter("@OrdRecNo", SqlDbType.Int)).Value = recHF.RecNo;

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
                    VouNoRes = Convert.ToInt32(cmd.Parameters["@UsedVouNo"].Value);

                    int i = 1;

                    foreach (Ord_ReleaseOrdersDF item in ReleaseOrdersDF)
                    {
                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["compno"] = company.comp_num;
                        DrTmp["VouYear"] = DateTime.Now.Year;
                        DrTmp["VouType"] = 22;
                        DrTmp["VouNo"] = VouNoRes;
                        DrTmp["StoreNo"] = ReleaseOrdersHF.StoreNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["BatchNo"] = item.batchNo;
                        DrTmp["ItemSer"] = i;
                        DrTmp["VouDate"] = DateTime.Now;
                        DrTmp["ResUpToDate"] = DateTime.Now;
                        DrTmp["ResNotes"] = "FG Reserve";
                        DrTmp["TransferToIssue"] = false;

                        DrTmp["ResQty"] = item.FreeQty;
                        DrTmp["ResQty2"] = 0;
                        DrTmp["ItemStatus"] = 1;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                        i = i + 1;
                    }

                    using (SqlCommand cmd1 = new SqlCommand())
                    {
                        cmd1.Connection = cn;
                        cmd1.CommandText = "InvT_AddInvReserveDF";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd1.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt, 4, "VouYear"));
                        cmd1.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt, 4, "VouType"));
                        cmd1.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.Int, 9, "VouNo"));
                        cmd1.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 9, "StoreNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                        cmd1.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "BatchNo"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 4, "ItemSer"));
                        cmd1.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime, 8, "VouDate"));
                        cmd1.Parameters.Add(new SqlParameter("@ResQty", SqlDbType.Float, 8, "ResQty"));
                        cmd1.Parameters.Add(new SqlParameter("@ResQty2", SqlDbType.Float, 8, "ResQty2"));
                        cmd1.Parameters.Add(new SqlParameter("@ResUpToDate", SqlDbType.SmallDateTime, 8, "ResUpToDate"));
                        cmd1.Parameters.Add(new SqlParameter("@ResNotes", SqlDbType.VarChar, 225, "ResNotes"));
                        cmd1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        cmd1.Parameters.Add(new SqlParameter("@TransferToIssue", SqlDbType.Bit, 1, "TransferToIssue"));
                        cmd1.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.BigInt)).Value = 1;
                        cmd1.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        cmd1.Parameters.Add(new SqlParameter("@Tunit", SqlDbType.VarChar)).Value = "";
                        cmd1.Parameters.Add(new SqlParameter("@UnitCodeSr", SqlDbType.SmallInt, 2, "UnitSerial"));
                        cmd1.Parameters.Add(new SqlParameter("@ItemStatus", SqlDbType.SmallInt, 2, "ItemStatus"));

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
                //using (SqlCommand cmd = new SqlCommand())
                //{
                //    cmd.Connection = cn;
                //    cmd.CommandText = "InvT_AddReservedOrderHF";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                //    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                //    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = 0;
                //    cmd.Parameters.Add(new SqlParameter("@RecYear", SqlDbType.SmallInt)).Value = recHF.RecYear;
                //    cmd.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = recHF.RecNo;
                //    cmd.Parameters.Add(new SqlParameter("@OrderDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;
                //    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = ReleaseOrdersHF.StoreNo;
                //    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = ordhf.Vend_Dept;
                //    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = ordhf.VendorNo;
                //    cmd.Parameters.Add(new SqlParameter("@EntryDate", SqlDbType.SmallDateTime)).Value = DateTime.Now;
                //    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                //    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "طلب فك حجز بضاعة من الويب";
                //    cmd.Parameters.Add(new SqlParameter("@UsedVouNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                //    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                //    cmd.Transaction = transaction;
                //    try
                //    {
                //        cmd.ExecuteNonQuery();
                //    }
                //    catch (Exception ex)
                //    {
                //        transaction.Rollback();
                //        cn.Dispose();
                //        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                //    }
                //    if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) != 0)
                //    {
                //        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == -10)
                //        {
                //            transaction.Rollback();
                //            cn.Dispose();
                //            if (Language == "ar-JO")
                //            {
                //                return Json(new { error = "يجب تعريف تسلسل للسند حتى تتم عملية التخزين للسند" }, JsonRequestBehavior.AllowGet);
                //            }
                //            else
                //            {
                //                return Json(new { error = "You Must Define Voucher Serial To Continue With The Save Operation" }, JsonRequestBehavior.AllowGet);
                //            }
                //        }
                //    }
                //    OrdNo = Convert.ToInt32(cmd.Parameters["@UsedVouNo"].Value);
                //}
                //int i = 1;
                //foreach (Ord_ReleaseOrdersDF item in ReleaseOrdersDF)
                //{
                //    DrTmp = ds.Tables["TmpTbl"].NewRow();
                //    DrTmp.BeginEdit();
                //    DrTmp["CompNo"] = company.comp_num;
                //    DrTmp["OrderNo"] = OrdNo;
                //    DrTmp["OrderYear"] = DateTime.Now.Year;
                //    DrTmp["RecYear"] = recHF.RecYear;
                //    DrTmp["RecNo"] = recHF.RecNo;
                //    DrTmp["ItemNo"] = item.ItemNo;
                //    DrTmp["ItemSer"] = i;
                //    DrTmp["Batch"] = item.batchNo;
                //    DrTmp["UnitSerial"] = item.UnitSerial;
                //    DrTmp["TUnit"] = item.TUnit;
                //    DrTmp["ResQty"] = item.FreeQty;
                //    DrTmp["ResQty2"] = 0;
                //    DrTmp["ResNotes"] = "";
                //    ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                //    i = i + 1;
                //}
                //using (SqlCommand cmdD = new SqlCommand())
                //{
                //    cmdD.Connection = cn;
                //    cmdD.CommandText = "Invt_AddReservedOrderDF";
                //    cmdD.CommandType = CommandType.StoredProcedure;
                //    cmdD.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                //    cmdD.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "OrderYear"));
                //    cmdD.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9, "OrderNo"));
                //    cmdD.Parameters.Add(new SqlParameter("@RecYear", SqlDbType.SmallInt, 4, "RecYear"));
                //    cmdD.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int, 9, "RecNo"));
                //    cmdD.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                //    cmdD.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 4, "ItemSer"));
                //    cmdD.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20, "Batch"));
                //    cmdD.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                //    cmdD.Parameters.Add(new SqlParameter("@TUnit", SqlDbType.VarChar, 5, "TUnit"));

                //    cmdD.Parameters.Add(new SqlParameter("@ResQty", SqlDbType.Float, 8, "ResQty"));
                //    cmdD.Parameters.Add(new SqlParameter("@ResQty2", SqlDbType.Float, 8, "ResQty2"));
                //    cmdD.Parameters.Add(new SqlParameter("@ResNotes", SqlDbType.VarChar, 250, "ResNotes"));
                //    cmdD.Transaction = transaction;
                //    DaPR.InsertCommand = cmdD;
                //    try
                //    {
                //        DaPR.Update(ds, "TmpTbl");
                //    }
                //    catch (Exception ex)
                //    {
                //        transaction.Rollback();
                //        cn.Dispose();
                //        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                //    }
                //}


                //bool InsertWorkFlow = AddWorkFlowLog(30, PreOrderHF.BusUnitID.Value, DateTime.Now.Year,84, OrdNo, 1, 0, transaction, cn);
                //if (InsertWorkFlow == false)
                //{
                //    transaction.Rollback();
                //    cn.Dispose();
                //    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                //}

                transaction.Commit();
                cn.Dispose();
            }

            ReleaseOrdersHF.IsApproval = true;
            int count = 0;
            List<Ord_ReleaseOrdersDF> ReleaseOrdersDFs = db.Ord_ReleaseOrdersDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).ToList();
            foreach (Ord_ReleaseOrdersDF item in ReleaseOrdersDFs)
            {
                double TotalQty = db.Ord_ReleaseOrdersDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
             && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN && x.ItemNo == item.ItemNo && x.ItemSrl == item.ItemSrl).Sum(o => o.FreeQty.Value);

                if(item.ReservedQty == TotalQty)
                {
                    count++;
                }
            }
           
            if(ReleaseOrdersDFs.Count == count)
            {
                Ord_InboundManagementHF InboundManagementHF = db.Ord_InboundManagementHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear && x.OrderNo == OrderNo
           && x.TawreedNo == TawreedNo && x.InboundSer == InboundSer && x.InboundGRN == InboundGRN).FirstOrDefault();
                InboundManagementHF.IsReleaseOrder = true;
                InboundManagementHF.IsReserveFreeReleaseOrder = true;
            }

            db.SaveChanges();
            return Json(new { TawreedNo = TawreedNo, OrdNo = OrderNo, OrdYear = OrdYear, InboundSer = InboundSer, InboundGRN = InboundGRN, Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }

        public bool AddWorkFlowLog(int FID, int BU, int TrYear, int TrType, int TrNo, int FStat, double? TrAmnt, SqlTransaction MyTrans, SqlConnection co)
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
                Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 10).Value = company.comp_num;
                Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 10).Value = TrYear;
                Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = TrNo;
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