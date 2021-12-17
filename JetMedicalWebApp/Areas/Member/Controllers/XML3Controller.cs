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

namespace JetMedicalWebApp.Areas.Member.Controllers
{
    public class XML3Controller : Controller
    {
        XML3Service xML3Service = new XML3Service();

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

            if (!string.IsNullOrEmpty(sortCondition))
            {
                sortCondition = "XML3ID ASC";
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

            Session["XML3_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["XML3_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["XML3_Filter"] = filters;

            IEnumerable<XML3Dto> displayedData = xML3Service.GetListDataFromViewExposeDto(filters, inData, out outData);

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

    }
}