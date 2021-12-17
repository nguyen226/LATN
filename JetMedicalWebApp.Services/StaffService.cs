using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.IO;

namespace JetMedicalWebApp.Services
{
    public class StaffService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Staff";
        private string tenTable = "Staff";

        public List<StaffDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<StaffDto> result = new List<StaffDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.StaffRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            StaffDto obj = new StaffDto();
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
                StaffDto obj = new StaffDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "Staff_DonVis":
                            break;

                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            break;

                        case "departmentid":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null) : -1, null);
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
        public IEnumerable<StaffDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public StaffDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "id=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        
        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.StaffRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                List<string> imageNames = new List<string>();

                try
                {
                    var query = unitOfWork.StaffRepository.Get(m => ids.Contains(m.id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "StaffID: " + string.Join(";", ids)
                        });
                      
                        foreach(var item in query)
                        {
                            if (!string.IsNullOrEmpty(item.img))
                            {
                                imageNames.Add(item.img);
                            }
                            unitOfWork.StaffRepository.Delete(item);
                            unitOfWork.Save();
                        }


                        if (imageNames.Count() > 0)
                        {
                            string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanUploadFileStaff);
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
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.StaffRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (Staff obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
                    {
                        switch (item.Key)
                        {
                            case "img":
                                if (!string.IsNullOrEmpty(obj.img) && obj.img != item.Value && !obj.img.Contains("http"))
                                {
                                    var array = obj.img.Split('/');
                                    string fileName = array[obj.img.Split('/').Length - 1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadFileStaff);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }

                                obj.img = item.Value;

                                break;

                            case "departmentname":
                            case "newcategoryname":
                                break; 

                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }

                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "Id: " + obj.id.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public List<StaffDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                   out Dictionary<string, string> outData)
        {
            List<StaffDto> result = new List<StaffDto>();
            string tableName = "uv_Staff";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "id ASC";

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
                            StaffDto obj = new StaffDto();

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

            var sqlCommand = "SELECT * FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<StaffDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                StaffDto obj = new StaffDto();

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



        public string AddOrUpdate(List<StaffDto> listItem)
        {
            Staff staff = null;
            string resultMessage = string.Empty;
            int newscategoryidValue ,languageIdValue, departmentidValue, orderValue = 0;
            bool activeValue = false, isexaminationValue = false;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;

            try
            {
                string languageCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                foreach (var item in listItem)
                {
                    if (!string.IsNullOrEmpty(item.fullname))
                    {
                        Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                        foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                        {
                            if (itemPropertyInfo.Name == "code")
                            {
                                updatedValues.Add(itemPropertyInfo.Name, languageCode);
                            } else if (itemPropertyInfo.Name == "description")
                            {
                                string value = itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty;
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if(value.IndexOf("src=" + '"') > 0)
                                    {
                                        value = value.Replace("src=" + '"', "src=" + '"'  + internalService.GetUrlHost());
                                    }
                                }

                                updatedValues.Add(itemPropertyInfo.Name, value);
                            }
                            else if (itemPropertyInfo.Name == "img")
                            {
                                string value = itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty;
                                if (value.IndexOf(internalService.GetUrlHost()) < 0 && value != CommonConstants.FileNoImage)
                                {
                                    updatedValues.Add(itemPropertyInfo.Name, internalService.GetUrlHost() + value);
                                }
                            }
                            else
                            {
                                updatedValues.Add(itemPropertyInfo.Name, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty);
                            }
                        }

                        if (item.id > 0)
                        {
                            updatedData = new Dictionary<int, Dictionary<string, string>>();
                            updatedData.Add(item.id, updatedValues);
                            resultMessage = UpdateByIds(updatedData);
                        }
                        else
                        {
                            nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                            if (!IsExit(updatedValues["code"]))
                            {
                                staff = new Staff()
                                {
                                    img = updatedValues.ContainsKey("img") ? updatedValues["img"] : string.Empty,
                                    brandname = updatedValues.ContainsKey("brandname") ? updatedValues["brandname"] : string.Empty,
                                    fullname = updatedValues.ContainsKey("fullname") ? updatedValues["fullname"] : string.Empty,
                                    position = updatedValues.ContainsKey("position") ? updatedValues["position"] : string.Empty,
                                    summary = updatedValues.ContainsKey("summary") ? updatedValues["summary"] : string.Empty,
                                    description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                                    code = updatedValues.ContainsKey("code") ? updatedValues["code"] : string.Empty,
                                    isactive = updatedValues.ContainsKey("isactive") ? (bool.TryParse(updatedValues["isactive"], out activeValue) ? activeValue : false) : false,
                                    Isexamination = updatedValues.ContainsKey("Isexamination") ? (bool.TryParse(updatedValues["Isexamination"], out isexaminationValue) ? isexaminationValue : false) : false,
                                    newscategoryid = updatedValues.ContainsKey("newscategoryid") ? (Int32.TryParse(updatedValues["newscategoryid"], out newscategoryidValue) ? newscategoryidValue : -1) : -1,
                                    languageId = updatedValues.ContainsKey("languageId") ? (Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : -1) : -1,
                                    departmentid = updatedValues.ContainsKey("departmentid") ? (Int32.TryParse(updatedValues["departmentid"], out departmentidValue) ? departmentidValue : new Nullable<Int32>()) : new Nullable<Int32>(),
                                    order = updatedValues.ContainsKey("order") ? (Int32.TryParse(updatedValues["order"], out orderValue) ? orderValue : 0) : 0,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                    CreatedDate = DateTime.Now,
                                    CreatedUserID = nguoiSuDungCapNhat.UserID
                                };

                                staff = unitOfWork.StaffRepository.Insert(staff);

                                appHistoryServiceService.Add(new AppHistory()
                                {
                                    Ma = maTable,
                                    Ten = tenTable,
                                    ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                                });

                                unitOfWork.Save();
                            }
                            else
                            {
                                resultMessage = "Mã đã tồn tại";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
        public bool IsExit(string code)
        {
            return unitOfWork.StaffRepository.Get(m => m.code.Trim().ToLower().Equals(code.Trim().ToLower())).FirstOrDefault() != null;
        }
    }
}
