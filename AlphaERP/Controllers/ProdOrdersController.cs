using Microsoft.VisualBasic.CompilerServices;
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
    public class ProdOrdersController : controller
    {
        // GET: ProdOrders
        public ActionResult Index()
        {
            //List<prod_prepare_info> prepare_info = db.prod_prepare_info.Where(x => x.comp_no == company.comp_num && x.prepare_year == DateTime.Now.Year).OrderByDescending(x => x.prepare_year).ToList();
            List<Prod_prepare_infoView> prepare_info = db.Database.SqlQuery<Prod_prepare_infoView>("exec ProdOrders_WebLists {0},{1}", company.comp_num, DateTime.Now.Year).ToList();

            return View(prepare_info);
        }
        public ActionResult ProdOrdersList(short Year)
        {
            List<Prod_prepare_infoView> prepare_info = db.Database.SqlQuery<Prod_prepare_infoView>("exec ProdOrders_WebLists {0},{1}", company.comp_num, Year).ToList();

            return View(prepare_info);
        }
        public ActionResult FormulaItem()
        {
            string Condition  = " where (comp_no = '" + company.comp_num + "') AND (Prod_BomGroupsPerms.UserID = '" + me.UserID + "')";
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Search";
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    cmd.Parameters.Add(new SqlParameter("@Condition", SqlDbType.VarChar, 2000)).Value = Condition;
                    cmd.Parameters.Add(new SqlParameter("@ProgID", SqlDbType.SmallInt)).Value = 393;
                   
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<Formula> Formula_Items = Dt.AsEnumerable().Select(row => new Formula
            {
                Formula_code = row.Field<string>("formula_code"),
                Formula_Desc = row.Field<string>("formula_desc"),
                Item_no = row.Field<string>("item_no"),
                Item_Desc = row.Field<string>("ItemDesc")
            }).ToList();

            return PartialView(Formula_Items);
        }
        public ActionResult eProdOrderInfo(short CompNo, int prepareyear, int preparecode)
        {
            prod_prepare_info prepare_info = db.prod_prepare_info.Where(x => x.comp_no == CompNo 
                                   && x.prepare_year == prepareyear && x.prepare_code == preparecode).FirstOrDefault();
            return PartialView(
               string.Format("~/Views/ProdOrders/tabs/eProdOrderInfo.cshtml"), prepare_info
                );
        }
        public ActionResult eRMItem(short CompNo, int prepareyear, int preparecode, string formulaCode, string Itemno, double ReqQty, int sataus)
        {
            IList<RMItems> RM_Items;
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderRM";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = Itemno;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@FStat", SqlDbType.Bit)).Value = sataus;
                    cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = prepareyear;
                    cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = preparecode;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            if (sataus == 0)
            {
                RM_Items = Dt.AsEnumerable().Select(row => {
                    string rmCode = row.Field<string>("RmCode");
                    string refNo = ""; 
                     
                        refNo = db.Prod_RevisionSetup.Where(x => x.CompNo == CompNo && x.ItemNo == rmCode && x.ProductionFlag == true).OrderByDescending(x => x.Id).Select(x => x.RevisionNo).FirstOrDefault() ?? "";
                     return new RMItems
                    {
                        RmCode = rmCode,
                        ItemDesc = row.Field<string>("ItemDesc"),
                        UnitSerial = row.Field<Byte>("UnitSerial"),
                        RmQty = row.Field<double>("RmQty"),
                        TotalQty = row.Field<decimal>("TotalQty"),
                        Shortage = row.Field<double>("Shortage"),
                        RefrenceNo = refNo,
                        count = 1,
                        staus = 2
                    };
                }).ToList();
            }
            else
            {
                 RM_Items = Dt.AsEnumerable().Select(row => {
                    return new RMItems
                    {
                        RmCode = row.Field<string>("RmCode"),
                        ItemDesc = row.Field<string>("ItemDesc"),
                        UnitSerial = Convert.ToByte(row.Field<short>("UnitSerial")),
                        RmQty = Convert.ToDouble(row.Field<double>("RmQty")),
                        TotalQty = row.Field<decimal>("TotalQty"),
                        Shortage = Convert.ToDouble(row.Field<double>("Shortage")),
                        RefrenceNo = row.Field<string>("RevisionNo"),
                        count = 1,
                        staus = 2
                    };
                }).ToList();
            }
         
            return PartialView(
               string.Format("~/Views/ProdOrders/RMItem.cshtml"), RM_Items
                );
        }
        [HttpGet]
        public ActionResult GetItemRevisions(string itemNo)
        {
            var revisions = db.Prod_RevisionSetup
                .Where(x => x.CompNo == company.comp_num && x.ItemNo == itemNo && x.ProductionFlag == true).ToList();
            
            return PartialView("_RevisionsModal", revisions);
        }

        public ActionResult RMItem(string formulaCode,string Itemno,double ReqQty)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderRM";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = Itemno;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@FStat", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value =0;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<RMItems> RM_Items = Dt.AsEnumerable().Select(row => {
                 string rmCode = row.Field<string>("RmCode");
                 string refNo = db.Prod_RevisionSetup.Where(x => x.CompNo == company.comp_num && x.ItemNo == rmCode && x.ProductionFlag == true).OrderByDescending(x => x.Id).Select(x => x.RevisionNo).FirstOrDefault() ?? "";
                 return new RMItems
                {
                    RmCode = rmCode,
                    ItemDesc = row.Field<string>("ItemDesc"),
                    UnitSerial = row.Field<Byte>("UnitSerial"),
                    RmQty = row.Field<double>("RmQty"),
                    TotalQty = row.Field<decimal>("TotalQty"),
                    Shortage = row.Field<double>("Shortage"),
                    count = 1,
                    staus = 1,
                    RefrenceNo = refNo,
                };
            }).ToList();

            return PartialView(RM_Items);
        }
        public ActionResult eReplacementPartial(string formulaCode, string Itemno, double ReqQty, int prepareyear, int preparecode)
        {
            int sataus = 0;
            prod_prepare_info prepare_info = db.prod_prepare_info.Where(x => x.comp_no == company.comp_num
                                   && x.prepare_year == prepareyear && x.prepare_code == preparecode).FirstOrDefault();

            if (prepare_info.formula_code == formulaCode && prepare_info.item_no == Itemno)
            {
                sataus = 1;
            }
            IList<RMItems> RM_Items;
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderRM";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = Itemno;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@FStat", SqlDbType.Bit)).Value = sataus;
                    cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = prepareyear;
                    cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = preparecode;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            List<string> isexest = new List<string>();

            if (sataus == 0)
            {
                RM_Items = Dt.AsEnumerable().Select(row => {
                    string rmCode = row.Field<string>("RmCode");
                    string refNo = row.Field<string>("RefrenceNo");
                   
                    return new RMItems
                    {
                        RmCode = rmCode,
                        ItemDesc = row.Field<string>("ItemDesc"),
                        UnitSerial = row.Field<Byte>("UnitSerial"),
                        RmQty = row.Field<double>("RmQty"),
                        TotalQty = row.Field<decimal>("TotalQty"),
                        Shortage = row.Field<double>("Shortage"),
                        RefrenceNo = refNo,
                        count = 1,
                        staus = 2
                    };
                }).ToList();
            }
            else
            {
                RM_Items = Dt.AsEnumerable().Select(row => {
                    return new RMItems
                    {
                        RmCode = row.Field<string>("RmCode"),
                        ItemDesc = row.Field<string>("ItemDesc"),
                        UnitSerial = Convert.ToByte(row.Field<short>("UnitSerial")),
                        RmQty = Convert.ToDouble(row.Field<double>("RmQty")),
                        TotalQty = row.Field<decimal>("TotalQty"),
                        Shortage = Convert.ToDouble(row.Field<double>("Shortage")),
                        RefrenceNo = row.Field<string>("RefrenceNo"),
                        count = 1,
                        staus = 2
                    };
                }).ToList();
            }
              
            isexest = RM_Items.Select(x => x.RmCode).ToList();

            if (!isexest.Contains(Itemno))
            {
                RMItems RM_Item = new RMItems();
                RM_Item.RmCode = Itemno;
                if (Language == "en")
                {
                    RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == Itemno).FirstOrDefault().ItemDesc_Ara;
                }
                else
                {
                    RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == Itemno).FirstOrDefault().ItemDesc;
                }
                RM_Item.RmQty = 0;
                RM_Item.UnitSerial = db.prod_formula_header_info.Where(x => x.comp_no == company.comp_num && x.formula_code == formulaCode
                                      && x.item_no == Itemno).FirstOrDefault().UnitSerial.Value;
                using (SqlConnection conn = new SqlConnection(ConnectionString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "InvT_GetItemQtyOH";
                        cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                        cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = Itemno;
                        RM_Item.TotalQty = Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
                RM_Item.Shortage = Convert.ToDouble(RM_Item.TotalQty) - 0;
                RM_Item.count = 1;
                RM_Item.staus = 2;
                RM_Items.Add(RM_Item);
            }
           

            return PartialView(
               string.Format("~/Views/ProdOrders/RMItem.cshtml"), RM_Items
                );
        }
        public ActionResult ReplacementPartial(string formulaCode, string Itemno, double ReqQty)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderRM";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = Itemno;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@FStat", SqlDbType.Bit)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = 0;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<RMItems> RM_Items = Dt.AsEnumerable().Select(row => {
                return new RMItems
                {
                    RmCode = row.Field<string>("RmCode"),
                    ItemDesc = row.Field<string>("ItemDesc"),
                    UnitSerial = row.Field<Byte>("UnitSerial"),
                    RmQty = row.Field<double>("RmQty"),
                    TotalQty = row.Field<decimal>("TotalQty"),
                    Shortage = row.Field<double>("Shortage"),
                    count = 1,
                    staus = 1
                };
            }).ToList();

            RMItems RM_Item = new RMItems();
            RM_Item.RmCode = Itemno;
            if (Language == "en")
            {
                RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == Itemno).FirstOrDefault().ItemDesc_Ara;
            }
            else
            {
                RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == Itemno).FirstOrDefault().ItemDesc;
            }
            RM_Item.RmQty = 0;
            RM_Item.UnitSerial = db.prod_formula_header_info.Where(x => x.comp_no == company.comp_num && x.formula_code == formulaCode
                                  && x.item_no == Itemno).FirstOrDefault().UnitSerial.Value;
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InvT_GetItemQtyOH";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = Itemno;
                    RM_Item.TotalQty = Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            RM_Item.Shortage = Convert.ToDouble(RM_Item.TotalQty) - 0;
            RM_Item.count = 1;
            RM_Item.staus = 1;
            RM_Items.Add(RM_Item);

            return PartialView(
               string.Format("~/Views/ProdOrders/RMItem.cshtml"), RM_Items
                );
        }
        public ActionResult ReplaceRMItem(string Formulacode, string ItemNo, double ReqQty,int stau)
        {
            List<RMItems> RM_Items = new List<RMItems>();
            RMItems RM_Item = new RMItems();
            RM_Item.RmCode = ItemNo;
            if (Language == "en")
            {
                RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == ItemNo).FirstOrDefault().ItemDesc_Ara;
            }
            else
            {
                RM_Item.ItemDesc = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == ItemNo).FirstOrDefault().ItemDesc;
            }
            RM_Item.RmQty = ReqQty;
            RM_Item.UnitSerial = db.prod_formula_header_info.Where(x => x.comp_no == company.comp_num && x.formula_code == Formulacode
                                  && x.item_no == ItemNo).FirstOrDefault().UnitSerial.Value;
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InvT_GetItemQtyOH";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = ItemNo;
                    RM_Item.TotalQty =Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            RM_Item.Shortage = Convert.ToDouble(RM_Item.TotalQty) - ReqQty;
            RM_Item.count = 1;
            RM_Item.staus = stau;
            RM_Items.Add(RM_Item);
            return PartialView(
               string.Format("~/Views/ProdOrders/RMItem.cshtml"), RM_Items
                );
        }
        public ActionResult Stages(string formulaCode, string Itemno, double ReqQty)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_OderStages";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 2)).Value =0;
                    cmd.Parameters.Add(new SqlParameter("@PrepForm", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@PrepQty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@NewStat", SqlDbType.Bit)).Value = 0;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<ItemStages> Item_Stages = Dt.AsEnumerable().Select(row => new ItemStages
            {
                Stage_code = row.Field<int>("Stage_code"),
                stage_desc = row.Field<string>("stage_desc"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate"),
                CalcTime =Convert.ToDouble(row.Field<decimal>("CalcTime")),
                staus = 1
            }).ToList();

            return PartialView(Item_Stages);
        }
        public ActionResult eStages(short CompNo, int prepareyear, int preparecode, string formulaCode, string Itemno, double ReqQty, int sataus)
        {
            IList<ItemStages> Item_Stages;
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_OderStages";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = CompNo;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = prepareyear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 2)).Value = preparecode;
                    cmd.Parameters.Add(new SqlParameter("@PrepForm", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@PrepQty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.Int)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@NewStat", SqlDbType.Bit)).Value = sataus;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            if (sataus == 0)
            {
                Item_Stages = Dt.AsEnumerable().Select(row => new ItemStages
                {
                    Stage_code = row.Field<int>("Stage_code"),
                    stage_desc = row.Field<string>("stage_desc"),
                    StartDate = row.Field<DateTime>("StartDate"),
                    EndDate = row.Field<DateTime>("EndDate"),
                    CalcTime = Convert.ToDouble(row.Field<decimal>("CalcTime")),
                    staus = 2
                }).ToList();
            }
            else
            {
                Item_Stages = Dt.AsEnumerable().Select(row => new ItemStages
                {
                    Stage_code = Convert.ToInt32(row.Field<string>("Stage_code")),
                    stage_desc = row.Field<string>("stage_desc"),
                    StartDate = row.Field<DateTime>("StartDate"),
                    EndDate = row.Field<DateTime>("EndDate"),
                    CalcTime = Convert.ToDouble(row.Field<decimal>("CalcTime")),
                    staus = 2
                }).ToList();
            }

            return PartialView(
               string.Format("~/Views/ProdOrders/Stages.cshtml"), Item_Stages
                );
        }
        public ActionResult Product(string formulaCode, string Itemno, double ReqQty)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderExtended";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = DateTime.Now.Year;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 2)).Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.Bit)).Value = 1;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<ItemProduct> Item_Product = Dt.AsEnumerable().Select(row => new ItemProduct
            {
                item_no = row.Field<string>("item_no"),
                ItemDesc = row.Field<string>("ItemDesc"),
                IUnit = row.Field<string>("IUnit"),
                Categ = row.Field<string>("Categ"),
                Qty = row.Field<double>("Qty"),
                CostPrc = Convert.ToDouble(row.Field<decimal>("CostPrc"))
            }).ToList();

            return PartialView(Item_Product);
        }
        public ActionResult eProduct(short CompNo, int prepareyear, int preparecode, string formulaCode, string Itemno, double ReqQty)
        {
            prod_prepare_info prepare_info = db.prod_prepare_info.Where(x => x.comp_no == CompNo
                                  && x.prepare_year == prepareyear && x.prepare_code == preparecode).FirstOrDefault();
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_GetOrderExtended";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar, 20)).Value = formulaCode;
                    cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9)).Value = ReqQty;
                    cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = prepareyear;
                    cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 2)).Value = preparecode;
                    cmd.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.Bit)).Value = 2;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<ItemProduct> Item_Product = Dt.AsEnumerable().Select(row => new ItemProduct
            {
                item_no = row.Field<string>("item_no"),
                ItemDesc = row.Field<string>("ItemDesc"),
                IUnit = row.Field<string>("IUnit"),
                Categ = row.Field<string>("Categ"),
                Qty = row.Field<double>("Qty"),
                CostPrc = Convert.ToDouble(row.Field<decimal>("CostPrc"))
            }).ToList();

            return PartialView(
               string.Format("~/Views/ProdOrders/Product.cshtml"), Item_Product
                );
        }
        public ActionResult AlternativeItem(string ItemNo)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "invt_getAltItemsForProdOrder";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = ItemNo;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@Glang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<ItemProduct> Item_Product = Dt.AsEnumerable().Select(row => new ItemProduct
            {
                item_no = row.Field<string>("AlternativeItemNo"),
                ItemDesc = row.Field<string>("ItemDesc"),
                IUnit = row.Field<string>("UnitC4"),
                AvlQty = row.Field<decimal>("AvlQty")
            }).ToList();

            return PartialView(Item_Product);
        }
        public ActionResult AlternativeRMItem(List<RMItems> RM_Items)
        {
            return PartialView(
               string.Format("~/Views/ProdOrders/RMItem.cshtml"), RM_Items
                );
        }
        public JsonResult Save_ProdOrders(prod_prepare_info ProdInfoHF, List<Prod_prepare_info_detail> ProdInfoDF,List<prod_approve_manufacure> manufacure,List<ProdCost_OrderExtended> OrderExtended)
        {
            DataSet TmpDs = new DataSet();
            DataSet TmpDs1 = new DataSet();
            DataSet TmpDs2 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp2;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select comp_no, prepare_year, prepare_code, item_no, RevisionNo, rawmaterial_no, UnitSerial, iqty, rqty, NoteDet from Prod_prepare_info_detail where Comp_No = 0", cn);
                DaPR.Fill(TmpDs, "ProdDF");

                DaPR1.SelectCommand = new SqlCommand("SELECT prod_approve_manufacure.*  FROM prod_approve_manufacure WHERE (((prod_approve_manufacure.comp_no)=0))", cn);
                DaPR1.Fill(TmpDs1, "ProdStages");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM ProdCost_OrderExtended WHERE compno = 0", cn);
                DaPR2.Fill(TmpDs2, "NewExtended");

                using (SqlCommand CmdH = new SqlCommand())
                {
                    CmdH.Connection = cn;
                    CmdH.CommandText = "ProdCost_AddProdPrepareHF";
                    CmdH.CommandType = CommandType.StoredProcedure;
                    CmdH.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = ProdInfoHF.comp_no;
                    CmdH.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = ProdInfoHF.prepare_year;
                    CmdH.Parameters.Add(new SqlParameter("@PrepCode", SqlDbType.Int)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@PrepDate", SqlDbType.SmallDateTime)).Value = ProdInfoHF.prepare_date;
                    CmdH.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ProdInfoHF.item_no;
                    CmdH.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar)).Value = ProdInfoHF.formula_code;
                    CmdH.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float)).Value = ProdInfoHF.qty_prepare;
                    CmdH.Parameters.Add(new SqlParameter("@CUser", SqlDbType.VarChar)).Value = me.UserID;
                    CmdH.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = ProdInfoHF.PlanYear;
                    CmdH.Parameters.Add(new SqlParameter("@PlanMonth", SqlDbType.SmallInt)).Value = ProdInfoHF.PlanMonth;
                    CmdH.Parameters.Add(new SqlParameter("@PlanDay", SqlDbType.SmallInt)).Value = DateTime.Now.Day;
                    CmdH.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.SmallInt)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@RefNo1", SqlDbType.VarChar, 100)).Value = ProdInfoHF.RefNo1 ?? "";
                    CmdH.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar, 100)).Value = ProdInfoHF.RefNo2 ?? "";
                    if (ProdInfoHF.waste == null)
                    {
                        ProdInfoHF.waste = 0;
                    }
                    CmdH.Parameters.Add(new SqlParameter("@Waste", SqlDbType.Money)).Value = ProdInfoHF.waste;
                    CmdH.Parameters.Add(new SqlParameter("@orderno", SqlDbType.Int)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@ProNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    if (ProdInfoHF.Note == null)
                    {
                        ProdInfoHF.Note = "";
                    }
                    CmdH.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = ProdInfoHF.Note;
                    CmdH.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@ExCost", SqlDbType.Money)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@ExCostReason", SqlDbType.VarChar, 50)).Value = "";
                    CmdH.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar, 10)).Value = ProdInfoHF.prepare_date;
                    CmdH.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = ProdInfoHF.BusUnitID;
                    CmdH.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = ProdInfoHF.StoreNo;
                    CmdH.Parameters.Add(new SqlParameter("@Actual_SG", SqlDbType.Money)).Value = ProdInfoHF.Actual_SG;
                    CmdH.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = ProdInfoHF.SalesOrderY;
                    CmdH.Parameters.Add(new SqlParameter("@SalesOrderN", SqlDbType.Int)).Value = 0;
                    transaction = cn.BeginTransaction();
                    CmdH.Transaction = transaction;
                    try
                    {
                        CmdH.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    int ProNo = Convert.ToInt32(CmdH.Parameters["@ProNo"].Value);
                    if (ProNo == -10)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = Resources.Resource.errorProdOrder }, JsonRequestBehavior.AllowGet);
                    }

                    foreach (Prod_prepare_info_detail item in ProdInfoDF)
                    {
                        DrTmp = TmpDs.Tables["ProdDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["comp_No"] = item.comp_no;
                        DrTmp["prepare_year"] = item.prepare_year;
                        DrTmp["prepare_code"] = ProNo;
                        DrTmp["item_no"] = item.item_no;
                        DrTmp["RevisionNo"] = item.RevisionNo;
                        DrTmp["rawmaterial_no"] = item.rawmaterial_no;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["Iqty"] = item.iqty;
                        DrTmp["RQty"] = item.rqty;
                        DrTmp["NoteDet"] = item.NoteDet;
                        DrTmp.EndEdit();
                        TmpDs.Tables["ProdDF"].Rows.Add(DrTmp);
                    }
                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "ProdCost_AddProdPrepareDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "comp_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt, 4, "prepare_year"));
                        CmdIns.Parameters.Add(new SqlParameter("@PrepCode", SqlDbType.Int, 8, "prepare_code"));
                        CmdIns.Parameters.Add(new SqlParameter("@FromCode", SqlDbType.VarChar, 20, "item_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@RevisionNo", SqlDbType.VarChar, 100, "RevisionNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemRm", SqlDbType.VarChar, 20, "rawmaterial_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@IQty", SqlDbType.Float, 9, "IQty"));
                        CmdIns.Parameters.Add(new SqlParameter("@RQty", SqlDbType.Float, 9, "rqty"));
                        CmdIns.Parameters.Add(new SqlParameter("@NoteDet", SqlDbType.VarChar, 300, "NoteDet"));
                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "ProdDF");
                    }

                    if(manufacure != null)
                    {
                        int i = 1;
                        foreach (prod_approve_manufacure item in manufacure)
                        {
                            DrTmp1 = TmpDs1.Tables["ProdStages"].NewRow();
                            DrTmp1.BeginEdit();
                            DrTmp1["comp_No"] = item.comp_no;
                            DrTmp1["prepare_year"] = item.prepare_year;
                            DrTmp1["prepare_code"] = ProNo;
                            DrTmp1["hiring_no"] = 1;
                            DrTmp1["stage_code"] = item.stage_code;
                            DrTmp1["stageSer"] = i;
                            DrTmp1["item_no"] = item.item_no;
                            DrTmp1["begin_date"] = item.begin_date;
                            DrTmp1["Closed_Date"] = item.Closed_Date;
                            DrTmp1["CalcTime"] = item.CalcTime;
                            DrTmp1.EndEdit();
                            TmpDs1.Tables["ProdStages"].Rows.Add(DrTmp1);
                            i++;
                        }

                        using (SqlCommand CmdIns1 = new SqlCommand())
                        {
                            CmdIns1.Connection = cn;
                            CmdIns1.CommandText = "ProdCost_AppProdAproveStages";
                            CmdIns1.CommandType = CommandType.StoredProcedure;
                            CmdIns1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "comp_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "prepare_year"));
                            CmdIns1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "prepare_code"));
                            CmdIns1.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.Int, 9, "hiring_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5, "stage_code"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Stageser", SqlDbType.SmallInt, 5, "stageSer"));
                            CmdIns1.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.SmallDateTime, 8, "begin_date"));
                            CmdIns1.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.SmallDateTime, 8, "Closed_Date"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Real_date_begining", SqlDbType.SmallDateTime, 8, "Real_date_begining"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Closed_Time", SqlDbType.SmallDateTime, 8, "Closed_Time"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StageStat", SqlDbType.Bit, 1, "Stage_Status"));
                            CmdIns1.Parameters.Add(new SqlParameter("@WorkedHour", SqlDbType.SmallInt, 1, "WorkedHour"));
                            CmdIns1.Parameters.Add(new SqlParameter("@workedmin", SqlDbType.SmallInt, 1, "workedmin"));
                            CmdIns1.Parameters.Add(new SqlParameter("@CalcTime", SqlDbType.Money, 8, "CalcTime"));
                            CmdIns1.Transaction = transaction;
                            DaPR1.InsertCommand = CmdIns1;
                            DaPR1.Update(TmpDs1, "ProdStages");
                        }

                    }

                    if (OrderExtended != null)
                    {
                        foreach (ProdCost_OrderExtended item in OrderExtended)
                        {
                            DrTmp2 = TmpDs2.Tables["NewExtended"].NewRow();
                            DrTmp2.BeginEdit();
                            DrTmp2["compno"] = item.CompNo;
                            DrTmp2["OrderYear"] = item.OrderYear;
                            DrTmp2["OrderNo"] = ProNo;
                            DrTmp2["item_no"] = item.item_no;
                            DrTmp2["formula_code"] = item.formula_code;
                            DrTmp2["Qty"] = item.Qty;
                            DrTmp2["CostPrc"] = item.CostPrc;
                            DrTmp2.EndEdit();
                            TmpDs2.Tables["NewExtended"].Rows.Add(DrTmp2);
                        }

                        using (SqlCommand CmdInsExt = new SqlCommand())
                        {
                            CmdInsExt.Connection = cn;
                            CmdInsExt.CommandText = "ProdCost_AddPrepExtend";
                            CmdInsExt.CommandType = CommandType.StoredProcedure;
                            CmdInsExt.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2, "compno"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2, "OrderYear"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 4, "OrderNo"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "item_no"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@FormulaCode", SqlDbType.VarChar, 20, "formula_code"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@CostPrc", SqlDbType.Float, 9, "CostPrc"));
                            CmdInsExt.Transaction = transaction;
                            DaPR2.InsertCommand = CmdInsExt;
                            DaPR2.Update(TmpDs2, "NewExtended");

                        }

                    }


                    bool InsertWorkFlow = AddWorkFlowLog(14, ProdInfoHF.prepare_year, 1, ProNo, ProdInfoHF.BusUnitID.Value, 1, 0, "-", transaction, cn);
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    transaction.Commit();
                    cn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Edit_ProdOrders(prod_prepare_info ProdInfoHF, List<Prod_prepare_info_detail> ProdInfoDF, List<prod_approve_manufacure> manufacure, List<ProdCost_OrderExtended> OrderExtended)
        {
            DataSet TmpDs = new DataSet();
            DataSet TmpDs1 = new DataSet();
            DataSet TmpDs2 = new DataSet();
            SqlDataAdapter DaPR = new SqlDataAdapter();
            SqlDataAdapter DaPR1 = new SqlDataAdapter();
            SqlDataAdapter DaPR2 = new SqlDataAdapter();
            DataRow DrTmp;
            DataRow DrTmp1;
            DataRow DrTmp2;
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                DaPR.SelectCommand = new SqlCommand("select comp_no, prepare_year, prepare_code, item_no, RevisionNo, rawmaterial_no, UnitSerial, iqty, rqty, NoteDet from Prod_prepare_info_detail where Comp_No = 0", cn);
                DaPR.Fill(TmpDs, "ProdDF");

                DaPR1.SelectCommand = new SqlCommand("SELECT prod_approve_manufacure.*  FROM prod_approve_manufacure WHERE (((prod_approve_manufacure.comp_no)=0))", cn);
                DaPR1.Fill(TmpDs1, "ProdStages");

                DaPR2.SelectCommand = new SqlCommand("SELECT * FROM ProdCost_OrderExtended WHERE compno = 0", cn);
                DaPR2.Fill(TmpDs2, "NewExtended");

                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "ProdCost_DelProdOrder";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = ProdInfoHF.comp_no;
                    cmddel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = ProdInfoHF.prepare_year;
                    cmddel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.SmallInt, 2)).Value = ProdInfoHF.prepare_code;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }

                using (SqlCommand CmdH = new SqlCommand())
                {
                    CmdH.Connection = cn;
                    CmdH.CommandText = "ProdCost_AddProdPrepareHF";
                    CmdH.CommandType = CommandType.StoredProcedure;
                    CmdH.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = ProdInfoHF.comp_no;
                    CmdH.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = ProdInfoHF.prepare_year;
                    CmdH.Parameters.Add(new SqlParameter("@PrepCode", SqlDbType.Int)).Value = ProdInfoHF.prepare_code;
                    CmdH.Parameters.Add(new SqlParameter("@PrepDate", SqlDbType.SmallDateTime)).Value = ProdInfoHF.prepare_date;
                    CmdH.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ProdInfoHF.item_no;
                    CmdH.Parameters.Add(new SqlParameter("@FormCode", SqlDbType.VarChar)).Value = ProdInfoHF.formula_code;
                    CmdH.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float)).Value = ProdInfoHF.qty_prepare;
                    CmdH.Parameters.Add(new SqlParameter("@CUser", SqlDbType.VarChar)).Value = me.UserID;
                    CmdH.Parameters.Add(new SqlParameter("@PlanYear", SqlDbType.SmallInt)).Value = ProdInfoHF.PlanYear;
                    CmdH.Parameters.Add(new SqlParameter("@PlanMonth", SqlDbType.SmallInt)).Value = ProdInfoHF.PlanMonth;
                    CmdH.Parameters.Add(new SqlParameter("@PlanDay", SqlDbType.SmallInt)).Value = DateTime.Now.Day;
                    CmdH.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.SmallInt)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@RefNo1", SqlDbType.VarChar, 100)).Value = ProdInfoHF.RefNo1 ?? "";
                    CmdH.Parameters.Add(new SqlParameter("@RefNo2", SqlDbType.VarChar, 100)).Value = ProdInfoHF.RefNo2 ?? "";
                    if (ProdInfoHF.waste == null)
                    {
                        ProdInfoHF.waste = 0;
                    }
                    CmdH.Parameters.Add(new SqlParameter("@Waste", SqlDbType.Money)).Value = ProdInfoHF.waste;
                    CmdH.Parameters.Add(new SqlParameter("@orderno", SqlDbType.Int)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@FrmStat", SqlDbType.SmallInt)).Value = 1;
                    CmdH.Parameters.Add(new SqlParameter("@ProNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    if (ProdInfoHF.Note == null)
                    {
                        ProdInfoHF.Note = "";
                    }
                    CmdH.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar)).Value = ProdInfoHF.Note;
                    CmdH.Parameters.Add(new SqlParameter("@CustNo", SqlDbType.BigInt)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@ExCost", SqlDbType.Money)).Value = 0;
                    CmdH.Parameters.Add(new SqlParameter("@ExCostReason", SqlDbType.VarChar, 50)).Value = "";
                    CmdH.Parameters.Add(new SqlParameter("@PersianDate", SqlDbType.VarChar, 10)).Value = ProdInfoHF.prepare_date;
                    CmdH.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = ProdInfoHF.BusUnitID;
                    CmdH.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = ProdInfoHF.StoreNo;
                    CmdH.Parameters.Add(new SqlParameter("@Actual_SG", SqlDbType.Money)).Value = ProdInfoHF.Actual_SG;
                    CmdH.Parameters.Add(new SqlParameter("@SalesOrderY", SqlDbType.SmallInt)).Value = ProdInfoHF.SalesOrderY;
                    CmdH.Parameters.Add(new SqlParameter("@SalesOrderN", SqlDbType.Int)).Value = 0;
                    CmdH.Transaction = transaction;
                    try
                    {
                        CmdH.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        var xxx = ex.Message;
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    foreach (Prod_prepare_info_detail item in ProdInfoDF)
                    {
                        DrTmp = TmpDs.Tables["ProdDF"].NewRow();
                        DrTmp.BeginEdit();
                        DrTmp["comp_No"] = item.comp_no;
                        DrTmp["prepare_year"] = item.prepare_year;
                        DrTmp["prepare_code"] = item.prepare_code;
                        DrTmp["item_no"] = item.item_no;
                        DrTmp["RevisionNo"] = item.RevisionNo;
                        DrTmp["rawmaterial_no"] = item.rawmaterial_no;
                        DrTmp["UnitSerial"] = item.UnitSerial;
                        DrTmp["Iqty"] = item.iqty;
                        DrTmp["RQty"] = item.rqty;
                        DrTmp["NoteDet"] = item.NoteDet;
                        DrTmp.EndEdit();
                        TmpDs.Tables["ProdDF"].Rows.Add(DrTmp);
                    }
                    using (SqlCommand CmdIns = new SqlCommand())
                    {
                        CmdIns.Connection = cn;
                        CmdIns.CommandText = "ProdCost_AddProdPrepareDF";
                        CmdIns.CommandType = CommandType.StoredProcedure;
                        CmdIns.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "comp_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt, 4, "prepare_year"));
                        CmdIns.Parameters.Add(new SqlParameter("@PrepCode", SqlDbType.Int, 8, "prepare_code"));
                        CmdIns.Parameters.Add(new SqlParameter("@FromCode", SqlDbType.VarChar, 20, "item_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@RevisionNo", SqlDbType.VarChar, 100, "RevisionNo"));
                        CmdIns.Parameters.Add(new SqlParameter("@ItemRm", SqlDbType.VarChar, 20, "rawmaterial_no"));
                        CmdIns.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdIns.Parameters.Add(new SqlParameter("@IQty", SqlDbType.Float, 9, "IQty"));
                        CmdIns.Parameters.Add(new SqlParameter("@RQty", SqlDbType.Float, 9, "rqty"));
                        CmdIns.Parameters.Add(new SqlParameter("@NoteDet", SqlDbType.VarChar, 300, "NoteDet"));
                        CmdIns.Transaction = transaction;
                        DaPR.InsertCommand = CmdIns;
                        DaPR.Update(TmpDs, "ProdDF");
                    }

                    if (manufacure != null)
                    {
                        int i = 1;
                        foreach (prod_approve_manufacure item in manufacure)
                        {
                            DrTmp1 = TmpDs1.Tables["ProdStages"].NewRow();
                            DrTmp1.BeginEdit();
                            DrTmp1["comp_No"] = item.comp_no;
                            DrTmp1["prepare_year"] = item.prepare_year;
                            DrTmp1["prepare_code"] = item.prepare_code;
                            DrTmp1["hiring_no"] = 1;
                            DrTmp1["stage_code"] = item.stage_code;
                            DrTmp1["stageSer"] = i;
                            DrTmp1["item_no"] = item.item_no;
                            DrTmp1["begin_date"] = item.begin_date;
                            DrTmp1["Closed_Date"] = item.Closed_Date;
                            DrTmp1["CalcTime"] = item.CalcTime;
                            DrTmp1.EndEdit();
                            TmpDs1.Tables["ProdStages"].Rows.Add(DrTmp1);
                            i++;
                        }

                        using (SqlCommand CmdIns1 = new SqlCommand())
                        {
                            CmdIns1.Connection = cn;
                            CmdIns1.CommandText = "ProdCost_AppProdAproveStages";
                            CmdIns1.CommandType = CommandType.StoredProcedure;
                            CmdIns1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "comp_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "prepare_year"));
                            CmdIns1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "prepare_code"));
                            CmdIns1.Parameters.Add(new SqlParameter("@BatchNo", SqlDbType.Int, 9, "hiring_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.VarChar, 5, "stage_code"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Stageser", SqlDbType.SmallInt, 5, "stageSer"));
                            CmdIns1.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20, "item_no"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.SmallDateTime, 8, "begin_date"));
                            CmdIns1.Parameters.Add(new SqlParameter("@CloseDate", SqlDbType.SmallDateTime, 8, "Closed_Date"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Real_date_begining", SqlDbType.SmallDateTime, 8, "Real_date_begining"));
                            CmdIns1.Parameters.Add(new SqlParameter("@Closed_Time", SqlDbType.SmallDateTime, 8, "Closed_Time"));
                            CmdIns1.Parameters.Add(new SqlParameter("@StageStat", SqlDbType.Bit, 1, "Stage_Status"));
                            CmdIns1.Parameters.Add(new SqlParameter("@WorkedHour", SqlDbType.SmallInt, 1, "WorkedHour"));
                            CmdIns1.Parameters.Add(new SqlParameter("@workedmin", SqlDbType.SmallInt, 1, "workedmin"));
                            CmdIns1.Parameters.Add(new SqlParameter("@CalcTime", SqlDbType.Money, 8, "CalcTime"));
                            CmdIns1.Transaction = transaction;
                            DaPR1.InsertCommand = CmdIns1;
                            DaPR1.Update(TmpDs1, "ProdStages");
                        }

                    }

                    if (OrderExtended != null)
                    {
                        foreach (ProdCost_OrderExtended item in OrderExtended)
                        {
                            DrTmp2 = TmpDs2.Tables["NewExtended"].NewRow();
                            DrTmp2.BeginEdit();
                            DrTmp2["compno"] = item.CompNo;
                            DrTmp2["OrderYear"] = item.OrderYear;
                            DrTmp2["OrderNo"] = item.OrderNo;
                            DrTmp2["item_no"] = item.item_no;
                            DrTmp2["formula_code"] = item.formula_code;
                            DrTmp2["Qty"] = item.Qty;
                            DrTmp2["CostPrc"] = item.CostPrc;
                            DrTmp2.EndEdit();
                            TmpDs2.Tables["NewExtended"].Rows.Add(DrTmp2);
                        }

                        using (SqlCommand CmdInsExt = new SqlCommand())
                        {
                            CmdInsExt.Connection = cn;
                            CmdInsExt.CommandText = "ProdCost_AddPrepExtend";
                            CmdInsExt.CommandType = CommandType.StoredProcedure;
                            CmdInsExt.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2, "compno"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2, "OrderYear"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 4, "OrderNo"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "item_no"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@FormulaCode", SqlDbType.VarChar, 20, "formula_code"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Float, 9, "Qty"));
                            CmdInsExt.Parameters.Add(new SqlParameter("@CostPrc", SqlDbType.Float, 9, "CostPrc"));
                            CmdInsExt.Transaction = transaction;
                            DaPR2.InsertCommand = CmdInsExt;
                            DaPR2.Update(TmpDs2, "NewExtended");

                        }

                    }


                    bool InsertWorkFlow = AddWorkFlowLog(14, ProdInfoHF.prepare_year, 1, ProdInfoHF.prepare_code, ProdInfoHF.BusUnitID.Value, 2, 0, "-", transaction, cn);
                    if (InsertWorkFlow == false)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }

                    transaction.Commit();
                    cn.Dispose();
                }
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        private string GetRefrenceNo(short compNo, string rmCode)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.ConnectionString()))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT RefrenceNo FROM InvItemsMF WHERE CompNo = @CompNo AND ItemNo = @ItemNo";
                    sqlCommand.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = (object)compNo;
                    sqlCommand.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = (object)rmCode;
                    return sqlCommand.ExecuteScalar()?.ToString();
                }
            }
        }

      
        public ActionResult GetBatch(string itemNo)
        {
            // Assign the item number to the ViewBag to make it accessible in the view
            ViewBag.ItemNo = itemNo;

            // Render a partial view, passing the necessary data (if any)
            return PartialView();
        }

        public JsonResult Delete_ProdOrders(short comp_no, int prepareyear, int preparecode)
        {
            int count = db.prod_hiring_prepare_info.Where(x => x.comp_no == comp_no && x.prepare_year == prepareyear
                                                          && x.prepare_code == preparecode).Count();
            if (count == 1)
            {
                return Json(new { error = Resources.Resource.errorDelprepare_info }, JsonRequestBehavior.AllowGet);
            }
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();
                using (SqlCommand cmddel = new SqlCommand())
                {
                    cmddel.Connection = cn;
                    cmddel.CommandText = "ProdCost_DelProdOrder";
                    cmddel.CommandType = CommandType.StoredProcedure;
                    cmddel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = comp_no;
                    cmddel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = prepareyear;
                    cmddel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.SmallInt, 2)).Value = preparecode;
                    transaction = cn.BeginTransaction();
                    cmddel.Transaction = transaction;
                    cmddel.ExecuteNonQuery();
                }
                transaction.Commit();
                cn.Dispose();
            }
            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }
        public bool AddWorkFlowLog(int FID, int TrYear, int TrType, int TrNo, int BU, int FStat, double TrAmnt, string TrDesc, SqlTransaction MyTrans, SqlConnection co)
        {
            using (SqlCommand Cmd = new SqlCommand())
            {
                    Cmd.Connection = co;
                    Cmd.CommandText = "Alpha_AddWorkFlowLog";
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    Cmd.Parameters.Add("@FID", SqlDbType.SmallInt).Value = FID;
                    Cmd.Parameters.Add("@BU", SqlDbType.SmallInt).Value = BU;
                    Cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                    Cmd.Parameters.Add("@K1", SqlDbType.VarChar, 10).Value = TrYear;
                    Cmd.Parameters.Add("@K2", SqlDbType.VarChar, 10).Value = TrType;
                    Cmd.Parameters.Add("@K3", SqlDbType.VarChar, 10).Value = TrNo;
                    Cmd.Parameters.Add("@TrAmount", SqlDbType.Money).Value = TrAmnt;
                    Cmd.Parameters.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                    Cmd.Parameters.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                    Cmd.Parameters.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    Cmd.Transaction = MyTrans;
                try
                {
                    Cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    string zz = ex.Message;
                    return false;
                }
            }
            return true;
        }
        public ActionResult QtyAllStoer(string Itemno, int unit)
        {
            DataTable Dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Invt_GetStoresQty";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = Itemno;
                    cmd.Parameters.Add(new SqlParameter("@RptUnit", SqlDbType.SmallInt, 2)).Value = unit;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt, 2)).Value = 1;
                    }
                    cmd.Parameters.Add(new SqlParameter("@PrintZero", SqlDbType.Bit, 2)).Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar, 8)).Value = me.UserID;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }
            IList<Itemstore> Item_store = Dt.AsEnumerable().Select(row => new Itemstore
            {
                StoreNo = row.Field<int>("StoreNo"),
                StoreName = row.Field<string>("StoreName"),
                StoreQty = Convert.ToDouble(row.Field<decimal>("StoreQty")),
                UnitC4 = row.Field<string>("UnitC4")
            }).ToList();

            return PartialView(Item_store);
        }
        public class RMItems
        {
            public string RmCode { get; set; }
            public string ItemDesc { get; set; }
            public Byte UnitSerial { get; set; }
            public double RmQty { get; set; }
            public decimal TotalQty { get; set; }
            public double Shortage { get; set; }
            public int count { get; set; }
            public int staus { get; set; }
            public string RefrenceNo { get; set; }
        }
        public class BatchOrder
        {
            public string BatchNo { get; set; }
        }
        public class ItemStages
        {
            public int Stage_code { get; set; }
            public string stage_desc { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public double CalcTime { get; set; }
            public int staus { get; set; }

        }
        public class ItemProduct
        {
            public string item_no { get; set; }
            public string ItemDesc { get; set; }
            public string IUnit { get; set; }
            public string Categ { get; set; }
            public double Qty { get; set; }
            public double CostPrc { get; set; }
            public decimal AvlQty { get; set; }
        }
        public class Itemstore
        {
            public int StoreNo { get; set; }
            public string StoreName { get; set; }
            public double StoreQty { get; set; }
            public string UnitC4 { get; set; }
        }
        public class Formula
        {
            public string Formula_code { get; set; }
            public string Formula_Desc { get; set; }
            public string Item_no { get; set; }
            public string Item_Desc { get; set; }
        }
    }
}