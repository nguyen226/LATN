using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    [LogActionFilter]
    [CustomizeAuthorize]
    public class CauHinhEmailController : Controller
    {
        CauHinhEmailService CauHinhEmailService = new CauHinhEmailService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            CauHinhEmailViewModels model = (CauHinhEmailViewModels)internalService.KhoiTaoModel(new CauHinhEmailViewModels());
            return View(model);
        }

        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            CauHinhEmailViewModels model = (CauHinhEmailViewModels)internalService.KhoiTaoModel(new CauHinhEmailViewModels());

            string selectedFields = string.Empty;


            if (id != null)
            {
                selectedFields = "Id, Host, Port, Active, Account, Password, SSL ";
                model.CauHinhEmail = CauHinhEmailService.GetByIdExposeDto(id.Value, selectedFields);
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
            string sortColumnDir = string.Empty;

            if (Request.Form.GetValues("order[0][column]") != null)
            {
                if (Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]") != null)
                {
                    sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                }
            }

            if (Request.Form.GetValues("order[0][dir]") != null)
            {
                sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }
            else
            {
                inputParam.Add(CommonConstants.StrSelectedFields, "ID, CompanyName, Active, ModifiedUsers.EmailID AS EmailNguoiCapNhat");
            }

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);
            inputParam.Add(CommonConstants.StrSortedColumnNames, sortColumn + " " + sortColumnDir);

            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            Session["CauHinhEmail_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["CauHinhEmail_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["CauHinhEmail_Filter"] = filters;

            IEnumerable<CauHinhEmailDto> displayedData = CauHinhEmailService.GetListExposeDto(filters, inData, out outData);

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
        public ActionResult AddOrUpdate(string id, List<string>fields, List<string> values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();
          
            for (int n = 0; n < fields.Count; n++)
            {
                updatedValues.Add(fields[n], values[n]);
            }

            resultMessage = CauHinhEmailService.AddOrUpdate(id, updatedValues);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(IList<string> ids)
        {
            string resultMessage = string.Empty;
            List<int> idValues = new List<int>();
            int intValue = 0;

            foreach (string item in ids)
            {
                if (Int32.TryParse(item, out intValue))
                {
                    idValues.Add(intValue);
                }
            }

            if (idValues.Count() > 0)
            {
                resultMessage = CauHinhEmailService.DeleteByIds(idValues);
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}