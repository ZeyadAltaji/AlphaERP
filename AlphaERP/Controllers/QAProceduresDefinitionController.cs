using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class QAProceduresDefinitionController : controller
    {
        // GET: QAProceduresDefinition
        public ActionResult Index()
        {
            short CompNo = company.comp_num;
             List<ProdCost_QASetupHFView> ProdCost_QASetupHF = db.Database.SqlQuery<ProdCost_QASetupHFView>("exec ProdCost_WebLists  {0},{1}", CompNo, 7).ToList();
            return View(ProdCost_QASetupHF);
        }
        public ActionResult List()
        {
            //List<ProdCost_QASetupHF> ProdCost_QASetupHF = db.ProdCost_QASetupHF.Where(x => x.CompNo == company.comp_num).ToList();
            short CompNo = company.comp_num;
             List<ProdCost_QASetupHFView> ProdCost_QASetupHF = db.Database.SqlQuery<ProdCost_QASetupHFView>("exec ProdCost_WebLists  {0},{1}", CompNo, 7).ToList();

            return View(ProdCost_QASetupHF);
        }
        public ActionResult InsertData()
        {

             return PartialView();
        }
 
        [HttpPost]
        public JsonResult send(ProdCost_QASetupHF ProdCost_QASetupHF, List<ProdCost_QASetup> Description)
        {
            short compNum = company.comp_num;
            ProdCost_QASetupHF.CompNo = compNum;
            User me = (User)Session["me"];
            db.ProdCost_QASetupHF.Add(ProdCost_QASetupHF);
            db.SaveChanges();

            short lastProcSubNo = db.ProdCost_QASetup.Where(x => x.CompNo == compNum && x.QA_ProcNo == ProdCost_QASetupHF.QA_ProcNo).Select(x => x.Proc_SubNo).DefaultIfEmpty().Max();
            foreach (var desc in Description)
            {
                lastProcSubNo++;
                ProdCost_QASetup prodCost_QASetup = new ProdCost_QASetup
                {
                    CompNo = compNum,
                    QA_ProcNo = ProdCost_QASetupHF.QA_ProcNo,
                    Description = desc.Description,
                    UserID =me.UserID,
                    Proc_SubNo = lastProcSubNo,
                    AddDate  = DateTime.Today

                };

                db.ProdCost_QASetup.Add(prodCost_QASetup);
                db.SaveChanges();

            }
            return Json(new { success = true, message = "Data inserted successfully." });
        }
       
        public ActionResult QAProcDefinitionList(short Comp_no)
        {
            short compNum = company.comp_num;

            var filteredData = db.ProdCost_QASetupHF.Where(x => x.CompNo == Comp_no).ToList();

            return PartialView(filteredData);
        }
        public ActionResult DescriptionList(short Comp_no)
        {
            short compNum = company.comp_num;

            var filteredData = db.ProdCost_QASetup.Where(x => x.CompNo == Comp_no).ToList();

            return PartialView(filteredData);
        }
        
       [HttpGet]
        public ActionResult Edit(short QAProc)
        {
            ViewBag.QAProc = QAProc;
            return PartialView();
        }
        [HttpPost]
        public JsonResult Edit(ProdCost_QASetupHF ProdCost_QASetupHF, List<ProdCost_QASetup> Description)
        {
            short compNum = company.comp_num;
            User me = (User)Session["me"];
             var existingData = db.ProdCost_QASetupHF.FirstOrDefault(qa => qa.CompNo == compNum && qa.QA_ProcNo == ProdCost_QASetupHF.QA_ProcNo);
            //short lastProcSubNo = db.ProdCost_QASetup.Where(x => x.CompNo == compNum && x.QA_ProcNo == ProdCost_QASetupHF.QA_ProcNo).Select(x => x.Proc_SubNo).DefaultIfEmpty().Max();
            short i = 0;
            if (existingData != null)
            {
                // Check if any of the properties have changed
                bool hasChanges = false;

                if (existingData.QA_Desc != ProdCost_QASetupHF.QA_Desc)
                {
                    existingData.QA_Desc = ProdCost_QASetupHF.QA_Desc;
                    hasChanges = true;
                }

                if (existingData.M_BusUnitID != ProdCost_QASetupHF.M_BusUnitID)
                {
                    existingData.M_BusUnitID = ProdCost_QASetupHF.M_BusUnitID;
                    hasChanges = true;
                }

                if (existingData.C_BusUnitID != ProdCost_QASetupHF.C_BusUnitID)
                {
                    existingData.C_BusUnitID = ProdCost_QASetupHF.C_BusUnitID;
                    hasChanges = true;
                }

                if (existingData.PostOnSeq != ProdCost_QASetupHF.PostOnSeq)
                {
                    existingData.PostOnSeq = ProdCost_QASetupHF.PostOnSeq;
                    hasChanges = true;
                }

                if (hasChanges)
                {
                    db.SaveChanges();
                    ViewBag.Message = "Data updated successfully.";
                }
                else
                {
                    ViewBag.Message = "No changes were made.";
                }
            }

            //    foreach (var desc in Description)
            //    {
            //        var existingDescData = db.ProdCost_QASetup.FirstOrDefault(qa => qa.CompNo == compNum && qa.QA_ProcNo == desc.QA_ProcNo && qa.Proc_SubNo == desc.Proc_SubNo);
            //     if (existingDescData != null)
            //        {

            //            existingDescData.CompNo = compNum;
            //            existingDescData.QA_ProcNo = desc.QA_ProcNo;
            //            existingDescData.Proc_SubNo = desc.Proc_SubNo;
            //            existingDescData.UserID = me.UserID;
            //            existingDescData.Description = desc.Description;
            //            existingDescData.AddDate = DateTime.Today;
            //            db.SaveChanges();


            //    }
            //    else if (desc.QA_ProcNo != null && desc.Proc_SubNo != null)
            //        {
            //             lastProcSubNo++;
            //            desc.CompNo = compNum;
            //            desc.QA_ProcNo = ProdCost_QASetupHF.QA_ProcNo;
            //            desc.Proc_SubNo = lastProcSubNo;
            //            desc.UserID = me.UserID;
            //            desc.Description = desc.Description;
            //            desc.AddDate = DateTime.Today;

            //            db.ProdCost_QASetup.Add(desc);
            //        db.SaveChanges();

            //    }

            //}
            var existingDescriptions = db.ProdCost_QASetup
       .Where(qa => qa.CompNo == compNum && qa.QA_ProcNo == ProdCost_QASetupHF.QA_ProcNo)
       .ToList();

            foreach (var existingDesc in existingDescriptions)
            {
                db.ProdCost_QASetup.Remove(existingDesc);
            }
            db.SaveChanges();

            short lastProcSubNo = db.ProdCost_QASetup
                .Where(x => x.CompNo == compNum && x.QA_ProcNo == ProdCost_QASetupHF.QA_ProcNo)
                .Select(x => x.Proc_SubNo)
                .DefaultIfEmpty()
                .Max();

            foreach (var desc in Description)
            {
                i++;
                desc.CompNo = compNum;
                desc.QA_ProcNo = ProdCost_QASetupHF.QA_ProcNo;
                desc.Proc_SubNo = i;
                desc.UserID = me.UserID;
                desc.AddDate = DateTime.Today;

                db.ProdCost_QASetup.Add(desc);
            }
            db.SaveChanges();


            ViewBag.Message = "Data updated successfully.";

            return Json(new { success = true, message = "Data updated successfully" });
        }
        [HttpPost]
        public JsonResult Delete(int QA_ProcNo)
        {
            short compNum = company.comp_num;

            using (var context = new MDB())
            {
                var qaProcNoParam = new SqlParameter("@QA_ProcNo", QA_ProcNo);
                var compNoParam = new SqlParameter("@CompNo", compNum);

                context.Database.ExecuteSqlCommand("EXEC [dbo].[DeleteQAData] @QA_ProcNo, @CompNo", qaProcNoParam, compNoParam);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}