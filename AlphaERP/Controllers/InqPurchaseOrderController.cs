using CrystalDecisions.Shared;
using Microsoft.VisualBasic;
using AlphaERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class InqPurchaseOrderController : controller
    {
        string rr;
        string hh;
        // GET: InqPurchaseOrder
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
        public ActionResult GenerateReport(int OrdYear, int OrderNo, string TawreedNo)
        {
            ReportInformation reportInfo = new ReportInformation();
            DataSet Alpha_ERP_DataSet = new DataSet();
          

            DataTable d1 = Ord_PurchOrderInfo(OrdYear, OrderNo, TawreedNo);
            d1.TableName = "Ord_PurchOrderInfo";
            Alpha_ERP_DataSet.Tables.Add(d1);

            string StrAmount = Convert.ToString(d1.Rows[0]["StrAmount"]);

            DataTable d2 = Ord_RptCondsGuaranties(OrdYear, OrderNo, TawreedNo);
            d2.TableName = "Ord_RptCondsGuaranties";
            Alpha_ERP_DataSet.Tables.Add(d2);

            DataTable d3 = Ord_RptPurchOrderShipments(OrdYear, OrderNo, TawreedNo);
            d3.TableName = "Ord_RptPurchOrderShipments";
            Alpha_ERP_DataSet.Tables.Add(d3);


            DataTable d4 = Ord_RptGetPOAttachments(OrdYear, OrderNo, TawreedNo);
            d4.TableName = "Ord_RptGetPOAttachments";
            Alpha_ERP_DataSet.Tables.Add(d4);

            DataTable d5 = Ord_PurchOrderBarcode(OrdYear, OrderNo);
            d5.TableName = "Ord_PurchOrderBarcode";
            Alpha_ERP_DataSet.Tables.Add(d5);

            DataTable d6 = Ord_GetRptCust();
            d6.TableName = "MyTable";
            Alpha_ERP_DataSet.Tables.Add(d6);

            DataTable d7 = Comp_FillImage();
            var img = d7.Rows[0]["comp_logo"];
            d7.TableName = "Comp_FillImage";
            Alpha_ERP_DataSet.Tables.Add(d7);

            reportInfo.myDataSet = Alpha_ERP_DataSet;

            reportInfo.fields = SetParameterField(d6, StrAmount, d2.Rows.Count);

            reportInfo.ReportName = "أمر الشراء";
            ClientsActive CActive = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault();
            //CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            //string path = System.Web.HttpContext.Current.Server.MapPath("~/Reports/Ordering/Arabic_Report/RptPurchOrderInfoKASIH.rpt");
            //rpt.Load(path);
            //rpt.SetDataSource(Alpha_ERP_DataSet);
            //Stream st = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
            //st.Flush();
            //rpt.Close();
            //rpt.Dispose();
            //return File(st, System.Net.Mime.MediaTypeNames.Application.Pdf);

            if (Language == "ar-JO")
            {
                //if(CActive.ClientNo == 239)
                //{
                //    reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptPurchOrderInfoKASIH.rpt");
                //}
                //else
                //{
                //    reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptPurchOrderInfo.rpt");
                //}
                reportInfo.path = Server.MapPath("~/Reports/Ordering/Arabic_Report/RptPurchOrderInfo.rpt");
            }
            else
            {
                //if(CActive.ClientNo == 239)
                //{
                //    reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptPurchOrderInfoKASIHEnf.rpt");
                //}
                //else
                //{
                //    reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptPurchOrderInfoEng.rpt");
                //}
                reportInfo.path = Server.MapPath("~/Reports/Ordering/English_Report/RptPurchOrderInfoEng.rpt");
            }

            string reportUniqueName = CreateReportUniqueId(reportInfo.path);
            ReportInfoManager.AddReport(reportUniqueName, reportInfo);

            this.HttpContext.Session["reportInfoID"] = reportInfo.Id;
            return RedirectToAction("ReportViewer", "CrystalReportViewer", new { area = "", id = reportInfo.Id });
        }
        public ParameterFields SetParameterField(DataTable d7,string StrAmount,int CondsGuaranties = 0)
        {
            ClientsActive c = db.ClientsActives.Where(x => x.CompNo == company.comp_num).FirstOrDefault(); 
            ParameterFields parameterFields = new ParameterFields();
            /****************************************************/

            /******************Param1*****************/
            ParameterField param1 = new ParameterField();
            param1.Name = "Para1";
            ParameterDiscreteValue discreteVal1 = new ParameterDiscreteValue();
            if(d7.Rows[0]["desc"] == DBNull.Value)
            {
                discreteVal1.Value = "";
            }
            else
            {
                discreteVal1.Value = d7.Rows[0]["desc"];
            }
            param1.CurrentValues.Add(discreteVal1);
            parameterFields.Add(param1);
            /******************Param2*****************/
            ParameterField param2 = new ParameterField();
            param2.Name = "para2";
            ParameterDiscreteValue discreteVal2 = new ParameterDiscreteValue();
            if (d7.Rows[1]["desc"] == DBNull.Value)
            {
                discreteVal2.Value = "";
            }
            else
            {
                discreteVal2.Value = d7.Rows[1]["desc"];
            }
            param2.CurrentValues.Add(discreteVal2);
            parameterFields.Add(param2);
            /******************Param3*****************/
            ParameterField param3 = new ParameterField();
            param3.Name = "para3";
            ParameterDiscreteValue discreteVal3 = new ParameterDiscreteValue();
            if (d7.Rows[2]["desc"] == DBNull.Value)
            {
                discreteVal3.Value = "";
            }
            else
            {
                discreteVal3.Value = d7.Rows[2]["desc"];
            }
            param3.CurrentValues.Add(discreteVal3);
            parameterFields.Add(param3);
            /******************Param4*****************/
            ParameterField param4 = new ParameterField();
            param4.Name = "para4";
            ParameterDiscreteValue discreteVal4 = new ParameterDiscreteValue();
            if (d7.Rows[3]["desc"] == DBNull.Value)
            {
                discreteVal4.Value = "";
            }
            else
            {
                discreteVal4.Value = d7.Rows[3]["desc"];
            }
            param4.CurrentValues.Add(discreteVal4);
            parameterFields.Add(param4);
            /******************Param5*****************/
            ParameterField param5 = new ParameterField();
            param5.Name = "para5";
            ParameterDiscreteValue discreteVal5 = new ParameterDiscreteValue();
            if (d7.Rows[4]["desc"] == DBNull.Value)
            {
                discreteVal5.Value = "";
            }
            else
            {
                discreteVal5.Value = d7.Rows[4]["desc"];
            }
            param5.CurrentValues.Add(discreteVal5);
            parameterFields.Add(param5);
            /******************para6*****************/
            ParameterField para6 = new ParameterField();
            para6.Name = "para6";
            ParameterDiscreteValue discreteVal6 = new ParameterDiscreteValue();
            if (d7.Rows[5]["desc"] == DBNull.Value)
            {
                discreteVal6.Value = "";
            }
            else
            {
                discreteVal6.Value = d7.Rows[5]["desc"];
            }
            para6.CurrentValues.Add(discreteVal6);
            parameterFields.Add(para6);
            /******************AmtString*****************/
            ParameterField AmtString = new ParameterField();
            AmtString.Name = "AmtString";
            ParameterDiscreteValue discreteVal7 = new ParameterDiscreteValue();
            discreteVal7.Value = StrAmount;
            AmtString.CurrentValues.Add(discreteVal7);
            parameterFields.Add(AmtString);

            /******************UserName*****************/
            ParameterField UserName = new ParameterField();
            UserName.Name = "UserName";
            ParameterDiscreteValue discreteVal8 = new ParameterDiscreteValue();
            discreteVal8.Value = me.UserID;
            UserName.CurrentValues.Add(discreteVal8);
            parameterFields.Add(UserName);
            /******************CompName*****************/
            ParameterField CompName = new ParameterField();
            CompName.Name = "CompName";
            ParameterDiscreteValue discreteVal9 = new ParameterDiscreteValue();
            discreteVal9.Value = company.comp_name;
            CompName.CurrentValues.Add(discreteVal9);
            parameterFields.Add(CompName);
            /******************OfferNo*****************/
            ParameterField OfferNo = new ParameterField();
            OfferNo.Name = "OfferNo";
            ParameterDiscreteValue discreteVal10 = new ParameterDiscreteValue();
            discreteVal10.Value = 0;
            OfferNo.CurrentValues.Add(discreteVal10);
            parameterFields.Add(OfferNo);
            /******************BonusShow*****************/
            ParameterField BonusShow = new ParameterField();
            BonusShow.Name = "BonusShow";
            ParameterDiscreteValue discreteVal11 = new ParameterDiscreteValue();
            discreteVal11.Value = true;
            BonusShow.CurrentValues.Add(discreteVal11);
            parameterFields.Add(BonusShow);
            /******************DiscShow*****************/
            ParameterField DiscShow = new ParameterField();
            DiscShow.Name = "DiscShow";
            ParameterDiscreteValue discreteVal12 = new ParameterDiscreteValue();
            discreteVal12.Value = true;
            DiscShow.CurrentValues.Add(discreteVal12);
            parameterFields.Add(DiscShow);
            /******************ISOCode*****************/
            ParameterField ISOCode = new ParameterField();
            ISOCode.Name = "ISOCode";
            ParameterDiscreteValue discreteValISOCode = new ParameterDiscreteValue();
            discreteValISOCode.Value = "PO00001122";
            ISOCode.CurrentValues.Add(discreteValISOCode);
            parameterFields.Add(ISOCode);
            /******************gClientNo*****************/
            ParameterField gClientNo = new ParameterField();
            gClientNo.Name = "gClientNo";
            ParameterDiscreteValue discreteValgClientNo = new ParameterDiscreteValue();
            discreteValgClientNo.Value = c.ClientNo;
            gClientNo.CurrentValues.Add(discreteValgClientNo);
            parameterFields.Add(gClientNo);
            /******************CurrFraction*****************/
            ParameterField CurrFraction = new ParameterField();
            CurrFraction.Name = "CurrFraction";
            ParameterDiscreteValue discreteValCurrFraction = new ParameterDiscreteValue();
            discreteValCurrFraction.Value = 3;
            CurrFraction.CurrentValues.Add(discreteValCurrFraction);
            parameterFields.Add(CurrFraction);
            /******************ExtraExp*****************/
            ParameterField ExtraExp = new ParameterField();
            ExtraExp.Name = "ExtraExp";
            ParameterDiscreteValue discreteValExtraExp = new ParameterDiscreteValue();
            discreteValExtraExp.Value = 0;
            ExtraExp.CurrentValues.Add(discreteValExtraExp);
            parameterFields.Add(ExtraExp);
            /************************/
            /******************ExtraExp*****************/
            ParameterField CondsGuarantie = new ParameterField();
            CondsGuarantie.Name = "CondsGuarantie";
            ParameterDiscreteValue discreteValCondsGuarantie = new ParameterDiscreteValue();
            discreteValCondsGuarantie.Value = CondsGuaranties;
            CondsGuarantie.CurrentValues.Add(discreteValCondsGuarantie);
            parameterFields.Add(CondsGuarantie);
            /************************/
            return parameterFields;
        }
        public DataTable Ord_RptCondsGuaranties(int OrdYear, int OrderNo, string TawreedNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptCondsGuaranties";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TwareedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;

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

        public DataTable Ord_RptPurchOrderShipments(int OrdYear, int OrderNo, string TawreedNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptPurchOrderShipments";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TwareedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;

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

        public DataTable Ord_PurchOrderInfo(int OrdYear, int OrderNo, string TawreedNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_PurchOrderInfo";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TwareedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
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

        public DataTable Ord_RptGetPOAttachments(int OrdYear, int OrderNo, string TawreedNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_RptGetPOAttachments";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.BigInt)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@TawreedNo", System.Data.SqlDbType.VarChar)).Value = TawreedNo;
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

        public DataTable Ord_PurchOrderBarcode(int OrdYear, int OrderNo)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_PurchOrderBarcode";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", System.Data.SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", System.Data.SqlDbType.Int)).Value = OrderNo;

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

        public DataTable Ord_GetRptCust()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_GetRptCust";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@RptType", System.Data.SqlDbType.SmallInt)).Value = 2;
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

        public ActionResult InqPurchaseOrderList(int OrdYear, int PurchaseType)
        {
            ViewBag.OrdYear = OrdYear;


            if (PurchaseType == 0)
            {
                return PartialView("InqPurchOrdDirectList");
            }
            else
            {
                return PartialView("InqPurchOrdInDirectList");
            }
        }
        public ActionResult InqPurchOrdDirect(int OrdYear, int OrderNo, string TawreedNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            return View();
        }
        public ActionResult InqPurchOrdInDirect(int OrdYear, int OrderNo, string TawreedNo, int OfferNo)
        {
            ViewBag.OrdYear = OrdYear;
            ViewBag.OrdNo = OrderNo;
            ViewBag.TawreedNo = TawreedNo;
            ViewBag.OfferNo = OfferNo;
            return View();
        }
        public ActionResult InsertFile(int OrdYear, int OrderNo, string TawreedNo,int Srl)
        {
            AttachmentFiles model = new AttachmentFiles();

            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Ord_GetAttachmentFiles_ById";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@TawreedNo", SqlDbType.VarChar)).Value = TawreedNo;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.BigInt)).Value = OrderNo;
                    cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                    cmd.Parameters.Add(new SqlParameter("@Serial", SqlDbType.BigInt)).Value = Srl;

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            model = Dt.AsEnumerable().Select(row => new AttachmentFiles
            {
                Serial = Convert.ToInt64(row["Serial"]),
                FileName = Convert.ToString(row["FileName"]),
                DateUploded = Convert.ToString(row["DateUploded"]),
                FileSize = Convert.ToInt32(row["FileSize"]),
                ContentType = Convert.ToString(row["ContentType"]),
                ArchiveData = (byte[])((byte[])row["FileData"])
            }).FirstOrDefault();

            AttachmentFiles Acrh = model;

            var File = Acrh;

            byte[] fileBytes = File.ArchiveData;
            string FileContent = File.ContentType;
            string FileName = File.FileName;
            MemoryStream ms = new MemoryStream(fileBytes, 0, 0, true, true);
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, FileContent);
        }

    }
}