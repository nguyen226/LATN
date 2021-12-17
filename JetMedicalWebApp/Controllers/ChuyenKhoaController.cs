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
    public class ChuyenKhoaController : Controller
    {
        public ActionResult Index()
        {
            BaseViewModel model = new BaseViewModel();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            // get newscategory chuyen khoa => news type == 2  bậc 1

            model.NewsCategories = new List<NewsCategoryDto>();
            string selectedFields = "id, parentId, slug, name, parent_ids, child_ids, typeId, languageId, banner";

            int languageId = Utilities.GetLanguage();

            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND typeId=" + (languageId == CommonConstants.TiengViet ? "1042" : "1042"));
            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.NewsCategories = newsCategoryService.GetListDataFromStoreExposeDto(filters, inData, out outData);

            return View(model);
        }

        public ActionResult List(int id, string slug, int pageIndex = 1)
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

            selectedFields = "id, name, typeId, slug, parentId, languageId";

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
            ViewBag.Slug = slug;

            selectedFields = "id, name, shorttitle, description, isactive, slug,categoriId, TypeId, TypeName, created_at, avatar, languageId";

            if (slug == "gioi-thieu")
            {
                model.IsList = false;
                inputParam = new Dictionary<string, string>();
                parameters = new Dictionary<string, Dictionary<string, string>>();
                inData = new Dictionary<string, string>();
                filters = new Dictionary<string, string>();
                requestParameters = new Dictionary<string, string>();

                inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                inputParam.Add(CommonConstants.StrStringFilter, "slug = '" + slug + "' AND isactive = 1 AND languageId = " + languageId.ToString() + " AND TypeId=" + _typeId.ToString() + " AND categoriId=" + id.ToString());
                parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);
                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];
                model.NewsContent = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(slug) && slug== "doi-ngu-bac-si")
            {
                StaffService staffService = new StaffService();

                selectedFields = "id, fullname, img, brandname, description, isactive,order , languageId, newscategoryid";
                model.IsList = true;

                inputParam = new Dictionary<string, string>();
                parameters = new Dictionary<string, Dictionary<string, string>>();
                inData = new Dictionary<string, string>();
                filters = new Dictionary<string, string>();
                requestParameters = new Dictionary<string, string>();

                inputParam.Add(CommonConstants.StrSortedColumnNames, "order asc");
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                inputParam.Add(CommonConstants.StrStringFilter, " isactive = true AND languageId = " + languageId.ToString() + " AND newscategoryid =" + id.ToString());
                parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);
                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];
                model.Staffs = staffService.GetListExposeDto(filters, inData, out outData);

                int totalRecords = 0;
                if (outData.ContainsKey("TotalRecords"))
                {
                    totalRecords = Convert.ToInt32(outData["TotalRecords"]);
                }
                model.PageIndex = pageIndex;
                model.TongSoTrang = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            }
            else
            {
                model.IsList = true;

                // list tin tức

                inputParam = new Dictionary<string, string>();
                parameters = new Dictionary<string, Dictionary<string, string>>();
                inData = new Dictionary<string, string>();
                filters = new Dictionary<string, string>();
                requestParameters = new Dictionary<string, string>();

                inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                inputParam.Add(CommonConstants.StrStringFilter, "slug = '" + slug + "' AND isactive = 1 AND languageId = " + languageId.ToString() + " AND TypeId=" + _typeId.ToString() + " AND categoriId=" + id.ToString());

                parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);
                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];
                model.NewsContents = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

                int totalRecords = 0;
                if (outData.ContainsKey("TotalRecords"))
                {
                    totalRecords = Convert.ToInt32(outData["TotalRecords"]);
                }
                model.PageIndex = pageIndex;
                model.TongSoTrang = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            }

            model.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            return View(model);
        }



        public ActionResult ChiTietTinTuc(int idct)
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

            inputParam.Add(CommonConstants.StrStringFilter, "id=" + idct.ToString());

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
                ViewBag.url = newsDetail.slug + "_ckct_" + newsDetail.id;

                ViewBag.categoriId = newsDetail.categoriId;

                categori = cateList.FirstOrDefault(m => m.id == newsDetail.categoriId);

                // News other
                inputParam = new Dictionary<string, string>();

                selectedFields = "id,  avatar, url, categoriId, slug,isactive, fullshorttitle, fullname, created_at, TypeCode, TypeId";

                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

                inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND languageId = " + languageId.ToString() + " AND id <> " + idct.ToString() + " AND categoriId=" + newsDetail.categoriId.ToString());

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


                if (!string.IsNullOrEmpty(newsDetail.GoiKham))
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
                    inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND (" + stringFilter + ")");
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


        public ActionResult ChiTietBacSi(int idct)
        {
            StaffService staffService = new StaffService();
            InternalService internalService = new InternalService();
            ResourceService resourceService = new ResourceService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();

            ViewBag.TitleHeader = "Chuyên gia";
            ViewBag.Title = "Chuyên gia";
            ViewBag.Title = "Thông tin Chuyên gia";
            ViewBag.Description = "Nội dung đang cập nhật";
            ViewBag.Isexamination = false;

            int languageId = Utilities.GetLanguage();

            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null, requestParametersOther = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;

            string selectedFields = "id, img, brandname, fullname, [order], Isexamination, description, newscategoryid, isactive, position";

            inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "id = " + idct.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            StaffDto staffDetail = staffService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();
            ViewData["StaffDetail"] = staffDetail;

            if (staffDetail != null)
            {
                ViewBag.Title = staffDetail.fullname;
                ViewBag.Description = staffDetail.description;
                ViewBag.brandname = staffDetail.brandname;
                ViewBag.img = staffDetail.img;
                ViewBag.StaffId = staffDetail.id;
                ViewBag.Isexamination = staffDetail.Isexamination;

                if (staffDetail.newscategoryid.HasValue && staffDetail.newscategoryid.Value > 0)
                {

                    selectedFields = "id, name, typeId, slug, parentId, languageId";
                    var category = newsCategoryService.GetByIdExposeDto(staffDetail.newscategoryid.Value, selectedFields);
                    ViewBag.categoriId = category.id;
                    ViewData["categori"] = category;
                }
            }
            inputParam = new Dictionary<string, string>();

            //inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "id <> " + staffDetail.id + " AND isactive = 1");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 15, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParametersOther = parameters[CommonConstants.StrRequestParamters];

            var staffDetailOthersStaff = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);

            ViewData["StaffDetailsOther"] = staffDetailOthersStaff;

            ViewData["IDicResource"] = resourceService.GetView();

            int intValue;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                ViewData["MemberId"] = intValue;
            }

            return View();
        }
    }
}
