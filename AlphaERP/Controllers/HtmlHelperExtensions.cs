using AlphaERP.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class DisplayExtensions
    {
        public static MvcHtmlString Recource(this HtmlHelper html, long TxID)
        {
            List<Alpha_Language> languages = (List<Alpha_Language>)HttpContext.Current.Session["languages"];
            if (languages == null)
            {
                return MvcHtmlString.Create("No Languages list");
            }
            string language = (string)HttpContext.Current.Session["language"];
            Alpha_Language word = languages.Where(x => x.TxtID == TxID).FirstOrDefault();
            if (word == null)
            {
                return MvcHtmlString.Create(string.Format("<{0}>", TxID));
            }
            if (language == "en")
            {
                return MvcHtmlString.Create(word.ENTxt);
            }
            else
            {
                return MvcHtmlString.Create(word.ARTxt);
            }
        }
        public static MvcHtmlString Translate(this HtmlHelper html, string Arabic, string English)
        {
            string language = (string)HttpContext.Current.Session["language"];
            if (language == "en")
            {
                return MvcHtmlString.Create(English);
            }
            else
            {
                return MvcHtmlString.Create(Arabic);
            }
        }
        public static MvcHtmlString Trim(this HtmlHelper html, string value, int length)
        {
            if (value.Length > length)
            {

                return MvcHtmlString.Create(value.Substring(0, length - 3) + "...");

            }
            else
            {
                return MvcHtmlString.Create(value);
            }
        }
        public static MvcHtmlString AddButton(this HtmlHelper html)
        {
            bool Allowed = false;
            RouteData routeData = html.ViewContext.RouteData;

            string controller = routeData.GetRequiredString("controller");
            string action = routeData.GetRequiredString("action");

            List<UserPermission> permissions = (List<UserPermission>)HttpContext.Current.Session["permissions"];
            User Me = (User)HttpContext.Current.Session["me"];
            string SourceForm = controller;
            if (action.ToLower() != "index")
            {
                SourceForm += "/" + action;
            }
            foreach (var item in permissions.Where(x => x.Menu != null))
            {
                if (item.Menu.SourceForm.ToLower() == SourceForm.ToLower())
                {
                    if (item.ProgAdd == true)
                    {
                        Allowed = true;
                    }
                }
            }
            if (Allowed)
            {
                return MvcHtmlString.Create("");
            }
            else
            {
                return MvcHtmlString.Create(" hidden=\"hidden\" ");
            }

        }
        public static MvcHtmlString EditButton(this HtmlHelper html)
        {
            bool Allowed = false;
            RouteData routeData = html.ViewContext.RouteData;

            string controller = routeData.GetRequiredString("controller");
            string action = routeData.GetRequiredString("action");

            List<UserPermission> permissions = (List<UserPermission>)HttpContext.Current.Session["permissions"];
            User Me = (User)HttpContext.Current.Session["me"];
            string SourceForm = controller;
            if (action.ToLower() != "index")
            {
                SourceForm += "/" + action;
            }
            foreach (var item in permissions.Where(x => x.Menu != null))
            {
                if (item.Menu.SourceForm.ToLower() == SourceForm.ToLower())
                {
                    if (item.ProgMod == true)
                    {
                        Allowed = true;
                    }
                }
            }
            if (Allowed)
            {
                return MvcHtmlString.Create("");
            }
            else
            {
                return MvcHtmlString.Create(" hidden=\"hidden\" ");
            }

        }
        public static MvcHtmlString DeleteButton(this HtmlHelper html)
        {
            bool Allowed = false;
            RouteData routeData = html.ViewContext.RouteData;

            string controller = routeData.GetRequiredString("controller");
            string action = routeData.GetRequiredString("action");

            List<UserPermission> permissions = (List<UserPermission>)HttpContext.Current.Session["permissions"];
            User Me = (User)HttpContext.Current.Session["me"];
            string SourceForm = controller;
            if (action.ToLower() != "index")
            {
                SourceForm += "/" + action;
            }
            foreach (var item in permissions.Where(x => x.Menu != null))
            {
                if (item.Menu.SourceForm.ToLower() == SourceForm.ToLower())
                {
                    if (item.ProgDel == true)
                    {
                        Allowed = true;
                    }
                }
            }
            if (Allowed)
            {
                return MvcHtmlString.Create("");
            }
            else
            {
                return MvcHtmlString.Create(" hidden=\"hidden\" ");
            }

        }
        public static MvcHtmlString ConnectionString(this HtmlHelper html)
        {
            string Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
            Server = Server.Replace(@"\\", @"\");
            string DataBase_ = System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");
            string ConnectionString = string.Format("Server={0};Database={1};User ID=Admin;Password=GceSoft01042000", Server, DataBase_);
            return MvcHtmlString.Create(ConnectionString);

        }
        public static MvcHtmlString CompNo(this HtmlHelper html)
        {
            short CompNo_ = 0;
            Company c = (Company)HttpContext.Current.Session["company"];
            CompNo_ = c.comp_num;
            return MvcHtmlString.Create(CompNo_.ToString());

        }
        public static MvcHtmlString UserID(this HtmlHelper html)
        {
            string UserID_ = "0";
            User u = (User)HttpContext.Current.Session["me"];
            UserID_ = u.UserID;
            return MvcHtmlString.Create(UserID_);

        }
        public static MvcHtmlString MyBusinessUnits(this HtmlHelper html)
        {
            string items = "";
            string Server = System.Configuration.ConfigurationManager.AppSettings.Get("Server");
            Server = Server.Replace(@"\\", @"\");
            string DataBase_ = System.Configuration.ConfigurationManager.AppSettings.Get("DataBase");
            string ConnectionString = string.Format("Server={0};Database={1};User ID=Admin;Password=GceSoft01042000", Server, DataBase_);

            string UserID_ = "0";
            User u = (User)HttpContext.Current.Session["me"];
            UserID_ = u.UserID;
            short CompNo_ = 0;
            Company c = (Company)HttpContext.Current.Session["company"];
            CompNo_ = c.comp_num;
            SqlConnection cn = new SqlConnection(ConnectionString);
            string language = (string)HttpContext.Current.Session["language"];


            SqlCommand sCmd;
            SqlDataReader sDR;
            string strsql;
            strsql = "SELECT BusUnitID, BusUnitDescAr, BusUnitDescEng  FROM Alpha_BusinessUnitDef  WHERE CompNo = " + CompNo_ + " AND BusUnitID IN (SELECT BusUnit From Alpha_BusUnitUserPermissions  WHERE (CompNo =  " + CompNo_ + " ) AND (UserID = '" + UserID_ + "') AND ((AllPerm = 1)))";
            sCmd = new SqlCommand();
            sCmd.Connection = cn;
            cn.Open();
            sCmd.CommandType = CommandType.Text;
            sCmd.CommandText = strsql;
            sDR = sCmd.ExecuteReader();
            while (sDR.Read())
            {

                if (language != "en")
                {
                    items += "<option value =\"" + Convert.ToInt32(sDR["BusUnitID"]) + "\" >" + sDR["BusUnitDescAr"].ToString() + "</option>" + Environment.NewLine;
                }
                else
                {
                    items += "<option value =\"" + Convert.ToInt32(sDR["BusUnitID"]) + "\" >" + sDR["BusUnitDescEng"].ToString() + "</option>" + Environment.NewLine;
                }
            }

            sDR.Close();
            cn.Close();
            return MvcHtmlString.Create(items.ToString());
        }

    }
}