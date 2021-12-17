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

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    [LogActionFilter]
    [CustomizeAuthorize]
    public class XML1Controller : Controller
    {
        XML1Service xML1Service = new XML1Service();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            XML1ViewModels model = (XML1ViewModels)internalService.KhoiTaoModel(new XML1ViewModels());

            return View(model);
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

        public ActionResult Update(string maLK, string maBN)
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

            var company_GroupPermission = (List<Company_GroupPermissionDto>)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission];
            string isAdminString = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }

            if (company_GroupPermission != null && !string.IsNullOrEmpty(isAdminString) && isAdminString == "false")
            {
                if (company_GroupPermission.Count() > 0)
                {
                    string filterCongTy = string.Empty;
                    if (!string.IsNullOrEmpty(stringFilter))
                    {
                        stringFilter += " AND ";
                    }
                    stringFilter += " CompanyId IN (";

                    for (int i = 0; i < company_GroupPermission.Count(); i++)
                    {
                        stringFilter += company_GroupPermission[i].CompanyId.ToString();
                        if (i != company_GroupPermission.Count() - 1)
                        {
                            stringFilter += ",";
                        }
                    }
                    stringFilter += " )";
                }
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

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddOrUpdate(string id, string fields, string values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();
            bool isError;

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
                        updatedValues.Add("XML1_File", internalService.GetUrlHost() +  Common.CommonConstants.DuongDanThuMucTapTin + "/" + fileName);
                    }
                }
            }

            resultMessage = xML1Service.AddOrUpdate(id, updatedValues, out isError);

            return Json(new {
                isError = isError,
                message = resultMessage
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(IList<string> ids)
        {
            string resultMessage = string.Empty;
            ids = ids.Where(m => !string.IsNullOrEmpty(m)).ToList();

            if (ids.Count() > 0)
            {
                resultMessage = xML1Service.DeleteByMA_LKs(ids.ToList());
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLastestMaLuotKham()
        {
            return Json(xML1Service.GetLastestMaLuotKham(null), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult NhapDuLieuTuFileExcel()
        {
            string resultMessage = string.Empty;

            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    Stream stream = file.InputStream;
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });

                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        resultMessage = "Không đọc được file.";
                    }

                    reader.Close();

                    if (ds.Tables.Count > 0)
                    {
                        foreach (System.Data.DataTable dt in ds.Tables)
                        {
                            switch (dt.TableName.Trim().ToLower())
                            {
                                case "xml1":
                                case "chitietkham":
                                    if (!string.IsNullOrEmpty(resultMessage))
                                    {
                                        resultMessage += "\n";
                                    }
                                    resultMessage += xML1Service.NhapDuLieuTuFileExcel(dt);
                                    break;

                                case "xml2":
                                    XML2Service xML2Service = new XML2Service();

                                    if (!string.IsNullOrEmpty(resultMessage))
                                    {
                                        resultMessage += "\n";
                                    }
                                    resultMessage += xML2Service.NhapDuLieuTuFileExcel(dt);
                                    break;

                                case "xml3":
                                    XML3Service xML3Service = new XML3Service();

                                    if (!string.IsNullOrEmpty(resultMessage))
                                    {
                                        resultMessage += "\n";
                                    }
                                    resultMessage += xML3Service.NhapDuLieuTuFileExcel(dt);
                                    break;

                                case "xml4":
                                    XML4Service xML4Service = new XML4Service();

                                    if (!string.IsNullOrEmpty(resultMessage))
                                    {
                                        resultMessage += "\n";
                                    }
                                    resultMessage += xML4Service.NhapDuLieuTuFileExcel(dt);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        resultMessage = "Không có dữ liệu.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}