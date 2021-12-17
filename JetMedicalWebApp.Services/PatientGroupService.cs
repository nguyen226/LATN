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
    public class PatientGroupService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "PatientGroup";
        private string tenTable = "PatientGroup";

        public List<PatientGroupDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<PatientGroupDto> result = new List<PatientGroupDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.PatientGroupRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            PatientGroupDto obj = new PatientGroupDto();
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
                PatientGroupDto obj = new PatientGroupDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "CreatedDate":
                            
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
        public IEnumerable<PatientGroupDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public PatientGroupDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.PatientGroupRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public bool IsExitting(string name)
        {
            return unitOfWork.PatientGroupRepository.Get(m => m.Name.Trim().ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
        }

        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.PatientGroupRepository.Get(m => ids.Contains(m.Id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Id: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            unitOfWork.PatientGroupRepository.Delete(item);
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

        public string UpdateByIds(Dictionary<int, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.PatientGroupRepository.Get(m => ids.Contains(m.Id)).ToList();

                foreach (PatientGroup obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.Id])
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
                            GhiChu = "Id: " + obj.Id.ToString() + ", field: " + item.Key
                        });

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues, List<int> userIDs, List<int> companyIDs)
        {
            PatientGroup patientGroup = null;
            string resultMessage = string.Empty;
            int idValue = -1;
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
                        resultMessage = "Giá trị Id=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                    if (updatedValues.ContainsKey("Name"))
                    {
                        if (IsExitting(updatedValues["Name"]))
                        {
                            resultMessage = "Đã tồn tại nhóm người bệnh này.";
                        }
                        else
                        {
                            patientGroup = new PatientGroup()
                            {
                                Code = updatedValues.ContainsKey("Code") ? updatedValues["Code"] : string.Empty,
                                Note = updatedValues.ContainsKey("Note") ? updatedValues["Note"] : string.Empty,
                                Name = updatedValues["Name"],
                                ModifiedDate = DateTime.Now,
                                ModifiedUsers = nguoiSuDungCapNhat,
                                CreatedDate = DateTime.Now,
                                CreatedUsers = nguoiSuDungCapNhat
                            };

                            patientGroup = unitOfWork.PatientGroupRepository.Insert(patientGroup);

                            appHistoryServiceService.Add(new AppHistory()
                            {
                                Ma = maTable,
                                Ten = tenTable,
                                ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                            });

                            unitOfWork.Save();
                            // add userIDs vào patient group 
                            if (patientGroup != null)
                            {
                                if(userIDs != null)
                                {
                                    if(userIDs.Count() > 0)
                                    {
                                        foreach (var item in userIDs)
                                        {
                                            PatientGroup_Users itemGroup = new PatientGroup_Users();
                                            itemGroup.UsersID = item;
                                            itemGroup.PatientGroupID = patientGroup.Id;
                                            itemGroup.CreatedUsers = nguoiSuDungCapNhat;
                                            itemGroup.ModifiedUsers = nguoiSuDungCapNhat;
                                            itemGroup.CompanyId = -1;
                                            itemGroup.CreatedDate = DateTime.Now;
                                            itemGroup.ModifiedDate = DateTime.Now;
                                            unitOfWork.PatientGroup_UsersRepository.Insert(itemGroup);
                                        }
                                        unitOfWork.Save();
                                    }
                                }


                                if (companyIDs != null)
                                {
                                    if (companyIDs.Count() > 0)
                                    {
                                        foreach (var item in companyIDs)
                                        {
                                            PatientGroup_Users itemGroup = new PatientGroup_Users();
                                            itemGroup.UsersID = -1;
                                            itemGroup.PatientGroupID = patientGroup.Id;
                                            itemGroup.CreatedUsers = nguoiSuDungCapNhat;
                                            itemGroup.ModifiedUsers = nguoiSuDungCapNhat;
                                            itemGroup.CompanyId = item;
                                            itemGroup.CreatedDate = DateTime.Now;
                                            itemGroup.ModifiedDate = DateTime.Now;
                                            unitOfWork.PatientGroup_UsersRepository.Insert(itemGroup);
                                        }
                                        unitOfWork.Save();
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        resultMessage = "Không tìm thấy trường Name";
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
