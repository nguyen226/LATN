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
    public class AppConfigController : Controller
    {
        AppConfigService appConfigService = new AppConfigService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            AppConfigViewModels model = (AppConfigViewModels)internalService.KhoiTaoModel(new AppConfigViewModels());

            if (!string.IsNullOrEmpty((string)Session["GetListAppConfig_NgayBatDauLocDuLieuFilter"]))
            {
                model.NgayBatDauLocDuLieu = (string)Session["GetListAppConfig_NgayBatDauLocDuLieuFilter"];
            }
            else
            {
                model.NgayBatDauLocDuLieu = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, 1).ToString("dd/MM/yyyy");
            }

            if (!string.IsNullOrEmpty((string)Session["GetListAppConfig_NgayKetThucLocDuLieuFilter"]))
            {
                model.NgayKetThucLocDuLieu = (string)Session["GetListAppConfig_NgayKetThucLocDuLieuFilter"];
            }
            else
            {
                model.NgayKetThucLocDuLieu = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, DateTime.DaysInMonth(System.DateTime.Now.Year, System.DateTime.Now.Month)).ToString("dd/MM/yyyy");
            }

            return View(model);
        }

        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            AppConfigViewModels model = (AppConfigViewModels)internalService.KhoiTaoModel(new AppConfigViewModels());

            string selectedFields = string.Empty;


            if (id != null)
            {
                selectedFields = "ID, CompanyName, Logo, Active, PhoneAuthentication, Hotline, Facebook, Zalo, Website, Email, SmsAccount, SmsPass, ";
                selectedFields += "SmsLink, MailAccount, MailPass, MailHost, MailSSL, MailPort, Introduce, Privacy, TermsOfUse, Support, ModifiedUsers.EmailID AS EmailNguoiCapNhat";

                model.AppConfig = appConfigService.GetByIdExposeDto(id.Value, selectedFields);
            }

            return View(model);
        }

        public ActionResult AppConfigUpdate(int? id)
        {
            InternalService internalService = new InternalService();
            AppConfigViewModels model = (AppConfigViewModels)internalService.KhoiTaoModel(new AppConfigViewModels());

            string selectedFields = string.Empty;


            if (id != null)
            {
                selectedFields = "ID, CompanyName, Logo, Active, PhoneAuthentication, Hotline, Facebook, Zalo, Website, Email, SmsAccount, SmsPass, ";
                selectedFields += "SmsLink, MailAccount, MailPass, MailHost, MailSSL, MailPort, Introduce, Privacy, TermsOfUse, Support, ModifiedUsers.EmailID AS EmailNguoiCapNhat, Title, Description, Keywords";

                model.AppConfig = appConfigService.GetByIdExposeDto(id.Value, selectedFields);
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

            Session["AppConfig_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["AppConfig_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["AppConfig_Filter"] = filters;

            IEnumerable<AppConfigDto> displayedData = appConfigService.GetListExposeDto(filters, inData, out outData);

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
                        string path = Server.MapPath(Common.CommonConstants.DuongDanUploadFileLogo);

                        string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(Path.Combine(path, fileName));
                        var length = new FileInfo(Path.Combine(path, fileName)).Length;

                        InternalService internalService = new InternalService();
                        
                        updatedValues.Add("Logo", internalService.GetUrlHost() + Common.CommonConstants.DuongDanUploadFileLogo+ "/"+ fileName);
                    }
                }
            }

            resultMessage = appConfigService.AddOrUpdate(id, updatedValues);

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
                resultMessage = appConfigService.DeleteByIds(idValues);
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}