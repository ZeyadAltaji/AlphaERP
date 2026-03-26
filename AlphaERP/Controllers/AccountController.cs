using Microsoft.AspNet.SignalR;
using AlphaERP.Hubs;
using AlphaERP.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AlphaERP.Controllers
{
    public class AccountController : controller
    {

        public int notify()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.notify(1, "1", "1");
            return 0;
        }

        public ActionResult Captcha()
        {
            Captcha captcha = new Captcha();
            Session["captcha"] = captcha.Text;
            return File(captcha.ImageAsByteArray, "image/png");
        }

        public ActionResult Index()
        {
            if (Session["me"] == null)
            {
                return View();
            }
            if(Session["company"] == null)
            {
                return RedirectToAction("Index", "Company");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(User model, string Captcha)
        {
            ModelState.Clear();

          //  if (Session["captcha"] == null)
          //  {
          //      ModelState.AddModelError(string.Empty, Resources.Resource.CaptchaMessage);
          //      return View();
          //  }
          //  if (!Captcha.ToString().ToLower().Equals(Session["captcha"].ToString().ToLower()))
          //  {
          //      ModelState.AddModelError(string.Empty, Resources.Resource.CaptchaMessage);
          //      return View();
          //  }

            if (model.UserID == null || model.UserPWD == null)
            {
                if (model.UserID == null)
                {
                    ModelState.AddModelError(string.Empty, Resources.Resource.EmptyEmail);
                }
                if (model.UserPWD == null)
                {
                    ModelState.AddModelError(string.Empty, Resources.Resource.EmptyPassword);
                }
                return View();
            }


            User me = new MDB().Users.FirstOrDefault(x => x.UserID == model.UserID && x.UserPWD == model.UserPWD);
            if (me == null)
            {
                ModelState.AddModelError(string.Empty, Resources.Resource.UnvalidUserOrPassword);
                return View();
            }
            else
            {
                if (!me.StoppedUser.Value)
                {
                    LoadResources();
                    Session["me"] = me;
                    return RedirectToAction("Index", "Company");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resources.Resource.DisabledUserName);
                    return View(model);
                }
            }
        }

        private void LoadResources()
        {
            List<Alpha_Language> languages = db.Languages.ToList();
            Session["languages"] = languages;
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Logout()
        {
            if (me != null)
            {
                OnlineUser del = db.OnlineUsers
                    .Where(x => x.UserID == me.UserID && x.CompNo == company.comp_num).FirstOrDefault();
                if (del != null)
                {
                    db.OnlineUsers.Remove(del);
                    db.SaveChanges();
                }
            }
            Session["me"] = null;
            Session["company"] = null;
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public JsonResult ChangeLanguage()
        {
            string r = Session["language"].ToString();
            string l = "";
            if (r == "en")
            {
                l = "ar-JO";
                Session["language"] = l;
                HttpCookie userInfo = new HttpCookie("GCE_Lang");
                userInfo["language"] = l;
                userInfo.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Add(userInfo);
            }
            else
            {
                l = "en";
                Session["language"] = l;
                HttpCookie userInfo = new HttpCookie("GCE_Lang");
                userInfo["language"] = l;
                userInfo.Expires = DateTime.Now.AddYears(10);
                Response.Cookies.Add(userInfo);
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(l);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(l);
            CultureInfo ci = new CultureInfo(l);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            return Json(new { ok = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ErrorMessage(string errorMsg)
        {
            return Content(errorMsg);
        }

        [HttpPost]
        private bool IsValidEmail(string email)
        {
            try
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult PrivateKey(string Password)
        {
            if (Password == "GceSoft01042000")
            {

                string MacAddress = db.Database.SqlQuery<string>("declare @t table (i uniqueidentifier default newsequentialid(), m as cast(i as char(36))) insert into @t default values;select substring(m,25,2) + '-' + substring(m,27,2) + '-' + substring(m,29,2) + '-' + substring(m,31,2) + '-' + substring(m,33,2) + '-' + substring(m,35,2) as UserName FROM @t").FirstOrDefault();
                string hashedData = ComputeSha256Hash(MacAddress);
                Session["PrivateKey"] = hashedData;
                return View("~/Views/Account/PrivateKey.cshtml");
            }
            else
            {
                return null;
            }
        }

    }
}