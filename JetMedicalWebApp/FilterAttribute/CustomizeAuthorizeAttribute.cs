using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JetMedicalWebApp.Services;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Common;
using System.Web.Mvc.Html;

namespace JetMedicalWebApp.FilterAttribute
{
    public class CustomizeAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            var area = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : null;
            string tenController = !string.IsNullOrEmpty(area) ? area + "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName : filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string tenAction = filterContext.ActionDescriptor.ActionName;
            System.Web.Routing.RouteValueDictionary rd;
            bool kiemTraDangNhap = true;
            bool kiemTraQuyen = true;
            Dictionary<string, string> filters = null;
            Dictionary<string, string> outData = null;
            Dictionary<string, string> inData = null;
            string stringFilter = string.Empty, selectedFields = string.Empty;
            Menu_GroupPermissionService menu_GroupPermissionService = null;
            Company_GroupPermissionService company_GroupPermissionService = new Company_GroupPermissionService();
            ;
            Menu_GroupPermissionDto menu_GroupPermission = null;
            string userID = string.Empty, isAdminString = string.Empty;

            if (filterContext.HttpContext.Request.Cookies[CommonConstants.CookieUserID] != null)
            {
                userID = filterContext.HttpContext.Request.Cookies.Get(CommonConstants.CookieUserID).Value;
            }

            if (filterContext.HttpContext.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = filterContext.HttpContext.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }

            HttpCookie userAdminCookie = new HttpCookie(Common.CommonConstants.CookieUserAdmin);

            switch (tenController)
            {
                case "Admin/XML2":
                case "Admin/XML3":
                case "Admin/XML4":
                    tenController = "Admin/XML1";
                    break;
                case "Admin/LeftNavigation":
                    tenController = "Admin/Home";
                    break;
            }

            switch (tenAction)
            {
                case "NhapDuLieuTuFileExcel":
                    tenAction = "Update";
                    break;
            }

