using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Web;
using System.Globalization;
using System.IO;
using System.Data;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Services
{
    public class KSK_CDHAService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private string maTable = "KSK_CDHA";
        private string tenTable = "KSK_CDHA";

        public List<KSK_CDHADto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<KSK_CDHADto> result = new List<KSK_CDHADto>();
            string stringFilter = string.Empty, usernameNguoiCapNhat = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.KSK_CDHARepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            KSK_CDHADto obj = new KSK_CDHADto();
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
                KSK_CDHADto obj = new KSK_CDHADto();

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
        public IEnumerable<KSK_CDHADto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public KSK_CDHADto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "UserID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        public KSK_CDHADto GetByUsernameExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Username=\"" + username + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public KSK_CDHADto GetByEmailOrPhoneExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "EmailID=(\"" + username + "\")  OR Phone =(\"" + username + "\")";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public KSK_CDHADto GetByUserIdExposeDto(string userId, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "UserId=\"" + userId + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            if (!string.IsNullOrEmpty(stringFilter))
            {
                return unitOfWork.KSK_CDHARepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
            }
            return unitOfWork.KSK_CDHARepository.Get().Select("new (" + selectedFields + ")").Count();
        }
        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;

            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryService = new AppHistoryService();

                try
                {
                    List<string> imageNames = new List<string>();
                    var query = unitOfWork.KSK_CDHARepository.Get(m => ids.Contains(m.KSK_CDHA_ID)).ToList();
                    if (query != null)
                    {
                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "TTBN: " + string.Join(";", ids)
                        });

                        unitOfWork.KSK_CDHARepository.DeleteRange(query);
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
                var nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                AppHistoryService appHistoryService = new AppHistoryService();
                var query = unitOfWork.KSK_CDHARepository.Get(m => ids.Contains(m.KSK_CDHA_ID)).ToList();
                foreach (KSK_CDHA obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.KSK_CDHA_ID])
                    {
                        switch (item.Key)
                        {
                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }
                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "UserID: " + obj.KSK_CDHA_ID.ToString() + ", field: " + item.Key
                        });
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            KSK_CDHA KSK_CDHA = null;
            string resultMessage = string.Empty;
            int idValue = -1;
            Dictionary<int, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryService = new AppHistoryService();
            InternalService internalService = new InternalService();
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
                        resultMessage = "Giá trị TTBN=" + id + " không phù hợp.";
                    }
                }
                else
                {
                    KSK_CDHA = new KSK_CDHA()
                    {
                        MA_LK = updatedValues.ContainsKey("MA_LK") ? updatedValues["MA_LK"] : string.Empty,
                        TEN_CDHA = updatedValues.ContainsKey("TEN_CDHA") ? updatedValues["TEN_CDHA"] : string.Empty,
                        KET_QUA = updatedValues.ContainsKey("KET_QUA") ? updatedValues["KET_QUA"] : string.Empty,
                    };

                    KSK_CDHA = unitOfWork.KSK_CDHARepository.Insert(KSK_CDHA);

                    unitOfWork.Save();

                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }


        public string NhapDuLieuTuFileExcel(DataTable dt)
        {
            string resultMessage = string.Empty;
            int rowIndex = 0;
            int tongSoDong = dt.Rows.Count;

            List<Task> tasks = null;
            if (dt.Columns.Count > 0)
            {
                int userID = -1;
                var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
                if (authCookie != null)
                {
                    userID = Int32.Parse(authCookie.Value);
                }

                if (dt.Rows.Count > 100)
                {
                    tasks = new List<Task>
                    {
                        new Task(async () =>  await Import1(rowIndex, (tongSoDong / 3), dt, dt.Columns.Count)),
                        new Task(async () =>  await Import2(((tongSoDong / 3) + 1), ((tongSoDong / 3) * 2), dt, dt.Columns.Count)),
                        new Task(async () =>  await Import3((((tongSoDong / 3) * 2) + 1), tongSoDong, dt, dt.Columns.Count)),
                    };
                }
                else
                {
                    tasks = new List<Task>
                    {
                        new Task(async () =>  await Import1(rowIndex, tongSoDong, dt, dt.Columns.Count)),
                    };
                }

                Parallel.ForEach(tasks, task =>
                {
                    task.Start();
                });

                Task.WhenAll(tasks).ContinueWith(done =>
                {
                    //Run the other tasks
                });
            }
            else
            {
                resultMessage = "File excel không đúng định dạng";
            }

            return resultMessage;
        }
        public async Task Import1(int tuDong, int toiDong, DataTable dt, int totalColumn)
        {
            ThucHienImport(tuDong, toiDong, dt, totalColumn);
        }
        public async Task Import2(int tuDong, int toiDong, DataTable dt, int totalColumn)
        {
            ThucHienImport(tuDong, toiDong, dt, totalColumn);
        }
        public async Task Import3(int tuDong, int toiDong, DataTable dt, int totalColumn)
        {
            ThucHienImport(tuDong, toiDong, dt, totalColumn);
        }
        public void ThucHienImport(int tuDong, int toiDong, DataTable dt, int totalColumn)
        {
            UnitOfWork unit3 = new UnitOfWork();

            for (int n = tuDong; n < toiDong; n++)
            {
                for(int c = 1; c < totalColumn; c++)
                {
                    string MA_LK = dt.Rows[n][0].ToString();
                    string TEN_CDHA = dt.Columns[c].ToString();
                    string KET_QUA = dt.Rows[n][c].ToString();

                    unit3.KSK_CDHARepository.Insert(new KSK_CDHA()
                    {
                        MA_LK = MA_LK,
                        TEN_CDHA = TEN_CDHA,
                        KET_QUA = KET_QUA
                    });


                    unit3.Save();
                }
            }
        }



        public List<KSK_CDHADto> GetListTTBNDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                out Dictionary<string, string> outData)
        {
            List<KSK_CDHADto> result = new List<KSK_CDHADto>();
            string tableName = "uv_KhamSucKhoeCDHA";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            UsersDto obj = new UsersDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<KSK_CDHADto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                KSK_CDHADto obj = new KSK_CDHADto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }

    }
}
