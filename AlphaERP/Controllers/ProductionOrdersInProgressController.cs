using Microsoft.VisualBasic;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlphaERP.Hubs;
using Microsoft.AspNet.SignalR;

namespace AlphaERP.Controllers
{
    public class ProductionOrdersInProgressController : controller
    {
        // GET: ProductionOrdersInProgress
        public DataTable TmpDt;
        public DataTable TmpDtD;
        public DataSet dataSet;
        private int MaxQCTrNo1;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }


        public ActionResult LoadOrderStages(short CompNo,int OrderYear, int OrderNo, string FormCode)
        {
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.FormCode = FormCode;
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsOrderstages = new DataSet();
            var DaOrderSatges = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_GetOrderStagesStatus_web";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            DaOrderSatges.SelectCommand = Cmd;
            DsOrderstages.Tables.Clear();
            DsOrderstages.Dispose();
            DaOrderSatges.Fill(DsOrderstages, "OrderStages");
            return PartialView(DsOrderstages);
        }
        public ActionResult LoadProductionOrder(short CompNo, int OrderYear, int OrderNo, string FormCode )
        {
            if (!CheckProdPermission(1)) return Content(Translate("ليس لديك صلاحية للدخول لهذه الشاشة", "You don't have permission to access this screen"));
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.FormCode = FormCode;
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsOrderstages = new DataSet();
            var DaOrderSatges = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_GetOrderStagesStatus_web";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            DaOrderSatges.SelectCommand = Cmd;
            DsOrderstages.Tables.Clear();
            DsOrderstages.Dispose();
            DaOrderSatges.Fill(DsOrderstages, "OrderStages");
            return PartialView("~/Views/ProductionOrdersInProgress/LoadProductionOrder.cshtml", DsOrderstages);

        }
        public ActionResult DailyProdListANDAllDailyProduction(short CompNo, int OrderYear, int OrderNo, string FormCode)
        {
            if (!CheckProdPermission(2)) return Content(Translate("ليس لديك صلاحية للدخول لهذه الشاشة", "You don't have permission to access this screen"));
            List<ProductionReceivingView> DailyProdHf = db.Database.SqlQuery<ProductionReceivingView>("exec ProdCost_OrderCostDetails_CostDriver @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}", CompNo, OrderYear, OrderNo).ToList();
            List<DailyProductionH> DailyProductionH = db.Database.SqlQuery<DailyProductionH>(string.Format("SELECT ProdCost_DailyProductionH.CompNo, ProdCost_DailyProductionH.ReportYear, ProdCost_DailyProductionH.ReportNo, ProdCost_DailyProductionD.ProdPrepYear, ProdCost_DailyProductionD.ProdPrepNo,ProdCost_DailyProductionH.Prod_Date, ProdCost_DailyProductionH.Closed FROM ProdCost_DailyProductionH INNER JOIN ProdCost_DailyProductionD ON ProdCost_DailyProductionH.CompNo = ProdCost_DailyProductionD.CompNo AND ProdCost_DailyProductionH.ReportYear = ProdCost_DailyProductionD.ReportYear AND ProdCost_DailyProductionH.ReportNo = ProdCost_DailyProductionD.ReportNo GROUP BY ProdCost_DailyProductionH.CompNo, ProdCost_DailyProductionH.ReportYear, ProdCost_DailyProductionH.ReportNo, ProdCost_DailyProductionD.ProdPrepYear, ProdCost_DailyProductionD.ProdPrepNo,ProdCost_DailyProductionH.Prod_Date, ProdCost_DailyProductionH.Closed HAVING(ProdCost_DailyProductionH.CompNo = {0}) AND(ProdCost_DailyProductionD.ProdPrepYear = {1}) AND(ProdCost_DailyProductionD.ProdPrepNo = {2})", CompNo, OrderYear, OrderNo)).ToList();
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.FormCode = FormCode;
            ViewBag.DailyProductionH = DailyProductionH;
            return PartialView("~/Views/ProductionOrdersInProgress/tabs/DailyProdListANDAllDailyProduction.cshtml", DailyProdHf);
        }
        public ActionResult LoadViewAndRFITab(short CompNo, int OrderYear, int OrderNo, string FormCode)
        {
            if (!CheckProdPermission(3)) return Content(Translate("ليس لديك صلاحية للدخول لهذه الشاشة", "You don't have permission to access this screen"));
            short ReqOption = 1;
            List<ViewRawMateria> listData = db.Database.SqlQuery<ViewRawMateria>("exec ProdCost_OrderCostDetails_CostDriver @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}, @ReqOption = {3}", CompNo, OrderYear, OrderNo, ReqOption).ToList();
            string queryString = "SELECT  CompNo, PrepYear, PrepNo, OrdYear, OrdNo, StoreNo, OrdDate, DocNo, BusUnitID, ByStage,AddititiveItems,IsApproved  FROM  InvT_IssueOrderHF WHERE(CompNo = {0}) AND(PrepYear = {1}) AND(PrepNo = {2})";
            List<InvStoresMFView> ModelInvStoresMF = db.Database.SqlQuery<InvStoresMFView>(queryString, CompNo, OrderYear, OrderNo).ToList();
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.FormCode = FormCode;

            ViewBag.lInvStoresMF = ModelInvStoresMF;

            return PartialView("~/Views/ProductionOrdersInProgress/tabs/LoadViewAndRFITab.cshtml", listData);
        }
        [HttpGet]
        public JsonResult LoadStage(short CompNo, int OrderYear, int OrderNo, short StageSer, string stage_code)
        {
            ProdCost_GetOrderStagesStatus_webView ListData = db.Database.SqlQuery<ProdCost_GetOrderStagesStatus_webView>("exec ProdCost_GetOrderStagesStatusOC_web {0},{1},{2},{3},{4}", CompNo, OrderYear, OrderNo, StageSer, stage_code).FirstOrDefault();
            return Json(new { ok = "OK" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CloseStage(short CompNo, int OrderYear, int OrderNo, short StageSer, string stage_code)
        {
            ViewBag.OrderNo = OrderNo;
            ViewBag.OrderYear = OrderYear;
            ProdCost_GetOrderStagesStatus_webView ListData = db.Database.SqlQuery<ProdCost_GetOrderStagesStatus_webView>("exec ProdCost_GetOrderStagesStatusOC_web {0},{1},{2},{3},{4}", CompNo, OrderYear, OrderNo, StageSer, stage_code).FirstOrDefault();

            return PartialView(ListData);
        }
        public JsonResult SendDataCloseStage(short CompNo, int OrderYear, int OrderNo, short StageSer, string stage_code, DateTime StageDate, int ReqOp, string UserID)
        {

            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_ManageStageStatus";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5)).Value = stage_code;
            Cmd.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 2)).Value = StageSer;
            Cmd.Parameters.Add(new SqlParameter("@StageDate", SqlDbType.SmallDateTime, 8)).Value = StageDate;
            Cmd.Parameters.Add(new SqlParameter("@ReqOp", SqlDbType.TinyInt, 1)).Value = ReqOp;
            Cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = UserID;
            Cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt, 4)).Direction = ParameterDirection.Output;
            Cmd.ExecuteNonQuery();
            int switchExpr = Convert.ToInt32(Cmd.Parameters["@ErrNo"].Value);
            cn.Close();
            switch (switchExpr)
            {
                case 10:
                    {
                        return Json(new { ok = Translate("المرحلة غير موجودة أو بدأت بالفعل", "The stage does not exist or already started") }, JsonRequestBehavior.AllowGet);
                    }

                case 20:
                    {
                        return Json(new { ok = Translate("المرحلة السابقة ليست مغلقة", "The previous stage is not closed") }, JsonRequestBehavior.AllowGet);
                    }

                case 30:
                    {
                        return Json(new { ok = Translate("المرحلة السابقة لم تبدأ", "The previous stage is not Started") }, JsonRequestBehavior.AllowGet);
                    }

                case 100:
                    {
                        return Json(new { ok = Translate("المرحلة مغلقة", "Stage closed") }, JsonRequestBehavior.AllowGet);

                     }

                case 110:
                    {
                        return Json(new { ok = Translate("ضبط الجودة فشل او لم ينته", "QC Report Faild Or Not Finished") }, JsonRequestBehavior.AllowGet);

                     }
                case 115:
                    {
                        return Json(new { ok = Translate("تقرير ضمان الجودة فشل أو لم ينته", "QA Report Faild Or Not Finished") }, JsonRequestBehavior.AllowGet);


                    }
                case 120:
                    {
                        return Json(new { ok = Translate("تاريخ و وقت الإغلاق أقل من تاريخ ووقت الافتتاح", "Closing Date & Time is Less Than Opening Stage Date and Time") }, JsonRequestBehavior.AllowGet);

                    }

                case 0:
                    {

                         return Json(new { ok = Translate("المرحلة مغلقة", "Stage closed") }, JsonRequestBehavior.AllowGet);

                    }
                default:
                    return Json(new { ok = "Unknown error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult StartStage(short CompNo, int OrderYear, int OrderNo, short StageSer, string stage_code)
        {
            ViewBag.OrderNo = OrderNo;
            ViewBag.OrderYear = OrderYear;

            ProdCost_GetOrderStagesStatus_webView ListData = db.Database.SqlQuery<ProdCost_GetOrderStagesStatus_webView>("exec ProdCost_GetOrderStagesStatusOC_web {0},{1},{2},{3},{4}", CompNo, OrderYear, OrderNo, StageSer, stage_code).FirstOrDefault();
            return PartialView(ListData);
        }
        public class ProdCost_GetByOrderYear_no
        {
            public string ItemDesc { get; set; }
        }
        public JsonResult SendDataOpenStage(short CompNo, int OrderYear, int OrderNo, short StageSer, string stage_code, DateTime StageDate, int ReqOp, string UserID)
        {

            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_ManageStageStatus";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5)).Value = stage_code;
            Cmd.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 2)).Value = StageSer;
            Cmd.Parameters.Add(new SqlParameter("@StageDate", SqlDbType.SmallDateTime, 8)).Value = StageDate;
            Cmd.Parameters.Add(new SqlParameter("@ReqOp", SqlDbType.TinyInt, 1)).Value = ReqOp;
            Cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = UserID;
            Cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt, 4)).Direction = ParameterDirection.Output;
            Cmd.ExecuteNonQuery();
            int switchExpr = Convert.ToInt32(Cmd.Parameters["@ErrNo"].Value);
            cn.Close();
            switch (switchExpr)
            {
                case 10:
                    {
                        return Json(new { ok = Translate("المرحلة غير موجودة أو بدأت بالفعل", "The stage does not exist or already started") }, JsonRequestBehavior.AllowGet);

                     }

                case 20:
                    {
                        return Json(new { ok = Translate("المرحلة السابقة ليست مغلقة", "The previous stage is not closed") }, JsonRequestBehavior.AllowGet);

                     }

                case 30:
                    {
                        return Json(new { ok = Translate("المرحلة السابقة لم تبدأ", "The previous stage is not Started") }, JsonRequestBehavior.AllowGet);
                    }

                case 100:
                    {
                        return Json(new { ok = Translate("المرحلة مفتوحة", "Stage Opened") }, JsonRequestBehavior.AllowGet);

                     }

                case 110:
                    {
                         return Json(new { ok = Translate("ضبط الجودة فشل او لم ينته", "QC Report Faild Or Not Finished") }, JsonRequestBehavior.AllowGet);

                    }
                case 115:
                    {
                         return Json(new { ok = Translate("تقرير ضمان الجودة فشل أو لم ينته", "QA Report Faild Or Not Finished") }, JsonRequestBehavior.AllowGet);

                    }
                case 120:
                    {
                        return Json(new { ok = Translate("تاريخ و وقت الإغلاق أقل من تاريخ ووقت الافتتاح", "Closing Date & Time is Less Than Opening Stage Date and Time") }, JsonRequestBehavior.AllowGet);

                     }

                case 0:
                    {
                        return Json(new { ok = Translate("المرحلة مفتوحة", "Stage Opened") }, JsonRequestBehavior.AllowGet);

                     }
                default:
                    return Json(new { ok = "Unknown error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewQAEventData(short CompNo, int OrderYear, int OrderNo, int StageSer, string stage_code)
        {
            try
            {
                short Option = 1;
                List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("exec Prodcost_QCQAResultView {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, StageSer, stage_code, Option).ToList();
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });
            }

        }
        public ActionResult ViewQCResult(short CompNo, int OrderYear, int OrderNo, int StageSer, string stage_code)
        {
            try
            {
                short Option = 2;

                List<LoadTestDetQcDfView> ListTestDF = db.Database.SqlQuery<LoadTestDetQcDfView>("exec Prodcost_QCQAResultView  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, StageSer, stage_code, Option).ToList();
                ProdCost_QCInspHF getTestDetQcHF = db.ProdCost_QCInspHF.Where(x => x.CompNo == company.comp_num && x.OrderNo == OrderNo && x.OrderYear == OrderYear &&x.StageSer == StageSer &&x.StageCode.ToString() ==stage_code).OrderByDescending(x=>x.RefNo).FirstOrDefault();
                ViewBag.getTestDetQcHF = getTestDetQcHF;
                return PartialView(ListTestDF);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });
            }

        }
        public JsonResult UpdateStage(int OpID, string FormCode, short year, int OrderNo, string StageCode, short StageSer, DateTime date, string hrs, string setuphrs, string stophrs)
        {

            decimal TotHrs = 0;
            decimal TotSetupHrs = 0;
            decimal TotStopHrs = 0;
            if (OpID == 2)
            {
                TotHrs = (decimal)(Convert.ToDouble(hrs.Substring(0, hrs.IndexOf(":"))) + Convert.ToDouble(hrs.Substring(hrs.IndexOf(":") + 1, Strings.Len(hrs) - hrs.IndexOf(":") - 1)) / 60);
                TotSetupHrs = (decimal)(Convert.ToDouble(setuphrs.Substring(0, setuphrs.IndexOf(":"))) + Convert.ToDouble(setuphrs.Substring(setuphrs.IndexOf(":") + 1, Strings.Len(setuphrs) - setuphrs.IndexOf(":") - 1)) / 60);
                TotStopHrs = (decimal)(Convert.ToDouble(stophrs.Substring(0, stophrs.IndexOf(":"))) + Convert.ToDouble(stophrs.Substring(stophrs.IndexOf(":") + 1, Strings.Len(stophrs) - stophrs.IndexOf(":") - 1)) / 60);
            }

            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_ManageStageStatus";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = year;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5)).Value = StageCode;
            Cmd.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 2)).Value = StageSer;
            Cmd.Parameters.Add(new SqlParameter("@StageDate", SqlDbType.SmallDateTime, 8)).Value = date;
            Cmd.Parameters.Add(new SqlParameter("@ReqOp", SqlDbType.TinyInt, 1)).Value = OpID;
            Cmd.Parameters.Add(new SqlParameter("@TotalWorkHours", SqlDbType.Money, 4)).Value = TotHrs;
            Cmd.Parameters.Add(new SqlParameter("@TotalSetupTime", SqlDbType.Money, 4)).Value = TotSetupHrs;
            Cmd.Parameters.Add(new SqlParameter("@TotalBreackTime", SqlDbType.Money, 4)).Value = TotStopHrs;
            Cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt, 4)).Direction = ParameterDirection.Output;
            Cmd.ExecuteNonQuery();
            int switchExpr = Convert.ToInt32(Cmd.Parameters["@ErrNo"].Value);
            cn.Close();
            switch (switchExpr)
            {
                case 10:
                    {
                        return Json(new { ok = "The stage does not exist or already started" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 20:
                    {
                        return Json(new { ok = "The previous stage is not closed" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 30:
                    {
                        return Json(new { ok = "The previous stage is not Started" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 100:
                    {
                        return Json(new { ok = "Stage closed" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 110:
                    {
                        return Json(new { ok = "QC Report Faild Or Not Finished" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 120:
                    {
                        return Json(new { ok = "Closing Date & Time is Less Than Opening Stage Date and Time" }, JsonRequestBehavior.AllowGet);
                        break;
                    }

                case 0:
                    {
                        CreateQCRequest(FormCode, year, OrderNo, StageCode, StageSer);
                        return Json(new { ok = "Stage Started" }, JsonRequestBehavior.AllowGet);
                        break;
                    }
            }

            return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListQCResult()
        {
            return PartialView();
        }
        public ActionResult NewWorkHrs()
        {
            return PartialView();
        }

        public void CreateQCRequest(string FormCode, short year, int OrderNo, string StageCode, short StageSer)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_AddOrderQCFile";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            Cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = FormCode;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = year;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.Int, 8)).Value = StageCode;
            Cmd.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 2)).Value = StageSer;
            Cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
            Cmd.ExecuteNonQuery();
        }
        public ActionResult EditLoadWorkHrs(short CompNo, short OrderYear, int OrderNo, short StageSer, string stage_code, DateTime begin_date)
        {
             ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.stage_code = stage_code;
            ViewBag.StageSer = StageSer;
            var Date = DateTime.Now;
            string formattedDate = Date.ToString("yyyy-MM-dd");
            List<ProdCost_GetEmpWorkHoursH_WebView> ProdCost_GetEmpWorkHoursH = db.Database.SqlQuery<ProdCost_GetEmpWorkHoursH_WebView>("exec ProdCost_GetEmpWorkHoursH_Web  {0},{1},{2},{3},{4}", CompNo, OrderYear, OrderNo, stage_code, formattedDate).ToList();
            List<ProdCost_EmpWorkHoursD_WebView> ListTestDF = new List<ProdCost_EmpWorkHoursD_WebView>();
            if (Language == "ar-JO")
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 1, formattedDate, stage_code).ToList();

            }
            else
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 2, formattedDate, stage_code).ToList();
            }

            ViewBag.ListTestDF = ListTestDF;
             return PartialView(ProdCost_GetEmpWorkHoursH);
        }
        public ActionResult DLaborEdit(short CompNo, short OrderYear, int OrderNo, short StageSer, string stage_code, string begin_date)
        {
            var Date = DateTime.Now;
            string formattedDate = Date.ToString("yyyy-MM-dd");
            List<ProdCost_GetEmpWorkHoursH_WebView> ProdCost_GetEmpWorkHoursH = db.Database.SqlQuery<ProdCost_GetEmpWorkHoursH_WebView>("exec ProdCost_GetEmpWorkHoursH_Web  {0},{1},{2},{3},{4}", CompNo, OrderYear, OrderNo, stage_code, formattedDate).ToList();
            List<ProdCost_EmpWorkHoursD_WebView> ListTestDF = new List<ProdCost_EmpWorkHoursD_WebView>();
            if (Language == "ar-JO")
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 1, formattedDate, stage_code).ToList();

            }
            else
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 2, formattedDate, stage_code).ToList();
            }
            ViewBag.ListTestDF = ListTestDF;
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            ViewBag.stage_code = stage_code;
            ViewBag.StageSer = StageSer;
            ViewBag.begin_date = begin_date;

            return PartialView("~/Views/ProductionOrdersInProgress/DLaborEditModal.cshtml", ProdCost_GetEmpWorkHoursH);

        }
        public ActionResult GetEmp(short CompNo)
        {
            List<EmpStage> ListEmp = db.Database.SqlQuery<EmpStage>("SELECT Emp_num, EmpName, EmpEngName,Pay_GetItemsLevels_1.Wplace AS WorkPlaceAr, Pay_GetItemsLevels_1.WplaceEng AS WorkPlaceEn,dbo.Pay_Job.job_Desc AS JobDescrAr, dbo.Pay_Job.EngDesc AS JobDescrEn FROM PayEmp INNER JOIN  dbo.Pay_Job ON dbo.PayEmp.Comp_num = dbo.Pay_Job.Comp_num AND dbo.PayEmp.Job_code = dbo.Pay_Job.job_code INNER JOIN dbo.Pay_GetItemsLevels(1) AS Pay_GetItemsLevels_1 ON dbo.PayEmp.Work_place = Pay_GetItemsLevels_1.MainId AND dbo.PayEmp.Comp_num = {0}", CompNo).ToList();
            return PartialView(ListEmp);
        }

        public ActionResult LoadTableEmp(short CompNo, short OrderYear, int OrderNo, short StageSer, string stage_code, string begin_date, string selectedDate)
        {
            if (string.IsNullOrEmpty(selectedDate))
            {
                selectedDate = DateTime.Now.ToString();
            }
            List<ProdCost_EmpWorkHoursD_WebView> ListTestDF = new List<ProdCost_EmpWorkHoursD_WebView>();
            if (Language == "ar-JO")
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 1, selectedDate, stage_code).ToList();

            }
            else
            {
                ListTestDF = db.Database.SqlQuery<ProdCost_EmpWorkHoursD_WebView>("exec ProdCost_GetEmpWorkHoursD_Web  {0},{1},{2},{3},{4},{5}", CompNo, OrderYear, OrderNo, 2, selectedDate, stage_code).ToList();
            }
            return PartialView(ListTestDF);

        }

        public ActionResult Save_WorkHrs(List<ProdCost_EmpWorkHoursD_Web> EmpWorkHoursD_Web, short PrepYear, int PrepNo, short StageSer, string StageCode, DateTime TrDate, string Notes)
        {
           
            try
            {
                SqlConnection cn = Cn();
                cn.Open();
                var Cmd1 = new SqlCommand();
                DataSet TmpDs = new DataSet();
                DataSet TmpDD = new DataSet();

                DataTable TmpDt = new DataTable();
                TmpDt.Columns.Add("CompNo", typeof(short));
                TmpDt.Columns.Add("PrepYear", typeof(short));
                TmpDt.Columns.Add("PrepNo", typeof(int));
                TmpDt.Columns.Add("TrDate", typeof(DateTime));
                TmpDt.Columns.Add("StageCode", typeof(int));
                TmpDt.Columns.Add("StageSer", typeof(short));
                TmpDt.Columns.Add("Notes", typeof(string));

                // Create DataTable for ProdCost_AddEmpWorkHoursD_Web
                DataTable TmpDtD = new DataTable();
                TmpDtD.Columns.Add("CompNo", typeof(short));
                TmpDtD.Columns.Add("PrepYear", typeof(short));
                TmpDtD.Columns.Add("PrepNo", typeof(int));
                TmpDtD.Columns.Add("TrDate", typeof(DateTime));
                TmpDtD.Columns.Add("ShiftNo", typeof(short));
                TmpDtD.Columns.Add("EmpNo", typeof(int));
                TmpDtD.Columns.Add("StageCode", typeof(string));
                TmpDtD.Columns.Add("ActualWorkHours", typeof(string));

                DataRow rowH = TmpDt.NewRow();
                rowH["CompNo"] = company.comp_num;
                rowH["PrepYear"] = PrepYear;
                rowH["PrepNo"] = PrepNo;
                rowH["TrDate"] = TrDate;
                rowH["StageCode"] = StageCode;
                rowH["StageSer"] = StageSer;
                rowH["Notes"] = Notes;
                TmpDt.Rows.Add(rowH);

                foreach (var detail in EmpWorkHoursD_Web)
                {
                    DataRow rowD = TmpDtD.NewRow();
                    rowD["CompNo"] = detail.CompNo;
                    rowD["PrepYear"] = detail.PrepYear;
                    rowD["PrepNo"] = detail.PrepNo;
                    rowD["TrDate"] = detail.TrDate;
                    rowD["ShiftNo"] = detail.ShiftNo;
                    rowD["EmpNo"] = detail.EmpNo;
                    rowD["StageCode"] = detail.StageCode;
                    rowD["ActualWorkHours"] = detail.ActualWorkHours;
                    TmpDtD.Rows.Add(rowD);
                }

                // Check if records with TrDate exist in both tables
                SqlCommand selectCmdH = new SqlCommand("SELECT COUNT(*) FROM ProdCost_EmpWorkHoursH_Web WHERE CompNo = @CompNo AND PrepYear = @PrepYear AND PrepNo = @PrepNo AND TrDate = @TrDate AND StageCode = @StageCode", cn);
                selectCmdH.Parameters.AddWithValue("@CompNo", company.comp_num);
                selectCmdH.Parameters.AddWithValue("@PrepYear", PrepYear);
                selectCmdH.Parameters.AddWithValue("@PrepNo", PrepNo);
                selectCmdH.Parameters.AddWithValue("@TrDate", TrDate);
                selectCmdH.Parameters.AddWithValue("@StageCode", StageCode);

                SqlCommand selectCmdD = new SqlCommand("SELECT COUNT(*) FROM ProdCost_EmpWorkHoursD_Web WHERE CompNo = @CompNo AND PrepYear = @PrepYear AND PrepNo = @PrepNo AND TrDate = @TrDate AND  StageCode = @StageCode", cn);
                selectCmdD.Parameters.AddWithValue("@CompNo", company.comp_num);
                selectCmdD.Parameters.AddWithValue("@PrepYear", PrepYear);
                selectCmdD.Parameters.AddWithValue("@PrepNo", PrepNo);
                selectCmdD.Parameters.AddWithValue("@TrDate", TrDate);
                selectCmdD.Parameters.AddWithValue("@StageCode", StageCode);

                int countH = (int)selectCmdH.ExecuteScalar();
                int countD = (int)selectCmdD.ExecuteScalar();

                // Delete records if they exist
                if (countD > 0)
                {
                    SqlCommand deleteCmdD = new SqlCommand("DELETE FROM ProdCost_EmpWorkHoursD_Web WHERE CompNo = @CompNo AND PrepYear = @PrepYear AND PrepNo = @PrepNo AND TrDate = @TrDate AND  StageCode = @StageCode", cn);
                    deleteCmdD.Parameters.AddWithValue("@CompNo", company.comp_num);
                    deleteCmdD.Parameters.AddWithValue("@PrepYear", PrepYear);
                    deleteCmdD.Parameters.AddWithValue("@PrepNo", PrepNo);
                    deleteCmdD.Parameters.AddWithValue("@TrDate", TrDate);
                    deleteCmdD.Parameters.AddWithValue("@StageCode", StageCode);
                    deleteCmdD.ExecuteNonQuery();
                }
                if (countH > 0)
                {
                    SqlCommand deleteCmdH = new SqlCommand("DELETE FROM ProdCost_EmpWorkHoursH_Web WHERE CompNo = @CompNo AND PrepYear = @PrepYear AND PrepNo = @PrepNo AND TrDate = @TrDate AND StageCode = @StageCode", cn);
                    deleteCmdH.Parameters.AddWithValue("@CompNo", company.comp_num);
                    deleteCmdH.Parameters.AddWithValue("@PrepYear", PrepYear);
                    deleteCmdH.Parameters.AddWithValue("@PrepNo", PrepNo);
                    deleteCmdH.Parameters.AddWithValue("@TrDate", TrDate);
                    deleteCmdH.Parameters.AddWithValue("@StageCode", StageCode);
                    deleteCmdH.ExecuteNonQuery();
                }



                // Insert new records
                using (var Da = new SqlDataAdapter())
                {
                    using (var Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.CommandText = "ProdCost_AddEmpWorkHoursH_Web";
                        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        Cmd.Parameters.Add("@PrepYear", SqlDbType.SmallInt).Value = PrepYear;
                        Cmd.Parameters.Add("@PrepNo", SqlDbType.Int).Value = PrepNo;
                        Cmd.Parameters.Add("@TrDate", SqlDbType.SmallDateTime).Value = TrDate;
                        Cmd.Parameters.Add("@StageCode", SqlDbType.Int).Value = StageCode;
                        Cmd.Parameters.Add("@StageSer", SqlDbType.SmallInt).Value = StageSer;
                        Cmd.Parameters.Add("@Notes", SqlDbType.VarChar, 100).Value = Notes;
                        Da.InsertCommand = Cmd;
                    }

                    Da.Update(TmpDt);
                }

                using (var SqlDP = new SqlDataAdapter())
                {
                    using (var Cmd = new SqlCommand())
                    {
                        Cmd.Connection = cn;
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.CommandText = "ProdCost_AddEmpWorkHoursD_Web";
                        Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                        Cmd.Parameters.Add("@PrepYear", SqlDbType.SmallInt).Value = PrepYear;
                        Cmd.Parameters.Add("@PrepNo", SqlDbType.Int).Value = PrepNo;
                        Cmd.Parameters.Add("@TrDate", SqlDbType.SmallDateTime).Value = TrDate;
                        Cmd.Parameters.Add("@ShiftNo", SqlDbType.SmallInt, 4, "ShiftNo");
                        Cmd.Parameters.Add("@EmpNo", SqlDbType.Int, 10, "EmpNo");
                        Cmd.Parameters.Add("@StageCode", SqlDbType.Int, 10, "StageCode");
                        Cmd.Parameters.Add("@ActualWorkHours", SqlDbType.VarChar, 100, "ActualWorkHours");
                        SqlDP.InsertCommand = Cmd;
                    }
                    SqlDP.Update(TmpDtD);
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ok = 4 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadViewRawMateria(short CompNo, short OrderYear, int OrderNo, short ReqOption, int? OrdNo)
        {
            List<ViewRawMateria> listData;
            if (ReqOption == 2 && OrdNo.HasValue)
            {
                string UserID = me.UserID;
                short GLang = (short)(Language == "ar-JO" ? 1 : 2);
                listData = db.Database.SqlQuery<ViewRawMateria>("exec Invt_LstIssueOrders @CompNo = {0}, @UserID = {1}, @GLang = {2}, @RptType = 2, @OrderYear = {3}, @OrderNo = {4}, @ProdPrepNo = {5}", CompNo, UserID, GLang, OrderYear, OrdNo, OrderNo).ToList();
            }
            else
            {
                listData = db.Database.SqlQuery<ViewRawMateria>("exec ProdCost_OrderCostDetails_CostDriver @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}, @ReqOption = {3}", CompNo, OrderYear, OrderNo, ReqOption).ToList();
            }

            return PartialView(listData);
        }
        
        public ActionResult LoadRFIAll(short CompNo, short OrderYear, int OrderNo, string FormCode)
        {
            string queryString = "SELECT  CompNo, PrepYear, PrepNo, OrdYear, OrdNo, StoreNo, OrdDate, DocNo, BusUnitID, ByStage,AddititiveItems,IsApproved  FROM  InvT_IssueOrderHF WHERE(CompNo = {0}) AND(PrepYear = {1}) AND(PrepNo = {2})";
            List<InvStoresMFView> ListData = db.Database.SqlQuery<InvStoresMFView>(queryString, CompNo, OrderYear, OrderNo).ToList();
            ViewBag.FormCode = FormCode;
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            return PartialView(ListData);
        }
        //public ActionResult LoadModalRFI(string FormCode, short CompNo, int OrderYear, int OrderNo, int StoreNo, bool PackingItems, bool AdditiveItems, bool IsRowMaterial,short OrdYear,int OrdNo)
        public ActionResult LoadModalRFI(string FormCode, short CompNo, int OrderYear, int OrderNo,short OrdYear,int OrdNo,bool? IsApproved)
        {
            var DsLoadParm = new DataSet();

            string queryString = "SELECT CompNo, PrepYear, PrepNo, OrdYear, OrdNo, StoreNo, OrdDate, DocNo, BusUnitID, ByStage,AddititiveItems  FROM  InvT_IssueOrderHF WHERE(CompNo = {0}) AND(PrepYear = {1}) AND(PrepNo = {2}) AND(OrdNo={3})";
            InvStoresMFView ListData = db.Database.SqlQuery<InvStoresMFView>(queryString, CompNo, OrderYear, OrderNo, OrdNo).FirstOrDefault();
            if (ListData!=null && ListData.OrdNo != 0)
            {
                User me = (User)Session["me"];

                ViewBag.FormCode = FormCode;
                ProdCost_GetProdOrderInfoView GetProdOrderInfo = db.Database.SqlQuery<ProdCost_GetProdOrderInfoView>(string.Format("SELECT  TOP 1 StoreNo FROM  dbo.InvStoreUsers WHERE (CompNo = {0}) AND (AllPerm = 1) AND (UserID = '{1}')", CompNo, me.UserID)).FirstOrDefault();
                if (GetProdOrderInfo == null)
                {
                    return Content("<div class='alert alert-danger'>" + Translate("المستخدم غير معرف على مستودع صلاحيات كاملة", "User is not defined on a warehouse with all permissions") + "</div>");
                }
                //LoadIssueOrder
                if (ListData.AddititiveItems == true && ListData.ByStage == false)
                {
                    List<BOM_GetFormStagesView> LoadStage = db.Database.SqlQuery<BOM_GetFormStagesView>("exec BOM_GetFormStages {0},{1}", CompNo, FormCode).ToList();

                    SqlConnection cn = new SqlConnection(ConnectionString());
                    cn.Open();
                    var DsLoadIssueOrder = new DataSet();
                    var DaLoadIssueOrder = new SqlDataAdapter();
                    var DaCmd = new SqlDataAdapter();
                    var Cmd = new SqlCommand();
                    Cmd.Connection = cn;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = "InvT_LoadIssueOrderDF";
                    Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                    Cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = ListData.OrdYear;
                    Cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int)).Value = ListData.OrdNo;
                    Cmd.Parameters.Add(new SqlParameter("@Lang", SqlDbType.TinyInt)).Value = 1;
                    DaLoadIssueOrder.SelectCommand = Cmd;
                    DsLoadIssueOrder.Tables.Clear();
                    DsLoadIssueOrder.Dispose();
                    DaLoadIssueOrder.Fill(DsLoadIssueOrder, "LoadIssueOrderOrDefult");
                    DsLoadParm = DsLoadIssueOrder;
                    ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                    ViewBag.OrderYear = OrderYear;
                    ViewBag.OrderNo = OrderNo;
                    ViewBag.ListData = ListData;
                    ViewBag.LoadStage = LoadStage;
                    ViewBag.IsApproved = IsApproved;


                }
                //Stage
                else if (ListData.ByStage == true && ListData.AddititiveItems == false)
                {
                    List<BOM_GetFormStagesView> LoadStage = db.Database.SqlQuery<BOM_GetFormStagesView>("exec BOM_GetFormStages {0},{1}", CompNo, FormCode).ToList();
                    SqlConnection cn = new SqlConnection(ConnectionString());
                    cn.Open();
                    var DsLoadIssueOrderStage = new DataSet();
                    var DaLoadIssueOrderStage = new SqlDataAdapter();
                    var DaCmd = new SqlDataAdapter();
                    var Cmd = new SqlCommand();
                    Cmd.Connection = cn;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = "InvT_LoadIssueOrderDF";
                    Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                    Cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = ListData.OrdYear;
                    Cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int)).Value = ListData.OrdNo;
                    Cmd.Parameters.Add(new SqlParameter("@Lang", SqlDbType.TinyInt)).Value = 1;
                    DaLoadIssueOrderStage.SelectCommand = Cmd;
                    DsLoadIssueOrderStage.Tables.Clear();
                    DsLoadIssueOrderStage.Dispose();
                    DaLoadIssueOrderStage.Fill(DsLoadIssueOrderStage, "LoadIssueOrderOrDefult");
                    DsLoadParm = DsLoadIssueOrderStage;
                    ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                    ViewBag.OrderYear = OrderYear;
                    ViewBag.OrderNo = OrderNo;
                    ViewBag.ListData = ListData;
                    ViewBag.LoadStage = LoadStage;
                    ViewBag.IsApproved = IsApproved;

                }
                //defult
                else
                {
                    List<BOM_GetFormStagesView> LoadStage = db.Database.SqlQuery<BOM_GetFormStagesView>("exec BOM_GetFormStages {0},{1}", CompNo, FormCode).ToList();

                    SqlConnection cn = new SqlConnection(ConnectionString());
                    cn.Open();
                    var DsLoadIssueOrderdefult = new DataSet();
                    var DaLoadIssueOrderdefult = new SqlDataAdapter();
                    var DaCmd = new SqlDataAdapter();
                    var Cmd = new SqlCommand();
                    Cmd.Connection = cn;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.CommandText = "InvT_LoadIssueOrderDF";
                    Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                    Cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = ListData.OrdYear;
                    Cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int)).Value = ListData.OrdNo;
                    Cmd.Parameters.Add(new SqlParameter("@Lang", SqlDbType.TinyInt)).Value = 1;

                    DaLoadIssueOrderdefult.SelectCommand = Cmd;
                    DsLoadIssueOrderdefult.Tables.Clear();
                    DsLoadIssueOrderdefult.Dispose();
                    DaLoadIssueOrderdefult.Fill(DsLoadIssueOrderdefult, "LoadIssueOrderOrDefult");
                    DsLoadParm = DsLoadIssueOrderdefult;
                    ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                    ViewBag.OrderYear = OrderYear;
                    ViewBag.OrderNo = OrderNo;
                    ViewBag.ListData = ListData;
                    ViewBag.LoadStage = LoadStage;
                    ViewBag.IsApproved = IsApproved;


                }

                return PartialView("~/Views/ProductionOrdersInProgress/EditLoadModalRFI.cshtml", DsLoadParm);


            }
            else
            {
                User me = (User)Session["me"];

                ViewBag.FormCode = FormCode;
                ProdCost_GetProdOrderInfoView GetProdOrderInfo = db.Database.SqlQuery<ProdCost_GetProdOrderInfoView>(string.Format("SELECT  TOP 1 StoreNo FROM  dbo.InvStoreUsers WHERE (CompNo = {0}) AND (AllPerm = 1) AND (UserID = '{1}')", CompNo, me.UserID)).FirstOrDefault();
                if (GetProdOrderInfo == null)
                {
                    return Content("<div class='alert alert-danger'>" + Translate("المستخدم غير معرف على مستودع صلاحيات كاملة", "User is not defined on a warehouse with all permissions") + "</div>");
                }
                SqlConnection cn = new SqlConnection(ConnectionString());
                cn.Open();
                var DsGetPrepDetlForIssueOrder = new DataSet();
                var DaGetPrepDetlForIssueOrder = new SqlDataAdapter();
                var DaCmd = new SqlDataAdapter();
                var Cmd = new SqlCommand();
                Cmd.Connection = cn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "InvT_GetPrepDetlForIssueOrder_Web";
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
                Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = GetProdOrderInfo.StoreNo;
                Cmd.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = false;
                Cmd.Parameters.Add(new SqlParameter("@AdditiveItems", SqlDbType.Bit)).Value = false;
                Cmd.Parameters.Add(new SqlParameter("@IsRowMaterial", SqlDbType.Bit)).Value = false;

                DaGetPrepDetlForIssueOrder.SelectCommand = Cmd;
                DsGetPrepDetlForIssueOrder.Tables.Clear();
                DsGetPrepDetlForIssueOrder.Dispose();
                DaGetPrepDetlForIssueOrder.Fill(DsGetPrepDetlForIssueOrder, "GetPrepDetlForIssueOrder");
                ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                ViewBag.OrderYear = OrderYear;
                ViewBag.OrderNo = OrderNo;
                //ViewBag.PackingItems = PackingItems;
                //ViewBag.AdditiveItems = AdditiveItems;
                //ViewBag.IsRowMaterial = IsRowMaterial;
                ViewBag.GetProdOrderInfo = GetProdOrderInfo;
 
                return PartialView("~/Views/ProductionOrdersInProgress/LoadModalRFI.cshtml", DsGetPrepDetlForIssueOrder);
            }
        }
        public ActionResult LoadTableRFI()
        {
            return PartialView();
        }
        public ActionResult LoadWarehouseData(short CompNo)
        {
            List<ViewWarehouseData> ListData = db.Database.SqlQuery<ViewWarehouseData>("SELECT StoreNo, StoreName, StoreNameEng FROM InvStoresMF WHERE(CompNo = {0}) ORDER BY StoreNo", CompNo).ToList();

            return PartialView(ListData);
        }

        public ActionResult LoadstageDropdown(short CompNo, string FormCode)
        {
            List<BOM_GetFormStagesView> ListData = db.Database.SqlQuery<BOM_GetFormStagesView>("exec BOM_GetFormStages {0},{1}", CompNo, FormCode).ToList();

            return PartialView(ListData);
        }
        public ActionResult LoadTableDefult(short CompNo, int OrderYear, int OrderNo, int StoreNo, bool PackingItems, bool AdditiveItems, bool IsRowMaterial)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsGetPrepDetlForIssueOrder = new DataSet();
            var DaGetPrepDetlForIssueOrder = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "InvT_GetPrepDetlForIssueOrder_Web";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = StoreNo;
            Cmd.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = PackingItems;
            Cmd.Parameters.Add(new SqlParameter("@AdditiveItems", SqlDbType.Bit)).Value = AdditiveItems;
            Cmd.Parameters.Add(new SqlParameter("@IsRowMaterial", SqlDbType.Bit)).Value = IsRowMaterial;

            DaGetPrepDetlForIssueOrder.SelectCommand = Cmd;
            DsGetPrepDetlForIssueOrder.Tables.Clear();
            DsGetPrepDetlForIssueOrder.Dispose();
            DaGetPrepDetlForIssueOrder.Fill(DsGetPrepDetlForIssueOrder, "GetPrepDetlForIssueOrder");
            return PartialView(DsGetPrepDetlForIssueOrder);
        }
        public ActionResult LoadTableAdditiveItems(short CompNo, int OrderYear, int OrderNo, int StoreNo, bool PackingItems, bool AdditiveItems, bool IsRowMaterial)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsGetPrepDetlForIssueOrder = new DataSet();
            var DaGetPrepDetlForIssueOrder = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "InvT_GetPrepDetlForIssueOrder_Web";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = StoreNo;
            Cmd.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = PackingItems;
            Cmd.Parameters.Add(new SqlParameter("@AdditiveItems", SqlDbType.Bit)).Value = AdditiveItems;

            DaGetPrepDetlForIssueOrder.SelectCommand = Cmd;
            DsGetPrepDetlForIssueOrder.Tables.Clear();
            DsGetPrepDetlForIssueOrder.Dispose();
            DaGetPrepDetlForIssueOrder.Fill(DsGetPrepDetlForIssueOrder, "LoadTableAdditiveItems");
            return PartialView(DsGetPrepDetlForIssueOrder);
        }
        public ActionResult LoadTableStage(short CompNo, int OrderYear, int OrderNo, int StoreNo, bool PackingItems, bool AdditiveItems, bool IsRowMaterial, int stageID)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsGetPrepDetlForIssueOrder = new DataSet();
            var DaGetPrepDetlForIssueOrder = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_GetOrderRMByStages";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = StoreNo;
            Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@stageID", SqlDbType.Int, 8)).Value = stageID;
            DaGetPrepDetlForIssueOrder.SelectCommand = Cmd;
            DsGetPrepDetlForIssueOrder.Tables.Clear();
            DsGetPrepDetlForIssueOrder.Dispose();
            DaGetPrepDetlForIssueOrder.Fill(DsGetPrepDetlForIssueOrder, "LoadTableStage");
            return PartialView(DsGetPrepDetlForIssueOrder);
        }
        public ActionResult DailyProdListView(short CompNo, short OrderYear,int OrderNo)
        {
            List<ProductionReceivingView> DailyProdHf = db.Database.SqlQuery<ProductionReceivingView>("exec ProdCost_OrderCostDetails_CostDriver @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}", CompNo, OrderYear, OrderNo).ToList();

            return PartialView(DailyProdHf);
         }


        public class DailyProductionH
        {
            public short CompNo { get; set; }
            public short ReportYear { get; set; }
            public int ReportNo { get; set; }
            public short ProdPrepYear { get; set; }
            public int ProdPrepNo { get; set; }
            public DateTime Prod_Date { get; set; }
            public bool? Closed { get; set; }
            public decimal SumProdQty { get; set; }
            public float? qty_prepare { get; set; }
            public int Prod_stage { get; set; }


        }
        public class ProdCost_GetDailyProduction_web
        {
            public short CompNo { get; set; }
            public short ReportYear { get; set; }
            public int ReportNo { get; set; }
            public DateTime Prod_Date { get; set; }
            public int ItemSer { get; set; }
            public short ProdPrepYear { get; set; }
            public int ProdPrepNo { get; set; }
            public string FinCode { get; set; }
            public decimal? Prod_Qty { get; set; }
            public string TUnit { get; set; }
            public short? UnitSerial { get; set; }


        } 
        public class GetLoadStage
        {
            public string stage_code { get; set; }
            public Int16 StageSer { get; set; }
            public string stage_desc { get; set; }
        }
        public ActionResult listAllDailyProduction(short CompNo, short OrderYear, short OrderNo)
        {
            List<DailyProductionH> DailyProductionH = db.Database.SqlQuery<DailyProductionH>(string.Format("SELECT ProdCost_DailyProductionH.CompNo, ProdCost_DailyProductionH.ReportYear, ProdCost_DailyProductionH.ReportNo, ProdCost_DailyProductionD.ProdPrepYear, ProdCost_DailyProductionD.ProdPrepNo,ProdCost_DailyProductionH.Prod_Date, ProdCost_DailyProductionH.Closed FROM ProdCost_DailyProductionH INNER JOIN ProdCost_DailyProductionD ON ProdCost_DailyProductionH.CompNo = ProdCost_DailyProductionD.CompNo AND ProdCost_DailyProductionH.ReportYear = ProdCost_DailyProductionD.ReportYear AND ProdCost_DailyProductionH.ReportNo = ProdCost_DailyProductionD.ReportNo GROUP BY ProdCost_DailyProductionH.CompNo, ProdCost_DailyProductionH.ReportYear, ProdCost_DailyProductionH.ReportNo, ProdCost_DailyProductionD.ProdPrepYear, ProdCost_DailyProductionD.ProdPrepNo,ProdCost_DailyProductionH.Prod_Date, ProdCost_DailyProductionH.Closed HAVING(ProdCost_DailyProductionH.CompNo = {0}) AND(ProdCost_DailyProductionD.ProdPrepYear = {1}) AND(ProdCost_DailyProductionD.ProdPrepNo = {2})", CompNo, OrderYear, OrderNo)).ToList();
            ViewBag.OrderYear = OrderYear;
            ViewBag.OrderNo = OrderNo;
            return PartialView(DailyProductionH);
        }
        public ActionResult ProductionReceiving(short CompNo, short OrderYear, int OrderNo,short? ReportYear, int? ReportNo,bool? IsClosed)
        {
           
            DailyProductionH DailyProductionH = db.Database.SqlQuery<DailyProductionH>("exec ProdCost_LoadDailyProduction_Web @CompNo = {0}, @PrepYear = {1}, @PrepNo = {2}", CompNo, OrderYear, OrderNo).FirstOrDefault();
            List<GetLoadStage> LoadStage = db.Database.SqlQuery<GetLoadStage>("exec ProdCost_GetOrderStagesStatus_web @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}", CompNo, OrderYear, OrderNo).ToList();

            if (DailyProductionH != null && ReportNo != null)
            {
                ProdCost_GetProdOrderInfoView GetProdOrderInfo = db.Database.SqlQuery<ProdCost_GetProdOrderInfoView>(string.Format("SELECT  TOP 1 StoreNo FROM  dbo.InvStoreUsers WHERE (CompNo = {0}) AND (AllPerm = 1) AND (UserID = '{1}')", CompNo, me.UserID)).FirstOrDefault();

                ViewBag.DailyProductionH = DailyProductionH;
                List<Prod_GetOrderNoteCloseView> ListData = db.Database.SqlQuery<Prod_GetOrderNoteCloseView>("exec Prod_GetOrderNoteClose_web {0},{1},{2}", CompNo, OrderYear, OrderNo).ToList();
                ViewBag.qty_prepare = ListData.Select(x => x.qty_prepare);
                ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                ViewBag.OrderYear = OrderYear;
                ViewBag.OrderNo = OrderNo;
                List<ProdCost_GetDailyProduction_web> DailyProdD = db.Database.SqlQuery<ProdCost_GetDailyProduction_web>("exec ProdCost_GetDailyProduction_web {0},{1},{2}", CompNo, ReportYear, ReportNo).ToList();
                ViewBag.DailyProdD = DailyProdD;
                ViewBag.Prod_Qty = DailyProductionH.SumProdQty;
                ViewBag.qty_prepare = DailyProductionH.qty_prepare;
                ViewBag.IsClosed = IsClosed;
                ViewBag.LoadStage = LoadStage;
                return PartialView("~/Views/ProductionOrdersInProgress/EditProductionReceiving.cshtml", ListData);
            }
            else
            {
                ProdCost_GetProdOrderInfoView GetProdOrderInfo = db.Database.SqlQuery<ProdCost_GetProdOrderInfoView>(string.Format("SELECT  TOP 1 StoreNo FROM  dbo.InvStoreUsers WHERE (CompNo = {0}) AND (AllPerm = 1) AND (UserID = '{1}')", CompNo, me.UserID)).FirstOrDefault();
                GetLoadStage getbyStage = db.Database.SqlQuery<GetLoadStage>("exec ProdCost_GetOrderStagesStatus_web @CompNo = {0}, @OrderYear = {1}, @OrderNo = {2}", CompNo, OrderYear, OrderNo).FirstOrDefault();

                List<Prod_GetOrderNoteCloseView> ListData = db.Database.SqlQuery<Prod_GetOrderNoteCloseView>("exec Prod_GetOrderNoteClose_web {0},{1},{2}", CompNo, OrderYear, OrderNo).ToList();
                List<ProdCost_DailyProductionD> DailyProdD = new MDB().ProdCost_DailyProductionD.Where(x => x.CompNo == CompNo
               && x.ReportYear == ReportYear && x.ReportNo == ReportNo).ToList();
                ViewBag.qty_prepare =ListData.Select(x => x.qty_prepare);
                ViewBag.StoreNo = GetProdOrderInfo.StoreNo;
                ViewBag.OrderYear = OrderYear;
                ViewBag.OrderNo = OrderNo;
                ViewBag.Prod_Qty = DailyProductionH?.SumProdQty;
                ViewBag.qty_prepare = DailyProductionH?.qty_prepare;
                ViewBag.LoadStage = LoadStage;
                ViewBag.getbyStage = getbyStage;
                return PartialView("~/Views/ProductionOrdersInProgress/ProductionReceiving.cshtml", ListData);

            }

        }


        public ActionResult LoadBatch(short CompNo,string ItemNo,int StoreNo,int UnitSerial)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsLoadBatch = new DataSet();
            var DaLoadBatch = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "InvT_LoadStoreBatchs";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = StoreNo;
            Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ItemNo;
            Cmd.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt)).Value = UnitSerial;
            Cmd.Parameters.Add(new SqlParameter("@VouType", SqlDbType.SmallInt)).Value = 11;
            DaLoadBatch.SelectCommand = Cmd;
            DsLoadBatch.Tables.Clear();
            DsLoadBatch.Dispose();
            DaLoadBatch.Fill(DsLoadBatch, "LoadBatch");
            return PartialView(DsLoadBatch);
        }
        public JsonResult qcreq(short CompNo, short OrderYear, int OrderNo, string StageCode, short StageSer, short ReOption)
        {
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_ReAddQCRequest";
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
            Cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            Cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            Cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.Int, 8)).Value = StageCode;
            Cmd.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 2)).Value = StageSer;
            Cmd.Parameters.Add(new SqlParameter("@ROption", SqlDbType.SmallInt, 2)).Value = ReOption;

            Cmd.ExecuteNonQuery();
            return Json(new { ok = Resources.Resource.Done }, JsonRequestBehavior.AllowGet);
        }
        public class GetProdOrderInfoPro
        {
            public double NoOfMixNet { get; set; }
            public int StoreNo { get; set; }
            public int RMDept { get; set; }
            public long RMAcc { get; set; }
        }
        public JsonResult SaveRFI(short CompNo, short OrderYear, int OrderNo, InvT_IssueOrderHFView OderHFData, List<InvT_IssueOrderDF> IssueOrderDF, string UserID)
        {

            GetProdOrderInfoPro GetProdOrderInfo = db.Database.SqlQuery<GetProdOrderInfoPro>("exec ProdCost_GetProdOrderInfo {0},{1},{2}", CompNo, OrderYear, OrderNo).First();

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlTransaction myTrans;
                int OrdNo;
                cn.Open();
                myTrans = cn.BeginTransaction();
                try
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = myTrans;

                        SqlCommand selectOrderHF = new SqlCommand("SELECT COUNT(*) FROM InvT_IssueOrderHF WHERE CompNo = @CompNo AND OrdYear = @OrdYear AND OrdNo = @OrdNo", cn);
                        selectOrderHF.Transaction = myTrans; // Set the transaction here
                        selectOrderHF.Parameters.AddWithValue("@CompNo", CompNo);
                        selectOrderHF.Parameters.AddWithValue("@OrdYear", OderHFData.OrdYear);
                        selectOrderHF.Parameters.AddWithValue("@OrdNo", OderHFData.OrdNo);

                        SqlCommand selectOrderDF = new SqlCommand("SELECT COUNT(*) FROM InvT_IssueOrderDF WHERE CompNo = @CompNo AND OrdYear = @OrdYear AND OrdNo = @OrdNo", cn);
                        selectOrderDF.Transaction = myTrans; // Set the transaction here
                        selectOrderDF.Parameters.AddWithValue("@CompNo", CompNo);
                        selectOrderDF.Parameters.AddWithValue("@OrdYear", OderHFData.OrdYear);
                        selectOrderDF.Parameters.AddWithValue("@OrdNo", OderHFData.OrdNo);

                        int countH = (int)selectOrderHF.ExecuteScalar();
                        int countD = (int)selectOrderDF.ExecuteScalar();

                        // Delete from InvT_DelIssueOrderDF
                        if (countD > 0)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "InvT_DelIssueOrderDF";
                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                            cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OderHFData.OrdYear;
                            cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OderHFData.OrdNo;
                            cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Transaction = myTrans;
                            cmd.ExecuteNonQuery();
                        }
                        if (countH > 0)
                        {

                            // Delete from InvT_DelIssueOrderHF
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "InvT_DelIssueOrderHF";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                            cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OderHFData.OrdYear;
                            cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OderHFData.OrdNo;
                            cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            cmd.Transaction = myTrans;
                            cmd.ExecuteNonQuery();
                        }
                    

                    }
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = myTrans;
                        // Add to InvT_AddIssueOrderHF
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InvT_AddIssueOrderHF";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                        cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OderHFData.OrdYear;
                        cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OderHFData.OrdNo;
                        cmd.Parameters.Add(new SqlParameter("@OrdDate", SqlDbType.SmallDateTime, 4)).Value = OderHFData.OrdDate;
                        cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 4)).Value = OderHFData.StoreNo;
                        cmd.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar, 15)).Value = OderHFData.DocNo;
                        cmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 300)).Value = "";
                        cmd.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = OderHFData.BusUnitID;
                        cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
                        cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
                        cmd.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.Int)).Value = OderHFData.StageCode;
                        cmd.Parameters.Add(new SqlParameter("@AddititiveItems", SqlDbType.Bit)).Value = OderHFData.AddititiveItems;
                        cmd.Parameters.Add(new SqlParameter("@IsProduction", SqlDbType.Bit)).Value = 1;
                        cmd.Parameters.Add(new SqlParameter("@ByStage", SqlDbType.Bit)).Value = OderHFData.ByStage;
                        cmd.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = GetProdOrderInfo.RMDept;
                        cmd.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = GetProdOrderInfo.RMAcc;
                        cmd.Parameters.Add(new SqlParameter("@vod_act", SqlDbType.VarChar, 10)).Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(new SqlParameter("@segment", SqlDbType.VarChar, 50)).Value = "0";
                        cmd.Parameters.Add(new SqlParameter("@Dist_RoleID", SqlDbType.VarChar, 50)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@NoOfMixOrd", SqlDbType.Float)).Value = GetProdOrderInfo.NoOfMixNet;
                        cmd.Parameters.Add(new SqlParameter("@AgrementYear", SqlDbType.SmallInt)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@AgrementNo", SqlDbType.Int)).Value = 0;
                        cmd.Transaction = myTrans;
                        cmd.ExecuteNonQuery();
                        int errNo = Convert.ToInt32(cmd.Parameters["@ErrNo"].Value);
                         OrdNo = Convert.ToInt32(cmd.Parameters["@UsedOrdNo"].Value);


                        ErrList(errNo);

                    }


                    DataSet dataSet1;
                        //DataRow datarow;
                        int i = 1;

                        DataTable TmpDtD = new DataTable();
                        TmpDtD.Columns.Add("ItemSer", typeof(short));
                        TmpDtD.Columns.Add("ItemNo", typeof(string));
                        TmpDtD.Columns.Add("Batch", typeof(string));
                        TmpDtD.Columns.Add("UnitCode", typeof(string));
                        TmpDtD.Columns.Add("Qty", typeof(double));
                        TmpDtD.Columns.Add("UnitSerial", typeof(short));
                        TmpDtD.Columns.Add("ItemNotes", typeof(string));

                        foreach (var row in IssueOrderDF)
                        {
                            if (row.Qty <= 0) continue; // Skip items with zero quantity

                            //if (row.RowState != DataRowState.Deleted)
                            //{
                            DataRow datarow = TmpDtD.NewRow();

                            datarow["ItemSer"] = i;
                            datarow["ItemNo"] = row.ItemNo;
                            datarow["Batch"] = "";
                            datarow["UnitCode"] = row.UnitCode;
                            datarow["Qty"] = row.Qty;
                            datarow["UnitSerial"] = row.UnitSerial;
                            datarow["ItemNotes"] = row.ItemNotes;
                            TmpDtD.Rows.Add(datarow);
                            i++;
                        }
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.Transaction = myTrans;
                            cmd.CommandText = "InvT_AddIssueOrderDF";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                            cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OderHFData.OrdYear;
                            cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrdNo;
                            cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).SourceColumn = "ItemNo";
                            cmd.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 20)).SourceColumn = "Batch";
                            cmd.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 2)).SourceColumn = "ItemSer";
                            cmd.Parameters.Add(new SqlParameter("@UnitCode", SqlDbType.VarChar, 5)).SourceColumn = "UnitCode";
                            cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 8)).SourceColumn = "Qty";
                            cmd.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 2)).SourceColumn = "UnitSerial";
                            cmd.Parameters.Add(new SqlParameter("@ItemNotes", SqlDbType.VarChar, 100)).SourceColumn = "ItemNotes";
                            cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                            int errNo1 = Convert.ToInt32(cmd.Parameters["@ErrNo"].Value);

                            ErrList(errNo1);
                            cmd.Transaction = myTrans;
                            adapter.InsertCommand = cmd;
                            adapter.Update(TmpDtD);
                    }
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = myTrans;
                        cmd.CommandText = "Alpha_AddWorkFlowLog";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                        cmd.Parameters.Add(new SqlParameter("@FID", SqlDbType.SmallInt)).Value = 6;
                        cmd.Parameters.Add(new SqlParameter("@BU", SqlDbType.Int, 4)).Value = OderHFData.BusUnitID;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = UserID;
                        cmd.Parameters.Add(new SqlParameter("@K1", SqlDbType.VarChar, 10)).Value = DateTime.Now.Year.ToString();
                        cmd.Parameters.Add(new SqlParameter("@K2", SqlDbType.VarChar, 10)).Value = 26;
                        cmd.Parameters.Add(new SqlParameter("@K3", SqlDbType.VarChar, 10)).Value = OrdNo;
                        cmd.Parameters.Add(new SqlParameter("@TrAmount", SqlDbType.Money)).Value = 0;
                        cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                        cmd.Parameters.Add(new SqlParameter("@TrFormDesc", SqlDbType.VarChar)).Value = "";
                        cmd.Parameters.Add(new SqlParameter("@FinalApprove", SqlDbType.Bit)).Direction = ParameterDirection.Output;
                        cmd.Transaction = myTrans;
                        cmd.ExecuteNonQuery();

                        myTrans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    return Json(new { error = ex.Message}, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    cn.Close();

                }
                return Json(new { ok = Resources.Resource.Done }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult DeleteRFI(short CompNo, short OrdYear, int OrdNo)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                cn.Open();
                SqlTransaction myTrans = cn.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = myTrans;
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Delete Details
                        cmd.CommandText = "InvT_DelIssueOrderDF";
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                        cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int)).Value = OrdNo;
                        cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        // Delete Header
                        cmd.CommandText = "InvT_DelIssueOrderHF";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt)).Value = OrdYear;
                        cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int)).Value = OrdNo;
                        cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();

                        myTrans.Commit();
                        return Json(new { ok = Resources.Resource.Done }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    return Json(new { ok = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        public JsonResult ErrList(int ErrNo)
        {
            switch (ErrNo)
            {
                case 110:
                    return Json(new { ok = "QC Report Failed Or Not Finished" }, JsonRequestBehavior.AllowGet);
                    break;
                case -10:
                    return Json(new { Ok = "You Must Define Voucher Serial To Continue With The Save Operation" }, JsonRequestBehavior.AllowGet);
                    break;
                case -20:
                    return Json(new { Ok = "The Voucher No Exists In Historical File" }, JsonRequestBehavior.AllowGet);
                    break;
                case -30:
                    return Json(new { Message = "The Account is Not Linked With the Department" }, JsonRequestBehavior.AllowGet);
                    break;
                case -40:
                    return Json(new { Message = "Customer No. Dosen't Exist, Operation Faild" }, JsonRequestBehavior.AllowGet);
                    break;
                case -50:
                    return Json(new { Message = "SalesMan Not Defined" }, JsonRequestBehavior.AllowGet);
                    break;
                case -60:
                    return Json(new { Message = "Document Type Not Defined" }, JsonRequestBehavior.AllowGet);
                    break;
                case -70:
                    return Json(new { Message = "The Voucher No Exists In Daily File" }, JsonRequestBehavior.AllowGet);
                    break;
                case -80:
                    return Json(new { Message = "Transaction date is less than the frozen period's date or system appliment's date" }, JsonRequestBehavior.AllowGet);
                    break;
                case -90:
                    return Json(new { Message = "The user don't have permission to work on this server" }, JsonRequestBehavior.AllowGet);
                    break;
                case -100:
                    return Json(new { Message = "Can't save, because there are transactions not posted in oldest date" }, JsonRequestBehavior.AllowGet);
                    break;
                case -660:
                    return Json(new { Message = "Transaction already exist" }, JsonRequestBehavior.AllowGet);
                    break;
                default:
                    return Json(new { Message = "Unknown Error" }, JsonRequestBehavior.AllowGet);
            }
        }
         public JsonResult SaveProdction(short CompNo, short ReportYear, int ReportNo, short OrderYear, int OrderNo, string UserID, ProdCost_DailyProductionH DailyProdHf, List<ProdCost_DailyProductionD> DailyProdD)
        {

         
                User me = (User)Session["me"];
                int ReportNos;

                DataRow row;
                int i = 1;

                using (SqlConnection cn = new SqlConnection(ConnectionString()))
                {
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlTransaction myTrans;
                cn.Open();
                myTrans = cn.BeginTransaction();
                try
                {
                  

                    using (SqlCommand cmd = new SqlCommand())
                    {

                        cmd.Connection = cn;
                        cmd.Transaction = myTrans;

                        SqlCommand selectOrderHF = new SqlCommand("SELECT COUNT(*) FROM ProdCost_DailyProductionH WHERE CompNo = @CompNo AND ReportYear = @ReportYear AND ReportNo = @ReportNo", cn);
                        selectOrderHF.Transaction = myTrans; // Set the transaction here
                        selectOrderHF.Parameters.AddWithValue("@CompNo", CompNo);
                        selectOrderHF.Parameters.AddWithValue("@ReportYear", DailyProdHf.ReportYear);
                        selectOrderHF.Parameters.AddWithValue("@ReportNo", DailyProdHf.ReportNo);

                        SqlCommand selectOrderDF = new SqlCommand("SELECT COUNT(*) FROM ProdCost_DailyProductionD WHERE CompNo = @CompNo AND ReportYear = @ReportYear AND ReportNo = @ReportNo", cn);
                        selectOrderDF.Transaction = myTrans; // Set the transaction here
                        selectOrderDF.Parameters.AddWithValue("@CompNo", CompNo);
                        selectOrderDF.Parameters.AddWithValue("@ReportYear", DailyProdHf.ReportYear);
                        selectOrderDF.Parameters.AddWithValue("@ReportNo", DailyProdHf.ReportNo);
                        //selectOrderDF.Parameters.AddWithValue("@ItemSer", ItemSer);

                        int countH = (int)selectOrderHF.ExecuteScalar();
                        int countD = (int)selectOrderDF.ExecuteScalar();

                        // Delete from InvT_DelIssueOrderDF
                        if (countH > 0 && countD > 0)
                        {


                            cmd.CommandText = "ProdCost_DelDailyProduction";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                            cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = ReportYear;
                            cmd.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int)).Value = ReportNo;
                            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = UserID;
                            cmd.Transaction = myTrans;
                            cmd.ExecuteNonQuery();
                        }



                    }



                    using (SqlCommand CmdH = new SqlCommand())
                    {
                        CmdH.Connection = cn;
                        CmdH.CommandText = "ProdCost_AddDailyProductionH";
                        CmdH.CommandType = CommandType.StoredProcedure;
                        CmdH.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = CompNo;
                        CmdH.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = DailyProdHf.ReportYear;
                        CmdH.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int)).Value = DailyProdHf.ReportNo;
                        CmdH.Parameters.Add(new SqlParameter("@Prod_Date", SqlDbType.SmallDateTime)).Value = DailyProdHf.Prod_Date;
                        CmdH.Parameters.Add(new SqlParameter("@ShiftNo", SqlDbType.Int)).Value = 0;
                        CmdH.Parameters.Add(new SqlParameter("@Prod_stage", SqlDbType.Int)).Value = DailyProdHf.Prod_stage;
                        CmdH.Parameters.Add(new SqlParameter("@MachineNo", SqlDbType.Int)).Value = 1;
                        CmdH.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                        CmdH.Parameters.Add(new SqlParameter("@UsedSrNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                        CmdH.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt)).Value=OrderYear;
                        CmdH.Parameters.Add(new SqlParameter("@ProdPrepNo", SqlDbType.Int)).Value = OrderNo;
                        CmdH.Transaction = myTrans;
                        CmdH.ExecuteNonQuery();

                        if (Convert.ToInt32(CmdH.Parameters["@UsedSrNo"].Value) == -10)
                        {
                            myTrans.Rollback();
                            return Json(new { ok = "Error Message " }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ReportNos = Convert.ToInt32(CmdH.Parameters["@UsedSrNo"].Value);
                        }
                    }

                    // Add details
                    DataSet dataset = new DataSet();
                    DataSet dataset1 = new DataSet();
                    SqlDataAdapter Adapter = new SqlDataAdapter();
                    DataRow Datarow2;
                  


                        DataTable TmpDtD = new DataTable();
                        TmpDtD.Columns.Add("CompNo", typeof(short));
                        TmpDtD.Columns.Add("ReportYear", typeof(short));
                        TmpDtD.Columns.Add("ReportNo", typeof(int));
                        TmpDtD.Columns.Add("ItemSer", typeof(int));
                        TmpDtD.Columns.Add("ProdPrepYear", typeof(short));
                        TmpDtD.Columns.Add("ProdPrepNo", typeof(int));
                        TmpDtD.Columns.Add("FinCode", typeof(string));
                        TmpDtD.Columns.Add("Prod_Qty", typeof(double));
                        TmpDtD.Columns.Add("Batch", typeof(string));
                        TmpDtD.Columns.Add("ManDate", typeof(DateTime));
                        TmpDtD.Columns.Add("ExpDate", typeof(DateTime));
                        TmpDtD.Columns.Add("Tunit", typeof(string));
                        TmpDtD.Columns.Add("UnitSerial", typeof(short));


                        foreach (var rows in DailyProdD)
                        {
                            //if (row.RowState != DataRowState.Deleted)
                            //{
                            DataRow datarow = TmpDtD.NewRow();
                            datarow["CompNo"] = rows.CompNo;
                            datarow["ReportYear"] = DailyProdHf.ReportYear;
                            datarow["ReportNo"] = ReportNos;
                            datarow["ItemSer"] = i;
                            datarow["ProdPrepYear"] = rows.ProdPrepYear;
                            datarow["ProdPrepNo"] = rows.ProdPrepNo;
                            datarow["FinCode"] = rows.FinCode;
                            datarow["Prod_Qty"] = rows.Prod_Qty;
                            datarow["Batch"] ="";
                            datarow["ManDate"] = DateTime.Now;
                            datarow["ExpDate"] = DateTime.Now;
                            datarow["Tunit"] = rows.TUnit;
                            datarow["UnitSerial"] = rows.UnitSerial;

                            TmpDtD.Rows.Add(datarow);
                            i++;
                        }


                    using (SqlCommand CmdDIns = new SqlCommand())
                    {
                        CmdDIns.Connection = cn;
                        CmdDIns.CommandText = "ProdCost_AddDailyProductionD";
                        CmdDIns.CommandType = CommandType.StoredProcedure;
                        CmdDIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = CompNo;
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportYear", SqlDbType.SmallInt, 4, "ReportYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ReportNo", SqlDbType.Int, 8, "ReportNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.Int, 8, "ItemSer"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepYear", SqlDbType.SmallInt, 4, "ProdPrepYear"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ProdPrepNo", SqlDbType.Int, 8, "ProdPrepNo"));
                        CmdDIns.Parameters.Add(new SqlParameter("@FinCode", SqlDbType.VarChar, 20, "FinCode"));
                        CmdDIns.Parameters.Add(new SqlParameter("@Prod_Qty", SqlDbType.Money, 8, "Prod_Qty"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = me.UserID;
                        CmdDIns.Parameters.Add(new SqlParameter("@Batch", SqlDbType.VarChar, 16, "Batch"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ManDate", SqlDbType.SmallDateTime, 10, "ManDate"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ExpDate", SqlDbType.SmallDateTime, 10, "ExpDate"));
                        CmdDIns.Parameters.Add(new SqlParameter("@ConsQty", SqlDbType.Money, 8)).Value = 0;
                        CmdDIns.Parameters.Add(new SqlParameter("@Tunit", SqlDbType.VarChar, 5, "Tunit"));
                        CmdDIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdDIns.Parameters.Add(new SqlParameter("@TransType", SqlDbType.SmallInt, 4)).Value = 1;
                        CmdDIns.Transaction = myTrans;
                        adapter.InsertCommand = CmdDIns;
                        adapter.Update(TmpDtD);
                    }

                    myTrans.Commit();
                     

                    return Json(new { ok = Resources.Resource.Done }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    myTrans.Rollback();
                    return Json(new { ok = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    cn.Close();

                }
                return Json(new { ok = Resources.Resource.Done }, JsonRequestBehavior.AllowGet);
            }
        
        }
    }

}