using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ProductionStagesController : controller
    {
        // GET: ProductionStages
        public ActionResult Index()
        {
            short CompNo = company.comp_num;
            short LstOption = 5;
            List<Prod_prodstage_infoView> lprodstage = db.Database.SqlQuery<Prod_prodstage_infoView>("exec ProdCost_WebLists {0},{1} ", CompNo, LstOption).ToList();
            return View(lprodstage);
        }
        public ActionResult ProdStagesList()
        {
            //List<prod_prodstage_info> lprodstage = db.prod_prodstage_info.Where(x => x.comp_no == company.comp_num).ToList();
            short CompNo = company.comp_num;
            short LstOption = 5;
            List<Prod_prodstage_infoView> lprodstage = db.Database.SqlQuery<Prod_prodstage_infoView>("exec ProdCost_WebLists {0},{1} ", CompNo, LstOption).ToList();

            return PartialView(lprodstage);
        }
        public ActionResult eProdStages(short CompNo,int StageCode)
        {
            prod_prodstage_info prodstage = db.prod_prodstage_info.Where(x => x.comp_no == CompNo && x.stage_code == StageCode).FirstOrDefault();
            return PartialView(prodstage);
        }
        public ActionResult FixAssest()
        {
            return PartialView();
        }
        public ActionResult SerialFixAssest(int FixAssestNo)
        {
            return PartialView(FixAssestNo);
        }
        public JsonResult Save_ProdStages(prod_prodstage_info stageinfo,List<ProdCost_MachineInfo> MachineInfo)
        {
            prod_prodstage_info IsExists = db.prod_prodstage_info.Where(x => x.comp_no == stageinfo.comp_no && x.stage_code == stageinfo.stage_code).FirstOrDefault();

            if (IsExists != null)
            {
                return Json(new { error = Resources.Resource.errorstagecode }, JsonRequestBehavior.AllowGet);
            }

            if (MachineInfo != null)
            {
                if (MachineInfo.Count != 0)
                {
                    List<int> l = new List<int>();
                    int i = 0;
                    foreach (ProdCost_MachineInfo item in MachineInfo)
                    {
                        if (i != 0)
                        {
                            int x = item.machine_code;
                            if (l.Contains(x))
                            {
                                return Json(new { error = Resources.Resource.errormachinecode }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        l.Add(item.machine_code);
                        i++;
                    }
                }
            }

            db.prod_prodstage_info.Add(stageinfo);
            db.SaveChanges();

            if(MachineInfo != null)
            {
                if(MachineInfo.Count != 0)
                {
                    db.ProdCost_MachineInfo.AddRange(MachineInfo);
                    db.SaveChanges();
                }
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_ProdStages(prod_prodstage_info stageinfo, List<ProdCost_MachineInfo> MachineInfo)
        {
            prod_prodstage_info ex = db.prod_prodstage_info.Where(x => x.comp_no == stageinfo.comp_no && x.stage_code == stageinfo.stage_code).FirstOrDefault();

            ex.stage_desc = stageinfo.stage_desc;
            ex.GroupID = stageinfo.GroupID;
            ex.no_yearly_daily_work = stageinfo.no_yearly_daily_work;
            ex.notes = stageinfo.notes;
            ex.Hr = stageinfo.Hr;
            ex.SetupTime = stageinfo.SetupTime;
            ex.SetupTimePrc = stageinfo.SetupTimePrc;
            ex.StopTimePrc = stageinfo.StopTimePrc;
            ex.QualityControl = stageinfo.QualityControl;

            if (MachineInfo != null)
            {
                if (MachineInfo.Count != 0)
                {
                    List<int> l = new List<int>();
                    int i = 0;
                    foreach (ProdCost_MachineInfo item in MachineInfo)
                    {
                        if (i != 0)
                        {
                            int x = item.machine_code;
                            if (l.Contains(x))
                            {
                                return Json(new { error = Resources.Resource.errormachinecode }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        l.Add(item.machine_code);
                        i++;
                    }

                    List<ProdCost_MachineInfo> ex1 = db.ProdCost_MachineInfo.Where(x => x.CompNo == stageinfo.comp_no && x.stage_code == stageinfo.stage_code).ToList();
                    if(ex1.Count != 0)
                    {
                        db.ProdCost_MachineInfo.RemoveRange(ex1);
                        db.SaveChanges();
                    }
                    db.ProdCost_MachineInfo.AddRange(MachineInfo);
                }
            }

            db.SaveChanges();


            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Del_ProdStages(short CompNo, int StageCode)
        {
            int ContSatge = db.Prod_ItemStages.Where(x => x.Comp_no == CompNo && x.Stage_code == StageCode).Count();
            if(ContSatge != 0)
            {
                return Json(new { error = Resources.Resource.CeckDeleteCompany }, JsonRequestBehavior.AllowGet);
            }

            List<ProdCost_MachineInfo> ex1 = db.ProdCost_MachineInfo.Where(x => x.CompNo == CompNo && x.stage_code == StageCode).ToList();
            if (ex1.Count != 0)
            {
                db.ProdCost_MachineInfo.RemoveRange(ex1);
                db.SaveChanges();
            }

            prod_prodstage_info ex = db.prod_prodstage_info.Where(x => x.comp_no == CompNo && x.stage_code == StageCode).FirstOrDefault();
            db.prod_prodstage_info.Remove(ex);
            db.SaveChanges();
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);

        }
    }
}