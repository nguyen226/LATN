
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
    public class ListNewsController : Controller
    {
        //
        // GET: /ListNews/

        public ActionResult Index(int id, int pageIndex = 1)
        {
            ViewBag.TitleHeader = "";
            ViewBag.Title = "";
            ViewBag.categoriId = id;

            BaseViewModel model = new BaseViewModel();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            int _typeId = 0, pageSize = 12;
            string selectedFields = string.Empty;

            model.NewsContents = new List<NewsContentDto>();
            model.NewsCategories = new List<NewsCategoryDto>();

            selectedFields = "id, name, typeId, languageId";

            var category = newsCategoryService.GetByIdExposeDto(id, selectedFields);

            if (category != null)
            {
                _typeId = category.typeId;
                ViewData["categori"] = category;
            }

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND typeId=" + _typeId.ToString());
            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsCategories = newsCategoryService.GetListExposeDto(filters, inData, out outData);
            ViewData["cateList"] = model.NewsCategories;


            selectedFields = "id, name, shorttitle, description, isactive, slug,categoriId, TypeId, TypeName, created_at, avatar, languageId";
            inputParam = new Dictionary<string, string>();
            parameters = new Dictionary<string, Dictionary<string, string>>();
            inData = new Dictionary<string, string>();
            filters = new Dictionary<string, string>();
            requestParameters = new Dictionary<string, string>();

            inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND TypeId=" + _typeId.ToString() + " AND categoriId=" + id.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsContents = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            ViewData["newsList"] = model.NewsContents;

            int totalRecords = 0;
            if (outData.ContainsKey("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(outData["TotalRecords"]);
            }

            model.PageIndex = pageIndex;
            model.TongSoTrang = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            model.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            return View(model);
        }
        public ActionResult List(int id) // id_categori
        {
            ViewBag.TitleHeader = "";
            ViewBag.Title = "";
            ViewBag.typeId = id;

            string selectedFields = "id, name, typeId, languageId";
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            var category = newsCategoryService.GetByIdExposeDto(id, selectedFields);

            if (category != null)
            {
                ViewBag.TitleHeader = category.name;
                ViewBag.Title = category.name;
            }

            return View();
        }
        public ActionResult Tag(string id, int pageIndex = 1)
        {
            ViewBag.TitleHeader = "Tag";
            ViewBag.Title = "";

            BaseViewModel model = new BaseViewModel();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            int _typeId = 0, pageSize = 12;
            string selectedFields = string.Empty;


            TagService tagService = new TagService();
            var tag = tagService.getByTagId(id);

            ViewBag.tagId = id;


            model.NewsContents = new List<NewsContentDto>();

            selectedFields = "id, name, shorttitle, description, slug,categoriId,isactive, TypeId, TypeName, created_at,TypeCode, Tags, avatar, languageId";
            inputParam = new Dictionary<string, string>();
            parameters = new Dictionary<string, Dictionary<string, string>>();
            inData = new Dictionary<string, string>();
            filters = new Dictionary<string, string>();
            requestParameters = new Dictionary<string, string>();

            inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND Tags LIKE N'%" + id + "%'");

            parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];


            model.NewsContents = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            ViewData["newsList"] = model.NewsContents;

            int totalRecords = 0;
            if (outData.ContainsKey("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(outData["TotalRecords"]);
            }

            model.PageIndex = pageIndex;
            model.TongSoTrang = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            model.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            return View(model);
        }
    }
}
