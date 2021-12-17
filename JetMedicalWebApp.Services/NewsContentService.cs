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
    public class NewsContentService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "NewsContent";
        private string tenTable = "NewsContent";

        public List<NewsContentDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<NewsContentDto> result = new List<NewsContentDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.NewsContentRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            NewsContentDto obj = new NewsContentDto();
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
                NewsContentDto obj = new NewsContentDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            break;

                        case "PackageId":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null) : -1, null);
                            break;

                        case "created_at":
                        case "updated_at":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString(CommonConstants.DateFormat2), null);
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
        public IEnumerable<NewsContentDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public NewsContentDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.NewsContentRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.NewsContentRepository.Get(m => ids.Contains(m.id)).ToList();
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
                            if(item.NewsContent_Tags != null)
                            {
                                if(item.NewsContent_Tags.Count() > 0)
                                {
                                    unitOfWork.NewsContent_TagRepository.DeleteRange(item.NewsContent_Tags);
                                }
                            }

                            unitOfWork.NewsContentRepository.Delete(item);
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
            return unitOfWork.NewsContentRepository.Get(m => m.name.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
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
                DateTime ngayValue;
                var query = unitOfWork.NewsContentRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (NewsContent obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
                    {
                        switch (item.Key)
                        {
                            case "id":
                            case "categoriName":
                            case "fullname":
                            case "fullshorttitle":
                            case "TypeId":
                            case "TypeName":
                            case "TypeCode":
                            case "DepartmentName":
                                
                                break;

                            case "avatar":
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value == CommonConstants.FileNoImage ? string.Empty : item.Value);
                                break;

                            case "created_at":

                                if (DateTime.TryParseExact(item.Value, Common.CommonConstants.DateFormat2, null, DateTimeStyles.None, out ngayValue))
                                {
                                    obj.GetType().GetProperty(item.Key).SetValue(obj, ngayValue, null);
                                }

                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value == CommonConstants.FileNoImage ? string.Empty : item.Value);
                                break;


                            case "Tags":

                                string[] tags = item.Value.Split(',');
                                if (tags.Length > 0)
                                {
                                    unitOfWork.NewsContent_TagRepository.DeleteRange(obj.NewsContent_Tags);
                                    unitOfWork.Save();
                                    foreach (var tag in tags)
                                    {
                                        Tag insertTag = new Tag();
                                        var tagId = StringHelpers.ToUnsignString(tag);
                                        var exitTag = this.CheckTag(tagId);
                                        if (!exitTag)
                                        {
                                            insertTag.TagId = tagId;
                                            insertTag.Name = tag;
                                            insertTag = unitOfWork.TagRepository.Insert(insertTag);
                                            unitOfWork.Save();
                                        }
                                        else
                                        {
                                            insertTag = unitOfWork.TagRepository.Get(m => m.TagId == tagId).FirstOrDefault();
                                        }

                                        NewsContent_Tag tinTuc_TagInsert = new NewsContent_Tag();
                                        tinTuc_TagInsert.NewsContent = obj;
                                        tinTuc_TagInsert.Tag = insertTag;
                                        obj.Tags = item.Value;
                                        tinTuc_TagInsert = unitOfWork.NewsContent_TagRepository.Insert(tinTuc_TagInsert);

                                        unitOfWork.Save();
                                    }
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
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "id: " + obj.id.ToString() + ", field: " + item.Key
                        });

                        obj.updated_at = System.DateTime.Now;
                        obj.ModifiedUserID = nguoiSuDungCapNhat != null ? nguoiSuDungCapNhat.UserID : -1;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }
        public string AddOrUpdate(List<NewsContentDto> listItem)
        {
            NewsContent newsContent = null;
            string resultMessage = string.Empty;
            int chuyenMucValue, categoriIdValue, nViewValue, positionValue, priceSaleValue, priceOldValue, isGroupValue, 
                languageIdValue, packageIdValue;
            bool approvedValue, isactiveValue, ishotValue, activePhoneValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            DateTime created_atValue;
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
                                case "ChuyenMuc":
                                case "nView":
                                    break;
                                
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

                                    if(valueA.IndexOf(internalService.GetUrlHost()) < 0 || valueA == CommonConstants.FileNoImage)
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
                            updatedData.Add(item.id, updatedValues);
                            resultMessage = UpdateByIds(updatedData);
                        }
                        else
                        {
                            nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                            newsContent = new NewsContent()
                            {
                                title = updatedValues.ContainsKey("title") ? updatedValues["title"] : string.Empty,
                                keywords = updatedValues.ContainsKey("keywords") ? updatedValues["keywords"] : string.Empty,
                                descriptions = updatedValues.ContainsKey("descriptions") ? updatedValues["descriptions"] : string.Empty,
                                description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                                created_by = updatedValues.ContainsKey("created_by") ? updatedValues["created_by"] : string.Empty,
                                avatar = updatedValues.ContainsKey("avatar") ? (updatedValues["avatar"] == CommonConstants.FileNoImage ? string.Empty : updatedValues["avatar"]): string.Empty,
                                url = updatedValues.ContainsKey("url") ? updatedValues["url"] : string.Empty,
                                categoriId = updatedValues.ContainsKey("categoriId") ? Int32.TryParse(updatedValues["categoriId"], out categoriIdValue) ? categoriIdValue : 0 : 0,
                                shorttitle = updatedValues.ContainsKey("shorttitle") ? updatedValues["shorttitle"] : string.Empty,
                                code = code,
                                name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                slug = updatedValues.ContainsKey("slug") ? updatedValues["slug"] : string.Empty,
                                updated_by = updatedValues.ContainsKey("updated_by") ? updatedValues["updated_by"] : string.Empty,
                                Tags = updatedValues.ContainsKey("Tags") ? updatedValues["Tags"] : string.Empty,
                                Noted = updatedValues.ContainsKey("Noted") ? updatedValues["Noted"] : string.Empty,
                                position = updatedValues.ContainsKey("position") ? Int32.TryParse(updatedValues["position"], out positionValue) ? positionValue : 0 : 0,
                                nView = updatedValues.ContainsKey("nView") ? Int32.TryParse(updatedValues["nView"], out nViewValue) ? nViewValue : 0 : 0,
                                isGroup = updatedValues.ContainsKey("isGroup") ? Int32.TryParse(updatedValues["isGroup"], out isGroupValue) ? isGroupValue : 0 : 0,
                                languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : 0 : 0,
                                priceSale = updatedValues.ContainsKey("priceSale") ? Int32.TryParse(updatedValues["priceSale"], out priceSaleValue) ? priceSaleValue : 0 : 0,
                                priceOld = updatedValues.ContainsKey("priceOld") ? Int32.TryParse(updatedValues["priceOld"], out priceOldValue) ? priceOldValue : 0 : 0,
                                approved = updatedValues.ContainsKey("approved") ? bool.TryParse(updatedValues["approved"], out approvedValue) ? approvedValue : false : false,
                                ishot = updatedValues.ContainsKey("ishot") ? bool.TryParse(updatedValues["ishot"], out ishotValue) ? ishotValue : false : false,
                                ActivePhone = updatedValues.ContainsKey("ActivePhone") ? bool.TryParse(updatedValues["ActivePhone"], out activePhoneValue) ? activePhoneValue : false : false,
                                isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                PackageId = updatedValues.ContainsKey("PackageId") ? Int32.TryParse(updatedValues["PackageId"], out packageIdValue) ? packageIdValue : new Nullable<Int32>() : new Nullable<Int32>(),
                                GoiKham = updatedValues.ContainsKey("GoiKham") ? updatedValues["GoiKham"] : string.Empty,
                                //ChuyenMuc = updatedValues.ContainsKey("ChuyenMuc") ? Int32.TryParse(updatedValues["ChuyenMuc"], out chuyenMucValue) ? chuyenMucValue : 0 : 0,
                                created_at = updatedValues.ContainsKey("created_at") ? (DateTime.TryParseExact(updatedValues["created_at"], CommonConstants.DateFormat2, null, DateTimeStyles.None, out created_atValue) ? created_atValue : DateTime.Now) : DateTime.Now,
                                updated_at = DateTime.Now,
                                ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                CreatedUserID = nguoiSuDungCapNhat.UserID,
                            };

                            newsContent = unitOfWork.NewsContentRepository.Insert(newsContent);

                            appHistoryServiceService.Add(new AppHistory()
                            {
                                Ma = maTable,
                                Ten = tenTable,
                                ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                            });

                            unitOfWork.Save();


                            if (updatedValues.ContainsKey("Tags"))
                            {
                                if (!string.IsNullOrEmpty(updatedValues["Tags"]))
                                {
                                    string[] tags = updatedValues["Tags"].Split(',');

                                    foreach (var tag in tags)
                                    {
                                        Tag insertTag = new Tag();
                                        var tagId = StringHelpers.ToUnsignString(tag);
                                        var exitTag = this.CheckTag(tagId);
                                        if (!exitTag)
                                        {
                                            insertTag.TagId = tagId;
                                            insertTag.Name = tag;
                                            insertTag = unitOfWork.TagRepository.Insert(insertTag);
                                            unitOfWork.Save();
                                        }
                                        else
                                        {
                                            insertTag = unitOfWork.TagRepository.Get(m => m.TagId == tagId).FirstOrDefault();
                                        }

                                        NewsContent_Tag tinTuc_TagInsert = new NewsContent_Tag();
                                        tinTuc_TagInsert.NewsContent = newsContent;
                                        tinTuc_TagInsert.Tag = insertTag;
                                        tinTuc_TagInsert = unitOfWork.NewsContent_TagRepository.Insert(tinTuc_TagInsert);
                                        unitOfWork.Save();
                                    }
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


        public bool CheckTag(string id)
        {
            return unitOfWork.TagRepository.Get(m => m.TagId == id).Count() > 0;
        }


        public List<NewsContentDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                   out Dictionary<string, string> outData)
        {
            List<NewsContentDto> result = new List<NewsContentDto>();
            string tableName = "uv_NewsContent";
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
                            NewsContentDto obj = new NewsContentDto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<NewsContentDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                NewsContentDto obj = new NewsContentDto();

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

        public List<Tag> GetListTagByTinTucId(int id)
        {
            List<Tag> tags = new List<Tag>();
            var TinTuc_Tag = unitOfWork.NewsContent_TagRepository.Get(m => m.NewsContent.id == id);
            if (TinTuc_Tag != null)
            {
                if (TinTuc_Tag.Count() > 0)
                {
                    tags = TinTuc_Tag.Select(m => m.Tag).ToList();
                }
            }

            return tags;
        }

        public void TangLuotXem(int id)
        {
            var news = unitOfWork.NewsContentRepository.GetByID(id);
            news.nView += 1;
            unitOfWork.Save();
        }
    }
}
