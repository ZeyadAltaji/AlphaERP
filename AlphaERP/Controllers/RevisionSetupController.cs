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
    public class RevisionSetupController : controller
    {
        // GET: RevisionSetup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RevisionSetuplist(string ItemNo)
        {
            List<Prod_RevisionSetup> Prod_RevisionSetup = db.Database.SqlQuery<Prod_RevisionSetup>("ProdCost_LoadRevisionSetup  {0},{1} ", company.comp_num, ItemNo).ToList();
            return View(Prod_RevisionSetup);
        }
        public ActionResult ItemsList()
        {
            List<InvItemsMF> litem = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num).ToList();
            return PartialView(litem);
        }
        public JsonResult SaveRevisionSetup(List<Prod_RevisionSetup> RevisionSetup)
        {
            Prod_RevisionSetup obj = RevisionSetup.FirstOrDefault();
            List<Prod_RevisionSetup> deldata = db.Prod_RevisionSetup.Where(x => x.CompNo == obj.CompNo && x.ItemNo == obj.ItemNo).ToList();
            if (deldata.Count > 0)
            {
                db.Prod_RevisionSetup.RemoveRange(deldata);
            }
            
            if (RevisionSetup != null)
            {
                if (RevisionSetup.Count != 0)
                {
                    db.Prod_RevisionSetup.AddRange(RevisionSetup);
                    db.SaveChanges();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}