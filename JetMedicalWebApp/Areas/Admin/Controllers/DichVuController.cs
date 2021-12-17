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
    public class DichVuController : Controller
    {
        ServiceService serviceService = new ServiceService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            ServiceViewModels model = (ServiceViewModels)internalService.KhoiTaoModel(new ServiceViewModels());
            return View(model);
        }

        public ActionResult Update(int? id, int? languageId, string languageCode)
        {
            InternalService internalService = new InternalService();
            ServiceViewModels model = (ServiceViewModels)internalService.KhoiTaoModel(new ServiceViewModels());
            Dictionary<string,string> filters = new Dictionary<string, string>();
            Dictionary<string,string> inData = new Dictionary<string, string>();
            Dictionary<string,string> outData = new Dictionary<string, string>();
            model.LanguageId = CommonConstants.TiengViet;
            if (languageId.HasValue)
            {
                model.LanguageId = languageId.Value;
            }

            string selectedFields = string.Empty;
            model.ServiceCode = languageCode;
            if (id != null)
            {
                selectedFields = "id, code, keywords, ActivePhone, descriptions, name,title, categoriId, languageId,";
                selectedFields += "shorttitle, position,isactive,ishot, approved,avatar,description, Tags, created_at";
                var content = serviceService.GetByIdExposeDto(id.Value, selectedFields);
                
                if(content != null)
                {
                    model.ServiceCode = content.code;
                }
            }

            if (!string.IsNullOrEmpty(model.ServiceCode))
            {
                NewsCategoryService newsCategoryService = new NewsCategoryService();

                string stringFilter = string.Empty;
                stringFilter = "code =\"" + model.ServiceCode + "\" AND languageId = " + model.LanguageId.ToString();

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
                model.Service = serviceService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();

                if (model.Service != null)
                {
                    model.NewsCategory = newsCategoryService.GetByIdExposeDto(model.Service.categoriId, "id, name");
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

            Session["Service_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["Service_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["Service_Filter"] = filters;

            IEnumerable<ServiceDto> displayedData = serviceService.GetListDataFromViewExposeDto(filters, inData, out outData);

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
        public ActionResult AddOrUpdate(ServiceDto data)
        {
            string resultMessage = string.Empty;
            resultMessage = serviceService.AddOrUpdate(data);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = serviceService.DeleteByIds(ids);
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
            return Json(serviceService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}