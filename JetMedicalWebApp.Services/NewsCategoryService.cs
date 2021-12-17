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
    public class NewsCategoryService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "NewsCategory";
        private string tenTable = "NewsCategory";

        public List<NewsCategoryDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<NewsCategoryDto> result = new List<NewsCategoryDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.NewsCategoryRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            NewsCategoryDto obj = new NewsCategoryDto();
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
                NewsCategoryDto obj = new NewsCategoryDto();

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

        public List<NewsCategoryDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<NewsCategoryDto> result = new List<NewsCategoryDto>();
            string tableName = "uv_NewsCategory";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData.ContainsKey(CommonConstants.StrSelectedFields) && inData.ContainsKey(CommonConstants.StrSelectedFields) ? inData[CommonConstants.StrSelectedFields] : "*";
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = inData.ContainsKey(CommonConstants.StrSortedColumnNames) && !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "MA_LK ASC";

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
                            NewsCategoryDto obj = new NewsCategoryDto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<NewsCategoryDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                NewsCategoryDto obj = new NewsCategoryDto();

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

        public IEnumerable<NewsCategoryDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public NewsCategoryDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.NewsCategoryRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.NewsCategoryRepository.Get(m => ids.Contains(m.id)).ToList();
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

                            var list_categorys_child_ids = item.child_ids.Split(',');
                            foreach (var child_ids in list_categorys_child_ids)
                            {
                                if (!string.IsNullOrEmpty(child_ids) && child_ids != item.id.ToString())
                                {
                                    var list_categorys_child = unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(child_ids));
                                    if(list_categorys_child != null)
                                    {
                                        unitOfWork.NewsCategoryRepository.Delete(list_categorys_child);
                                    }
                                }
                            }

                            var tempQuery = unitOfWork.NewsCategoryRepository.Get(m => m.code.Equals(item.code));
                            if(tempQuery.Count() > 0)
                            {
                                unitOfWork.NewsCategoryRepository.DeleteRange(tempQuery);
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
            return unitOfWork.NewsCategoryRepository.Get(m => m.name.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
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

                var query = unitOfWork.NewsCategoryRepository.Get(m => ids.Contains(m.id)).ToList();
                int intValue;
                foreach (NewsCategory obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.id])
                    {
                        switch (item.Key)
                        {
                            case "banner":
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value == CommonConstants.FileNoImage ? string.Empty : item.Value);
                                break;

                            case "parentId":
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (intValue != obj.parentId && intValue > 0)
                                    {
                                        // lấy parent_id cũ
                                        var oldParentIds = obj.parent_ids.Split(',');

                                        //THay đổi parentId của obj hiện tại
                                        Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);

                                        //Xóa giá trị child_ids trong child_ids của các old_parent
                                        if (oldParentIds.Count(m => !string.IsNullOrEmpty(m)) > 0)
                                        {
                                            foreach (var categorys_old_parent_ids in oldParentIds)
                                            {
                                                if (categorys_old_parent_ids != obj.id.ToString())
                                                {
                                                    var list_categorys_old_parent = unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(categorys_old_parent_ids));
                                                    if ((list_categorys_old_parent != null) && (list_categorys_old_parent.child_ids.IndexOf("," + obj.child_ids) > 0))
                                                    {
                                                        list_categorys_old_parent.child_ids = list_categorys_old_parent.child_ids.Replace("," + obj.child_ids, string.Empty);
                                                    }
                                                }
                                            }
                                        }

                                        //THay đổi parent_ids của obj hiện tại và child_ids của các parent
                                        if (intValue != 0)
                                        {
                                            var list_categorys_parent = unitOfWork.NewsCategoryRepository.GetByID(intValue);
                                            if (list_categorys_parent != null)
                                            {
                                                obj.parent_ids = list_categorys_parent.parent_ids != null ? (obj.id + "," + list_categorys_parent.parent_ids) : (obj.id + "," + intValue);

                                                var list_categorys_parent_ids = list_categorys_parent.parent_ids.Split(',');
                                                foreach (var categorys_parent_ids in list_categorys_parent_ids)
                                                {
                                                    var list_categorys_parent_parent = !string.IsNullOrEmpty(categorys_parent_ids) ? unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(categorys_parent_ids)) : null;
                                                    if ((list_categorys_parent_parent != null) && !string.IsNullOrEmpty(obj.child_ids))
                                                    {
                                                        list_categorys_parent_parent.child_ids += "," + obj.child_ids;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            obj.parent_ids = obj.id.ToString();
                                        }

                                        //THay đổi parent_ids của các child_ids
                                        var list_categorys_child_ids = obj.child_ids.Split(',');
                                        foreach (var categorys_child_ids in list_categorys_child_ids.Where(m => !string.IsNullOrEmpty(m)))
                                        {
                                            if (categorys_child_ids != obj.id.ToString())
                                            {
                                                var list_categorys_child = unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(categorys_child_ids));
                                                list_categorys_child.parent_ids = list_categorys_child.id + "," + obj.parent_ids;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        obj.parent_ids = obj.id.ToString();
                                    }
                                }

                                break;

                            case "typeName":
                            case "GoiKham":
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
        public string AddOrUpdate(List<NewsCategoryDto> listItem)
        {
            NewsCategory newsCategory = null;
            string resultMessage = string.Empty;
            int orderByValue, parentIdValue, typeIdValue, isGroupValue, languageIdValue;
            bool isHomeValue, isactiveValue, activePhoneValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;

            try
            {
                var code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                foreach (var item in listItem)
                {
                    Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                    foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                    {
                        if (itemPropertyInfo.Name == "parentName")
                        {
                            break;
                        }
                        else if (itemPropertyInfo.Name == "code")
                        {
                            updatedValues.Add(itemPropertyInfo.Name, code);
                        }
                        else if (itemPropertyInfo.Name == "description")
                        {

                            string value = itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString() : string.Empty;
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (value.IndexOf("src=" + '"' + '/') > 0)
                                {
                                    value = value.Replace("src=" + '"' + '/', "src=" + '"' + internalService.GetUrlHost() + '/');
                                }
                            }

                            updatedValues.Add(itemPropertyInfo.Name, value);
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
                            var check = unitOfWork.NewsCategoryRepository.Get(x => x.code == item.code && item.languageId == x.languageId).Count() > 0;
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
                                newsCategory = new NewsCategory()
                                {
                                    title = updatedValues.ContainsKey("title") ? updatedValues["title"] : string.Empty,
                                    keywords = updatedValues.ContainsKey("keywords") ? updatedValues["keywords"] : string.Empty,
                                    descriptions = updatedValues.ContainsKey("descriptions") ? updatedValues["descriptions"] : string.Empty,
                                    description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                                    url = updatedValues.ContainsKey("url") ? updatedValues["url"] : string.Empty,
                                    typeId = updatedValues.ContainsKey("typeId") ? Int32.TryParse(updatedValues["typeId"], out typeIdValue) ? typeIdValue : 0 : 0,
                                    parentId = updatedValues.ContainsKey("parentId") ? Int32.TryParse(updatedValues["parentId"], out parentIdValue) ? parentIdValue : 0 : 0,

                                    banner = updatedValues.ContainsKey("banner") ? (updatedValues["banner"] == CommonConstants.FileNoImage ? string.Empty : updatedValues["banner"]) : string.Empty,
                                    code = item.code,
                                    name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                    slug = updatedValues.ContainsKey("slug") ? updatedValues["slug"] : string.Empty,
                                    odx = updatedValues.ContainsKey("odx") ? Int32.TryParse(updatedValues["odx"], out orderByValue) ? orderByValue : 0 : 0,
                                    IconPhone = updatedValues.ContainsKey("IconPhone") ? updatedValues["IconPhone"] : string.Empty,
                                    isGroup = updatedValues.ContainsKey("isGroup") ? Int32.TryParse(updatedValues["isGroup"], out isGroupValue) ? isGroupValue : 0 : 0,
                                    languageId = item.languageId,
                                    isHome = updatedValues.ContainsKey("isHome") ? bool.TryParse(updatedValues["isHome"], out isHomeValue) ? isHomeValue : false : false,
                                    ActivePhone = updatedValues.ContainsKey("ActivePhone") ? bool.TryParse(updatedValues["ActivePhone"], out activePhoneValue) ? activePhoneValue : false : false,
                                    isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                    CreatedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                    CreatedUserID = nguoiSuDungCapNhat.UserID
                                };

                                newsCategory = unitOfWork.NewsCategoryRepository.Insert(newsCategory);

                                appHistoryServiceService.Add(new AppHistory()
                                {
                                    Ma = maTable,
                                    Ten = tenTable,
                                    ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                                });

                                unitOfWork.Save();

                                newsCategory.child_ids = newsCategory.id.ToString();
                                newsCategory.parent_ids = newsCategory.id.ToString();
                                unitOfWork.Save();

                                var list_categorys_parent = unitOfWork.NewsCategoryRepository.GetByID(newsCategory.parentId);
                                if (list_categorys_parent != null)
                                {
                                    newsCategory.parent_ids = list_categorys_parent.parent_ids != null ? (newsCategory.id + "," + list_categorys_parent.parent_ids) : (newsCategory.id + "," + newsCategory.parentId);

                                    var list_categorys_parent_ids = list_categorys_parent.parent_ids.Split(',');
                                    if (list_categorys_parent_ids.Count() > 0)
                                    {
                                        foreach (var u in list_categorys_parent_ids)
                                        {
                                            {
                                                var list_categorys_parent_parent = unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(u));
                                                if (list_categorys_parent_parent != null)
                                                {
                                                    list_categorys_parent_parent.child_ids += "," + newsCategory.id.ToString();
                                                }
                                            }
                                        }
                                    }

                                    unitOfWork.Save();
                                }
                            }
                        }
                        else
                        {

                            if (item.id > 0)
                            {
                                updatedData = new Dictionary<int, Dictionary<string, string>>();

                                if (updatedValues.ContainsKey("typeName"))
                                {
                                    updatedValues.Remove("typeName");
                                }

                                if (updatedValues.ContainsKey("typeCode"))
                                {
                                    updatedValues.Remove("typeCode");
                                }

                                if (updatedValues.ContainsKey("GoiKham"))
                                {
                                    updatedValues.Remove("GoiKham");
                                }

                                updatedData.Add(item.id, updatedValues);
                                resultMessage = UpdateByIds(updatedData);
                            }
                            else
                            {
                                if (!IsExit(updatedValues["name"]))
                                {
                                    newsCategory = new NewsCategory()
                                    {
                                        title = updatedValues.ContainsKey("title") ? updatedValues["title"] : string.Empty,
                                        keywords = updatedValues.ContainsKey("keywords") ? updatedValues["keywords"] : string.Empty,
                                        descriptions = updatedValues.ContainsKey("descriptions") ? updatedValues["descriptions"] : string.Empty,
                                        description = updatedValues.ContainsKey("description") ? updatedValues["description"] : string.Empty,
                                        url = updatedValues.ContainsKey("url") ? updatedValues["url"] : string.Empty,
                                        typeId = updatedValues.ContainsKey("typeId") ? Int32.TryParse(updatedValues["typeId"], out typeIdValue) ? typeIdValue : 0 : 0,
                                        parentId = updatedValues.ContainsKey("parentId") ? Int32.TryParse(updatedValues["parentId"], out parentIdValue) ? parentIdValue : 0 : 0,

                                        banner = updatedValues.ContainsKey("banner") ? (updatedValues["banner"] == CommonConstants.FileNoImage ? string.Empty : updatedValues["banner"]) : string.Empty,
                                        code = code,
                                        name = updatedValues.ContainsKey("name") ? updatedValues["name"] : string.Empty,
                                        slug = updatedValues.ContainsKey("slug") ? updatedValues["slug"] : string.Empty,
                                        odx = updatedValues.ContainsKey("odx") ? Int32.TryParse(updatedValues["odx"], out orderByValue) ? orderByValue : 0 : 0,
                                        IconPhone = updatedValues.ContainsKey("IconPhone") ? updatedValues["IconPhone"] : string.Empty,
                                        isGroup = updatedValues.ContainsKey("isGroup") ? Int32.TryParse(updatedValues["isGroup"], out isGroupValue) ? isGroupValue : 0 : 0,
                                        languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : 0 : 0,
                                        isHome = updatedValues.ContainsKey("isHome") ? bool.TryParse(updatedValues["isHome"], out isHomeValue) ? isHomeValue : false : false,
                                        ActivePhone = updatedValues.ContainsKey("ActivePhone") ? bool.TryParse(updatedValues["ActivePhone"], out activePhoneValue) ? activePhoneValue : false : false,
                                        isactive = updatedValues.ContainsKey("isactive") ? bool.TryParse(updatedValues["isactive"], out isactiveValue) ? isactiveValue : false : false,
                                        CreatedDate = DateTime.Now,
                                        ModifiedDate = DateTime.Now,
                                        ModifiedUserID = nguoiSuDungCapNhat.UserID,
                                        CreatedUserID = nguoiSuDungCapNhat.UserID
                                    };

                                    newsCategory = unitOfWork.NewsCategoryRepository.Insert(newsCategory);

                                    appHistoryServiceService.Add(new AppHistory()
                                    {
                                        Ma = maTable,
                                        Ten = tenTable,
                                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                                    });

                                    unitOfWork.Save();



                                    newsCategory.child_ids = newsCategory.id.ToString();
                                    newsCategory.parent_ids = newsCategory.id.ToString();
                                    unitOfWork.Save();

                                    var list_categorys_parent = unitOfWork.NewsCategoryRepository.GetByID(newsCategory.parentId);
                                    if (list_categorys_parent != null)
                                    {
                                        newsCategory.parent_ids = list_categorys_parent.parent_ids != null ? (newsCategory.id + "," + list_categorys_parent.parent_ids) : (newsCategory.id + "," + newsCategory.parentId);

                                        var list_categorys_parent_ids = list_categorys_parent.parent_ids.Split(',');
                                        if (list_categorys_parent_ids.Count() > 0)
                                        {
                                            foreach (var u in list_categorys_parent_ids)
                                            {
                                                {
                                                    var list_categorys_parent_parent = unitOfWork.NewsCategoryRepository.GetByID(Int32.Parse(u));
                                                    if (list_categorys_parent_parent != null)
                                                    {
                                                        list_categorys_parent_parent.child_ids += "," + newsCategory.id.ToString();
                                                    }
                                                }
                                            }
                                        }

                                        unitOfWork.Save();
                                    }
                                }
                                else
                                {
                                    resultMessage = "Tên đã tồn tại";
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


        public List<NewsCategoryDto> GetByStore( string selectedFields, string stringFilter, int top)
        {
            string sqlCommand = "exec pListCategorysGets @selectedFields, @stringFilter, @pageSize, @pageNumber";
            List<NewsCategoryDto> result = new List<NewsCategoryDto>();

            var param1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "selectedFields",
                Value = selectedFields
            };

            var param2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "stringFilter",
                Value = stringFilter
            };

            var param3 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "pageSize",
                Value = top
            };

            var param4 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "pageNumber",
                Value = 0
            };
            result = unitOfWork.DataContext.Database.SqlQuery<NewsCategoryDto>(sqlCommand, param1, param2, param3, param4).AsEnumerable().ToList();

            return result;
        }


        public List<NewsCategoryDto> GetListDataFromStoreExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                               out Dictionary<string, string> outData)
        {
            List<NewsCategoryDto> result = new List<NewsCategoryDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();
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
                            NewsCategoryDto obj = new NewsCategoryDto();

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
            string selectedFields = inData.ContainsKey(CommonConstants.StrSelectedFields) && inData.ContainsKey(CommonConstants.StrSelectedFields) ? inData[CommonConstants.StrSelectedFields] : "*";
            string sqlCommand = "exec pListCategorysGets @selectedFields, @stringFilter, @pageSize, @pageNumber";
            var param1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "selectedFields",
                Value = selectedFields
            };

            var param2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "stringFilter",
                Value = (!string.IsNullOrEmpty(stringFilter) ? " AND " + stringFilter : string.Empty)
            };

            var param3 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "pageSize",
                Value = (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999")
            };

            var param4 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "pageNumber",
                Value = (inData.ContainsKey("Skip") ? inData["Skip"] : "0")
            };
            result = unitOfWork.DataContext.Database.SqlQuery<NewsCategoryDto>(sqlCommand, param1, param2, param3, param4).AsEnumerable().ToList();

            string tableName = "uv_NewsCategory";
            sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);
            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());
            return result;
        }


    }
}
