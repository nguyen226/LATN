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
    public class SignedAppointmentService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "SignedAppointment";
        private string tenTable = "SignedAppointment";

        public List<SignedAppointmentDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<SignedAppointmentDto> result = new List<SignedAppointmentDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.SignedAppointmentRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            SignedAppointmentDto obj = new SignedAppointmentDto();
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
                SignedAppointmentDto obj = new SignedAppointmentDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {

                        case "SignedAppointment_GroupPermissions":
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
        public IEnumerable<SignedAppointmentDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public SignedAppointmentDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.SignedAppointmentRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.SignedAppointmentRepository.Get(m => ids.Contains(m.id)).ToList();
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
                            unitOfWork.SignedAppointmentRepository.Delete(item);
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

                var query = unitOfWork.SignedAppointmentRepository.Get(m => ids.Contains(m.id)).ToList();
                bool checkUpdate = true;

                if (checkUpdate)
                {
                    foreach (SignedAppointment obj in query)
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
            SignedAppointment signedAppointment = null;
            string resultMessage = string.Empty;
            Dictionary<Int32, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            int idValue, departmentIdValue, doctorIdValue;
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
                    signedAppointment = new SignedAppointment()
                    {
                        fullname = updatedValues.ContainsKey("fullname") ? updatedValues["fullname"] : string.Empty,
                        mobile = updatedValues.ContainsKey("mobile") ? updatedValues["mobile"] : string.Empty,
                        email = updatedValues.ContainsKey("email") ? updatedValues["email"] : string.Empty,
                        date = updatedValues.ContainsKey("date") ? updatedValues["date"] : DateTime.Now.ToString(),
                        departmentId = updatedValues.ContainsKey("departmentId") ? Int32.TryParse(updatedValues["departmentId"], out departmentIdValue) ? departmentIdValue : -1 : -1,
                        doctorId = updatedValues.ContainsKey("doctorId") ? Int32.TryParse(updatedValues["doctorId"], out doctorIdValue) ? doctorIdValue : -1 : -1,
                        created_at = updatedValues.ContainsKey("created_at") ? updatedValues["created_at"] : DateTime.Now.ToString(),
                    };

                    signedAppointment = unitOfWork.SignedAppointmentRepository.Insert(signedAppointment);

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
