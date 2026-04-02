using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Linq;
namespace AlphaERP.Controllers
{
    public class controller : Controller
    {
        public SqlConnection Cn()
        {
            return new SqlConnection(ConnectionString());
        }
        public int gGceClient = 251;
        
        public void AddWorkFlowLog(SqlConnection cn, int FID, int TrYear, int TrType, int TrNo, int BU, int FStat, double TrAmnt, string TrDesc, System.Data.SqlClient.SqlTransaction MyTrans)
        {
            using (var Cmd = new SqlCommand())
            {
                Cmd.Connection = cn;
                Cmd.CommandText = "Alpha_AddWorkFlowLog";
                Cmd.CommandType = CommandType.StoredProcedure;
                {
                    var withBlock = Cmd.Parameters;
                    withBlock.Add("@CompNo", SqlDbType.SmallInt).Value = company.comp_num;
                    withBlock.Add("@FID", SqlDbType.SmallInt).Value = FID;
                    withBlock.Add("@BU", SqlDbType.SmallInt).Value = BU;
                    withBlock.Add("@UserID", SqlDbType.VarChar, 8).Value = me.UserID;
                    withBlock.Add("@K1", SqlDbType.VarChar, 10).Value = TrYear;
                    withBlock.Add("@K2", SqlDbType.VarChar, 10).Value = TrType;
                    withBlock.Add("@K3", SqlDbType.VarChar, 10).Value = TrNo;
                    withBlock.Add("@TrAmount", SqlDbType.Money).Value = TrAmnt;
                    withBlock.Add("@TrFormDesc", SqlDbType.VarChar, 300).Value = TrDesc;
                    withBlock.Add("@FrmStat", SqlDbType.SmallInt).Value = FStat;
                    withBlock.Add("@FinalApprove", SqlDbType.Bit).Direction = ParameterDirection.Output;
                }

                Cmd.Transaction = MyTrans;
                Cmd.ExecuteNonQuery();
            }
        }
        public int RunProcedure(int IdTrans, string Procedure, long BankAcc = 0)
        {
            SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = new SqlConnection(ConnectionString());
            cmd.Connection = cn;
            cn.Open();
            cmd.CommandText = Procedure;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = 1;
            cmd.Parameters.Add(new SqlParameter("@IdTrans", System.Data.SqlDbType.Int)).Value = IdTrans;
            if (BankAcc != 0)
            {
                cmd.Parameters.Add(new SqlParameter("@BankAcc", System.Data.SqlDbType.BigInt)).Value = BankAcc;
            }
            cmd.Parameters.Add(new SqlParameter("@ErrNo", System.Data.SqlDbType.SmallInt)).Direction = System.Data.ParameterDirection.Output;
            transaction = cn.BeginTransaction();
            cmd.Transaction = transaction;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                // throw ex;
            }

            int ErrNo = Convert.ToInt32(cmd.Parameters["@ErrNo"].Value);

            if (ErrNo == 0)
            {
                transaction.Commit();
                cn.Dispose();
            }
            else
            {
                transaction.Rollback();
                cn.Dispose();
            }
            return ErrNo;
        }

