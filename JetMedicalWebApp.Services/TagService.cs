using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Net;
using System.Globalization;

namespace JetMedicalWebApp.Services
{
    public class TagService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public List<TagDto> GetListExposeDto(Dictionary<string, string> filters, string selectedFields,
                                                                    string sortedColumnNames, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<TagDto> result = new List<TagDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.TagRepository.Get().Select("new (" + selectedFields + ")");

            if (filters != null)
            {
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "All":
                            stringFilter = "TagId.Contains(@0) OR Ten.Contains(@0)";
                            query = query.AsQueryable().Where(stringFilter, item.Value.Trim().ToLower());

                            break;

                        case "StringFilter":
                            query = query.AsQueryable().Where(item.Value);
                            break;

                        default:
                            TagDto obj = new TagDto();
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

            outData.Add("TotalRecords", query.Count().ToString());


            if (!string.IsNullOrEmpty(sortedColumnNames))
            {
                query = query.AsQueryable().OrderBy(sortedColumnNames);
            }

            if (inData != null)
            {
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
                TagDto obj = new TagDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "NgayTao":
                        case "NgayCapNhat":

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
        public IEnumerable<TagDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            return GetListExposeDto(filters, selectedFields, sortedColumnNames, null, out outData);
        }
        public TagDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            filters.Add("Id", id.ToString());
            return GetListExposeDto(filters, selectedFields, "Id asc", null, out outData).FirstOrDefault();
        }

        public string DeleteById(int id)
        {
            string result = string.Empty;

            try
            {
                var query = unitOfWork.TagRepository.GetByID(id);
                if (query != null)
                {
                    unitOfWork.TagRepository.Delete(query);
                    unitOfWork.Save();
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string Update(int id, Dictionary<string, string> updatedValues)
        {
            string messageResult = String.Empty;
            int intValue;
            double doubleValue;
            bool boolValue;
            DateTime ngayValue;
            Tag query = unitOfWork.TagRepository.GetByID(id);
            if (query != null)
            {
                foreach (KeyValuePair<string, string> item in updatedValues)
                {
                    string value = WebUtility.HtmlDecode(item.Value);
                    switch (item.Key)
                    {
                        case "Id":
                            break;

                        case "TagId":
                            InternalService internalService = new InternalService();
                            value = internalService.kiemTraUnitMetaTitleTag(value, id);
                            query.GetType().GetProperty(item.Key).SetValue(query, value, null);
                            break;

                        default:
                            Tag obj = new Tag();
                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                query.GetType().GetProperty(item.Key).SetValue(query, value, null);
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(int))
                            {
                                if (Int32.TryParse(value, out intValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, intValue, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(Nullable<int>))
                            {
                                if (Int32.TryParse(value, out intValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, intValue, null);
                                }
                                else
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, null, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(double))
                            {
                                if (double.TryParse(value, out doubleValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, doubleValue, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(Nullable<double>))
                            {
                                if (double.TryParse(value, out doubleValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, doubleValue, null);
                                }
                                else
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, null, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(bool))
                            {
                                if (bool.TryParse(value, out boolValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, boolValue, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(Nullable<bool>))
                            {
                                if (bool.TryParse(value, out boolValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, boolValue, null);
                                }
                                else
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, null, null);
                                }
                            }
                            else if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(DateTime))
                            {
                                if (DateTime.TryParseExact(value, Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, ngayValue, null);
                                }
                            }
                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(Nullable<DateTime>))
                            {
                                if (DateTime.TryParseExact(value, Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayValue))
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, ngayValue, null);
                                }
                                else
                                {
                                    query.GetType().GetProperty(item.Key).SetValue(query, null, null);
                                }
                            }

                            break;
                    }
                }
                unitOfWork.Save();
            }
            else
            {
                messageResult = "Không tìm thấy Id='" + id.ToString() + "'";
            }

            return messageResult;
        }
        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            Tag tag = null;
            string resultMessage = string.Empty;
            int idValue = -1, intValue = 0;
            List<string> ids, fields, values;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out idValue))
                    {
                        ids = new List<string>();
                        fields = new List<string>();
                        values = new List<string>();

                        foreach (KeyValuePair<string, string> item in updatedValues)
                        {
                            ids.Add(id);
                            fields.Add(item.Key);
                            values.Add(item.Value);
                        }

                        resultMessage = Update(idValue, updatedValues);

                    }
                    else
                    {
                        resultMessage = "Giá trị Id=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    if (updatedValues.ContainsKey("TagId"))
                    {
                        InternalService internalService = new InternalService();
                        updatedValues["TagId"] = internalService.kiemTraUnitMetaTitleTag(updatedValues["TagId"], -1);
                    }
                    tag = new Tag()
                    {
                        TagId = updatedValues.ContainsKey("TagId") ? updatedValues["TagId"] : string.Empty,
                    };

                    tag = unitOfWork.TagRepository.Insert(tag);
                  
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }

        public Tag getByTagId(string tagId)
        {
            Tag tag = unitOfWork.TagRepository.Get(m => m.TagId == tagId).FirstOrDefault();

            return tag;
        }
    }
}
