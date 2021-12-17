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
    public class XML4Controller : Controller
    {
        XML4Service xML4Service = new XML4Service();

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
                sortCondition = "XML4ID ASC";
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

            Session["XML4_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["XML4_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["XML4_Filter"] = filters;

            IEnumerable<XML4Dto> displayedData = xML4Service.GetListDataFromViewExposeDto(filters, inData, out outData);

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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdate(string id, string fields, string values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            List<string> fieldsValue = JsonConvert.DeserializeObject<List<string>>(fields);
            List<string> valuesValue = JsonConvert.DeserializeObject<List<string>>(values);

            for (int n = 0; n < fieldsValue.Count; n++)
            {
                updatedValues.Add(fieldsValue[n], valuesValue[n]);
            }


            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        string path = Server.MapPath(Common.CommonConstants.DuongDanThuMucTapTin);

                        string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(Path.Combine(path, fileName));
                        var length = new FileInfo(Path.Combine(path, fileName)).Length;
                        InternalService internalService = new InternalService();
                        updatedValues.Add("XML4_01", internalService.GetUrlHost() + CommonConstants.DuongDanThuMucTapTin + "/" + fileName);
                    }
                }
            }

            resultMessage = xML4Service.AddOrUpdate(id, updatedValues);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = xML4Service.DeleteByIds(ids);
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}