using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    [LogActionFilter]
    [CustomizeAuthorize]
    public class DaoTaoController : Controller
    {
        NewsContentService newsContentService = new NewsContentService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            NewsContentViewModels model = (NewsContentViewModels)internalService.KhoiTaoModel(new NewsContentViewModels());
            return View(model);
        }

        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            NewsContentViewModels model = (NewsContentViewModels)internalService.KhoiTaoModel(new NewsContentViewModels());
            Dictionary<string,string> filters = new Dictionary<string, string>();
            Dictionary<string,string> inData = new Dictionary<string, string>();
            Dictionary<string,string> outData = new Dictionary<string, string>();

            string selectedFields = string.Empty;

            if (id != null)
            {
                selectedFields = "id, code, keywords,ActivePhone, descriptions, name,title, categoriId, languageId,";
                selectedFields += "shorttitle, position,isactive,ishot, approved,avatar,description";
                var content = newsContentService.GetByIdExposeDto(id.Value, selectedFields);
                
                if(content != null)
                {
                    NewsCategoryService newsCategoryService = new NewsCategoryService();
                    List<NewsContentDto> contentList = null;

                    if (!string.IsNullOrEmpty(content.code))
                    {
                        string stringFilter = string.Empty;
                        // lấy tất cả newcontent theo code ( tiếng anh , việt )
                        contentList = new List<NewsContentDto>();
                        stringFilter = "code =\"" + content.code+"\"";

                        Dictionary<string, Dictionary<string, string>> parameters = null;
                        Dictionary<string, string> inputParam = new Dictionary<string, string>();
                        if (!string.IsNullOrEmpty(selectedFields))
                        {
                            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                        }
                        inputParam.Add(CommonConstants.StrSortedColumnNames, "id asc");
                        inputParam.Add(CommonConstants.StrStringFilter, stringFilter);
                        parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
                        inData = parameters[CommonConstants.StrInData];
                        filters = parameters[CommonConstants.StrFilters];
                        contentList = newsContentService.GetListExposeDto(filters, inData, out outData).ToList();
                    }
                    
                    if(contentList != null)
                    {
                        if(contentList.Count() > 0)
                        {
                            //model.NewsContentVN = contentList.Where(m => m.languageId == CommonConstants.TiengViet).FirstOrDefault();
                            //model.NewsContent = contentList.Where(m => m.languageId == CommonConstants.TiengAnh).FirstOrDefault();


                            if (contentList.Where(m => m.languageId == CommonConstants.TiengViet).FirstOrDefault() != null)
                            {
                                model.NewsContentVN = new NewsContentDto();
                                model.NewsContentVN = contentList.Where(m => m.languageId == CommonConstants.TiengViet).FirstOrDefault();
                            }

                            if (contentList.Where(m => m.languageId == CommonConstants.TiengAnh).FirstOrDefault() != null)
                            {
                                model.NewsContent = new NewsContentDto();
                                model.NewsContent = contentList.Where(m => m.languageId == CommonConstants.TiengAnh).FirstOrDefault();
                            }
                        }
                    }
                    else
                    {
                        if(content.languageId == CommonConstants.TiengViet)
                        {
                            model.NewsContentVN = new NewsContentDto();
                            model.NewsContentVN = content;
                        }

                        if (content.languageId == CommonConstants.TiengAnh)
                        {
                            model.NewsContent = new NewsContentDto();
                            model.NewsContent = content;
                        }
                    }

                    if(model.NewsContentVN != null)
                    {
                        model.NewsCategoryVN = newsCategoryService.GetByIdExposeDto(model.NewsContentVN.categoriId, "id, name");
                    }

                    if (model.NewsContent != null)
                    {
                        model.NewsCategory = newsCategoryService.GetByIdExposeDto(model.NewsContent.categoriId, "id, name");
                    }

                    if (model.NewsContentVN == null)
                    {
                        model.NewsContentVN = new NewsContentDto();
                    }

                    if (model.NewsContent == null)
                    {
                        model.NewsContent = new NewsContentDto();
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetList(int? draw, int? start, int? length, string selectedFields, string stringFilter)
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

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            Session["NewsContent_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["NewsContent_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["NewsContent_Filter"] = filters;

            IEnumerable<NewsContentDto> displayedData = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

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
        public ActionResult AddOrUpdate(List<NewsContentDto> listItem)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            resultMessage = newsContentService.AddOrUpdate(listItem);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrUpdateList(int id, List<string> fields, List<string> values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();
            Dictionary<int, Dictionary<string, string>> updatedData = new Dictionary<int, Dictionary<string, string>>();
            for (int n = 0; n < fields.Count; n++)
            {
                updatedValues.Add(fields[n], values[n]);
            }

            updatedData.Add(id, updatedValues);

            resultMessage = newsContentService.UpdateByIds(updatedData);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = newsContentService.DeleteByIds(ids);
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Filter(string selectedFields, string stringFilter, string sortCondition, string top)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> outData = null;
            Dictionary<string, string> input = new Dictionary<string, string>();

            input.Add(CommonConstants.StrSelectedFields, selectedFields);
            input.Add(CommonConstants.StrStringFilter, stringFilter);
            input.Add(CommonConstants.StrSortedColumnNames, sortCondition);
            if (!string.IsNullOrEmpty(top))
            {
                input.Add(CommonConstants.StrTop, top);
            }
            Dictionary<string, Dictionary<string, string>> parameters = internalService.SetThamSoChoFilter(input);
            return Json(newsContentService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}