using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AlphaERP.Controllers.BomDefinitionController;

namespace AlphaERP.Controllers
{
    public class DefVendorEvalItemsController : controller
    {
        // GET: DefVendorEvalItems
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DefVendorEvalItemsList()
        {
            return PartialView();
        }
        public JsonResult Save_VendorEvalItems(string CodeId, string DescAr, string DescEn)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                using (SqlCommand CmdFiles = new SqlCommand())
                {
                    CmdFiles.Connection = cn;
                    CmdFiles.CommandText = "Ord_AddSysParameters";
                    CmdFiles.CommandType = CommandType.StoredProcedure;
                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdFiles.Parameters.Add(new SqlParameter("@ParamID", SqlDbType.TinyInt)).Value = 13;
                    CmdFiles.Parameters.Add(new SqlParameter("@SysMajor", SqlDbType.VarChar)).Value = "EItem";
                    CmdFiles.Parameters.Add(new SqlParameter("@CodeID", SqlDbType.VarChar)).Value = CodeId;
                    CmdFiles.Parameters.Add(new SqlParameter("@ArbDesc", SqlDbType.VarChar)).Value = DescAr;
                    if (DescEn == null)
                    {
                        DescEn = DescAr;
                    }
                    CmdFiles.Parameters.Add(new SqlParameter("@EngDesc", SqlDbType.VarChar)).Value = DescEn;
                    CmdFiles.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdFiles.Transaction = transaction;
                    try
                    {
                        CmdFiles.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit_VendorEvalItems(string CodeId, string DescAr, string DescEn)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                using (SqlCommand CmdFiles = new SqlCommand())
                {
                    CmdFiles.Connection = cn;
                    CmdFiles.CommandText = "Ord_UpdSysParameters";
                    CmdFiles.CommandType = CommandType.StoredProcedure;
                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdFiles.Parameters.Add(new SqlParameter("@ParamID", SqlDbType.TinyInt)).Value = 13;
                    CmdFiles.Parameters.Add(new SqlParameter("@SysMajor", SqlDbType.VarChar)).Value = "EItem";
                    CmdFiles.Parameters.Add(new SqlParameter("@CodeID", SqlDbType.VarChar)).Value = CodeId;
                    CmdFiles.Parameters.Add(new SqlParameter("@ArbDesc", SqlDbType.NVarChar)).Value = DescAr;
                    if (DescEn == null)
                    {
                        DescEn = DescAr;
                    }
                    CmdFiles.Parameters.Add(new SqlParameter("@EngDesc", SqlDbType.NVarChar)).Value = DescEn;
                    CmdFiles.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdFiles.Transaction = transaction;
                    try
                    {
                        CmdFiles.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Del_VendorEvalItems(string CodeId)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlTransaction transaction;
                cn.Open();

                transaction = cn.BeginTransaction();
                using (SqlCommand CmdFiles = new SqlCommand())
                {
                    CmdFiles.Connection = cn;
                    CmdFiles.CommandText = "Ord_DelSysParameters";
                    CmdFiles.CommandType = CommandType.StoredProcedure;
                    CmdFiles.Parameters.Add(new SqlParameter("@CompNo", SqlDbType.SmallInt)).Value = company.comp_num;
                    CmdFiles.Parameters.Add(new SqlParameter("@ParamID", SqlDbType.TinyInt)).Value = 13;
                    CmdFiles.Parameters.Add(new SqlParameter("@SysMajor", SqlDbType.VarChar)).Value = "EItem";
                    CmdFiles.Parameters.Add(new SqlParameter("@CodeID", SqlDbType.VarChar)).Value = CodeId;
                    CmdFiles.Parameters.Add(new SqlParameter("@ErrNo", SqlDbType.SmallInt)).Direction = ParameterDirection.Output;
                    CmdFiles.Transaction = transaction;
                    try
                    {
                        CmdFiles.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        cn.Dispose();
                        return Json(new { error = "حدث خطأ" }, JsonRequestBehavior.AllowGet);
                    }
                }
                transaction.Commit();
                cn.Dispose();
            }

            return Json(new { Ok = "Ok" }, JsonRequestBehavior.AllowGet);
        }

    }
}