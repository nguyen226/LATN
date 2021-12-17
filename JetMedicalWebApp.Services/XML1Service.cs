using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Globalization;
using System.Data;
using System.Threading.Tasks;
using System.IO;

namespace JetMedicalWebApp.Services
{
    public class XML1Service
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "XML1";
        private string tenTable = "XML1";

        public List<XML1Dto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<XML1Dto> result = new List<XML1Dto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.XML1Repository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            XML1Dto obj = new XML1Dto();
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
                XML1Dto obj = new XML1Dto();

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

        public List<XML1Dto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<XML1Dto> result = new List<XML1Dto>();
            string tableName = "uv_XML1";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData.ContainsKey(CommonConstants.StrSelectedFields) && inData.ContainsKey(CommonConstants.StrSelectedFields) ? inData[CommonConstants.StrSelectedFields] : "*";
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = inData.ContainsKey(CommonConstants.StrSortedColumnNames) && !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "MA_LK ASC";

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
                            XML1Dto obj = new XML1Dto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<XML1Dto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                XML1Dto obj = new XML1Dto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy"), null);
                            break;

                        case "NGAY_VAO":
                            obj.GetType().GetProperty("STRNGAY_VAO").SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy HH:mm") : string.Empty, null);
                            break;

                        case "NGAY_RA":
                            obj.GetType().GetProperty("STRNGAY_RA").SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy HH:mm") : string.Empty, null);
                            break;

                        case "NGAY_TAI_KHAM":
                            obj.GetType().GetProperty("STRNGAY_TAI_KHAM").SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy HH:mm") : string.Empty, null);
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

        public IEnumerable<XML1Dto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public XML1Dto GetByMA_LKExposeDto(string malk, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "MA_LK.Equals(\"" + malk + "\")";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public int CountDataRow(string selectedFields, string stringFilter)
        {
            return unitOfWork.XML1Repository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
        }

        public bool IsExitting(string ma_LK)
        {
            return unitOfWork.XML1Repository.Get(m => m.MA_LK.Trim().ToLower().Equals(ma_LK.Trim().ToLower())).FirstOrDefault() != null;
        }

        public string DeleteByMA_LKs(List<string> ids)
        {
            string result = string.Empty;
            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryServiceService = new AppHistoryService();

                try
                {
                    var query = unitOfWork.XML1Repository.Get(m => ids.Contains(m.MA_LK)).ToList();
                    List<string> fileNames = new List<string>();
                    string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanThuMucTapTin);
                    if (query != null)
                    {
                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacXoaDuLieu,
                            GhiChu = "MA_LK: " + string.Join(";", ids)
                        });

                        foreach (var item in query)
                        {
                            var xml2s = unitOfWork.XML2Repository.Get(m => m.MA_LK == item.MA_LK);
                            if (xml2s.Count() > 0)
                            {
                                foreach (var xml2 in xml2s)
                                {
                                    if (!string.IsNullOrEmpty(xml2.XML2_01))
                                    {
                                        fileNames.Add(xml2.XML2_01);
                                    }
                                }
                                unitOfWork.XML2Repository.DeleteRange(xml2s);
                            }

                            var xml3s = unitOfWork.XML3Repository.Get(m => m.MA_LK == item.MA_LK);
                            if (xml3s.Count() > 0)
                            {
                                foreach (var xml3 in xml3s)
                                {
                                    if (!string.IsNullOrEmpty(xml3.XML3_01))
                                    {
                                        fileNames.Add(xml3.XML3_01);
                                    }
                                }
                                unitOfWork.XML3Repository.DeleteRange(xml3s);
                            }

                            var xml4s = unitOfWork.XML4Repository.Get(m => m.MA_LK == item.MA_LK);
                            if (xml4s.Count() > 0)
                            {
                                foreach (var xml4 in xml4s)
                                {
                                    if (!string.IsNullOrEmpty(xml4.XML4_01))
                                    {
                                        fileNames.Add(xml4.XML4_01);
                                    }
                                }
                                unitOfWork.XML4Repository.DeleteRange(xml4s);
                            }

                            if (!string.IsNullOrEmpty(item.XML1_File))
                            {
                                fileNames.Add(item.XML1_File);
                            }

                            unitOfWork.XML1Repository.Delete(item);
                            unitOfWork.Save();
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
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }

            return result;
        }

