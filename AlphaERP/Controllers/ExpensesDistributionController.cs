using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class ExpensesDistributionController : controller
    {
        // GET: ExpensesDistribution
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveExpensesDistribution(List<para> parameters, int StageCode, short DExpYear)
        {
            DataTable Direct = new DataTable();
            DataTable inDirect = new DataTable();
            Direct.Columns.Add("CompNo", typeof(short));
            Direct.Columns.Add("DExpYear", typeof(short));
            Direct.Columns.Add("StageCode", typeof(int));
            Direct.Columns.Add("DExpCode", typeof(short));
            Direct.Columns.Add("DExpValue", typeof(decimal));

            inDirect.Columns.Add("CompNo", typeof(short));
            inDirect.Columns.Add("DExpYear", typeof(short));
            inDirect.Columns.Add("StageCode", typeof(int));
            inDirect.Columns.Add("DExpCode", typeof(short));
            inDirect.Columns.Add("DExpValue", typeof(decimal));

            foreach (var parameter in parameters.Where(x => x.DExpValue > 0 && x.Direct))
            {
                Direct.Rows.Add(company.comp_num, DExpYear, StageCode, parameter.DExpCode, parameter.DExpValue);
            }
            foreach (var parameter in parameters.Where(x => x.DExpValue > 0 && !x.Direct))
            {
                inDirect.Rows.Add(company.comp_num, DExpYear, StageCode, parameter.DExpCode, parameter.DExpValue);
            }
            ExecuteProcedure("Manage_ProdCost_DirectExpValues", Direct, inDirect,  StageCode,  DExpYear);
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExpensesDistribution(int StageCode, short DExpYear)
        {
            List<para> parameters = new List<para>();
            string query = string.Format("select * from ProdCost_DirectExpValues where DExpYear = {0} and StageCode = {1}", DExpYear, StageCode);
            string inquery = string.Format("select * from ProdCost_InDirectExpValues where InDExpYear = {0} and StageCode = {1}", DExpYear, StageCode);
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        parameters.Add(new para { 
                            DExpCode = Convert.ToInt16(rdr["DExpCode"]),
                            DExpValue = Convert.ToDecimal(rdr["DExpValue"]),
                            Direct = true
                        });
                    }
                }
            }

            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(inquery, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        parameters.Add(new para
                        {
                            DExpCode = Convert.ToInt16(rdr["InDExpCode"]),
                            DExpValue = Convert.ToDecimal(rdr["InDExpValue"]),
                            Direct = false
                        });
                    }
                }
            }


            return Json(new { parameters }, JsonRequestBehavior.AllowGet);
        }
        public class para
        {
            public short DExpCode { get; set; }
            public decimal DExpValue { get; set; }
            public bool Direct { get; set; }
        }








    }

}