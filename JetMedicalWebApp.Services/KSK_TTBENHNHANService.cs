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
using System.Data.SqlClient;

namespace JetMedicalWebApp.Services
{
    public class KSK_TTBENHNHANService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private string maTable = "KSK_TTBENHNHAN";
        private string tenTable = "KSK_TTBENHNHAN";

        public List<KSK_TTBENHNHANDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<KSK_TTBENHNHANDto> result = new List<KSK_TTBENHNHANDto>();
            string stringFilter = string.Empty, usernameNguoiCapNhat = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.KSK_TTBENHNHANRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            KSK_TTBENHNHANDto obj = new KSK_TTBENHNHANDto();
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
                KSK_TTBENHNHANDto obj = new KSK_TTBENHNHANDto();

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
        public IEnumerable<KSK_TTBENHNHANDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public KSK_TTBENHNHANDto GetByIdExposeDto(int id, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "KSK_ID=" + id.ToString();
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }
        public KSK_TTBENHNHANDto GetByUsernameExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "Username=\"" + username + "\"";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public KSK_TTBENHNHANDto GetByEmailOrPhoneExposeDto(string username, string selectedFields)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);

            string stringFilter = "EmailID=(\"" + username + "\")  OR Phone =(\"" + username + "\")";
            filters.Add("StringFilter", stringFilter);

            return GetListExposeDto(filters, inData, out outData).FirstOrDefault();
        }

        public KSK_TTBENHNHANDto GetByUserIdExposeDto(string userId, string selectedFields)
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
                return unitOfWork.KSK_TTBENHNHANRepository.Get().Select("new (" + selectedFields + ")")
                                                    .Where(stringFilter)
                                                    .Count();
            }
            return unitOfWork.KSK_TTBENHNHANRepository.Get().Select("new (" + selectedFields + ")").Count();
        }
        public string DeleteByIds(List<int> ids)
        {
            string result = string.Empty;

            if (ids.Count() > 0)
            {
                AppHistoryService appHistoryService = new AppHistoryService();

                try
                {
                    string sqlCommand = "exec usp_DeleteKhamChuaBenh @id";

                  
                    foreach (var item in ids)
                    {
                        SqlParameter[] param = new SqlParameter[] {
                            new SqlParameter
                            {
                                 ParameterName = "id",
                                SqlDbType = SqlDbType.Int,
                                Direction = System.Data.ParameterDirection.Input,
                                Value = item
                            }
                        };

                        unitOfWork.DataContext.Database.ExecuteSqlCommand(sqlCommand, param).ToString();
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
                var query = unitOfWork.KSK_TTBENHNHANRepository.Get(m => ids.Contains(m.KSK_ID)).ToList();
                decimal decimalValue = -1;
                foreach (KSK_TTBENHNHAN obj in query)
                {
                    foreach (KeyValuePair<string, string> item in updatedData[obj.KSK_ID])
                    {
                        switch (item.Key)
                        {
                            case "GIOI_TINH":
                                if (decimal.TryParse(item.Value, out decimalValue))
                                {
                                    obj.GetType().GetProperty(item.Key).SetValue(obj, decimalValue, null);
                                }
                                break;


                            default:
                                Common.Services.ObjectService.SetValue(obj, item.Key, item.Value);
                                break;
                        }
                        appHistoryService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacCapNhatDuLieu,
                            GhiChu = "KSK_ID: " + obj.KSK_ID.ToString() + ", field: " + item.Key
                        });
                    }
                }

                unitOfWork.Save();
            }

            return messageResult;
        }

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            KSK_TTBENHNHAN KSK_TTBENHNHAN = null;
            string resultMessage = string.Empty;
            int idValue = -1;
            decimal gioiTinhValue = -1;
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
                        if (string.IsNullOrEmpty(resultMessage))
                        {
                            resultMessage = id;
                        }
                    }
                    else
                    {
                        resultMessage = "Giá trị TTBN=" + id + " không phù hợp.";
                    }
                }
                else
                {

                    KSK_TTBENHNHAN = new KSK_TTBENHNHAN()
                    {
                        UserID = -1,
                        MA_LK = updatedValues.ContainsKey("MA_LK") ? updatedValues["MA_LK"] : string.Empty,
                        MA_BN = updatedValues.ContainsKey("MA_BN") ? updatedValues["MA_BN"] : string.Empty,
                        HO_TEN = updatedValues.ContainsKey("HO_TEN") ? updatedValues["HO_TEN"] : string.Empty,
                        NGAY_SINH = updatedValues.ContainsKey("NGAY_SINH") ? updatedValues["NGAY_SINH"] : string.Empty,
                        GIOI_TINH = updatedValues.ContainsKey("GIOI_TINH") ? (decimal.TryParse(updatedValues["GIOI_TINH"], out gioiTinhValue) ? gioiTinhValue : -1) : -1,
                        CMND = updatedValues.ContainsKey("CMND") ? updatedValues["CMND"] : string.Empty,
                        DIENTHOAI = updatedValues.ContainsKey("DIENTHOAI") ? updatedValues["DIENTHOAI"] : string.Empty,
                        TEN_DOANKSK = updatedValues.ContainsKey("TEN_DOANKSK") ? updatedValues["TEN_DOANKSK"] : string.Empty,
                        CAN_NANG = updatedValues.ContainsKey("CAN_NANG") ? updatedValues["CAN_NANG"] : string.Empty,
                        CHIEU_CAO = updatedValues.ContainsKey("CHIEU_CAO") ? updatedValues["CHIEU_CAO"] : string.Empty,
                        HUYET_AP = updatedValues.ContainsKey("HUYET_AP") ? updatedValues["HUYET_AP"] : string.Empty,
                        MACH = updatedValues.ContainsKey("MACH") ? updatedValues["MACH"] : string.Empty,
                        KHAM_LAM_SANG = updatedValues.ContainsKey("KHAM_LAM_SANG") ? updatedValues["KHAM_LAM_SANG"] : string.Empty,
                        PHAN_LOAI = updatedValues.ContainsKey("PHAN_LOAI") ? updatedValues["PHAN_LOAI"] : string.Empty,
                        KET_LUAN = updatedValues.ContainsKey("KET_LUAN") ? updatedValues["KET_LUAN"] : string.Empty,
                        TU_VAN = updatedValues.ContainsKey("TU_VAN") ? updatedValues["TU_VAN"] : string.Empty,
                    };

                    KSK_TTBENHNHAN = unitOfWork.KSK_TTBENHNHANRepository.Insert(KSK_TTBENHNHAN);

                    unitOfWork.Save();
                    resultMessage = KSK_TTBENHNHAN.KSK_ID.ToString();
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

            KSK_TTBENHNHAN KSK_TTBENHNHAN = null;

            KSK_TTBENHNHAN nguoiSuDungCapNhat = unit3.KSK_TTBENHNHANRepository.Get(m => m.UserID.Equals(userID)).FirstOrDefault();

            for (int n = tuDong; n < toiDong; n++)
            {
                decimal gioiTinhValue = -1;
                KSK_TTBENHNHAN = null;
                string MA_LK = dt.Rows[n][0].ToString();
                string MA_BN = dt.Rows[n][1].ToString();
                string HO_TEN = dt.Rows[n][2].ToString();
                string GIOI_TINH = dt.Rows[n][3].ToString();
                string CMND = dt.Rows[n][4].ToString();
                string DIENTHOAI = dt.Rows[n][5].ToString();
                string NGAY_SINH = dt.Rows[n][6].ToString();
                string TEN_DOANKSK = dt.Rows[n][7].ToString();
                string CHIEU_CAO = dt.Rows[n][8].ToString();
                string CAN_NANG = dt.Rows[n][9].ToString();
                string HUYET_AP = dt.Rows[n][10].ToString();
                string MACH = dt.Rows[n][11].ToString();
                string KHAM_LAM_SANG = dt.Rows[n][12].ToString();
                string PHAN_LOAI = dt.Rows[n][13].ToString();
                string KET_LUAN = dt.Rows[n][14].ToString();
                string TU_VAN = dt.Rows[n][15].ToString();

                KSK_TTBENHNHAN = new KSK_TTBENHNHAN()
                {
                    MA_LK = MA_LK,
                    MA_BN = MA_BN,
                    HO_TEN = HO_TEN,
                    NGAY_SINH = NGAY_SINH,
                    GIOI_TINH = decimal.TryParse(GIOI_TINH, out gioiTinhValue) ? gioiTinhValue : -1,
                    CMND = CMND,
                    DIENTHOAI = DIENTHOAI,
                    TEN_DOANKSK = TEN_DOANKSK,
                    CAN_NANG = CAN_NANG,
                    CHIEU_CAO = CHIEU_CAO,
                    HUYET_AP = HUYET_AP,
                    MACH = MACH,
                    KHAM_LAM_SANG = KHAM_LAM_SANG,
                    PHAN_LOAI = PHAN_LOAI,
                    KET_LUAN = KET_LUAN,
                    TU_VAN = TU_VAN
                };

                KSK_TTBENHNHAN = unit3.KSK_TTBENHNHANRepository.Insert(KSK_TTBENHNHAN);
                unit3.Save();
            }
        }

    }
}
