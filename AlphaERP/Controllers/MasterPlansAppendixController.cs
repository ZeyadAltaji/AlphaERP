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
    public class MasterPlansAppendixController : controller
    {
        // GET: ProductionPlanAnalysis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllProductionPlan(int year)
        {
            List<MRP_GeneralPlanInfoH> PlanH = db.MRP_GeneralPlanInfoH.Where(x => x.PlanYear == year).ToList();
            return PartialView(PlanH);
        }
        public ActionResult ProdPlanDetails(short Planyear, int PlanNo)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "MRPW_GetAllGeneralPlanDet";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@PlanYear", System.Data.SqlDbType.Int)).Value = Planyear;
                    cmd.Parameters.Add(new SqlParameter("@PlanNo", System.Data.SqlDbType.Int)).Value = PlanNo;

                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<MRP_ProdPlanDetails> lMRP_ProductionPlain = Dt.AsEnumerable().Select(row => new MRP_ProdPlanDetails
            {
                PlanYear = row.Field<short>("PlanYear"),
                PlanNo = row.Field<int>("PlanNo"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate"),
                ItemNo = row.Field<string>("ItemNo"),
                ItemDesc = row.Field<string>("ItemDesc"),
                ItemDesc_Ara = row.Field<string>("ItemDesc_Ara"),
                FormulaCode = row.Field<string>("FormulaCode"),
                formula_desc = row.Field<string>("formula_desc"),
                UnitDesc = row.Field<string>("UnitCode"),
                PQty = row.Field<decimal>("PQty"),
                PlanNote = row.Field<string>("PlanNotes")
            }).ToList();

            return PartialView(lMRP_ProductionPlain);
        }
        
        public ActionResult ProductionPlanAppendix(MRP_ProdPlanDetails SubDetails)
        {
            return PartialView(SubDetails);
        }

        public JsonResult CreateGeneralPlanSub(List<MRP_GeneralPlanSubDtls> PlanSub)
        {
            short i = 1;
            List<MRP_GeneralPlanSubDtls> lPlanSub = PlanSub.OrderBy(x => x.FromDate).ToList();
            foreach (MRP_GeneralPlanSubDtls item in lPlanSub)
            {
                List<MRP_GeneralPlanSubDtls> t = lPlanSub.Where(x => x.FromDate < item.FromDate).ToList();
                if (t.Count != 0)
                {
                    foreach (MRP_GeneralPlanSubDtls item1 in t)
                    {
                        DateTime FromDate = item.FromDate.Value.Date;
                        DateTime FromDate1 = item1.FromDate.Value.Date;
                        DateTime ToDate = item1.ToDate.Value.Date;

                        if(FromDate > FromDate1 && FromDate < ToDate)
                        {
                            return Json(new { error = "يوجد تداخل بالتواريخ" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            foreach (MRP_GeneralPlanSubDtls subitem in lPlanSub)
            {
                subitem.CompNo = company.comp_num;
                subitem.Sr = i;
                i++;
            }
            MRP_GeneralPlanSubDtls GeneralPlan = lPlanSub.FirstOrDefault();
            List<MRP_GeneralPlanSubDtls> l = db.MRPGeneralPlanSubDtls.Where(x => x.CompNo == GeneralPlan.CompNo && x.PlanYear == GeneralPlan.PlanYear
                         && x.PlanNo == GeneralPlan.PlanNo && x.ItemNo == GeneralPlan.ItemNo
                         && x.FormulaCode == GeneralPlan.FormulaCode).ToList();
            if(l.Count != 0)
            {
                db.MRPGeneralPlanSubDtls.RemoveRange(l);
                db.SaveChanges();
            }
            db.MRPGeneralPlanSubDtls.AddRange(lPlanSub);
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}