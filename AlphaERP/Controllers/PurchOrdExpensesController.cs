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
    public class PurchOrdExpensesController : controller
    {
        // GET: PurchOrdExpenses
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PurchOrdExpensesList(int OrdYear)
        {
            ViewBag.OrdYear = OrdYear;
            return PartialView();
        }
        public ActionResult AddPurchOrdExpenses()
        {
            return View();
        }
        public ActionResult EditPurchOrdExpenses(int TransYear, int TransNo)
        {
            ViewBag.TransYear = TransYear;
            ViewBag.TransNo = TransNo;
            return View();
        }

        public ActionResult InqPurchOrdExpenses(int TransYear, int TransNo)
        {
            ViewBag.TransYear = TransYear;
            ViewBag.TransNo = TransNo;
            return View();
        }

        public JsonResult GetDeptName(int DeptNo)
        {
            string DeptName = "";
            GLN_UsersDept UsersDept = db.GLN_UsersDept.Where(x => x.CompNo == company.comp_num && x.DeptID == DeptNo && x.UserID == me.UserID && x.Permission == true).FirstOrDefault();
            if (UsersDept == null)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "لا تملك الصلاحية لهذه الدائرة", DeptName = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "You dont have permissions For this department", DeptName = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            GLDEPMF dept = db.GLDEPMFs.Where(x => x.DEP_COMP == company.comp_num && x.DEP_NUM == DeptNo).FirstOrDefault();

            if (Language == "ar-JO")
            {
                DeptName = dept.DEP_NAME;
            }
            else
            {
                DeptName = dept.DEP_ENAME;
            }

            return Json(new { DeptName = DeptName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Account(int DeptId)
        {
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, glactmf.acc_num as AccId, glactmf.acc_desc as AccDescAr, glactmf.acc_edesc as AccDescEn " +
                                                     "FROM glactmf INNER JOIN GLCRBMF ON glactmf.acc_comp = GLCRBMF.CRB_COMP AND glactmf.acc_num = GLCRBMF.crb_acc " +
                                                      "WHERE(GLCRBMF.crb_dep = '{0}') AND (acc_comp = '{1}') AND (acc_halt = 0)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public ActionResult eAccount(int DeptId, int Id)
        {
            ViewBag.Id = Id;
            List<Acc> Acc = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, glactmf.acc_num as AccId, glactmf.acc_desc as AccDescAr, glactmf.acc_edesc as AccDescEn " +
                                                     "FROM glactmf INNER JOIN GLCRBMF ON glactmf.acc_comp = GLCRBMF.CRB_COMP AND glactmf.acc_num = GLCRBMF.crb_acc " +
                                                      "WHERE(GLCRBMF.crb_dep = '{0}') AND (acc_comp = '{1}') AND (acc_halt = 0)", DeptId, company.comp_num)).ToList();

            return PartialView(Acc);
        }
        public JsonResult GetVendorName(int DeptNo, long AccNo)
        {

            Acc ex = new MDB().Database.SqlQuery<Acc>(string.Format("SELECT GLCRBMF.crb_dep as deptid, glactmf.acc_num as AccId, glactmf.acc_desc as AccDescAr, glactmf.acc_edesc as AccDescEn " +
                                                       "FROM glactmf INNER JOIN GLCRBMF ON glactmf.acc_comp = GLCRBMF.CRB_COMP AND glactmf.acc_num = GLCRBMF.crb_acc " +
                                                        "WHERE(GLCRBMF.crb_dep = '{0}') AND (acc_comp = '{1}') AND (glactmf.acc_num = '{2}') AND (acc_halt = 0)", DeptNo, company.comp_num, AccNo)).FirstOrDefault();


            string VendorName = "";
            if (ex == null)
            {
                return Json(new { VendorName = VendorName }, JsonRequestBehavior.AllowGet);
            }


            if (Language == "ar-JO")
            {
                VendorName = ex.AccDescAr;
            }
            else
            {
                VendorName = ex.AccDescEn;
            }
            return Json(new { VendorName = VendorName }, JsonRequestBehavior.AllowGet);
        }
        public double CurrRatio(int CurrCode)
        {
            double CurrRatio = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = cn;
                    Cmd.CommandText = "Gln_GetCurrExchRatio";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@CurrCode", SqlDbType.SmallInt).Value = CurrCode;
                    Cmd.Parameters.Add("@VDate", SqlDbType.SmallDateTime).Value = DateTime.Now;
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
                        string zz = ex.Message;
                        CurrRatio = 1;
                    }
                }
            }
            return CurrRatio;
        }
        public JsonResult GetCurrRatio(int CurrCode)
        {

            double CurrRat = CurrRatio(CurrCode);
            
           
            return Json(new { CurrRat = CurrRat }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExpCodes(int TransYear)
        {
            List<ExpCodes> ExpCodes = new MDB().Database.SqlQuery<ExpCodes>(string.Format("SELECT OrdYear, OrderNo, TawreedNo,ExpID,ExpArDesc , ExpEDesc " +
                                                     "FROM OrderHF inner  join Ord_LcExpCodes on OrderHF.CompNo = Ord_LcExpCodes.CompNo " +
                                                      "WHERE (OrderHF.CompNo = '{1}') AND (Approved = 1) AND (OrderClose = 0)", TransYear, company.comp_num)).ToList();

            return PartialView(ExpCodes);
        }
        public ActionResult eExpCodes(int TransYear, int Id)
        {
            ViewBag.Id = Id;
            List<ExpCodes> ExpCodes = new MDB().Database.SqlQuery<ExpCodes>(string.Format("SELECT OrdYear, OrderNo, TawreedNo,ExpID,ExpArDesc , ExpEDesc " +
                                                     "FROM OrderHF inner  join Ord_LcExpCodes on OrderHF.CompNo = Ord_LcExpCodes.CompNo " +
                                                      "WHERE (OrderHF.CompNo = '{1}') AND (Approved = 1) AND (OrderClose = 0)", TransYear, company.comp_num)).ToList();

            return PartialView(ExpCodes);
        }
        public ActionResult PurchOrdExp(List<ExpCodes> items, int srl)
        {
            ViewBag.Serial = srl;
            return PartialView(items);
        }
        public ActionResult ePurchOrdExp(List<ExpCodes> items, int srl, int Id)
        {
            ViewBag.Serial = srl;
            ViewBag.Id = Id;
            return PartialView(items);
        }
        public int GetOrderSerial(short PYear, int Scode, SqlTransaction MyTrans, SqlConnection co)
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
            return OrdSerial;
        }
        public int GetTransSerial(int TransYear,int TransType, int Collector,DateTime VouDate, int SerDeptNo, SqlTransaction MyTrans, SqlConnection co)
        {
            int OrdSerial = 0;
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandText = "GLN_GetVouchSerial";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@Year", SqlDbType.VarChar).Value = TransYear;
                Cmd.Parameters.Add("@VouchType", SqlDbType.Int).Value = TransType;
                Cmd.Parameters.Add("@Collector", SqlDbType.SmallInt).Value = Collector;
                Cmd.Parameters.Add("@VouDate", SqlDbType.SmallDateTime).Value = VouDate;
                Cmd.Parameters.Add("@SerDeptNo", SqlDbType.Int).Value = SerDeptNo;
                Cmd.Transaction = MyTrans;
                try
                {
                    SqlDataReader rdr = Cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            OrdSerial = Convert.ToInt32(rdr["srl_srl"]);
                        }
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    OrdSerial = 0;
                }
            }
            return OrdSerial;
        }
        public JsonResult CreatePurchOrdExpenses(Ord_PurchOrdExpensesHF POExpensesHF, List<Ord_PurchOrdExpensesDF> POExpensesDF)
        {
            int TransNo = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                TransNo = GetOrderSerial(POExpensesHF.TransYear, 80, transaction, cn);
                if (TransNo == 0)
                {
                    transaction.Rollback();
                    cn.Dispose();
                    return Json(new { error = Resources.Resource.errorPurchaseOrderExpensesSerial }, JsonRequestBehavior.AllowGet);
                }
            }

            Ord_PurchOrdExpensesHF PurchOrdExpensesHF = new Ord_PurchOrdExpensesHF();
            PurchOrdExpensesHF.CompNo = company.comp_num;
            PurchOrdExpensesHF.TransYear = POExpensesHF.TransYear;
            PurchOrdExpensesHF.TransNo = TransNo;
            PurchOrdExpensesHF.TransDate = POExpensesHF.TransDate;
            PurchOrdExpensesHF.DeptNo = POExpensesHF.DeptNo;
            PurchOrdExpensesHF.AccNo = POExpensesHF.AccNo;
            PurchOrdExpensesHF.CurrCode = POExpensesHF.CurrCode;
            PurchOrdExpensesHF.ExRate = POExpensesHF.ExRate;
            PurchOrdExpensesHF.TotalAmount = POExpensesHF.TotalAmount;
            PurchOrdExpensesHF.FrAmount = POExpensesHF.FrAmount;
            PurchOrdExpensesHF.Note = POExpensesHF.Note;
            PurchOrdExpensesHF.IsPosted = false;
            db.Ord_PurchOrdExpensesHF.Add(PurchOrdExpensesHF);
            db.SaveChanges();

            foreach (Ord_PurchOrdExpensesDF item in POExpensesDF)
            {
                Ord_PurchOrdExpensesDF ex = new Ord_PurchOrdExpensesDF();
                ex.CompNo = company.comp_num;
                ex.TransYear = item.TransYear;
                ex.TransNo = TransNo;
                ex.OrderYear = item.OrderYear;
                ex.OrderNo = item.OrderNo;
                ex.TawreedNo = item.TawreedNo;
                ex.ExpID = item.ExpID;
                ex.Amount = item.Amount;
                ex.FrAmount = item.FrAmount;
                ex.SalesTaxAmount = item.SalesTaxAmount;
                ex.SalesTaxAmountFr = item.SalesTaxAmountFr;
                ex.IncomeTaxAmount = item.IncomeTaxAmount;
                ex.IncomeTaxAmountFr = item.IncomeTaxAmountFr;
                ex.CustomtaxAmount = item.CustomtaxAmount;
                ex.CustomtaxAmountFr = item.CustomtaxAmountFr;
                db.Ord_PurchOrdExpensesDF.Add(ex);
                db.SaveChanges();
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();

                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.Connection = cn;
                    Cmd.CommandText = "Ord_Web_UodOrdSerials";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@PYear", SqlDbType.SmallInt).Value = POExpensesHF.TransYear;
                    Cmd.Parameters.Add("@SCode", SqlDbType.Int).Value = 80;
                    Cmd.Transaction = transaction;
                    Cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EditPurchOrdExp(Ord_PurchOrdExpensesHF POExpensesHF, List<Ord_PurchOrdExpensesDF> POExpensesDF)
        {
            Ord_PurchOrdExpensesHF ex = db.Ord_PurchOrdExpensesHF.Where(x => x.CompNo == company.comp_num && x.TransYear == POExpensesHF.TransYear && x.TransNo == POExpensesHF.TransNo).FirstOrDefault();

            if(ex != null)
            {
                ex.TransDate = POExpensesHF.TransDate;
                ex.DeptNo = POExpensesHF.DeptNo;
                ex.AccNo = POExpensesHF.AccNo;
                ex.CurrCode = POExpensesHF.CurrCode;
                ex.ExRate = POExpensesHF.ExRate;
                ex.TotalAmount = POExpensesHF.TotalAmount;
                ex.FrAmount = POExpensesHF.FrAmount;
                ex.Note = POExpensesHF.Note;
                ex.IsPosted = false;
                db.SaveChanges();
            }

            List<Ord_PurchOrdExpensesDF> exDel = db.Ord_PurchOrdExpensesDF.Where(x => x.CompNo == company.comp_num && x.TransYear == POExpensesHF.TransYear
                && x.TransNo == POExpensesHF.TransNo).ToList();

            if(exDel.Count != 0)
            {
                db.Ord_PurchOrdExpensesDF.RemoveRange(exDel);
                db.SaveChanges();
            }


            foreach (Ord_PurchOrdExpensesDF item in POExpensesDF)
            {
                Ord_PurchOrdExpensesDF exDF = new Ord_PurchOrdExpensesDF();
                exDF.CompNo = company.comp_num;
                exDF.TransYear = item.TransYear;
                exDF.TransNo = item.TransNo;
                exDF.OrderYear = item.OrderYear;
                exDF.OrderNo = item.OrderNo;
                exDF.TawreedNo = item.TawreedNo;
                exDF.ExpID = item.ExpID;
                exDF.Amount = item.Amount;
                exDF.FrAmount = item.FrAmount;
                exDF.SalesTaxAmount = item.SalesTaxAmount;
                exDF.SalesTaxAmountFr = item.SalesTaxAmountFr;
                exDF.IncomeTaxAmount = item.IncomeTaxAmount;
                exDF.IncomeTaxAmountFr = item.IncomeTaxAmountFr;
                exDF.CustomtaxAmount = item.CustomtaxAmount;
                exDF.CustomtaxAmountFr = item.CustomtaxAmountFr;
                db.Ord_PurchOrdExpensesDF.Add(exDF);
                db.SaveChanges();
            }

            return Json(new { TransYear = POExpensesHF.TransYear, TransNo = POExpensesHF.TransNo, Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_PurchOrdExpenses(int TransYear, int TransNo)
        {
            List<Ord_PurchOrdExpensesDF> exDel = db.Ord_PurchOrdExpensesDF.Where(x => x.CompNo == company.comp_num && x.TransYear == TransYear
               && x.TransNo == TransNo).ToList();

            if (exDel.Count != 0)
            {
                db.Ord_PurchOrdExpensesDF.RemoveRange(exDel);
                db.SaveChanges();
            }

            Ord_PurchOrdExpensesHF ex = db.Ord_PurchOrdExpensesHF.Where(x => x.CompNo == company.comp_num && x.TransYear == TransYear && x.TransNo == TransNo).FirstOrDefault();
            db.Ord_PurchOrdExpensesHF.Remove(ex);
            db.SaveChanges();

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Post_OrdExpenses(int TransYear, int TransNo)
        {
            List<Ord_PurchOrdExpensesDF> PurchOrdExpensesDF = db.Ord_PurchOrdExpensesDF.Where(x => x.CompNo == company.comp_num &&
            x.TransYear == TransYear && x.TransNo == TransNo).ToList();

            var ExpensesDF = PurchOrdExpensesDF.GroupBy(x => new  { x.OrderYear, x.OrderNo,x.TawreedNo }).ToList();

            int gLcDept = 0;
            long gLcAcc = 0;
            string Seial = "0001";
            foreach (var V in ExpensesDF)
            {
                OrderHF OrdHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == V.Key.OrderYear && x.OrderNo == V.Key.OrderNo && x.TawreedNo == V.Key.TawreedNo).FirstOrDefault();
                if (OrdHF.LcDept.Value == 0 || OrdHF.LcAccNo.Value == 0)
                {
                    if (Language == "ar-JO")
                    {
                        return Json(new { error = "يجب إدخال حساب الطلبيه ل امر شراء رقم : " + " " + V.Key.TawreedNo }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { error = "The Order Account Must Be Entered for the P.O.No: " + " " + V.Key.TawreedNo }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            int gTax_Dept = 0;
            long gTax_acc = 0;
            int gIncomeTaxDept = 0;
            long gIncomeTaxAcc = 0;
            int gCustomTaxDept = 0;
            long gCustomTaxAcc = 0;
            OrdCompMF CompMF = db.OrdCompMFs.Where(x => x.CompNo == company.comp_num).FirstOrDefault();

            if(CompMF.tax_Dept == null || CompMF.tax_Dept == 0 || CompMF.tax_acc == null || CompMF.tax_acc == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب إدخال حساب ضريبة المبيعات" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Sales tax account must be entered" }, JsonRequestBehavior.AllowGet);
                }
            }

            if (CompMF.CustomsTaxDept == null || CompMF.CustomsTaxDept == 0 || CompMF.CustomsTaxAcc == null || CompMF.CustomsTaxAcc == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب إدخال حساب الامانات الجمركية" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "The customs Tax account must be entered" }, JsonRequestBehavior.AllowGet);
                }
            }

            if (CompMF.IncomeTaxDept == null || CompMF.IncomeTaxDept == 0 || CompMF.IncomeTaxAcc == null || CompMF.IncomeTaxAcc == 0)
            {
                if (Language == "ar-JO")
                {
                    return Json(new { error = "يجب إدخال حساب ضريبة الدخل" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Income tax account must be entered" }, JsonRequestBehavior.AllowGet);
                }
            }

            Ord_PurchOrdExpensesHF PurchOrdExpensesHF = db.Ord_PurchOrdExpensesHF.Where(x => x.CompNo == company.comp_num && x.TransYear == TransYear && x.TransNo == TransNo).FirstOrDefault();


            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();

            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();

            DataRow DrTmp;
            DataRow DrTmp1;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                int TranNo = 0;
               
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("SELECT OrdLcExpDf.* FROM dbo.OrdLcExpDf  WHERE CompNo = 0", cn);
                DaPR.Fill(ds, "PermT");

                DaPR1.SelectCommand = new SqlCommand("SELECT glvodmf.* FROM dbo.glvodmf where dbo.glvodmf.[vod_comp] = 0", cn);
                DaPR1.Fill(ds1, "GLVodMF");
                ds1.Tables["GLVodMF"].PrimaryKey = new DataColumn[] { ds1.Tables["GLVodMF"].Columns["vod_srl"], ds1.Tables["GLVodMF"].Columns["vod_dep"], ds1.Tables["GLVodMF"].Columns["vod_acc"] };
                int i = 1;

                transaction = cn.BeginTransaction();

                int VNo = GetTransSerial(DateTime.Now.Year, 56, 0, DateTime.Now, PurchOrdExpensesHF.DeptNo.Value, transaction, cn);

                using (SqlCommand CmdSr = new SqlCommand())
                {
                    CmdSr.Connection = cn;
                    CmdSr.CommandText = "Gln_ManageVouSrial2";
                    CmdSr.CommandType = CommandType.StoredProcedure;
                    CmdSr.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdSr.Parameters.Add(new SqlParameter("@VYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    CmdSr.Parameters.Add(new SqlParameter("@VType", SqlDbType.SmallInt)).Value =56;
                    CmdSr.Parameters.Add(new SqlParameter("@VouNo", SqlDbType.SmallInt)).Value = VNo;
                    CmdSr.Parameters.Add(new SqlParameter("@SerDeptNo", SqlDbType.SmallInt)).Value = PurchOrdExpensesHF.DeptNo;
                    CmdSr.Transaction = transaction;
                    CmdSr.ExecuteNonQuery();
                }

                foreach (var V in ExpensesDF)
                {
                   decimal SalesTaxAmount = 0;
                   decimal SalesTaxAmountFr =0;
                   decimal IncomeTaxAmount = 0;
                   decimal IncomeTaxAmountFr = 0;
                   decimal CustomtaxAmount = 0;
                   decimal CustomtaxAmountFr = 0;

                    TranNo = Convert.ToInt32(Convert.ToString(PurchOrdExpensesHF.TransNo) + "" + Seial);

                    List<Ord_PurchOrdExpensesDF> OrdExpensesDF = db.Ord_PurchOrdExpensesDF.Where(x => x.CompNo == company.comp_num &&
                    x.TransYear == TransYear && x.TransNo == TransNo && x.OrderYear == V.Key.OrderYear && x.OrderNo == V.Key.OrderNo && x.TawreedNo == V.Key.TawreedNo).ToList();

                    foreach (Ord_PurchOrdExpensesDF item in OrdExpensesDF)
                    {
                        DrTmp = ds.Tables["PermT"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["CompNo"] = company.comp_num;
                        DrTmp["TransYear"] = PurchOrdExpensesHF.TransYear;
                        DrTmp["TransNo"] = TranNo;
                        DrTmp["ExpID"] = item.ExpID;
                        DrTmp["Amount"] = item.Amount;
                        DrTmp["FrAmount"] = item.FrAmount;

                        SalesTaxAmount = SalesTaxAmount + item.SalesTaxAmount.Value;
                        SalesTaxAmountFr = SalesTaxAmountFr + item.SalesTaxAmountFr.Value;

                        IncomeTaxAmount = IncomeTaxAmount + item.IncomeTaxAmount.Value;
                        IncomeTaxAmountFr = IncomeTaxAmountFr + item.IncomeTaxAmountFr.Value;

                        CustomtaxAmount = CustomtaxAmount + item.CustomtaxAmount.Value;
                        CustomtaxAmountFr = CustomtaxAmountFr + item.CustomtaxAmountFr.Value;

                        DrTmp.EndEdit();
                        ds.Tables["PermT"].Rows.Add(DrTmp);
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        OrderHF OrdHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == V.Key.OrderYear && x.OrderNo == V.Key.OrderNo && x.TawreedNo == V.Key.TawreedNo).FirstOrDefault();

                        cmd.Connection = cn;
                        cmd.CommandText = "Ord_AddOrderLcExpHf";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                        cmd.Parameters.Add(new SqlParameter("@TransYear", SqlDbType.SmallInt)).Value = PurchOrdExpensesHF.TransYear;
                        cmd.Parameters.Add(new SqlParameter("@TransNo", SqlDbType.Int)).Value = TranNo;
                        cmd.Parameters.Add(new SqlParameter("@TransDate", SqlDbType.SmallDateTime)).Value = PurchOrdExpensesHF.TransDate;
                        cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = V.Key.OrderYear;
                        cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = V.Key.OrderNo;
                        cmd.Parameters.Add(new SqlParameter("@AccDep", SqlDbType.Int)).Value = OrdHF.LcDept;
                        cmd.Parameters.Add(new SqlParameter("@AccNo", SqlDbType.BigInt)).Value = OrdHF.LcAccNo;
                        cmd.Parameters.Add(new SqlParameter("@CurrType", SqlDbType.Int)).Value = PurchOrdExpensesHF.CurrCode;
                        cmd.Parameters.Add(new SqlParameter("@ExRate", SqlDbType.Money)).Value = PurchOrdExpensesHF.ExRate;
                        cmd.Parameters.Add(new SqlParameter("@DeptDr", SqlDbType.Int)).Value = PurchOrdExpensesHF.DeptNo;
                        cmd.Parameters.Add(new SqlParameter("@AccDr", SqlDbType.BigInt)).Value = PurchOrdExpensesHF.AccNo;
                        cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = V.Key.TawreedNo;

                        if(PurchOrdExpensesHF.Note == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = "سند مصاريف اجمالية من نظام المشتريات الويب -  " + PurchOrdExpensesHF.TransNo;

                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar)).Value = PurchOrdExpensesHF.Note + "   " + "سند مصاريف اجمالية من نظام المشتريات الويب - " + PurchOrdExpensesHF.TransNo;
                        }

                        cmd.Parameters.Add(new SqlParameter("@taxval", SqlDbType.Money)).Value = SalesTaxAmount;
                        cmd.Parameters.Add(new SqlParameter("@taxvalF", SqlDbType.Money)).Value = SalesTaxAmountFr;
                        cmd.Parameters.Add(new SqlParameter("@IncomeTaxVal", SqlDbType.Money)).Value = IncomeTaxAmount;
                        cmd.Parameters.Add(new SqlParameter("@IncomeTaxValF", SqlDbType.Money)).Value = IncomeTaxAmountFr;
                        cmd.Parameters.Add(new SqlParameter("@taxValcustoms", SqlDbType.Money)).Value = CustomtaxAmount;
                        cmd.Parameters.Add(new SqlParameter("@taxValcustomsF", SqlDbType.Money)).Value = CustomtaxAmountFr;
                        cmd.Parameters.Add(new SqlParameter("@InvSr", SqlDbType.SmallInt)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@IsPostWeb", SqlDbType.SmallInt)).Value = 1;
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

                    

                    using (SqlCommand cmdDF = new SqlCommand())
                    {
                        cmdDF.Connection = cn;
                        cmdDF.CommandText = "Ord_AddOrderLcExpDf";
                        cmdDF.CommandType = CommandType.StoredProcedure;
                        cmdDF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        cmdDF.Parameters.Add(new SqlParameter("@TransYear", SqlDbType.SmallInt, 4, "TransYear"));
                        cmdDF.Parameters.Add(new SqlParameter("@TransNo", SqlDbType.Int, 9, "TransNo"));
                        cmdDF.Parameters.Add(new SqlParameter("@ExpID", SqlDbType.SmallInt, 9, "ExpID"));
                        cmdDF.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Money, 8, "Amount"));
                        cmdDF.Parameters.Add(new SqlParameter("@FrAmount", SqlDbType.Money, 8, "FrAmount"));
                        cmdDF.Transaction = transaction;
                        DaPR.InsertCommand = cmdDF;
                        try
                        {
                            DaPR.Update(ds, "PermT");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            cn.Dispose();
                            return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    Seial = "000" + Convert.ToString(Convert.ToInt32(Seial) + Convert.ToInt32("0001"));
                }

                int iden = Gln_VodMFLOGH(TransYear, 56, TransNo, PurchOrdExpensesHF.TransDate.Value, transaction, cn);

                decimal SaleTaxAmount = 0;
                decimal SaleTaxAmountFr = 0;
                decimal IncomTaxAmount = 0;
                decimal IncomTaxAmountFr = 0;
                decimal CustotaxAmount = 0;
                decimal CustotaxAmountFr = 0;

                foreach (var V in ExpensesDF)
                {
                    decimal Amount = 0;
                    decimal AmountFr = 0;
                    

                    OrderHF OrdHF = db.OrderHFs.Where(x => x.CompNo == company.comp_num && x.OrdYear == V.Key.OrderYear && x.OrderNo == V.Key.OrderNo && x.TawreedNo == V.Key.TawreedNo).FirstOrDefault();

                    List<Ord_PurchOrdExpensesDF> OrdExpensesDF = db.Ord_PurchOrdExpensesDF.Where(x => x.CompNo == company.comp_num &&
                    x.TransYear == TransYear && x.TransNo == TransNo && x.OrderYear == V.Key.OrderYear && x.OrderNo == V.Key.OrderNo && x.TawreedNo == V.Key.TawreedNo).ToList();

                    foreach (Ord_PurchOrdExpensesDF item in OrdExpensesDF)
                    {
                        Amount += item.Amount.Value;
                        AmountFr += item.FrAmount.Value;
                        SaleTaxAmount = SaleTaxAmount + item.SalesTaxAmount.Value;
                        SaleTaxAmountFr = SaleTaxAmountFr + item.SalesTaxAmountFr.Value;

                        IncomTaxAmount = IncomTaxAmount + item.IncomeTaxAmount.Value;
                        IncomTaxAmountFr = IncomTaxAmountFr + item.IncomeTaxAmountFr.Value;

                        CustotaxAmount = CustotaxAmount + item.CustomtaxAmount.Value;
                        CustotaxAmountFr = CustotaxAmountFr + item.CustomtaxAmountFr.Value;
                    }

                    DrTmp1 = ds1.Tables["GLVodMF"].NewRow();
                    DrTmp1.BeginEdit();
                    DrTmp1["vod_comp"] = company.comp_num;
                    DrTmp1["vod_year"] = TransYear;
                    DrTmp1["vod_type"] = 56;
                    DrTmp1["vod_num"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_srl"] = i;
                    DrTmp1["vod_dep"] = OrdHF.LcDept.Value;
                    DrTmp1["vod_acc"] = OrdHF.LcAccNo.Value;
                    DrTmp1["vod_date"] = PurchOrdExpensesHF.TransDate.Value.ToShortDateString();
                    DrTmp1["vod_side"] = 1;
                    DrTmp1["vod_amount"] = Amount;
                    DrTmp1["vod_for_amount"] = AmountFr;
                    DrTmp1["vod_doctype"] = 1;
                    DrTmp1["vod_docnum"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_remark"] = "ستد قيد مصاريف للطلب شراء رقم : " + V.Key.OrderNo + " وامر شراء رقم : " + V.Key.TawreedNo;
                    DrTmp1["Emp_Collect"] = 0;
                    DrTmp1.EndEdit();
                    ds1.Tables["GLVodMF"].Rows.Add(DrTmp1);
                    i++;
                }

                DrTmp1 = ds1.Tables["GLVodMF"].NewRow();
                DrTmp1.BeginEdit();
                DrTmp1["vod_comp"] = company.comp_num;
                DrTmp1["vod_year"] = TransYear;
                DrTmp1["vod_type"] = 56;
                DrTmp1["vod_num"] = PurchOrdExpensesHF.TransNo;
                DrTmp1["vod_srl"] = i;
                DrTmp1["vod_dep"] = PurchOrdExpensesHF.DeptNo.Value;
                DrTmp1["vod_acc"] = PurchOrdExpensesHF.AccNo.Value;
                DrTmp1["vod_date"] = PurchOrdExpensesHF.TransDate.Value.ToShortDateString();
                DrTmp1["vod_side"] = 0;
                DrTmp1["vod_amount"] = PurchOrdExpensesHF.TotalAmount.Value + Convert.ToDouble(SaleTaxAmount + IncomTaxAmount + CustotaxAmount);
                DrTmp1["vod_for_amount"] = PurchOrdExpensesHF.FrAmount.Value + Convert.ToDouble(SaleTaxAmountFr + IncomTaxAmountFr + CustotaxAmountFr);
                DrTmp1["vod_doctype"] = 1;
                DrTmp1["vod_docnum"] = PurchOrdExpensesHF.TransNo;
                DrTmp1["vod_remark"] = "مصاريف فيد من الويب برقم حركة : " + TransNo;
                DrTmp1["Emp_Collect"] = 0;
                DrTmp1.EndEdit();
                ds1.Tables["GLVodMF"].Rows.Add(DrTmp1);

                if (SaleTaxAmount > 0)
                {
                    i++;
                    DrTmp1 = ds1.Tables["GLVodMF"].NewRow();
                    DrTmp1.BeginEdit();
                    DrTmp1["vod_comp"] = company.comp_num;
                    DrTmp1["vod_year"] = TransYear;
                    DrTmp1["vod_type"] = 56;
                    DrTmp1["vod_num"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_srl"] = i;
                    DrTmp1["vod_dep"] = CompMF.tax_Dept.Value;
                    DrTmp1["vod_acc"] = CompMF.tax_acc.Value;
                    DrTmp1["vod_date"] = PurchOrdExpensesHF.TransDate.Value.ToShortDateString();
                    DrTmp1["vod_side"] = 1;
                    DrTmp1["vod_amount"] = SaleTaxAmount;
                    DrTmp1["vod_for_amount"] = SaleTaxAmountFr;
                    DrTmp1["vod_doctype"] = 1;
                    DrTmp1["vod_docnum"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_remark"] = "ستد قيد مصاريف ضريبة المبيعات";
                    DrTmp1["Emp_Collect"] = 0;
                    DrTmp1.EndEdit();
                    ds1.Tables["GLVodMF"].Rows.Add(DrTmp1);
                }

                if (IncomTaxAmount > 0)
                {
                    i++;
                    DrTmp1 = ds1.Tables["GLVodMF"].NewRow();
                    DrTmp1.BeginEdit();
                    DrTmp1["vod_comp"] = company.comp_num;
                    DrTmp1["vod_year"] = TransYear;
                    DrTmp1["vod_type"] = 56;
                    DrTmp1["vod_num"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_srl"] = i;
                    DrTmp1["vod_dep"] = CompMF.IncomeTaxDept.Value;
                    DrTmp1["vod_acc"] = CompMF.IncomeTaxAcc.Value;
                    DrTmp1["vod_date"] = PurchOrdExpensesHF.TransDate.Value.ToShortDateString();
                    DrTmp1["vod_side"] = 1;
                    DrTmp1["vod_amount"] = IncomTaxAmount;
                    DrTmp1["vod_for_amount"] = IncomTaxAmountFr;
                    DrTmp1["vod_doctype"] = 1;
                    DrTmp1["vod_docnum"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_remark"] = "ستد قيد مصاريف ضريبة الدخل";
                    DrTmp1["Emp_Collect"] = 0;
                    DrTmp1.EndEdit();
                    ds1.Tables["GLVodMF"].Rows.Add(DrTmp1);
                }

                if (CustotaxAmount > 0)
                {
                    i++;
                    DrTmp1 = ds1.Tables["GLVodMF"].NewRow();
                    DrTmp1.BeginEdit();
                    DrTmp1["vod_comp"] = company.comp_num;
                    DrTmp1["vod_year"] = TransYear;
                    DrTmp1["vod_type"] = 56;
                    DrTmp1["vod_num"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_srl"] = i;
                    DrTmp1["vod_dep"] = CompMF.CustomsTaxDept.Value;
                    DrTmp1["vod_acc"] = CompMF.CustomsTaxAcc.Value;
                    DrTmp1["vod_date"] = PurchOrdExpensesHF.TransDate.Value.ToShortDateString();
                    DrTmp1["vod_side"] = 1;
                    DrTmp1["vod_amount"] = CustotaxAmount;
                    DrTmp1["vod_for_amount"] = CustotaxAmountFr;
                    DrTmp1["vod_doctype"] = 1;
                    DrTmp1["vod_docnum"] = PurchOrdExpensesHF.TransNo;
                    DrTmp1["vod_remark"] = "ستد قيد مصاريف الامانات الجمركية";
                    DrTmp1["Emp_Collect"] = 0;
                    DrTmp1.EndEdit();
                    ds1.Tables["GLVodMF"].Rows.Add(DrTmp1);
                }

                using (SqlCommand cmdDF = new SqlCommand())
                {
                    cmdDF.Connection = cn;
                    cmdDF.CommandText = "GLN_AddGlVodMF";
                    cmdDF.CommandType = CommandType.StoredProcedure;
                    cmdDF.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "vod_comp"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouYear", SqlDbType.SmallInt, 4, "vod_year"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt, 4, "vod_type"));
                    cmdDF.Parameters.Add(new SqlParameter("@Vouno", SqlDbType.Int, 9, "vod_num"));
                    cmdDF.Parameters.Add(new SqlParameter("@Srl", SqlDbType.SmallInt, 9, "vod_srl"));
                    cmdDF.Parameters.Add(new SqlParameter("@Dep", SqlDbType.Int, 9, "vod_dep"));
                    cmdDF.Parameters.Add(new SqlParameter("@accno", SqlDbType.BigInt, 8, "vod_acc"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouDate", SqlDbType.SmallDateTime, 10, "vod_date"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouSide", SqlDbType.Bit, 1, "vod_side"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouAmnt", SqlDbType.Float, 9, "vod_amount"));
                    cmdDF.Parameters.Add(new SqlParameter("@DocType", SqlDbType.SmallInt, 4, "vod_doctype"));
                    cmdDF.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar, 20, "vod_docnum"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouFAmnt", SqlDbType.Float, 9, "vod_for_amount"));
                    cmdDF.Parameters.Add(new SqlParameter("@VouRem", SqlDbType.VarChar, 255, "vod_remark"));
                    cmdDF.Parameters.Add(new SqlParameter("@EmpCollect", SqlDbType.SmallInt, 4, "Emp_Collect"));
                    cmdDF.Parameters.Add(new SqlParameter("@ModuleID", SqlDbType.VarChar, 10)).Value = "ORD";
                    cmdDF.Parameters.Add(new SqlParameter("@UserId", SqlDbType.VarChar, 10)).Value = me.UserID;
                    cmdDF.Parameters.Add(new SqlParameter("@TrType", SqlDbType.SmallInt)).Value = 1;
                    cmdDF.Parameters.Add(new SqlParameter("@TrNo", SqlDbType.Int)).Value = iden;
                    cmdDF.Parameters.Add(new SqlParameter("@TrMod", SqlDbType.Int)).Value = 1;
                    cmdDF.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    cmdDF.Transaction = transaction;
                    DaPR1.InsertCommand = cmdDF;
                    try
                    {
                        DaPR1.Update(ds1, "GLVodMF");
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
            }

            PurchOrdExpensesHF.IsPosted = true;
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }

        public int Gln_VodMFLOGH(int TransYear,int VouType,int TransNo, DateTime TransDate, SqlTransaction MyTrans, SqlConnection co)
        {
            int RID = 0;
            using (SqlCommand Cmd = new SqlCommand())
            {
                Cmd.Connection = co;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "Gln_AddVodMFLog";
                Cmd.Parameters.Add("@Vod_comp", SqlDbType.SmallInt).Value = company.comp_num;
                Cmd.Parameters.Add("@Vod_year", SqlDbType.SmallInt).Value = TransYear;
                Cmd.Parameters.Add("@Vod_type", SqlDbType.SmallInt).Value = VouType;
                Cmd.Parameters.Add("@Vod_num", SqlDbType.Int).Value = TransNo;
                Cmd.Parameters.Add("@Vod_entry", SqlDbType.DateTime).Value = TransDate;
                Cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = me.UserID;
                Cmd.Parameters.Add("@TrType", SqlDbType.SmallInt).Value = 1;
                Cmd.Parameters.Add(new SqlParameter("@RID", SqlDbType.Int)).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
                try
                {
                    Cmd.ExecuteNonQuery();
                    RID = Convert.ToInt32(Cmd.Parameters["@RID"].Value);
                }
                catch (Exception ex)
                {
                    RID = 0;
                }
            }
            return RID;
        }
    }
}