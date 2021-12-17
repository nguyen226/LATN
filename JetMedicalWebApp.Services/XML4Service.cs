using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Data;

namespace JetMedicalWebApp.Services
{
    public class XML4Service
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "XML4";
        private string tenTable = "XML4";

        public List<XML4Dto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<XML4Dto> result = new List<XML4Dto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.XML4Repository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            XML4Dto obj = new XML4Dto();
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
                XML4Dto obj = new XML4Dto();

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

        public List<XML4Dto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<XML4Dto> result = new List<XML4Dto>();
            string tableName = "uv_XML4";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData.ContainsKey(CommonConstants.StrSelectedFields) && inData.ContainsKey(CommonConstants.StrSelectedFields) ? inData[CommonConstants.StrSelectedFields] : "*";
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = inData.ContainsKey(CommonConstants.StrSortedColumnNames) && !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

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
                            XML4Dto obj = new XML4Dto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<XML4Dto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                XML4Dto obj = new XML4Dto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            break;

                        case "NGAY_VAO":
                            obj.GetType().GetProperty("STRNGAY_VAO").SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
                            break;

                        case "NGAY_RA":
                            obj.GetType().GetProperty("STRNGAY_RA").SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
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

        public IEnumerable<XML4Dto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public XML4Dto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.XML4Repository.Get().Select("new (" + selectedFields + ")")
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
                    string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanThuMucTapTin);
                    List<string> fileNames = new List<string>();

                    var query = unitOfWork.XML4Repository.Get(m => ids.Contains(m.XML4ID)).ToList();
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "Id: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            if (!string.IsNullOrEmpty(item.XML4_01))
                            {
                                fileNames.Add(item.XML4_01);
                            }

                            unitOfWork.XML4Repository.Delete(item);
                            unitOfWork.Save();
                        }
                    }

                    if (fileNames.Count() > 0)
                    {
                        foreach (string fileName in fileNames)
                        {
                            var array = fileName.Split('/');
                            string fileName1 = array[fileName.Split('/').Length - 1];

                            string tempPath = Path.Combine(path, fileName1);
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
            string messageResult = string.Empty;

            if (updatedData != null)
            {
                List<int> ids = new List<int>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                var query = unitOfWork.XML4Repository.Get(m => ids.Contains(m.XML4ID)).ToList();

                foreach (XML4 obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.XML4ID])
                    {
                        switch (item.Key)
                        {
                            case "XML4_01":
                                if (!string.IsNullOrEmpty(obj.XML4_01) && obj.XML4_01 != item.Value)
                                {
                                    var array = obj.XML4_01.Split('/');
                                    string fileName = array[obj.XML4_01.Split('/').Length - 1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(Common.CommonConstants.DuongDanThuMucTapTin);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }
                                obj.XML4_01 = item.Value;
                                break;

                            case "STT":
                                break;

                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }

                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "Id: " + obj.XML4ID + ", field: " + item.Key
                        });

                        obj.ModifiedDate = DateTime.Now;
                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            XML4 xML4 = null;
            string resultMessage = string.Empty;
            int idValue;
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
                        resultMessage = "Giá trị ID=" + id + " không phù hợp.";
                    }
                    
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                    int sTTValue;
                    DateTime ngayKQValue;
                    xML4 = new XML4()
                    {
                        STT = updatedValues.ContainsKey("STT") ? Int32.TryParse(updatedValues["STT"], out sTTValue) ? sTTValue : 0 : 0,
                        MA_LK = updatedValues.ContainsKey("MA_LK") ? updatedValues["MA_LK"] : string.Empty,
                        TEN_CHI_SO = updatedValues.ContainsKey("TEN_CHI_SO") ? updatedValues["TEN_CHI_SO"] : string.Empty,
                        GIA_TRI = updatedValues.ContainsKey("GIA_TRI") ? updatedValues["GIA_TRI"] : string.Empty,
                        MO_TA = updatedValues.ContainsKey("MO_TA") ? updatedValues["MO_TA"] : string.Empty,
                        KET_LUAN = updatedValues.ContainsKey("KET_LUAN") ? updatedValues["KET_LUAN"] : string.Empty,
                        XML4_01 = updatedValues.ContainsKey("XML4_01") ? updatedValues["XML4_01"] : string.Empty,
                        NGAY_KQ = updatedValues.ContainsKey("NGAY_KQ") ? (DateTime.TryParseExact(updatedValues["NGAY_KQ"], CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayKQValue) ? ngayKQValue : DateTime.Now) : DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    xML4 = unitOfWork.XML4Repository.Insert(xML4);

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

        public string NhapDuLieuTuFileExcel(DataTable dt)
        {
            string resultMessage = string.Empty;
            int rowIndex = 0;
            int tongSoDong = dt.Rows.Count;

            List<Task> tasks = null;
            if (dt.Columns.Count > 6)
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
                        new Task(async () =>  await Import1(rowIndex, (tongSoDong / 3), dt, userID)),
                        new Task(async () =>  await Import2(((tongSoDong / 3) + 1), ((tongSoDong / 3) * 2), dt, userID)),
                        new Task(async () =>  await Import3((((tongSoDong / 3) * 2) + 1), tongSoDong, dt, userID)),
                    };
                }
                else
                {
                    tasks = new List<Task>
                    {
                        new Task(async () =>  await Import1(rowIndex, tongSoDong, dt, userID)),
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
        public async Task Import1(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public async Task Import2(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public async Task Import3(int tuDong, int toiDong, DataTable dt, int userID)
        {
            ThucHienImport(tuDong, toiDong, dt, userID);
        }
        public void ThucHienImport(int tuDong, int toiDong, DataTable dt, int userID)
        {
            UnitOfWork unit3 = new UnitOfWork();

            XML4 xML4 = null;
            DateTime ngayKQValue;

            Users nguoiSuDungCapNhat = unit3.UsersRepository.Get(m => m.UserID.Equals(userID)).FirstOrDefault();

            for (int n = tuDong; n < toiDong; n++)
            {
                xML4 = null;

                string maLK = dt.Rows[n][0].ToString();
                string maDichVu = dt.Rows[n][1].ToString();
                string maMay = dt.Rows[n][2].ToString();
                string tenChiSo = dt.Rows[n][3].ToString();
                string giaTri = dt.Rows[n][4].ToString();
                string moTa = dt.Rows[n][5].ToString();
                string ketLuan = dt.Rows[n][6].ToString();
                string ngayKQ = dt.Rows[n][7].ToString();
                string xml401 = dt.Rows[n][8].ToString();

                if (!string.IsNullOrEmpty(ngayKQ))
                {
                    int start = ngayKQ.IndexOf(":");
                    int end = ngayKQ.LastIndexOf(":");

                    if (start < end)
                    {
                        ngayKQ = ngayKQ.Substring(0, end);
                    }
                }

                if (unit3.XML4Repository.Get().FirstOrDefault(m => m.MA_LK.Trim().ToLower().Equals(maLK.Trim().ToLower())
                                                     && m.MA_DICH_VU.Trim().ToLower().Equals(maDichVu.Trim().ToLower())
                                                     && m.TEN_CHI_SO.Trim().ToLower().Equals(tenChiSo.Trim().ToLower())
                                                     ) == null)
                {
                    xML4 = new XML4()
                    {
                        STT = 0,
                        MA_LK = maLK,
                        MA_DICH_VU = maDichVu,
                        TEN_CHI_SO = tenChiSo,
                        GIA_TRI = giaTri,
                        MO_TA = moTa,
                        KET_LUAN = ketLuan,
                        XML4_01 = xml401,
                        NGAY_KQ = (DateTime.TryParseExact(ngayKQ, ngayKQ.IndexOf(":") > 0 ? CommonConstants.DateTimeFormat : CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayKQValue) ? ngayKQValue : DateTime.Now),
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    xML4 = unit3.XML4Repository.Insert(xML4);

                    unit3.Save();
                }
                else
                {
                    unit3.AppHistoryRepository.Insert(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = CommonConstants.ThaoTacImport,
                        GhiChu = "Mã dich vụ '" + maDichVu + "' và tên chỉ số '" + tenChiSo + "' này đã có trong dữ liệu",
                        CreatedUsers = nguoiSuDungCapNhat,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    });
                    unit3.Save();
                }
            }
        }
    }
}
