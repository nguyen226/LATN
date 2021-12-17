using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.IO;

namespace JetMedicalWebApp.Services
{
    public class AppConfigService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "AppConfig";
        private string tenTable = "AppConfig";

        public List<AppConfigDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<AppConfigDto> result = new List<AppConfigDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.AppConfigRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            AppConfigDto obj = new AppConfigDto();
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
                AppConfigDto obj = new AppConfigDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "AppConfig_DonVis":
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
        public IEnumerable<AppConfigDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public AppConfigDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "ID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public AppConfigDto GetByActiveExposeDto(string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Active= True";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.AppConfigRepository.Get().Select("new (" + selectedFields + ")")
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
                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadFileLogo);
                    List<string> imageNames = new List<string>();

                    var query = unitOfWork.AppConfigRepository.Get(m => ids.Contains(m.ID)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "ID: " + string.Join(";", ids)
                        });
                      
                        foreach(var item in query)
                        {
                            if (item.Active == false)
                            {
                                if (!string.IsNullOrEmpty(item.Logo))
                                {
                                    imageNames.Add(item.Logo);
                                }

                                unitOfWork.AppConfigRepository.Delete(item);
                            }
                        }

                        unitOfWork.Save();
                    }

                    if (imageNames.Count() > 0)
                    {
                        foreach (string imageName in imageNames)
                        {
                            var array = imageName.Split('/');
                            string fileName = array[imageName.Split('/').Length - 1];
                            string tempPath = Path.Combine(path, fileName);
                            if (File.Exists(tempPath))
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                File.Delete(tempPath);
                            }
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

        public string UpdateByIds(Dictionary<int, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.AppConfigRepository.Get(m => ids.Contains(m.ID)).ToList();

                foreach (AppConfig obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.ID])
                    {
                        switch (item.Key)
                        {
                            case "Active":
                                bool activeValue = false;

                                if (bool.TryParse(item.Value, out activeValue))
                                {
                                    if (activeValue == true)
                                    {
                                        foreach (AppConfig applicationConfigTemp in unitOfWork.AppConfigRepository.Get(m => m.Active == true).ToList())
                                        {
                                            applicationConfigTemp.Active = false;
                                        }
                                    }
                                    obj.Active = activeValue;
                                }

                                break;

                            case "Logo":
                                if (!string.IsNullOrEmpty(obj.Logo) && obj.Logo != item.Value)
                                {
                                    var array = obj.Logo.Split('/');
                                    string fileName = array[obj.Logo.Split('/').Length -1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadFileLogo);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }

                                obj.Logo = item.Value;

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
                            GhiChu = "ID: " + obj.ID.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            AppConfig appConfig = null;
            string resultMessage = string.Empty;
            int idValue = -1;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            bool boolValue = false, activeValue = false, mailSSLValue = false;
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    if (Int32.TryParse(id, out idValue))
                    {
                        updatedData = new Dictionary<int, Dictionary<string, string>>();
                        updatedData.Add(idValue, updatedValues);
                        resultMessage = UpdateByIds(updatedData);
                    }
                    else
                    {
                        resultMessage = "Giá trị ID=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    int lastestID = 0;

                    if (updatedValues.ContainsKey("Active") && bool.TryParse(updatedValues["Active"], out activeValue))
                    {
                        if (activeValue == true)
                        {
                            IEnumerable<AppConfig> applicationConfigs = unitOfWork.AppConfigRepository.Get(m => m.Active == true);
                            foreach (AppConfig applicationConfigTemp in applicationConfigs)
                            {
                                applicationConfigTemp.Active = false;
                            }

                            unitOfWork.Save();
                        }
                    }

                    appConfig = new AppConfig()
                    {
                        ID = lastestID + 1,
                        CompanyName = updatedValues.ContainsKey("CompanyName") ? updatedValues["CompanyName"] : string.Empty,
                        Logo = updatedValues.ContainsKey("Logo") ? updatedValues["Logo"] : string.Empty,
                        Active = activeValue,
                        PhoneAuthentication = updatedValues.ContainsKey("PhoneAuthentication") ? (bool.TryParse(updatedValues["PhoneAuthentication"], out boolValue) ? boolValue : false) : false,
                        Hotline = updatedValues.ContainsKey("Hotline") ? updatedValues["Hotline"] : string.Empty,
                        Facebook = updatedValues.ContainsKey("Facebook") ? updatedValues["Facebook"] : string.Empty,
                        Zalo = updatedValues.ContainsKey("Zalo") ? updatedValues["Zalo"] : string.Empty,
                        Website = updatedValues.ContainsKey("Website") ? updatedValues["Website"] : string.Empty,
                        Email = updatedValues.ContainsKey("Email") ? updatedValues["Email"] : string.Empty,
                        SmsAccount = updatedValues.ContainsKey("SmsAccount") ? updatedValues["SmsAccount"] : string.Empty,
                        SmsPass = updatedValues.ContainsKey("SmsPass") ? updatedValues["SmsPass"] : string.Empty,
                        SmsLink = updatedValues.ContainsKey("SmsLink") ? updatedValues["SmsLink"] : string.Empty,
                        MailAccount = updatedValues.ContainsKey("MailAccount") ? updatedValues["MailAccount"] : string.Empty,
                        MailPass = updatedValues.ContainsKey("MailPass") ? updatedValues["MailPass"] : string.Empty,
                        MailPort = updatedValues.ContainsKey("MailPort") ? updatedValues["MailPort"] : string.Empty,
                        MailHost = updatedValues.ContainsKey("MailHost") ? updatedValues["MailHost"] : string.Empty,
                        MailSSL = updatedValues.ContainsKey("MailSSL") ? (bool.TryParse(updatedValues["MailSSL"], out mailSSLValue) ? mailSSLValue : false) : false,
                        Introduce = updatedValues.ContainsKey("Introduce") ? updatedValues["Introduce"] : string.Empty,
                        Privacy = updatedValues.ContainsKey("Privacy") ? updatedValues["Privacy"] : string.Empty,
                        TermsOfUse = updatedValues.ContainsKey("TermsOfUse") ? updatedValues["TermsOfUse"] : string.Empty,
                        Support = updatedValues.ContainsKey("Support") ? updatedValues["Support"] : string.Empty,
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreateDate = DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    appConfig = unitOfWork.AppConfigRepository.Insert(appConfig);

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