        public string UpdateByMA_LKs(Dictionary<string, Dictionary<string, string>> updatedData)
        {
            string messageResult = String.Empty;

            if (updatedData != null)
            {
                List<string> ids = new List<string>(updatedData.Keys);
                InternalService internalService = new InternalService();
                AppHistoryService appHistoryServiceService = new AppHistoryService();
                Users nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                DateTime dateTimeValue;

                var query = unitOfWork.XML1Repository.Get(m => ids.Contains(m.MA_LK)).ToList();

                foreach (XML1 obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.MA_LK])
                    {
                        switch (item.Key)
                        {
                            case "MA_LK":
                                break;

                            case "NGAY_VAO":
                                if (!DateTime.TryParseExact(item.Value, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out dateTimeValue))
                                {
                                    dateTimeValue = DateTime.Now;
                                }
                                obj.GetType().GetProperty(item.Key).SetValue(obj, dateTimeValue, null);
                                break;

                            case "NGAY_RA":
                            case "NGAY_TAI_KHAM":
                                obj.GetType().GetProperty(item.Key).SetValue(obj, DateTime.TryParseExact(item.Value, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out dateTimeValue) ? dateTimeValue : new Nullable<DateTime>(), null);
                                break;

                            case "XML1_File":
                                if (!string.IsNullOrEmpty(obj.XML1_File) && obj.XML1_File != item.Value)
                                {
                                    var array = obj.XML1_File.Split('/');
                                    string fileName = array[obj.XML1_File.Split('/').Length - 1];

                                    string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanThuMucTapTin);
                                    path = Path.Combine(path, fileName);
                                    if (File.Exists(path))
                                    {
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        File.Delete(path);
                                    }
                                }
                                obj.XML1_File = item.Value;
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
                            GhiChu = "MA_LK: " + obj.MA_LK + ", field: " + item.Key
                        });

