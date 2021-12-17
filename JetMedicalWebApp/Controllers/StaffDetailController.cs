using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class StaffDetailController : Controller
    {
        public ActionResult Index(int id)
        {
            StaffService staffService = new StaffService();
            InternalService internalService = new InternalService();
            ResourceService resourceService = new ResourceService();

            ViewBag.TitleHeader = "Chuyên gia";
            ViewBag.Title = "Chuyên gia";
            ViewBag.Title = "Thông tin Chuyên gia";
            ViewBag.Description = "Nội dung đang cập nhật";
            ViewBag.Isexamination = false;

            int languageId = Utilities.GetLanguage();

            Dictionary<string, string>  inputParam = new Dictionary<string, string>();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null, requestParametersOther = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;

            string selectedFields = "id, img, brandname, fullname, [order], Isexamination, description, departmentid, isactive, position";

            inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "id = " + id.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            StaffDto staffDetail = staffService.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

            if (staffDetail != null)
            {
                ViewBag.Title = staffDetail.fullname;
                ViewBag.Description = staffDetail.description;
                ViewBag.brandname = staffDetail.brandname;
                ViewBag.img = staffDetail.img;
                ViewBag.StaffId = staffDetail.id;
                ViewBag.Isexamination = staffDetail.Isexamination;

                if (staffDetail.departmentid > 0)
                {
                    inputParam = new Dictionary<string, string>();

                    inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
                    inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                    inputParam.Add(CommonConstants.StrStringFilter, "id <> " + staffDetail.id + " AND departmentid = " + staffDetail.departmentid.ToString() + " and isactive = 1");

                    parameters = internalService.SetThamSoChoDatatable_GetList(0, 4, inputParam);

                    inData = parameters[CommonConstants.StrInData];
                    filters = parameters[CommonConstants.StrFilters];
                    requestParameters = parameters[CommonConstants.StrRequestParamters];

                    var staffDetails = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);

                    ViewData["StaffDetails"] = staffDetails;
                }
                
               
                   
            }
            inputParam = new Dictionary<string, string>();

            //inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "id <> " + staffDetail.id + " AND isactive = 1");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 15, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParametersOther = parameters[CommonConstants.StrRequestParamters];

            var staffDetailOthersStaff = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);

            ViewData["StaffDetailsOther"] = staffDetailOthersStaff;

            ViewData["IDicResource"] = resourceService.GetView();

            int intValue;

            HttpCookie cookieMemberID = Request.Cookies[CommonConstants.CookieMemberID];
            if (cookieMemberID != null)
            {
                Int32.TryParse(cookieMemberID.Value, out intValue);
                ViewData["MemberId"] = intValue;
            }

            return View();
        }

    }
}
