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
    public class Menu_GroupPermissionService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "Menu_GroupPermission";
        private string tenTable = "Menu_GroupPermission";

        public List<Menu_GroupPermissionDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<Menu_GroupPermissionDto> result = new List<Menu_GroupPermissionDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.Menu_GroupPermissionRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            Menu_GroupPermissionDto obj = new Menu_GroupPermissionDto();
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
                Menu_GroupPermissionDto obj = new Menu_GroupPermissionDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "Menu_GroupPermission":
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
        public IEnumerable<Menu_GroupPermissionDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public Menu_GroupPermissionDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            string stringFilter = "Id=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public List<Menu_GroupPermissionDto> GetListByGroupPermissionIdExposeDto(string groupPermissionId, string selectedFields)
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

        public bool IsExisting(int GroupPermissionId, int menuId)
        {
            return unitOfWork.Menu_GroupPermissionRepository.CountDataRow(m =>m.GroupPermission.Id.Equals(GroupPermissionId)
                                                                                && m.Menu.Id.Equals(menuId)) > 0;
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.Menu_GroupPermissionRepository.Get()
                                                    .Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }
        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;

            if (ids.Count() > 0)
            {
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.Menu_GroupPermissionRepository.Get(m => ids.Contains(m.Id)).ToList();

                    if (query != null)
                    {
                        lichSuHeThongService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Id: " + string.Join(";", ids)
                        });

                        unitOfWork.Menu_GroupPermissionRepository.DeleteRange(query);

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

                var query = unitOfWork.Menu_GroupPermissionRepository.Get(m =>ids.Contains(m.Id)).ToList();

                foreach (Menu_GroupPermission obj in query)
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

                            case "MenuId":
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    obj.Menu = unitOfWork.MenuRepository.GetByID(intValue);
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

        public string AddOrUpdate(int phanQuyenId, int menuId, Dictionary<string, bool> updatedValues)
        {
            string result = string.Empty;
            try
            {
                InternalService internalService = new InternalService();
                AppHistoryService lichSuHeThongService = new AppHistoryService();

                Menu_GroupPermission groupPermission_Menu = unitOfWork.Menu_GroupPermissionRepository.Get(m => m.GroupPermission.Id == phanQuyenId
                                                                            && m.Menu.Id == menuId).FirstOrDefault();
                if (groupPermission_Menu == null)
                {

                    GroupPermission GroupPermission = unitOfWork.GroupPermissionRepository.GetByID(phanQuyenId);
                    Menu menu = unitOfWork.MenuRepository.GetByID(menuId);

                    groupPermission_Menu = unitOfWork.Menu_GroupPermissionRepository.Insert(new Menu_GroupPermission()
                    {
                        Menu = menu,
                        GroupPermission = GroupPermission,
                    });

                    lichSuHeThongService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                        GhiChu = "Menu_GroupPermission: " + GroupPermission.Name + ", Menu: " + menu.Name,
                    });
                }
                

                if (updatedValues.ContainsKey("SystemView"))
                {
                    groupPermission_Menu.SystemView = updatedValues["SystemView"];
                }

                if (updatedValues.ContainsKey("PersonalView"))
                {
                    groupPermission_Menu.PersonalView = updatedValues["PersonalView"];
                }

                if (updatedValues.ContainsKey("SystemEdit"))
                {
                    groupPermission_Menu.SystemEdit = updatedValues["SystemEdit"];
                }

                if (updatedValues.ContainsKey("PersonalEdit"))
                {
                    groupPermission_Menu.PersonalEdit = updatedValues["PersonalEdit"];
                }

                if (updatedValues.ContainsKey("PersonalDelete"))
                {
                    groupPermission_Menu.PersonalDelete = updatedValues["PersonalDelete"];
                }

                if (updatedValues.ContainsKey("SystemDelete"))
                {
                    groupPermission_Menu.SystemDelete = updatedValues["SystemDelete"];
                }

                unitOfWork.Save();

                string maMenuCapTren = groupPermission_Menu.Menu.Code;

                foreach (Menu subMenu in unitOfWork.MenuRepository.Get(m => m.MaMenuCapTren.Trim().ToLower().Equals(maMenuCapTren.Trim().ToLower())).ToList())
                {
                    AddOrUpdate(phanQuyenId, subMenu.Id, updatedValues);
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
