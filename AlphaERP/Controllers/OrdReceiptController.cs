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
    public class OrdReceiptController : controller
    {
        // GET: OrdReceipt
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrdReceiptList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult ChooseOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            return PartialView();
        }
        public ActionResult DetailsOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();

            return PartialView();
        }
        public ActionResult AddOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            string srl = ShipSer.Replace(TawreedNo, "");
            srl = srl.Replace("/ ", TawreedNo);
            ViewBag.srl = srl.TrimStart();
            return View();

        }
        public DataSet LoadArchOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Ord_GetOrdReceiptArchiveInfo";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrdYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                cmd.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = ShipSer;
                cmd.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = RecNo;
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
        public ActionResult EditOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.RecNo = RecNo;

            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesOrdReceipt"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesOrdReceipt"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();

            DataSet DSArch = new DataSet();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                DSArch = LoadArchOrdReceipt(OrdYear, OrderNo, TawreedNo, ShipSer, RecNo, transaction, cn);

            }
            int i = 1;
            if (DSArch.Tables["Arch"].Rows != null)
            {
                foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                {
                    SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(DrArch["OrderYear"]) && a.OrderNo == Convert.ToInt32(DrArch["OrderNo"]) && a.TawreedNo == DrArch["TawreedNo"].ToString() && a.ShipSer == DrArch["ShipSer"].ToString() && a.RecNo == Convert.ToInt32(DrArch["RecNo"]) && a.FileId == Convert.ToInt32(i));
                    if (SinglFile == null)
                    {
                        byte[] FileByte = (byte[])DrArch["ArchiveData"];

                        SinglFile = new UploadFileOrdRec();
                        SinglFile.OrderYear = Convert.ToInt16(DrArch["OrderYear"]);
                        SinglFile.OrderNo = Convert.ToInt32(DrArch["OrderNo"]);
                        SinglFile.TawreedNo = DrArch["TawreedNo"].ToString();
                        SinglFile.ShipSer = DrArch["ShipSer"].ToString();
                        SinglFile.RecNo = Convert.ToInt32(DrArch["RecNo"]);
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
                        SinglFile.RecNo = Convert.ToInt32(DrArch["RecNo"]);
                        SinglFile.FileId = Convert.ToInt32(i);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = Convert.ToInt32(DrArch["FileSize"]);
                        SinglFile.ContentType = DrArch["ContentType"].ToString();
                        SinglFile.FileName = DrArch["FileName"].ToString();
                    }

                    i = i + 1;
                    Session["FilesOrdReceipt"] = lstFile;
                }
            }

            return View();
        }
        public ActionResult ViewOrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.ShipSer = ShipSer;
            ViewBag.RecNo = RecNo;

            return View();
        }
        public string InsertFile()
        {
            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesOrdReceipt"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesOrdReceipt"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();
            var Id = Request.Params["Id"];
            var OrderYear = Request.Params["OrderYear"];
            var OrderNo = Request.Params["OrderNo"];
            var TawreedNo = Request.Params["TawreedNo"];
            var ShipSer = Request.Params["ShipSer"];
            var RecNo = Request.Params["RecNo"];


            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrderYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.ShipSer == ShipSer && a.RecNo == Convert.ToInt32(RecNo) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile.RecNo = Convert.ToInt32(RecNo);
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
                        SinglFile.RecNo = Convert.ToInt32(RecNo);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["FilesOrdReceipt"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }
        public string RemoveFile(int Id, int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo)
        {
            List<UploadFileOrdRec> lstFile = null;
            if (Session["FilesOrdReceipt"] == null)
            {
                lstFile = new List<UploadFileOrdRec>();
            }
            else
            {
                lstFile = (List<UploadFileOrdRec>)Session["FilesOrdReceipt"];
            }
            UploadFileOrdRec SinglFile = new UploadFileOrdRec();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.OrderYear == Convert.ToInt16(OrdYear) && a.OrderNo == Convert.ToInt32(OrderNo) && a.TawreedNo == TawreedNo && a.ShipSer == ShipSer && a.RecNo == Convert.ToInt32(RecNo) && a.FileId == Convert.ToInt32(Id));
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["FilesOrdReceipt"] = lstFile;
            }

            return "done";
        }
        public int GetOrderSerial(int PYear,int RecNo)
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
                    Cmd.CommandText = "Ord_Web_GetOrdSerials";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@RecYear", SqlDbType.SmallInt).Value = PYear;
                    Cmd.Parameters.Add("@RecNo", SqlDbType.SmallInt).Value = RecNo;
                    Cmd.Parameters.Add(new SqlParameter("@NewRecNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
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
                    

                    if (Convert.ToInt32(Cmd.Parameters["@NewRecNo"].Value) <= 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                       return OrdSerial = 0;
                    }

                    OrdSerial = Convert.ToInt32(Cmd.Parameters["@NewRecNo"].Value);
                    transaction.Commit();

                }
            }
          
            return OrdSerial;
        }
        public JsonResult Save_OrdReceipt(Ord_OrdReceiptHF OrdReceiptHF, List<Ord_OrdReceiptDF> OrdReceiptDF, List<UploadFileOrdRec> FileArchive)
        {
            foreach (Ord_OrdReceiptDF item in OrdReceiptDF)
            {
                bool ChkStores = ChkStore(OrdReceiptHF.StoreNo.Value, item.ItemNo);
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

            int RecNo = GetOrderSerial(OrdReceiptHF.OrderYear, OrdReceiptHF.RecNo);
            if (RecNo == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا يمكنك الاستمرار يجب تعريف تسلسل لمحاضر الاستلام أولا" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "You Cant Proceed ,You Should Define a Serial For Receipt Documents" }, JsonRequestBehavior.AllowGet);
                }
            }
            Ord_OrdReceiptHF ex = new Ord_OrdReceiptHF();
            ex.CompNo = OrdReceiptHF.CompNo;
            ex.OrderYear = OrdReceiptHF.OrderYear;
            ex.OrderNo = OrdReceiptHF.OrderNo;
            ex.TawreedNo = OrdReceiptHF.TawreedNo;
            ex.ShipSer = OrdReceiptHF.ShipSer;
            ex.RecNo = RecNo;
            ex.RecDate = OrdReceiptHF.RecDate;
            ex.CarNo = OrdReceiptHF.CarNo;
            ex.DriverName = OrdReceiptHF.DriverName;
            ex.VendorNo = OrdReceiptHF.VendorNo;
            ex.StoreNo = OrdReceiptHF.StoreNo;
            ex.Notes = OrdReceiptHF.Notes;
            ex.IsApproval = false;
            db.Ord_OrdReceiptHF.Add(ex);
            db.SaveChanges();

            foreach (Ord_OrdReceiptDF item in OrdReceiptDF)
            {
                Ord_OrdReceiptDF ex2 = new Ord_OrdReceiptDF();
                ex2.CompNo = item.CompNo;
                ex2.OrderYear = item.OrderYear;
                ex2.OrderNo = item.OrderNo;
                ex2.TawreedNo = item.TawreedNo;
                ex2.ShipSer = item.ShipSer;
                ex2.RecNo = RecNo;
                ex2.ItemNo = item.ItemNo;
                ex2.PortQty = item.PortQty;
                ex2.PortTUnit = item.PortTUnit;
                ex2.PortUnitSerial = item.PortUnitSerial;
                ex2.PortQty2 = item.PortQty2;
                db.Ord_OrdReceiptDF.Add(ex2);
                db.SaveChanges();
            }
            List<UploadFileOrdRec> upFile = (List<UploadFileOrdRec>)Session["FilesOrdReceipt"];

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
                            foreach (UploadFileOrdRec item in FileArchive)
                            {
                                UploadFileOrdRec fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.RecNo == item.RecNo && x.FileId == item.FileId).FirstOrDefault();
                                
                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddOrdRecArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                    CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                    CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = item.ShipSer;
                                    CmdFiles.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = RecNo;
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
                                        fl.RecNo = RecNo;
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

            return Json(new { TawreedNo = OrdReceiptHF.TawreedNo, OrdNo = OrdReceiptHF.OrderNo, OrdYear = OrdReceiptHF.OrderYear, ShipSer = OrdReceiptHF.ShipSer, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Edit_OrdReceipt(Ord_OrdReceiptHF OrdReceiptHF, List<Ord_OrdReceiptDF> OrdReceiptDF, List<UploadFileOrdRec> FileArchive)
        {

            foreach (Ord_OrdReceiptDF item in OrdReceiptDF)
            {
                bool ChkStores = ChkStore(OrdReceiptHF.StoreNo.Value, item.ItemNo);
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

            Ord_OrdReceiptHF ex = db.Ord_OrdReceiptHF.Where(x => x.CompNo == OrdReceiptHF.CompNo && x.OrderYear == OrdReceiptHF.OrderYear
            && x.OrderNo == OrdReceiptHF.OrderNo && x.TawreedNo == OrdReceiptHF.TawreedNo && x.ShipSer == OrdReceiptHF.ShipSer && x.RecNo == OrdReceiptHF.RecNo).FirstOrDefault();

            if(ex != null)
            {
                ex.RecDate = OrdReceiptHF.RecDate;
                ex.CarNo = OrdReceiptHF.CarNo;
                ex.DriverName = OrdReceiptHF.DriverName;
                ex.DocType = OrdReceiptHF.DocType;
                ex.CurrNo = OrdReceiptHF.CurrNo;
                ex.VendorNo = OrdReceiptHF.VendorNo;
                ex.StoreNo = OrdReceiptHF.StoreNo;
                ex.Notes = OrdReceiptHF.Notes;
                ex.IsApproval = false;
                db.SaveChanges();
            }

            foreach (Ord_OrdReceiptDF item in OrdReceiptDF)
            {
                Ord_OrdReceiptDF ex2 = db.Ord_OrdReceiptDF.Where(x => x.CompNo == item.CompNo && x.OrderYear == item.OrderYear
            && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.RecNo == item.RecNo && x.ItemNo == item.ItemNo).FirstOrDefault();

                ex2.RecQty = item.RecQty;
                ex2.RecTUnit = item.RecTUnit;
                ex2.RecUnitSerial = item.RecUnitSerial;
                ex2.RecQty2 = item.RecQty2;

                ex2.CantTransport = item.CantTransport;
                ex2.Shortage = item.Shortage;
                ex2.lostQty = item.lostQty;
                ex2.WeightBreaker = item.WeightBreaker;

                db.SaveChanges();
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                

                List<UploadFileOrdRec> upFile = (List<UploadFileOrdRec>)Session["FilesOrdReceipt"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        using (SqlCommand DelCmd = new SqlCommand())
                        {
                            DelCmd.Connection = cn;
                            DelCmd.CommandText = "Ord_Web_DelOrdRecArchiveInfo";
                            DelCmd.CommandType = CommandType.StoredProcedure;
                            DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                            DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdReceiptHF.OrderYear;
                            DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrdReceiptHF.OrderNo;
                            DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = OrdReceiptHF.TawreedNo;
                            DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = OrdReceiptHF.ShipSer;
                            DelCmd.Parameters.Add("@RecNo", SqlDbType.Int).Value = OrdReceiptHF.RecNo;
                            DelCmd.Transaction = transaction;
                            DelCmd.ExecuteNonQuery();
                        }

                        if (FileArchive != null)
                        {

                            foreach (UploadFileOrdRec item in FileArchive)
                            {
                                UploadFileOrdRec fl = upFile.Where(x => x.OrderYear == item.OrderYear && x.OrderNo == item.OrderNo && x.TawreedNo == item.TawreedNo && x.ShipSer == item.ShipSer && x.RecNo == item.RecNo && x.FileId == item.FileId).FirstOrDefault();

                                if (fl != null)
                                {
                                    using (SqlCommand CmdFiles = new SqlCommand())
                                    {
                                        CmdFiles.Connection = cn;
                                        CmdFiles.CommandText = "Ord_AddOrdRecArchiveInfo";
                                        CmdFiles.CommandType = CommandType.StoredProcedure;
                                        CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = item.OrderYear;
                                        CmdFiles.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = item.OrderNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = item.TawreedNo;
                                        CmdFiles.Parameters.Add(new SqlParameter("@ShipSer", SqlDbType.VarChar)).Value = item.ShipSer;
                                        CmdFiles.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = item.RecNo;
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

            return Json(new { TawreedNo = OrdReceiptHF.TawreedNo, OrdNo = OrdReceiptHF.OrderNo, OrdYear = OrdReceiptHF.OrderYear, ShipSer = OrdReceiptHF.ShipSer, RecNo = OrdReceiptHF.RecNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Del_OrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo)
        {
            List<Ord_OrdReceiptDF> ex1 = db.Ord_OrdReceiptDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.RecNo == RecNo).ToList();

            db.Ord_OrdReceiptDF.RemoveRange(ex1);
            db.SaveChanges();

            Ord_OrdReceiptHF ex = db.Ord_OrdReceiptHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.RecNo == RecNo).FirstOrDefault();

            if (ex != null)
            {
                db.Ord_OrdReceiptHF.Remove(ex);
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
                    DelCmd.CommandText = "Ord_Web_DelOrdRecArchiveInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@OrderYear", SqlDbType.SmallInt).Value = OrdYear;
                    DelCmd.Parameters.Add("@OrderNo", SqlDbType.Int).Value = OrderNo;
                    DelCmd.Parameters.Add("@TawreedNo", SqlDbType.VarChar).Value = TawreedNo;
                    DelCmd.Parameters.Add("@ShipSer", SqlDbType.VarChar).Value = ShipSer;
                    DelCmd.Parameters.Add("@RecNo", SqlDbType.Int).Value = RecNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public double GetLastExRate(int CCode, DateTime VDate)
        {
            double CurrRatio = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    SqlTransaction transaction;
                    cn.Open();

                    transaction = cn.BeginTransaction();

                    Cmd.Connection = cn;
                    Cmd.CommandText = "Gln_GetCurrExchRatio";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@CurrCode", SqlDbType.SmallInt).Value = CCode;
                    Cmd.Parameters.Add("@VDate", SqlDbType.SmallDateTime).Value = VDate;
                    Cmd.Transaction = transaction;
                    try
                    {
                        SqlDataReader rdr = Cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                CurrRatio = Convert.ToDouble(rdr["CurrRatio"]);
                            }
                        }
                        rdr.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        CurrRatio = 0;
                    }
                }
            }

            return CurrRatio;
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
        public JsonResult Posted_OrdReceipt(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo)
        {
            List<Ord_OrdReceiptDF> ReceiptDF = db.Ord_OrdReceiptDF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
           && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.RecNo == RecNo).ToList();

            foreach (Ord_OrdReceiptDF item in ReceiptDF)
            {
                if(item.RecQty == null)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "لا يمكن ترحيل الحركة قبل ادخال كميات الاستلام الفعلي" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "The transaction cannot be posted before the actual receipt quantities are entered" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            int gLcDept = 0;
            long gLcAcc = 0;


            Ord_OrdReceiptHF ReceiptHF = db.Ord_OrdReceiptHF.Where(x => x.CompNo == company.comp_num && x.OrderYear == OrdYear
            && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo && x.ShipSer == ShipSer && x.RecNo == RecNo).FirstOrDefault();

            double CurrRatio = GetLastExRate(ReceiptHF.CurrNo.Value, ReceiptHF.RecDate.Value);
            Vendor Vend = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == ReceiptHF.VendorNo).FirstOrDefault();

            OrderHF OrdHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == OrdYear && x.OrderNo == OrderNo && x.TawreedNo == TawreedNo).FirstOrDefault();
            gLcDept = OrdHF.LcDept.Value;
            gLcAcc = OrdHF.LcAccNo.Value;


            if (gLcDept == 0 || gLcAcc == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب إدخال حساب الطلبيه" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The Order Account Must Be Entered" }, JsonRequestBehavior.AllowGet);
                }
            }


            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;


            int i = 1;
            int VouhNo = 1;

            double? Qty = 0;
            double? QtyUnit = 0;
            double? NetAmount = 0;
            double? NetFAmount = 0;



            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.invdailydf  WHERE compno = 0", cn);
                DaPR.Fill(ds, "TmpTbl");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.InvBatchsMF  WHERE compno = 0", cn);
                DaPR1.Fill(ds1, "TmpBatchs");


                transaction = cn.BeginTransaction();



                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "InvT_AddInvdailyHF";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@VouchNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime)).Value = ReceiptHF.RecDate.Value.ToShortDateString();
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = ReceiptHF.StoreNo.Value;
                    cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = gLcDept;
                    cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = gLcAcc;
                    cmd.Parameters.Add(new SqlParameter("@ToStore", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt)).Value = ReceiptHF.DocType.Value;
                    cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar)).Value = ReceiptHF.OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@NetAmount", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@VouDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PerDisc", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Vou_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Per_Tax", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalseMan", SqlDbType.Float)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CaCr", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@ConvRate_Cost", SqlDbType.Float)).Value = CurrRatio;
                    cmd.Parameters.Add(new SqlParameter("@SusFlag", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CurrNo", SqlDbType.SmallInt)).Value = ReceiptHF.CurrNo;
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
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = "إنشاء سند استلام من رقم طلب شراء" + " - " + ReceiptHF.OrderNo + " و رقم امر شراء " + " - " + ReceiptHF.TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@CarNum", SqlDbType.VarChar)).Value = ReceiptHF.CarNo;
                    cmd.Parameters.Add(new SqlParameter("@Transporter", SqlDbType.VarChar)).Value = ReceiptHF.DriverName;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@SalesOrderNo", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@CustRef", SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar)).Value = "";
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

                    foreach (Ord_OrdReceiptDF item in ReceiptDF)
                    {
                        DataSet DsBatchs = new DataSet();
                        DsBatchs = LoadStoreBatchs(item.ItemNo, ReceiptHF.StoreNo.Value, item.RecUnitSerial.Value, transaction, cn);

                        DataRow DrBatch;
                        object[] findTheseVals = new object[2];
                        findTheseVals[0] = item.ItemNo;
                        findTheseVals[1] = item.ShipSer;
                        if(DsBatchs.Tables["BatchsTbl"] == null)
                        {
                            DrTmp1 = ds1.Tables["TmpBatchs"].NewRow();
                            DrTmp1["CompNo"] = company.comp_num;
                            DrTmp1["StoreNo"] = ReceiptHF.StoreNo.Value;
                            DrTmp1["ItemNo"] = item.ItemNo;
                            DrTmp1["batchNo"] = ReceiptHF.ShipSer;
                            DrTmp1["ExpDate"] = DateTime.Now.ToShortDateString();
                            DrTmp1["ManDate"] = DateTime.Now.ToShortDateString();
                            DrTmp1["IsHalt"] = 0;
                            DrTmp1["UnitCost"] = 0;
                            DrTmp1["RefNo"] = "";
                            DrTmp1["RefNo2"] = "";
                            DrTmp1["Location"] = "";
                            DrTmp1["SellPrice"] = 0;
                            DrTmp1["BatchSerial"] = item.RecUnitSerial.Value;
                            ds1.Tables["TmpBatchs"].Rows.Add(DrTmp1);
                        }
                        else
                        {
                            DrBatch = DsBatchs.Tables["BatchsTbl"].Rows.Find(findTheseVals);

                            if (DrBatch == null)
                            {
                                DrTmp1 = ds1.Tables["TmpBatchs"].NewRow();
                                DrTmp1["CompNo"] = company.comp_num;
                                DrTmp1["StoreNo"] = ReceiptHF.StoreNo.Value;
                                DrTmp1["ItemNo"] = item.ItemNo;
                                DrTmp1["batchNo"] = ReceiptHF.ShipSer;
                                DrTmp1["ExpDate"] = DateTime.Now.ToShortDateString();
                                DrTmp1["ManDate"] = DateTime.Now.ToShortDateString();
                                DrTmp1["IsHalt"] = 0;
                                DrTmp1["UnitCost"] = 0;
                                DrTmp1["RefNo"] = "";
                                DrTmp1["RefNo2"] = "";
                                DrTmp1["Location"] = "";
                                DrTmp1["SellPrice"] = 0;
                                DrTmp1["BatchSerial"] = item.RecUnitSerial.Value;
                                ds1.Tables["TmpBatchs"].Rows.Add(DrTmp1);
                            }
                        }

                      
                    }
                    if (ds1.Tables["TmpBatchs"].Rows.Count != 0)
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
                            DaPR1.InsertCommand = cmdBatch;
                            try
                            {
                                DaPR1.Update(ds1, "TmpBatchs");
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                cn.Dispose();
                                return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    foreach (Ord_OrdReceiptDF item in ReceiptDF)
                    { 
                        DrTmp = ds.Tables["TmpTbl"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["compno"] = company.comp_num;
                        DrTmp["VouYear"] = OrdYear;
                        DrTmp["VouType"] = 1;
                        DrTmp["VouNo"] = VouhNo;
                        DrTmp["StoreNo"] = ReceiptHF.StoreNo;
                        DrTmp["ItemNo"] = item.ItemNo;
                        DrTmp["Batch"] = item.ShipSer;
                        DrTmp["ItemSer"] = i;
                        DrTmp["VouDate"] = ReceiptHF.RecDate.Value;
                        if (CheckMinusQty(ReceiptHF.StoreNo.Value, transaction, cn) == false)
                        {
                            transaction.Rollback();
                            cn.Dispose();

                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "لا يمكن التخزين بسبب وجود مشكلة بكمية مادة" + " " + "الموجودة في مستودع رقم" + " " + ReceiptHF.StoreNo }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "Can't execute because of a problem  in quantity of item  That Exist In Store No  " + ReceiptHF.StoreNo }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        DrTmp["Qty"] = item.RecQty;
                        if (item.RecQty2 != 0)
                        {
                            DrTmp["Qty2"] = item.RecQty2;
                        }
                        else
                        {
                            DrTmp["Qty2"] = 0;
                        }
                        DrTmp["ItemDimension"] = "0*0*0";
                        DrTmp["Bonus"] = 0;
                        DrTmp["TUnit"] = item.RecTUnit;
                        DrTmp["NetSellValue"] = 0;
                        DrTmp["Discount"] = 0;
                        DrTmp["PerDiscount"] = 0;
                        DrTmp["VouDiscount"] = 0;
                        DrTmp["CurrNo"] = ReceiptHF.CurrNo;
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
                        DrTmp["ExRate"] = CurrRatio;
                        DrTmp["UnitSerial"] = item.RecUnitSerial;
                        DrTmp["ProdOrderNo"] = 1;
                        DrTmp["NoteDet"] = "";
                        DrTmp.EndEdit();
                        ds.Tables["TmpTbl"].Rows.Add(DrTmp);
                        i = i + 1;
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
                    DSArch = LoadArch(OrdYear, OrderNo, TawreedNo, ReceiptHF.ShipSer, ReceiptHF.RecNo, transaction, cn);
                    foreach (DataRow DrArch in DSArch.Tables["Arch"].Rows)
                    {
                        using (SqlCommand cmdArch = new SqlCommand())
                        {
                            cmdArch.Connection = cn;
                            cmdArch.CommandText = "Invt_AddArchiveInfo";
                            cmdArch.CommandType = CommandType.StoredProcedure;
                            cmdArch.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            cmdArch.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt)).Value = OrdYear;
                            cmdArch.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 1;
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

            ReceiptHF.IsApproval = true;
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public DataSet LoadArch(int OrdYear, int OrderNo, string TawreedNo, string ShipSer, int RecNo, SqlTransaction MyTrans, SqlConnection co)
        {
            DataSet DsArch = new DataSet();
            SqlDataAdapter DaCmd = new SqlDataAdapter();
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "Ord_GetOrdReceiptArchiveInfo";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@OrderYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                Cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@TawreedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
                Cmd.Parameters.Add(new SqlParameter("@ShipSer", System.Data.SqlDbType.VarChar)).Value = ShipSer;
                Cmd.Parameters.Add(new SqlParameter("@RecNo", SqlDbType.Int)).Value = RecNo;
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