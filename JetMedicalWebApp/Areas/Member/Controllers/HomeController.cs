using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Member.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModels model = new LoginViewModels();
            model.ReturnUrl = returnUrl;

            HttpCookie currentUserCookie = Request.Cookies[Common.CommonConstants.CookieMemberID];
            if(currentUserCookie != null)
            {
                Response.Cookies.Remove(Common.CommonConstants.CookieMemberID);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Login(LoginViewModels model, string returnUrl)
        {
            UsersService memberSerivce = new UsersService();
            model.Message = string.Empty;

            if (ModelState.IsValid)
            {
                var member = memberSerivce.GetByEmailOrPhoneExposeDto(model.Username, "UserID,Active,FirstName, LastName, EmailID, Phone, Password");

                if (member != null)
                {
                    if (member.Active)
                    {
                        if (member.Password == Encryptor.Hash(model.Password))
                        {
                            setCookieLogin(member.UserID.ToString(), (member.LastName + " " + member.FirstName), "true");

                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { area = "Member" });
                            }
                        }
                        else
                        {
                            model.Message = "Mật khẩu không chính xác.";
                        }
                    }
                    else
                    {
                        model.Message = "Tài khoản chưa kích hoạt.";
                    }

                }
                else
                {
                    model.Message = "Email hoặc số điện thoại không chính xác.";
                }
            }
            else
            {
                model.Message = "Vui lòng nhập đầy đủ thông tin.";
            }

            return View(model);
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Member" });
            }
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session[CommonConstants.SessionName_Menu] = null;
            return RedirectToAction("Login", "Home", new { area = "Member" });
        }

        public void setCookieLogin(string memberId, string memberName, string isAdmin)
        {
            HttpCookie memberIdCookie = new HttpCookie(CommonConstants.CookieMemberID);
            memberIdCookie.Value = memberId;
            memberIdCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(memberIdCookie);

            HttpCookie memberNameCookie = new HttpCookie(CommonConstants.CookieMemberName);
            memberNameCookie.Value = HttpUtility.UrlEncode(memberName);
            memberNameCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(memberNameCookie);
        }
    }
}