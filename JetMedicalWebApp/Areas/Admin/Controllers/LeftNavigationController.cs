using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Services;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Common;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    public class LeftNavigationController : Controller
    {
        public PartialViewResult _PartialLeftSideBar()
        {
            InternalService internalService = new InternalService();
            LeftNavigationViewModel model = (LeftNavigationViewModel)internalService.KhoiTaoModel(new LeftNavigationViewModel());
            string selectedFields = string.Empty, stringFilter= string.Empty;
            model.Menus = new List<MenuDto>();
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;
            Dictionary<string, string> inData = new Dictionary<string, string>();


            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
            if (authCookie != null)
            {
                if (Session[CommonConstants.SessionName_Menu] != null)
                {
                    model.Menus = (List<MenuDto>)Session[CommonConstants.SessionName_Menu];
                }
                else
                {
                    MenuService menuService = new MenuService();
                   
                    selectedFields = "Id, No, Code, Name, IsActive, ControllerName, ActionName, ";
                    selectedFields += "MaMenuCapTren, Parameters, Icon,Menu_GroupPermissions ";

                    stringFilter = "IsActive = true";

                    stringFilter += " AND Menu_GroupPermissions.Any((SystemView = True OR PersonalView = True) ";
                    stringFilter += " AND GroupPermission.Users_GroupPermissions.Any(Users.UserId=" + authCookie.Value + "))";

                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        filters.Add("StringFilter", stringFilter);
                    }

                    inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inData.Add(CommonConstants.StrSortedColumnNames, "No asc");

                    model.Menus = menuService.GetListExposeDto(filters, inData, out outData).ToList();
                }

                var menu_GroupPermission = (Menu_GroupPermissionDto)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Menu_GroupPermission];
                if(menu_GroupPermission == null)
                {
                    selectedFields = "Id, Menu.ActionName AS ActionName, Menu.ControllerName AS ControllerName,";
                    selectedFields += "Menu.Code AS MaMenu, GroupPermission, ";
                    selectedFields += "SystemView, PersonalView, PersonalDelete, SystemDelete, SystemEdit, PersonalEdit";
                    stringFilter = "(ControllerName=\"" + CommonConstants.ControllerName_User + "\" OR ControllerName=\"" + CommonConstants.ControllerName_XML1 + "\")";
                    stringFilter += " AND (SystemEdit = True) ";
                    stringFilter += " AND GroupPermission.Users_GroupPermissions.Any(Users.UserID=" + authCookie.Value + ") ";

                    inData = new Dictionary<string, string>();
                    inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inData.Add(CommonConstants.StrSortedColumnNames, "Id asc");
                    filters = new Dictionary<string, string>();
                    filters.Add("StringFilter", stringFilter);
                    Menu_GroupPermissionService menu_GroupPermissionService = new Menu_GroupPermissionService();
                    List<Menu_GroupPermissionDto> menu_GroupPermissions = menu_GroupPermissionService.GetListExposeDto(filters, inData, out outData);

                    menu_GroupPermission = new Menu_GroupPermissionDto()
                    {
                        IsSystemUpdate_User = false,
                        IsSystemUpdate_XML1 = false
                    };

                    menu_GroupPermission.IsSystemUpdate_User = menu_GroupPermissions.Any(m => m.ControllerName.Equals(CommonConstants.ControllerName_User) && m.SystemEdit == true);
                    menu_GroupPermission.IsSystemUpdate_XML1 = menu_GroupPermissions.Any(m => m.ControllerName.Equals(CommonConstants.ControllerName_XML1) && m.SystemEdit == true);
                    model.Menu_GroupPermission = menu_GroupPermission;
                }
            }


            return PartialView(model);
        }
        public PartialViewResult _SnailSoftLeftSideBar()
        {
            InternalService internalService = new InternalService();
            LeftNavigationViewModel model = (LeftNavigationViewModel)internalService.KhoiTaoModel(new LeftNavigationViewModel());
            string selectedFields = string.Empty, stringFilter = string.Empty;
            model.Menus = new List<MenuDto>();
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;
            Dictionary<string, string> inData = new Dictionary<string, string>();


            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
            if (authCookie != null)
            {
                if (Session[CommonConstants.SessionName_Menu] != null)
                {
                    model.Menus = (List<MenuDto>)Session[CommonConstants.SessionName_Menu];
                }
                else
                {
                    MenuService menuService = new MenuService();

                    selectedFields = "Id, No, Code, Name, IsActive, ControllerName, ActionName, ";
                    selectedFields += "MaMenuCapTren, Parameters, Icon,Menu_GroupPermissions ";

                    stringFilter = "IsActive = true";

                    stringFilter += " AND Menu_GroupPermissions.Any((SystemView = True OR PersonalView = True) ";
                    stringFilter += " AND GroupPermission.Users_GroupPermissions.Any(Users.UserId=" + authCookie.Value + "))";

                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        filters.Add("StringFilter", stringFilter);
                    }

                    inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inData.Add(CommonConstants.StrSortedColumnNames, "No asc");

                    model.Menus = menuService.GetListExposeDto(filters, inData, out outData).ToList();
                }

                var menu_GroupPermission = (Menu_GroupPermissionDto)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Menu_GroupPermission];
                if (menu_GroupPermission == null)
                {
                    selectedFields = "Id, Menu.ActionName AS ActionName, Menu.ControllerName AS ControllerName,";
                    selectedFields += "Menu.Code AS MaMenu, GroupPermission, ";
                    selectedFields += "SystemView, PersonalView, PersonalDelete, SystemDelete, SystemEdit, PersonalEdit";
                    stringFilter = "(ControllerName=\"" + CommonConstants.ControllerName_User + "\" OR ControllerName=\"" + CommonConstants.ControllerName_XML1 + "\")";
                    stringFilter += " AND (SystemEdit = True) ";
                    stringFilter += " AND GroupPermission.Users_GroupPermissions.Any(Users.UserID=" + authCookie.Value + ") ";

                    inData = new Dictionary<string, string>();
                    inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inData.Add(CommonConstants.StrSortedColumnNames, "Id asc");
                    filters = new Dictionary<string, string>();
                    filters.Add("StringFilter", stringFilter);
                    Menu_GroupPermissionService menu_GroupPermissionService = new Menu_GroupPermissionService();
                    List<Menu_GroupPermissionDto> menu_GroupPermissions = menu_GroupPermissionService.GetListExposeDto(filters, inData, out outData);

                    menu_GroupPermission = new Menu_GroupPermissionDto()
                    {
                        IsSystemUpdate_User = false,
                        IsSystemUpdate_XML1 = false
                    };

                    menu_GroupPermission.IsSystemUpdate_User = menu_GroupPermissions.Any(m => m.ControllerName.Equals(CommonConstants.ControllerName_User) && m.SystemEdit == true);
                    menu_GroupPermission.IsSystemUpdate_XML1 = menu_GroupPermissions.Any(m => m.ControllerName.Equals(CommonConstants.ControllerName_XML1) && m.SystemEdit == true);
                    model.Menu_GroupPermission = menu_GroupPermission;
                }
            }


            return PartialView(model);
        }
    }
}