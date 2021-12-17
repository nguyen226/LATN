using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    [LogActionFilter]
    [CustomizeAuthorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            HomeViewModels model = (HomeViewModels)internalService.KhoiTaoModel(new HomeViewModels());

            DateTime ngayBatDauSuDungPhanMemValue;
            if (DateTime.TryParseExact(CommonConstants.NgayBatDauSuDungPhanMem, CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayBatDauSuDungPhanMemValue))
            {
                model.NamBatDauSuDungPhamMem = ngayBatDauSuDungPhanMemValue.Year;
            }
            else
            {
                model.NamBatDauSuDungPhamMem = DateTime.Now.Year;
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult GetDuLieuBaoCao(int nam)
        {
            InternalService internalService = new InternalService();
            return Json(new
            {
                data = internalService.GetDuLieuBaoCaoTheoNam(nam)
                //dataBaoCaoXML3 = internalService.GetDuLieuBaoCaoXML3TheoNam(nam),
                //dataBaoCaoXML4 = internalService.GetDuLieuBaoCaoXML4TheoNam(nam)
            }, JsonRequestBehavior.AllowGet);
        }

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

            HttpCookie currentUserCookie = Request.Cookies[Common.CommonConstants.CookieUserID];
            if(currentUserCookie != null)
            {
                Response.Cookies.Remove(Common.CommonConstants.CookieUserID);
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
            UsersService userSerivce = new UsersService();
            model.Message = string.Empty;

            if (ModelState.IsValid)
            {
                var user = userSerivce.GetByEmailOrPhoneExposeDto(model.Username, "UserID,FirstName, LastName, EmailID, Phone, Password, IsAdmin");

                if (user != null)
                {
                    if (user.Password == Encryptor.Hash(model.Password))
                    {
                        if (user.IsAdmin)
                        {
                            setCookieLogin(user.UserID.ToString(), (user.LastName + " " + user.FirstName), "true");

                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                            }
                        }
                        else
                        {
                            Users_GroupPermissionService users_GroupPermissionService = new Users_GroupPermissionService();
                            Dictionary<string, string> filters = new Dictionary<string, string>();
                            Dictionary<string, string> inData = new Dictionary<string, string>();
                            Dictionary<string, string> outData = new Dictionary<string, string>();
                            string selectedFields = "Id, Users.UserID AS UserID, Users.Active AS UserActive, (GroupPermission != null ? GroupPermission.IsActive : false) AS Active";
                            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                            filters.Add("StringFilter", "UserActive = TRUE AND Active = TRUE ");
                            var user_NhomPhanQuyens = users_GroupPermissionService.GetListExposeDto(filters, inData, out outData).ToList();

                            if (user_NhomPhanQuyens != null)
                            {
                                if (user_NhomPhanQuyens.Count() > 0)
                                {
                                    setCookieLogin(user.UserID.ToString(), (user.LastName + " " + user.FirstName), "false");

                                    if (!string.IsNullOrEmpty(returnUrl))
                                    {
                                        return RedirectToLocal(returnUrl);
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                                    }
                                }
                                else
                                {
                                    model.Message = "Quyền hoặc tài khoản đã hủy kích hoạt vui lòng liên hệ quản trị.";
                                }
                            }
                            else
                            {
                                model.Message = "Tài khoản không được phân quyền";
                            }
                        }
                        
                    }
                    else
                    {
                        model.Message = "Mật khẩu không chính xác.";
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
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session[CommonConstants.SessionName_Menu] = null;
            return RedirectToAction("Login", "Home", new { area = "Admin" });
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        public void setCookieLogin(string userId, string userName, string isAdmin)
        {
            HttpCookie userIdCookie = new HttpCookie(CommonConstants.CookieUserID);
            userIdCookie.Value = userId;
            userIdCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(userIdCookie);

            //HttpCookie userNameCookie = new HttpCookie(CommonConstants.CookieUserName);
            //userNameCookie.Value = userName;
            //userNameCookie.Expires = DateTime.Now.AddMonths(1);
            //Response.Cookies.Add(userNameCookie);

            HttpCookie userAdminCookie = new HttpCookie(CommonConstants.CookieUserAdmin);
            userAdminCookie.Value = isAdmin;
            userAdminCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(userAdminCookie);
        }

        
        [HttpPost]
        public ActionResult GetListTableDichVu(int? draw, int? start, int? length, string selectedFields, string stringFilter)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            int totalRecords = 0, totalPages = 0;

            string sortColumn = string.Empty;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            if (Request.Form.GetValues("order[0][column]") != null)
            {
                if (Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]") != null)
                {
                    sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                }
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                inputParam.Add(CommonConstants.StrSortedColumnNames, sortColumn + " " + sortColumnDir);
            }


            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }

            var company_GroupPermission = (List<Company_GroupPermissionDto>)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission];
            string isAdminString = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }

            if (company_GroupPermission != null && !string.IsNullOrEmpty(isAdminString) && isAdminString == "false")
            {
                if (company_GroupPermission.Count() > 0)
                {
                    string filterCongTy = string.Empty;
                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        stringFilter += " AND ";
                    }
                    stringFilter += " CompanyId IN (";

                    for (int i = 0; i < company_GroupPermission.Count(); i++)
                    {
                        stringFilter += company_GroupPermission[i].CompanyId.ToString();
                        if (i != company_GroupPermission.Count() - 1)
                        {
                            stringFilter += ",";
                        }
                    }
                    stringFilter += " )";
                }
            }

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            IEnumerable<DataThongKeDto> displayedData = internalService.GetListDataThongKeDichVuFromViewExposeDto(filters, inData, out outData);

            if (outData.ContainsKey("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(outData["TotalRecords"]);
                totalPages = (int)Math.Ceiling((float)totalRecords / (float.Parse(requestParameters[CommonConstants.StrLength])));
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = displayedData
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetListTableNewsContent(int? draw, int? start, int? length, string selectedFields, string stringFilter)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            int totalRecords = 0, totalPages = 0;

            string sortColumn = string.Empty;
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            if (Request.Form.GetValues("order[0][column]") != null)
            {
                if (Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]") != null)
                {
                    sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                }
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDir))
            {
                inputParam.Add(CommonConstants.StrSortedColumnNames, sortColumn + " " + sortColumnDir);
            }


            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }

            var company_GroupPermission = (List<Company_GroupPermissionDto>)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission];
            string isAdminString = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }

            if (company_GroupPermission != null && !string.IsNullOrEmpty(isAdminString) && isAdminString == "false")
            {
                if (company_GroupPermission.Count() > 0)
                {
                    string filterCongTy = string.Empty;
                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        stringFilter += " AND ";
                    }
                    stringFilter += " CompanyId IN (";

                    for (int i = 0; i < company_GroupPermission.Count(); i++)
                    {
                        stringFilter += company_GroupPermission[i].CompanyId.ToString();
                        if (i != company_GroupPermission.Count() - 1)
                        {
                            stringFilter += ",";
                        }
                    }
                    stringFilter += " )";
                }
            }

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            
            IEnumerable<DataThongKeDto> displayedData = internalService.GetListDataThongKeNewsContentFromViewExposeDto(filters, inData, out outData);

            if (outData.ContainsKey("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(outData["TotalRecords"]);
                totalPages = (int)Math.Ceiling((float)totalRecords / (float.Parse(requestParameters[CommonConstants.StrLength])));
            }

            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = displayedData
            }, JsonRequestBehavior.AllowGet);
        }
    }
}