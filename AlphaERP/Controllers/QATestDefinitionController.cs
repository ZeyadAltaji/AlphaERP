using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using AlphaERP.Models;

namespace AlphaERP.Controllers
{
    public class QATestDefinitionController : controller
    {
        public ActionResult Index()
        {
            short CompNo = company.comp_num;
            short LstOption = 6;
            List<PordCost_FormTestView> listFormTest = db.Database.SqlQuery<PordCost_FormTestView>("exec ProdCost_WebLists {0},{1} ", CompNo, LstOption).ToList();
            return View(listFormTest);
        }
        public ActionResult List()
        {

            short CompNo = company.comp_num;
            short LstOption = 6;
            List<PordCost_FormTestView> listFormTest = db.Database.SqlQuery<PordCost_FormTestView>("exec ProdCost_WebLists {0},{1} ", CompNo, LstOption).ToList();
            return View(listFormTest);
        }
        public ActionResult LoadAddNewQC()
        {
            return PartialView();
        }


        public JsonResult Action(List<ProdCost_FormTest> Forms, int FormNo, List<HttpPostedFileBase> files)
        {
            if (Forms == null)
            {
                return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
            }
            if (Forms.Count == 0)
            {
                return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
            }
            foreach (var frm in Forms)
            {
                frm.FormNo = FormNo;
                frm.CompNo = company.comp_num;
            }

            if (FormNo == 0)
            {
                // Add new
                int LFormNo = 1;
                ProdCost_FormTest l = db.ProdCost_FormTest.OrderByDescending(x => x.FormNo).FirstOrDefault();
                if (l != null)
                {
                    LFormNo += l.FormNo;
                }
                if (Forms != null)
                {
                    foreach (var frm in Forms)
                    {
                        frm.FormNo = LFormNo;
                    }
                }
                db.ProdCost_FormTest.AddRange(Forms);
                UploadFiles(LFormNo, files);
                db.SaveChanges();
            }
            else
            {
                // edit
                List<ProdCost_FormTest> ExForms = db.ProdCost_FormTest.Where(x => x.CompNo == company.comp_num && x.FormNo == FormNo).ToList();
                if (ExForms != null)
                {
                    db.ProdCost_FormTest.RemoveRange(ExForms);
                    db.SaveChanges();
                }
                db.ProdCost_FormTest.AddRange(Forms);
                UploadFiles(FormNo, files);
                db.SaveChanges();
            }
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditQCForm(ProdCost_FormTest frm)
        {
            var prodCostFormTest = db.ProdCost_FormTest
                .FirstOrDefault(x => x.CompNo == company.comp_num && x.FormNo == frm.FormNo);

            var list = db.ProdCost_FormTest
                .Where(x => x.CompNo == company.comp_num && x.FormNo == frm.FormNo)
                .ToList();

            string fileData = MyFiles(frm.FormNo);

            ViewBag.files = fileData;
            ViewBag.f = prodCostFormTest;

            return PartialView(list);
        }

        public JsonResult GetForm(ProdCost_FormTest frm)
        {
            List<ProdCost_FormTest> f = db.ProdCost_FormTest.Where(x => x.CompNo == company.comp_num && x.FormNo == frm.FormNo).ToList();
            List<ProdCost_Parameter> l = new List<ProdCost_Parameter>();
            foreach (var item in f)
            {
                ProdCost_Parameter tmp = db.ProdCost_Parameters.Where(x => x.CompNo == company.comp_num && x.ParmCode == item.TestNo && x.ParmID == 5).FirstOrDefault();
                if (tmp == null)
                {
                    l.Add(new ProdCost_Parameter());
                }
                else
                {
                    l.Add(tmp);
                }
            }
            string files = MyFiles(frm.FormNo);

            return Json(new { f, l, files }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteForm(ProdCost_FormTest frm)
        {
            List<ProdCost_FormTest> f = db.ProdCost_FormTest.Where(x => x.CompNo == company.comp_num && x.FormNo == frm.FormNo).ToList();
            if (f != null)
            {
                db.ProdCost_FormTest.RemoveRange(f);
                db.SaveChanges();
                string fullPath = Request.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + frm.FormNo);
                if (Directory.Exists(fullPath))
                {
                    DeleteFiles(frm.FormNo);
                }
            }
            return Json(new { ok = "" }, JsonRequestBehavior.AllowGet);
        }

        private void UploadFiles(int FormNo, List<HttpPostedFileBase> files)
        {

            if (files != null)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file.ContentLength > 1000000)
                    {

                    }
                    else
                    {
                        string path = Server.MapPath("~/Uploads");
                        if (!Directory.Exists(path))
                        { Directory.CreateDirectory(path); }

                        path = Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num);
                        if (!Directory.Exists(path))
                        { Directory.CreateDirectory(path); }

                        path = Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + FormNo);
                        if (!Directory.Exists(path))
                        { Directory.CreateDirectory(path); }

                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string type = Path.GetExtension(fileName);
                            if (AcceptedExtentions.Contains(type.ToLower()))
                            {
                                fileName = Path.GetFileNameWithoutExtension(fileName);
                                string GfileName = Guid.NewGuid().ToString() + type;
                                Stream fileContent = file.InputStream;
                                string path2 = Path.Combine(Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + FormNo + "/"), fileName + type);
                                string ext = type.ToLower().Replace(".", "");
                                file.SaveAs(path2);
                            }

                        }

                    }
                }
            }
        }

        private void DeleteFiles(int FormNo)
        {
            string fullPath = Request.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + FormNo);
            var dir = new DirectoryInfo(fullPath);
            dir.Delete(true);
        }

        private string MyFiles(int FormNo)
        {
            string f = "";
            string path = Server.MapPath("~/Uploads");
            if (!Directory.Exists(path))
            { Directory.CreateDirectory(path); }

            path = Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num);
            if (!Directory.Exists(path))
            { Directory.CreateDirectory(path); }

            path = Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + FormNo);
            if (!Directory.Exists(path))
            { Directory.CreateDirectory(path); }
            IEnumerable<string> files = Directory.EnumerateFiles(Server.MapPath("~/Uploads/TestDefinition/" + company.comp_num + "/" + FormNo + "/"));
            foreach (var fullPath in files)
            {
                var fileName = Path.GetFileName(fullPath);
                var hrf = "../../Uploads/TestDefinition/" + company.comp_num + "/" + FormNo + "/" + fileName;
                f += "<a target=\"_blank\" href=\"" + hrf + "\" >" + fileName + ",</a>";

            }
            return f;
        }


    }
}