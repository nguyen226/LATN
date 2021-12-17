using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class NewsDetailController : Controller
    {
        //
        // GET: /NewsDetail/

        public ActionResult Index(int id)
        {
            ViewBag.categoriId = 0;
            ViewBag.TitleHeader = "";
            ViewBag.Title = "";
            ViewBag.Description = "";

            NewsContentService newsContentService = new NewsContentService();
            NewsTypeService newsTypeService = new NewsTypeService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();


            // Get News Content
            string selectedFields = "id, keywords, slug,descriptions, avatar, url, categoriId, name, ";
            selectedFields += "shorttitle, fullshorttitle, fullname, description, created_at, ";
            selectedFields += "created_by, approved, isactive, position, nView, ishot, TypeId, GoiKham";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "id=" + id.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            NewsContentDto newsDetail = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();


            // Get News cate
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, url, name, typeId, banner, isHome, isactive";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "typeId=" + newsDetail.TypeId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            List<NewsCategoryDto> cateList = newsCategoryService.GetListDataFromViewExposeDto(filters, inData, out outData);
            ViewData["cateList"] = cateList;

            NewsCategoryDto categori = new NewsCategoryDto();

            List<NewsContentDto> newsOther = new List<NewsContentDto>();
            if (newsDetail.id > 0)
            {
                newsContentService.TangLuotXem(newsDetail.id);
                ViewBag.seoTitle = newsDetail.fullname;
                ViewBag.seoKeyword = newsDetail.keywords;
                ViewBag.seoDescription = newsDetail.descriptions;

                ViewBag.Title = newsDetail.fullname;
                ViewBag.Description = newsDetail.description;
                ViewBag.avartar = newsDetail.avatar;
                ViewBag.url = newsDetail.slug + "_dt_" + newsDetail.id;

                ViewBag.categoriId = newsDetail.categoriId;

                categori = cateList.FirstOrDefault(m => m.id == newsDetail.categoriId);

                // News other
                inputParam = new Dictionary<string, string>();

                selectedFields = "id,  avatar, url, categoriId, slug,isactive, fullshorttitle, fullname, created_at, TypeCode, TypeId";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id <> " + id.ToString() + " AND categoriId=" + newsDetail.categoriId.ToString());

                inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");

                parameters = internalService.SetThamSoChoDatatable_GetList(0, 6, inputParam);

                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                newsOther = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

                // Type detail
                NewsTypeDto typeDetail = newsTypeService.GetByIdExposeDto(newsDetail.categoriId, "id, code, name, show, isMennu, isactive, languageId, languageCode");
                ViewBag.Name = string.Empty;
                ViewBag.Code = string.Empty;
                ViewBag.Show = string.Empty;
                if (typeDetail != null)
                {
                    ViewBag.Name = typeDetail.name;
                    ViewBag.Code = typeDetail.code.ToLower();
                    ViewBag.Show = typeDetail.show;
                }


                ViewBag.Tags = newsContentService.GetListTagByTinTucId(newsDetail.id);


                if(!string.IsNullOrEmpty(newsDetail.GoiKham))
                {
                    // News other
                    inputParam = new Dictionary<string, string>();

                    selectedFields = "id, title, name, keywords, slug,descriptions, avatar, url, categoriId, fullshorttitle, fullname, description, isactive, position, priceOld, priceSale, created_at, TypeCode, TypeId";


                    var arrayIds = newsDetail.GoiKham.Split(';');

                    string stringFilter = string.Empty;
                    foreach (var arrayId in arrayIds)
                    {
                        if (!string.IsNullOrEmpty(stringFilter))
                        {
                            stringFilter += " OR ";
                        }
                        stringFilter += " id = " + arrayId;
                    } 
                    inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND (" +stringFilter + ")");
                    inputParam.Add(CommonConstants.StrSortedColumnNames, "position ASC");
                    parameters = internalService.SetThamSoChoDatatable_GetList(0, 12, inputParam);
                    inData = parameters[CommonConstants.StrInData];
                    filters = parameters[CommonConstants.StrFilters];
                    requestParameters = parameters[CommonConstants.StrRequestParamters];
                    ViewData["PackagesOther"] = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);
                }
            }

            ViewData["categori"] = categori;
            ViewData["newsOther"] = newsOther;

            int intValue;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                ViewData["MemberId"] = intValue;
            }
            return View();
        }

        public ActionResult Cate(int id)
        {
            NewsContentService newsContentService = new NewsContentService();
            NewsTypeService newsTypeService = new NewsTypeService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();

            ViewBag.categoriId = id;
            ViewBag.TitleHeader = "";
            ViewBag.Title = "";
            ViewBag.Description = "";


            // Get News Content
            string selectedFields = "id, keywords, descriptions, avatar, slug,url, categoriId, name, shorttitle, fullshorttitle, fullname, description, created_at, created_by, approved, isactive, position, nView, ishot, TypeId";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "categoriId=" + id.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            NewsContentDto newsDetail = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

            // Get News cate
            inputParam = new Dictionary<string, string>();

            selectedFields = "id, title, keywords, descriptions, url, name, typeId, banner, isHome, isactive";

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "typeId=" + newsDetail.TypeId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            List<NewsCategoryDto> cateList = newsCategoryService.GetListDataFromViewExposeDto(filters, inData, out outData);

            ViewData["cateList"] = cateList;

            NewsCategoryDto categori = new NewsCategoryDto();
            List<NewsContentDto> newsOther = new List<NewsContentDto>();
            if (newsDetail.id > 0)
            {
                ViewBag.seoTitle = newsDetail.fullname;
                ViewBag.seoKeyword = newsDetail.keywords;
                ViewBag.seoDescription = newsDetail.descriptions;

                ViewBag.Title = newsDetail.fullname;
                ViewBag.Description = newsDetail.description;

                categori = cateList.FirstOrDefault(m => m.id == newsDetail.categoriId);
            }

            // Type detail
            NewsTypeDto typeDetail = newsTypeService.GetByIdExposeDto(categori.typeId, "id, code, name, show, isMennu, isactive, languageId, languageCode");
            ViewBag.Name = string.Empty;
            ViewBag.Code = string.Empty;
            ViewBag.Show = string.Empty;
            if (typeDetail != null)
            {
                ViewBag.Name = typeDetail.name;
                ViewBag.Code = typeDetail.code.ToLower();
                ViewBag.Show = typeDetail.show;
            }

            ViewData["categori"] = categori;
            ViewData["newsOther"] = newsOther;
            return View("~/Views/NewsDetail/Index.cshtml");
        }
    }
}
