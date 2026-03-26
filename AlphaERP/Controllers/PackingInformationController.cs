using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic;
using AlphaERP.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace AlphaERP.Controllers
{
    public class PackingInformationController : controller
    {

        int OrdNo = 0;
        private DataSet DSPack = new DataSet();
        private SqlDataAdapter DaCmd = new SqlDataAdapter();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        public JsonResult GetOrdPackReqSr(int OrderYear, int OrderNo, short serial)
        {
            int SR = serial;
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();

            if (SR == 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "ProdCost_GetOrdPackReqSr";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@NewSr", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                SR = Convert.ToInt32(cmd.Parameters["@NewSr"].Value);
                serial = (short)SR;
            }
            SqlDataReader CmdRd;
            SqlCommand cmd2 = new SqlCommand();
            cmd2.Connection = cn;
            cmd2.CommandText = "ProdCost_GetPrepInfo";
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
            cmd2.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
            cmd2.Parameters.Add(new SqlParameter("@PrepCode", SqlDbType.Int)).Value = OrderNo;
            cmd2.Parameters.Add(new SqlParameter("@PrepAval", SqlDbType.Bit)).Direction = ParameterDirection.Output;
            cmd2.ExecuteNonQuery();
            if (!Convert.ToBoolean(cmd2.Parameters["@PrepAval"].Value))
            {

                return Json(new { error = Translate("السجل غير موجود", "The record does not exist") }, JsonRequestBehavior.AllowGet);
            }

            CmdRd = cmd2.ExecuteReader();
            CmdRd.Read();
            string FinItemCode = Convert.ToString(CmdRd["item_no"]);
            string FormCode = Convert.ToString(CmdRd["formula_code"]);
            string Qty = Convert.ToString(CmdRd["qty_prepare"]);
            string CalculatedSG = Convert.ToString(Interaction.IIf(Information.IsDBNull(CmdRd["Calculated_SG"]), 0, CmdRd["Calculated_SG"]));
            string Actual_SG = Convert.ToString(Interaction.IIf(Information.IsDBNull(CmdRd["Actual_SG"]), 0, CmdRd["Actual_SG"]));
            if (Actual_SG == "0")
            {
                Actual_SG = CalculatedSG;
            }
            string ToalReqQty = Convert.ToString(Interaction.IIf(Information.IsDBNull(CmdRd["TotalReqQty"]), 0, CmdRd["TotalReqQty"]));



            CmdRd.Close();
            string FormDesc = GetFormDEsc(FormCode, cn);
            string FinItem = GetItemsDesc(FinItemCode, cn);
            List<BOMPackInfo> Items = GetItems(OrderYear, OrderNo, FormCode, serial, cn);
            cn.Close();
            EventArgs i = null;
            return Json(new { Items, FinItem, FormDesc, SR, FinItemCode, FormCode, Qty, CalculatedSG, Actual_SG, ToalReqQty }, JsonRequestBehavior.AllowGet);
            // if (FrmStat > 1)
            // {
            //     var Cmd2 = new SqlCommand();
            //     SqlDataReader CmdRd2;
            //     Cmd2.Connection = cn;
            //     Cmd2.CommandText = "ProdCost_GetPackingRequestInfo";
            //     Cmd2.CommandType = CommandType.StoredProcedure;
            //     Cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
            //     Cmd2.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
            //     Cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
            //     Cmd2.Parameters.Add(new SqlParameter("@Serial", SqlDbType.SmallInt)).Value = SR;
            //     Cmd2.ExecuteNonQuery();
            //     CmdRd2 = Cmd2.ExecuteReader();
            //     while (CmdRd2.Read())
            //     {
            //         string IssueOrderYr = Convert.ToString(Interaction.IIf(Information.IsDBNull(CmdRd2["IssueOrderYr"]), 0, CmdRd2["IssueOrderYr"]));
            //         string IssueOrderNo = Convert.ToString(Interaction.IIf(Information.IsDBNull(CmdRd2["IssueOrderNo"]), 0, CmdRd2["IssueOrderNo"]));
            //     }
            //
            //     CmdRd2.Close();
            // }



        }
        public string GetFormDEsc(string FormCode, SqlConnection cn)
        {
            string GetFormDEscRet = "";
            GetFormDEscRet = "";
            var cmd = new SqlCommand();
            SqlDataReader CmdRead;
            var i = default(int);
            cmd.Connection = cn;
            cmd.CommandText = "select formula_code,formula_desc, Item_no, susflag, Calculated_SG from Prod_Formula_header_info where  comp_no=" + company.comp_num + " and formula_code='" + FormCode + "'";
            cmd.CommandType = CommandType.Text;
            CmdRead = cmd.ExecuteReader();
            while (CmdRead.Read())
            {
                i = i + 1;
                GetFormDEscRet = Convert.ToString(CmdRead["formula_desc"]);
                //gFinCode = Convert.ToString(CmdRead["Item_no"]);
                //gSusFormula = Convert.ToBoolean(Interaction.IIf(Information.IsDBNull(CmdRead["susflag"]), false, CmdRead["susflag"]));
                //gCalculated_SG = Convert.ToDouble(Interaction.IIf(Information.IsDBNull(CmdRead["Calculated_SG"]), false, CmdRead["Calculated_SG"]));
            }

            if (i == 0)
            {
                GetFormDEscRet = " ";
                //gFinCode = null;
            }

            CmdRead.Close();
            return GetFormDEscRet;
        }
        private string GetItemsDesc(string ItemNo, SqlConnection cn)
        {
            var Cmd = new SqlCommand();
            SqlDataReader DRead;
            // gSusItem = false;
            string Item_Desc = "";
            // Item_Unit = "";
            // Item_Categ = "";
            Cmd.Connection = cn;
            Cmd.CommandText = "Ma_GetInvItemDesc";
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
            Cmd.Parameters.Add(new SqlParameter("@RptLang", SqlDbType.SmallInt)).Value = 1;
            Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar)).Value = ItemNo;
            DRead = Cmd.ExecuteReader();
            while (DRead.Read())
            {
                Item_Desc = Convert.ToString(DRead["ItemDesc"]);
                string Item_Unit = Convert.ToString(DRead["IUnit"]);
                string Item_Categ = Convert.ToString(DRead["Categ"]);
                bool gSusItem = Convert.ToBoolean(Interaction.IIf(Information.IsDBNull(DRead["IsHalt"]), 0, DRead["IsHalt"]));
            }

            DRead.Close();
            return Item_Desc;
        }
        private List<BOMPackInfo> GetItems(int OrderYear, int OrderNo, string FormCode, short serial, SqlConnection cn)
        {
            DataTable Dt = new DataTable();
            List<BOMPackInfo> infos = new List<BOMPackInfo>();
            var cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.CommandText = "ProdCost_LoadBOMPackInfo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
            cmd.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = FormCode;
            cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
            cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
            using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
            {
                DA.Fill(Dt);
            }
            infos = Dt.AsEnumerable().Select(row => new BOMPackInfo
            {
                CompNo = row.Field<short>("CompNo"),
                PackItem = row.Field<string>("PackItem"),
                FinItem = row.Field<string>("FinItem"),
                Qty = row.Field<int>("Qty"),
                FillQty = row.Field<int>("FillQty"),
                Capacity = row.Field<double>("Capacity"),
                TotalQty = row.Field<decimal>("TotalQty"),
                FinItemDesc = row.Field<string>("FinItemDesc"),
                LeadTime = row.Field<short>("LeadTime"),
                AvlQty = row.Field<decimal>("AvlQty"),
                PK_desc = row.Field<string>("PK_desc"),
                IsLiter = row.Field<bool>("IsLiter"),
                IssuedQty = row.Field<decimal>("IssuedQty"),
                TotReqQty = row.Field<decimal>("TotReqQty"),
                OrgQty = row.Field<int>("OrgQty")
            }).ToList();

            if (infos.Count == 0)
            {
                cmd.Parameters.Clear();
                cmd.Connection = cn;
                cmd.CommandText = "ProdCost_LoadOrderPackRequest";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@SR", SqlDbType.SmallInt)).Value = serial - 1;
                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {
                    DA.Fill(Dt);
                }
                foreach (DataRow row in Dt.Rows)
                {
                    BOMPackInfo v = new BOMPackInfo();
                    v.CompNo = row.Field<short>("CompNo");
                    v.PackItem = row.Field<string>("PackItem");
                    v.FinItem = row.Field<string>("FinItem");
                    v.Qty = Convert.ToDecimal(row["Qty"]);
                    v.FillQty = Convert.ToDecimal(row["FillQty"]);
                    v.Capacity = row.Field<double>("Capacity");
                    v.TotalQty = Convert.ToDecimal(row["TotalQty"]);
                    v.FinItemDesc = row.Field<string>("FinItemDesc");
                    v.LeadTime = row.Field<short>("LeadTime");
                    v.AvlQty = Convert.ToDecimal(row["AvlQty"]);
                    v.PK_desc = row.Field<string>("PK_desc");
                    v.IsLiter = row.Field<bool>("IsLiter");
                    v.IssuedQty = Convert.ToDecimal(row["IssuedQty"]);
                    v.TotReqQty = Convert.ToDecimal(row["TotReqQty"]);
                    v.OrgQty = Convert.ToDecimal(row["OrgQty"]);
                    infos.Add(v);
                }

            }


            return infos;
        }
        public partial class BOMPackInfo
        {
            public short CompNo { get; set; }
            public string PackItem { get; set; }
            public string FinItem { get; set; }
            public decimal Qty { get; set; }
            public decimal FillQty { get; set; }
            public double Capacity { get; set; }
            public decimal TotalQty { get; set; }
            public string FinItemDesc { get; set; }
            public short LeadTime { get; set; }
            public decimal AvlQty { get; set; }
            public string PK_desc { get; set; }
            public bool IsLiter { get; set; }
            public decimal IssuedQty { get; set; }
            public decimal TotReqQty { get; set; }
            public decimal OrgQty { get; set; }
        }
        public void LoadOrderPackingInfo(SqlTransaction MyTrans, SqlConnection cn, int OrderYear, int OrderNo, string FormCode, int FStat, int SR)
        {
            DSPack = new DataSet();
            var DaCmd = new SqlDataAdapter();
            var cmd = new SqlCommand();
            cmd.Transaction = MyTrans;
            var DaCmd2 = new SqlDataAdapter();
            var cmd2 = new SqlCommand();
            cmd2.Transaction = MyTrans;

            if (FStat == 1)
            {
                cmd.Connection = cn;

                cmd.CommandText = "ProdCost_LoadOrderPackInfo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                DaCmd.SelectCommand = cmd;

                DaCmd.Fill(DSPack, "PackingInfo");
                cmd2.Connection = cn;
                cmd2.CommandText = "ProdCost_LoadBOMPackInfo";
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                cmd2.Parameters.Add(new SqlParameter("@FinProd", SqlDbType.VarChar, 20)).Value = Interaction.IIf(string.IsNullOrEmpty(FormCode), 0, FormCode);
                cmd2.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
                cmd2.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                DaCmd2.SelectCommand = cmd2;
                DaCmd2.Fill(DSPack, "PackingInfo");

            }
            else
            {
                cmd.Connection = cn;
                cmd.CommandText = "ProdCost_LoadOrderPackRequest";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                cmd.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt)).Value = OrderYear;
                cmd.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int)).Value = OrderNo;
                cmd.Parameters.Add(new SqlParameter("@SR", SqlDbType.SmallInt)).Value = SR;
                DaCmd.SelectCommand = cmd;
                DaCmd.Fill(DSPack, "PackingInfo");
                if (DSPack.Tables["PackingInfo"].Rows.Count == 0)
                {
                    return;
                }
            }

        }
        public JsonResult AddRec(int SR, int OrderYear, int OrderNo, DateTime Date, int StoreNo, short BusnUnit, int DeptNo, long AccNo, string FormCode, string FormDesc, List<BOMPackInfo> items_list, string func)
        {

            int OrdNo = 0;
            SqlConnection cn = new SqlConnection(ConnectionString());
            cn.Open();
            SqlTransaction MyTrans;
            MyTrans = cn.BeginTransaction();
            switch (func)
            {
                case "edit":
                    UpdRec(MyTrans, cn, SR, OrderYear, OrderNo, Date, StoreNo, BusnUnit, DeptNo, AccNo, FormCode, FormDesc, items_list);
                    cn.Close();
                    return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
                case "delete":
                    DelRec(MyTrans, cn, SR, OrderYear, OrderNo, Date, StoreNo, BusnUnit, DeptNo, AccNo, FormCode, FormDesc, items_list);
                    cn.Close();
                    return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
                default:
                    LoadOrderPackingInfo(MyTrans, cn, OrderYear, OrderNo, FormCode, 1, SR);
                    GridToDataSet(cn, SR, OrderYear, OrderNo, items_list, MyTrans);
                    var cmdDel = new SqlCommand();
                    cmdDel.Connection = cn;
                    cmdDel.CommandText = "ProdCost_DelOrderPackingInfo";
                    cmdDel.CommandType = CommandType.StoredProcedure;
                    cmdDel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmdDel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    cmdDel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
                    cmdDel.Transaction = MyTrans;
                    cmdDel.ExecuteNonQuery();
                    var cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.Transaction = MyTrans;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ProdCost_PackingRequestHFAdd";
                    {
                        var withBlock = cmd.Parameters;
                        withBlock.AddWithValue("@CompNo", company.comp_num);
                        withBlock.AddWithValue("@OrderYear", OrderYear);
                        withBlock.AddWithValue("@OrderNo", OrderNo);
                        withBlock.AddWithValue("@Serial", SR);
                        withBlock.AddWithValue("@TransDate", Date);
                        withBlock.AddWithValue("@StoreNo", StoreNo);
                        withBlock.AddWithValue("@IssueOrderYr", OrderYear);
                        withBlock.AddWithValue("@IssueOrderNo", OrdNo);
                        withBlock.AddWithValue("@UserID", me.UserID);
                    }

                    cmd.ExecuteNonQuery();
                    if (DSPack.Tables["TmpPack2"].Rows.Count > 0)
                    {
                        var CmdDIns3 = new SqlCommand();
                        CmdDIns3.Connection = cn;
                        CmdDIns3.CommandText = "ProdCost_PackingRequestDFAdd";
                        CmdDIns3.CommandType = CommandType.StoredProcedure;
                        CmdDIns3.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "OrderYear"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@Serial", SqlDbType.SmallInt, 4, "Serial"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@PkItem", SqlDbType.VarChar, 20, "PkItem"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@FGItem", SqlDbType.VarChar, 20, "FGItem"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@UnitCode", SqlDbType.VarChar, 5, "UnitCode"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 4, "Qty"));
                        CmdDIns3.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                        CmdDIns3.Transaction = MyTrans;
                        DaCmd.InsertCommand = CmdDIns3;
                        DaCmd.Update(DSPack, "TmpPack2");
                    }

                    var cmd1 = new SqlCommand();
                    cmd1.Connection = cn;
                    cmd1.CommandText = "ProdCost_AddOrderPackInfo_By_RequestDF";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    cmd1.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
                    cmd1.Transaction = MyTrans;
                    cmd1.ExecuteNonQuery();
                    AddIssueOrder(cn, SR, OrderYear, OrderNo, StoreNo, Date, BusnUnit, DeptNo, AccNo, FormCode, FormDesc, MyTrans);
                    MyTrans.Commit();
                    // ** //   if (ProdCost.Security_Module.gGceClient == 251)
                    // ** //   {
                    // ** //       RunReport(Convert.ToInt32(OrderYear), Convert.ToInt32(OrderNo));
                    // ** //   }

                    //  if (FrmStat == 1 | FrmStat == 2)
                    //  {
                    //      Result = Interaction.MsgBox(ProdCost.PInterface.MsgTxt(18), MsgBoxStyle.YesNo);
                    //      if (Result == MsgBoxResult.Yes)
                    //      {
                    //          RunReportProduction(Convert.ToInt32(OrderYear), OrdNo);
                    //      }
                    //  }


                    // Text = Text + " ------------- " + ProdCost.PInterface.MsgTxt(1086) + " " + OrdNo;
                    // PanelControl(4);
                    // ProdCost.Connection.StateBar(ProdCost.PInterface.MsgTxt(27));

                    cn.Close();
                    return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
                    break;
            }

        }
        public void GridToDataSet(SqlConnection cn, int SR, int OrderYear, int OrderNo, List<BOMPackInfo> items_list, SqlTransaction MyTrans)
        {



            DataRow DrPack2;
            var Cmd = new SqlCommand();

            Cmd.Connection = cn;
            Cmd.CommandText = "SELECT     CompNo, OrderYear, OrderNo, Serial, PKItem, FGItem, UnitCode, Qty, UnitSerial FROM         ProdCost_PackingRequestDF where CompNo= 0 ";
            Cmd.CommandType = CommandType.Text;
            Cmd.Transaction = MyTrans;
            DaCmd.SelectCommand = Cmd;
            DaCmd.Fill(DSPack, "TmpPack2");
            foreach (DataRow Dr in DSPack.Tables["PackingInfo"].Rows)
            {
                if (items_list != null)
                {
                    BOMPackInfo tmp = items_list.Where(x => x.PackItem == Dr["PackItem"].ToString() && x.FinItem == Dr["FinItem"].ToString()).FirstOrDefault();
                    if (null != tmp)
                    {
                        Dr["FillQty"] = tmp.FillQty;
                    }
                }

                if (Dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToBoolean(!Operators.ConditionalCompareObjectEqual(Dr["FillQty"], 0, false)))
                    {
                        DrPack2 = DSPack.Tables["TmpPack2"].NewRow();
                        DrPack2["CompNo"] = company.comp_num;
                        DrPack2["OrderYear"] = OrderYear;
                        DrPack2["OrderNo"] = OrderNo;
                        DrPack2["Serial"] = SR;
                        DrPack2["PKItem"] = Dr["PackItem"];
                        DrPack2["FGItem"] = Dr["FinItem"];
                        DrPack2["UnitCode"] = GetUnitCodeBySerial(cn, Conversions.ToString(Dr["PackItem"]), 4, MyTrans);
                        DrPack2["UnitSerial"] = 4;
                        DrPack2["Qty"] = Dr["FillQty"];
                        DSPack.Tables["TmpPack2"].Rows.Add(DrPack2);
                    }
                }
            }
        }
        private void AddIssueOrder(SqlConnection cn, int SR, int OrderYear, int OrderNo, int StoreNo, DateTime Date, short BusnUnit, int DeptNo, long AccNo, string FormCode, string FormDesc, SqlTransaction MyTrans)
        {
            SqlCommand Cmd;
            try
            {
                Cmd = new SqlCommand();
                {
                    var withBlock = Cmd;
                    withBlock.Connection = cn;
                    withBlock.CommandText = "InvT_AddIssueOrderHF";
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    withBlock.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    withBlock.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = 0;
                    withBlock.Parameters.Add(new SqlParameter("@OrdDate", SqlDbType.SmallDateTime, 4)).Value = Date;
                    withBlock.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 4)).Value = StoreNo;
                    withBlock.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar, 15)).Value = 1;
                    withBlock.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 300)).Value = "";
                    withBlock.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = BusnUnit;
                    Cmd.Parameters.Add(new SqlParameter("@IsProduction", SqlDbType.Bit)).Value = 1;
                    Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
                    Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
                    withBlock.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DeptNo;
                    withBlock.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = AccNo;
                    withBlock.Parameters.Add(new SqlParameter("@vod_act", SqlDbType.VarChar, 10)).Value = "";
                    withBlock.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.Int)).Value = 0;
                    withBlock.Parameters.Add(new SqlParameter("@AddititiveItems", SqlDbType.Bit)).Value = 0;
                    withBlock.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = 1;
                    withBlock.Parameters.Add(new SqlParameter("@ByStage", SqlDbType.Bit)).Value = 0;
                    withBlock.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    withBlock.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    withBlock.Transaction = MyTrans;
                    withBlock.ExecuteNonQuery();
                }

                if (Conversions.ToBoolean(!Operators.ConditionalCompareObjectEqual(Cmd.Parameters["@ErrNo"].Value, 0, false)))
                {
                    MyTrans.Rollback();
                    // ** // ProdCost.PInterface.ErrList(Conversions.ToInteger(Cmd.Parameters["@ErrNo"].Value));
                    return;
                }

                OrdNo = Conversions.ToInteger(Cmd.Parameters["@UsedOrdNo"].Value);
                var DsTmp = new DataSet();
                DataRow DrTmp;
                var Da = new SqlDataAdapter();
                int i = 1;
                var CmdTmp = new SqlCommand();
                var FindPKItem = new object[1];
                DataRow DrItem;
                CmdTmp.Connection = cn;
                CmdTmp.CommandText = "select * from InvT_IssueOrderDF where CompNo=0";
                CmdTmp.CommandType = CommandType.Text;
                CmdTmp.Transaction = MyTrans;
                DaCmd.SelectCommand = CmdTmp;
                DaCmd.Fill(DsTmp, "IssueOrder");
                DsTmp.Tables["IssueOrder"].PrimaryKey = new DataColumn[] { DsTmp.Tables["IssueOrder"].Columns["ItemNo"] };
                foreach (DataRow Dr in DSPack.Tables["PackingInfo"].Select("FillQty<> 0"))
                {
                    if (Dr.RowState != DataRowState.Deleted)
                    {
                        DataRow Dr21;
                        Dr21 = DsTmp.Tables["IssueOrder"].Rows.Find(Dr["PackItem"]);
                        if (Information.IsNothing(Dr21))
                        {
                            DrTmp = DsTmp.Tables["IssueOrder"].NewRow();
                            DrTmp["ItemSer"] = i;
                            DrTmp["ItemNo"] = Dr["PackItem"];
                            DrTmp["UnitCode"] = GetUnitCodeBySerial(cn, Convert.ToString(Dr["PackItem"]), 4, MyTrans);
                            DrTmp["Qty"] = Dr["FillQty"];
                            DrTmp["UnitSerial"] = 4;
                            DsTmp.Tables["IssueOrder"].Rows.Add(DrTmp);
                        }
                        else
                        {
                            Dr21.BeginEdit();
                            Dr21["Qty"] = Convert.ToDouble(Dr21["qty"].ToString()) + Convert.ToDouble(Dr["FillQty"].ToString());
                            Dr21.EndEdit();
                        }

                        i += 1;

                        /// ãáÍÞÇÊ ÇáÊÚÈÆÉ
                        var Cmd_pack = new SqlCommand();
                        Cmd_pack.Connection = cn;
                        Cmd_pack.CommandType = CommandType.Text;
                        Cmd_pack.CommandText = Conversions.ToString(Conversions.ToString("SELECT     BOM_FinPackingInfo.*  FROM BOM_FinPackingInfo WHERE     (CompNo = " + company.comp_num + ") AND (FormCode = '" + FormCode + "') AND (FinItem = '" + Dr["FinItem"] + "') AND (PackItem = '") + Dr["PackItem"] + "')");
                        Cmd_pack.Transaction = MyTrans;
                        DaCmd.SelectCommand = Cmd_pack;
                        DaCmd.Fill(DsTmp, "FinPackingInfo");
                        DsTmp.Tables["FinPackingInfo"].PrimaryKey = new DataColumn[] { DsTmp.Tables["FinPackingInfo"].Columns["RMCode"] };
                        foreach (DataRow dr_pack in DsTmp.Tables["FinPackingInfo"].Rows)
                        {
                            FindPKItem[0] = dr_pack["RMCode"];
                            DrItem = DsTmp.Tables["FinPackingInfo"].Rows.Find(FindPKItem);
                            if (Information.IsNothing(DrItem))
                            {
                                DrTmp = DsTmp.Tables["IssueOrder"].NewRow();
                                DrTmp["ItemSer"] = i;
                                DrTmp["ItemNo"] = dr_pack["RMCode"];
                                DrTmp["UnitCode"] = GetUnitCodeBySerial(cn, Conversions.ToString(dr_pack["RMCode"]), 4, MyTrans);
                                DrTmp["Qty"] = Dr["Qty"];
                                DrTmp["UnitSerial"] = 4;
                                DsTmp.Tables["IssueOrder"].Rows.Add(DrTmp);
                            }
                            else
                            {
                                DrItem.BeginEdit();
                                DrItem["Qty"] = Convert.ToDouble(DrItem["Qty"].ToString()) + Convert.ToDouble(Dr["Qty"].ToString());
                                DrItem.EndEdit();
                            }
                        }
                    }
                }

                Cmd = new SqlCommand();
                {
                    var withBlock1 = Cmd;
                    withBlock1.Connection = cn;
                    withBlock1.CommandText = "InvT_AddIssueOrderDF";
                    withBlock1.CommandType = CommandType.StoredProcedure;
                    withBlock1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    withBlock1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    withBlock1.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrdNo;
                    withBlock1.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                    withBlock1.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 2, "ItemSer"));
                    withBlock1.Parameters.Add(new SqlParameter("@UnitCode", SqlDbType.VarChar, 5, "UnitCode"));
                    withBlock1.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 8, "Qty"));
                    withBlock1.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 2, "UnitSerial"));
                    withBlock1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    withBlock1.Transaction = MyTrans;
                }

                Da.InsertCommand = Cmd;
                Da.Update(DsTmp, "IssueOrder");


                // -- WorkFlow --------------------------------------------
                string WFTxt;
                if (Session["language"].ToString() == "ar-JO")
                {
                    WFTxt = "ØáÈ ãæÇÝÞÉ Úáì ÃãÑ ÕÑÝ ãæÇÏ ÊÚÈÆÉ ÑÞã " + OrdNo + "ãä ÇáãÓÊÎÏã " + me.UserName + " áÇãÑ ÊÌåíÒ ÑÞã  " + OrderNo + "ááãÇÏÉ ÇáÌÇåÒÉ " + FormDesc;
                }
                else
                {
                    WFTxt = "Request approval for the following Packing Items issue order " + OrderNo + " from the user " + me.UserName + " For Production Order  " + OrderNo + " for the finish Item   " + FormDesc;
                }

                AddWorkFlowLog(cn, 6, Conversions.ToInteger(OrderYear), 26, OrdNo, Conversions.ToInteger(BusnUnit), 1, 0.1, "", MyTrans);
            }
            catch (Exception ex)
            {
                MyTrans.Rollback();
                Interaction.MsgBox(ex.Message);
                return;
            }
        }
        public string GetUnitCodeBySerial(SqlConnection cn, string ItemNo, int UnitSerial, SqlTransaction MyTrans)
        {
            using (var Cmd = new SqlCommand())
            {
                Cmd.Connection = cn;
                Cmd.CommandText = "InvT_GetUnitBySerial";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20)).Value = ItemNo;
                Cmd.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.TinyInt)).Value = UnitSerial;
                Cmd.Transaction = MyTrans;
                return Conversions.ToString(Cmd.ExecuteScalar());
            }
        }
        public JsonResult GetYearList(short year)
        {
            List<int> ints = new List<int>();
            string query0 = string.Format("select DISTINCT ProdCost_PackingRequestHF.OrderNo  from OrdInvcHF INNER JOIN ProdCost_PackingRequestHF ON OrdInvcHF.CompNo = ProdCost_PackingRequestHF.CompNo WHERE     (ProdCost_PackingRequestHF.CompNo = {0}) AND (ProdCost_PackingRequestHF.OrderYear = {1}) ", company.comp_num, year);
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query0, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ints.Add(Convert.ToInt32(rdr["OrderNo"].ToString()));
                    }
                }
            }
            return Json(new { ints }, JsonRequestBehavior.AllowGet);
        }

        public void UpdRec(SqlTransaction MyTrans, SqlConnection cn, int SR, int OrderYear, int OrderNo, DateTime Date, int StoreNo, short BusnUnit, int DeptNo, long AccNo, string FormCode, string FormDesc, List<BOMPackInfo> items_list)
        {
            LoadOrderPackingInfo(MyTrans, cn, OrderYear, OrderNo, FormCode, 2, SR);
            GridToDataSet(cn, SR, OrderYear, OrderNo, items_list, MyTrans);
            var cmdDel = new SqlCommand();
            cmdDel.Connection = cn;
            cmdDel.CommandText = "ProdCost_DelOrderPackingInfo";
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            cmdDel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            cmdDel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            cmdDel.Transaction = MyTrans;
            cmdDel.ExecuteNonQuery();
            cmdDel = new SqlCommand();
            cmdDel.Connection = cn;
            cmdDel.CommandText = "ProdCost_PackingRequestDelete";
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            cmdDel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            cmdDel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            cmdDel.Parameters.Add(new SqlParameter("@Serial", SqlDbType.SmallInt, 2)).Value = SR;
            cmdDel.Transaction = MyTrans;
            cmdDel.ExecuteNonQuery();
            var cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.Transaction = MyTrans;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ProdCost_PackingRequestHFAdd";
            {
                var withBlock = cmd.Parameters;
                withBlock.AddWithValue("@CompNo", company.comp_num);
                withBlock.AddWithValue("@OrderYear", OrderYear);
                withBlock.AddWithValue("@OrderNo", OrderNo);
                withBlock.AddWithValue("@Serial", SR);
                withBlock.AddWithValue("@TransDate", Date);
                withBlock.AddWithValue("@StoreNo", StoreNo);
                withBlock.AddWithValue("@IssueOrderYr", OrderYear);
                withBlock.AddWithValue("@IssueOrderNo", OrdNo);
                withBlock.AddWithValue("@UserID", me.UserID);
            }

            cmd.ExecuteNonQuery();
            if (DSPack.Tables["TmpPack2"].Rows.Count > 0)
            {
                var CmdDIns3 = new SqlCommand();
                CmdDIns3.Connection = cn;
                CmdDIns3.CommandText = "ProdCost_PackingRequestDFAdd";
                CmdDIns3.CommandType = CommandType.StoredProcedure;
                CmdDIns3.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 4, "CompNo"));
                CmdDIns3.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 4, "OrderYear"));
                CmdDIns3.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 8, "OrderNo"));
                CmdDIns3.Parameters.Add(new SqlParameter("@Serial", SqlDbType.SmallInt, 4, "Serial"));
                CmdDIns3.Parameters.Add(new SqlParameter("@PkItem", SqlDbType.VarChar, 20, "PkItem"));
                CmdDIns3.Parameters.Add(new SqlParameter("@FGItem", SqlDbType.VarChar, 20, "FGItem"));
                CmdDIns3.Parameters.Add(new SqlParameter("@UnitCode", SqlDbType.VarChar, 5, "UnitCode"));
                CmdDIns3.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 4, "Qty"));
                CmdDIns3.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 4, "UnitSerial"));
                CmdDIns3.Transaction = MyTrans;
                DaCmd.InsertCommand = CmdDIns3;
                DaCmd.Update(DSPack, "TmpPack2");
            }

            var cmd1 = new SqlCommand();
            cmd1.Connection = cn;
            cmd1.CommandText = "ProdCost_AddOrderPackInfo_By_RequestDF";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            cmd1.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            cmd1.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            cmd1.Transaction = MyTrans;
            cmd1.ExecuteNonQuery();
            UpdateIssueOrder(MyTrans, cn, SR, OrderYear, OrderNo, Date, StoreNo, BusnUnit, DeptNo, AccNo, FormCode, FormDesc, items_list);
            MyTrans.Commit();



        }

        private void UpdateIssueOrder(SqlTransaction MyTrans, SqlConnection cn, int SR, int OrderYear, int OrderNo, DateTime Date, int StoreNo, short BusnUnit, int DeptNo, long AccNo, string FormCode, string FormDesc, List<BOMPackInfo> items_list)
        {
            if (OrderNo == 0)
                return;
            SqlCommand Cmd;
            try
            {
                Cmd = new SqlCommand();
                {
                    var withBlock = Cmd;
                    withBlock.Connection = cn;
                    withBlock.CommandText = "InvT_DelIssueOrderDF";
                    withBlock.CommandType = CommandType.StoredProcedure;
                    withBlock.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    withBlock.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    withBlock.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrderNo;
                    withBlock.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    withBlock.Transaction = MyTrans;
                    withBlock.ExecuteNonQuery();
                }

                Cmd = new SqlCommand();
                {
                    var withBlock1 = Cmd;
                    withBlock1.Connection = cn;
                    withBlock1.CommandText = "InvT_DelIssueOrderHF";
                    withBlock1.CommandType = CommandType.StoredProcedure;
                    withBlock1.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    withBlock1.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    withBlock1.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrderNo;
                    withBlock1.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    withBlock1.Transaction = MyTrans;
                    withBlock1.ExecuteNonQuery();
                }

                Cmd = new SqlCommand();
                {
                    var withBlock2 = Cmd;
                    withBlock2.Connection = cn;
                    withBlock2.CommandText = "InvT_AddIssueOrderHF";
                    withBlock2.CommandType = CommandType.StoredProcedure;
                    withBlock2.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                    withBlock2.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                    withBlock2.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrderNo;
                    withBlock2.Parameters.Add(new SqlParameter("@OrdDate", SqlDbType.SmallDateTime, 4)).Value = Date.ToShortDateString();
                    withBlock2.Parameters.Add(new SqlParameter("@StoreNo", SqlDbType.Int, 4)).Value = StoreNo;
                    withBlock2.Parameters.Add(new SqlParameter("@DocNo", SqlDbType.VarChar, 15)).Value = 1;
                    withBlock2.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 300)).Value = "";
                    withBlock2.Parameters.Add(new SqlParameter("@BusUnitID", SqlDbType.SmallInt)).Value = BusnUnit;
                    Cmd.Parameters.Add(new SqlParameter("@IsProduction", SqlDbType.Bit)).Value = 1;
                    Cmd.Parameters.Add(new SqlParameter("@PrepYear", SqlDbType.SmallInt)).Value = OrderYear;
                    Cmd.Parameters.Add(new SqlParameter("@PrepNo", SqlDbType.Int)).Value = OrderNo;
                    withBlock2.Parameters.Add(new SqlParameter("@CustDep", SqlDbType.Int)).Value = DeptNo;
                    withBlock2.Parameters.Add(new SqlParameter("@CustAcc", SqlDbType.BigInt)).Value = AccNo;
                    withBlock2.Parameters.Add(new SqlParameter("@vod_act", SqlDbType.VarChar, 10)).Value = "";
                    withBlock2.Parameters.Add(new SqlParameter("@StageCode", SqlDbType.Int)).Value = 0;
                    withBlock2.Parameters.Add(new SqlParameter("@AddititiveItems", SqlDbType.Bit)).Value = 0;
                    withBlock2.Parameters.Add(new SqlParameter("@PackingItems", SqlDbType.Bit)).Value = 1;
                    withBlock2.Parameters.Add(new SqlParameter("@ByStage", SqlDbType.Bit)).Value = 0;
                    withBlock2.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    withBlock2.Parameters.Add(new SqlParameter("@UsedOrdNo", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    withBlock2.Transaction = MyTrans;
                    withBlock2.ExecuteNonQuery();
                }

                if (Conversions.ToBoolean(!Operators.ConditionalCompareObjectEqual(Cmd.Parameters["@ErrNo"].Value, 0, false)))
                {
                    MyTrans.Rollback();
                    //ProdCost.PInterface.ErrList(Conversions.ToInteger(Cmd.Parameters["@ErrNo"].Value));
                    return;
                }

                OrdNo = Conversions.ToInteger(Cmd.Parameters["@UsedOrdNo"].Value);
                var DsTmp = new DataSet();
                DataRow DrTmp;
                var Da = new SqlDataAdapter();
                int i = 1;
                var CmdTmp = new SqlCommand();
                CmdTmp.Connection = cn;
                CmdTmp.CommandText = "select * from InvT_IssueOrderDF where CompNo=0";
                CmdTmp.CommandType = CommandType.Text;
                CmdTmp.Transaction = MyTrans;
                DaCmd.SelectCommand = CmdTmp;
                DaCmd.Fill(DsTmp, "IssueOrder");
                DsTmp.Tables["IssueOrder"].PrimaryKey = new DataColumn[] { DsTmp.Tables["IssueOrder"].Columns["ItemNo"] };
                foreach (var Dr in DSPack.Tables["PackingInfo"].Select("FillQty<> 0"))
                {
                    if (Dr.RowState != DataRowState.Deleted)
                    {
                        DataRow Dr21;
                        Dr21 = DsTmp.Tables["IssueOrder"].Rows.Find(Dr["PackItem"]);
                        if (Information.IsNothing(Dr21))
                        {
                            DrTmp = DsTmp.Tables["IssueOrder"].NewRow();
                            DrTmp["ItemSer"] = i;
                            DrTmp["ItemNo"] = Dr["PackItem"];
                            DrTmp["UnitCode"] = GetUnitCodeBySerial(cn, Conversions.ToString(Dr["PackItem"]), 4, MyTrans);
                            DrTmp["Qty"] = Dr["FillQty"];
                            DrTmp["UnitSerial"] = 4;
                            DsTmp.Tables["IssueOrder"].Rows.Add(DrTmp);
                        }
                        else
                        {
                            Dr21.BeginEdit();
                            Dr21["Qty"] = Convert.ToDouble(Dr21["qty"].ToString()) + Convert.ToDouble(Dr["FillQty"].ToString());
                            Dr21.EndEdit();
                        }

                        i += 1;
                    }
                }

                Cmd = new SqlCommand();
                Cmd.Connection = cn;
                Cmd.CommandText = "InvT_AddIssueOrderDF";
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
                Cmd.Parameters.Add(new SqlParameter("@OrdYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
                Cmd.Parameters.Add(new SqlParameter("@OrdNo", SqlDbType.Int, 4)).Value = OrderNo;
                Cmd.Parameters.Add(new SqlParameter("@ItemNo", SqlDbType.VarChar, 20, "ItemNo"));
                Cmd.Parameters.Add(new SqlParameter("@ItemSer", SqlDbType.SmallInt, 2, "ItemSer"));
                Cmd.Parameters.Add(new SqlParameter("@UnitCode", SqlDbType.VarChar, 5, "UnitCode"));
                Cmd.Parameters.Add(new SqlParameter("@Qty", SqlDbType.Money, 8, "Qty"));
                Cmd.Parameters.Add(new SqlParameter("@UnitSerial", SqlDbType.SmallInt, 2, "UnitSerial"));
                Cmd.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                Cmd.Transaction = MyTrans;
                Da.InsertCommand = Cmd;
                Da.Update(DsTmp, "IssueOrder");
            }


            // -- WorkFlow --------------------------------------------
            // Dim WFTxt As String
            // If gLang = 1 Then
            // WFTxt = "ØáÈ ãæÇÝÞÉ Úáì ÃãÑ ÕÑÝ ãæÇÏ ÊÚÈÆÉ ÑÞã " & OrdNo & "ãä ÇáãÓÊÎÏã " & AppUser & " áÇãÑ ÊÌåíÒ ÑÞã  " & TxtOrderNo.Text & "ááãÇÏÉ ÇáÌÇåÒÉ " & LblFormDesc.Text
            // Else
            // WFTxt = "Request approval for the following Packing Items issue order " & TxtOrderNo.Text & " from the user " & AppUser & " For Production Order  " & TxtOrderNo.Text & " for the finish Item   " & LblFormDesc.Text
            // End If

            // AddWorkFlowLog(6, TxtYear.Text, 26, OrdNo, txtBusnUnit.Text, 1, 0.1, "", MyTrans)

            catch (Exception ex)
            {
                MyTrans.Rollback();
                Interaction.MsgBox(ex.Message);
                return;
            }
        }

        public void DelRec(SqlTransaction MyTrans, SqlConnection cn, int SR, int OrderYear, int OrderNo, DateTime Date, int StoreNo, short BusnUnit, int DeptNo, long AccNo, string FormCode, string FormDesc, List<BOMPackInfo> items_list)
        {
            var cmdDel = new SqlCommand();
            cmdDel = new SqlCommand();
            cmdDel.Connection = cn;
            cmdDel.CommandText = "ProdCost_PackingRequestDelete";
            cmdDel.CommandType = CommandType.StoredProcedure;
            cmdDel.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt, 2)).Value = company.comp_num;
            cmdDel.Parameters.Add(new SqlParameter("@OrderYear", SqlDbType.SmallInt, 2)).Value = OrderYear;
            cmdDel.Parameters.Add(new SqlParameter("@OrderNo", SqlDbType.Int, 9)).Value = OrderNo;
            cmdDel.Parameters.Add(new SqlParameter("@Serial", SqlDbType.SmallInt, 2)).Value = SR;
            cmdDel.Transaction = MyTrans;
            cmdDel.ExecuteNonQuery();
            MyTrans.Commit();
        }

    }
}