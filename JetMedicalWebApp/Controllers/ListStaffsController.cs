
using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class ListStaffsController : Controller
    {
        //
        // GET: /ListStaffs/

        public ActionResult Index(string ten, int? phongBanId, int pageIndex = 1)
        {
            BaseViewModel model = new BaseViewModel();
            model.LanguageId = Utilities.GetLanguage();
            ViewBag.TitleHeader = "Chuyên gia Bệnh viện 199";
            ViewBag.Title = "Chuyên gia";
            ViewBag.Description = "Nội dung đang cập nhật";

          
            StaffService staffService = new StaffService();
            ResourceService resourceService = new ResourceService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            int pageSize = 12;

            string selectedFields = "id, img, brandname,  fullname, [order], description, departmentid, isactive, position";

            
            string filter = "languageId = " + model.LanguageId.ToString() + "  ";

            if (!string.IsNullOrEmpty(ten))
            {
                filter += "fullname like N'%" + ten + "%'";
                model.Text = ten;
            }
            else
            {
                filter += "AND fullname like N'%" + ten + "%'";
                model.Text = ten;
            }
            

            if (phongBanId.HasValue && phongBanId.Value > 0)
            {               
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " AND ";
                }
                filter += "departmentid = " + phongBanId;
                model.PhongBanId = phongBanId.Value;

                DepartmentService departmentService = new DepartmentService();
                model.TextPhongBan = departmentService.GetByIdExposeDto(phongBanId.Value, "id, name").name;
            }
            else
            {
                model.PhongBanId = -1;
            }


            inputParam.Add(CommonConstants.StrSortedColumnNames, "[order] asc");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, filter);

            parameters = internalService.SetThamSoChoDatatable_GetList((pageIndex - 1) * pageSize, pageSize, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];

            ViewData["staffs"] = staffService.GetListDataFromViewExposeDto(filters, inData, out outData);

            int totalRecords = 0;
            if (outData.ContainsKey("TotalRecords"))
            {
                totalRecords = Convert.ToInt32(outData["TotalRecords"]);
            }

            model.PageIndex = pageIndex;
            model.TongSoTrang = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            model.Url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;

            model.IDicResource = resourceService.GetView();


            //newType staff
            NewsTypeService newsTypeService = new NewsTypeService();
            inputParam = new Dictionary<string, string>();

            inputParam.Add(CommonConstants.StrSortedColumnNames, "orderBy ASC");
            selectedFields = "id, code, name, show, isMennu, isactive, languageId, languageCode, orderBy";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 1, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            var newType = newsTypeService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();

            ViewBag.DoiNguChuyenGia = newType != null ? newType.name : (languageId == CommonConstants.TiengViet ? "Đội ngũ chuyên gia" : "Expert team");

            return View(model);
        }

        public ActionResult List(int id) // id_categori
        {
            ViewBag.TitleHeader = "";
            ViewBag.Title = "";
            ViewBag.typeId = id;

            string selectedFields = "id, name, typeId, languageId";
            NewsCategoryService newsCategoryService = new NewsCategoryService();
            var category = newsCategoryService.GetByIdExposeDto(id, selectedFields);

            if (category != null)
            {
                ViewBag.TitleHeader = category.name;
                ViewBag.Title = category.name;
            }

            return View();
        }
    }
}
