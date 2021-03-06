using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Globalization;

namespace JetMedicalWebApp.Services
{
    public class RegisterServiceService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "RegisterService";
        private string tenTable = "RegisterService";

        public List<RegisterServiceDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<RegisterServiceDto> result = new List<RegisterServiceDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.RegisterServiceRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            RegisterServiceDto obj = new RegisterServiceDto();
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
                RegisterServiceDto obj = new RegisterServiceDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":

                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);

                            break;

                        case "RegisterDate":
                            obj.GetType().GetProperty("NgayKham").SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            obj.GetType().GetProperty("ThoiGianKham").SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("HH:mm "), null);
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
        public IEnumerable<RegisterServiceDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public RegisterServiceDto GetByIdExposeDto(string id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "RegisterID=" + id ;
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.RegisterServiceRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public List<RegisterServiceDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<RegisterServiceDto> result = new List<RegisterServiceDto>();
            string tableName = "uv_RegisterService";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData.ContainsKey(CommonConstants.StrSelectedFields) && inData.ContainsKey(CommonConstants.StrSelectedFields) ? inData[CommonConstants.StrSelectedFields] : "*";
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = inData.ContainsKey(CommonConstants.StrSortedColumnNames) && !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "RegisterID ASC";

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
                            RegisterServiceDto obj = new RegisterServiceDto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<RegisterServiceDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                RegisterServiceDto obj = new RegisterServiceDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            break;

                        case "RegisterDate":
                            obj.GetType().GetProperty("StrRegisterDate").SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy HH:mm "), null);
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
        public string UpdateByIds(Dictionary<int, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                DateTime registerDateValue;
                var query = unitOfWork.RegisterServiceRepository.Get(m => ids.Contains(m.RegisterID)).ToList();

                foreach (RegisterService obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.RegisterID])
                    {

                   
                        switch (item.Key)
                        {
                            case "UserID":
                            case "CreatedUserID":
                                break;

                            case "RegisterDate":

                                if (DateTime.TryParseExact(item.Value, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out registerDateValue))
                                {
                                    obj.GetType().GetProperty(item.Key).SetValue(obj, registerDateValue, null);
                                }
                                break;

                            case "MA_LK":
                                if (!string.IsNullOrEmpty(obj.MA_LK) && obj.MA_LK.Trim() != item.Value.Trim())
                                {
                                    int registerID = obj.RegisterID;
                                    if (unitOfWork.RegisterServiceRepository.CountDataRow(m => !string.IsNullOrEmpty(m.MA_LK) && m.MA_LK.Trim().ToString().Equals(item.Value.Trim().ToString()) && m.RegisterID != registerID) == 0)
                                    {
                                        Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                    }
                                    else
                                    {
                                        if(!string.IsNullOrEmpty(messageResult))
                                        {
                                            messageResult += "\n";
                                        }
                                        messageResult += "Mã lượt khám '" + item.Value + "' đã có trong dữ liệu";
                                    }
                                }
                                else
                                {
                                    Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                }
                                break;

                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }

                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "Id: " + obj.RegisterID.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedDate = DateTime.Now;
                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                if (string.IsNullOrEmpty(messageResult))
                {
                    unitOfWork.Save();
                }
            }

            return messageResult;
        }


        public string DeleteById(int id)
        {
            string messageResult = String.Empty;

            AppHistoryService appHistoryServiceService = new AppHistoryService();

            try
            {
                var query = unitOfWork.RegisterServiceRepository.GetByID(id);
                if (query != null)
                {
                    appHistoryServiceService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                        GhiChu = "ID: " +id
                    });

                    unitOfWork.RegisterServiceRepository.Delete(query);


                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                messageResult = ex.Message;
            }

            return messageResult;
        }


        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues, out bool res)
        {
            RegisterService registerService = null;
            string resultMessage = string.Empty;
            int idValue = -1, staffIdValue = -1, memberIdValue = -1;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            res = false;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out idValue))
                    {
                        updatedData = new Dictionary<int, Dictionary<string, string>>();
                        updatedData.Add(idValue, updatedValues);
                        resultMessage = UpdateByIds(updatedData);
                    }
                    else
                    {
                        resultMessage = "Giá trị Id=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                    DateTime registerDateValue, dobValue;
                    var registerUser = updatedValues.ContainsKey("CreatedUserID") ? (Int32.TryParse(updatedValues["CreatedUserID"], out memberIdValue) ? unitOfWork.UsersRepository.GetByID(memberIdValue) : nguoiSuDungCapNhat) : nguoiSuDungCapNhat;
                    var uf = unitOfWork.UserInfoRepository.Get(x=>x.UserID == registerUser.UserID).FirstOrDefault();
                    if(uf == null)
                    {
                        UserInfo userInfo = new UserInfo()
                        {
                            UserID = registerUser.UserID,
                            MA_BN = "BN_" + registerUser.UserID,
                            FirstName = registerUser.FirstName,
                            LastName = registerUser.LastName,
                            DateOfBirth = updatedValues.ContainsKey("DOB") ? (DateTime.TryParseExact(updatedValues["DOB"], CommonConstants.DateFormat, null, DateTimeStyles.None, out dobValue) ? dobValue :DateTime.Now) : DateTime.Now,
                            CMND = updatedValues.ContainsKey("CMND") ? updatedValues["CMND"] : string.Empty,
                            BHYT = updatedValues.ContainsKey("BHYT") ? updatedValues["BHYT"] : string.Empty,
                            Occupation = updatedValues.ContainsKey("Occupation") ? updatedValues["Occupation"] : string.Empty,
                            Avartar = updatedValues.ContainsKey("Avartar") ? updatedValues["Avartar"] : string.Empty,
                            Sex =  false,
                            Height =  0,
                            weight =  0,
                            Address =  string.Empty,
                            BloodTypeID =0,
                            ProvinceID = string.Empty,
                            DistrictID =  string.Empty,
                            IsDefault = true,
                            Ma_Cong_Ty =  0,
                        };
                        unitOfWork.UserInfoRepository.Insert(userInfo);
                        unitOfWork.Save();
                    }

                    int userIDValue, departmentIdValue, packageIdValue, statusValue;
                    registerDateValue = updatedValues.ContainsKey("RegisterDate") ? (DateTime.TryParseExact(updatedValues["RegisterDate"], CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out registerDateValue) ? registerDateValue : DateTime.Now) : DateTime.Now;
                    int lk = GetRegisterNoLastest(registerDateValue);
                    if (lk == 0)
                    {
                        resultMessage = "Số lượt khám khung giờ này đã đủ vui lòng chọn khung giờ khác";
                    }
                    else
                    {
                        registerService = new RegisterService()
                        {
                            RegisterNo = lk,
                            Emaill = updatedValues.ContainsKey("Emaill") ? updatedValues["Emaill"] : string.Empty,
                            PhoneNumber = updatedValues.ContainsKey("PhoneNumber") ? updatedValues["PhoneNumber"] : string.Empty,
                            FullName = updatedValues.ContainsKey("FullName") ? updatedValues["FullName"] : string.Empty,
                            DOB = updatedValues.ContainsKey("DOB") ? (DateTime.TryParseExact(updatedValues["DOB"], CommonConstants.DateFormat, null, DateTimeStyles.None, out dobValue) ? dobValue : new Nullable<DateTime>()) : new Nullable<DateTime>(),
                            UserID = updatedValues.ContainsKey("UserID") ? (Int32.TryParse(updatedValues["UserID"], out userIDValue) ? userIDValue : -1) : -1,
                            RegisterDate = registerDateValue,
                            DepartmentId = updatedValues.ContainsKey("DepartmentId") ? (Int32.TryParse(updatedValues["DepartmentId"], out departmentIdValue) ? departmentIdValue : new Nullable<Int32>()) : new Nullable<Int32>(),
                            RegisterNote = updatedValues.ContainsKey("RegisterNote") ? updatedValues["RegisterNote"] : string.Empty,
                            PackageId = updatedValues.ContainsKey("PackageId") ? (Int32.TryParse(updatedValues["PackageId"], out packageIdValue) ? packageIdValue : new Nullable<Int32>()) : new Nullable<Int32>(),
                            StaffId = updatedValues.ContainsKey("StaffId") ? (Int32.TryParse(updatedValues["StaffId"], out staffIdValue) ? staffIdValue : -1) : -1,
                            Status = updatedValues.ContainsKey("Status") ? (Int32.TryParse(updatedValues["Status"], out statusValue) ? statusValue : CommonConstants.RegisterService_DangKy) : CommonConstants.RegisterService_DangKy,
                            MA_LK = updatedValues.ContainsKey("MA_LK") ? updatedValues["MA_LK"] : string.Empty,
                            ModifiedDate = DateTime.Now,
                            ModifiedUsers = updatedValues.ContainsKey("CreatedUserID") ? (Int32.TryParse(updatedValues["CreatedUserID"], out memberIdValue) ? unitOfWork.UsersRepository.GetByID(memberIdValue) : nguoiSuDungCapNhat) : nguoiSuDungCapNhat,
                            CreateDate = DateTime.Now,
                            CreatedUsers = updatedValues.ContainsKey("CreatedUserID") ? (Int32.TryParse(updatedValues["CreatedUserID"], out memberIdValue) ? unitOfWork.UsersRepository.GetByID(memberIdValue) : nguoiSuDungCapNhat) : nguoiSuDungCapNhat,
                        };
                        registerService = unitOfWork.RegisterServiceRepository.Insert(registerService);
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                        });
                        unitOfWork.Save();

                        XML1 xml1 = new XML1()
                        {
                            MA_LK = registerService.RegisterDate.ToString("dd/MM/yyyy_HH:mm_") + lk.ToString(),
                            STT = lk,
                            MA_BN = uf.MA_BN,
                            BENH_ID =  -1,
                            TEN_BENH =  string.Empty,
                            MA_BENH = string.Empty,
                            MA_BENHKHAC = string.Empty,
                            NGAY_VAO = registerDateValue,
                            NGAY_RA =  new Nullable<DateTime>(),
                            NGAY_TAI_KHAM =new Nullable<DateTime>(),
                            KET_QUA_DTRI =  0,
                            TINH_TRANG_RV =  0,
                            DepartmentId = registerService.DepartmentId,
                            MA_KHOA =  string.Empty,
                            CHUAN_DOAN = string.Empty,
                            PPDIEUTRI = string.Empty,
                            LOIDANTHAYTHUOC = string.Empty,
                            GHICHU =  string.Empty,
                            XML1_File =  string.Empty,
                            ModifiedDate = DateTime.Now,
                            ModifiedUsers = nguoiSuDungCapNhat,
                            CreatedDate = DateTime.Now,
                            CreatedUsers = nguoiSuDungCapNhat
                        };
                        unitOfWork.XML1Repository.Insert(xml1);
                        unitOfWork.Save();

                        resultMessage = registerService.RegisterDate.ToString("dd/MM/yyyy HH:mm") + "," + registerService.RegisterNo;

                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }


        public int GetRegisterNoLastest(DateTime registerDate)
        {
            int total = unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == registerDate.Hour);
            return total == 20 ? 0 : total + 1;
        }

        public Dictionary<string, int> GetRegisterNoLastestByTime(DateTime registerDate)
        {
            var updatedValues = new Dictionary<string, int>();
          
            updatedValues.Add("07:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 7));
            updatedValues.Add("09:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 9));
            updatedValues.Add("11:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 11));
            updatedValues.Add("13:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 13));
            updatedValues.Add("15:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 15));
            updatedValues.Add("17:00", unitOfWork.RegisterServiceRepository.CountDataRow(x => x.RegisterDate.Day == registerDate.Day
                                                            && x.RegisterDate.Month == registerDate.Month
                                                            && x.RegisterDate.Year == registerDate.Year
                                                            && x.RegisterDate.Hour == 17));

            return updatedValues;
        }



        public RegisterService GetRegisterServiceApi(DateTime registerDate)
        {
            var reg = unitOfWork.RegisterServiceRepository.Get(x => x.Status == 1 
            && x.RegisterDate.Year == registerDate.Year
            && x.RegisterDate.Month == registerDate.Month 
            && x.RegisterDate.Day == registerDate.Day).OrderBy(m => new { m.RegisterDate, m.RegisterNo }).FirstOrDefault();
            if(reg != null)
            {
                reg.Status = 0;
                unitOfWork.Save();
            }

            return reg;
        }
    }
}
