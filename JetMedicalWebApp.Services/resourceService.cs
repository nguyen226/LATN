using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;

namespace JetMedicalWebApp.Services
{
    public class ResourceService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Resource";
        private string tenTable = "Resource";

        public List<ResourceDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<ResourceDto> result = new List<ResourceDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.ResourceRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            ResourceDto obj = new ResourceDto();
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
                ResourceDto obj = new ResourceDto();

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
        public IEnumerable<ResourceDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public ResourceDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.ResourceRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }



        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();

                try
                {
                    List<string> imageNames = new List<string>();
                    var query = unitOfWork.ResourceRepository.Get(m => ids.Contains(m.id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "id: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            unitOfWork.ResourceRepository.Delete(item);
                        }

                        unitOfWork.Save();
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

                var query = unitOfWork.ResourceRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (resource obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
                    {
                        switch (item.Key)
                        {
                            case "id":
                            case "categoriName":
                                break;
                            case "url":
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value == CommonConstants.FileNoImage ? string.Empty : item.Value);
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
                            GhiChu = "id: " + obj.id.ToString() + ", field: " + item.Key
                        });

                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }
        public string AddOrUpdate(List<ResourceDto> listItem)
        {
            resource resource = null;
            string resultMessage = string.Empty;
            int languageIdValue;
            bool isactiveValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            var code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            try
            {
                foreach (var item in listItem)
                {
                    if (!string.IsNullOrEmpty(item.name))
                    {
                        Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                        foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                        {
                            if (itemPropertyInfo.Name == "code")
                            {
                                updatedValues.Add(itemPropertyInfo.Name, code);
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
                            resource = new resource()
                            {
                                name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                code = updatedValues.ContainsKey("code") ? updatedValues["code"] : string.Empty,
                                isView = updatedValues.ContainsKey("isView") ? bool.TryParse(updatedValues["isView"], out isactiveValue) ? isactiveValue : false : false,
                                languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : 0 : 0,
                            };

                            resource = unitOfWork.ResourceRepository.Insert(resource);

                            appHistoryServiceService.Add(new AppHistory()
                            {
                                Ma = maTable,
                                Ten = tenTable,
                                ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                            });

                            unitOfWork.Save();
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



        public List<ResourceDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                   out Dictionary<string, string> outData)
        {
            List<ResourceDto> result = new List<ResourceDto>();
            string tableName = "uv_Resource";
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
                            ResourceDto obj = new ResourceDto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<ResourceDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                ResourceDto obj = new ResourceDto();

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


        public IDictionary<string, string> GetView()
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                int languageId = Utilities.GetLanguage();

                var data = unitOfWork.ResourceRepository.Get(x => x.isView == true && x.languageId == languageId).ToList();

                if (data != null && data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        result.Add(item.code, item.name);
                    }
                }
            }
            catch (Exception ex) { }
            return result;
        }
    }
}
