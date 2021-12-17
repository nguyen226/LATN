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
    public class NewsTypeService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "NewsType";
        private string tenTable = "NewsType";

        public List<NewsTypeDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<NewsTypeDto> result = new List<NewsTypeDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.NewsTypeRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            NewsTypeDto obj = new NewsTypeDto();
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
                NewsTypeDto obj = new NewsTypeDto();

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
        public IEnumerable<NewsTypeDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public NewsTypeDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.NewsTypeRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.NewsTypeRepository.Get(m => ids.Contains(m.id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "id: " + string.Join(";", ids)
                        });

                        foreach (var item in query.ToList())
                        {
                            var tempQuery = unitOfWork.NewsTypeRepository.Get(m => m.languageCode.Equals(item.languageCode));
                            if(tempQuery.Count() > 0)
                            {
                                unitOfWork.NewsTypeRepository.DeleteRange(tempQuery);
                            }
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
        public bool IsExit(string code)
        {
            return unitOfWork.NewsTypeRepository.Get(m => m.code.Trim().ToLower().Equals(code.Trim().ToLower())).FirstOrDefault() != null;
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

                var query = unitOfWork.NewsTypeRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (NewsType obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
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
                            GhiChu = "id: " + obj.id.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }
        public string AddOrUpdate(List<NewsTypeDto> listItem)
        {
            NewsType newsType = null;
            string resultMessage = string.Empty;
            int orderByValue, languageIdValue;
            bool isCateValue, isactiveValue, isMennuValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            try
            {
                string languageCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");


                foreach (var item in listItem)
                {
                    Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                    foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                    {
                        if (itemPropertyInfo.Name == "languageCode")
                        {
                            updatedValues.Add(itemPropertyInfo.Name, languageCode);
                        }
                        else
                        {
                            updatedValues.Add(itemPropertyInfo.Name, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty);
                        }
                    }
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);








                    if (!string.IsNullOrEmpty(item.name))
                    {
                        if (!string.IsNullOrEmpty(item.code))
                        {
                            //kiểm tra bài viết theo ngôn ngữ và code
                            var check = unitOfWork.NewsTypeRepository.Get(x => x.code == item.code && item.languageId == x.languageId).Count()> 0;
                            if (check)
                            {
                                //update
                                updatedData = new Dictionary<int, Dictionary<string, string>>();
                                updatedData.Add(item.id, updatedValues);
                                resultMessage = UpdateByIds(updatedData);
                            }
                            else
                            {
                                //thêm mới
                                newsType = new NewsType()
                                {
                                    code = updatedValues.ContainsKey("code") ? updatedValues["code"] : string.Empty,
                                    name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                    slug = updatedValues.ContainsKey("slug") ? updatedValues["slug"] : string.Empty,
                                    show = updatedValues.ContainsKey("show") ? updatedValues["show"] : string.Empty,
                                    languageCode = item.languageCode,
                                    orderBy = updatedValues.ContainsKey("orderBy") ? Int32.TryParse(updatedValues["orderBy"], out orderByValue) ? orderByValue : 0 : 0,
                                    languageId = item.languageId,
                                    isCate = updatedValues.ContainsKey("isCate") ? bool.TryParse(updatedValues["isCate"], out isCateValue) ? isCateValue : false : false,
                                    isMennu = updatedValues.ContainsKey("isMennu") ? bool.TryParse(updatedValues["isMennu"], out isMennuValue) ? isMennuValue : false : false,
                                    isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                    ModifiedDate = DateTime.Now,
                                    CreatedDate = DateTime.Now,
                                    ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                    CreatedUserID = nguoiSuDungCapNhat.UserID
                                };

                                newsType = unitOfWork.NewsTypeRepository.Insert(newsType);

                                appHistoryServiceService.Add(new AppHistory()
                                {
                                    Ma = maTable,
                                    Ten = tenTable,
                                    ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                                });

                                unitOfWork.Save();
                            }
                        }
                        else
                        {
                            
                            if (item.id > 0)
                            {
                                updatedData = new Dictionary<int, Dictionary<string, string>>();
                                updatedData.Add(item.id, updatedValues);
                                resultMessage = UpdateByIds(updatedData);
                            }
                            else
                            {
                                if (!IsExit(updatedValues["code"]))
                                {
                                    newsType = new NewsType()
                                    {
                                        code = updatedValues.ContainsKey("code") ? updatedValues["code"] : string.Empty,
                                        name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                        slug = updatedValues.ContainsKey("slug") ? updatedValues["slug"] : string.Empty,
                                        show = updatedValues.ContainsKey("show") ? updatedValues["show"] : string.Empty,
                                        languageCode = updatedValues.ContainsKey("languageCode") ? updatedValues["languageCode"] : languageCode,
                                        orderBy = updatedValues.ContainsKey("orderBy") ? Int32.TryParse(updatedValues["orderBy"], out orderByValue) ? orderByValue : 0 : 0,
                                        languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : 0 : 0,
                                        isCate = updatedValues.ContainsKey("isCate") ? bool.TryParse(updatedValues["isCate"], out isCateValue) ? isCateValue : false : false,
                                        isMennu = updatedValues.ContainsKey("isMennu") ? bool.TryParse(updatedValues["isMennu"], out isMennuValue) ? isMennuValue : false : false,
                                        isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                        ModifiedDate = DateTime.Now,
                                        CreatedDate = DateTime.Now,
                                        ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                        CreatedUserID = nguoiSuDungCapNhat.UserID
                                    };

                                    newsType = unitOfWork.NewsTypeRepository.Insert(newsType);

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
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }

    }
}
