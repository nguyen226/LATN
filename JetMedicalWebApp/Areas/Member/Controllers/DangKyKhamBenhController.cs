using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Member.Controllers
{
    public class DangKyKhamBenhController : Controller
    {
        RegisterServiceService registerServiceService = new RegisterServiceService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            RegisterServiceViewModels model = (RegisterServiceViewModels)internalService.KhoiTaoMemberModel(new RegisterServiceViewModels());
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

            string sortCondition = string.Empty;

            if (Request.Form.GetValues("order[0][dir]") != null)
            {
                var tempFormDatas = Request.Form.AllKeys.Where(m => m.StartsWith("order")).ToList();

                if (tempFormDatas.Count() % 2 == 0)
                {
                    for (int i = 0; i < tempFormDatas.Count(); i += 2)
                    {
                        if (!string.IsNullOrEmpty(sortCondition))
                        {
                            sortCondition += ", ";
                        }

                        sortCondition += Request.Form.GetValues("columns[" + Request.Form.GetValues(tempFormDatas[i]).FirstOrDefault() + "][name]").FirstOrDefault() + " " + Request.Form.GetValues(tempFormDatas[i + 1]).FirstOrDefault();
                    }
                }
            }

             if (string.IsNullOrEmpty(sortCondition))
            {
                sortCondition = "RegisterID ASC";
            }
            inputParam.Add(CommonConstants.StrSortedColumnNames, sortCondition);

            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            IEnumerable<RegisterServiceDto> displayedData = registerServiceService.GetListDataFromViewExposeDto(filters, inData, out outData);

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
        public ActionResult AddOrUpdate(string id, List<string> fields, List<string> values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            for (int n = 0; n < fields.Count; n++)
            {
                updatedValues.Add(fields[n], values[n]);
            }

            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieMemberID];
            if (authCookie != null)
            {
                updatedValues.Add("UserID", authCookie.Value);
                updatedValues.Add("CreatedUserID", authCookie.Value);
            }
            bool res = false;
            resultMessage = registerServiceService.AddOrUpdate(id, updatedValues, out res);

            return Json(new { success = res , data = resultMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult KhungGioKhamBenh(string ngay)
        {
            string resultMessage = string.Empty;
            Dictionary<string, int> updatedValues = new Dictionary<string, int>();
            DateTime ngayValue;
            ngayValue = (DateTime.TryParseExact(ngay, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayValue) ? ngayValue : DateTime.Now);
            updatedValues = registerServiceService.GetRegisterNoLastestByTime(ngayValue);
            return Json(updatedValues, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteById(int id)
        {
            string resultMessage = string.Empty;
          
            resultMessage = registerServiceService.DeleteById(id);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}