                        obj.ModifiedDate = System.DateTime.Now;
                        obj.ModifiedUsers = nguoiSuDungCapNhat;
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string ma_lk, Dictionary<string, string> updatedValues, out bool isError)
        {
            isError = false;
            XML1 xML1 = null;
            string resultMessage = string.Empty;
            int bENHIDValue;
            Dictionary<string, Dictionary<string, string>> updatedData;
            AppHistoryService appHistoryServiceService = new AppHistoryService();
            InternalService internalService = new InternalService();
            Users nguoiSuDungCapNhat;
            try
            {
                if (!string.IsNullOrEmpty(ma_lk))
                {
                    updatedData = new Dictionary<string, Dictionary<string, string>>();
                    updatedData.Add(ma_lk, updatedValues);
                    resultMessage = UpdateByMA_LKs(updatedData);

                    if (string.IsNullOrEmpty(resultMessage))
                    {
                        resultMessage = ma_lk;
                    }
                    else
                    {
                        isError = true;
                    }
                }
                else
                {
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);
                    decimal ketQuaDieuTriValue, tinhTrangRaVienValue;
                    DateTime ngayVaoValue, ngayRaValue, ngayTaiKhamValue;

                    if (updatedValues.ContainsKey("NGAY_VAO"))
                    {
                        if (!DateTime.TryParseExact(updatedValues["NGAY_VAO"], CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayVaoValue))
                        {
                            ngayVaoValue = DateTime.Now;
                        }
                    }
                    else
                    {
                        ngayVaoValue = DateTime.Now;
                    }

                    xML1 = new XML1()
                    {
                        MA_LK = GetLastestMaLuotKham(unitOfWork),
                        STT = GetLastestSTT(ngayVaoValue, unitOfWork),
                        MA_BN = updatedValues.ContainsKey("MA_BN") ? updatedValues["MA_BN"] : string.Empty,
                        BENH_ID = updatedValues.ContainsKey("BENH_ID") ? (Int32.TryParse(updatedValues["BENH_ID"], out bENHIDValue) ? bENHIDValue : -1) : -1,
                        TEN_BENH = updatedValues.ContainsKey("TEN_BENH") ? updatedValues["TEN_BENH"] : string.Empty,
                        MA_BENH = updatedValues.ContainsKey("MA_BENH") ? updatedValues["MA_BENH"] : string.Empty,
                        MA_BENHKHAC = updatedValues.ContainsKey("MA_BENHKHAC") ? updatedValues["MA_BENHKHAC"] : string.Empty,
                        NGAY_VAO = ngayVaoValue,
                        NGAY_RA = updatedValues.ContainsKey("NGAY_RA") ? (DateTime.TryParseExact(updatedValues["NGAY_RA"], CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayRaValue) ? ngayRaValue : new Nullable<DateTime>()) : new Nullable<DateTime>(),
                        NGAY_TAI_KHAM = updatedValues.ContainsKey("NGAY_TAI_KHAM") ? (DateTime.TryParseExact(updatedValues["NGAY_TAI_KHAM"], CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayTaiKhamValue) ? ngayTaiKhamValue : new Nullable<DateTime>()) : new Nullable<DateTime>(),
                        KET_QUA_DTRI = updatedValues.ContainsKey("KET_QUA_DTRI") ? (decimal.TryParse(updatedValues["KET_QUA_DTRI"], out ketQuaDieuTriValue) ? ketQuaDieuTriValue : 0) : 0,
                        TINH_TRANG_RV = updatedValues.ContainsKey("TINH_TRANG_RV") ? (decimal.TryParse(updatedValues["TINH_TRANG_RV"], out tinhTrangRaVienValue) ? tinhTrangRaVienValue : 0) : 0,
                        MA_KHOA = updatedValues.ContainsKey("MA_KHOA") ? updatedValues["MA_KHOA"] : string.Empty,
                        CHUAN_DOAN = updatedValues.ContainsKey("CHUAN_DOAN") ? updatedValues["CHUAN_DOAN"] : string.Empty,
                        PPDIEUTRI = updatedValues.ContainsKey("PPDIEUTRI") ? updatedValues["PPDIEUTRI"] : string.Empty,
                        LOIDANTHAYTHUOC = updatedValues.ContainsKey("LOIDANTHAYTHUOC") ? updatedValues["LOIDANTHAYTHUOC"] : string.Empty,
                        GHICHU = updatedValues.ContainsKey("GHICHU") ? updatedValues["GHICHU"] : string.Empty,
                        XML1_File = updatedValues.ContainsKey("XML1_File") ? updatedValues["XML1_File"] : string.Empty,
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    xML1 = unitOfWork.XML1Repository.Insert(xML1);

                    appHistoryServiceService.Add(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                    });

                    unitOfWork.Save();

                    resultMessage = xML1.MA_LK;
                }
            }
            catch (Exception ex)
            {
                isError = true;
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
        public string GetLastestMaLuotKham(UnitOfWork uOW)
        {
            if (uOW == null)
            {
                uOW = new UnitOfWork();
            }
            string result = string.Empty;
            string prefix = CommonConstants.Prefix_LuotKham + DateTime.Now.Year.ToString();
            int stt = 0;
            int maxLength = 0;
            var xML1s = uOW.XML1Repository.Get(m => m.MA_LK.StartsWith(prefix));
            if (xML1s.Count() > 0)
            {
                maxLength = xML1s.Select(m => m.MA_LK.Substring(prefix.Length).Length).Max();
                var max = xML1s.Where(m => m.MA_LK.Substring(prefix.Length).Length == maxLength).Select(m => m.MA_LK.Substring(prefix.Length)).Max();

                if (!Int32.TryParse(max, out stt))
                {
                    stt = 0;
                }
            }

            stt++;

            result = stt.ToString();

            maxLength = maxLength > CommonConstants.DoDaiMaLuotKham ? maxLength - result.Length : (CommonConstants.DoDaiMaLuotKham - result.Length);
            for (int i = 0; i < maxLength; i++)
            {
                result = "0" + result;
            }

            return prefix + result;
        }
        public int GetLastestSTT(DateTime ngayKham, UnitOfWork uOW)
        {
            int stt = 0;
            var xML1s = uOW.XML1Repository.Get(m => m.NGAY_VAO == ngayKham);
            if (xML1s.Count() > 0)
            {
                stt = xML1s.Max(m => m.STT);
            }

            return stt++;
        }

        public string NhapDuLieuTuFileExcel(DataTable dt)
        {
            string resultMessage = string.Empty;
            int rowIndex = 0;
            int tongSoDong = dt.Rows.Count;

            List<Task> tasks = null;
            if (dt.Columns.Count > 13)
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

            XML1 xML1 = null;
            ICD10 benh = null;
            DateTime ngayVaoValue, ngayRaValue, ngayTaiKhamValue;
            decimal ketQuaDieuTriValue, tinhTrangRaVienValue;

            Users nguoiSuDungCapNhat = unit3.UsersRepository.Get(m => m.UserID.Equals(userID)).FirstOrDefault();

            for (int n = tuDong; n < toiDong; n++)
            {
                xML1 = null;
                string maBN = dt.Rows[n][0].ToString();
                string maLK = dt.Rows[n][1].ToString();
                string maBenh = dt.Rows[n][2].ToString();
                string tenBenh = dt.Rows[n][3].ToString();
                string maBenhKhac = dt.Rows[n][4].ToString();
                string ngayVao = dt.Rows[n][5].ToString();
                string ngayRa = dt.Rows[n][6].ToString();
                string ketQuaDTri = dt.Rows[n][7].ToString();
                string tinhTrangRaVien = dt.Rows[n][8].ToString();
                string maKhoa = dt.Rows[n][9].ToString();
                string chanDoan = dt.Rows[n][10].ToString();
                string pPDieuTri = dt.Rows[n][11].ToString();
                string loiDanThayThuoc = dt.Rows[n][12].ToString();
                string ngayTaiKham = dt.Rows[n][13].ToString();

                if (!string.IsNullOrEmpty(ngayVao))
                {
                    int start = ngayVao.IndexOf(":");
                    int end = ngayVao.LastIndexOf(":");

                    if (start < end)
                    {
                        ngayVao = ngayVao.Substring(0, end);
                    }
                }

                if (!string.IsNullOrEmpty(ngayRa))
                {
                    int start = ngayRa.IndexOf(":");
                    int end = ngayRa.LastIndexOf(":");

                    if (start < end)
                    {
                        ngayRa = ngayRa.Substring(0, end);
                    }
                }

                if (!string.IsNullOrEmpty(ngayTaiKham))
                {
                    int start = ngayTaiKham.IndexOf(":");
                    int end = ngayTaiKham.LastIndexOf(":");

                    if (start < end)
                    {
                        ngayTaiKham = ngayTaiKham.Substring(0, end);
                    }
                }

                if (!DateTime.TryParseExact(ngayVao, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayVaoValue))
                {
                    ngayVaoValue = DateTime.Now;
                }

                benh = unit3.ICD10Repository.Get(m => m.Ma.Trim().ToLower().Equals(maBenh.Trim().ToLower())).FirstOrDefault();

                if (unit3.XML1Repository.Get().FirstOrDefault(m => m.MA_LK.Trim().ToLower().Equals(maLK.Trim().ToLower())) == null)
                {
                    xML1 = new XML1()
                    {
                        MA_LK = !string.IsNullOrEmpty(maLK) ? maLK : GetLastestMaLuotKham(unit3),
                        STT = GetLastestSTT(ngayVaoValue, unit3),
                        MA_BN = maBN,
                        TEN_BENH = benh != null ? benh.Ten : tenBenh,
                        BENH_ID = benh != null ? benh.Id : -1,
                        MA_BENH = maBenh,
                        MA_BENHKHAC = maBenhKhac,
                        NGAY_VAO = ngayVaoValue,
                        NGAY_RA = (DateTime.TryParseExact(ngayRa, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayRaValue) ? ngayRaValue : new Nullable<DateTime>()),
                        NGAY_TAI_KHAM = (DateTime.TryParseExact(ngayTaiKham, CommonConstants.DateTimeFormat, null, DateTimeStyles.None, out ngayTaiKhamValue) ? ngayTaiKhamValue : new Nullable<DateTime>()),
                        KET_QUA_DTRI = (decimal.TryParse(ketQuaDTri, out ketQuaDieuTriValue) ? ketQuaDieuTriValue : 0),
                        TINH_TRANG_RV = (decimal.TryParse(tinhTrangRaVien, out tinhTrangRaVienValue) ? tinhTrangRaVienValue : 0),
                        MA_KHOA = maKhoa,
                        CHUAN_DOAN = chanDoan,
                        PPDIEUTRI = pPDieuTri,
                        LOIDANTHAYTHUOC = loiDanThayThuoc,
                        ModifiedDate = DateTime.Now,
                        ModifiedUsers = nguoiSuDungCapNhat,
                        CreatedDate = DateTime.Now,
                        CreatedUsers = nguoiSuDungCapNhat
                    };

                    xML1 = unit3.XML1Repository.Insert(xML1);

                    unit3.Save();
                }
                else
                {
                    unit3.AppHistoryRepository.Insert(new AppHistory()
                    {
                        Ma = maTable,
                        Ten = tenTable,
                        ThaoTac = CommonConstants.ThaoTacImport,
                        GhiChu = "Mã lượt khám: '" + maLK + "' đã có trong dữ liệu",
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
