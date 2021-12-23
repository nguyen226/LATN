using ExcelDataReader;
using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class UsersController : Controller
    {
        UsersService usersService = new UsersService();

        public ActionResult Index()
        {
            InternalService internalService = new InternalService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());
            return View(model);
        }

        public ActionResult KhamSucKhoe()
        {
            InternalService internalService = new InternalService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());
            return View(model);
        }

        public ActionResult KhamSucKhoeUpdate(int? Id)
        {
            InternalService internalService = new InternalService();
            KSK_TTBENHNHANService kSK_TTBENHNHANService = new KSK_TTBENHNHANService();
            KSKTTBenhNhanViewModels model = (KSKTTBenhNhanViewModels)internalService.KhoiTaoModel(new KSKTTBenhNhanViewModels());

            if (Id != null)
            {
                Dictionary<string, string> filters = new Dictionary<string, string>(), inData = new Dictionary<string, string>(), outData = null;
                filters.Add("KSK_ID", Id.Value.ToString());

                string selectedFields = "KSK_ID,MA_LK,MA_BN , HO_TEN, NGAY_SINH,GIOI_TINH";
                selectedFields += ",CMND,DIENTHOAI,TEN_DOANKSK,CAN_NANG,CHIEU_CAO,HUYET_AP";
                selectedFields += ",MACH,KHAM_LAM_SANG,PHAN_LOAI,KET_LUAN,TU_VAN";

                inData.Add(CommonConstants.StrSelectedFields, selectedFields);
                inData.Add(CommonConstants.StrSortedColumnNames, "KSK_ID ASC");
                model.TTBN = usersService.GetListTTBNDataFromViewExposeDto(filters, inData, out outData).FirstOrDefault();

            }

            return View(model);
        }


        public ActionResult NguoiDung()
        {
            InternalService internalService = new InternalService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());
            return View(model);
        }


        public ActionResult ThemLuotKhamBenh()
        {
            InternalService internalService = new InternalService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());
            return View(model);
        }

        public ActionResult Update(int? userId)
        {
            InternalService internalService = new InternalService();
            GroupsService groupsService = new GroupsService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());

            if (userId != null)
            {
                string selectedFields = "UserId, EmailID, Phone, FirstName, GroupID, ";
                selectedFields += "LastName, Password, IsVerified, Avartar, Active, IsMobile";
                model.Users = usersService.GetByIdExposeDto(userId.Value, selectedFields);

                if (model.Users != null)
                {
                    ProvinceService provinceService = new ProvinceService();
                    DistrictService districtService = new DistrictService();
                    BloodTypeService bloodTypeService = new BloodTypeService();
                    CompanyService companyService = new CompanyService();
                    UserInfoService userInfoService = new UserInfoService();

                    selectedFields = "USER_INFO_ID, UserID, FirstName, LastName, MA_BN, DateOfBirth, ";
                    selectedFields += "CMND, BHYT, Sex, Address, Height, Occupation, weight, BloodTypeID, ";
                    selectedFields += "ProvinceID, DistrictID, IsDefault, Avartar, Ma_Cong_Ty";

                    model.UserInfo = userInfoService.GetByUserIDExposeDto(userId.Value, selectedFields);

                    if (model.UserInfo != null)
                    {
                        if (!string.IsNullOrEmpty(model.UserInfo.ProvinceID))
                        {
                            model.Province = provinceService.GetByIdExposeDto(model.UserInfo.ProvinceID, "ProvinceID, ProvinceName");
                        }

                        if (!string.IsNullOrEmpty(model.UserInfo.DistrictID))
                        {
                            model.District = districtService.GetByIdExposeDto(model.UserInfo.DistrictID, "DistrictID, DistrictName");
                        }

                        if (model.UserInfo.BloodTypeID > 0)
                        {
                            model.BloodType = bloodTypeService.GetByIdExposeDto(model.UserInfo.BloodTypeID, "BloodTypeID, BloodName");
                        }

                        if (model.UserInfo.Ma_Cong_Ty > 0)
                        {
                            model.Company = companyService.GetByIdExposeDto(model.UserInfo.Ma_Cong_Ty, "CompanyId, ComCode, ComName");
                        }
                    }

                    if (model.Users.GroupID > 0)
                    {
                        model.Groups = groupsService.GetByIdExposeDto(model.Users.GroupID, "GroupID, GroupName");
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetListKhamSucKhoe(int? draw, int? start, int? length, string selectedFields, string stringFilter)
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

            var company_GroupPermission = (List<Company_GroupPermissionDto>)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Company_GroupPermission];
            string isAdminString = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }


            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];


            IEnumerable<KSK_TTBENHNHANDto> displayedData = usersService.GetListTTBNDataFromViewExposeDto(filters, inData, out outData);

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

            string isAdminString = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }

           
            inputParam.Add(CommonConstants.StrStringFilter, stringFilter);


            parameters = internalService.SetThamSoChoDatatable_GetList(start != null ? start.Value : 0, length != null ? length.Value : 999, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            Session["Users_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["Users_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["Users_Filter"] = filters;

            IEnumerable<UsersDto> displayedData = usersService.GetListDataFromViewExposeDto(filters, inData, out outData);

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
        public ActionResult GetListNguoiDungHeThong(int? draw, int? start, int? length, string selectedFields, string stringFilter)
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

            Session["Users_SelectedFields"] = inData[CommonConstants.StrSelectedFields];
            Session["Users_SortCondition"] = inData[CommonConstants.StrSortedColumnNames];
            Session["Users_Filter"] = filters;

            IEnumerable<UsersDto> displayedData = usersService.GetListDataHeThongFromViewExposeDto(filters, inData, out outData);

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
            string resultMessage = string.Empty, username = string.Empty, password = string.Empty,
                messageAddnew = string.Empty, newUserId = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            UsersDto user = null;
            Int32 intValue;
            bool kiemTra = true;

            List<string> fieldsValue = JsonConvert.DeserializeObject<List<string>>(fields);
            List<string> valuesValue = JsonConvert.DeserializeObject<List<string>>(values);

            for (int n = 0; n < fieldsValue.Count; n++)
            {
                updatedValues.Add(fieldsValue[n], valuesValue[n]);
            }

            if (updatedValues.ContainsKey("EmailID"))
            {
                username = updatedValues["EmailID"];

                if (!Common.Services.Utilities.checkFormatEmail(username))
                {
                    resultMessage += "Định dạng Email không hợp lệ.";
                    kiemTra = false;
                }
            }

            if (updatedValues.ContainsKey("Password") && !string.IsNullOrEmpty(updatedValues["Password"]))
            {
                updatedValues["Password"] = Encryptor.Hash(updatedValues["Password"]);
                password = updatedValues["Password"];
            }

            if (kiemTra)
            {
                // cập nhật user
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out intValue))
                    {
                        user = usersService.GetByIdExposeDto(intValue, "UserID, EmailId, Phone, Password");

                        if (user != null)
                        {
                            // đổi tên truy cập
                            if (!string.IsNullOrEmpty(username) && !user.EmailID.Equals(username))
                            {
                                if (!usersService.IsExistingEmail(username))
                                {
                                    if (updatedValues.ContainsKey("EmailID"))
                                    {
                                        updatedValues["EmailID"] = username;
                                    }
                                    else
                                    {
                                        updatedValues.Add("EmailID", username);
                                    }
                                }
                                else
                                {
                                    resultMessage += "Email hoặc số điện thoại " + username + " đã được đăng ký, vui lòng nhập tên khác.";
                                    kiemTra = false;
                                }

                            }
                        }
                        else
                        {
                            resultMessage += "Không tìm thấy user theo Id " + id;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(resultMessage))
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (!string.IsNullOrEmpty(file.FileName))
                        {
                            string path = Server.MapPath(CommonConstants.DuongDanUploadFileHinhAnhUsers);

                            string fileName = System.DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            file.SaveAs(Path.Combine(path, fileName));
                            var length = new FileInfo(Path.Combine(path, fileName)).Length;
                            InternalService internalService = new InternalService();
                            updatedValues.Add("Avartar", internalService.GetUrlHost() + CommonConstants.DuongDanUploadFileHinhAnhUsers + "/" + fileName);
                        }
                    }
                }

                resultMessage = usersService.AddOrUpdate(id, updatedValues);
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddOrUpdateList(string id, List<string> fields, List<string> values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            for (int n = 0; n < fields.Count; n++)
            {
                updatedValues.Add(fields[n], values[n]);
            }

            resultMessage = usersService.AddOrUpdate(id, updatedValues);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrUpdateKhamSucKhoe(string id, List<string> fields, List<string> values)
        {
            string resultMessage = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            for (int n = 0; n < fields.Count; n++)
            {
                updatedValues.Add(fields[n], values[n]);
            }
            KSK_TTBENHNHANService kSK_TTBENHNHANService = new KSK_TTBENHNHANService();
            resultMessage = kSK_TTBENHNHANService.AddOrUpdate(id, updatedValues);
            bool isError = false;
            int idValue = 0;
            if (!Int32.TryParse(resultMessage, out idValue))
            {
                isError = true;
            }

            return Json(new
            {
                isError = isError,
                message = resultMessage
            }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = usersService.DeleteByIds(ids);
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteKhamSucKhoeByIds(List<int> ids)
        {
            KSK_TTBENHNHANService kSK_TTBENHNHANService = new KSK_TTBENHNHANService();
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = kSK_TTBENHNHANService.DeleteByIds(ids);
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
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

                    System.Data.DataTable dataTable = null;

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
                        if (ds.Tables.Count == 1)
                        {
                            dataTable = ds.Tables[0];
                        }
                        else
                        {
                            foreach (System.Data.DataTable dt in ds.Tables)
                            {
                                if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_User.ToLower())
                                {
                                    dataTable = dt;
                                    break;
                                }
                            }
                        }

                        if (dataTable != null)
                        {
                            resultMessage = usersService.NhapDuLieuTuFileExcel(dataTable);
                        }
                        else
                        {
                            resultMessage = "Không có dữ liệu.";
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


        [AllowAnonymous]
        public ActionResult XemChiTiet(int userId)
        {

            InternalService internalService = new InternalService();
            GroupsService groupsService = new GroupsService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoModel(new UsersViewModels());
            if (model.UsersID != userId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            else
            {
                string selectedFields = "UserId, EmailID, Phone, FirstName, GroupID, ";
                selectedFields += "LastName, Password, IsVerified, Avartar, Active, IsMobile";
                model.Users = usersService.GetByIdExposeDto(userId, selectedFields);

                if (model.Users != null)
                {
                    ProvinceService provinceService = new ProvinceService();
                    DistrictService districtService = new DistrictService();
                    BloodTypeService bloodTypeService = new BloodTypeService();
                    CompanyService companyService = new CompanyService();
                    UserInfoService userInfoService = new UserInfoService();

                    selectedFields = "USER_INFO_ID, UserID, FirstName, LastName, MA_BN, DateOfBirth, ";
                    selectedFields += "CMND, BHYT, Sex, Address, Height, Occupation, weight, BloodTypeID, ";
                    selectedFields += "ProvinceID, DistrictID, IsDefault, Avartar, Ma_Cong_Ty";

                    model.UserInfo = userInfoService.GetByUserIDExposeDto(userId, selectedFields);

                    if (model.UserInfo != null)
                    {
                        if (!string.IsNullOrEmpty(model.UserInfo.ProvinceID))
                        {
                            model.Province = provinceService.GetByIdExposeDto(model.UserInfo.ProvinceID, "ProvinceID, ProvinceName");
                        }

                        if (!string.IsNullOrEmpty(model.UserInfo.DistrictID))
                        {
                            model.District = districtService.GetByIdExposeDto(model.UserInfo.DistrictID, "DistrictID, DistrictName");
                        }

                        if (model.UserInfo.BloodTypeID > 0)
                        {
                            model.BloodType = bloodTypeService.GetByIdExposeDto(model.UserInfo.BloodTypeID, "BloodTypeID, BloodName");
                        }

                        if (model.UserInfo.Ma_Cong_Ty > 0)
                        {
                            model.Company = companyService.GetByIdExposeDto(model.UserInfo.Ma_Cong_Ty, "CompanyId, ComCode, ComName");
                        }
                    }

                    if (model.Users.GroupID > 0)
                    {
                        model.Groups = groupsService.GetByIdExposeDto(model.Users.GroupID, "GroupID, GroupName");
                    }
                }

            }

            return View(model);
        }

        public ActionResult DoiMatKhau(int userId, string matKhauCu, string matKhauMoi)
        {
            string resultMessage = string.Empty, matKhauCuValue = string.Empty, matKhauMoiValue = string.Empty;
            Dictionary<string, string> updatedValues = new Dictionary<string, string>();

            UsersDto user = null;

            matKhauCuValue = Encryptor.Hash(matKhauCu);
            matKhauMoiValue = Encryptor.Hash(matKhauMoi);

            user = usersService.GetByIdExposeDto(userId, "UserID, Password");

            if (user != null)
            {
                if (user.Password == matKhauCuValue)
                {
                    updatedValues.Add("Password", matKhauMoiValue);

                    resultMessage = usersService.AddOrUpdate(userId.ToString(), updatedValues);
                }
                else
                {
                    resultMessage = "Mật khẩu cũ không đúng";
                }
            }
            else
            {
                resultMessage = "Không tìm thấy người dùng";
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
            return Json(usersService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult NhapDuLieuTuFileExcelXetNghiem()
        {
            string resultMessage = string.Empty;
            KSK_TTBENHNHANService kSK_TTBENHNHANService = new KSK_TTBENHNHANService();
            KSK_CDHAService kSK_CDHAService = new KSK_CDHAService();
            KSK_XNCTMService kSK_XNCTMService = new KSK_XNCTMService();
            KSK_XNKService kSK_XNKService = new KSK_XNKService();
            KSK_XNNTService kSK_XNNTService = new KSK_XNNTService();


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

                    System.Data.DataTable dataTable = null;

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
                            if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_KCB.ToLower())
                            {
                                resultMessage += kSK_TTBENHNHANService.NhapDuLieuTuFileExcel(dt);

                            }
                            else if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_CDHA.ToLower())
                            {
                                resultMessage += kSK_CDHAService.NhapDuLieuTuFileExcel(dt);
                            }
                            else if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_XNK.ToLower())
                            {
                                resultMessage += kSK_XNKService.NhapDuLieuTuFileExcel(dt);

                            }
                            else if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_XNNT.ToLower())
                            {
                                resultMessage += kSK_XNNTService.NhapDuLieuTuFileExcel(dt);

                            }
                            else if (dt.TableName.Trim().ToLower() == CommonConstants.ImportData_SheetName_XNCTM.ToLower())
                            {
                                resultMessage += kSK_XNCTMService.NhapDuLieuTuFileExcel(dt);
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