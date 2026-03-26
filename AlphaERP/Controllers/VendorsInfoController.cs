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
    public class VendorsInfoController : controller
    {
        // GET: VendorsInfo
        public ActionResult Index()
        {
            Session["VendorFiles"] = null;
            return View();
        }
        public ActionResult VendorsInfoList()
        {
            return PartialView();
        }
        public ActionResult VendorExistAcount()
        {
            return PartialView();
        }
        public ActionResult AddExistAcount()
        {
            return PartialView();
        }
        public ActionResult EditVendorsInfo(long VendorNo)
        {
            ViewBag.VendorNo = VendorNo;
            List<UploadFileVendor> lstFile = null;
             if (Session["VendorFiles"] == null)
            {
                lstFile = new List<UploadFileVendor>();
            }
            else
            {
                lstFile = (List<UploadFileVendor>)Session["VendorFiles"];
            }
            UploadFileVendor SinglFile = new UploadFileVendor();
            DataTable DtAttachment = new DataTable();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                DtAttachment = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_GetVendArchiveInfo";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = VendorNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(DtAttachment);
                    }
                }
            }

            int seial = 1;
            foreach (DataRow dr in DtAttachment.Rows)
            {
                SinglFile = lstFile.Find(a => a.VendorNo == Convert.ToInt64(VendorNo) && a.FileId == Convert.ToInt32(seial));
                if (SinglFile == null)
                {
                    byte[] FileByte = (byte[])dr["ArchiveData"];

                    SinglFile = new UploadFileVendor();
                    SinglFile.VendorNo = VendorNo;
                    SinglFile.FileId = seial;
                    SinglFile.File = (byte[])dr["ArchiveData"];
                    SinglFile.FileSize = Convert.ToInt32(dr["FileSize"]);
                    SinglFile.ContentType = Convert.ToString(dr["ContentType"]);
                    SinglFile.FileName = Convert.ToString(dr["FileName"]);
                    lstFile.Add(SinglFile);
                }
                else
                {
                    SinglFile.VendorNo = VendorNo;
                    SinglFile.FileId = seial;
                    SinglFile.File = (byte[])dr["ArchiveData"];
                    SinglFile.FileSize = Convert.ToInt32(dr["FileSize"]);
                    SinglFile.ContentType = Convert.ToString(dr["ContentType"]);
                    SinglFile.FileName = Convert.ToString(dr["FileName"]);
                }
                seial++;
                Session["VendorFiles"] = lstFile;
            }

            return PartialView();
        }
        public ActionResult InqVendorsInfo(long VendorNo)
        {
            ViewBag.VendorNo = VendorNo;

            return PartialView();
        }
        public ActionResult AddWithoutAcount()
        {
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Vendors_LoadVenGroups";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<VendorsAccGroups> VendorsAccGroups = Dt.AsEnumerable().Select(row => new VendorsAccGroups
            {
                GroupNo = row.Field<long>("GroupNo"),
                acc_desc = row.Field<string>("acc_desc"),
                acc_edesc = row.Field<string>("acc_edesc"),
                GroupLevel = row.Field<byte>("GroupLevel")
            }).ToList();

            return PartialView(VendorsAccGroups);
        }
        public JsonResult VendorAccGroups(long AccGroups)
        {
            long LastAccNo = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GLN_GetLastAccInq";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FromAcc", System.Data.SqlDbType.BigInt)).Value = AccGroups;
                    cmd.Parameters.Add(new SqlParameter("@ToAcc", System.Data.SqlDbType.BigInt)).Value = 999999999999999;
                    cmd.Parameters.Add(new SqlParameter("@AccPart", System.Data.SqlDbType.VarChar)).Value = "";
                    cmd.Parameters.Add(new SqlParameter("@LastAccNo", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    LastAccNo = Convert.ToInt64(cmd.Parameters["@LastAccNo"].Value) + 1;
                }
            }
            return Json(new { LastAccNo = LastAccNo }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadArea(int CountryId,long srl)
        {
            List<PayCodes> Areas = new MDB().Database.SqlQuery<PayCodes>(string.Format("select Area as Id ,[Desc] as DescAr,isnull(EngDesc,'No Desc.') as DescEn from Areas WHERE (Country = {0})", CountryId)).ToList();

            return Json(new { Area = Areas,id = srl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadSubArea(int CountryId, int AreaId, long srl)
        {
            List<PayCodes> SAreas = new MDB().Database.SqlQuery<PayCodes>(string.Format("select SArea as Id ,[Desc] as DescAr,isnull(EngDesc,'No Desc.') as DescEn from SAreas WHERE (Country = {0}) AND (Area = {1})", CountryId, AreaId)).ToList();
            return Json(new { SAreas = SAreas, id = srl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadStreet(int CountryId, int AreaId, int SubAreaId, long srl)
        {
            List<PayCodes> Street = new MDB().Database.SqlQuery<PayCodes>(string.Format("select Street_No as Id ,St_Desc as DescAr,isnull(EngDesc,'No Desc.') as DescEn from StreetMF WHERE (Country = {0}) AND (Area = {1}) AND (SArea = {2})", CountryId, AreaId, SubAreaId)).ToList();
            return Json(new { Street = Street, id = srl }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateVendorsInfo(Vendor Vendors, List<GLCR> GLCRs, List<Ven_Contact> VenContact, List<Ven_bankInfo> VenbankInfo, List<UploadFileVendor> FileArchive)
        {
            Vendor Ven = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == Vendors.VendorNo).FirstOrDefault();
            if(Ven != null)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
                }
            }
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_Contact  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "Contact");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_bankInfo  WHERE CompNo = 0", cn);
                DaPR1.Fill(ds1, "bankInfo");

                transaction = cn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Vendors_AddVendor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@comp", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = Vendors.VendorNo;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar)).Value = Vendors.Name;
                    if (Vendors.Eng_Name == null)
                    {
                        Vendors.Eng_Name = Vendors.Name;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Eng_Name", SqlDbType.VarChar)).Value = Vendors.Eng_Name;
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar)).Value = Vendors.Title;
                    cmd.Parameters.Add(new SqlParameter("@IsHalt", SqlDbType.Bit)).Value = Vendors.IsHalt;
                    if (Vendors.Notes == null)
                    {
                        Vendors.Notes = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = Vendors.Notes;
                    cmd.Parameters.Add(new SqlParameter("@pmethod", SqlDbType.SmallInt)).Value = Vendors.Pmethod;
                    cmd.Parameters.Add(new SqlParameter("@Pay_Method", SqlDbType.SmallInt)).Value = Vendors.Pay_Method;
                    cmd.Parameters.Add(new SqlParameter("@Cr_Lim", SqlDbType.Float)).Value = Vendors.Cr_Lim;
                    cmd.Parameters.Add(new SqlParameter("@Curr", SqlDbType.SmallInt)).Value = Vendors.Curr;
                    if (Vendors.DelayDays == null)
                    {
                        Vendors.DelayDays = 0;
                    }
                    if (Vendors.VenLevel == null)
                    {
                        Vendors.VenLevel = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Due", SqlDbType.SmallInt)).Value = Vendors.DelayDays;
                    cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.SmallInt)).Value = Vendors.VenLevel;
                    if (Vendors.Address == null)
                    {
                        Vendors.Address = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = Vendors.Address;

                    if (Vendors.Country == null)
                    {
                        Vendors.Country = 0;
                    }
                    if (Vendors.Area == null)
                    {
                        Vendors.Area = 0;
                    }
                    if (Vendors.SArea == null)
                    {
                        Vendors.SArea = 0;
                    }
                    if (Vendors.Street_No == null)
                    {
                        Vendors.Street_No = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.SmallInt)).Value = Vendors.Country;
                    cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.SmallInt)).Value = Vendors.Area;
                    cmd.Parameters.Add(new SqlParameter("@SArea", SqlDbType.SmallInt)).Value = Vendors.SArea;
                    cmd.Parameters.Add(new SqlParameter("@Street_No", SqlDbType.SmallInt)).Value = Vendors.Street_No;
                    if (Vendors.Location == null)
                    {
                        Vendors.Location = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar)).Value = Vendors.Location;
                    if (Vendors.Resp_Person == null)
                    {
                        Vendors.Resp_Person = "";
                    }

                    cmd.Parameters.Add(new SqlParameter("@Resp_Person", SqlDbType.VarChar)).Value = Vendors.Resp_Person;
                    if (Vendors.Tel1 == null)
                    {
                        Vendors.Tel1 = "";
                    }
                    if (Vendors.Tel2 == null)
                    {
                        Vendors.Tel2 = "";
                    }
                    if (Vendors.Mobile_No == null)
                    {
                        Vendors.Mobile_No = "";
                    }
                    if (Vendors.POBox == null)
                    {
                        Vendors.POBox = "";
                    }
                    if (Vendors.Postal_Code == null)
                    {
                        Vendors.Postal_Code = "";
                    }
                    if (Vendors.Fax == null)
                    {
                        Vendors.Fax = "";
                    }
                    if (Vendors.Telex == null)
                    {
                        Vendors.Telex = "";
                    }
                    if (Vendors.EMail == null)
                    {
                        Vendors.EMail = "";
                    }
                    if (Vendors.Penf == null)
                    {
                        Vendors.Penf = "";
                    }
                    if (Vendors.GenCondition == null)
                    {
                        Vendors.GenCondition = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Tel1", SqlDbType.VarChar)).Value = Vendors.Tel1;
                    cmd.Parameters.Add(new SqlParameter("@Tel2", SqlDbType.VarChar)).Value = Vendors.Tel2;
                    cmd.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = Vendors.Mobile_No;
                    cmd.Parameters.Add(new SqlParameter("@POBox", SqlDbType.VarChar)).Value = Vendors.POBox;
                    cmd.Parameters.Add(new SqlParameter("@Postal_Code", SqlDbType.VarChar)).Value = Vendors.Postal_Code;
                    cmd.Parameters.Add(new SqlParameter("@Fax", SqlDbType.VarChar)).Value = Vendors.Fax;
                    cmd.Parameters.Add(new SqlParameter("@Telex", SqlDbType.VarChar)).Value = Vendors.Telex;
                    cmd.Parameters.Add(new SqlParameter("@EMail", SqlDbType.VarChar)).Value = Vendors.EMail;
                    cmd.Parameters.Add(new SqlParameter("@Disc", SqlDbType.Money)).Value = Vendors.Disc;
                    cmd.Parameters.Add(new SqlParameter("@Penf", SqlDbType.VarChar)).Value = Vendors.Penf;
                    cmd.Parameters.Add(new SqlParameter("@Evaluated", SqlDbType.Bit)).Value = Vendors.Evaluated;
                    cmd.Parameters.Add(new SqlParameter("@GroupNo", SqlDbType.SmallInt)).Value = Vendors.GroupNo;
                    cmd.Parameters.Add(new SqlParameter("@GenCondition", SqlDbType.Text)).Value = Vendors.GenCondition;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@TransporterNo", SqlDbType.BigInt)).Value = Vendors.TransporterNo;
                    cmd.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit)).Value = Vendors.Taxable;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;

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
                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == 2627)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
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


                List<UploadFileVendor> upFile = (List<UploadFileVendor>)Session["VendorFiles"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        foreach (UploadFileVendor item in FileArchive)
                        {
                            UploadFileVendor fl = upFile.Where(x => x.VendorNo == item.VendorNo && x.FileId == item.FileId).FirstOrDefault();
                            using (SqlCommand CmdFiles = new SqlCommand())
                            {
                                CmdFiles.Connection = cn;
                                CmdFiles.CommandText = "Ord_AddVendArchiveInfo";
                                CmdFiles.CommandType = CommandType.StoredProcedure;
                                CmdFiles.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                                CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                CmdFiles.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = item.VendorNo;
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

                if (GLCRs != null)
                {
                    foreach (GLCR item in GLCRs)
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandText = "GLN_LinkAccWithDept";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = item.DeptNo;
                            cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = item.AccNo;
                            cmd.Parameters.Add(new SqlParameter("@ToAccNo", SqlDbType.BigInt)).Value = item.AccNo;
                            cmd.Parameters.Add(new SqlParameter("@ERR", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@Success", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@Ignore", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
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
                            if (Convert.ToInt32(cmd.Parameters["@ERR"].Value) != 0)
                            {
                                if (Convert.ToInt32(cmd.Parameters["@ERR"].Value) == 2627)
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                }
                if (VenContact != null)
                {
                    foreach (Ven_Contact item in VenContact)
                    {
                        DrTmp = ds.Tables["Contact"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["VendorNo"] = item.VendorNo;
                        DrTmp["ContactNo"] = item.ContactNo;
                        DrTmp["ContactName"] = item.ContactName;
                        DrTmp.EndEdit();
                        ds.Tables["Contact"].Rows.Add(DrTmp);
                    }
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "Ven_AddContact";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                        cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.SmallInt, 4, "ContactNo"));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.VarChar, 50, "ContactName"));
                        cmd.Transaction = transaction;
                        DaPR.InsertCommand = cmd;
                        try
                        {
                            DaPR.Update(ds, "Contact");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (VenbankInfo != null)
                {
                    foreach (Ven_bankInfo item in VenbankInfo)
                    {
                        DrTmp1 = ds1.Tables["bankInfo"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = company.comp_num;
                        DrTmp1["VendorNo"] = item.VendorNo;
                        DrTmp1["ID"] = item.ID;
                        if (item.BankName == null)
                        {
                            item.BankName = "";
                        }
                        if (item.AccNo == null)
                        {
                            item.AccNo = 0;
                        }
                        if (item.Iban == null)
                        {
                            item.Iban = "";
                        }
                        DrTmp1["BankName"] = item.BankName;
                        DrTmp1["AccNo"] = item.AccNo;
                        DrTmp1["Iban"] = item.Iban;
                        DrTmp1.EndEdit();
                        ds1.Tables["bankInfo"].Rows.Add(DrTmp1);
                    }
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "Ven_AddBankInfo";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Char, 10, "ID"));
                        cmd.Parameters.Add(new SqlParameter("@BankName", SqlDbType.VarChar, 50, "BankName"));
                        cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt, 4, "AccNo"));
                        cmd.Parameters.Add(new SqlParameter("@Iban", SqlDbType.VarChar, 50, "Iban"));
                        cmd.Transaction = transaction;
                        DaPR1.InsertCommand = cmd;
                        try
                        {
                            DaPR1.Update(ds1, "bankInfo");
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

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CreateWithoutVendorsInfo(long AccGroups,Vendor Vendors, List<GLCR> GLCRs, List<Ven_Contact> VenContact, List<Ven_bankInfo> VenbankInfo, List<UploadFileVendor> FileArchive)
        {
            Vendor Ven = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == Vendors.VendorNo).FirstOrDefault();
            if (Ven != null)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
                }
            }
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_Contact  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "Contact");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_bankInfo  WHERE CompNo = 0", cn);
                DaPR1.Fill(ds1, "bankInfo");

                transaction = cn.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "GLN_AddNewAccount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@accnum", SqlDbType.BigInt)).Value = Vendors.VendorNo;
                    cmd.Parameters.Add(new SqlParameter("@accdesc", SqlDbType.VarChar)).Value = Vendors.Name;
                    if(Vendors.Eng_Name == null)
                    {
                        Vendors.Eng_Name = Vendors.Name;
                    }
                    cmd.Parameters.Add(new SqlParameter("@accedesc", SqlDbType.VarChar)).Value = Vendors.Eng_Name;
                    cmd.Parameters.Add(new SqlParameter("@acctype", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@accnorbal", SqlDbType.Int)).Value = -1;
                    cmd.Parameters.Add(new SqlParameter("@accreport", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@acctotlev", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@acclineadd", SqlDbType.Int)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@acccurr", SqlDbType.Int)).Value = Vendors.Curr;
                    cmd.Parameters.Add(new SqlParameter("@acchalt", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@Err", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(new SqlParameter("@acc_parent", SqlDbType.BigInt)).Value = AccGroups;

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
                    if (Convert.ToInt32(cmd.Parameters["@Err"].Value) != 0)
                    {
                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == 2627)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
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

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Vendors_AddVendor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@comp", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = Vendors.VendorNo;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar)).Value = Vendors.Name;
                    if (Vendors.Eng_Name == null)
                    {
                        Vendors.Eng_Name = Vendors.Name;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Eng_Name", SqlDbType.VarChar)).Value = Vendors.Eng_Name;
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar)).Value = Vendors.Title;
                    cmd.Parameters.Add(new SqlParameter("@IsHalt", SqlDbType.Bit)).Value = Vendors.IsHalt;
                    if (Vendors.Notes == null)
                    {
                        Vendors.Notes = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = Vendors.Notes;
                    cmd.Parameters.Add(new SqlParameter("@pmethod", SqlDbType.SmallInt)).Value = Vendors.Pmethod;
                    cmd.Parameters.Add(new SqlParameter("@Pay_Method", SqlDbType.SmallInt)).Value = Vendors.Pay_Method;
                    cmd.Parameters.Add(new SqlParameter("@Cr_Lim", SqlDbType.Float)).Value = Vendors.Cr_Lim;
                    if (Vendors.Curr == null)
                    {
                        Vendors.Curr = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Curr", SqlDbType.SmallInt)).Value = Vendors.Curr;
                    if (Vendors.DelayDays == null)
                    {
                        Vendors.DelayDays = 0;
                    }
                    if (Vendors.VenLevel == null)
                    {
                        Vendors.VenLevel = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Due", SqlDbType.SmallInt)).Value = Vendors.DelayDays;
                    cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.SmallInt)).Value = Vendors.VenLevel;
                    if (Vendors.Address == null)
                    {
                        Vendors.Address = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = Vendors.Address;
                    if (Vendors.Country == null)
                    {
                        Vendors.Country = 0;
                    }
                    if (Vendors.Area == null)
                    {
                        Vendors.Area = 0;
                    }
                    if (Vendors.SArea == null)
                    {
                        Vendors.SArea = 0;
                    }
                    if (Vendors.Street_No == null)
                    {
                        Vendors.Street_No = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.SmallInt)).Value = Vendors.Country;
                    cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.SmallInt)).Value = Vendors.Area;
                    cmd.Parameters.Add(new SqlParameter("@SArea", SqlDbType.SmallInt)).Value = Vendors.SArea;
                    cmd.Parameters.Add(new SqlParameter("@Street_No", SqlDbType.SmallInt)).Value = Vendors.Street_No;
                    if(Vendors.Location == null)
                    {
                        Vendors.Location = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar)).Value = Vendors.Location;
                    if (Vendors.Resp_Person == null)
                    {
                        Vendors.Resp_Person = "";
                    }

                    cmd.Parameters.Add(new SqlParameter("@Resp_Person", SqlDbType.VarChar)).Value = Vendors.Resp_Person;
                    if (Vendors.Tel1 == null)
                    {
                        Vendors.Tel1 = "";
                    }
                    if (Vendors.Tel2 == null)
                    {
                        Vendors.Tel2 = "";
                    }
                    if (Vendors.Mobile_No == null)
                    {
                        Vendors.Mobile_No = "";
                    }
                    if (Vendors.POBox == null)
                    {
                        Vendors.POBox = "";
                    }
                    if (Vendors.Postal_Code == null)
                    {
                        Vendors.Postal_Code = "";
                    }
                    if (Vendors.Fax == null)
                    {
                        Vendors.Fax = "";
                    }
                    if (Vendors.Telex == null)
                    {
                        Vendors.Telex = "";
                    }
                    if (Vendors.EMail == null)
                    {
                        Vendors.EMail = "";
                    }
                    if (Vendors.Penf == null)
                    {
                        Vendors.Penf = "";
                    }
                    if (Vendors.GenCondition == null)
                    {
                        Vendors.GenCondition = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Tel1", SqlDbType.VarChar)).Value = Vendors.Tel1;
                    cmd.Parameters.Add(new SqlParameter("@Tel2", SqlDbType.VarChar)).Value = Vendors.Tel2;
                    cmd.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = Vendors.Mobile_No;
                    cmd.Parameters.Add(new SqlParameter("@POBox", SqlDbType.VarChar)).Value = Vendors.POBox;
                    cmd.Parameters.Add(new SqlParameter("@Postal_Code", SqlDbType.VarChar)).Value = Vendors.Postal_Code;
                    cmd.Parameters.Add(new SqlParameter("@Fax", SqlDbType.VarChar)).Value = Vendors.Fax;
                    cmd.Parameters.Add(new SqlParameter("@Telex", SqlDbType.VarChar)).Value = Vendors.Telex;
                    cmd.Parameters.Add(new SqlParameter("@EMail", SqlDbType.VarChar)).Value = Vendors.EMail;
                    cmd.Parameters.Add(new SqlParameter("@Disc", SqlDbType.Money)).Value = Vendors.Disc;
                    cmd.Parameters.Add(new SqlParameter("@Penf", SqlDbType.VarChar)).Value = Vendors.Penf;
                    cmd.Parameters.Add(new SqlParameter("@Evaluated", SqlDbType.Bit)).Value = Vendors.Evaluated;
                    cmd.Parameters.Add(new SqlParameter("@GroupNo", SqlDbType.SmallInt)).Value = Vendors.GroupNo;
                    cmd.Parameters.Add(new SqlParameter("@GenCondition", SqlDbType.Text)).Value = Vendors.GenCondition;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@TransporterNo", SqlDbType.BigInt)).Value = Vendors.TransporterNo;
                    cmd.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit)).Value = Vendors.Taxable;
                    cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;

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
                        if (Convert.ToInt32(cmd.Parameters["@ErrNo"].Value) == 2627)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            if (Language == "ar-JO")
                            {
                                return Json(new { error = "الرمز المدخل معرف مسبقاً" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { error = "This code  already exists" }, JsonRequestBehavior.AllowGet);
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

                List<UploadFileVendor> upFile = (List<UploadFileVendor>)Session["VendorFiles"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        foreach (UploadFileVendor item in FileArchive)
                        {
                            UploadFileVendor fl = upFile.Where(x => x.VendorNo == item.VendorNo && x.FileId == item.FileId).FirstOrDefault();
                            using (SqlCommand CmdFiles = new SqlCommand())
                            {
                                CmdFiles.Connection = cn;
                                CmdFiles.CommandText = "Ord_AddVendArchiveInfo";
                                CmdFiles.CommandType = CommandType.StoredProcedure;
                                CmdFiles.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                                CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                CmdFiles.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = item.VendorNo;
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

                if (GLCRs != null)
                {
                    foreach (GLCR item in GLCRs)
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandText = "GLN_LinkAccWithDept";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            cmd.Parameters.Add(new SqlParameter("@DeptNo", SqlDbType.Int)).Value = item.DeptNo;
                            cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = item.AccNo;
                            cmd.Parameters.Add(new SqlParameter("@ToAccNo", SqlDbType.BigInt)).Value = item.AccNo;
                            cmd.Parameters.Add(new SqlParameter("@ERR", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@Success", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(new SqlParameter("@Ignore", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
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
                            if (Convert.ToInt32(cmd.Parameters["@ERR"].Value) != 0)
                            {
                                if (Convert.ToInt32(cmd.Parameters["@ERR"].Value) == 2627)
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    transaction.Rollback();
                                    cn.Dispose();
                                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                }

                  
                if(VenContact != null) {
                    foreach (Ven_Contact item in VenContact)
                    {
                        DrTmp = ds.Tables["Contact"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["VendorNo"] = item.VendorNo;
                        DrTmp["ContactNo"] = item.ContactNo;
                        DrTmp["ContactName"] = item.ContactName;
                        DrTmp.EndEdit();
                        ds.Tables["Contact"].Rows.Add(DrTmp);
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "Ven_AddContact";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                        cmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.SmallInt, 4, "ContactNo"));
                        cmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.VarChar, 50, "ContactName"));
                        cmd.Transaction = transaction;
                        DaPR.InsertCommand = cmd;
                        try
                        {
                            DaPR.Update(ds, "Contact");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (VenbankInfo != null)
                {
                    foreach (Ven_bankInfo item in VenbankInfo)
                    {
                        DrTmp1 = ds1.Tables["bankInfo"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = company.comp_num;
                        DrTmp1["VendorNo"] = item.VendorNo;
                        DrTmp1["ID"] = item.ID;
                        if (item.BankName == null)
                        {
                            item.BankName = "";
                        }
                        if (item.AccNo == null)
                        {
                            item.AccNo = 0;
                        }
                        if (item.Iban == null)
                        {
                            item.Iban = "";
                        }
                        DrTmp1["BankName"] = item.BankName;
                        DrTmp1["AccNo"] = item.AccNo;
                        DrTmp1["Iban"] = item.Iban;
                        DrTmp1.EndEdit();
                        ds1.Tables["bankInfo"].Rows.Add(DrTmp1);
                    }
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "Ven_AddBankInfo";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                        cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Char, 10, "ID"));
                        cmd.Parameters.Add(new SqlParameter("@BankName", SqlDbType.VarChar, 50, "BankName"));
                        cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt, 4, "AccNo"));
                        cmd.Parameters.Add(new SqlParameter("@Iban", SqlDbType.VarChar, 50, "Iban"));
                        cmd.Transaction = transaction;
                        DaPR1.InsertCommand = cmd;
                        try
                        {
                            DaPR1.Update(ds1, "bankInfo");
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

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult EditVendorInfo(Vendor Vendors, List<Ven_Contact> VenContact, List<Ven_bankInfo> VenbankInfo, List<UploadFileVendor> FileArchive)
        {
            Vendor Ven = db.Vendors.Where(x => x.comp == company.comp_num && x.VendorNo == Vendors.VendorNo).FirstOrDefault();
            
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_Contact  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "Contact");

                DaPR1.SelectCommand = new SqlCommand("SELECT * FROM dbo.Ven_bankInfo  WHERE CompNo = 0", cn);
                DaPR1.Fill(ds1, "bankInfo");

                transaction = cn.BeginTransaction();

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ven_Web_DelContact";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@VendorNo", SqlDbType.BigInt).Value = Vendors.VendorNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ven_Web_DelBankInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@VendorNo", SqlDbType.BigInt).Value = Vendors.VendorNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }
                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ord_Web_DelVendArchiveInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@AccNo", SqlDbType.BigInt).Value = Vendors.VendorNo;
                    DelCmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Vendors_UpdVendor";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@comp", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt)).Value = Vendors.VendorNo;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar)).Value = Vendors.Name;
                    if (Vendors.Eng_Name == null)
                    {
                        Vendors.Eng_Name = Vendors.Name;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Eng_Name", SqlDbType.VarChar)).Value = Vendors.Eng_Name;
                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.VarChar)).Value = Vendors.Title;
                    cmd.Parameters.Add(new SqlParameter("@IsHalt", SqlDbType.Bit)).Value = Vendors.IsHalt;
                    if (Vendors.Notes == null)
                    {
                        Vendors.Notes = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = Vendors.Notes;
                    if (Vendors.Pmethod == null)
                    {
                        Vendors.Pmethod = 0;
                    }
                    if (Vendors.Pay_Method == null)
                    {
                        Vendors.Pay_Method = "0";
                    }
                    if (Vendors.Curr == null)
                    {
                        Vendors.Curr = 0;
                    }
                    if (Vendors.DelayDays == null)
                    {
                        Vendors.DelayDays = 0;
                    }
                    if (Vendors.VenLevel == null)
                    {
                        Vendors.VenLevel = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@pmethod", SqlDbType.SmallInt)).Value = Vendors.Pmethod;
                    cmd.Parameters.Add(new SqlParameter("@Pay_Method", SqlDbType.SmallInt)).Value = Vendors.Pay_Method;
                    cmd.Parameters.Add(new SqlParameter("@Curr", SqlDbType.SmallInt)).Value = Vendors.Curr;
                    cmd.Parameters.Add(new SqlParameter("@Due", SqlDbType.SmallInt)).Value = Vendors.DelayDays;
                    cmd.Parameters.Add(new SqlParameter("@Level", SqlDbType.SmallInt)).Value = Vendors.VenLevel;
                    if (Vendors.Address == null)
                    {
                        Vendors.Address = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Address", SqlDbType.VarChar)).Value = Vendors.Address;
                    if (Vendors.Country == null)
                    {
                        Vendors.Country = 0;
                    }
                    if (Vendors.Area == null)
                    {
                        Vendors.Area = 0;
                    }
                    if (Vendors.SArea == null)
                    {
                        Vendors.SArea = 0;
                    }
                    if (Vendors.Street_No == null)
                    {
                        Vendors.Street_No = 0;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.SmallInt)).Value = Vendors.Country;
                    cmd.Parameters.Add(new SqlParameter("@Area", SqlDbType.SmallInt)).Value = Vendors.Area;
                    cmd.Parameters.Add(new SqlParameter("@SArea", SqlDbType.SmallInt)).Value = Vendors.SArea;
                    cmd.Parameters.Add(new SqlParameter("@Street_No", SqlDbType.SmallInt)).Value = Vendors.Street_No;
                    if (Vendors.Location == null)
                    {
                        Vendors.Location = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Location", SqlDbType.VarChar)).Value = Vendors.Location;
                    if (Vendors.Resp_Person == null)
                    {
                        Vendors.Resp_Person = "";
                    }

                    cmd.Parameters.Add(new SqlParameter("@Resp_Person", SqlDbType.VarChar)).Value = Vendors.Resp_Person;
                    if (Vendors.Tel1 == null)
                    {
                        Vendors.Tel1 = "";
                    }
                    if (Vendors.Tel2 == null)
                    {
                        Vendors.Tel2 = "";
                    }
                    if (Vendors.Mobile_No == null)
                    {
                        Vendors.Mobile_No = "";
                    }
                    if (Vendors.POBox == null)
                    {
                        Vendors.POBox = "";
                    }
                    if (Vendors.Postal_Code == null)
                    {
                        Vendors.Postal_Code = "";
                    }
                    if (Vendors.Fax == null)
                    {
                        Vendors.Fax = "";
                    }
                    if (Vendors.Telex == null)
                    {
                        Vendors.Telex = "";
                    }
                    if (Vendors.EMail == null)
                    {
                        Vendors.EMail = "";
                    }
                    if (Vendors.Penf == null)
                    {
                        Vendors.Penf = "";
                    }
                    if (Vendors.GenCondition == null)
                    {
                        Vendors.GenCondition = "";
                    }
                    cmd.Parameters.Add(new SqlParameter("@Tel1", SqlDbType.VarChar)).Value = Vendors.Tel1;
                    cmd.Parameters.Add(new SqlParameter("@Tel2", SqlDbType.VarChar)).Value = Vendors.Tel2;
                    cmd.Parameters.Add(new SqlParameter("@Mobile_No", SqlDbType.VarChar)).Value = Vendors.Mobile_No;
                    cmd.Parameters.Add(new SqlParameter("@POBox", SqlDbType.VarChar)).Value = Vendors.POBox;
                    cmd.Parameters.Add(new SqlParameter("@Postal_Code", SqlDbType.VarChar)).Value = Vendors.Postal_Code;
                    cmd.Parameters.Add(new SqlParameter("@Fax", SqlDbType.VarChar)).Value = Vendors.Fax;
                    cmd.Parameters.Add(new SqlParameter("@Telex", SqlDbType.VarChar)).Value = Vendors.Telex;
                    cmd.Parameters.Add(new SqlParameter("@EMail", SqlDbType.VarChar)).Value = Vendors.EMail;
                    cmd.Parameters.Add(new SqlParameter("@Penf", SqlDbType.VarChar)).Value = Vendors.Penf;
                    cmd.Parameters.Add(new SqlParameter("@Evaluated", SqlDbType.Bit)).Value = Vendors.Evaluated;
                    cmd.Parameters.Add(new SqlParameter("@Disc", SqlDbType.Money)).Value = Vendors.Disc;
                    cmd.Parameters.Add(new SqlParameter("@GroupNo", SqlDbType.SmallInt)).Value = Vendors.GroupNo;
                    cmd.Parameters.Add(new SqlParameter("@GenCondition", SqlDbType.Text)).Value = Vendors.GenCondition;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@TransporterNo", SqlDbType.BigInt)).Value = Vendors.TransporterNo;
                    cmd.Parameters.Add(new SqlParameter("@Taxable", SqlDbType.Bit)).Value = Vendors.Taxable;
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
                }

                List<UploadFileVendor> upFile = (List<UploadFileVendor>)Session["VendorFiles"];
                if (upFile != null)
                {
                    if (upFile.Count != 0)
                    {
                        if(FileArchive != null)
                        {
                            foreach (UploadFileVendor item in FileArchive)
                            {
                                UploadFileVendor fl = upFile.Where(x => x.VendorNo == item.VendorNo && x.FileId == item.FileId).FirstOrDefault();
                                using (SqlCommand CmdFiles = new SqlCommand())
                                {
                                    CmdFiles.Connection = cn;
                                    CmdFiles.CommandText = "Ord_AddVendArchiveInfo";
                                    CmdFiles.CommandType = CommandType.StoredProcedure;
                                    CmdFiles.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = 0;
                                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                                    CmdFiles.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = item.VendorNo;
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

                if (VenContact != null)
                {
                    foreach (Ven_Contact item in VenContact)
                    {
                        DrTmp = ds.Tables["Contact"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["VendorNo"] = item.VendorNo;
                        DrTmp["ContactNo"] = item.ContactNo;
                        DrTmp["ContactName"] = item.ContactName;
                        DrTmp.EndEdit();
                        ds.Tables["Contact"].Rows.Add(DrTmp);
                    }
                }


                using (SqlCommand InsCmd = new SqlCommand())
                {
                    InsCmd.Connection = cn;
                    InsCmd.CommandText = "Ven_AddContact";
                    InsCmd.CommandType = CommandType.StoredProcedure;
                    InsCmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.SmallInt, 4, "ContactNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@ContactName", SqlDbType.VarChar, 50, "ContactName"));
                    InsCmd.Transaction = transaction;
                    DaPR.InsertCommand = InsCmd;
                }
                    try
                    {
                        DaPR.Update(ds, "Contact");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                if (VenbankInfo != null)
                {
                    foreach (Ven_bankInfo item in VenbankInfo)
                    {
                        DrTmp1 = ds1.Tables["bankInfo"].NewRow();
                        DrTmp1.BeginEdit();
                        DrTmp1["CompNo"] = company.comp_num;
                        DrTmp1["VendorNo"] = item.VendorNo;
                        DrTmp1["ID"] = item.ID;
                        if (item.BankName == null)
                        {
                            item.BankName = "";
                        }
                        if (item.AccNo == null)
                        {
                            item.AccNo = 0;
                        }
                        if (item.Iban == null)
                        {
                            item.Iban = "";
                        }
                        DrTmp1["BankName"] = item.BankName;
                        DrTmp1["AccNo"] = item.AccNo;
                        DrTmp1["Iban"] = item.Iban;
                        DrTmp1.EndEdit();
                        ds1.Tables["bankInfo"].Rows.Add(DrTmp1);
                    }
                }

                using (SqlCommand InsCmd = new SqlCommand())
                {
                    InsCmd.Connection = cn;
                    InsCmd.CommandText = "Ven_AddBankInfo";
                    InsCmd.CommandType = CommandType.StoredProcedure;
                    InsCmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@VendorNo", SqlDbType.BigInt, 4, "VendorNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.Char, 10, "ID"));
                    InsCmd.Parameters.Add(new SqlParameter("@BankName", SqlDbType.VarChar, 50, "BankName"));
                    InsCmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt, 4, "AccNo"));
                    InsCmd.Parameters.Add(new SqlParameter("@Iban", SqlDbType.VarChar, 50, "Iban"));
                    InsCmd.Transaction = transaction;
                    DaPR1.InsertCommand = InsCmd;
                }
                try
                {
                    DaPR1.Update(ds1, "bankInfo");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                }

                transaction.Commit();
                cn.Dispose();
            }


            return Json(new { VendorNo = Vendors.VendorNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Del_VendorsInfo(long VendorNo)
        {

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();


                transaction = cn.BeginTransaction();

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ven_Web_DelContact";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@VendorNo", SqlDbType.BigInt).Value = VendorNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }


                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Ven_Web_DelBankInfo";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@VendorNo", SqlDbType.BigInt).Value = VendorNo;
                    DelCmd.Transaction = transaction;
                    DelCmd.ExecuteNonQuery();
                }

                using (SqlCommand DelCmd = new SqlCommand())
                {
                    DelCmd.Connection = cn;
                    DelCmd.CommandText = "Vendors_DelVendor";
                    DelCmd.CommandType = CommandType.StoredProcedure;
                    DelCmd.Parameters.Add("@comp", SqlDbType.SmallInt).Value = company.comp_num;
                    DelCmd.Parameters.Add("@VendorNo", SqlDbType.BigInt).Value = VendorNo;
                    DelCmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = me.UserID;
                    DelCmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    DelCmd.Transaction = transaction;
                    try
                    {
                        DelCmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    if (Convert.ToInt32(DelCmd.Parameters["@ErrNo"].Value) == 547)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        if (Language == "ar-JO")
                        {
                            return Json(new { error = "لا يمكن حذف السجل بسبب ارتباطه بسجلات أخرى" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { error = "The Record Cant Be Deleted,Its Related To other Records" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (Convert.ToInt32(DelCmd.Parameters["@ErrNo"].Value) != 0)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        if (Language == "ar-JO")
                        {
                            return Json(new { error = "حدث خطأ لم تتم عملية الحذف" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { error = "An error occurred , deletion process faild" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                transaction.Commit();
                cn.Dispose();

            }


            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public string InsertFile()
        {
            List<UploadFileVendor> lstFile = null;
            if (Session["VendorFiles"] == null)
            {
                lstFile = new List<UploadFileVendor>();
            }
            else
            {
                lstFile = (List<UploadFileVendor>)Session["VendorFiles"];
            }
            UploadFileVendor SinglFile = new UploadFileVendor();
            var Id = Request.Params["Id"];
            var VendorNo = Request.Params["VendorNo"];

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.VendorNo == Convert.ToInt64(VendorNo) && a.FileId == Convert.ToInt32(Id));
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
                        SinglFile = new UploadFileVendor();
                        SinglFile.VendorNo = Convert.ToInt64(VendorNo);
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
                        SinglFile.VendorNo = Convert.ToInt64(VendorNo);
                        SinglFile.FileId = Convert.ToInt32(Id);
                        SinglFile.File = FileByte;
                        SinglFile.FileSize = _file.ContentLength;
                        SinglFile.ContentType = _file.ContentType;
                        SinglFile.FileName = _file.FileName;
                    }
                }
                Session["VendorFiles"] = lstFile;
            }

            return Convert.ToBase64String(FileByte);
        }

        public string RemoveFile(int Id,long VendorNo)
        {
            List<UploadFileVendor> lstFile = null;
            if (Session["VendorFiles"] == null)
            {
                lstFile = new List<UploadFileVendor>();
            }
            else
            {
                lstFile = (List<UploadFileVendor>)Session["VendorFiles"];
            }
            UploadFileVendor SinglFile = new UploadFileVendor();

            if (lstFile != null)
            {
                SinglFile = lstFile.Find(a => a.VendorNo == VendorNo && a.FileId == Id);
            }

            if (SinglFile != null)
            {
                lstFile.Remove(SinglFile);
                Session["VendorFiles"] = lstFile;
            }

            return "done";
        }
    }
}