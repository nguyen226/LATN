using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace JetMedicalWebApp.Services
{
    public class ChienDichService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string maTable = "ChienDich";
        private string tenTable = "ChienDich";

        public List<ChienDichDto> GetListExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                        out Dictionary<string, string> outData)
        {
            List<ChienDichDto> result = new List<ChienDichDto>();
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            var query = unitOfWork.ChienDichRepository.Get().Select("new (" + inData[CommonConstants.StrSelectedFields] + ")");

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
                            ChienDichDto obj = new ChienDichDto();
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
                ChienDichDto obj = new ChienDichDto();

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
        public IEnumerable<ChienDichDto> GetAllExposeDto(string selectedFields, string sortedColumnNames)
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> outData = null;

            inData.Add(CommonConstants.StrSelectedFields, selectedFields);
            inData.Add(CommonConstants.StrSortedColumnNames, sortedColumnNames);

            return GetListExposeDto(filters, inData, out outData);
        }
        public ChienDichDto GetByIdExposeDto(int id, string selectedFields)
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
            return unitOfWork.ChienDichRepository.Get().Select("new (" + selectedFields + ")")
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
                    var query = unitOfWork.ChienDichRepository.Get(m => ids.Contains(m.Id)).ToList();
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
                            unitOfWork.ChienDichRepository.Delete(item);
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

                var query = unitOfWork.ChienDichRepository.Get(m => ids.Contains(m.Id)).ToList();

                foreach (ChienDich obj in query)
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

        public string AddOrUpdate(string id, Dictionary<string, string> updatedValues)
        {
            ChienDich chienDich = null;
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
                    int patientGroupIDValue, notificationIDValue, typeValue;
                    nguoiSuDungCapNhat = internalService.GetNguoiSuDungCapNhat(unitOfWork);

                    if(Int32.TryParse(updatedValues.ContainsKey("PatientGroupID") ? updatedValues["PatientGroupID"] : string.Empty, out patientGroupIDValue) 
                        && Int32.TryParse(updatedValues.ContainsKey("NotificationID") ? updatedValues["NotificationID"] : string.Empty, out notificationIDValue)
                        && Int32.TryParse(updatedValues.ContainsKey("Type") ? updatedValues["Type"] : string.Empty, out typeValue))
                    {
                        chienDich = new ChienDich()
                        {
                            PatientGroupID = patientGroupIDValue,
                            NotificationID = notificationIDValue,
                            Type = typeValue,
                            FileDinhKem = updatedValues.ContainsKey("FileDinhKem") ? updatedValues["FileDinhKem"] : string.Empty,
                            Content = updatedValues.ContainsKey("Content") ? updatedValues["Content"] : string.Empty,
                            ModifiedDate = DateTime.Now,
                            ModifiedUsers = nguoiSuDungCapNhat,
                            CreatedDate = DateTime.Now,
                            CreatedUsers = nguoiSuDungCapNhat
                        };

                        chienDich = unitOfWork.ChienDichRepository.Insert(chienDich);

                        appHistoryServiceService.Add(new AppHistory()
                        {
                            Ma = maTable,
                            Ten = tenTable,
                            ThaoTac = Common.CommonConstants.ThaoTacThemMoiDuLieu,
                        });

                        unitOfWork.Save();

                        // Gửi email chiến dịch

                        switch (typeValue)
                        {
                            case 2: // email
                                resultMessage = GuiChienDich(patientGroupIDValue, notificationIDValue, chienDich.Id, chienDich.FileDinhKem, unitOfWork);
                                break;
                            case 1:
                                break;
                            case 3:
                                break;
                        }

                    }
                    else
                    {
                        resultMessage = "Không tìm thấy giá trị: PatientGroupID hoặc NotificationID hoặc Type";
                    }
                }
            }
            catch (Exception ex)
             {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }

        public List<ChienDichDto> GetListDataFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                                    out Dictionary<string, string> outData)
        {
            List<ChienDichDto> result = new List<ChienDichDto>();
            string tableName = "uv_ChienDich";
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
                            ChienDichDto obj = new ChienDichDto();

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
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<ChienDichDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                ChienDichDto obj = new ChienDichDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "ModifiedDate":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
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


        public string GuiChienDich(int patientGroupId, int mauThongBaoId, int chienDichId, string duongDanFileDinhKem, UnitOfWork uOW)
        {
            List<UsersDto> listUserEmail = new List<UsersDto>();
            listUserEmail = GetListEmailIdByPatientGroupIdFromView(patientGroupId, uOW);
            Notification mauThongBao = uOW.NotificationRepository.GetByID(mauThongBaoId);
            string res = string.Empty;
            string path = System.Web.HttpContext.Current.Server.MapPath(CommonConstants.DuongDanUploadNotifications);
            SmtpConfigurationDto _config1 = null, _config2 = null, _config3 = null, _config4 = null, _config5 = null;

            if (listUserEmail != null)
            {
                if(listUserEmail.Count() > 0)
                {
                    var listEmail = uOW.CauHinhEmailRepository.Get(m => m.Active == true).ToList();
                    if(listEmail != null)
                    {
                        if(listEmail.Count() > 0)
                        {
                            int length = listEmail.Count() > 5 ? 5 : listEmail.Count();
                            for(int i =0; i < length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        _config1 = new SmtpConfigurationDto();
                                        _config1.Username = listEmail[i].Account;
                                        _config1.Password = listEmail[i].Password;
                                        _config1.Host = listEmail[i].Host;
                                        _config1.Port = Int32.Parse(listEmail[i].Port);
                                        _config1.Ssl = listEmail[i].SSL;
                                        break;

                                    case 1:
                                        _config2 = new SmtpConfigurationDto();
                                        _config2.Username = listEmail[i].Account;
                                        _config2.Password = listEmail[i].Password;
                                        _config2.Host = listEmail[i].Host;
                                        _config2.Port = Int32.Parse(listEmail[i].Port);
                                        _config2.Ssl = listEmail[i].SSL;
                                        break;

                                    case 2:
                                        _config3 = new SmtpConfigurationDto();
                                        _config3.Username = listEmail[i].Account;
                                        _config3.Password = listEmail[i].Password;
                                        _config3.Host = listEmail[i].Host;
                                        _config3.Port = Int32.Parse(listEmail[i].Port);
                                        _config3.Ssl = listEmail[i].SSL;
                                        break;

                                    case 3:
                                        _config4 = new SmtpConfigurationDto();
                                        _config4.Username = listEmail[i].Account;
                                        _config4.Password = listEmail[i].Password;
                                        _config4.Host = listEmail[i].Host;
                                        _config4.Port = Int32.Parse(listEmail[i].Port);
                                        _config4.Ssl = listEmail[i].SSL;
                                        break;

                                    case 4:
                                        _config5 = new SmtpConfigurationDto();
                                        _config5.Username = listEmail[i].Account;
                                        _config5.Password = listEmail[i].Password;
                                        _config5.Host = listEmail[i].Host;
                                        _config5.Port = Int32.Parse(listEmail[i].Port);
                                        _config5.Ssl = listEmail[i].SSL;
                                        break;
                                }
                            }

                            if(_config2 == null)
                            {
                                _config2 = _config1;
                            }

                            if (_config3 == null)
                            {
                                _config3 = _config2;
                            }

                            if (_config4 == null)
                            {
                                _config4 = _config3;
                            }

                            if (_config5 == null)
                            {
                                _config5 = _config4;
                            }

                            List<Task> tasks = null;

                            if (listUserEmail.Count() > 100)
                            {
                                List<UsersDto> danhSach1 = new List<UsersDto>();
                                List<UsersDto> danhSach2 = new List<UsersDto>();
                                List<UsersDto> danhSach3 = new List<UsersDto>();
                                List<UsersDto> danhSach4 = new List<UsersDto>();
                                List<UsersDto> danhSach5 = new List<UsersDto>();
                                danhSach1 = listUserEmail.GetRange(0, listUserEmail.Count() / 5);
                                danhSach2 = listUserEmail.GetRange((listUserEmail.Count() / 5) + 1, (listUserEmail.Count() / 5) * 2);
                                danhSach3 = listUserEmail.GetRange(((listUserEmail.Count() / 5) * 2) + 1, (listUserEmail.Count() / 5) * 3);
                                danhSach4 = listUserEmail.GetRange(((listUserEmail.Count() / 5) * 3) + 1, (listUserEmail.Count() / 5) * 4);
                                danhSach5 = listUserEmail.GetRange(((listUserEmail.Count() / 5) * 4) + 1, listUserEmail.Count());
                                tasks = new List<Task>
                                {
                                    new Task(async () => await GuiChienDich(danhSach1,mauThongBao, _config1,path, chienDichId,duongDanFileDinhKem)),
                                    new Task(async () => await GuiChienDich(danhSach2,mauThongBao, _config2,path, chienDichId,duongDanFileDinhKem)),
                                    new Task(async () => await GuiChienDich(danhSach3,mauThongBao, _config3,path, chienDichId,duongDanFileDinhKem)),
                                    new Task(async () => await GuiChienDich(danhSach4,mauThongBao, _config4,path, chienDichId,duongDanFileDinhKem)),
                                    new Task(async () => await GuiChienDich(danhSach5,mauThongBao, _config5,path, chienDichId,duongDanFileDinhKem)),
                                };
                            }
                            else
                            {
                                tasks = new List<Task>
                                {
                                    new Task(async () => await GuiChienDich(listUserEmail,mauThongBao, _config1,path, chienDichId,duongDanFileDinhKem)),
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

                        }else
                        {
                            res = "Chưa cấu hình email server";
                        }
                    }
                    else
                    {
                        res = "Chưa cấu hình email server";
                    }
                }
                else
                {
                    res = "Không có user nào trong nhóm chiến dịch này";
                }
            }
            else
            {
                res = "Không có user nào trong nhóm chiến dịch này";
            }
            return res;
        }


        public async Task GuiChienDich(List<UsersDto> danhSach, Notification mauThongBao, SmtpConfigurationDto _config, string path, int chienDichId, string duongDanFileDinhKem)
        {
            ThucHienGuiChienDich(danhSach, mauThongBao, _config, path, chienDichId, duongDanFileDinhKem);
        }

        
        public void ThucHienGuiChienDich(List<UsersDto> danhSach, Notification mauThongBao, SmtpConfigurationDto _config, string path, int chienDichId, string duongDanFileDinhKem)
        {
            UnitOfWork uOW = new UnitOfWork();
            GmailService gmailService = new GmailService();
            string resultMessage = string.Empty, messageSendEmail = string.Empty;
            LichSuGuiChienDichService lichSuGuiChienDichService = new LichSuGuiChienDichService();
            try
            {
                foreach (var item in danhSach)
                {
                    if (!string.IsNullOrEmpty(item.EmailID))
                    {
                        if (mauThongBao.NoHTML.IndexOf("{{HoVaTen}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{HoVaTen}}", item.HoTen);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{MaBenhNhan}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{MaBenhNhan}}", item.MA_BN);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{NgaySinh}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{NgaySinh}}", item.DateOfBirth);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{CMND}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{CMND}}", item.CMND);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{BHYT}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{BHYT}}", item.BHYT);
                        }


                        if (mauThongBao.NoHTML.IndexOf("{{NgayVao}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{NgayVao}}", item.NGAY_VAO);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{NgayRa}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{NgayRa}}", item.NGAY_RA);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{NgayTaiKham}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{NgayTaiKham}}", item.NGAY_TAI_KHAM);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{MaBenh}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{MaBenh}}", item.MA_BENH);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{KetQuaDieuTri}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{KetQuaDieuTri}}", item.KET_QUA_DTRI);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{TenBenh}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{TenBenh}}", item.TEN_BENH);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{ChuanDoan}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{ChuanDoan}}", item.CHUAN_DOAN);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{PhuongPhapDieuTri}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{PhuongPhapDieuTri}}", item.PPDIEUTRI);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{LoiDanThayThuoc}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{LoiDanThayThuoc}}", item.LOIDANTHAYTHUOC);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{GhiChu}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{GhiChu}}", item.GHICHU);
                        }

                        if (mauThongBao.NoHTML.IndexOf("{{FileDinhKem}}") > 0)
                        {
                            mauThongBao.NoHTML = mauThongBao.NoHTML.Replace("{{FileDinhKem}}", duongDanFileDinhKem);
                        }


                        if (mauThongBao.NoTitle.IndexOf("{{HoVaTen}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{HoVaTen}}", item.HoTen);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{MaBenhNhan}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{MaBenhNhan}}", item.MA_BN);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{NgaySinh}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{NgaySinh}}", item.DateOfBirth);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{CMND}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{CMND}}", item.CMND);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{BHYT}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{BHYT}}", item.BHYT);
                        }


                        if (mauThongBao.NoTitle.IndexOf("{{NgayVao}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{NgayVao}}", item.NGAY_VAO);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{NgayRa}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{NgayRa}}", item.NGAY_RA);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{NgayTaiKham}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{NgayTaiKham}}", item.NGAY_TAI_KHAM);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{MaBenh}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{MaBenh}}", item.MA_BENH);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{KetQuaDieuTri}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{KetQuaDieuTri}}", item.KET_QUA_DTRI);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{TenBenh}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{TenBenh}}", item.TEN_BENH);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{ChuanDoan}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{ChuanDoan}}", item.CHUAN_DOAN);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{PhuongPhapDieuTri}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{PhuongPhapDieuTri}}", item.PPDIEUTRI);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{LoiDanThayThuoc}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{LoiDanThayThuoc}}", item.LOIDANTHAYTHUOC);
                        }

                        if (mauThongBao.NoTitle.IndexOf("{{GhiChu}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{GhiChu}}", item.GHICHU);
                        }
                        if (mauThongBao.NoTitle.IndexOf("{{FileDinhKem}}") > 0)
                        {
                            mauThongBao.NoTitle = mauThongBao.NoTitle.Replace("{{FileDinhKem}}", duongDanFileDinhKem);
                        }


                        if (mauThongBao.NoDes.IndexOf("{{HoVaTen}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{HoVaTen}}", item.HoTen);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{MaBenhNhan}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{MaBenhNhan}}", item.MA_BN);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{NgaySinh}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{NgaySinh}}", item.DateOfBirth);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{CMND}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{CMND}}", item.CMND);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{BHYT}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{BHYT}}", item.BHYT);
                        }


                        if (mauThongBao.NoDes.IndexOf("{{NgayVao}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{NgayVao}}", item.NGAY_VAO);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{NgayRa}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{NgayRa}}", item.NGAY_RA);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{NgayTaiKham}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{NgayTaiKham}}", item.NGAY_TAI_KHAM);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{MaBenh}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{MaBenh}}", item.MA_BENH);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{KetQuaDieuTri}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{KetQuaDieuTri}}", item.KET_QUA_DTRI);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{TenBenh}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{TenBenh}}", item.TEN_BENH);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{ChuanDoan}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{ChuanDoan}}", item.CHUAN_DOAN);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{PhuongPhapDieuTri}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{PhuongPhapDieuTri}}", item.PPDIEUTRI);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{LoiDanThayThuoc}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{LoiDanThayThuoc}}", item.LOIDANTHAYTHUOC);
                        }

                        if (mauThongBao.NoDes.IndexOf("{{GhiChu}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{GhiChu}}", item.GHICHU);
                        }
                    
                        if (mauThongBao.NoDes.IndexOf("{{FileDinhKem}}") > 0)
                        {
                            mauThongBao.NoDes = mauThongBao.NoDes.Replace("{{FileDinhKem}}", duongDanFileDinhKem);
                        } 

                        // gửi email
                        EmailMessageDto message = new EmailMessageDto();
                        message.ToEmail = item.EmailID;
                        message.Subject = mauThongBao.NoTitle;
                        message.Body = mauThongBao.NoHTML;
                        message.IsHtml = true;

                        messageSendEmail = gmailService.GuiEmailToiUser(_config, message, mauThongBao.Image);
                        // lưu trang thái vào db LichSuGuiChienDich
                        lichSuGuiChienDichService.Add((string.IsNullOrEmpty(messageSendEmail) ? 1 : 2),
                                                        chienDichId, item.UserID, item.CompanyId, messageSendEmail, 
                                                        mauThongBao.NoTitle,
                                                        mauThongBao.NoHTML,
                                                        mauThongBao.NoDes,
                                                        uOW);
                    }else
                    {
                        // lưu trang thái vào db LichSuGuiChienDich
                        lichSuGuiChienDichService.Add(2,chienDichId, item.UserID, item.CompanyId, "Không có email để gửi",
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        uOW);
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }
        }

        public List<UsersDto> GetListEmailIdByPatientGroupIdFromView(int patientGroupId, UnitOfWork u)
        {
            List<UsersDto> result = new List<UsersDto>();
            string tableName = "uv_EmailIdOfUsers";
            var sqlCommand = "SELECT UserID, CompanyId, EmailID,BHYT, CMND,MA_BN,HoTen,DateOfBirth,NGAY_VAO";
            sqlCommand += ", NGAY_RA,NGAY_TAI_KHAM,MA_BENH,TEN_BENH,KET_QUA_DTRI";
            sqlCommand += ",CHUAN_DOAN,PPDIEUTRI,LOIDANTHAYTHUOC,GHICHU ";
            sqlCommand += "FROM " + tableName + " " + (" WHERE PatientGroupId = " + patientGroupId.ToString());
    
            var displayedData = u.DataContext.Database.SqlQuery<UsersDto>(sqlCommand).AsEnumerable();

            foreach (dynamic item in displayedData)
            {
                UsersDto obj = new UsersDto();

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
