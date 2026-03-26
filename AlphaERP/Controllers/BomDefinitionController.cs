using AlphaERP.Models;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class BomDefinitionController : controller
    {
        // GET: BomDefinition
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ItemRawMaterial(List<InvItemsMF> items)
        {
            return PartialView(items);
        }
        public ActionResult ProdstageInfo(List<prod_prodstage_info> stage)
        {
            return PartialView(stage);
        }
        public ActionResult ItemStageTestQC(Prod_ItemStages SubDetails)
        {
            return PartialView(SubDetails);
        }
        public ActionResult GetQASetupHFBystages(short Comp_no, string Formula_code, int Stage_code, short StageSer)
        {
            var filteredData = db.ProdCost_QASetupHF.Where(x => x.CompNo == Comp_no).ToList();

            ViewBag.Comp_no = Comp_no;
            ViewBag.Formula_code = Formula_code;
            ViewBag.Stage_code = Stage_code;
            ViewBag.StageSer = StageSer;

            return PartialView(filteredData);
        }
        public ActionResult GetFormTest(short Comp_no, string Formula_code, int Stage_code)
        {
            var filteredData = db.ProdCost_QASetupHF.Where(x => x.CompNo == Comp_no).ToList();

            ViewBag.Comp_no = Comp_no;
            ViewBag.Formula_code = Formula_code;
            ViewBag.Stage_code = Stage_code;

            return PartialView(filteredData);
        }
        public ActionResult GetQASetupHFBystagesadd(short Comp_no, int Stage_code)
        {
            var filteredData = db.ProdCost_QASetupHF.Where(x => x.CompNo == Comp_no).ToList();

            ViewBag.Comp_no = Comp_no;
            ViewBag.Stage_code = Stage_code;

            return PartialView(filteredData);
        }
        public ActionResult ItemByProduct(List<InvItemsMF> items)
        {
            return PartialView(items);
        }
        public ActionResult AdditiveItems(List<InvItemsMF> items)
        {
            return PartialView(items);
        }
        public ActionResult BomDefinitionList()
        {
            return PartialView();
        }
        public ActionResult eBOMInfo(short CompNo, string formulaCode)
        {
            prod_formula_header_info BomDef = db.prod_formula_header_info.Where(x => x.comp_no == CompNo && x.formula_code == formulaCode).FirstOrDefault();
            return PartialView(
               string.Format("~/Views/BomDefinition/tabs/eBOMInfo.cshtml"), BomDef
                );
        }
        public ActionResult GetItemsData ()
        {
            return PartialView();
        }
        public ActionResult eBOMByProducts(short CompNo, string formulaCode)
        {
           List<prod_extend_item> extendItem = db.prod_extend_item.Where(x => x.comp_no == CompNo && x.formula_code == formulaCode).ToList();
            return PartialView(
               string.Format("~/Views/BomDefinition/tabs/eBOMByProducts.cshtml"), extendItem
                );
        }
        public ActionResult eBOMAddItems(short CompNo, string formulaCode)
        {
            List<Prod_BomAdditionalRM> AdditionalRM = db.Prod_BomAdditionalRM.Where(x => x.CompNo == CompNo && x.BOMCode == formulaCode).ToList();
            return PartialView(
               string.Format("~/Views/BomDefinition/tabs/eBOMAddItems.cshtml"), AdditionalRM
                );
        }
        public ActionResult eBOMSpecification(short CompNo, string formulaCode)
        {
            List<ProdCost_FormulaProperties> Properties = db.ProdCost_FormulaProperties.Where(x => x.CompNo == CompNo && x.FormulaCode == formulaCode).ToList();
            return PartialView(
               string.Format("~/Views/BomDefinition/tabs/eBOMSpecification.cshtml"), Properties
                );
        }
        public ActionResult eBOMProdStages(short CompNo, string formulaCode)
        {
            List<Prod_ItemStages> ItemStages = db.Prod_ItemStages.Where(x => x.Comp_no == CompNo && x.Formula_code == formulaCode).ToList();
            return PartialView(
               string.Format("~/Views/BomDefinition/tabs/eBOMProdStages.cshtml"), ItemStages
                );
        }
        public JsonResult Save_BomDefinition(prod_formula_header_info BomDefH,List<prod_formula_detail_info> BomDefD,List<prod_extend_item> BomDefProduct,List<Prod_BomAdditionalRM> BomDefAdditiveItems, List<Prod_ItemStages> Prod_ItemStages, List<ProdCost_FormulaProperties> BomDefProperty)
        {
            prod_formula_header_info IsExists = db.prod_formula_header_info.Where(x => x.comp_no == BomDefH.comp_no && x.formula_code == BomDefH.formula_code).FirstOrDefault();
            if(IsExists != null)
            {
                return Json(new { error = Resources.Resource.errorFormulaCode }, JsonRequestBehavior.AllowGet);
            }
            int i = 1;
            DataSet TmpDs = new DataSet();
            DataSet TmpDs1 = new DataSet();
            DataSet TmpDs2 = new DataSet();
            DataSet TmpDs3 = new DataSet();
            DataSet TmpDs4 = new DataSet();
            DataSet TmpDs5 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            SqlDataAdapter DaPR3 = new SqlDataAdapter();
            SqlDataAdapter DaPR4 = new SqlDataAdapter();
            SqlDataAdapter DaPR5 = new SqlDataAdapter();
            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp2;
            DataRow DrTmp3;
            DataRow DrTmp4;
            DataRow DrTmp5;
            decimal PerCost = 100;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select Prod_formula_Detail_info.* from Prod_formula_Detail_info where comp_no=0", cn);
                DaPR.Fill(TmpDs, "tmp");

                DaPR1.SelectCommand = new SqlCommand("select Prod_extend_item.* from Prod_extend_item where comp_no=0", cn);
                DaPR1.Fill(TmpDs1, "tmp1");

                DaPR2.SelectCommand = new SqlCommand("select Prod_BomAdditionalRM.* from Prod_BomAdditionalRM where CompNo=0", cn);
                DaPR2.Fill(TmpDs2, "tmp2");

                DaPR3.SelectCommand = new SqlCommand("select ProdCost_FormulaProperties.* from ProdCost_FormulaProperties where CompNo=0", cn);
                DaPR3.Fill(TmpDs3, "tmp3");

                DaPR4.SelectCommand = new SqlCommand("select Prod_itemstages.* from Prod_itemstages where comp_no=0", cn);
                DaPR4.Fill(TmpDs4, "tmp4");

                DaPR5.SelectCommand = new SqlCommand("select ProditemsCostPer.* from ProditemsCostPer where CompNo=0", cn);
                DaPR5.Fill(TmpDs5, "tmp5");

                using (SqlCommand CmdHead = new SqlCommand())
                {
                    CmdHead.Connection = cn;
                    CmdHead.CommandText = "Bom_AddFormHead";
                    CmdHead.CommandType = CommandType.StoredProcedure;
                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = BomDefH.comp_no;
                    CmdHead.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = BomDefH.formula_code;
                    CmdHead.Parameters.Add(new SqlParameter("@FormDesc", SqlDbType.VarChar, 100)).Value = BomDefH.formula_desc;
                    CmdHead.Parameters.Add(new SqlParameter("@FormLevel", SqlDbType.SmallInt, 2)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@FormDate", SqlDbType.SmallDateTime)).Value = BomDefH.formula_date;
                    CmdHead.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = BomDefH.item_no;
                    CmdHead.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.Float, 9)).Value = BomDefH.UnitSerial;
                    CmdHead.Parameters.Add(new SqlParameter("@BestQty", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.best_qty);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty1", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty1);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty2);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty3", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty3);
                    if(BomDefH.BomNotes == null)
                    {
                        BomDefH.BomNotes = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@BomNotes", SqlDbType.VarChar, 8000)).Value = BomDefH.BomNotes;
                    CmdHead.Parameters.Add(new SqlParameter("@Visc", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Spec_Gr", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Grind", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Covering", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Gloss", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Scratch", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@F_Ball", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@BEnding", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@SolVan", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@EVAPort", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@BLush", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Colo", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@PH", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Soli", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Spr", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Draging", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Thickness", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@StgWidth", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Fona", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@StgQty", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.SmallInt, 2)).Value = BomDefH.GroupId;
                    CmdHead.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.SmallDateTime)).Value = BomDefH.ExpiryDate;
                    CmdHead.Parameters.Add(new SqlParameter("@StandardCost", SqlDbType.Bit)).Value = BomDefH.StandardCost;
                    CmdHead.Parameters.Add(new SqlParameter("@susflag", SqlDbType.Bit)).Value = BomDefH.SusFlag;
                    CmdHead.Parameters.Add(new SqlParameter("@CalculatedSG", SqlDbType.Money)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    transaction = cn.BeginTransaction();
                    CmdHead.Transaction = transaction;
                    try
                    {
                        CmdHead.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    long LogID = Convert.ToInt64(CmdHead.Parameters["@LogID"].Value);
                   

                    foreach (prod_formula_detail_info item in BomDefD)
                    {
                        DrTmp = TmpDs.Tables["tmp"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["Comp_no"] = item.comp_no;
                        DrTmp["formula_code"] = item.formula_code;
                        DrTmp["item_no"] = item.item_no;
                        DrTmp["best_qty"] = item.best_qty;
                        DrTmp["lowest_qty"] = item.lowest_qty;
                        DrTmp["rawmaterial_no"] = item.rawmaterial_no;
                        DrTmp["LossPerc"] = item.LossPerc;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp.EndEdit();
                        TmpDs.Tables["tmp"].Rows.Add(DrTmp);
                        i = i + 1;
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "Bom_AddFrOmDet";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                        CmdIns.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@RMCode", SqlDbType.VarChar, 20, "rawmaterial_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@BestQty", SqlDbType.Float, 9, "best_qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@low_Qty", SqlDbType.Float, 9, "lowest_qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@LossPerc", SqlDbType.Money, 8, "LossPerc"));
                        CmdIns.Parameters.Add(new SqlParameter("@stage_no", SqlDbType.SmallInt, 4, "stage_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "tmp");
                    }

                    if (BomDefProduct != null)
                    {
                        if(BomDefProduct.Count != 0)
                        {
                            i = 1;
                            foreach (prod_extend_item item in BomDefProduct)
                            {
                                DrTmp1 = TmpDs1.Tables["tmp1"].NewRow();
                                DrTmp1.BeginEdit();
                                DrTmp1["Comp_no"] = item.comp_no;
                                DrTmp1["formula_code"] = item.formula_code;
                                DrTmp1["item_no"] = item.item_no;
                                DrTmp1["ItemDesc"] = item.itemdesc;
                                DrTmp1["qty1"] = item.Qty1;
                                DrTmp1["qty2"] = item.Qty2;
                                DrTmp1["qty3"] = item.Qty3;
                                DrTmp1["percost"] = item.percost;
                                DrTmp1["UnitSerial"] = item.UnitSerial;
                                DrTmp1.EndEdit();
                                TmpDs1.Tables["tmp1"].Rows.Add(DrTmp1);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns2 = new SqlCommand())
                            {
                                CmdIns2.Connection = cn;
                                CmdIns2.CommandText = "Bom_AddExtendItem";
                                CmdIns2.CommandType = CommandType.StoredProcedure;
                                CmdIns2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                                CmdIns2.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                                CmdIns2.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                                CmdIns2.Parameters.Add(new SqlParameter("@ItemDesc", SqlDbType.VarChar, 60, "ItemDesc"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty1", SqlDbType.Float, 9, "Qty1"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty3", SqlDbType.Float, 9, "qty3"));
                                CmdIns2.Parameters.Add(new SqlParameter("@percost", SqlDbType.Float, 9, "percost"));
                                CmdIns2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                                CmdIns2.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns2.Transaction = transaction;
                                DaPR1.InsertCommand = CmdIns2;
                                DaPR1.Update(TmpDs1, "tmp1");
                            }
                            PerCost = 100;
                            foreach (prod_extend_item item in BomDefProduct)
                            {
                                DrTmp5 = TmpDs5.Tables["tmp5"].NewRow();
                                DrTmp5.BeginEdit();
                                DrTmp5["compno"] = item.comp_no;
                                DrTmp5["FormulaCode"] = item.formula_code;
                                DrTmp5["ItemNo"] = item.item_no;
                                DrTmp5["ItemDesc"] = item.itemdesc;
                                DrTmp5["ProductPer"] = item.percost;
                                DrTmp5["C1Per"] = 100;
                                PerCost = PerCost - Convert.ToDecimal(DrTmp5["ProductPer"] ?? "0");
                                DrTmp5.EndEdit();
                                TmpDs5.Tables["tmp5"].Rows.Add(DrTmp5);
                            }

                            DrTmp5 = TmpDs5.Tables["tmp5"].NewRow();
                            DrTmp5.BeginEdit();
                            DrTmp5["compno"] = company.comp_num;
                            DrTmp5["FormulaCode"] = BomDefH.formula_code;
                            DrTmp5["ItemNo"] = BomDefH.item_no;
                            DrTmp5["ItemDesc"] = db.InvItemsMFs.Where(x => x.CompNo == BomDefH.comp_no && x.ItemNo == BomDefH.item_no).FirstOrDefault().ItemDesc;
                            DrTmp5["ProductPer"] = PerCost;
                            DrTmp5["C1Per"] = 100;
                            DrTmp5.EndEdit();
                            TmpDs5.Tables["tmp5"].Rows.Add(DrTmp5);

                            using (SqlCommand CmdIns6 = new SqlCommand())
                            {
                                CmdIns6.Connection = cn;
                                CmdIns6.CommandText = "Bom_AddProdItemsCostPer";
                                CmdIns6.CommandType = CommandType.StoredProcedure;
                                CmdIns6.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Compno"));
                                CmdIns6.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formulacode"));
                                CmdIns6.Parameters.Add(new SqlParameter("@ItemCode", SqlDbType.VarChar, 20, "ItemNo"));
                                CmdIns6.Parameters.Add(new SqlParameter("@itemDesc", SqlDbType.VarChar, 50, "ItemDesc"));
                                CmdIns6.Parameters.Add(new SqlParameter("@prodPer", SqlDbType.Float, 9, "ProductPer"));
                                CmdIns6.Parameters.Add(new SqlParameter("@C1Per", SqlDbType.Float, 9, "C1Per"));
                                CmdIns6.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                                CmdIns6.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns6.Parameters.Add(new SqlParameter("@EditLogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                                CmdIns6.Transaction = transaction;
                                DaPR5.InsertCommand = CmdIns6;
                                DaPR5.Update(TmpDs5, "tmp5");
                            }
                        }
                    }
                    if(BomDefAdditiveItems != null)
                    {
                        if (BomDefAdditiveItems.Count != 0)
                        {
                            i = 1;
                            foreach (Prod_BomAdditionalRM item in BomDefAdditiveItems)
                            {
                                DrTmp2 = TmpDs2.Tables["tmp2"].NewRow();
                                DrTmp2.BeginEdit();
                                DrTmp2["CompNo"] = item.CompNo;
                                DrTmp2["BomCode"] = item.BOMCode;
                                DrTmp2["RmCode"] = item.RMCode;
                                DrTmp2["UnitSerial"] = item.UnitSerial;
                                DrTmp2["Qty"] = item.QTY;
                                DrTmp2["Notes"] = item.Notes;
                                DrTmp2.EndEdit();
                                TmpDs2.Tables["tmp2"].Rows.Add(DrTmp2);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns3 = new SqlCommand())
                            {
                                CmdIns3.Connection = cn;
                                CmdIns3.CommandText = "Prodcost_AddBOMAdditionalItems";
                                CmdIns3.CommandType = CommandType.StoredProcedure;
                                CmdIns3.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                                CmdIns3.Parameters.Add(new SqlParameter("@bomCode", SqlDbType.VarChar, 20, "BOMCode"));
                                CmdIns3.Parameters.Add(new SqlParameter("@RmCode", SqlDbType.VarChar, 20, "RmCode"));
                                CmdIns3.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                                CmdIns3.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                                CmdIns3.Parameters.Add(new SqlParameter("@notes", SqlDbType.VarChar, 200, "notes"));
                                CmdIns3.Transaction = transaction;
                                DaPR2.InsertCommand = CmdIns3;
                                DaPR2.Update(TmpDs2, "tmp2");
                            }
                        }
                    }
                    if (Prod_ItemStages != null)
                    {
                        if (Prod_ItemStages.Count != 0)
                        {
                            i = 1;
                            foreach (Prod_ItemStages item in Prod_ItemStages)
                            {
                                DrTmp4 = TmpDs4.Tables["tmp4"].NewRow();
                                DrTmp4.BeginEdit();
                                DrTmp4["comp_no"] = item.Comp_no;
                                DrTmp4["formula_code"] = item.Formula_code;
                                DrTmp4["stage_code"] = item.Stage_code;
                                DrTmp4["StageSer"] = i;
                                DrTmp4["Serial1"] = i;
                                DrTmp4["hr"] = item.Hr;
                                DrTmp4["min"] = item.Min;
                                DrTmp4["Item_no"] = item.Item_no;
                                DrTmp4["Stage_no"] = Prod_ItemStages.Count;
                                DrTmp4["ReqQty"] = 0;
                                DrTmp4["Repeat"] = 0;
                                DrTmp4["UnitPcs"] = 0;
                                DrTmp4["StopTime"] = 0;
                                DrTmp4["FixedTime"] = item.FixedTime;
                                DrTmp4["TimePerUnit"] = (item.Hr + item.Min / 60) / BomDefH.best_qty;
                                DrTmp4["ClosePStage"] = item.ClosePStage;
                                if (item.QAProc != null)
                                {
                                    DrTmp4["QAProc"] = item.QAProc;
                                }

                                if (item.QCFromNo != null)
                                {
                                    DrTmp4["QCFromNo"] = item.QCFromNo;
                                }
                                DrTmp4.EndEdit();
                                TmpDs4.Tables["tmp4"].Rows.Add(DrTmp4);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns5 = new SqlCommand())
                            {
                                CmdIns5.Connection = cn;
                                CmdIns5.CommandText = "Bom_AddFormStages";
                                CmdIns5.CommandType = CommandType.StoredProcedure;
                                CmdIns5.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "Item_No"));
                                CmdIns5.Parameters.Add(new SqlParameter("@NoOfStages", SqlDbType.SmallInt, 4, "Stage_No"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5, "stage_code"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 4, "StageSer"));
                                CmdIns5.Parameters.Add(new SqlParameter("@HR", SqlDbType.Float,8, "hr"));
                                CmdIns5.Parameters.Add(new SqlParameter("@Min", SqlDbType.Float, 8, "min"));
                                CmdIns5.Parameters.Add(new SqlParameter("@ReqQty", SqlDbType.Float, 8, "ReqQty"));
                                CmdIns5.Parameters.Add(new SqlParameter("@TimePerUnit", SqlDbType.Float, 8, "TimePerUnit"));
                                CmdIns5.Parameters.Add(new SqlParameter("@Repeat", SqlDbType.SmallInt, 2, "Repeat"));
                                CmdIns5.Parameters.Add(new SqlParameter("@UnitPcs", SqlDbType.SmallInt, 2, "UnitPcs"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StopTime", SqlDbType.Money, 8, "StopTime"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FixedTime", SqlDbType.Bit, 1, "FixedTime"));
                                CmdIns5.Parameters.Add(new SqlParameter("@ClosePStage", SqlDbType.Bit, 1, "ClosePStage"));
                                CmdIns5.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns5.Parameters.Add(new SqlParameter("@Serial1", SqlDbType.SmallInt, 4, "Serial1"));
                                CmdIns5.Parameters.Add(new SqlParameter("@QAProc", SqlDbType.SmallInt, 4, "QAProc"));
                                CmdIns5.Parameters.Add(new SqlParameter("@QCFromNo", SqlDbType.SmallInt, 4, "QCFromNo"));

                                CmdIns5.Transaction = transaction;
                                DaPR4.InsertCommand = CmdIns5;
                                DaPR4.Update(TmpDs4, "tmp4");
                            }
                        }
                    }
                    if (BomDefProperty != null)
                    {
                        if (BomDefProperty.Count != 0)
                        {
                            i = 1;
                            foreach (ProdCost_FormulaProperties item in BomDefProperty)
                            {
                                DrTmp3 = TmpDs3.Tables["tmp3"].NewRow();
                                DrTmp3.BeginEdit();
                                DrTmp3["CompNo"] = item.CompNo;
                                DrTmp3["FormulaCode"] = item.FormulaCode;
                                DrTmp3["PropertID"] = i;
                                DrTmp3["PropertName"] = item.PropertName;
                                DrTmp3["PropertValue"] = item.PropertValue;
                                DrTmp3.EndEdit();
                                TmpDs3.Tables["tmp3"].Rows.Add(DrTmp3);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns4 = new SqlCommand())
                            {
                                CmdIns4.Connection = cn;
                                CmdIns4.CommandText = "ProdCost_AddBomProperties";
                                CmdIns4.CommandType = CommandType.StoredProcedure;
                                CmdIns4.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2, "CompNo"));
                                CmdIns4.Parameters.Add(new SqlParameter("@FormulaCode", SqlDbType.VarChar, 20, "FormulaCode"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertID", SqlDbType.SmallInt, 2, "PropertID"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertName", SqlDbType.VarChar, 100, "PropertName"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertValue", SqlDbType.VarChar, 200, "PropertValue"));
                                CmdIns4.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns4.Transaction = transaction;
                                DaPR3.InsertCommand = CmdIns4;
                                DaPR3.Update(TmpDs3, "tmp3");
                            }
                        }
                    }
                  
                }
                transaction.Commit();
                cn.Dispose();
            }
            //foreach (var itemIdentifier in Prod_ItemStages)
            //{
            //    var newData = new Prod_ItemStages
            //    {
            //        QAProc = itemIdentifier.QAProc 
            //    };
            //    db.Prod_ItemStages.Add(newData);
            //    db.SaveChanges();
            //}
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_BomDefinition(prod_formula_header_info BomDefH, List<prod_formula_detail_info> BomDefD, List<prod_extend_item> BomDefProduct, List<Prod_BomAdditionalRM> BomDefAdditiveItems, List<Prod_ItemStages> Prod_ItemStages, List<ProdCost_FormulaProperties> BomDefProperty)
        {
            int i = 1;
            DataSet TmpDs = new DataSet();
            DataSet TmpDs1 = new DataSet();
            DataSet TmpDs2 = new DataSet();
            DataSet TmpDs3 = new DataSet();
            DataSet TmpDs4 = new DataSet();
            DataSet TmpDs5 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            SqlDataAdapter DaPR3 = new SqlDataAdapter();
            SqlDataAdapter DaPR4 = new SqlDataAdapter();
            SqlDataAdapter DaPR5 = new SqlDataAdapter();
            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp2;
            DataRow DrTmp3;
            DataRow DrTmp4;
            DataRow DrTmp5;
            decimal PerCost = 100;

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select Prod_formula_Detail_info.* from Prod_formula_Detail_info where comp_no=0", cn);
                DaPR.Fill(TmpDs, "tmp");

                DaPR1.SelectCommand = new SqlCommand("select Prod_extend_item.* from Prod_extend_item where comp_no=0", cn);
                DaPR1.Fill(TmpDs1, "tmp1");

                DaPR2.SelectCommand = new SqlCommand("select Prod_BomAdditionalRM.* from Prod_BomAdditionalRM where CompNo=0", cn);
                DaPR2.Fill(TmpDs2, "tmp2");

                DaPR3.SelectCommand = new SqlCommand("select ProdCost_FormulaProperties.* from ProdCost_FormulaProperties where CompNo=0", cn);
                DaPR3.Fill(TmpDs3, "tmp3");

                DaPR4.SelectCommand = new SqlCommand("select Prod_itemstages.* from Prod_itemstages where comp_no=0", cn);
                DaPR4.Fill(TmpDs4, "tmp4");

                DaPR5.SelectCommand = new SqlCommand("select ProditemsCostPer.* from ProditemsCostPer where CompNo=0", cn);
                DaPR5.Fill(TmpDs5, "tmp5");


                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "bom_Deleteform";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = BomDefH.comp_no;
                    cmddel.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = BomDefH.formula_code;
                    cmddel.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    cmddel.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 2;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }

                
                using (SqlCommand CmdHead = new SqlCommand())
                {
                    CmdHead.Connection = cn;
                    CmdHead.CommandText = "Bom_AddFormHead";
                    CmdHead.CommandType = CommandType.StoredProcedure;
                    CmdHead.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4)).Value = BomDefH.comp_no;
                    CmdHead.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = BomDefH.formula_code;
                    CmdHead.Parameters.Add(new SqlParameter("@FormDesc", SqlDbType.VarChar, 100)).Value = BomDefH.formula_desc;
                    CmdHead.Parameters.Add(new SqlParameter("@FormLevel", SqlDbType.SmallInt, 2)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@FormDate", SqlDbType.SmallDateTime)).Value = BomDefH.formula_date;
                    CmdHead.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = BomDefH.item_no;
                    CmdHead.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.Float, 9)).Value = BomDefH.UnitSerial;
                    CmdHead.Parameters.Add(new SqlParameter("@BestQty", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.best_qty);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty1", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty1);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty2);
                    CmdHead.Parameters.Add(new SqlParameter("@Qty3", SqlDbType.Float, 9)).Value = System.Convert.ToDouble(BomDefH.Qty3);
                    if (BomDefH.BomNotes == null)
                    {
                        BomDefH.BomNotes = "";
                    }
                    CmdHead.Parameters.Add(new SqlParameter("@BomNotes", SqlDbType.VarChar, 8000)).Value = BomDefH.BomNotes;
                    CmdHead.Parameters.Add(new SqlParameter("@Visc", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Spec_Gr", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Grind", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Covering", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Gloss", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Scratch", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@F_Ball", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@BEnding", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@SolVan", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@EVAPort", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@BLush", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Colo", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@PH", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Soli", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Spr", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Draging", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Thickness", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@StgWidth", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@Fona", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@StgQty", SqlDbType.VarChar, 20)).Value = "";
                    CmdHead.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.SmallInt, 2)).Value = BomDefH.GroupId;
                    CmdHead.Parameters.Add(new SqlParameter("@ExpiryDate", SqlDbType.SmallDateTime)).Value = BomDefH.ExpiryDate;
                    CmdHead.Parameters.Add(new SqlParameter("@StandardCost", SqlDbType.Bit)).Value = BomDefH.StandardCost;
                    CmdHead.Parameters.Add(new SqlParameter("@susflag", SqlDbType.Bit)).Value = BomDefH.SusFlag;
                    CmdHead.Parameters.Add(new SqlParameter("@CalculatedSG", SqlDbType.Money)).Value = 0;
                    CmdHead.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    CmdHead.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 10)).Value = me.UserID;
                    CmdHead.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                    CmdHead.Transaction = transaction;
                    try
                    {
                        CmdHead.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                    long LogID = Convert.ToInt64(CmdHead.Parameters["@LogID"].Value);


                    foreach (prod_formula_detail_info item in BomDefD)
                    {
                        DrTmp = TmpDs.Tables["tmp"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["Comp_no"] = item.comp_no;
                        DrTmp["formula_code"] = item.formula_code;
                        DrTmp["item_no"] = item.item_no;
                        DrTmp["best_qty"] = item.best_qty;
                        if (item.lowest_qty == null)
                        {
                            DrTmp["lowest_qty"] = 0;
                        }
                        else
                        {
                            DrTmp["lowest_qty"] = item.lowest_qty;
                        }
                        DrTmp["rawmaterial_no"] = item.rawmaterial_no;
                        DrTmp["LossPerc"] = item.LossPerc;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp.EndEdit();
                        TmpDs.Tables["tmp"].Rows.Add(DrTmp);
                        i = i + 1;
                    }

                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "Bom_AddFrOmDet";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                        CmdIns.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@RMCode", SqlDbType.VarChar, 20, "rawmaterial_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@BestQty", SqlDbType.Float, 9, "best_qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@low_Qty", SqlDbType.Float, 9, "lowest_qty"));
                        CmdIns.Parameters.Add(new SqlParameter("@LossPerc", SqlDbType.Money, 8, "LossPerc"));
                        CmdIns.Parameters.Add(new SqlParameter("@stage_no", SqlDbType.SmallInt, 4, "stage_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "tmp");
                    }

                    if (BomDefProduct != null)
                    {
                        if (BomDefProduct.Count != 0)
                        {
                            i = 1;

                            foreach (prod_extend_item item in BomDefProduct)
                            {
                                DrTmp1 = TmpDs1.Tables["tmp1"].NewRow();
                                DrTmp1.BeginEdit();
                                DrTmp1["Comp_no"] = item.comp_no;
                                DrTmp1["formula_code"] = item.formula_code;
                                DrTmp1["item_no"] = item.item_no;
                                DrTmp1["ItemDesc"] = item.itemdesc;
                                DrTmp1["qty1"] = item.Qty1;
                                DrTmp1["qty2"] = item.Qty2;
                                DrTmp1["qty3"] = item.Qty3;
                                DrTmp1["percost"] = item.percost;
                                DrTmp1["UnitSerial"] = item.UnitSerial;
                                DrTmp1.EndEdit();
                                TmpDs1.Tables["tmp1"].Rows.Add(DrTmp1);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns2 = new SqlCommand())
                            {
                                CmdIns2.Connection = cn;
                                CmdIns2.CommandText = "Bom_AddExtendItem";
                                CmdIns2.CommandType = CommandType.StoredProcedure;
                                CmdIns2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                                CmdIns2.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                                CmdIns2.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                                CmdIns2.Parameters.Add(new SqlParameter("@ItemDesc", SqlDbType.VarChar, 60, "ItemDesc"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty1", SqlDbType.Float, 9, "Qty1"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty2", SqlDbType.Float, 9, "Qty2"));
                                CmdIns2.Parameters.Add(new SqlParameter("@Qty3", SqlDbType.Float, 9, "qty3"));
                                CmdIns2.Parameters.Add(new SqlParameter("@percost", SqlDbType.Float, 9, "percost"));
                                CmdIns2.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                                CmdIns2.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns2.Transaction = transaction;
                                DaPR1.InsertCommand = CmdIns2;
                                DaPR1.Update(TmpDs1, "tmp1");
                            }
                            PerCost = 100;
                            foreach (prod_extend_item item in BomDefProduct)
                            {
                                DrTmp5 = TmpDs5.Tables["tmp5"].NewRow();
                                DrTmp5.BeginEdit();
                                DrTmp5["compno"] = item.comp_no;
                                DrTmp5["FormulaCode"] = item.formula_code;
                                DrTmp5["ItemNo"] = item.item_no;
                                DrTmp5["ItemDesc"] = item.itemdesc;
                                DrTmp5["ProductPer"] = item.percost;
                                DrTmp5["C1Per"] = 100;
                                PerCost = PerCost - Convert.ToDecimal(DrTmp5["ProductPer"] ?? "0");
                                DrTmp5.EndEdit();
                                TmpDs5.Tables["tmp5"].Rows.Add(DrTmp5);
                            }

                            DrTmp5 = TmpDs5.Tables["tmp5"].NewRow();
                            DrTmp5.BeginEdit();
                            DrTmp5["compno"] = company.comp_num;
                            DrTmp5["FormulaCode"] = BomDefH.formula_code;
                            DrTmp5["ItemNo"] = BomDefH.item_no;
                            DrTmp5["ItemDesc"] = db.InvItemsMFs.Where(x => x.CompNo == BomDefH.comp_no && x.ItemNo == BomDefH.item_no).FirstOrDefault().ItemDesc;
                            DrTmp5["ProductPer"] = PerCost;
                            DrTmp5["C1Per"] = 100;
                            DrTmp5.EndEdit();
                            TmpDs5.Tables["tmp5"].Rows.Add(DrTmp5);

                            using (SqlCommand CmdIns6 = new SqlCommand())
                            {
                                CmdIns6.Connection = cn;
                                CmdIns6.CommandText = "Bom_AddProdItemsCostPer";
                                CmdIns6.CommandType = CommandType.StoredProcedure;
                                CmdIns6.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Compno"));
                                CmdIns6.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formulacode"));
                                CmdIns6.Parameters.Add(new SqlParameter("@ItemCode", SqlDbType.VarChar, 20, "ItemNo"));
                                CmdIns6.Parameters.Add(new SqlParameter("@itemDesc", SqlDbType.VarChar, 50, "ItemDesc"));
                                CmdIns6.Parameters.Add(new SqlParameter("@prodPer", SqlDbType.Float, 9, "ProductPer"));
                                CmdIns6.Parameters.Add(new SqlParameter("@C1Per", SqlDbType.Float, 9, "C1Per"));
                                CmdIns6.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                                CmdIns6.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns6.Parameters.Add(new SqlParameter("@EditLogID", SqlDbType.BigInt)).Direction = ParameterDirection.Output;
                                CmdIns6.Transaction = transaction;
                                DaPR5.InsertCommand = CmdIns6;
                                DaPR5.Update(TmpDs5, "tmp5");
                            }
                        }
                    }
                    if (BomDefAdditiveItems != null)
                    {
                        if (BomDefAdditiveItems.Count != 0)
                        {
                            i = 1;
                            foreach (Prod_BomAdditionalRM item in BomDefAdditiveItems)
                            {
                                DrTmp2 = TmpDs2.Tables["tmp2"].NewRow();
                                DrTmp2.BeginEdit();
                                DrTmp2["CompNo"] = item.CompNo;
                                DrTmp2["BomCode"] = item.BOMCode;
                                DrTmp2["RmCode"] = item.RMCode;
                                DrTmp2["UnitSerial"] = item.UnitSerial;
                                DrTmp2["Qty"] = item.QTY;
                                DrTmp2["Notes"] = item.Notes;
                                DrTmp2.EndEdit();
                                TmpDs2.Tables["tmp2"].Rows.Add(DrTmp2);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns3 = new SqlCommand())
                            {
                                CmdIns3.Connection = cn;
                                CmdIns3.CommandText = "Prodcost_AddBOMAdditionalItems";
                                CmdIns3.CommandType = CommandType.StoredProcedure;
                                CmdIns3.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                                CmdIns3.Parameters.Add(new SqlParameter("@bomCode", SqlDbType.VarChar, 20, "BOMCode"));
                                CmdIns3.Parameters.Add(new SqlParameter("@RmCode", SqlDbType.VarChar, 20, "RmCode"));
                                CmdIns3.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt, 1, "UnitSerial"));
                                CmdIns3.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                                CmdIns3.Parameters.Add(new SqlParameter("@notes", SqlDbType.VarChar, 200, "notes"));
                                CmdIns3.Transaction = transaction;
                                DaPR2.InsertCommand = CmdIns3;
                                DaPR2.Update(TmpDs2, "tmp2");
                            }
                        }
                    }
                    if (Prod_ItemStages != null)
                    {
                        if (Prod_ItemStages.Count != 0)
                        {
                            i = 1;
                            foreach (Prod_ItemStages item in Prod_ItemStages)
                            {
                                DrTmp4 = TmpDs4.Tables["tmp4"].NewRow();
                                DrTmp4.BeginEdit();
                                DrTmp4["comp_no"] = item.Comp_no;
                                DrTmp4["formula_code"] = item.Formula_code;
                                DrTmp4["stage_code"] = item.Stage_code;
                                DrTmp4["StageSer"] = i;
                                DrTmp4["Serial1"] = i;
                                DrTmp4["hr"] = item.Hr;
                                DrTmp4["min"] = item.Min;
                                DrTmp4["Item_no"] = item.Item_no;
                                DrTmp4["Stage_no"] = Prod_ItemStages.Count;
                                DrTmp4["ReqQty"] = 0;
                                DrTmp4["Repeat"] = 0;
                                DrTmp4["UnitPcs"] = 0;
                                DrTmp4["StopTime"] = 0;
                                DrTmp4["FixedTime"] = item.FixedTime;
                                DrTmp4["TimePerUnit"] = (item.Hr + item.Min / 60) / BomDefH.best_qty;
                                DrTmp4["ClosePStage"] = item.ClosePStage;
                                if (item.QAProc != null)
                                {
                                    DrTmp4["QAProc"] = item.QAProc;
                                }

                                if (item.QCFromNo != null)
                                {
                                    DrTmp4["QCFromNo"] = item.QCFromNo;
                                }
                                DrTmp4.EndEdit();
                                TmpDs4.Tables["tmp4"].Rows.Add(DrTmp4);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns5 = new SqlCommand())
                            {
                                CmdIns5.Connection = cn;
                                CmdIns5.CommandText = "Bom_AddFormStages";
                                CmdIns5.CommandType = CommandType.StoredProcedure;
                                CmdIns5.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "Comp_no"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20, "formula_code"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "Item_No"));
                                CmdIns5.Parameters.Add(new SqlParameter("@NoOfStages", SqlDbType.SmallInt, 4, "Stage_No"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5, "stage_code"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 4, "StageSer"));
                                CmdIns5.Parameters.Add(new SqlParameter("@HR", SqlDbType.Float, 8, "hr"));
                                CmdIns5.Parameters.Add(new SqlParameter("@Min", SqlDbType.Float, 8, "min"));
                                CmdIns5.Parameters.Add(new SqlParameter("@ReqQty", SqlDbType.Float, 8, "ReqQty"));
                                CmdIns5.Parameters.Add(new SqlParameter("@TimePerUnit", SqlDbType.Float, 8, "TimePerUnit"));
                                CmdIns5.Parameters.Add(new SqlParameter("@Repeat", SqlDbType.SmallInt, 2, "Repeat"));
                                CmdIns5.Parameters.Add(new SqlParameter("@UnitPcs", SqlDbType.SmallInt, 2, "UnitPcs"));
                                CmdIns5.Parameters.Add(new SqlParameter("@StopTime", SqlDbType.Money, 8, "StopTime"));
                                CmdIns5.Parameters.Add(new SqlParameter("@FixedTime", SqlDbType.Bit, 1, "FixedTime"));
                                CmdIns5.Parameters.Add(new SqlParameter("@ClosePStage", SqlDbType.Bit, 1, "ClosePStage"));
                                CmdIns5.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns5.Parameters.Add(new SqlParameter("@Serial1", SqlDbType.SmallInt, 4, "Serial1"));
                                CmdIns5.Parameters.Add(new SqlParameter("@QAProc", SqlDbType.SmallInt, 4, "QAProc"));
                                CmdIns5.Parameters.Add(new SqlParameter("@QCFromNo", SqlDbType.SmallInt, 4, "QCFromNo"));
                                CmdIns5.Transaction = transaction;
                                DaPR4.InsertCommand = CmdIns5;
                                DaPR4.Update(TmpDs4, "tmp4");
                            }
                        }
                    }
                    if (BomDefProperty != null)
                    {
                        if (BomDefProperty.Count != 0)
                        {
                            i = 1;
                            foreach (ProdCost_FormulaProperties item in BomDefProperty)
                            {
                                DrTmp3 = TmpDs3.Tables["tmp3"].NewRow();
                                DrTmp3.BeginEdit();
                                DrTmp3["CompNo"] = item.CompNo;
                                DrTmp3["FormulaCode"] = item.FormulaCode;
                                DrTmp3["PropertID"] = i;
                                DrTmp3["PropertName"] = item.PropertName;
                                DrTmp3["PropertValue"] = item.PropertValue;
                                DrTmp3.EndEdit();
                                TmpDs3.Tables["tmp3"].Rows.Add(DrTmp3);
                                i = i + 1;
                            }
                            using (SqlCommand CmdIns4 = new SqlCommand())
                            {
                                CmdIns4.Connection = cn;
                                CmdIns4.CommandText = "ProdCost_AddBomProperties";
                                CmdIns4.CommandType = CommandType.StoredProcedure;
                                CmdIns4.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2, "CompNo"));
                                CmdIns4.Parameters.Add(new SqlParameter("@FormulaCode", SqlDbType.VarChar, 20, "FormulaCode"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertID", SqlDbType.SmallInt, 2, "PropertID"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertName", SqlDbType.VarChar, 100, "PropertName"));
                                CmdIns4.Parameters.Add(new SqlParameter("@PropertValue", SqlDbType.VarChar, 200, "PropertValue"));
                                CmdIns4.Parameters.Add(new SqlParameter("@LogID", SqlDbType.BigInt)).Value = LogID;
                                CmdIns4.Transaction = transaction;
                                DaPR3.InsertCommand = CmdIns4;
                                DaPR3.Update(TmpDs3, "tmp3");
                            }
                        }
                    }

                }
                transaction.Commit();
                cn.Dispose();
            }

            //foreach (var itemIdentifier in Prod_ItemStages)
            //{
            //    //var existingData = db.Prod_ItemStages.FirstOrDefault(x => x.Comp_no == itemIdentifier.Comp_no && x.Formula_code == itemIdentifier.Formula_code && x.Stage_code == itemIdentifier.Stage_code);
            //    var existingData = db.Prod_ItemStages.Where(x => x.Stage_code == itemIdentifier.Stage_code).FirstOrDefault(x => x.Comp_no == itemIdentifier.Comp_no && x.Formula_code == itemIdentifier.Formula_code);
            //    if (existingData != null)
            //    {
            //        var oldQAProc = existingData.QAProc;
            //        if (itemIdentifier.QAProc != null)
            //        {
            //            existingData.QAProc = itemIdentifier.QAProc;
            //        }
            //        else if (existingData.QAProc == itemIdentifier.QAProc)
            //        {
            //            existingData.QAProc = existingData.QAProc;
            //        }
            //        else if (existingData.QAProc != null && itemIdentifier.QAProc == null)
            //        {
            //            existingData.QAProc = oldQAProc;// old value
            //        }

            //        db.SaveChanges();
            //    }
            //}
            //foreach (var EditTest in Prod_ItemStages)
            //{
            //    //var existingData = db.Prod_ItemStages.FirstOrDefault(x => x.Comp_no == itemIdentifier.Comp_no && x.Formula_code == itemIdentifier.Formula_code && x.Stage_code == itemIdentifier.Stage_code);
            //    var existingData = db.Prod_ItemStages.Where(x => x.Stage_code == EditTest.Stage_code).FirstOrDefault(x => x.Comp_no == EditTest.Comp_no && x.Formula_code == EditTest.Formula_code);
            //    if (existingData != null)
            //    {
            //        var oldEditTest = existingData.QCFromNo;
            //        if (EditTest.QCFromNo != null)
            //        {
            //            existingData.QCFromNo = EditTest.QCFromNo;
            //        }
            //        else if (existingData.QCFromNo == EditTest.QCFromNo)
            //        {
            //            existingData.QCFromNo = existingData.QCFromNo;
            //        }
            //        else if (existingData.QCFromNo != null && EditTest.QCFromNo == null)
            //        {
            //            existingData.QCFromNo = oldEditTest;// old value
            //        }

            //        db.SaveChanges();
            //    }
            //}


            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadUnitItem(string ItemNo)
        {
            DataTable Dt = new DataTable();
            decimal QtyOH = 0;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "InvT_LoadItemUnits";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemCode", System.Data.SqlDbType.VarChar, 20)).Value = ItemNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<InvUnitCode> InvUnitCode = Dt.AsEnumerable().Select(row => new InvUnitCode
            {
                VSUnitCode = row.Field<int>("UnitSerial"),
                UnitName = row.Field<string>("UnitName"),
                UnitNameEng = row.Field<string>("UnitNameEng")
            }).ToList();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "InvT_GetItemQtyOH";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", System.Data.SqlDbType.VarChar, 20)).Value = ItemNo;
                    QtyOH = (decimal)cmd.ExecuteScalar();
                }
            }


            return Json(new { unit = InvUnitCode,Qty = QtyOH }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadQCTestRelatedToForm(int FormNo)
        {
            DataTable Dt = new DataTable();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                Dt = new DataTable();
                cn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCostWeb_LoadStageFromTest";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormNo", System.Data.SqlDbType.Int)).Value = FormNo;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<PordCost_FormTest> QCTestRelForm = Dt.AsEnumerable().Select(row => new PordCost_FormTest
            {
                TestNo = row.Field<short>("TestNo"),
                LocalDesc = row.Field<string>("LocalDesc"),
                EngDesc = row.Field<string>("EngDesc"),
                FromNo = row.Field<decimal>("FromNo"),
                ToNo = row.Field<decimal>("ToNo")
            }).ToList();
            return Json(new { FormQC = QCTestRelForm }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save_QCTestBOM(List<Prod_ItemStagesTestQuality> BomQCTest)
        {
            DataTable TmpDb = new DataTable();
            TmpDb.Columns.Add("Formula_code", typeof(string));
            TmpDb.Columns.Add("Item_no", typeof(string));
            TmpDb.Columns.Add("Stage_code", typeof(int));
            TmpDb.Columns.Add("StageSer", typeof(short));
            TmpDb.Columns.Add("TestNo", typeof(short));
            TmpDb.Columns.Add("FromNo", typeof(double));
            TmpDb.Columns.Add("ToNo", typeof(double));
            TmpDb.Columns.Add("Serial1", typeof(int));
            SqlDataAdapter TmpDa = new SqlDataAdapter();
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                transaction = cn.BeginTransaction();
                Prod_ItemStagesTestQuality e1 = BomQCTest.FirstOrDefault();
                List<Prod_ItemStagesTestQuality> ex = db.Prod_ItemStagesTestQuality.Where(x => x.CompNo == e1.CompNo && x.Stage_code == e1.Stage_code && x.StageSer == e1.StageSer && x.Item_no == e1.Item_no).ToList();
                if (ex.Count != 0)
                {
                    db.Prod_ItemStagesTestQuality.RemoveRange(ex);
                    db.SaveChanges();
                }
                if (BomQCTest != null)
                {
                    if (BomQCTest.Count != 0)
                    {
                        foreach (Prod_ItemStagesTestQuality item in BomQCTest)
                        {
                            DataRow DrTmp = TmpDb.NewRow();
                            DrTmp.BeginEdit();
                            DrTmp["Formula_code"] = item.Formula_code;
                            DrTmp["Item_no"] = item.Item_no;
                            DrTmp["Stage_code"] = item.Stage_code;
                            DrTmp["StageSer"] = item.StageSer;
                            DrTmp["TestNo"] = item.TestNo;
                            DrTmp["FromNo"] = item.FromNo;
                            DrTmp["ToNo"] = item.ToNo;
                            DrTmp["Serial1"] = item.Serial1;
                            DrTmp.EndEdit();
                            TmpDb.Rows.Add(DrTmp);
                        }
                        using (SqlCommand CmdIns = new SqlCommand())
                        {
                            CmdIns.Connection = cn;
                            CmdIns.CommandText = "ProdCost_AddStageTestQuality";
                            CmdIns.CommandType = CommandType.StoredProcedure;
                            CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                            CmdIns.Parameters.Add(new SqlParameter("@Formula_code", SqlDbType.VarChar, 50, "Formula_code"));
                            CmdIns.Parameters.Add(new SqlParameter("@Item_no", SqlDbType.VarChar, 50, "Item_no"));
                            CmdIns.Parameters.Add(new SqlParameter("@Stage_code", SqlDbType.Int, 8, "Stage_code"));
                            CmdIns.Parameters.Add(new SqlParameter("@StageSer", SqlDbType.SmallInt, 4, "StageSer"));
                            CmdIns.Parameters.Add(new SqlParameter("@TestNo", SqlDbType.SmallInt, 4, "TestNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@FromNo", SqlDbType.Money, 8, "FromNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@ToNo", SqlDbType.Money, 8, "ToNo"));
                            CmdIns.Parameters.Add(new SqlParameter("@Serial1", SqlDbType.SmallInt, 4, "Serial1"));
                            CmdIns.Transaction = transaction;
                            TmpDa.InsertCommand = CmdIns;
                            TmpDa.Update(TmpDb);
                        }
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public class PordCost_FormTest
        {
            public short TestNo { get; set; }
             public string LocalDesc { get; set; }
            public string EngDesc { get; set; }
            public decimal FromNo { get; set; }
            public decimal ToNo { get; set; }
        }

        public JsonResult GetDataQCTestBOM(Prod_ItemStagesTestQuality itemQCTest)
        {
            List<Prod_ItemStagesTestQuality> ex = db.Prod_ItemStagesTestQuality.Where(
                x => x.CompNo == company.comp_num && x.Formula_code == itemQCTest.Formula_code
                && x.Item_no == itemQCTest.Item_no && x.Stage_code == itemQCTest.Stage_code
                && x.StageSer == itemQCTest.StageSer).ToList();

            if (ex.Count != 0)
            {
                return Json(new { d = "NotDelete" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { d = "Delete" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RemoveDataQCTestBOM(Prod_ItemStagesTestQuality itemQCTest)
        {
            List<Prod_ItemStagesTestQuality> ex = db.Prod_ItemStagesTestQuality.Where(
                x => x.CompNo == company.comp_num && x.Formula_code == itemQCTest.Formula_code
                && x.Item_no == itemQCTest.Item_no && x.Stage_code == itemQCTest.Stage_code
                && x.StageSer == itemQCTest.StageSer).ToList();

            Prod_ItemStages ex1 = db.Prod_ItemStages.Where(x => x.Comp_no == company.comp_num
            && x.Formula_code == itemQCTest.Formula_code && x.Item_no == itemQCTest.Item_no
            && x.Stage_code == itemQCTest.Stage_code && x.StageSer == itemQCTest.StageSer).FirstOrDefault();

            if (ex.Count != 0)
            {
                db.Prod_ItemStagesTestQuality.RemoveRange(ex);
                db.SaveChanges();
                db.Prod_ItemStages.Remove(ex1);
                db.SaveChanges();
            }
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelBom_Form(string FormCode)
        {
            var dele = db.prod_prepare_info.Where(x => x.comp_no == (int)this.company.comp_num && x.formula_code == FormCode).ToList();
            if (dele.Count  > 0)
                return this.Json((object)new
                {
                    error = this.Translate("لا يمكن حذف المعادلة الانتاج", "The production equation cannot be deleted."),
                    AllowGet = JsonRequestBehavior.AllowGet
                });
            using (SqlConnection sqlConnection = new SqlConnection(this.ConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "Bom_DeleteForm";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = (object)this.company.comp_num;
                    command.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar)).Value = (object)FormCode;
                    command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = (object)this.me.UserID;
                    command.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = (object)3;
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    command.Transaction = sqlTransaction;
                    try
                    {
                        command.ExecuteNonQuery();
                        sqlTransaction.Commit();
                        sqlConnection.Dispose();
                        return this.Json((object)new
                        {
                            ok = "ok",
                            AllowGet = JsonRequestBehavior.AllowGet
                        });
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        sqlConnection.Dispose();
                        return this.Json((object)new
                        {
                            error = this.Translate("حدث خطأ", "An error occurred."),
                            AllowGet = JsonRequestBehavior.AllowGet
                        });
                    }
                }
            }
        }
    }
}