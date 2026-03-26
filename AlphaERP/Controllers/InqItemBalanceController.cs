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
    public class InqItemBalanceController : controller
    {
        // GET: InqItemBalance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult InqItemBal()
        {
            return PartialView();
        }
        public JsonResult GetStoreName(int StoreNo)
        {
            string StoreName = "";
            InvStoresMF StoresMF = db.InvStoresMFs.Where(x => x.CompNo == company.comp_num && x.StoreNo == StoreNo).FirstOrDefault();
            if(StoresMF == null)
            {
                StoreName = "";
            }
            else
            {
                if (Language == "ar-JO")
                {
                    StoreName = StoresMF.StoreName;
                }
                else
                {
                    StoreName = StoresMF.StoreNameEng;
                }
            }

            return Json(new { StoreName = StoreName }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InqItemBalanceItemsList(int StoreNo)
        {
            ViewBag.StoreNo = StoreNo;
            return PartialView();
        }

        public JsonResult GetItemName(string ItemNo)
        {
            string ItemDesc = "";
            InvItemsMF ItemsMf = db.InvItemsMFs.Where(x => x.CompNo == company.comp_num && x.ItemNo == ItemNo).FirstOrDefault();
            if (ItemsMf == null)
            {
                ItemDesc = "";
            }
            else
            {
                if (Language == "ar-JO")
                {
                    ItemDesc = ItemsMf.ItemDesc;
                }
                else
                {
                    ItemDesc = ItemsMf.ItemDesc_Ara;
                }
            }

            return Json(new { ItemDesc = ItemDesc }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InfoItem(int StoreNo, string ItemNo)
        {
            ViewBag.StoreNo = StoreNo;
            ViewBag.ItemNo = ItemNo;
            int Lang = 1;
            if (Language == "en")
            {
                Lang = 2;
            }

               InfoItems IItem = new MDB().Database.SqlQuery<InfoItems>(string.Format("SELECT InvItemsMF.ItemNo, CASE WHEN {3} = 1 THEN InvItemsMF.ItemDesc ELSE InvItemsMF.ItemDesc_Ara END AS ItemDesc,InvItemsMF.SellPrice_1,  " +
              "CASE WHEN {3} = 1 THEN dbo.InvCategCodes.CategName ELSE dbo.InvCategCodes.CategNameEng END AS CatName, InvItemsMF.RefrenceNo, InvItemsMF.Conv2, InvItemsMF.UnitC2,  " +
              "CASE WHEN {3} = 1 THEN dbo.Invt_SubCategInfo.SubCategName ELSE dbo.Invt_SubCategInfo.SubCategNameEng END AS SubCatName, InvSItemsMF.BinLoc,  " +
              "InvItemsMF.ItemUnitCost AS ItemUnitCost,ISNULL(InvItemsMF.ValidityInMonth, 0) AS ValidityInMonth, isnull(InvItemsMF.UseExpiry,0) as UseExpiry  " +
              "FROM dbo.InvItemsMF INNER JOIN InvSItemsMF ON InvItemsMF.CompNo = InvSItemsMF.CompNo AND InvItemsMF.ItemNo = InvSItemsMF.ItemNo INNER JOIN  " +
              "InvCategCodes ON InvItemsMF.CompNo = InvCategCodes.CompNo AND InvItemsMF.Categ = InvCategCodes.Categ INNER JOIN  " +
              "Invt_SubCategInfo ON InvItemsMF.CompNo = Invt_SubCategInfo.CompNo AND InvItemsMF.Categ = Invt_SubCategInfo.Categ AND InvItemsMF.SubCateg = Invt_SubCategInfo.SubCateg  " +
              "WHERE (dbo.InvItemsMF.CompNo = '{0}') AND (dbo.InvItemsMF.ItemNo = '{1}') AND (dbo.InvSItemsMF.StoreNo = '{2}') ", company.comp_num, ItemNo, StoreNo, Lang)).FirstOrDefault();
            
            return PartialView(IItem);
        }

        public ActionResult AllStoresQty(int StoreNo, string ItemNo, int UnitSerial,bool PrintZero)
        {
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Invt_GetItemBatchs";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int)).Value = StoreNo;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ItemNo;
                    cmd.Parameters.Add(new SqlParameter("@PrintZero", SqlDbType.Bit)).Value = PrintZero;
                    cmd.Parameters.Add(new SqlParameter("@RptUnit", SqlDbType.TinyInt)).Value = UnitSerial;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<AllStores> LAllStores = Dt.AsEnumerable().Select(row => new AllStores
            {
                BatchNo = row.Field<string>("BatchNo"),
                ManDate = row.Field<DateTime>("ManDate"),
                ExpDate = row.Field<DateTime>("ExpDate"),
                IsHalt = row.Field<bool>("IsHalt"),
                QtyOh = row.Field<decimal>("QtyOh"),
                TotQtyRes = row.Field<decimal>("TotQtyRes"),
                RefNo = row.Field<string>("RefNo"),
                UnitC4 = row.Field<string>("UnitC4"),
                UnitCost = row.Field<double>("UnitCost")
            }).ToList();


            
            
            return PartialView(LAllStores);
        }

        public ActionResult StoresQty(string ItemNo, int UnitSerial, bool PrintZero)
        {
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Invt_GetStoresQty";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ItemNo;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt)).Value = 1;
                    }
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)).Value = me.UserID;
                    cmd.Parameters.Add(new SqlParameter("@RptUnit", SqlDbType.TinyInt)).Value = UnitSerial;
                    cmd.Parameters.Add(new SqlParameter("@PrintZero", SqlDbType.Bit)).Value = PrintZero;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<AllStores> LAllStores = Dt.AsEnumerable().Select(row => new AllStores
            {
                StoreNo = row.Field<int>("StoreNo"),
                StoreName = row.Field<string>("StoreName"),
                StoreQty = row.Field<decimal>("StoreQty"),
                TotQtyRes = row.Field<decimal>("TotQtyRes"),
                UnitC4 = row.Field<string>("UnitC4")
            }).ToList();


            return PartialView(LAllStores);
        }
        public ActionResult AlternativeItems(string ItemNo, int UnitSerial, bool PrintZero)
        {
            DataTable Dt = new DataTable();
            using (var conn = new SqlConnection(ConnectionString()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Invt_GetAlternativeItemsQty";
                    cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ItemNo;
                    if (Language == "en")
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt)).Value = 2;
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt)).Value = 1;
                    }
                    cmd.Parameters.Add(new SqlParameter("@RptUnit", SqlDbType.TinyInt)).Value = UnitSerial;
                    cmd.Parameters.Add(new SqlParameter("@PrintZero", SqlDbType.Bit)).Value = PrintZero;
                    using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                    {
                        DA.Fill(Dt);
                    }
                }
            }

            IList<AllStores> LAllStores = Dt.AsEnumerable().Select(row => new AllStores
            {
                AlternativeItemNo = row.Field<string>("AlternativeItemNo"),
                ItemDesc = row.Field<string>("ItemDesc"),
                StoreQty = row.Field<decimal>("StoreQty"),
                TotQtyRes = row.Field<decimal>("TotQtyRes"),
                UnitC4 = row.Field<string>("UnitC4")
            }).ToList();


            return PartialView(LAllStores);
        }
    }
}