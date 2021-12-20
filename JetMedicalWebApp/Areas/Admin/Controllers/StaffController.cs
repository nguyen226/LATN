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
    public class StaffController : Controller
    {
        StaffService staffService = new StaffService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            StaffViewModels model = (StaffViewModels)internalService.KhoiTaoModel(new StaffViewModels());
            return View(model);
        }
        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            DepartmentService departmentService = new DepartmentService();

            StaffViewModels model = (StaffViewModels)internalService.KhoiTaoModel(new StaffViewModels());
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = new Dictionary<string, string>();

            string selectedFields = string.Empty;

            model.LanguageId = CommonConstants.TiengViet;

            if (id != null)
            {
                selectedFields = "id, code, img, brandname, departmentid,  fullname, position,summary, description, languageId,";
                selectedFields += "isactive, Isexamination, newscategoryid, order";

                model.Staff = staffService.GetByIdExposeDto(id.Value, selectedFields);

                if (model.Staff != null)
                {
                    if (model.Staff.newscategoryid.HasValue && model.Staff.newscategoryid.Value > 0)
                    {
                        var category = newsCategoryService.GetByIdExposeDto(model.Staff.newscategoryid.Value, "id, name");

                        if (category != null)
                        {
                            model.Staff.newcategoryname = category.name;
                        }
                    }

                    if (model.Staff.departmentid > 0)
                    {
                        var dep = departmentService.GetByIdExposeDto(model.Staff.departmentid, "id, name");

                        if (dep != null)
                        {
                            model.Staff.departmentname = dep.name;
                        }
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
                
                inputParam.Add(CommonConstants.StrSortedColumnNames, (sortColumn == "order" ? "[" + sortColumn +"]" : sortColumn) + " " + sortColumnDir);
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

            Session["Staff_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["Staff_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["Staff_Filter"] = filters;

            IEnumerable<StaffDto> displayedData = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);

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
        public ActionResult AddOrUpdate(List<StaffDto> listItem)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            resultMessage = staffService.AddOrUpdate(listItem);

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

            resultMessage = staffService.UpdateByIds(updatedData);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = staffService.DeleteByIds(ids);
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
            return Json(staffService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListByCode(string code, string selectedFields)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }

            inputParam.Add(CommonConstants.StrStringFilter, "code =\"" + code + "\"");
            inputParam.Add(CommonConstants.StrSortedColumnNames, "Id desc");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            IEnumerable<StaffDto> displayedData = staffService.GetListExposeDto(filters, inData, out outData);
            return Json(staffService.GetListExposeDto(filters, inData, out outData), JsonRequestBehavior.AllowGet);
        }


    }
}