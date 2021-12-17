using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Web;
using System.Globalization;
using System.IO;

namespace JetMedicalWebApp.Services
{
    public class UserInfoService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private string maTable = "UserInfo";
        private string tenTable = "UserInfo";

        public List<UserInfoDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<UserInfoDto> result = new List<UserInfoDto>();
            string stringFilter = string.Empty, usernameNguoiCapNhat = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.UserInfoRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

            if (filters != null)
            {
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            query = query.AsQueryable().Where(item.Value);
                            break;

                        default:
                            UserInfoDto obj = new UserInfoDto();
                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                query = query.AsQueryable().Where(item.Key + "=@0", item.Value);
                            }
                            else
                            {
                                query = query.AsQueryable().Where(item.Key + "=" + item.Value);
                            }

                            break;
                    }
                }
            }

            if (inData != null)
            {
                if (inData.ContainsKey(CommonConstants.StrTotalRecords))
                {
                    outData.Add(CommonConstants.StrTotalRecords, query.Count().ToString());
                }

                if (inData.ContainsKey(CommonConstants.StrSortedColumnNames))
                {
                    if (!string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]))
                    {
                        query = query.AsQueryable().OrderBy(inData[CommonConstants.StrSortedColumnNames]);
                    }
                }

                if (inData.ContainsKey("Skip"))
                {
                    query = query.AsQueryable().Skip(Convert.ToInt32(inData["Skip"]));
                }

                if (inData.ContainsKey("Take"))
                {
                    query = query.AsQueryable().Take(Convert.ToInt32(inData["Take"]));
                }
            }

            foreach (dynamic item in query)
            {
                UserInfoDto obj = new UserInfoDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "DateOfBirth":

                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);

                            break;

                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }
        public IEnumerable<UserInfoDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public UserInfoDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "USER_INFO_ID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        public UserInfoDto GetByUsernameExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Username=\"" + username + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public UserInfoDto GetByUserIDExposeDto(int userId, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "UserID= " + userId;
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public bool IsExisting(string maBenhNhan)
        {
            return unitOfWork.UserInfoRepository.CountDataRow(m => m.MA_BN.Trim().ToLower().Equals(maBenhNhan.Trim().ToLower())) > 0;
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.UserInfoRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }
        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;

            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryService = new AppHistoryService();

                try
                {
                    List<string> imageNames = new List<string>();
                    var query = unitOfWork.UserInfoRepository.Get(m => ids.Contains(m.USER_INFO_ID)).ToList();
                    if (query != null)
                    {
                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "USER_INFO_ID: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            if (!string.IsNullOrEmpty(item.Avartar))
                            {
                                imageNames.Add(item.Avartar);
                            }
                        }

                        unitOfWork.UserInfoRepository.DeleteRange(query);
                        unitOfWork.Save();
                    }

                    if (imageNames.Count() > 0)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanUploadFileHinhAnhUsers);
                        foreach (string imageName in imageNames)
                        {
                            var array = imageName.Split('/');
                            string fileName = array[imageName.Split('/').Length - 1];

                            string tempPath = Path.Combine(path, fileName);
                            if (File.Exists(tempPath))
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                File.Delete(tempPath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }

            return result;
        }

        public string UpdateByIds(Dictionary<int, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryService = new AppHistoryService();
                var query = unitOfWork.UserInfoRepository.Get(m => ids.Contains(m.USER_INFO_ID)).ToList();
                foreach (UserInfo obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.USER_INFO_ID])
                    {
                        switch (item.Key)
                        {
                            case "Avartar":
                                if (!string.IsNullOrEmpty(obj.Avartar) && obj.Avartar != item.Value)
                                {
                                    var array = obj.Avartar.Split('/');
                                    string fileName = array[obj.Avartar.Split('/').Length - 1];
                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadFileHinhAnhUsers);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }

                                obj.Avartar = item.Value;

                                break;

                            case "Password":
                                if (!string.IsNullOrEmpty(item.Value))
                                {
                                    Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                }
                                break;

                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);

                                break;
                        }

                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "USER_INFO_ID: " + obj.USER_INFO_ID.ToString() + ", field: " + item.Key
                        });
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            UserInfo UserInfo = null;
            string resultMessage = string.Empty;
            int idValue = -1;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryService = new AppHistoryService();
            InternalService internalService = new InternalService();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out idValue))
                    {
                        updatedData = new Dictionary<int, Dictionary<string, string>>();
                        updatedData.Add(idValue, updatedValues);
                        resultMessage = UpdateByIds(updatedData);
                        if (string.IsNullOrEmpty(resultMessage))
                        {
                            resultMessage = id;
                        }
                    }
                    else
                    {
                        resultMessage = "Giá trị USER_INFO_ID=" + id + " không phù hợp.";
                    }
                }
                else
                {

                    if (updatedValues.ContainsKey("MA_BN"))
                    {
                        if (IsExisting(updatedValues["MA_BN"]))
                        {
                            resultMessage += "Mã bệnh nhân này đã tồn tại.";
                        }
                        else
                        {
                            int userIDValue, bloodTypeIDValue, ma_Cong_TyValue;
                            DateTime dateOfBirthValue;
                            bool sexValue, isDefaultValue;
                            decimal heightValue, weight;

                            UserInfo = new UserInfo()
                            {
                                UserID = updatedValues.ContainsKey("UserID") ? (Int32.TryParse(updatedValues["UserID"], out userIDValue) ? userIDValue : 0) : 0,
                                MA_BN = updatedValues.ContainsKey("MA_BN") ? updatedValues["MA_BN"] : string.Empty,
                                FirstName = updatedValues.ContainsKey("FirstName") ? updatedValues["FirstName"] : string.Empty,
                                LastName = updatedValues.ContainsKey("LastName") ? updatedValues["LastName"] : string.Empty,
                                DateOfBirth = updatedValues.ContainsKey("DateOfBirth") ? (System.DateTime.TryParseExact(updatedValues["DateOfBirth"], Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out dateOfBirthValue) ? dateOfBirthValue : DateTime.Now) : DateTime.Now,
                                CMND = updatedValues.ContainsKey("CMND") ? updatedValues["CMND"] : string.Empty,
                                BHYT = updatedValues.ContainsKey("BHYT") ? updatedValues["BHYT"] : string.Empty,
                                Occupation = updatedValues.ContainsKey("Occupation") ? updatedValues["Occupation"] : string.Empty,
                                Avartar = updatedValues.ContainsKey("Avartar") ? updatedValues["Avartar"] : string.Empty,
                                Sex = updatedValues.ContainsKey("Sex") ? (bool.TryParse(updatedValues["Sex"], out sexValue) ? sexValue : false) : false,
                                Height = updatedValues.ContainsKey("Height") ? (decimal.TryParse(updatedValues["Height"], out heightValue) ? heightValue : 0) : 0,
                                weight = updatedValues.ContainsKey("weight") ? (decimal.TryParse(updatedValues["weight"], out weight) ? weight : 0) : 0,
                                Address = updatedValues.ContainsKey("Address") ? updatedValues["Address"] : string.Empty,
                                BloodTypeID = updatedValues.ContainsKey("BloodTypeID") ? (Int32.TryParse(updatedValues["BloodTypeID"], out bloodTypeIDValue) ? bloodTypeIDValue : 0) : 0,
                                ProvinceID = updatedValues.ContainsKey("ProvinceID") ? updatedValues["ProvinceID"] : string.Empty,
                                DistrictID = updatedValues.ContainsKey("DistrictID") ? updatedValues["DistrictID"] : string.Empty,
                                IsDefault = updatedValues.ContainsKey("IsDefault") ? (bool.TryParse(updatedValues["IsDefault"], out isDefaultValue) ? isDefaultValue : false) : false,
                                Ma_Cong_Ty = updatedValues.ContainsKey("Ma_Cong_Ty") ? (Int32.TryParse(updatedValues["Ma_Cong_Ty"], out ma_Cong_TyValue) ? ma_Cong_TyValue : 0) : 0,
                            };

                            UserInfo = unitOfWork.UserInfoRepository.Insert(UserInfo);

                            appHistoryService.Add(new AppHistory()
                            {
                                Ma = maTable,
                                Ten = tenTable,
                                ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu
                            });
                            unitOfWork.Save();

                            resultMessage = UserInfo.USER_INFO_ID.ToString();

                        }
                    }
                    else
                    {
                        resultMessage = "Trường email/sđt không được phép trống";
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }

        public string GetLastestMaBN(string tienTo)
        {
            if (string.IsNullOrEmpty(tienTo))
            {
                tienTo = CommonConstants.TienToMaBenhNhanMacDinh;
            }

            var maBNs = unitOfWork.UserInfoRepository.Get(m => m.MA_BN.StartsWith(tienTo)).Select(m => m.MA_BN);
            string result = string.Empty;

            int number = 0;
            
            if (maBNs.Count() > 0)
            {
                int chieuDaiChuoi = maBNs.Select(m => m.Length).Max();
                string maBN = maBNs.Where(m => m.Length == chieuDaiChuoi).Max();

                result = tienTo;

                if (!Int32.TryParse(maBN, out number))
                {
                    number = 0;
                }

                number += 1;

                for (int i = 0; i < chieuDaiChuoi - number.ToString().Length; i++)
                {
                    result += '0';
                }

                result += number.ToString();
            }

            return result;
        }
    }
}
