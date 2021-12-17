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
    public class ContactService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Contact";
        private string tenTable = "Contact";

        public List<ContactDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<ContactDto> result = new List<ContactDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.ContactRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            ContactDto obj = new ContactDto();
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
                ContactDto obj = new ContactDto();

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

        public IEnumerable<ContactDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public ContactDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.ContactRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.ContactRepository.Get(m => ids.Contains(m.id)).ToList();
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
                            var tempQuery = unitOfWork.ContactRepository.Get(m => m.languageCode.Equals(item.languageCode));
                            if(tempQuery.Count() > 0)
                            {
                                unitOfWork.ContactRepository.DeleteRange(tempQuery);
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
        public string UpdateByIds(Dictionary<int, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.ContactRepository.Get(m => ids.Contains(m.id)).ToList();

                foreach (Contact obj in query)
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
        public string AddOrUpdate(List<ContactDto> listItem)
        {
            Contact contact = null;
            string resultMessage = string.Empty;
            int languageIdValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;

            var code = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            try
            {
                foreach (var item in listItem)
                {
                    Dictionary<string, string> updatedValues = new Dictionary<string, string>();
                    foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                    {
                        switch (itemPropertyInfo.Name)
                        {
                            case "languageCode":
                                updatedValues.Add(itemPropertyInfo.Name, code);
                                break;

                            case "footer":
                            case "note":
                                
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
                        contact = new Contact()
                        {
                            address = updatedValues.ContainsKey("address") ? updatedValues["address"] : string.Empty,
                            phone = updatedValues.ContainsKey("phone") ? updatedValues["phone"] : string.Empty,
                            hotline = updatedValues.ContainsKey("hotline") ? updatedValues["hotline"] : string.Empty,
                            email = updatedValues.ContainsKey("email") ? updatedValues["email"] : string.Empty,
                            fax = updatedValues.ContainsKey("fax") ? updatedValues["fax"] : string.Empty,
                            note = updatedValues.ContainsKey("note") ? updatedValues["note"] : string.Empty,
                            languageCode = code,
                            footer = updatedValues.ContainsKey("footer") ? updatedValues["footer"] : string.Empty,
                            maps = updatedValues.ContainsKey("maps") ? updatedValues["maps"] : string.Empty,
                            latitude = updatedValues.ContainsKey("latitude") ? updatedValues["latitude"] : string.Empty,
                            longitude = updatedValues.ContainsKey("longitude") ? updatedValues["longitude"] : string.Empty,
                            languageId = updatedValues.ContainsKey("languageId") ? Int32.TryParse(updatedValues["languageId"], out languageIdValue) ? languageIdValue : CommonConstants.TiengViet : CommonConstants.TiengViet,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserID = nguoiSuDungCapNhat.UserID,
                            CreatedUserID = nguoiSuDungCapNhat.UserID
                        };

                        contact = unitOfWork.ContactRepository.Insert(contact);

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
