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
    public class Users_GroupPermissionService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Users_GroupPermission";
        private string tenTable = "Users_GroupPermission";

        public List<Users_GroupPermissionDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<Users_GroupPermissionDto> result = new List<Users_GroupPermissionDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.Users_GroupPermissionRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            Users_GroupPermissionDto obj = new Users_GroupPermissionDto();
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
                Users_GroupPermissionDto obj = new Users_GroupPermissionDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "Active":
                        case "UserActive":
                        case "Users_GroupPermission":
                        case "GroupPermissions":
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
        public IEnumerable<Users_GroupPermissionDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public Users_GroupPermissionDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            string stringFilter = "Id=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public List<Users_GroupPermissionDto> GetListByGroupUserIdExposeDto(string usersId, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, "UserID asc");

            string stringFilter = "UserID=" + usersId;
            filters.Add("StringFilter", stringFilter);
            return GetListExposeDto(filters, inData, out outData);
        }

        public bool IsExisting(int groupPermissionId, int usersId)
        {
            return unitOfWork.Users_GroupPermissionRepository.CountDataRow(m =>m.GroupPermission.Id.Equals(groupPermissionId)
                                                                                && m.Users.UserID.Equals(usersId)) > 0;
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.Users_GroupPermissionRepository.Get()
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
                var query = unitOfWork.Users_GroupPermissionRepository.GetByID(id);

                if (query != null)
                {
                    lichSuHeThongService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                        GhiChu = "Xóa user _ phân quyền: " + id
                    });
                    unitOfWork.Users_GroupPermissionRepository.Delete(query);
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
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

                var query = unitOfWork.Users_GroupPermissionRepository.Get(m =>ids.Contains(m.Id)).ToList();

                foreach (Users_GroupPermission obj in query)
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

                            case "UsersId":
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    obj.Users = unitOfWork.UsersRepository.GetByID(intValue);
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

        public string AddOrUpdate(int phanQuyenId, int usersId)
        {
            string result = string.Empty;
            try
            {
                InternalService internalService = new InternalService();
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                Users_GroupPermission groupPermission_Users = new Users_GroupPermission();
                
                if (!IsExisting(phanQuyenId, usersId))
                {

                    GroupPermission GroupPermission = unitOfWork.GroupPermissionRepository.GetByID(phanQuyenId);
                    Users users = unitOfWork.UsersRepository.GetByID(usersId);

                    groupPermission_Users = unitOfWork.Users_GroupPermissionRepository.Insert(new Users_GroupPermission()
                    {
                        Users = users,
                        GroupPermission = GroupPermission,
                    });

                    lichSuHeThongService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                        GhiChu = "Users_GroupPermission: " + GroupPermission.Name + ", Users: " + users.UserID,
                    });
                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
