using ExcelDataReader;
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
    public class XML1Controller : Controller
    {
        XML1Service xML1Service = new XML1Service();


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
                sortCondition = "CreatedDate DESC";
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

            Session["XML1_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["XML1_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["XML1_Filter"] = filters;

            IEnumerable<XML1Dto> displayedData = xML1Service.GetListDataFromViewExposeDto(filters, inData, out outData);

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

        public ActionResult XemChiTietXML4(string maLK, string maDichVu)
        {
            InternalService internalService = new InternalService();
            XML1ViewModels model = new XML1ViewModels();
            model.XML3 = new XML3Dto();
            model.XML3.MA_LK = maLK;
            model.XML3.MA_DICH_VU = maDichVu;
            return View(model);
        }

        public ActionResult XemChiTiet(string maLK, string maBN)
        {
            InternalService internalService = new InternalService();
            UserInfoService userInfoService = new UserInfoService();
            XML1ViewModels model = (XML1ViewModels)internalService.KhoiTaoModel(new XML1ViewModels());

            string selectedFields = string.Empty;

            if (!string.IsNullOrEmpty(maBN))
            {
                selectedFields = "MA_BN, FirstName, LastName";

                Dictionary<string, string> filters = new Dictionary<string, string>();
                Dictionary<string, string> inData = new Dictionary<string, string>();
                Dictionary<string, string> outData = null;

                inData.Add(CommonConstants.StrSelectedFields, selectedFields);

                string stringFilter = "MA_BN.Equals(\"" + maBN + "\")";
                filters.Add("StringFilter", stringFilter);

                model.UserInfo = userInfoService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(maLK))
            {
                selectedFields = "MA_LK, STT, MA_BN, FIRSTNAME, LASTNAME, BENH_ID, TEN_BENH, MA_BENH, MA_BENHKHAC, NGAY_VAO, STRNGAY_VAO, NGAY_RA, STRNGAY_RA, NGAY_TAI_KHAM, ";
                selectedFields += "STRNGAY_TAI_KHAM, KET_QUA_DTRI, TINH_TRANG_RV, MA_KHOA, TEN_KHOA, CHUAN_DOAN, PPDIEUTRI, LOIDANTHAYTHUOC, XML1_File, GHICHU";

                Dictionary<string, string> filters = new Dictionary<string, string>();
                Dictionary<string, string> inData = new Dictionary<string, string>();
                Dictionary<string, string> outData = null;

                inData.Add(CommonConstants.StrSelectedFields, selectedFields);

                string stringFilter = "MA_LK=N'" + maLK + "'";
                filters.Add("StringFilter", stringFilter);

                model.XML1 = xML1Service.GetListDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();
            }
            return View(model);
        }



        [AllowAnonymous]
        public ActionResult XemChiTietNhanVien()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetListNhanVien(int? draw, int? start, int? length, string selectedFields, string stringFilter)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();
            int totalRecords = 0, totalPages = 0;
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoMemberModel(new UsersViewModels());

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
                sortCondition = "CreatedDate DESC";
            }
            inputParam.Add(CommonConstants.StrSortedColumnNames, sortCondition);

            if (!string.IsNullOrEmpty(selectedFields))
            {
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            }
            List<int> companyIds = new List<int>();
            UsersService userService = new UsersService();

            companyIds = userService.GetListCompanyManagement(model.UsersID);

            if (companyIds != null)
            {
                if (companyIds.Count() > 0)
                {
                    string filterCongTy = string.Empty;
                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        stringFilter += " AND ";
                    }
                    stringFilter += " CompanyId IN (";
                    stringFilter += string.Join(",", companyIds);
                    stringFilter += " )";
                }else
                {
                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        stringFilter += " AND ";
                    }
                    stringFilter += "(CompanyId = -1)";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(stringFilter))
                {
                    stringFilter += " AND ";
                }
                stringFilter += "(CompanyId = -1)";
            }

            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);

            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            IEnumerable<XML1Dto> displayedData = xML1Service.GetListDataFromViewExposeDto(filters, inData, out outData);

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