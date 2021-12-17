using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MennuSelected = "Home";
            BaseViewModel model = new BaseViewModel();
            ResourceService resourceService = new ResourceService();
            StaffService staffService = new StaffService();
            VideoService videoService = new VideoService();
            InternalService internalService = new InternalService();
          
            model.IDicResource = resourceService.GetView();
            model.Tags = internalService.GetListDataThongKeTagsFromViewExposeDto();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            
            model.LanguageId = Utilities.GetLanguage();

            string selectedFields = "id, brandname, [order], img, fullname,position, languageId";
            inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 12, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Staffs = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);


            //newType staff
            //NewsTypeService newsTypeService = new NewsTypeService();
            //inputParam = new Dictionary<string, string>();

            //inputParam.Add(CommonConstants.StrSortedColumnNames, "orderBy ASC");
            //selectedFields = "id, code, name, show, isMennu, isactive, languageId, languageCode, orderBy";
            //inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            //inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString() + " AND code.Equals(\"" +CommonConstants.NewsType_STAFF+ "\")");

            //parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            //inData = parameters[CommonConstants.StrInData];
            //filters = parameters[CommonConstants.StrFilters];
            //requestParameters = parameters[CommonConstants.StrRequestParamters];

            //var newType = newsTypeService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();
            //ViewBag.DoiNguChuyenGia = newType != null ? newType.name : (model.LanguageId == CommonConstants.TiengViet ? "Đội ngũ chuyên gia" : "Expert team");


            //inputParam[CommonConstants.StrStringFilter] = "languageId = " + model.LanguageId.ToString() + " AND code.Equals(\"" + CommonConstants.NewsType_FEEDBACK + "\")";

            //parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            //inData = parameters[CommonConstants.StrInData];
            //filters = parameters[CommonConstants.StrFilters];
            //requestParameters = parameters[CommonConstants.StrRequestParamters];

            //newType = newsTypeService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();
            //ViewBag.Feedback = newType != null ? newType.name : (model.LanguageId == CommonConstants.TiengViet ? "Đánh giá từ Khách hàng" : "Review from Customers");


            // feddback

            //FeedbackService feedbackService = new FeedbackService();
            //inputParam = new Dictionary<string, string>();

            //selectedFields = "id, avatar, fullname,content, languageId";
            //inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            //inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString());
            //parameters = internalService.SetThamSoChoDatatable_GetList(0, 15, inputParam);
            //inData = parameters[CommonConstants.StrInData];
            //filters = parameters[CommonConstants.StrFilters];
            //requestParameters = parameters[CommonConstants.StrRequestParamters];

            //model.Feedbacks = feedbackService.GetListExposeDto(filters, inData, out outData);


            // package
            NewsContentService newsContentService = new NewsContentService();
            //model.NewsCategories = new List<NewsCategoryDto>();
            //NewsCategoryService newsCategoryService = new NewsCategoryService();
            //inputParam = new Dictionary<string, string>();

            //selectedFields = "id, slug, banner, name ";
            //inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            //inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            //inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + model.LanguageId.ToString() + " AND isHome= 1 ");

            //parameters = internalService.SetThamSoChoDatatable_GetList(0, 15, inputParam);
            //inData = parameters[CommonConstants.StrInData];
            //filters = parameters[CommonConstants.StrFilters];
            //requestParameters = parameters[CommonConstants.StrRequestParamters];

            //model.NewsCategories = newsCategoryService.GetListDataFromStoreExposeDto(filters, inData, out outData);
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            model.NewsCategories = new List<NewsCategoryDto>();
            inputParam = new Dictionary<string, string>();
            selectedFields = "id, name, typeId, banner, slug, languageId, isactive, isHome";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + model.LanguageId.ToString() + " AND isHome= 1 AND typeId = 1046");
            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsCategories = newsCategoryService.GetListDataFromStoreExposeDto(filters, inData, out outData);
            ViewData["cateList"] = model.NewsCategories;


            //Thông báo
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords,descriptions, avatar,slug, url, categoriId,TypeCode,  name, categoriName, isactive, created_by, shorttitle,description,created_at,languageId ";
            inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            //2058 tin Benh vien
            if (model.LanguageId == 1)
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 1 AND categoriId = 3082");
            }
            else
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 2 AND categoriId = 3082");
            }
            parameters = internalService.SetThamSoChoDatatable_GetList(0, 10, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.News = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            // News trong nuoc 3080

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords,descriptions, avatar, slug,url, categoriId, name, isactive,TypeCode,  categoriName, created_by, shorttitle,description,created_at,languageId ";
            inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            if (model.LanguageId == 1)
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 1 AND categoriId = 3080");
            }
            else
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 2 AND categoriId = 3080");
            }

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 10, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsTrongNuoc = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            // Tin đoàn thể 3081

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords,descriptions, avatar, slug, isactive,url, categoriId, name,TypeCode,  categoriName, created_by, shorttitle,description,created_at,languageId ";
            inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            if(model.LanguageId == 1)
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 1 AND categoriId = 3081");
            }   
            else
            {
                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = 2 AND categoriId = 3081");
            }    
           

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 10, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsQuocTe = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            // Video hoạt động
            //inputParam = new Dictionary<string, string>();

            //selectedFields = "id, title, keywords, descriptions, avatar, url, name, languageId, position";

            //inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            //inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            //inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString());

            //parameters = internalService.SetThamSoChoDatatable_GetList(0, 3, inputParam);

            //inData = parameters[CommonConstants.StrInData];
            //filters = parameters[CommonConstants.StrFilters];
            //requestParameters = parameters[CommonConstants.StrRequestParamters];

            //model.Videos = videoService.GetListExposeDto(filters, inData, out outData);




            // banner
            inputParam = new Dictionary<string, string>();
            selectedFields = "id, title, text, url, isactive, languageId, position";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            inputParam.Add(CommonConstants.StrStringFilter, "isactive = TRUE AND languageId = " + model.LanguageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 0, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            BannerService bannerService = new BannerService();
            model.Banners = bannerService.GetListExposeDto(filters, inData, out outData);

            int intValue;
            model.MemberId = -1;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                model.MemberId = intValue;
            }

            HttpCookie cookieMemberName = Request.Cookies[CommonConstants.CookieMemberName];
            if (cookieMemberName != null)
            {
                model.MemberName = HttpUtility.UrlDecode(cookieMemberName.Value);
            }
            //SEO HOME
            AppConfigService appConfigService = new AppConfigService();
            selectedFields = "ID,Title, Description, Keywords, Logo, CompanyName, Hotline, Facebook,Email";
            AppConfigDto app = appConfigService.GetByIdExposeDto(1, selectedFields);
            if (app != null)
            {
                ViewBag.Logo = app.Logo;
                ViewBag.Hotline = app.Hotline;
                ViewBag.Email = app.Email;
                ViewBag.seoTitle = app.Title;
                ViewBag.seoKeyword = app.Keywords;
                ViewBag.seoDescription = app.Description;
            }
            return View(model);
        }

        public JsonResult Language(int id)
        {
            System.Web.HttpContext.Current.Session["languageId"] = id;
            JsonResult _Json = new JsonResult();
            _Json = Json(new { msg = "success" }, JsonRequestBehavior.AllowGet);
            return _Json;
        }

        public JsonResult DangNhap(string username, string password)
        {
            UsersService memberSerivce = new UsersService();
            string resultMessage = string.Empty;
            bool loginSuccess = false;
            if (ModelState.IsValid)
            {
                var member = memberSerivce.GetByEmailOrPhoneExposeDto(username, "UserID, Active,FirstName, LastName, EmailID, Phone, Password");

                if (member != null)
                {
                    if (member.Active)
                    {
                        if (member.Password == Encryptor.Hash(password))
                        {
                            setCookieLogin(member.UserID.ToString(), (member.LastName + " " + member.FirstName), "true");
                            loginSuccess = true;
                        }
                        else
                        {
                            resultMessage = "Mật khẩu không chính xác.";
                        }
                    }
                    else
                    {
                        resultMessage = "Tài khoản chưa kích hoạt.";
                    }
                }
                else
                {
                    resultMessage = "Email hoặc số điện thoại không chính xác.";
                }
            }
            else
            {
                resultMessage = "Vui lòng nhập đầy đủ thông tin.";
            }

            return Json(new { success = loginSuccess, message = resultMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DangKy(List<string> fields, List<string> values)
        {
            UsersService usersService = new UsersService();
            UserInfoService userInfoService = new UserInfoService();

            Dictionary<string, string> getUpdatedValues = new Dictionary<string, string>();

            for (int n = 0; n < fields.Count; n++)
            {
                getUpdatedValues.Add(fields[n], values[n]);
            }

            string resultMessage = string.Empty;
            bool check = true, registerSuccess = false;
            int idValue = -1;

            string firstName = getUpdatedValues.ContainsKey("FirstName") ? getUpdatedValues["FirstName"] : string.Empty;
            string lastName = getUpdatedValues.ContainsKey("LastName") ? getUpdatedValues["LastName"] : string.Empty;
            string sodienthoai = getUpdatedValues.ContainsKey("SoDienThoai") ? getUpdatedValues["SoDienThoai"] : string.Empty;
            string email = getUpdatedValues.ContainsKey("Email") ? getUpdatedValues["Email"] : string.Empty;
            string password = getUpdatedValues.ContainsKey("Password") ? getUpdatedValues["Password"] : string.Empty;

            if (!string.IsNullOrEmpty(email) && !Utilities.checkFormatEmail(email))
            {
                check = false;
                resultMessage = "Email không đúng định dạng";
            }

            if ((!string.IsNullOrEmpty(email) && usersService.IsExistingEmail(email)) || (!string.IsNullOrEmpty(sodienthoai) && usersService.IsExistingPhone(sodienthoai)))
            {
                check = false;
                resultMessage = "Email hoặc số điện thoại " + email + " đã được đăng ký, vui lòng nhập tên khác.";
            }


            AppConfigService appConfigService = new AppConfigService();
            AppConfigDto appConfig = appConfigService.GetByActiveExposeDto("Id, Active, Email, PhoneAuthentication, MailAccount, MailPass, MailPort, MailHost, MailSSL");

            if (check)
            {
                string randomCode = Utilities.GenerateRandomCode(6, 100000);
               
                Dictionary<string, string> updatedValues = new Dictionary<string, string>();

                if (string.IsNullOrEmpty(email) && appConfig.PhoneAuthentication == false)
                {
                    updatedValues.Add("Active", "true");
                }
               

                updatedValues.Add("EmailID", email);
                updatedValues.Add("Phone", sodienthoai);
                updatedValues.Add("FirstName", firstName);
                updatedValues.Add("LastName", lastName);
                updatedValues.Add("Password", Encryptor.Hash(password));
                updatedValues.Add("ActivationCode", randomCode);

                resultMessage = usersService.AddOrUpdate(string.Empty, updatedValues);

                if (Int32.TryParse(resultMessage, out idValue))
                {
                    registerSuccess = true;
                    if(appConfig.PhoneAuthentication == false && string.IsNullOrEmpty(email))
                    {
                        HttpCookie memberIdCookie = new HttpCookie(CommonConstants.CookieMemberID);
                        memberIdCookie.Value = resultMessage;
                        memberIdCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(memberIdCookie);

                        HttpCookie memberNameCookie = new HttpCookie(CommonConstants.CookieMemberName);
                        memberNameCookie.Value = HttpUtility.UrlEncode(lastName + " " + firstName);
                        memberNameCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(memberNameCookie);
                    }
                    else if(!string.IsNullOrEmpty(email))
                    {
                        GmailService gmailService = new GmailService();

                        appConfig.MailAccount = "manhquoc00@gmail.com";
                        appConfig.MailPass = "manhquoc11a9";
                        appConfig.MailHost = "smtp.gmail.com";
                        appConfig.MailPort = "587";
                        appConfig.MailSSL = true;

                        SmtpConfigurationDto _config = new SmtpConfigurationDto();
                        _config.Username = appConfig.MailAccount;
                        _config.Password = appConfig.MailPass;
                        _config.Host = appConfig.MailHost;
                        _config.Port = Int32.Parse(appConfig.MailPort);
                        _config.Ssl = appConfig.MailSSL;

                        EmailMessageDto message = new EmailMessageDto();
                        message.ToEmail = email;
                        message.Subject = "Kich hoạt tài khoản ";
                        message.Body = "Sử dụng mã: " + randomCode + " để kích hoạt tài khoản của bạn";
                        message.IsHtml = true;

                        resultMessage = gmailService.GuiEmailToiUser(_config, message, string.Empty);
                    }
                }
            }

            return Json(new { success = registerSuccess, message = resultMessage, userId = idValue, activePhone = appConfig.PhoneAuthentication }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DangXuat(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            HttpCookie currentUserCookie = Request.Cookies[Common.CommonConstants.CookieMemberID];
            if (currentUserCookie != null)
            {
                Response.Cookies.Remove(Common.CommonConstants.CookieMemberID);
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

            return Redirect(returnUrl);
        }
        public JsonResult GuiLaiActivateCode(int userId)
        {
            UsersService usersService = new UsersService();
            UserInfoService userInfoService = new UserInfoService();

            string resultMessage = string.Empty;
            int idValue = -1;

            AppConfigService appConfigService = new AppConfigService();
            GmailService gmailService = new GmailService();

            AppConfigDto appConfig = appConfigService.GetByActiveExposeDto("Id, Active, Email, MailAccount, MailPass, MailPort, MailHost, MailSSL");
            UsersDto users = usersService.GetByIdExposeDto(userId, "Id, EmailID");

            if (users != null)
            {
                string randomCode = Utilities.GenerateRandomCode(6, 100000);

                Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                updatedValues.Add("ActivationCode", randomCode);

                resultMessage = usersService.AddOrUpdate(userId.ToString(), updatedValues);

                if (Int32.TryParse(resultMessage, out idValue))
                {
                    SmtpConfigurationDto _config = new SmtpConfigurationDto();
                    _config.Username = appConfig.MailAccount;
                    _config.Password = appConfig.MailPass;
                    _config.Host = appConfig.MailHost;
                    _config.Port = Int32.Parse(appConfig.MailPort);
                    _config.Ssl = appConfig.MailSSL;

                    EmailMessageDto message = new EmailMessageDto();
                    message.ToEmail = users.EmailID;
                    message.Subject = "Kich hoạt tài khoản ";
                    message.Body = "Sử dụng mã: " + randomCode + " để kích hoạt tài khoản của bạn";
                    message.IsHtml = true;

                    resultMessage = gmailService.GuiEmailToiUser(_config, message, string.Empty);
                }
            }
            else
            {
                resultMessage = "Không tìm thấy User";
            }

            return Json(new { message = resultMessage, userId = idValue }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActivateCode(int? userId, string activeCode)
        {
            UsersService usersService = new UsersService();
            UserInfoService userInfoService = new UserInfoService();

            string resultMessage = string.Empty;
            bool active = false, check = true;

            if (userId != null)
            {
                AppConfigService appConfigService = new AppConfigService();
                GmailService gmailService = new GmailService();

                AppConfigDto appConfig = appConfigService.GetByActiveExposeDto("Id, Active, Email, MailAccount, MailPass, MailPort, MailHost, MailSSL");
                UsersDto users = usersService.GetByIdExposeDto(userId.Value, "UserID, FirstName, LastName, ActivationCode");

                if (string.IsNullOrEmpty(activeCode))
                {
                    check = false;
                    resultMessage = "Bạn chưa nhập mã kích hoạt";
                }

                if (check)
                {
                    if (users != null)
                    {
                        active = users.ActivationCode.Equals(activeCode);

                        if (active == false)
                        {
                            resultMessage = "Mã kích hoạt không đúng";
                        }
                        else
                        {
                            Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                            updatedValues.Add("Active", active.ToString());

                            resultMessage = usersService.AddOrUpdate(userId.ToString(), updatedValues);

                            int intValue;
                            if (Int32.TryParse(resultMessage, out intValue))
                            {
                                HttpCookie memberIdCookie = new HttpCookie(CommonConstants.CookieMemberID);
                                memberIdCookie.Value = resultMessage;
                                memberIdCookie.Expires = DateTime.Now.AddMonths(1);
                                Response.Cookies.Add(memberIdCookie);

                                HttpCookie memberNameCookie = new HttpCookie(CommonConstants.CookieMemberName);
                                memberNameCookie.Value = HttpUtility.UrlEncode(users.LastName + " " + users.FirstName); 
                                memberNameCookie.Expires = DateTime.Now.AddMonths(1);
                                Response.Cookies.Add(memberNameCookie);
                            }
                        }
                    }
                }
            }

            return Json(new { success = active, message = resultMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SDTEmailActivateCode(string username, string activeCode)
        {
            UsersService usersService = new UsersService();
            UserInfoService userInfoService = new UserInfoService();

            string resultMessage = string.Empty;
            bool active = false;

            UsersDto users = usersService.GetByEmailOrPhoneExposeDto(username, "UserID, Phone, EmailID, FirstName, LastName, ActivationCode");

            if (users != null)
            {
                active = users.ActivationCode.Equals(activeCode);

                if (active == false)
                {
                    resultMessage = "Mã kích hoạt không đúng";
                }
                else
                {
                    Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                    updatedValues.Add("Active", active.ToString());

                    resultMessage = usersService.AddOrUpdate(users.UserID.ToString(), updatedValues);

                    int intValue;
                    if (Int32.TryParse(resultMessage, out intValue))
                    {
                        HttpCookie memberIdCookie = new HttpCookie(CommonConstants.CookieMemberID);
                        memberIdCookie.Value = resultMessage;
                        memberIdCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(memberIdCookie);

                        HttpCookie memberNameCookie = new HttpCookie(CommonConstants.CookieMemberName);
                        memberNameCookie.Value = HttpUtility.UrlEncode(users.LastName + " " + users.FirstName);
                        memberNameCookie.Expires = DateTime.Now.AddMonths(1);
                        Response.Cookies.Add(memberNameCookie);
                    }
                }
            }

            return Json(new {
                success = active,
                message = resultMessage,
                userId = users != null ? users.UserID : -1,
                userFirstName = users != null ? users.FirstName : string.Empty,
                userlastName = users != null ? users.LastName : string.Empty,
            }, JsonRequestBehavior.AllowGet);
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
