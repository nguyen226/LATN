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
    public class PackageServiceService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "PackageService";
        private string tenTable = "PackageService";

        public List<PackageServiceDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<PackageServiceDto> result = new List<PackageServiceDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.PackageServiceRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            PackageServiceDto obj = new PackageServiceDto();
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
                PackageServiceDto obj = new PackageServiceDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {

                        case "PackageService_GroupPermissions":
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
        public IEnumerable<PackageServiceDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public PackageServiceDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.PackageServiceRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.PackageServiceRepository.Get(m => ids.Contains(m.id)).ToList();
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
                            unitOfWork.PackageServiceRepository.Delete(item);
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

                var query = unitOfWork.PackageServiceRepository.Get(m => ids.Contains(m.id)).ToList();
                bool checkUpdate = true;

                if (checkUpdate)
                {
                    foreach (PackageService obj in query)
                    {
                        var tempUpdateData = updatedData[obj.id];
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
                                GhiChu = "Id: " + obj.id.ToString() + ", field: " + item.Key
                            });
                        }
                    }

                    unitOfWork.Save();
                }
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            PackageService packageService = null;
            string resultMessage = string.Empty;
            Dictionary<Int32, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            int idValue, categoriIdValue, positionValue, languageIdValue, priceSaleValue, priceOldValue, nViewValue, ishotValue;
            bool isactiveValue;
            DateTime created_atValue;

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
                    packageService = new PackageService()
                    {
                        title = updatedValues.ContainsKey("title") ? updatedValues["title"] : string.Empty,
                        keywords = updatedValues.ContainsKey("keywords") ? updatedValues["keywords"] : string.Empty,
                        descriptions = updatedValues.ContainsKey("descriptions") ? updatedValues["descriptions"] : string.Empty,
                        avatar = updatedValues.ContainsKey("avatar") ? updatedValues["avatar"] : string.Empty,
                        url = updatedValues.ContainsKey("url") ? updatedValues["url"] : string.Empty,
                        categoriId = updatedValues.ContainsKey("categoriId") ? Int32.TryParse(updatedValues["categoriId"], out categoriIdValue) ? categoriIdValue : -1 : -1,
                        categoriName = updatedValues.ContainsKey("categoriName") ? updatedValues["categoriName"] : string.Empty,
                        name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                        shorttitle = updatedValues.ContainsKey("shorttitle") ? updatedValues["shorttitle"] : string.Empty,
                        description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                        position = updatedValues.ContainsKey("position") ? Int32.TryParse(updatedValues["position"], out positionValue) ? positionValue : 0 : 0,
                        isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                        languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : CommonConstants.TiengViet : CommonConstants.TiengViet,
                        code = updatedValues.ContainsKey("code") ? updatedValues["code"] : string.Empty,
                        priceSale = updatedValues.ContainsKey("priceSale") ? Int32.TryParse(updatedValues["priceSale"], out priceSaleValue) ? priceSaleValue : 0 : 0,
                        priceOld = updatedValues.ContainsKey("priceOld") ? Int32.TryParse(updatedValues["priceOld"], out priceOldValue) ? priceOldValue : 0 : 0,
                        nView = updatedValues.ContainsKey("nView") ? Int32.TryParse(updatedValues["nView"], out nViewValue) ? nViewValue : 0 : 0,
                        ishot = updatedValues.ContainsKey("ishot") ? Int32.TryParse(updatedValues["ishot"], out ishotValue) ? ishotValue : 0 : 0,
                        created_at = updatedValues.ContainsKey("created_at") ? (DateTime.TryParseExact(updatedValues["created_at"], CommonConstants.DateFormat, null, DateTimeStyles.None, out created_atValue) ? created_atValue : DateTime.Now) : DateTime.Now,
                    };

                    packageService = unitOfWork.PackageServiceRepository.Insert(packageService);

                    appHistoryServiceService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                    });

                    unitOfWork.Save();
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
