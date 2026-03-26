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
    public class PackingDefentionController : controller
    {
        // GET: PackingDefention
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PackingDefentionList()
        {
            return PartialView();
        }
        public ActionResult PackDef_FormulaHead(short CompNo,string formula_code) 
        {
            prod_formula_header_info PackingDef = db.prod_formula_header_info.Where(
                x => x.comp_no == CompNo && x.formula_code == formula_code).FirstOrDefault();
            return PartialView(PackingDef);
        }
        public ActionResult InqPackDef_FormulaHead(short CompNo, string formula_code)
        {
            prod_formula_header_info PackingDef = db.prod_formula_header_info.Where(
                x => x.comp_no == CompNo && x.formula_code == formula_code).FirstOrDefault();
            return PartialView(PackingDef);
        }
        public ActionResult PackingDefItem(List<InvItemsMF> items)
        {
            return PartialView(items);
        }
        public JsonResult Save_PackingDefention(List<BOM_PakingInfo> PackDefD)
        {
            DataTable TmpDb = new DataTable();
            TmpDb.Columns.Add("FinItem", typeof(string));
            TmpDb.Columns.Add("PackItem", typeof(string));
            TmpDb.Columns.Add("Capacity", typeof(double));
            TmpDb.Columns.Add("IsLiter", typeof(bool));
            SqlDataAdapter TmpDa = new SqlDataAdapter();

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "Bom_DeletePackingInfo";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmddel.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = PackDefD.FirstOrDefault().FormCode;
                    cmddel.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 2;
                    cmddel.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }


                foreach (BOM_PakingInfo item in PackDefD)
                {
                    DataRow DrTmp = TmpDb.NewRow();
                    DrTmp.BeginEdit();
                    DrTmp["FinItem"] = item.FinItem;
                    DrTmp["PackItem"] = item.PackItem;
                    DrTmp["Capacity"] = item.Capacity;
                    DrTmp["IsLiter"] = item.IsLiter;
                    DrTmp.EndEdit();
                    TmpDb.Rows.Add(DrTmp);
                }

                using (SqlCommand CmdIns = new SqlCommand())
                {
                    CmdIns.Connection = cn;
                    CmdIns.CommandText = "Bom_AddBomPackInfo";
                    CmdIns.CommandType = CommandType.StoredProcedure;
                    CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdIns.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = PackDefD.FirstOrDefault().FormCode; ;
                    CmdIns.Parameters.Add(new SqlParameter("@FinItem", SqlDbType.VarChar, 20, "FinItem"));
                    CmdIns.Parameters.Add(new SqlParameter("@PackItem", SqlDbType.VarChar, 20, "PackItem"));
                    CmdIns.Parameters.Add(new SqlParameter("@Capacity", SqlDbType.Money, 9, "Capacity"));
                    CmdIns.Parameters.Add(new SqlParameter("@IsLiter", SqlDbType.Bit, 1, "IsLiter"));
                    CmdIns.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 2;
                    CmdIns.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    CmdIns.Transaction = transaction;
                    TmpDa.InsertCommand = CmdIns;
                    TmpDa.Update(TmpDb);
                }
                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemPackAccessories(List<InvItemsMF> SubDetails)
        {
            return PartialView(SubDetails);
        }
        public ActionResult PackingAccessories(BOM_PakingInfo SubDetails)
        {
            return PartialView(SubDetails);
        }
        public JsonResult Save_PackingAccessors(List<BOM_FinPackingInfo> PackAcc)
        {
            BOM_FinPackingInfo packingInfo = PackAcc.FirstOrDefault();
            List<BOM_FinPackingInfo> ex = db.BOM_FinPackingInfo.Where(x => x.CompNo == packingInfo.CompNo
             && x.FormCode == packingInfo.FormCode && x.PackItem == packingInfo.PackItem && x.FinItem == packingInfo.FinItem).ToList();
            if(ex.Count != 0)
            {
                db.BOM_FinPackingInfo.RemoveRange(ex);
                db.SaveChanges();
            }

            DataSet TmpDs = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            DataRow DrTmp;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select BOM_FinPackingInfo.* from BOM_FinPackingInfo where CompNo=0", cn);
                DaPR.Fill(TmpDs, "tmp");

                foreach (BOM_FinPackingInfo item in PackAcc)
                {
                    DrTmp = TmpDs.Tables["tmp"].NewRow();
                    DrTmp.BeginEdit();
                    DrTmp["compno"] = item.CompNo;
                    DrTmp["FormCode"] = item.FormCode;
                    DrTmp["PackItem"] = item.PackItem;
                    DrTmp["FinItem"] = item.FinItem;
                    DrTmp["RMCode"] = item.RMCode;
                    DrTmp["UnitSerial"] = item.UnitSerial;
                    DrTmp["Qty"] = item.Qty;
                    DrTmp.EndEdit();
                    TmpDs.Tables["tmp"].Rows.Add(DrTmp);
                }

                using (SqlCommand CmdIns = new SqlCommand())
                {
                    CmdIns.Connection = cn;
                    CmdIns.CommandText = "Bom_AddFinPackingInfo";
                    CmdIns.CommandType = CommandType.StoredProcedure;
                    CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Compno"));
                    CmdIns.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "FormCode"));
                    CmdIns.Parameters.Add(new SqlParameter("@PackItem", SqlDbType.VarChar, 20, "PackItem"));
                    CmdIns.Parameters.Add(new SqlParameter("@FinItem", SqlDbType.VarChar, 20, "FinItem"));
                    CmdIns.Parameters.Add(new SqlParameter("@RMCode", SqlDbType.VarChar, 20, "RMCode"));
                    CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                    CmdIns.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 9, "Qty"));
                    transaction = cn.BeginTransaction();
                    CmdIns.Transaction = transaction;
                    DaPR.InsertCommand = CmdIns;
                    DaPR.Update(TmpDs, "tmp");
                }

                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataPackingInfo(BOM_FinPackingInfo itemPack)
        {
            List<BOM_FinPackingInfo> ex = db.BOM_FinPackingInfo.Where(
                x => x.CompNo == company.comp_num && x.FormCode == itemPack.FormCode
                && x.PackItem == itemPack.PackItem && x.FinItem == itemPack.FinItem).ToList();

            if (ex.Count != 0)
            {
                return Json(new { d = "NotDelete" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { d = "Delete" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RemoveDataPackingInfo(BOM_FinPackingInfo itemPack)
        {
            List<BOM_FinPackingInfo> ex = db.BOM_FinPackingInfo.Where(
                x => x.CompNo == company.comp_num && x.FormCode == itemPack.FormCode
                && x.PackItem == itemPack.PackItem && x.FinItem == itemPack.FinItem).ToList();

            BOM_PakingInfo ex1 = db.BOM_PakingInfo.Where(x => x.CompNo == company.comp_num
            && x.FormCode == itemPack.FormCode && x.FinItem == itemPack.FinItem
            && x.PackItem == itemPack.PackItem).FirstOrDefault();

            if (ex.Count != 0)
            {
                db.BOM_FinPackingInfo.RemoveRange(ex);
                db.SaveChanges();
                db.BOM_PakingInfo.Remove(ex1);
                db.SaveChanges();
            }
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }
    }
}