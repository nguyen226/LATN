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
    public class NhanTuVanService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "NhanTuVan";
        private string tenTable = "NhanTuVan";

        public List<NhanTuVanDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<NhanTuVanDto> result = new List<NhanTuVanDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.NhanTuVanRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            NhanTuVanDto obj = new NhanTuVanDto();
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
                NhanTuVanDto obj = new NhanTuVanDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "NhanTuVan_DonVis":
                            break;

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
        public IEnumerable<NhanTuVanDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public NhanTuVanDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "NhanTuVanID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        
        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.NhanTuVanRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.NhanTuVanRepository.Get(m => ids.Contains(m.Id)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "NhanTuVanID: " + string.Join(";", ids)
                        });
                      
                        foreach(var item in query)
                        {
                            unitOfWork.NhanTuVanRepository.Delete(item);
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

                var query = unitOfWork.NhanTuVanRepository.Get(m => ids.Contains(m.Id)).ToList();

                foreach (NhanTuVan obj in query)
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
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            NhanTuVan nhanTuVan = null;
            string resultMessage = string.Empty;
            int idValue = -1, memberIdValue = -1;
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

                    nhanTuVan = new NhanTuVan()
                    {
                        TieuDe = updatedValues.ContainsKey("TieuDe") ? updatedValues["TieuDe"] : string.Empty,
                        NoiDung = updatedValues.ContainsKey("NoiDung") ? updatedValues["NoiDung"] : string.Empty,
                        HoVaTen = updatedValues.ContainsKey("HoVaTen") ? updatedValues["HoVaTen"] : string.Empty,
                        SoDienThoai = updatedValues.ContainsKey("SoDienThoai") ? updatedValues["SoDienThoai"] : string.Empty,
                        Email = updatedValues.ContainsKey("Email") ? updatedValues["Email"] : string.Empty,
                        TrangThai = 1,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreatedUserID = updatedValues.ContainsKey("CreatedUserID") ? (Int32.TryParse(updatedValues["CreatedUserID"], out memberIdValue) ? memberIdValue : -1) : -1,
                    };

                    nhanTuVan = unitOfWork.NhanTuVanRepository.Insert(nhanTuVan);

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
