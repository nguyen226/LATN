using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Services;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Common;
using System.Web;
using System.IO;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    public class TopNavigationController : Controller
    {
    
        public PartialViewResult _PartialTopNavigationBar()
        {
            TopNavigationModels model = new TopNavigationModels();
            string selectedFields = "UserID, Avartar, EmailID, (LastName + \" \" + FirstName) AS HoTen";

            if (Request.Cookies.Get(CommonConstants.CookieUserID) != null)
            {
                UsersService usersService = new UsersService();
                UsersDto user = usersService.GetByIdExposeDto(Int32.Parse(Request.Cookies.Get(CommonConstants.CookieUserID).Value), selectedFields);
                if(user != null)
                {
                    model.User = user;
                }
            }

            AppConfigService appConfigService = new AppConfigService();
            selectedFields = "Id, Logo, Active";
            AppConfigDto app = appConfigService.GetByActiveExposeDto(selectedFields);
            if(app != null)
            {
                model.Logo = app.Logo;
            }
            return PartialView(model);
        }
        public PartialViewResult _SnailSoftTopNavigationBar()
        {
            TopNavigationModels model = new TopNavigationModels();
            string selectedFields = "UserID, Avartar, EmailID, (LastName + \" \" + FirstName) AS HoTen";

            if (Request.Cookies.Get(CommonConstants.CookieUserID) != null)
            {
                UsersService usersService = new UsersService();
                UsersDto user = usersService.GetByIdExposeDto(Int32.Parse(Request.Cookies.Get(CommonConstants.CookieUserID).Value), selectedFields);
                if (user != null)
                {
                    model.User = user;
                }
            }

            AppConfigService appConfigService = new AppConfigService();
            selectedFields = "Id, Logo, Active";
            AppConfigDto app = appConfigService.GetByActiveExposeDto(selectedFields);
            if (app != null)
            {
                model.Logo = app.Logo;
            }

            return PartialView(model);
        }
    }
}