            if (!(tenAction.Trim().ToLower().Equals("login")) && !(tenAction.Trim().ToLower().Equals("accessdenied")))
            {
                if (!string.IsNullOrEmpty(isAdminString) && isAdminString == "true" && !string.IsNullOrEmpty(userID))
                {
                    MenuService menuService = new MenuService();

                    filters = new Dictionary<string, string>();
                    inData = new Dictionary<string, string>();

                    stringFilter = "IsActive = true";

                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        filters.Add("StringFilter", stringFilter);
                    }

                    inData.Add(CommonConstants.StrSelectedFields, "Id, No, Code, Name, IsActive, ControllerName, ActionName, MaMenuCapTren, Parameters, Icon");
                    inData.Add(CommonConstants.StrSortedColumnNames, "No ASC, Id ASC");
                    var menu = menuService.GetListExposeDto(filters, inData, out outData).ToList();
                    filterContext.HttpContext.Session[CommonConstants.SessionName_Menu] = menu;
                }
                else if (!string.IsNullOrEmpty(isAdminString) && isAdminString == "false" && !string.IsNullOrEmpty(userID))
                {
                    AppHistory appHistoryService = new AppHistory()
                    {
                        Ma = tenController,
                        Ten = tenAction,
                        ThaoTac = CommonConstants.ThaoTacTruyCap
                    };

                    if (!CommonConstants.LaChucNangTruyCapChung(tenController.Trim(), tenAction.Trim()))
                    {
                        menu_GroupPermissionService = new Menu_GroupPermissionService();
                        company_GroupPermissionService = new Company_GroupPermissionService();
                        filters = null;
                        outData = null;
                        inData = new Dictionary<string, string>();
                        stringFilter = string.Empty;
                        selectedFields = string.Empty;

                        filters = new Dictionary<string, string>();
                        selectedFields = "Id, Menu.ActionName AS ActionName, Menu.ControllerName AS ControllerName,";
                        selectedFields += "Menu.Code AS MaMenu, GroupPermission, ";
                        selectedFields += "SystemView, PersonalView, PersonalDelete, SystemDelete, SystemEdit, PersonalEdit";

                        stringFilter = "(ControllerName=\"" + tenController + "\" OR ControllerName=\"" + CommonConstants.ControllerName_User + "\" OR ControllerName=\"" + CommonConstants.ControllerName_XML1 + "\")";

                        if (tenAction.IndexOf("Add") > -1 || tenAction.IndexOf("Update") > -1)
                        {
                            stringFilter += " AND ActionName=\"Index\" AND (SystemEdit = True OR PersonalEdit = True) ";
                        }
                        else if (tenAction.IndexOf("Delete") > -1)
                        {
                            stringFilter += " AND ActionName=\"Index\" AND (PersonalDelete = True OR SystemDelete = True) ";
                        }
                        else
                        {
                            stringFilter += " AND ActionName=\"" + tenAction + "\" AND (SystemView = True OR PersonalView = True) ";
                        }

                        stringFilter += " AND GroupPermission.Users_GroupPermissions.Any(Users.UserID=" + userID + ") ";


                        inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                        inData.Add(CommonConstants.StrSortedColumnNames, "Id asc");

                        filters.Add("StringFilter", stringFilter);

                        List<Menu_GroupPermissionDto> menu_GroupPermissions = menu_GroupPermissionService.GetListExposeDto(filters, inData, out outData);

                        inData = new Dictionary<string, string>();
                        selectedFields = "Id, GroupPermission, (Company != null ? Company.CompanyID : -1) AS CompanyId";
                        stringFilter = " GroupPermission.Users_GroupPermissions.Any(Users.UserID=" + userID + ") ";
                        filters["StringFilter"] = stringFilter;
                        inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                        inData.Add(CommonConstants.StrSortedColumnNames, "Id asc");

                        List<Company_GroupPermissionDto> company_GroupPermissions = company_GroupPermissionService.GetListExposeDto(filters, inData, out outData);

                        menu_GroupPermission = new Menu_GroupPermissionDto()
                        {
                            ControllerName = tenController,
                            ActionName = tenAction,
                            AllowUpdate = true,
                            AllowDelete = true,
                            SystemView = true,
                            PersonalView = false,
                            SystemDelete = false,
                            PersonalDelete = false,
                            SystemEdit = true,
                            PersonalEdit = false,
                            IsSystemUpdate_User = true,
                            IsSystemUpdate_XML1 = true
                        };

                        if (menu_GroupPermissions == null)
                        {
                            appHistoryService.GhiChu = "Bạn không có quyền truy cập.";
                            kiemTraQuyen = false;
                        }
                        else if (menu_GroupPermissions.Count(m=>m.ControllerName.Equals(tenController)) == 0)
                        {
                            appHistoryService.GhiChu = "Bạn không có quyền truy cập.";
                            kiemTraQuyen = false;
                        }

                        if (kiemTraDangNhap && kiemTraQuyen)
                        {
                            menu_GroupPermission.IsSystemUpdate_User = menu_GroupPermissions.Exists(m => m.ControllerName.Equals(CommonConstants.ControllerName_User) && m.SystemEdit == true);
                            menu_GroupPermission.IsSystemUpdate_XML1 = menu_GroupPermissions.Exists(m => m.ControllerName.Equals(CommonConstants.ControllerName_XML1) && m.SystemEdit == true);

                            var tempMenu_GroupPermissions = menu_GroupPermissions.Where(m => m.ControllerName.Equals(tenController)).ToList();
                            if (tempMenu_GroupPermissions.Count() > 0)
                            {
                                menu_GroupPermission.MaMenu = tempMenu_GroupPermissions.FirstOrDefault().MaMenu;
                                menu_GroupPermission.SystemView = tempMenu_GroupPermissions.Exists(m => m.SystemView == true);
                                menu_GroupPermission.PersonalView = tempMenu_GroupPermissions.Exists(m => m.PersonalView == true);
                                menu_GroupPermission.PersonalEdit = tempMenu_GroupPermissions.Exists(m => m.PersonalEdit == true);
                                menu_GroupPermission.SystemEdit = tempMenu_GroupPermissions.Exists(m => m.SystemEdit == true);
                                menu_GroupPermission.SystemDelete = tempMenu_GroupPermissions.Exists(m => m.SystemDelete == true);
                                menu_GroupPermission.PersonalDelete = tempMenu_GroupPermissions.Exists(m => m.PersonalDelete == true);

                                if (menu_GroupPermission.PersonalEdit == false && menu_GroupPermission.SystemEdit == false)
                                {
                                    menu_GroupPermission.AllowUpdate = false;
                                }

                                if (menu_GroupPermission.SystemDelete == false && menu_GroupPermission.PersonalDelete == false)
                                {
                                    menu_GroupPermission.AllowDelete = false;
                                }
                            }
                            else
                            {
                                appHistoryService.GhiChu = "Bạn không có quyền truy cập.";
                                kiemTraQuyen = false;
                            }
                        }

                        if (kiemTraDangNhap == true && kiemTraQuyen == true)
                        {
                            filterContext.HttpContext.Session[CommonConstants.Session_CurrentUser_Menu_GroupPermission] = menu_GroupPermission;
                            filterContext.HttpContext.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission] = company_GroupPermissions;
                        }
                        else
                        {
                            if (kiemTraDangNhap == false)
                            {
                                rd = new System.Web.Routing.RouteValueDictionary(new { action = "Login", controller = "Home", area = "Admin" });
                                filterContext.Result = new RedirectToRouteResult(rd);
                            }
                            else if (kiemTraQuyen == false)
                            {
                                rd = new System.Web.Routing.RouteValueDictionary(new { action = "AccessDenied", controller = "Home", area = "Admin" });
                            }
                        }
                    }else
                    {
                        inData = new Dictionary<string, string>();
                        filters = new Dictionary<string, string>();
                        outData = new Dictionary<string, string>();
                        List<Company_GroupPermissionDto> company_GroupPermissions = new List<Company_GroupPermissionDto>();
                        selectedFields = "Id, GroupPermission, (Company != null ? Company.CompanyID : -1) AS CompanyId";
                        stringFilter = " GroupPermission.Users_GroupPermissions.Any(Users.UserID=" + userID + ") ";
                        filters["StringFilter"] = stringFilter;
                        inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                        inData.Add(CommonConstants.StrSortedColumnNames, "Id asc");
                        company_GroupPermissions = company_GroupPermissionService.GetListExposeDto(filters, inData, out outData);
                        filterContext.HttpContext.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission] = company_GroupPermissions;
                        appHistoryServiceService.Add(appHistoryService);
                    }
                }
                else
                {
                    kiemTraDangNhap = false;
                }

                if (kiemTraDangNhap == false)
                {
                    rd = new System.Web.Routing.RouteValueDictionary(new { action = "Login", controller = "Home", area = "Admin" });
                    filterContext.Result = new RedirectToRouteResult(rd);
                }
                else if (kiemTraQuyen == false)
                {
                    rd = new System.Web.Routing.RouteValueDictionary(new { action = "AccessDenied", controller = "Home", area = "Admin" });
                    filterContext.Result = new RedirectToRouteResult(rd);
                }
            }

            base.OnAuthorization(filterContext);
        }
    }
}