using ExcelDataReader;
using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
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
    public class UsersController : Controller
    {
        UsersService usersService = new UsersService();

        public ActionResult Update(int? userId)
        {
            InternalService internalService = new InternalService();
            GroupsService groupsService = new GroupsService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoMemberModel(new UsersViewModels());

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

        [AllowAnonymous]
        public ActionResult XemChiTiet(int userId)
        {
            InternalService internalService = new InternalService();
            GroupsService groupsService = new GroupsService();
            UsersViewModels model = (UsersViewModels)internalService.KhoiTaoMemberModel(new UsersViewModels());
            if (model.UsersID != userId)
            {
                return RedirectToAction("AccessDenied", "Home");
            }else
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

            if(user != null)
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
    }
}