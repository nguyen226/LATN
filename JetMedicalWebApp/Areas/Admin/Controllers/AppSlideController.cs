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
    public class AppSlideController : Controller
    {
        AppSlideService appSlideService = new AppSlideService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            AppSlideViewModels model = (AppSlideViewModels)internalService.KhoiTaoModel(new AppSlideViewModels());
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

            Session["PhieuChiTienChiTiet_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["PhieuChiTienChiTiet_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["PhieuChiTienChiTiet_Filter"] = filters;

            IEnumerable<AppSlideDto> displayedData = appSlideService.GetListDataFromViewExposeDto(filters, inData, out outData);

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

        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            AppSlideViewModels model = (AppSlideViewModels)internalService.KhoiTaoModel(new AppSlideViewModels());

            string selectedFields = string.Empty;

            if (id != null)
            {
                selectedFields = "Id, Images, Active, Odx, NoCategory, NoID ";
                model.AppSlide = appSlideService.GetByIdExposeDto(id.Value, selectedFields);
            }

            if(model.AppSlide != null && model.AppSlide.NoID > 0)
            {
                NewsContentService newsContentService = new NewsContentService();
                model.NewsContent = newsContentService.GetByIdExposeDto(model.AppSlide.NoID, "id, name");
            }

            return View(model);
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
                        string path = Server.MapPath(Common.CommonConstants.DuongDanUploadFileSlide);

                        string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        file.SaveAs(Path.Combine(path, fileName));
                        var length = new FileInfo(Path.Combine(path, fileName)).Length;
                        InternalService internalService = new InternalService();
                        
                        updatedValues.Add("Images", internalService.GetUrlHost() + Common.CommonConstants.DuongDanUploadFileSlide + "/" + fileName);
                    }
                }
            }

            resultMessage = appSlideService.AddOrUpdate(id, updatedValues);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = appSlideService.DeleteByIds(ids);
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

            return Json(appSlideService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}