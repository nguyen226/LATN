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
    public class ICD10Service
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "ICD10";
        private string tenTable = "ICD10";

        public List<ICD10Dto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<ICD10Dto> result = new List<ICD10Dto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.ICD10Repository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            ICD10Dto obj = new ICD10Dto();
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
                ICD10Dto obj = new ICD10Dto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {

                        case "ICD10_GroupPermissions":
                        case "Users_GroupPermissions":
                            break;

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
        public IEnumerable<ICD10Dto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public ICD10Dto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Id=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.ICD10Repository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public bool IsExitting(string code)
        {
            return unitOfWork.ICD10Repository.Get(m => m.Ma.Trim().ToLower().Equals(code.Trim().ToLower())).FirstOrDefault() != null;
        }

        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.ICD10Repository.Get(m => ids.Contains(m.Id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Ids : " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            unitOfWork.ICD10Repository.Delete(item);
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

        public string UpdateByIds(Dictionary<Int32, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<Int32> ids = new List<Int32>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.ICD10Repository.Get(m => ids.Contains(m.Id)).ToList();
                bool checkUpdate = true;

                foreach (ICD10 obj in query)
                {
                    var tempUpdateData = updatedData[obj.Id];
                    if (tempUpdateData.ContainsKey("Ma"))
                    {
                        string ma = tempUpdateData["Ma"];
                        if (unitOfWork.ICD10Repository.CountDataRow(m=>m.Ma.Trim().ToLower() == ma.Trim().ToLower() && m.Id != obj.Id) > 0)
                        {
                            checkUpdate = false;
                            if (!string.IsNullOrEmpty(messageResult))
                            {
                                messageResult += "\n";
                            }
                            messageResult += "Mã '" + ma + "' đã có trên hệ thống.";
                        }
                    }
                }

                if (checkUpdate)
                {
                    foreach (ICD10 obj in query)
                    {
                        var tempUpdateData = updatedData[obj.Id];
                        foreach (KeyValuePair<string, string> item in tempUpdateData)
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
                                GhiChu = "Ma: " + obj.Ma + ", field: " + item.Key
                            });

                            obj.ModifiedDate = System.DateTime.Now;
                            obj.ModifiedUsers = nguoiSuDungCapNhat;
                        }
                    }

                    unitOfWork.Save();
                }
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            ICD10 iCD10 = null;
            string resultMessage = string.Empty;
            Dictionary<Int32, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            bool isActiveValue, manTinhValue, thuongGapValue, khongBHValue, ngoaiDSValue;
            int idValue;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out idValue))
                    {
                        updatedData = new Dictionary<Int32, Dictionary<string, string>>();
                        updatedData.Add(idValue, updatedValues);
                        resultMessage = UpdateByIds(updatedData);
                    }
                    else
                    {
                        resultMessage = "Không tìm thấy id";
                    }
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                    if (IsExitting(updatedValues["Ma"]))
                    {
                        resultMessage = "Đã tồn tại mã bệnh này.";
                    }
                    else
                    {
                        iCD10 = new ICD10()
                        {
                            Ma = updatedValues["Ma"],
                            Ten = updatedValues.ContainsKey("Ten") ? updatedValues["Ten"] : string.Empty,
                            NhomBenh = updatedValues.ContainsKey("NhomBenh") ? updatedValues["NhomBenh"] : string.Empty,
                            MoTa = updatedValues.ContainsKey("MoTa") ? updatedValues["MoTa"] : string.Empty,
                            ManTinh = updatedValues.ContainsKey("ManTinh") ? bool.TryParse(updatedValues["ManTinh"], out manTinhValue) ? manTinhValue : false : false,
                            ThuongGap = updatedValues.ContainsKey("ThuongGap") ? bool.TryParse(updatedValues["ThuongGap"], out thuongGapValue) ? thuongGapValue : false : false,
                            KhongBH = updatedValues.ContainsKey("KhongBH") ? bool.TryParse(updatedValues["KhongBH"], out khongBHValue) ? khongBHValue : false : false,
                            NgoaiDS = updatedValues.ContainsKey("NgoaiDS") ? bool.TryParse(updatedValues["NgoaiDS"], out ngoaiDSValue) ? ngoaiDSValue : false : false,
                            HieuLuc = updatedValues.ContainsKey("HieuLuc") ? bool.TryParse(updatedValues["HieuLuc"], out isActiveValue) ? isActiveValue : false : false,
                            ModifiedDate = DateTime.Now,
                            ModifiedUsers = nguoiSuDungCapNhat,
                            CreatedDate = DateTime.Now,
                            CreatedUsers = nguoiSuDungCapNhat
                        };

                        iCD10 = unitOfWork.ICD10Repository.Insert(iCD10);

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
