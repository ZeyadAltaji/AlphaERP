using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class MasterPlansController : controller
    {
        public ActionResult Index()
        {
            List<MRP_GeneralPlanInfoH> List = db.MRP_GeneralPlanInfoH.Where(x => x.CompNo == company.comp_num && x.PlanYear == DateTime.Now.Year).ToList();
            return View(List);
        }

        public ActionResult List(short PlanYear)
        {
            List<MRP_GeneralPlanInfoH> List = db.MRP_GeneralPlanInfoH.Where(x => x.CompNo == company.comp_num && x.PlanYear == PlanYear).ToList();
            return View(List);
        }

        public JsonResult Action(MRP_GeneralPlanInfoH HF, List<MRP_GeneralPlanInfoD> DF)
        {
            int PlanNo = 1;
            string query = string.Format("select * from Mrp_Serial where compno = '{0}'  and SYear = '{1}' and progid = 4", company.comp_num, HF.PlanYear);
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    int ExPlanNo = Convert.ToInt32(rdr["CurrSr"].ToString());
                    if (ExPlanNo > 0)
                    {
                        PlanNo += ExPlanNo;
                    }
                    else
                    {
                        string insert_ = string.Format("Insert into Mrp_Serial (CompNo,SYear,ProgId,CurrSr)values({0},{1},4,1)", company.comp_num, HF.PlanYear);
                        Excute(insert_);
                    }
                }
            }
            if (HF.PlanNo == 0)
            {
                // Add New Plan
                HF.PlanActive = false;
                HF.EntryDate = DateTime.Now;
                HF.PlanNo = PlanNo;
                HF.UserID = me.UserID;
                HF.CompNo = company.comp_num;
                db.MRP_GeneralPlanInfoH.Add(HF);

                if (DF != null)
                    if (DF.Count > 0)
                        foreach (var d in DF)
                        {
                            d.PlanNo = HF.PlanNo;
                            d.CompNo = company.comp_num;
                        }

                db.MRP_GeneralPlanInfoD.AddRange(DF);
                db.SaveChanges();

                string update_ = string.Format("Update Mrp_Serial Set CurrSr={2} where CompNo={0} and SYear={1} and ProgId=4", company.comp_num, HF.PlanYear, PlanNo);
                Excute(update_);
            }
            else
            {
                MRP_GeneralPlanInfoH exHF = db.MRP_GeneralPlanInfoH.Where(x => x.CompNo == company.comp_num && x.PlanYear == HF.PlanYear && x.PlanNo == HF.PlanNo).FirstOrDefault();
                exHF.CompNo = company.comp_num;
                exHF.EndDate = HF.EndDate;
                exHF.EntryDate = HF.EntryDate;
                if (exHF.EntryDate == null)
                {
                    exHF.EntryDate = DateTime.Now;
                }
                exHF.PlanNo = HF.PlanNo;
                exHF.PlanNotes = HF.PlanNotes;
                exHF.PlanYear = HF.PlanYear;
                exHF.StartDate = HF.StartDate;
                exHF.UserID = me.UserID;
                List<MRP_GeneralPlanInfoD> exDF = db.MRP_GeneralPlanInfoD
                    .Where(x => x.CompNo == HF.CompNo && x.PlanNo == HF.PlanNo && x.PlanYear == HF.PlanYear).ToList();
                db.MRP_GeneralPlanInfoD.RemoveRange(exDF);
                db.SaveChanges();
                if (DF != null)
                    if (DF.Count > 0)
                        foreach (var d in DF)
                        {
                            d.PlanNo = HF.PlanNo;
                            d.CompNo = company.comp_num;
                        }

                db.MRP_GeneralPlanInfoD.AddRange(DF);
                db.SaveChanges();

            }

            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MRP_GeneralPlanInfoHList()
        {
            List<MRP_GeneralPlanInfoH> List = db.MRP_GeneralPlanInfoH.ToList();
            return View("~/Views/MasterPlans/List.cshtml", List);
        }
        [HttpPost]
        public JsonResult GetThisPlan(short CompNo, short PlanYear, int PlanNo)
        {
            MRP_GeneralPlanInfoH HF = db.MRP_GeneralPlanInfoH.Where(x => x.CompNo == CompNo && x.PlanYear == PlanYear && x.PlanNo == PlanNo).FirstOrDefault();
            List<TMP_GeneralPlanInfoD> DF = new List<TMP_GeneralPlanInfoD>();
            string query = string.Format("MRP_WEB_GeneralPlanInfoH {0},{1},{2}", company.comp_num, HF.PlanYear, HF.PlanNo);
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        DF.Add(new TMP_GeneralPlanInfoD()
                        {
                            CompNo = HF.CompNo,
                            PlanNo = Convert.ToInt32(rdr["PlanNo"].ToString()),
                            FormulaCode = (string)rdr["FormulaCode"],
                            ItemNo = (string)rdr["ItemNo"],
                            UnitCode = (string)rdr["UnitCode"],
                            PQty = (decimal)rdr["PQty"]
                        });
                    }
                }
            }
            foreach (var df in DF)
            {
                df.Formula = db.prod_formula_header_info.Where(x => x.formula_code == df.FormulaCode).FirstOrDefault();
                df.Item = db.InvItemsMFs.Where(x => x.ItemNo == df.ItemNo && x.CompNo == df.CompNo).FirstOrDefault();
            }
            HF.MRP_GeneralPlanInfoD.Clear();
            return Json(new { HF, DF }, JsonRequestBehavior.AllowGet);
        }



    }

}