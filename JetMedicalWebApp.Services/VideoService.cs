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
    public class VideoService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Video";
        private string tenTable = "Video";

        public List<VideoDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<VideoDto> result = new List<VideoDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.VideoRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            VideoDto obj = new VideoDto();
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
                VideoDto obj = new VideoDto();

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

        public IEnumerable<VideoDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public VideoDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.VideoRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.VideoRepository.Get(m => ids.Contains(m.id)).ToList();
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
                            var tempQuery = unitOfWork.VideoRepository.Get(m => m.code.Equals(item.code));
                            if(tempQuery.Count() > 0)
                            {
                                unitOfWork.VideoRepository.DeleteRange(tempQuery);
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
        public bool IsExit(string name)
        {
            return unitOfWork.VideoRepository.Get(m => m.name.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
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

                var query = unitOfWork.VideoRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (Video obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
                    {
                        switch (item.Key)
                        {
                            case "avatar":
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

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }
        public string AddOrUpdate(List<VideoDto> listItem)
        {
            Video video = null;
            string resultMessage = string.Empty;
            int positionValue, totalViewValue, languageIdValue;
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

                            switch (itemPropertyInfo.Name)
                            {
                                case "code":
                                    updatedValues.Add(itemPropertyInfo.Name, code);
                                    break;
                                case "description":

                                    string value = itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty;
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        if (value.IndexOf("src=" + '"' + '/') > 0)
                                        {
                                            value = value.Replace("src=" + '"' + '/', "src=" + '"' + internalService.GetUrlHost() + '/');
                                        }
                                    }

                                    updatedValues.Add(itemPropertyInfo.Name, value);
                                    break;

                                case "avatar":
                                    string valueA = itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty;
                                    
                                    if (valueA.IndexOf(internalService.GetUrlHost()) < 0 || valueA == CommonConstants.FileNoImage)
                                    {
                                        updatedValues.Add(itemPropertyInfo.Name, internalService.GetUrlHost() + valueA);
                                    }

                                    break;


                                default:
                                    updatedValues.Add(itemPropertyInfo.Name, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty);
                                    break;
                            }
                        }

                        if (item.id > 0)
                        {
                            updatedData = new Dictionary<int, Dictionary<string, string>>();

                            if (updatedValues.ContainsKey("typeName"))
                            {
                                updatedValues.Remove("typeName");
                            }

                            updatedData.Add(item.id, updatedValues);
                            resultMessage = UpdateByIds(updatedData);
                        }
                        else
                        {
                            nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                            video = new Video()
                            {
                                title = updatedValues.ContainsKey("title") ? updatedValues["title"] : string.Empty,
                                keywords = updatedValues.ContainsKey("keywords") ? updatedValues["keywords"] : string.Empty,
                                descriptions = updatedValues.ContainsKey("descriptions") ? updatedValues["descriptions"] : string.Empty,
                                description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                                url = updatedValues.ContainsKey("url") ? updatedValues["url"] : string.Empty,
                                avatar = updatedValues.ContainsKey("avatar") ? (updatedValues["avatar"] == CommonConstants.FileNoImage ? string.Empty : updatedValues["avatar"]): string.Empty,
                                code = code,
                                name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                shorttitle = updatedValues.ContainsKey("shorttitle") ? updatedValues["shorttitle"] : string.Empty,
                                position = updatedValues.ContainsKey("position") ? Int32.TryParse(updatedValues["position"], out positionValue) ? positionValue : 0 : 0,
                                languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : 0 : 0,
                                totalView = updatedValues.ContainsKey("totalView") ? Int32.TryParse(updatedValues["totalView"], out totalViewValue) ? totalViewValue : 0 : 0,
                                isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                CreatedUserID = nguoiSuDungCapNhat.UserID
                            };

                            video = unitOfWork.VideoRepository.Insert(video);

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


    }
}
