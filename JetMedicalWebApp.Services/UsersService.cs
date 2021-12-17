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
using System.Data;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Services
{
    public class UsersService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private string maTable = "Users";
        private string tenTable = "Users";

        public List<UsersDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<UsersDto> result = new List<UsersDto>();
            string stringFilter = string.Empty, usernameNguoiCapNhat = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.UsersRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            UsersDto obj = new UsersDto();
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
                UsersDto obj = new UsersDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":

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
        public IEnumerable<UsersDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public UsersDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "UserID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        public UsersDto GetByUsernameExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Username=\"" + username + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public UsersDto GetByEmailOrPhoneExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "EmailID=(\"" + username + "\")  OR Phone =(\"" + username + "\")";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public UsersDto GetByUserIdExposeDto(string userId, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "UserId=\"" + userId + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public bool IsExisting(string emailId, string phone)
        {
            return unitOfWork.UsersRepository.CountDataRow(m => m.EmailID.Trim().ToLower().Equals(emailId.Trim().ToLower())
                                                            && m.Phone.Trim().ToLower().Equals(phone.Trim().ToLower())) > 0;
        }

        public bool IsExistingEmail(string emailId)
        {
            return unitOfWork.UsersRepository.CountDataRow(m => m.EmailID.Trim().ToLower().Equals(emailId.Trim().ToLower())) > 0;
        }

        public bool IsExistingPhone(string phone)
        {
            return unitOfWork.UsersRepository.CountDataRow(m => m.Phone.Trim().ToLower().Equals(phone.Trim().ToLower())) > 0;
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            if (!string.IsNullOrEmpty(stringFilter))
            {
                return unitOfWork.UsersRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
            }
            return unitOfWork.UsersRepository.Get().Select("new (" + selectedFields + ")").Count();
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
                    var query = unitOfWork.UsersRepository.Get(m => ids.Contains(m.UserID)).ToList();
                    if (query != null)
                    {
                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "UserID: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            int usersId = item.UserID;

                            var users_GroupPermissions = unitOfWork.Users_GroupPermissionRepository.Get(m => m.Users.UserID.Equals(usersId)).ToList();

                            if (users_GroupPermissions != null)
                            {
                                if (users_GroupPermissions.Count() > 0)
                                {
                                    unitOfWork.Users_GroupPermissionRepository.DeleteRange(users_GroupPermissions);
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Avartar))
                            {
                                imageNames.Add(item.Avartar);
                            }
                        }

                        unitOfWork.UsersRepository.DeleteRange(query);
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
                var nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                AppHistoryService appHistoryService = new AppHistoryService();
                var query = unitOfWork.UsersRepository.Get(m => ids.Contains(m.UserID)).ToList();
                foreach (Users obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.UserID])
                    {
                        switch (item.Key)
                        {
                            case "Avartar":
                                if (!string.IsNullOrEmpty(obj.Avartar) && obj.Avartar != item.Value)
                                {
                                    var array = obj.Avartar.Split('/');
                                    string fileName = array[obj.Avartar.Split('/').Length - 1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadFileHinhAnhUsers);
                                    path = Path.Combine(path, obj.Avartar);
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

                            case "EmailID":
                                if (!string.IsNullOrEmpty(item.Value))
                                {
                                    if (!IsExistingEmail(item.Value))
                                    {
                                        Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                    }
                                }
                                break;

                            case "Phone":
                                if (!string.IsNullOrEmpty(item.Value))
                                {
                                    if (!IsExistingPhone(item.Value))
                                    {
                                        Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                    }
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
                            GhiChu = "UserID: " + obj.UserID.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            Users Users = null;
            string resultMessage = string.Empty;
            int idValue = -1;
            bool check = false;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat = null;
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
                        resultMessage = "Giá trị UserID=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    if (updatedValues.ContainsKey("EmailID"))
                    {
                        if (updatedValues.ContainsKey("IsAdmin"))
                        {
                            check = true;
                        }
                        else
                        {
                            if (IsExisting(updatedValues["EmailID"], updatedValues["Phone"]))
                            {
                                resultMessage += "Email hoặc số điện thoại này đã tồn tại.";
                            }
                            else
                            {
                                nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                                appHistoryService.Add(new AppHistory()
                                {
                                    Ma = maTable,
                                    Ten = tenTable,
                                    ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu
                                });
                                check = true;
                            }
                        }

                        if (check)
                        {
                            bool isVerifiedValue, activeValue, isMobileValue, isAdminValue;
                            int groupIDValue, loginFailedCountValue;
                            DateTime lastLoginDateValue, lastAccessedDateValue, createDateValue;

                            Users = new Users()
                            {
                                EmailID = updatedValues.ContainsKey("EmailID") ? updatedValues["EmailID"] : string.Empty,
                                Phone = updatedValues.ContainsKey("Phone") ? updatedValues["Phone"] : string.Empty,
                                FirstName = updatedValues.ContainsKey("FirstName") ? updatedValues["FirstName"] : string.Empty,
                                LastName = updatedValues.ContainsKey("LastName") ? updatedValues["LastName"] : string.Empty,
                                Password = updatedValues.ContainsKey("Password") ? updatedValues["Password"] : string.Empty,
                                ResetPasswordCode = updatedValues.ContainsKey("ResetPasswordCode") ? updatedValues["ResetPasswordCode"] : string.Empty,
                                IsVerified = updatedValues.ContainsKey("IsVerified") ? (bool.TryParse(updatedValues["IsVerified"], out isVerifiedValue) ? isVerifiedValue : false) : true,
                                ActivationCode = updatedValues.ContainsKey("ActivationCode") ? updatedValues["ActivationCode"] : string.Empty,
                                Avartar = updatedValues.ContainsKey("Avartar") ? updatedValues["Avartar"] : string.Empty,
                                GroupID = updatedValues.ContainsKey("GroupID") ? (Int32.TryParse(updatedValues["GroupID"], out groupIDValue) ? groupIDValue : 0) : 0,
                                Active = updatedValues.ContainsKey("Active") ? (bool.TryParse(updatedValues["Active"], out activeValue) ? activeValue : false) : false,
                                IsMobile = updatedValues.ContainsKey("IsMobile") ? (bool.TryParse(updatedValues["IsMobile"], out isMobileValue) ? isMobileValue : false) : false,
                                IsAdmin = updatedValues.ContainsKey("IsAdmin") ? (bool.TryParse(updatedValues["IsAdmin"], out isAdminValue) ? isAdminValue : false) : false,
                                LoginFailedCount = updatedValues.ContainsKey("LoginFailedCount") ? (Int32.TryParse(updatedValues["LoginFailedCount"], out loginFailedCountValue) ? loginFailedCountValue : 0) : 0,
                                LastLoginDate = updatedValues.ContainsKey("LastLoginDate") ? (System.DateTime.TryParseExact(updatedValues["LastLoginDate"], Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out lastLoginDateValue) ? lastLoginDateValue : new Nullable<DateTime>()) : new Nullable<DateTime>(),
                                LastAccessedDate = updatedValues.ContainsKey("LastAccessedDate") ? (System.DateTime.TryParseExact(updatedValues["LastAccessedDate"], Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out lastAccessedDateValue) ? lastAccessedDateValue : new Nullable<DateTime>()) : new Nullable<DateTime>(),
                                CreateDate = updatedValues.ContainsKey("CreateDate") ? (System.DateTime.TryParseExact(updatedValues["CreateDate"], Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out createDateValue) ? createDateValue : DateTime.Now) : DateTime.Now,
                                CreatedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1,
                                ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1
                            };

                            Users = unitOfWork.UsersRepository.Insert(Users);

                            unitOfWork.Save();

                            resultMessage = Users.UserID.ToString();

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


        public List<KSK_TTBENHNHANDto> GetListTTBNDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                out Dictionary<string, string> outData)
        {
            List<KSK_TTBENHNHANDto> result = new List<KSK_TTBENHNHANDto>();
            string tableName = "uv_KhamSucKhoe";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "KSK_ID ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            KSK_TTBENHNHANDto obj = new KSK_TTBENHNHANDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<KSK_TTBENHNHANDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                KSK_TTBENHNHANDto obj = new KSK_TTBENHNHANDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }


        public List<UsersDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                   out Dictionary<string, string> outData)
        {
            List<UsersDto> result = new List<UsersDto>();
            string tableName = "uv_NguoiDung";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            UsersDto obj = new UsersDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<UsersDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                UsersDto obj = new UsersDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "CreatedDate":
                        case "ModifiedDate":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
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

        public List<UsersDto> GetListDataHeThongFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                   out Dictionary<string, string> outData)
        {
            List<UsersDto> result = new List<UsersDto>();
            string tableName = "uv_NguoiDungHeThong";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            UsersDto obj = new UsersDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<UsersDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                UsersDto obj = new UsersDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "CreatedDate":
                        case "ModifiedDate":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
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

        public string NhapDuLieuTuFileExcel(DataTable dt)
        {
            string resultMessage = string.Empty;
            int rowIndex = 0;
            int tongSoDong = dt.Rows.Count;

            List<Task> tasks = null;
            if (dt.Columns.Count > 15)
            {
                int userID = -1;
                var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
                if (authCookie != null)
                {
                    userID = Int32.Parse(authCookie.Value);
                }

                if (dt.Rows.Count > 100)
                {
                    tasks = new List<Task>
                    {
                        new Task(async () =>  await Import1(rowIndex, (tongSoDong / 3), dt, userID)),
                        new Task(async () =>  await Import2(((tongSoDong / 3) + 1), ((tongSoDong / 3) * 2), dt, userID)),
                        new Task(async () =>  await Import3((((tongSoDong / 3) * 2) + 1), tongSoDong, dt, userID)),
                    };
                }
                else
                {
                    tasks = new List<Task>
                    {
                        new Task(async () =>  await Import1(rowIndex, tongSoDong, dt, userID)),
                    };
                }

                Parallel.ForEach(tasks, task =>
                {
                    task.Start();
                });

                Task.WhenAll(tasks).ContinueWith(done =>
                {
                    //Run the other tasks
                });
            }
            else
            {
                resultMessage = "File excel không đúng định dạng";
            }

            return resultMessage;
        }
        public async Task Import1(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public async Task Import2(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public async Task Import3(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public void ThucHienImport(int tuDong, int toiDong, DataTable dt, int userID)
        {
            UnitOfWork unit3 = new UnitOfWork();

            Users Users = null;
            UserInfo UserInfo = null;
            DateTime dateOfBirthValue;
            bool sexValue, kichHoatValue;
            int bloodTypeIDValue, maCongTyValue;

            Users nguoiSuDungCapNhat = unit3.UsersRepository.Get(m => m.UserID.Equals(userID)).FirstOrDefault();

            for (int n = tuDong; n < toiDong; n++)
            {
                Users = null;
                UserInfo = null;

                string maBN = dt.Rows[n][0].ToString();
                string firstName = dt.Rows[n][1].ToString();
                string lastName = dt.Rows[n][2].ToString();
                string cmnd = dt.Rows[n][3].ToString();
                string bhyt = dt.Rows[n][4].ToString();
                string password = dt.Rows[n][5].ToString();
                string emailID = dt.Rows[n][6].ToString();
                string phone = dt.Rows[n][7].ToString();
                string dateOfBirth = dt.Rows[n][8].ToString();
                string sex = dt.Rows[n][9].ToString();
                string address = dt.Rows[n][10].ToString();
                string bloodTypeID = dt.Rows[n][11].ToString();
                string provinceID = dt.Rows[n][12].ToString();
                string districtID = dt.Rows[n][13].ToString();
                string occupation = dt.Rows[n][14].ToString();
                string maCongTy = dt.Rows[n][15].ToString();
                string kichHoat = dt.Rows[n][16].ToString();

                if (unit3.UserInfoRepository.Get().FirstOrDefault(m => m.MA_BN.Trim().ToLower().Equals(maBN.Trim().ToLower())) == null)
                {
                    Users = new Users()
                    {
                        EmailID = emailID,
                        Phone = phone,
                        FirstName = firstName,
                        LastName = lastName,
                        Password = !string.IsNullOrEmpty(password) ? Encryptor.Hash(password) : string.Empty,
                        IsVerified = true,
                        GroupID = 0,
                        Active = bool.TryParse(kichHoat, out kichHoatValue) ? kichHoatValue : false,
                        IsMobile = true,
                        IsAdmin = false,
                        LoginFailedCount = 0,
                        LastLoginDate = new Nullable<DateTime>(),
                        LastAccessedDate = new Nullable<DateTime>(),
                        CreateDate = DateTime.Now,
                        ActivationCode = string.Empty,
                        ResetPasswordCode = string.Empty,
                        Avartar = string.Empty,
                        CreatedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1,
                        ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1
                    };

                    Users = unit3.UsersRepository.Insert(Users);

                    unit3.Save();

                    if (Users.UserID > 0)
                    {
                        Company company = !string.IsNullOrEmpty(maCongTy) ? unit3.CompanyRepository.Get().FirstOrDefault(m => m.ComCode.Trim().ToLower().Equals(maCongTy.Trim().ToLower())) : null;

                        UserInfo = new UserInfo()
                        {
                            UserID = Users.UserID,
                            MA_BN = maBN,
                            FirstName = firstName,
                            LastName = lastName,
                            DateOfBirth = DateTime.TryParseExact(dateOfBirth, Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out dateOfBirthValue) ? dateOfBirthValue : DateTime.Now,
                            CMND = cmnd,
                            BHYT = bhyt,
                            Occupation = occupation,
                            Avartar = string.Empty,
                            Sex = bool.TryParse(sex, out sexValue) ? sexValue : false,
                            Height = 0,
                            weight = 0,
                            Address = address,
                            BloodTypeID = Int32.TryParse(bloodTypeID, out bloodTypeIDValue) ? bloodTypeIDValue : 0,
                            ProvinceID = provinceID,
                            DistrictID = districtID,
                            IsDefault = true,
                            Ma_Cong_Ty = company != null ? company.CompanyID : (Int32.TryParse(maCongTy, out maCongTyValue) ? maCongTyValue : 0),
                        };

                        UserInfo = unit3.UserInfoRepository.Insert(UserInfo);
                        unit3.Save();
                    }
                }
                else
                {
                    unit3.AppHistoryRepository.Insert(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = CommonConstants.ThaoTacImport,
                        GhiChu = "Mã bệnh nhân: '" + maBN + "' đã có trong dữ liệu",
                        CreatedUsers = nguoiSuDungCapNhat,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    });
                    unit3.Save();
                }
            }
        }


        public List<int> GetListCompanyManagement(int userId)
        {
            List<int> ids = new List<int>();

            var groupPermission = unitOfWork.Users_GroupPermissionRepository.Get(m => m.Users.UserID == userId).ToList();
            if(groupPermission != null)
            {
                if(groupPermission.Count() > 0)
                {
                    foreach(var item in groupPermission)
                    {
                        int id = item.GroupPermission.Id;

                        var company_groups = unitOfWork.Company_GroupPermissionRepository.Get(m => m.GroupPermission.Id == id).ToList();


                        if(company_groups != null)
                        {
                            if(company_groups.Count() > 0)
                            {
                                foreach (var u in company_groups)
                                {
                                    ids.Add(u.Company.CompanyID);
                                }
                            }
                        }
                    }
                }
            }

            return ids;
        }
    }
}
