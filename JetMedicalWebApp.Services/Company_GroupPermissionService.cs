using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Web;

namespace JetMedicalWebApp.Services
{
    public class Company_GroupPermissionService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Company_GroupPermission";
        private string tenTable = "Company_GroupPermission";

        public List<Company_GroupPermissionDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<Company_GroupPermissionDto> result = new List<Company_GroupPermissionDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.Company_GroupPermissionRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            Company_GroupPermissionDto obj = new Company_GroupPermissionDto();
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
                Company_GroupPermissionDto obj = new Company_GroupPermissionDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "Company_GroupPermission":
                        case "GroupPermission":
                            
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
        public IEnumerable<Company_GroupPermissionDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public Company_GroupPermissionDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            string stringFilter = "Id=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public List<Company_GroupPermissionDto> GetListByGroupPermissionIdExposeDto(string groupPermissionId, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, "GroupPermissionId asc");

            string stringFilter = "GroupPermissionId=" + groupPermissionId;
            filters.Add("StringFilter", stringFilter);
            return GetListExposeDto(filters, inData, out outData);
        }

        public bool IsExisting(int GroupPermissionId, int companyId)
        {
            return unitOfWork.Company_GroupPermissionRepository.CountDataRow(m =>m.GroupPermission.Id.Equals(GroupPermissionId)
                                                                                && m.Company.CompanyID.Equals(companyId)) > 0;
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.Company_GroupPermissionRepository.Get()
                                                    .Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public string DeleteById(int id)
        {
            string result = string.Empty;

            AppHistoryService lichSuHeThongService = new AppHistoryService();

            try
            {
                var query = unitOfWork.Company_GroupPermissionRepository.GetByID(id);

                if (query != null)
                {
                    lichSuHeThongService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                        GhiChu = "Xóa Company _ phân quyền: " + id
                    });
                    unitOfWork.Company_GroupPermissionRepository.Delete(query);
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }


        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;

            if (ids.Count() > 0)
            {
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.Company_GroupPermissionRepository.Get(m => ids.Contains(m.Id)).ToList();

                    if (query != null)
                    {
                        lichSuHeThongService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Id: " + string.Join(";", ids)
                        });

                        unitOfWork.Company_GroupPermissionRepository.DeleteRange(query);

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
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                int intValue;

                var query = unitOfWork.Company_GroupPermissionRepository.Get(m =>ids.Contains(m.Id)).ToList();

                foreach (Company_GroupPermission obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.Id])
                    {
                        switch (item.Key)
                        {
                            case "GroupPermissionId":
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    obj.GroupPermission = unitOfWork.GroupPermissionRepository.GetByID(intValue);
                                }
                                break;

                            case "CompanyId":
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    obj.Company = unitOfWork.CompanyRepository.GetByID(intValue);
                                }
                                break;

                            default:

                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);

                                break;
                        }

                        lichSuHeThongService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "Id: " + obj.Id.ToString() + ", field: " + item.Key
                        });
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(int phanQuyenId, int companyId)
        {
            string result = string.Empty;
            try
            {
                InternalService internalService = new InternalService();
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                Company_GroupPermission groupPermission_Company = unitOfWork.Company_GroupPermissionRepository.Get(m => m.GroupPermission.Id == phanQuyenId
                                                                            && m.Company.CompanyID == companyId).FirstOrDefault();
                if (groupPermission_Company == null)
                {
                    GroupPermission GroupPermission = unitOfWork.GroupPermissionRepository.GetByID(phanQuyenId);
                    Company company = unitOfWork.CompanyRepository.GetByID(companyId);

                    groupPermission_Company = unitOfWork.Company_GroupPermissionRepository.Insert(new Company_GroupPermission()
                    {
                        Company = company,
                        GroupPermission = GroupPermission,
                    });

                    lichSuHeThongService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                        GhiChu = "Company_GroupPermission: " + GroupPermission.Name + ", Company: " + company.ComName,
                    });
                }
                
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
