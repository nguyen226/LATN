using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Services;
using JetMedicalWebApp.Common;
using System.Linq;
using JetMedicalWebApp.Models;
using System.Web;

namespace JetMedicalWebApp.Controllers
{
    public class SignedServiceController : Controller
    {
        SignedServiceService signedServiceService = new SignedServiceService();
        public ActionResult Index(int id = 0)
        {
            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, name, title, keywords, slug,descriptions, avatar, url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode";

            inputParam.Add(CommonConstants.StrSortedColumnNames, "created_at DESC");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND TypeCode=N'" + CommonConstants.NewsType_Package + "'" + (id > 0 ? " AND categoriId=" + id : ""));

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            ViewData["PackageList"] = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            int intValue;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                ViewData["MemberId"] = intValue;
            }

            return View();
        }

        public ActionResult Detail(int id, NewsContentDto model)
        {
            NewsContentService newsContentService = new NewsContentService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsTypeService newsTypeService = new NewsTypeService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            // content detail
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar,slug, url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id=" + id);

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

            ViewBag.categoriId = model.categoriId;

            // Type detail
            NewsTypeDto typeDetail = newsTypeService.GetByIdExposeDto(model.TypeId, "id, code, name, show, isMennu, isactive, languageId, languageCode");
            ViewBag.Name = typeDetail.name;
            ViewBag.Code = typeDetail.code.ToLower();
            ViewBag.Show = typeDetail.show;

            ViewBag.seoTitle = model.name;
            ViewBag.seoKeyword = model.keywords;
            ViewBag.seoDescription = model.descriptions;

            ViewBag.avartar = model.avatar;
            ViewBag.url = model.slug + "_pdt_" + model.id;

            // News other
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions,slug, avatar, url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id <> " + id.ToString() + " AND categoriId=" + model.categoriId.ToString());

            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 12, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            ViewData["newsOther"] = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            int intValue;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                ViewData["MemberId"] = intValue;
            }





            ViewBag.Tags = newsContentService.GetListTagByTinTucId(model.id);


            return View(model);
        }

        [HttpPost]
        public JsonResult dangkygoikham(int id, SignedServiceDto model)
        {
            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;
            Boolean Mailsuccess = false;

            // content detail
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar,slug, url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id=" + id);

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            ViewBag.isPost = true;

            var detail = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

            model.name = detail.name;
            model.priceOld = detail.priceOld;
            model.priceSale = detail.priceSale;
            model.title = detail.title;

            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            updatedValues.Add("code", "GK-" + DateTime.Now.ToString("yyyyMMddHHmmss.fffffff"));
            updatedValues.Add("fullname", model.fullname);
            updatedValues.Add("mobile", model.mobile);
            updatedValues.Add("email", "GK-" + model.email);
            updatedValues.Add("date", DateTime.Now.ToString("dd-MM-yyyy"));
            updatedValues.Add("packageId", model.id.ToString());

            var result = signedServiceService.AddOrUpdate(string.Empty, updatedValues);
            
            if (string.IsNullOrEmpty(result))
            {
                Mailsuccess = true;
                //Dangkylichhen(hoten, sodienthoai, checkin, time);
            }
            return Json(new { success = Mailsuccess }, JsonRequestBehavior.AllowGet);
        }
        public static CaptchaResponse ValidateCaptcha(string response)
        {
            //string secret = System.Web.Configuration.WebConfigurationManager.AppSettings["6Lfr19AUAAAAAKM5lIWIR_rB3peXqx9BTCJw9JmE"];
            string secret = "6Lfr19AUAAAAAKM5lIWIR_rB3peXqx9BTCJw9JmE";
            var client = new System.Net.WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        public ActionResult Register(int id, SignedServiceDto model)
        {
            ViewBag.Error = "";
            ViewBag.Success = "";
            ViewBag.isPost = false;
            ViewBag.Id = id;

            if (Request.HttpMethod == "POST")
            {
                NewsContentService newsContentService = new NewsContentService();

                InternalService internalService = new InternalService();
                Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
                Dictionary<string, Dictionary<string, string>> parameters = null;
                Dictionary<string, string> inputParam = new Dictionary<string, string>();

                int languageId = Utilities.GetLanguage();
                string selectedFields = string.Empty;

                // content detail
                inputParam = new Dictionary<string, string>();

                selectedFields = "id, title, keywords, descriptions, avatar, slug,url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id=" + id);

                parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                var detail = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

                ViewBag.isPost = true;

                model.name = detail.name;
                model.priceOld = detail.priceOld;
                model.priceSale = detail.priceSale;
                model.title = detail.title;

                CaptchaResponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
                if (response.Success == false || ModelState.IsValid == false)
                {
                    ViewBag.Error = "Vui lòng xác nhận bạn không phải người máy!";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Dictionary<string, string> updatedValues = new Dictionary<string, string>();

                        updatedValues.Add("code", "GK-" + DateTime.Now.ToString("yyyyMMddHHmmss.fffffff"));
                        updatedValues.Add("fullname", model.fullname);
                        updatedValues.Add("mobile", model.mobile);
                        updatedValues.Add("email", "GK-" + model.email);
                        updatedValues.Add("date", DateTime.Now.ToString("dd-MM-yyyy"));
                        updatedValues.Add("packageId", model.id.ToString());

                        var result = signedServiceService.AddOrUpdate(string.Empty, updatedValues);

                        if (string.IsNullOrEmpty(result))
                        {
                            ViewBag.Success = "Thêm mới thông tin thành công.";
                        }
                        else
                        {
                            ViewBag.Error = "Thêm mới thông tin không thành công.";
                        }
                    }
                }
            }
            else
            {
                NewsContentService newsContentService = new NewsContentService();

                InternalService internalService = new InternalService();
                Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
                Dictionary<string, Dictionary<string, string>> parameters = null;
                Dictionary<string, string> inputParam = new Dictionary<string, string>();

                int languageId = Utilities.GetLanguage();
                string selectedFields = string.Empty;

                // content detail
                inputParam = new Dictionary<string, string>();

                selectedFields = "id, title, keywords, descriptions, avatar, slug,url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id=" + id);

                parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                var detail = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

                model.id = detail.id;
                model.name = detail.name;
                model.priceOld = detail.priceOld;
                model.priceSale = detail.priceSale;
                model.title = detail.title;
                model.keywords = detail.keywords;
                model.descriptions = detail.descriptions;
                model.description = detail.description;
                model.shorttitle = detail.shorttitle;
            }

            return View(model);
        }
       
        public ActionResult Comfirm(int id, SignedServiceDto model)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, code, fullname, mobile, email, date, packageId, packageName, packageCode, packagePrice, created_at";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "id=" + id);

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model = signedServiceService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();
            return View(model);
        }
    }
}
