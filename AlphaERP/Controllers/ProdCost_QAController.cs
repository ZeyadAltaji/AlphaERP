using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AlphaERP.Controllers
{
    public class ProdCost_QAController : controller
    { 
        // GET: ProdCost_QA page
        public ActionResult Index()
        {
            return View();
        }
     
        public ActionResult ListProdCost_QARequests()
        {
            List<ProdCost_QARequestDataModels> requestData = db.Database.SqlQuery<ProdCost_QARequestDataModels>("exec ProdCost_QARequest {0}", company.comp_num).ToList();
            return PartialView(requestData);
        }
        public ActionResult LoadListEventsummary(short reqNo, int? QA_ProcNo)
        {
            ViewBag.QA_ProcNo = QA_ProcNo;
            ViewBag.MainReqNo = reqNo;
 

            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            var DsListEventsummary = new DataSet();
            var DaListEventsummary = new SqlDataAdapter();
            var DaCmd = new SqlDataAdapter();
            var Cmd = new SqlCommand();
            Cmd.Connection = cn;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = "ProdCost_QAEventListDatasummary";
            Cmd.Parameters.Add(new SqlParameter("@MainReqNo", SqlDbType.BigInt, 2)).Value = reqNo;
            DaListEventsummary.SelectCommand = Cmd;
            DsListEventsummary.Tables.Clear();
            DsListEventsummary.Dispose();
            DaListEventsummary.Fill(DsListEventsummary, "ListEventsummary");
            return PartialView(DsListEventsummary);
        }
        public ActionResult GetByReqNo(short CompNo, int QA_ProcNo, long MainReqNo, bool AddMood)
        {
            try
            {
                List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("exec ProdCost_GetQAEvents {0},{1},{2},{3}", CompNo, QA_ProcNo, MainReqNo, AddMood).ToList();
                User user = (User)Session["me"];
                var UserID = user.UserID;
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });
            }

        }
        public ActionResult GetByReqNoedit(short CompNo, int QA_ProcNo, long MainReqNo, short Eventserial, bool AddMood)
        {
            try
            {
                List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("exec ProdCost_GetQAEvents {0},{1},{2},{3},{4}", CompNo, QA_ProcNo, MainReqNo, Eventserial, AddMood).ToList();
                User user = (User)Session["me"];
                var UserID = user.UserID;
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });
            }

        }
        public ActionResult GetByModalNoedit(short CompNo, int QA_ProcNo, long MainReqNo, short Eventserial, bool AddMood)
        {
            try
            {
                List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("exec ProdCost_GetQAEvents {0},{1},{2},{3},{4}", CompNo, QA_ProcNo, MainReqNo, Eventserial, AddMood).ToList();
                User user = (User)Session["me"];
                var UserID = user.UserID;
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });


            }

        }
        public ActionResult GetByModalNoShow(short CompNo, int QA_ProcNo, long MainReqNo, short Eventserial, bool AddMood)
        {
            try
            {
                List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("exec ProdCost_GetQAEvents {0},{1},{2},{3},{4}", CompNo, QA_ProcNo, MainReqNo, Eventserial, AddMood).ToList();
                User user = (User)Session["me"];
                var UserID = user.UserID;
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting event data: " + ex.Message });


            }

        }
        public JsonResult insertMaker(ProdCost_QAEvents eventData, short Proc_SubNo, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            var item = db.ProdCost_QAEvents.FirstOrDefault(x => x.MainReqNo == MainReqNo && x.Proc_SubNo == Proc_SubNo && x.EventSerial == Eventserial);

            if (item == null)
            {
                // If there's no existing record with the given MainReqNo and Proc_SubNo, create a new one
                item = new ProdCost_QAEvents();
                item.CompNo = company.comp_num;
                item.MainReqNo = MainReqNo;
                item.Proc_SubNo = Proc_SubNo;
                item.EventSerial = Eventserial;
                item.Proc_MainNo = Proc_MainNo;
                item.MakerValue = eventData.MakerValue;
                item.MDate = DateTime.Now;
                item.MUser = me.UserID;
                item.MPostStat = eventData.MakerValue != 0;
                item.MakerReadValue = eventData.MakerReadValue;

                db.ProdCost_QAEvents.Add(item);
            }
            db.SaveChanges();
            return Json(new { success = true, message = "Event data insert/update successfully." });

        }
        public JsonResult editMaker(ProdCost_QAEvents eventData, short Proc_SubNo, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            var existingDataList = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo && x.EventSerial == Eventserial).ToList();
            var existingData = existingDataList.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);
            // Check if MakerValue is not null  
            if (existingData.MakerValue != null || existingData.MakerValue == null)
            {
                // Handle Maker logic here
                existingData.MakerValue = eventData.MakerValue;
                existingData.MDate = DateTime.Now;
                existingData.MUser = me.UserID;
                existingData.MPostStat = eventData.MakerValue != 0;
                existingData.MakerReadValue = eventData.MakerReadValue;
                db.SaveChanges();

            }
            return Json(new { success = true, message = "Event data insert/update successfully." });

        }
        public JsonResult insertChecker(ProdCost_QAEvents eventData, short Proc_SubNo, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            var existingDataList = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo && x.EventSerial == Eventserial && x.MakerValue != null).ToList();
            var existingData = existingDataList.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);


            // Check if MakerValue is not null  
            //if (existingData.MakerValue != null)
            //{
            // Handle Maker logic here
            existingData.CheckerValue = eventData.CheckerValue;
            existingData.CDate = DateTime.Now;
            existingData.CUser = me.UserID;
            existingData.CPostStat = eventData.CheckerValue != 0;
            existingData.CheckerReadValue = eventData.CheckerReadValue;

            //}
            db.SaveChanges();
            return Json(new { success = true, message = "Event data insert/update successfully." });


        }

        public JsonResult InsertEvent(ProdCost_QAEvents eventData, short Proc_SubNo, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            try
            {
                // Fetch the existing record based on MainReqNo
                var item = db.ProdCost_QAEvents.FirstOrDefault(x => x.MainReqNo == MainReqNo && x.Proc_SubNo == Proc_SubNo);
                // Check if CheckerValue is not null  
                if (eventData.CheckerValue != null)
                {
                    // Only update the CheckerValue and related properties, leave other fields unchanged
                    item.CheckerValue = eventData.CheckerValue;
                    item.CDate = DateTime.Now;
                    item.CUser = me.UserID;
                    item.CPostStat = eventData.CheckerValue != 0;
                    item.CheckerReadValue = eventData.CheckerReadValue;
                }

                // Save the changes to the database
                db.SaveChanges();

                return Json(new { success = true, message = "Event data insert/update successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error inserting/updating event data: " + ex.Message });
            }
        }
        public JsonResult GetByMainReqNoandProcSubNo(Int64 MainReqNo, short Proc_SubNo, short Eventserial)
        {
            List<ProdCost_QAEvent_SetupView> ListData = db.Database.SqlQuery<ProdCost_QAEvent_SetupView>("SELECT * FROM ProdCost_QAEvents WHERE MainReqNo = {0} AND Proc_SubNo = {1} AND EventSerial = {2}", MainReqNo, Proc_SubNo, Eventserial).ToList();
            return Json(ListData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveChanges(List<ProdCost_QAEvents> eventDataList, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            try
            {

                foreach (var eventData in eventDataList)
                {
                    var existingDataList = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo && x.EventSerial == Eventserial).ToList();
                    var existingData = existingDataList.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);
                    if (existingData == null)
                    {
                        var newItem = new ProdCost_QAEvents();
                        newItem.CompNo = company.comp_num;
                        newItem.MakerValue = eventData.MakerValue;
                        newItem.MakerReadValue = eventData.MakerReadValue;
                        newItem.MUser = me.UserID;
                        newItem.MainReqNo = MainReqNo;
                        newItem.Proc_SubNo = eventData.Proc_SubNo;
                        newItem.EventSerial = Eventserial;
                        newItem.Proc_MainNo = Proc_MainNo;

                        db.ProdCost_QAEvents.Add(newItem);
                    }
                }

                // Save changes to the database in one transaction
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error saving event data: " + ex.Message });
            }
            return Json(new { success = true, message = "Event data saved successfully." });

        }
        [HttpPost]
        public JsonResult SaveChangesedit(List<ProdCost_QAEvents> eventDataList, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            var editFinaldata = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo && x.EventSerial == Eventserial).ToList();
            var editProdCost_QARequests = db.ProdCost_QARequests.Where(x => x.CompNo == company.comp_num && x.ReqNo == MainReqNo);

             bool allCPostDatesTrue = editFinaldata.All(item => item.CPostStat == true);
             bool allCPostDatesFalse = editFinaldata.All(item => item.CPostStat == false);

            if (editFinaldata.All(item => item.MPostStat != null && item.CPostStat != null))
            {
                // Update the FinalPostUser and FPostDate for each record in the eventDataList
                foreach (var eventData in eventDataList)
                {
                    var existingData = editFinaldata.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);
                    if (existingData != null)
                    {
                        existingData.FinalPostUser = me.UserID;
                        existingData.FPostDate = DateTime.Now;
                    }
                     if (allCPostDatesTrue)
                    {
                        existingData.FinalPostStat = true;
                    }
                    else if (allCPostDatesFalse)
                    {
                        existingData.FinalPostStat = false;
                    }
                    else
                    {
                        existingData.FinalPostStat = false; // Mixed values
                    }

                }
                if (allCPostDatesTrue)
                {
                    foreach (var request in editProdCost_QARequests)
                    {
                        request.RequestStat = true;
                    }
                }

                db.SaveChanges();
             
                 

            }
            else
            {

                foreach (var eventData in eventDataList)
                {
                    var existingDataList = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo && x.EventSerial == Eventserial).ToList();
                    var existingData = existingDataList.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);
                    if (existingData == null)
                    {
                        var newItem = new ProdCost_QAEvents();
                        newItem.CompNo = company.comp_num;
                        newItem.MakerValue = eventData.MakerValue;
                        existingData.MakerReadValue = eventData.MakerReadValue;
                        newItem.MUser = me.UserID;
                        newItem.MainReqNo = MainReqNo;
                        newItem.Proc_SubNo = eventData.Proc_SubNo;
                        newItem.EventSerial = Eventserial;
                        newItem.Proc_MainNo = Proc_MainNo;
                        db.ProdCost_QAEvents.Add(newItem);
                    }
                    if (existingData.MakerValue == null)
                    {
                        existingData.CompNo = company.comp_num;
                        existingData.MakerValue = eventData.MakerValue;
                        existingData.MakerReadValue = eventData.MakerReadValue;
                        existingData.MUser = me.UserID;
                        existingData.MainReqNo = MainReqNo;
                        existingData.Proc_SubNo = eventData.Proc_SubNo;
                        existingData.EventSerial = Eventserial;
                        existingData.Proc_MainNo = Proc_MainNo;
                        db.SaveChanges();
                    }
                    else if (existingData.CheckerValue == null)
                    {
                        existingData.CheckerValue = eventData.CheckerValue;
                        existingData.CheckerReadValue = eventData.CheckerReadValue;
                        existingData.CUser = me.UserID;

                        db.SaveChanges();
                    }
                    if (existingData.MakerValue != null)
                    {
                        existingData.MakerValue = eventData.MakerValue;
                        existingData.MakerReadValue = eventData.MakerReadValue;
                        db.SaveChanges();
                    }

                    if (existingData.CheckerValue != null)
                    {
                        existingData.CheckerValue = eventData.CheckerValue;
                        existingData.CheckerReadValue = eventData.CheckerReadValue;
                        db.SaveChanges();
                    }
                    else if (existingData.MakerValue != null || existingData.CheckerValue == null)
                    {
                        existingData.MakerValue = eventData.MakerValue;
                        existingData.MakerReadValue = eventData.MakerReadValue;
                        db.SaveChanges();

                    }
                    else if (existingData.CheckerValue != null && existingData.MakerValue != null)
                    {
                        existingData.CheckerValue = eventData.CheckerValue;
                        existingData.CheckerReadValue = eventData.CheckerReadValue;
                        db.SaveChanges();

                    }

                }
            }
            return Json(new { success = true, message = "Event data saved successfully." });


        }
        [HttpPost]
        public ActionResult editFinaldata(List<ProdCost_QAEvents> eventDataList, Int64 MainReqNo, short Eventserial, short Proc_MainNo)
        {
            var existingDataList = db.ProdCost_QAEvents.Where(x => x.MainReqNo == MainReqNo).ToList();
            if (existingDataList.All(item => item.MPostStat != null && item.CPostStat != null))
            {
                // Update the FinalPostUser and FPostDate for each record in the eventDataList
                foreach (var eventData in eventDataList)
                {
                    var existingData = existingDataList.FirstOrDefault(e => e.Proc_SubNo == eventData.Proc_SubNo);
                    if (existingData != null)
                    {
                        existingData.FinalPostUser = me.UserID;
                        existingData.FPostDate = DateTime.Now;
                    }
                }

                // Save the changes to the database
                db.SaveChanges();

            }
            else
            {
                return Json(new { success = false, message = "MPostStat or CPostDate is null in the eventDataList." });
            }
            return Json(new { success = true, message = "Event data saved successfully." });

        }
    }
}