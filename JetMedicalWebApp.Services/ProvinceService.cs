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
    public class ProvinceService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Province";
        private string tenTable = "Province";

        public List<ProvinceDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<ProvinceDto> result = new List<ProvinceDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.ProvinceRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            ProvinceDto obj = new ProvinceDto();
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
                ProvinceDto obj = new ProvinceDto();

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
        public IEnumerable<ProvinceDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public ProvinceDto GetByIdExposeDto(string id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "ProvinceID=\"" + id + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        
        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.ProvinceRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public bool IsExitting(string name)
        {
            return unitOfWork.ProvinceRepository.Get(m => m.ProvinceName.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
        }

        public string DeleteByIds(List<string> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.ProvinceRepository.Get(m => ids.Contains(m.ProvinceID)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Id: " + string.Join(";", ids)
                        });
                      
                        foreach(var item in query)
                        {
                            unitOfWork.ProvinceRepository.Delete(item);
                            unitOfWork.Save();
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

        public string UpdateByIds(Dictionary<string, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<string> ids = new List<string>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.ProvinceRepository.Get(m => ids.Contains(m.ProvinceID)).ToList();

                foreach (Province obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.ProvinceID])
                    {
                        switch (item.Key)
                        {
                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }

                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "Id: " + obj.ProvinceID.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            Province Province = null;
            string resultMessage = string.Empty;
            Dictionary<string, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            int intValue = -1;
            bool boolValue = false;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    updatedData = new Dictionary<string, Dictionary<string, string>>();
                    updatedData.Add(id, updatedValues);
                    resultMessage = UpdateByIds(updatedData);
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                    if (IsExitting(updatedValues["ProvinceName"]))
                    {
                        resultMessage = "Đã tồn tại Tỉnh này này.";
                    }
                    else
                    {
                        Province = new Province()
                        {
                            ProvinceID = updatedValues["ProvinceID"],
                            ProvinceName = updatedValues["ProvinceName"],
                            IsActive = updatedValues.ContainsKey("IsActive") ? (bool.TryParse(updatedValues["IsActive"], out boolValue) ? boolValue : false) : false,
                            ODX = updatedValues.ContainsKey("ODX") ? (Int32.TryParse(updatedValues["ODX"], out intValue) ? intValue : 0) : 0,
                            ModifiedUsers = nguoiSuDungCapNhat,
                            CreatedUsers = nguoiSuDungCapNhat
                        };
                        Province = unitOfWork.ProvinceRepository.Insert(Province);

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
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
    }
}
