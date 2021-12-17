using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.IO;
using System.Globalization;

namespace JetMedicalWebApp.Services
{
    public class NotificationService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Notification";
        private string tenTable = "Notification";

        public List<NotificationDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<NotificationDto> result = new List<NotificationDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.NotificationRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            NotificationDto obj = new NotificationDto();
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
                NotificationDto obj = new NotificationDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "CreateDate":
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
        public IEnumerable<NotificationDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public NotificationDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "NoID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.NotificationRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.NotificationRepository.Get(m => ids.Contains(m.NoID)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "NoID: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            if (!string.IsNullOrEmpty(item.Image))
                            {
                                imageNames.Add(item.Image);
                            }

                            unitOfWork.NotificationRepository.Delete(item);
                        }

                        unitOfWork.Save();
                    }

                    if (imageNames.Count() > 0)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanUploadNotifications);
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

                var query = unitOfWork.NotificationRepository.Get(m => ids.Contains(m.NoID)).ToList();

                foreach (Notification obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.NoID])
                    {
                        switch (item.Key)
                        {
                            case "Image":
                                if (!string.IsNullOrEmpty(obj.Image) && obj.Image != item.Value)
                                {
                                    var array = obj.Image.Split('/');
                                    string fileName = array[obj.Image.Split('/').Length - 1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanUploadNotifications);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }

                                obj.Image = item.Value;

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
                            GhiChu = "NoID: " + obj.NoID.ToString() + ", field: " + item.Key
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
            Notification notification = null;
            string resultMessage = string.Empty;
            int idValue = -1, noCategoryValue, noTypeValue;
            bool noStatusValue;
            DateTime createdDateValue;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
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
                        resultMessage = "Giá trị NoID=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                    notification = new Notification()
                    {
                        NoCategory = updatedValues.ContainsKey("NoCategory") ? Int32.TryParse(updatedValues["NoCategory"], out noCategoryValue) ? noCategoryValue : 0 : 0,
                        NoTitle = updatedValues.ContainsKey("NoTitle") ? updatedValues["NoTitle"] : string.Empty,
                        NoDes = updatedValues.ContainsKey("NoDes") ? updatedValues["NoDes"] : string.Empty,
                        NoType = updatedValues.ContainsKey("NoType") ? Int32.TryParse(updatedValues["NoType"], out noTypeValue) ? noTypeValue : 0 : 0,
                        NewID = updatedValues.ContainsKey("NewID") ? updatedValues["NewID"] : string.Empty,
                        NoHTML = updatedValues.ContainsKey("NoHTML") ? updatedValues["NoHTML"] : string.Empty,
                        Image = updatedValues.ContainsKey("Image") ? updatedValues["Image"] : string.Empty,
                        NoStatus = updatedValues.ContainsKey("NoStatus") ? bool.TryParse(updatedValues["NoStatus"], out noStatusValue) ? noStatusValue : false : false,
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreateDate = updatedValues.ContainsKey("CreateDate") ? (System.DateTime.TryParseExact(updatedValues["CreateDate"], Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out createdDateValue) ? createdDateValue : DateTime.Now) : DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    notification = unitOfWork.NotificationRepository.Insert(notification);

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
