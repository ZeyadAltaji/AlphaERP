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
    public class ActivateCancelProductionPlanController : controller
    {
        // GET: ActivateCancelProductionPlan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RequestProductionPlain(int RequestType,int year)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "MRPW_GetProductionPlain";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@RequestType", System.Data.SqlDbType.SmallInt)).Value = RequestType;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", System.Data.SqlDbType.Int)).Value = year;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<MRP_ProductionPlain> lMRP_ProductionPlain = Dt.AsEnumerable().Select(row => new MRP_ProductionPlain
            {
                PlanYear = row.Field<short>("PlanYear"),
                PlanNo = row.Field<int>("PlanNo"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate"),
                PlanNotes = row.Field<string>("PlanNotes")
            }).ToList();
            return PartialView(lMRP_ProductionPlain);
        }
        public ActionResult ActivatePlan(MRP_ProductionPlain ActivateProdPlan)
        {
            return PartialView(ActivateProdPlan);
        }
        public ActionResult DeactivatePlan(MRP_ProductionPlain DeactivateProdPlan)
        {
            return PartialView(DeactivateProdPlan);
        }
        public JsonResult Save_ActivatePlan(MRPActivatePlan MRPActivate)
        {
            MRPActivate.ReqYear = Convert.ToInt16(MRPActivate.ReqDate.Value.Year);

            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "MRPW_AddActiveDeActivPlan";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@RequestType", System.Data.SqlDbType.SmallInt)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@ReqYear", System.Data.SqlDbType.SmallInt)).Value = MRPActivate.ReqYear;
                    cmd.Parameters.Add(new SqlParameter("@ReqNo", System.Data.SqlDbType.Int)).Value = MRPActivate.ReqNo;
                    cmd.Parameters.Add(new SqlParameter("@ReqDate", System.Data.SqlDbType.SmallDateTime)).Value = MRPActivate.ReqDate;
                    cmd.Parameters.Add(new SqlParameter("@BUID", System.Data.SqlDbType.SmallInt)).Value = MRPActivate.BUID;
                    cmd.Parameters.Add(new SqlParameter("@UserID", System.Data.SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", System.Data.SqlDbType.SmallInt)).Value = MRPActivate.PlanYear;
                    cmd.Parameters.Add(new SqlParameter("@PlanNo", System.Data.SqlDbType.Int)).Value = MRPActivate.PlanNo;
                    cmd.Parameters.Add(new SqlParameter("@Activiate", System.Data.SqlDbType.Bit)).Value = MRPActivate.Activiate;
                    cmd.Parameters.Add(new SqlParameter("@Reasoun", System.Data.SqlDbType.VarChar)).Value = MRPActivate.Reasoun;
                    cmd.Parameters.Add(new SqlParameter("@Notes", System.Data.SqlDbType.VarChar)).Value = MRPActivate.Notes;
                    cmd.Parameters.Add(new SqlParameter("@ReqStatus", System.Data.SqlDbType.Bit)).Value = MRPActivate.ReqStatus;
                    cmd.Parameters.Add(new SqlParameter("@OrdNo", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;
                    transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        conn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    int OrdNo = Convert.ToInt32(cmd.Parameters["@OrdNo"].Value);
                    string WFTxt = "";
                    string TrNo = "( " + MRPActivate.PlanYear + " - " + MRPActivate.PlanNo + " ) ";
                    WFTxt = "طلب تفعيل خطة رقم " + TrNo + " من  قبل المستخدم " + me.UserID;

                    bool InsertWorkFlow = AddWorkFlowLog(50, MRPActivate.ReqYear, MRPActivate.BUID, OrdNo, WFTxt, transaction, conn,"1");
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        conn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    transaction.Commit();
                    conn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save_DeactivatePlan(MRPActivatePlan MRPDeActivate)
        {
            MRPDeActivate.ReqYear = Convert.ToInt16(MRPDeActivate.ReqDate.Value.Year);
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "MRPW_AddActiveDeActivPlan";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@RequestType", System.Data.SqlDbType.SmallInt)).Value = 2;
                    cmd.Parameters.Add(new SqlParameter("@ReqYear", System.Data.SqlDbType.SmallInt)).Value = MRPDeActivate.ReqYear;
                    cmd.Parameters.Add(new SqlParameter("@ReqNo", System.Data.SqlDbType.Int)).Value = MRPDeActivate.ReqNo;
                    cmd.Parameters.Add(new SqlParameter("@ReqDate", System.Data.SqlDbType.SmallDateTime)).Value = MRPDeActivate.ReqDate;
                    cmd.Parameters.Add(new SqlParameter("@BUID", System.Data.SqlDbType.SmallInt)).Value = MRPDeActivate.BUID;
                    cmd.Parameters.Add(new SqlParameter("@UserID", System.Data.SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", System.Data.SqlDbType.SmallInt)).Value = MRPDeActivate.PlanYear;
                    cmd.Parameters.Add(new SqlParameter("@PlanNo", System.Data.SqlDbType.Int)).Value = MRPDeActivate.PlanNo;
                    cmd.Parameters.Add(new SqlParameter("@Activiate", System.Data.SqlDbType.Bit)).Value = MRPDeActivate.Activiate;
                    cmd.Parameters.Add(new SqlParameter("@Reasoun", System.Data.SqlDbType.VarChar)).Value = MRPDeActivate.Reasoun;
                    cmd.Parameters.Add(new SqlParameter("@Notes", System.Data.SqlDbType.VarChar)).Value = MRPDeActivate.Notes;
                    cmd.Parameters.Add(new SqlParameter("@ReqStatus", System.Data.SqlDbType.Bit)).Value = MRPDeActivate.ReqStatus;
                    cmd.Parameters.Add(new SqlParameter("@OrdNo", System.Data.SqlDbType.Int)).Direction = System.Data.ParameterDirection.Output;
                    transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        conn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    int OrdNo = Convert.ToInt32(cmd.Parameters["@OrdNo"].Value);
                    string WFTxt = "";
                    string TrNo = "( " + MRPDeActivate.PlanYear + " - " + MRPDeActivate.PlanNo + " ) ";
                    WFTxt = "طلب إلغاء تفعيل خطة رقم " + TrNo + " من قبل المستخدم " + me.UserID;

                    bool InsertWorkFlow = AddWorkFlowLog(50, MRPDeActivate.ReqYear, MRPDeActivate.BUID, OrdNo, WFTxt, transaction, conn,"2");
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        conn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    transaction.Commit();
                    conn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLog(int FID, int TrYear, int BU, long TrNo, string TrFormDesc, SqlTransaction MyTrans, SqlConnection co,string K3)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = co;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Alpha_AddWorkFlowLog";
                cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@FID", System.Data.SqlDbType.SmallInt)).Value = FID;
                cmd.Parameters.Add(new SqlParameter("@BU", System.Data.SqlDbType.SmallInt)).Value = BU;
                cmd.Parameters.Add(new SqlParameter("@UserID", System.Data.SqlDbType.VarChar)).Value = me.UserID;
                cmd.Parameters.Add(new SqlParameter("@K1", System.Data.SqlDbType.VarChar)).Value = TrYear;
                cmd.Parameters.Add(new SqlParameter("@K2", System.Data.SqlDbType.VarChar)).Value = TrNo;
                cmd.Parameters.Add(new SqlParameter("@K3", System.Data.SqlDbType.VarChar)).Value = K3;
                cmd.Parameters.Add(new SqlParameter("@K4", System.Data.SqlDbType.VarChar)).Value = 1;
                cmd.Parameters.Add(new SqlParameter("@TrAmount", System.Data.SqlDbType.Money)).Value = 0;
                cmd.Parameters.Add(new SqlParameter("@FrmStat", System.Data.SqlDbType.SmallInt)).Value = 1;
                cmd.Parameters.Add(new SqlParameter("@TrFormDesc", System.Data.SqlDbType.VarChar)).Value = TrFormDesc;
                cmd.Parameters.Add(new SqlParameter("@FinalApprove", System.Data.SqlDbType.Bit)).Direction = System.Data.ParameterDirection.Output;
                cmd.Transaction = MyTrans;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string zz = ex.Message;
                    return false;
                }
            }
            return true;
        }
      
    }
}