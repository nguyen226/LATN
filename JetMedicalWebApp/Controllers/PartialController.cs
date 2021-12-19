using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult _Footer(string typeCode)
        {
            PartialViewModel model = new PartialViewModel();

            NewsCategoryService newsCategoryService = new NewsCategoryService();
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();

            string selectedFields = "id, name, typeName, typeCode, languageId";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND typeCode='" + typeCode + "'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.NewsCategories = newsCategoryService.GetListDataFromViewExposeDto(filters, inData, out outData);

            return PartialView(model);
        }
        public PartialViewResult _Header()
        {


            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            InternalService internalService = new InternalService();
            PartialViewModel model = new PartialViewModel();

            NewsTypeService newsTypeService = new NewsTypeService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();


            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;

            int intValue;

            model.LanguageId = Utilities.GetLanguage();

            inputParam.Add(CommonConstants.StrSortedColumnNames, "orderBy ASC");

            string selectedFields = "id, code, name, show, isMennu, isactive, languageId, languageCode, orderBy";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString() + " AND isMennu=true");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.NewsTypes = newsTypeService.GetListExposeDto(filters, inData, out outData);
            model.NewsCategories = new List<NewsCategoryDto>();

            if (model.NewsTypes != null)
            {
                selectedFields = "id, title, keywords, descriptions, url, name, parentId, description, typeId, typeName, banner, isHome, isactive, languageId, code";

                foreach (var item in model.NewsTypes)
                {
                    inputParam = new Dictionary<string, string>();

                    inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                    inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + model.LanguageId.ToString() + " AND isactive=1 AND typeId=" + item.id.ToString());

                    inputParam.Add(CommonConstants.StrSortedColumnNames, "typeId ASC");

                    parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

                    inData = parameters[CommonConstants.StrInData];
                    filters = parameters[CommonConstants.StrFilters];
                    requestParameters = parameters[CommonConstants.StrRequestParamters];

                    model.NewsCategories.AddRange(newsCategoryService.GetListDataFromViewExposeDto(filters, inData, out outData));
                }
            }

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
           

            return PartialView(model);
        }
        public PartialViewResult _LeftAbout(string typeCode, int? id, int packageId = 0)
        {
            PartialViewModel model = new PartialViewModel();

            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsContentService newsContentService = new NewsContentService();
            VideoService videoService = new VideoService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            int _typeId = 0;
            string selectedFields = string.Empty;

            model.NewsContents = new List<NewsContentDto>();
            model.Packages = new List<NewsContentDto>();

            ViewBag.Id = id != null ? id.Value : -1;

            if (id != null)
            {
                selectedFields = "id, name, typeId, languageId";

                var category = newsCategoryService.GetByIdExposeDto(id.Value, selectedFields);

                if (category != null)
                {
                    _typeId = category.typeId;
                }

                selectedFields = "id, name, description, slug,categoriId, TypeId, TypeName, languageId";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeId=" + _typeId);

                parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                model.NewsContents = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);
            }

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, avatar, categoriId, fullname, created_at, TypeCode, languageId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeCode=N'" + CommonConstants.NewsType_Package + "'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Packages = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar, url, name, languageId, position";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Videos = videoService.GetListExposeDto(filters, inData, out outData);

            return PartialView(model);
        }
        public PartialViewResult _LeftLayout(int id = 0)
        {
            PartialViewModel model = new PartialViewModel();

            NewsContentService newsContentService = new NewsContentService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsTypeService newsTypeService = new NewsTypeService();
            VideoService videoService = new VideoService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            ViewBag.categoriId = id;
            ViewBag.Name = string.Empty;
            ViewBag.Code = string.Empty;
            ViewBag.Show = string.Empty;

            if (id > 0)
            {
                int _typeId = -1;
                selectedFields = "id, name, typeId, languageId";

                var category = newsCategoryService.GetByIdExposeDto(id, selectedFields);

                if (category != null)
                {
                    _typeId = category.typeId;

                    selectedFields = "id, name, typeId, languageId";

                    inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                    inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeId=" + _typeId);

                    parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

                    inData = parameters[CommonConstants.StrInData];
                    filters = parameters[CommonConstants.StrFilters];
                    requestParameters = parameters[CommonConstants.StrRequestParamters];

                    model.NewsCategories = newsCategoryService.GetListDataFromViewExposeDto(filters, inData, out outData);

                    NewsTypeDto typeDetail = newsTypeService.GetByIdExposeDto(category.typeId, "id, code, name, show, isMennu, isactive, languageId, languageCode");
                    ViewBag.Name = typeDetail.name;
                    ViewBag.Code = typeDetail.code.ToLower();
                    ViewBag.Show = typeDetail.show;
                }
            }

            model.Packages = new List<NewsContentDto>();

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, avatar, categoriId, fullname, created_at, TypeCode, languageId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeCode=N'" + CommonConstants.NewsType_Package + "'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Packages = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar, url, name, languageId, position";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Videos = videoService.GetListExposeDto(filters, inData, out outData);

            return PartialView(model);
        }

        public PartialViewResult _LeftChuyenKhoaLayout(int id = 0)
        {
            PartialViewModel model = new PartialViewModel();

            NewsContentService newsContentService = new NewsContentService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsTypeService newsTypeService = new NewsTypeService();
            VideoService videoService = new VideoService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            ViewBag.categoriId = id;
            ViewBag.Name = string.Empty;
            ViewBag.Code = string.Empty;
            ViewBag.Show = string.Empty;

            if (id > 0)
            {
                int _typeId = -1;
                selectedFields = "id, name, typeId, languageId";

                var category = newsCategoryService.GetByIdExposeDto(id, selectedFields);

                if (category != null)
                {
                    _typeId = category.typeId;
                    ViewData["categori"] = category;
                }

                selectedFields = "id, code, name, typeId, typeName, slug, parentId, isHome,isactive, odx, parent_ids, ActivePhone, languageId, ISNULL(CreatedUserID,-1) AS CreatedUserID";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeId=" + _typeId);

                parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                model.NewsCategories = newsCategoryService.GetListDataFromStoreExposeDto(filters, inData, out outData);

                NewsTypeDto typeDetail = newsTypeService.GetByIdExposeDto(category.typeId, "id, code, name, show, isMennu, isactive, languageId, languageCode");
                ViewBag.Name = typeDetail.name;
                ViewBag.Code = typeDetail.code.ToLower();
                ViewBag.Show = typeDetail.show;
            }

            model.Packages = new List<NewsContentDto>();

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, avatar, categoriId, fullname, created_at, TypeCode, languageId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeCode=N'" + CommonConstants.NewsType_Package + "'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Packages = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar, url, name, languageId, position";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Videos = videoService.GetListExposeDto(filters, inData, out outData);

            return PartialView(model);
        }

        public PartialViewResult _LeftLayoutStaff()
        {
            PartialViewModel model = new PartialViewModel();

            NewsContentService newsContentService = new NewsContentService();
            VideoService videoService = new VideoService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            model.Packages = new List<NewsContentDto>();

            inputParam = new Dictionary<string, string>();

            selectedFields = "id, avatar, categoriId, fullname, created_at, TypeCode, languageId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND TypeCode=N'" + CommonConstants.NewsType_Package + "'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Packages = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);


            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, avatar, url, name, languageId, position";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 5, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            model.Videos = videoService.GetListExposeDto(filters, inData, out outData);

            return PartialView(model);
        }
    }
}