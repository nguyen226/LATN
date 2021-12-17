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
    public class ChienDichController : Controller
    {
        ChienDichService chienDichService = new ChienDichService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            ChienDichViewModels model = (ChienDichViewModels)internalService.KhoiTaoModel(new ChienDichViewModels());
            return View(model);
        }

        public ActionResult Update(int? id)
        {
            InternalService internalService = new InternalService();
            ChienDichViewModels model = (ChienDichViewModels)internalService.KhoiTaoModel(new ChienDichViewModels());

            string selectedFields = string.Empty;


            if (id != null)
            {
                selectedFields = "Id, PatientGroupID, NotificationID, Type, Content, ModifiedUsers.EmailID AS EmailNguoiCapNhat";

                model.ChienDich = chienDichService.GetByIdExposeDto(id.Value, selectedFields);
                if (model.ChienDich != null)
                {
                    if (model.ChienDich.PatientGroupID > 0)
                    {
                        PatientGroupService patientGroupService = new PatientGroupService();
                        model.PatientGroup = patientGroupService.GetByIdExposeDto(model.ChienDich.PatientGroupID, "Id, Name");
                    }

                    if (model.ChienDich.NotificationID > 0)
                    {
                        NotificationService notificationService = new NotificationService();
                        model.Notification = notificationService.GetByIdExposeDto(model.ChienDich.NotificationID, "NoID, NoTitle");
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
                sortCondition = "Id ASC";
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

            Session["ChienDich_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["ChienDich_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["ChienDich_Filter"] = filters;

            IEnumerable<ChienDichDto> displayedData = chienDichService.GetListDataFromViewExposeDto(filters, inData, out outData);

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

        //[HttpPost]
        //public ActionResult AddOrUpdate(string id, List<string> fields, List<string> values)
        //{
        //    string resultMessage = string.Empty;
        //    Dictionary<string, string> updatedValues = new Dictionary<string, string>();

        //    for (int n = 0; n < fields.Count; n++)
        //    {
        //        updatedValues.Add(fields[n], values[n]);
        //    }

        //    resultMessage = chienDichService.AddOrUpdate(id, updatedValues);

        //    return Json(resultMessage, JsonRequestBehavior.AllowGet);
        //}


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
                        updatedValues.Add("FileDinhKem", internalService.GetUrlHost() + Common.CommonConstants.DuongDanThuMucTapTin + "/" + fileName);
                    }
                }
            }

            resultMessage = chienDichService.AddOrUpdate(id, updatedValues);
            return Json(resultMessage, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = chienDichService.DeleteByIds(ids);
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult XemChiTiet(int id)
        {
            InternalService internalService = new InternalService();
            ChienDichViewModels model = (ChienDichViewModels)internalService.KhoiTaoModel(new ChienDichViewModels());
            string selectedFields = string.Empty;
            selectedFields = "Id";
            model.ChienDich = chienDichService.GetByIdExposeDto(id, selectedFields);
            return View(model);
        }
    }
}