        public int ExecuteProcedure(string ProcedureName, DataTable Direct, DataTable inDirect, int StageCode, short DExpYear)
        {
            SqlTransaction transaction;
            using (SqlConnection connection = new SqlConnection(ConnectionString()))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = ProcedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter parameter;
                    parameter = command.Parameters.AddWithValue("@Direct", Direct);
                    parameter = command.Parameters.AddWithValue("@inDirect", inDirect);
                    command.Parameters.Add(new SqlParameter("@CompNo", System.Data.SqlDbType.SmallInt)).Value = company.comp_num;
                    command.Parameters.Add(new SqlParameter("@DExpYear", System.Data.SqlDbType.SmallInt)).Value = DExpYear;
                    command.Parameters.Add(new SqlParameter("@StageCode", System.Data.SqlDbType.Int)).Value = StageCode;
                    command.Parameters.Add(new SqlParameter("@ErrNo", System.Data.SqlDbType.SmallInt)).Direction = System.Data.ParameterDirection.Output;
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.TypeName = "dbo.ProdCost_DirectExpValue";
                    transaction = connection.BeginTransaction();
                    command.Transaction = transaction;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                    }
                    int ErrNo = Convert.ToInt32(command.Parameters["@ErrNo"].Value);
                    if (ErrNo == 0)
                    {
                        transaction.Commit();
                        connection.Dispose();
                    }
                    else
                    {
                        transaction.Rollback();
                        connection.Dispose();
                    }
                    return ErrNo;
                }
            }
        }





        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public List<string> AcceptedExtentions = new List<string>() { ".pdf", ".txt", ".png", ".jpg", ".gif", ".bmp", ".xls", ".xlsx", ".doc", ".docx" };

        public string error = "";

        public string Title { get; set; }

        public MDB db = new MDB();

        public bool checkView(int ProgID)
        {
            return true;
        }

        public bool CheckProdPermission(int progID)
        {
            if (Session["ProdOrdersPermissions"] == null) return false;
            var permissions = (List<ProductionOrdersPermission>)Session["ProdOrdersPermissions"];
            var perm = permissions.FirstOrDefault(x => x.ProgID == progID);
            return perm != null && perm.ProgAccess == true;
        }

        public string Dummy = "";

        private string Lang;

        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["language"]))
                { Lang = "ar"; }
                else { Lang = (string)Session["language"]; }
                return Lang;
            }
        }

       

        private User user;

        public User me
        {
            get
            {
                if ((User)Session["me"] != null)
                {
                    user = (User)Session["me"];
                }
                return user;
            }
        }

        public Company company => (Company)Session["company"];
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string CuurentURL = filterContext.Controller.ToString();
            string originAction = filterContext.RouteData.Values["action"].ToString();
            HttpCookie reqCookies = Request.Cookies["GCE_Lang"];
            HttpCookie reqCookies2 = Request.Cookies["Theme"];
            string langName = "";

            if (Session["Theme"] == null)
            {
                try
                {
                    if (reqCookies2 == null)
                    {
                        Session["Theme"] = "sidebar-mini skin-purple";
                    }
                    else
                    {
                        Session["Theme"] = reqCookies2["Theme"].ToString();
                    }
                }
                catch (Exception)
                {
                    Session["Theme"] = "sidebar-mini skin-purple";
                }
            }
            if (Session["language"] == null)
            {
                if (reqCookies == null)
                {
                    langName = "ar-JO";
                    Session["language"] = "ar-JO";
                    HttpCookie userInfo = new HttpCookie("GCE_Lang");
                    userInfo["language"] = "ar-JO";
                    userInfo.Expires = DateTime.Now.AddYears(10);
                    Response.Cookies.Add(userInfo);
                }
                else
                {
                    Session["language"] = reqCookies["language"].ToString();
                    langName = reqCookies["language"].ToString();
                }
            }
            else
            {

                langName = Session["language"].ToString();
                HttpCookie userInfo = new HttpCookie("GCE_Lang");
                userInfo["language"] = Session["language"].ToString();
                userInfo.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Add(userInfo);
            }
            ViewBag.ID = langName;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langName);
            CultureInfo ci = new CultureInfo(langName);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            if (!filterContext.Controller.ToString().Contains("AccountController"))
            {
                filterContext.HttpContext.Response.Cache.SetNoStore();
                if (Session["me"] == null)
                    filterContext.Result = new RedirectResult(Url.Action("Logout", "Account", new { area = string.Empty }));
                if (!filterContext.Controller.ToString().Contains("CompanyController"))
                {
                    if (company == null)
                        filterContext.Result = new RedirectResult("~/Company");
                }
            }
        }

        private void CreateDirectory(string path)
        {
            string MyPath = Server.MapPath(path);
            if (!Directory.Exists(MyPath))
            { Directory.CreateDirectory(MyPath); }
        }

        public void DoDirs(int id)
        {
            CreateDirectory("~/Uploads/" + id);

        }

        public void MD(string tp, int id)
        {
            CreateDirectory("~/Uploads/" + tp + "/" + id);
        }

        public string GetIpAddress()
        {
            string userip = Request.UserHostAddress;
            if (Request.UserHostAddress != null)
            {
                long macinfo = new long();
                string macSrc = macinfo.ToString("X");
                if (macSrc == "0")
                {
                    return userip;
                }
            }
            return "";
        }

        public string ConnectionString()
        {
            string Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
            Server = Server.Replace(@"\\", @"\");
            string DataBase_ = System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");
            return string.Format("Server={0};Database={1};User ID=admin;Password=GceSoft01042000", Server, DataBase_);
        }

        public void Excute(string query)
        {
            using (var cn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public string RenderRazorViewToString(string viewName, User model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public string Translate(string Ar, string En)
        {
            if (Session["language"].ToString() == "ar-JO")
            {
                return Ar;
            }
            else
            {
                return En;
            }
        }
    }


}
