using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace JetMedicalWebApp.Common
{
    public static class CommonConstants
    {

        public static string SessionUserId = "SessionUserId";
        public static string CookieUserID = "CookieUserID";
        public static string CookieUserName = "CookieUserName";
        public static string CookieUserAdmin = "CookieUserAdmin";

        public static string CookieMemberID = "CookieMemberID";
        public static string CookieMemberName = "CookieMemberName";

        public static string Session_CurrentUser_Company_GroupPermission = "SessionCompanyGroupPermission";

        public static string AdminUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["AdminUsername"].ToString();
        public static string AdminPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["AdminPassword"].ToString();
        public static string AdminRole = System.Web.Configuration.WebConfigurationManager.AppSettings["AdminRole"].ToString();
        public static string UserRole = System.Web.Configuration.WebConfigurationManager.AppSettings["UserRole"].ToString();
        public static string DateFormat = System.Web.Configuration.WebConfigurationManager.AppSettings["DateFormat"];
        public static string DateFormat2 = System.Web.Configuration.WebConfigurationManager.AppSettings["DateFormat2"];
        public static string DateTimeFormat = System.Web.Configuration.WebConfigurationManager.AppSettings["DateTimeFormat"];
        public static string FileNoImagePerson = System.Web.Configuration.WebConfigurationManager.AppSettings["FileNoImagePerson"];
        public static string FileNoImage = System.Web.Configuration.WebConfigurationManager.AppSettings["FileNoImage"];
        public static string NgayBatDauSuDungPhanMem = System.Web.Configuration.WebConfigurationManager.AppSettings["NgayBatDauSuDungPhanMem"].ToString();
        public static string DuongDanUploadFile = System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadFile"];
        public static string DuongDanUploadFileHinhAnhUsers = DuongDanUploadFile + System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadFileHinhAnhUsers"];
        public static string DuongDanUploadFileLogo = DuongDanUploadFile + System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadFileLogo"];
        public static string DuongDanUploadFileSlide = DuongDanUploadFile + System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadFileSlide"];
        public static string DuongDanUploadFileStaff = DuongDanUploadFile + System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadFileStaff"];

        public static string DuongDanUploadNotifications = DuongDanUploadFile + System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanUploadNotifications"];
        public static string TextButtonThem = System.Web.Configuration.WebConfigurationManager.AppSettings["TextButtonThem"].ToString();
        public static string TextButtonCapNhat = System.Web.Configuration.WebConfigurationManager.AppSettings["TextButtonCapNhat"].ToString();
        public static string KeyEncodeAndDecodeURL = System.Web.Configuration.WebConfigurationManager.AppSettings["KeyEncodeAndDecodeURL"].ToString();
        public static string ThaoTacXoaDuLieu = System.Web.Configuration.WebConfigurationManager.AppSettings["ThaoTacXoaDuLieu"].ToString();
        public static string ThaoTacThemMoiDuLieu = System.Web.Configuration.WebConfigurationManager.AppSettings["ThaoTacThemMoiDuLieu"].ToString();
        public static string ThaoTacCapNhatDuLieu = System.Web.Configuration.WebConfigurationManager.AppSettings["ThaoTacCapNhatDuLieu"].ToString();
        public static string ThaoTacTruyCap = System.Web.Configuration.WebConfigurationManager.AppSettings["ThaoTacTruyCap"].ToString();
        public static string ThaoTacImport = System.Web.Configuration.WebConfigurationManager.AppSettings["ThaoTacImport"].ToString();
        public static string Session_CurrentUser_Menu_GroupPermission = System.Web.Configuration.WebConfigurationManager.AppSettings["Session_CurrentUser_Menu_GroupPermission"].ToString();
        public static string StrSelectedFields = System.Web.Configuration.WebConfigurationManager.AppSettings["StrSelectedFields"].ToString();
        public static string DuongDanThuMucTapTin = System.Web.Configuration.WebConfigurationManager.AppSettings["DuongDanThuMucTapTin"].ToString();
        public static string StrSortedColumnNames = System.Web.Configuration.WebConfigurationManager.AppSettings["StrSortedColumnNames"].ToString();
        public static string StrTotalRecords = System.Web.Configuration.WebConfigurationManager.AppSettings["StrTotalRecords"].ToString();
        public static string StrTotalPages = System.Web.Configuration.WebConfigurationManager.AppSettings["StrTotalPages"].ToString();
        public static string StrStringFilter = System.Web.Configuration.WebConfigurationManager.AppSettings["StrStringFilter"].ToString();
        public static string StrTop = System.Web.Configuration.WebConfigurationManager.AppSettings["StrTop"].ToString();
        public static string StrTake = System.Web.Configuration.WebConfigurationManager.AppSettings["StrTake"].ToString();
        public static string StrSkip = System.Web.Configuration.WebConfigurationManager.AppSettings["StrSkip"].ToString();
        public static string StrFilters = System.Web.Configuration.WebConfigurationManager.AppSettings["StrFilters"].ToString();
        public static string StrInData = System.Web.Configuration.WebConfigurationManager.AppSettings["StrInData"].ToString();
        public static string StrOutData = System.Web.Configuration.WebConfigurationManager.AppSettings["StrOutData"].ToString();
        public static string StrPage = System.Web.Configuration.WebConfigurationManager.AppSettings["StrPage"].ToString();
        public static string StrPageSize = System.Web.Configuration.WebConfigurationManager.AppSettings["StrPageSize"].ToString();
        public static string StrSortColumnDir = System.Web.Configuration.WebConfigurationManager.AppSettings["StrSortColumnDir"].ToString();
        public static string StrSortColumn = System.Web.Configuration.WebConfigurationManager.AppSettings["StrSortColumn"].ToString();
        public static string StrLength = System.Web.Configuration.WebConfigurationManager.AppSettings["StrLength"].ToString();
        public static string StrRequestParamters = System.Web.Configuration.WebConfigurationManager.AppSettings["StrRequestParamters"].ToString();
        public static string SessionName_Menu = System.Web.Configuration.WebConfigurationManager.AppSettings["SessionName_Menu"].ToString();
        public static int NotificationType_App_Popup = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["NotificationType_App_Popup"].ToString());
        public static int NotificationType_App_TinTuc = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["NotificationType_App_TinTuc"].ToString());
        public static int NotificationType_App_DichVu = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["NotificationType_App_DichVu"].ToString());
        public static int NotificationType_App_KetQuaKham = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["NotificationType_App_KetQuaKham"].ToString());
        public static int KieuChienDich_ThongBao = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KieuChienDich_ThongBao"].ToString());
        public static int KieuChienDich_Gmail = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KieuChienDich_Gmail"].ToString());
        public static int KieuChienDich_TinNhan = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KieuChienDich_TinNhan"].ToString());
        public static int RegisterService_DangKy = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RegisterService_DangKy"].ToString());
        public static int RegisterService_TiepNhan = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RegisterService_TiepNhan"].ToString());
        public static int RegisterService_Kham = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RegisterService_Kham"].ToString());
        public static int RegisterService_KetThuc = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RegisterService_KetThuc"].ToString());
        public static int DoDaiMaLuotKham = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["DoDaiMaLuotKham"].ToString());
        public static int TinhTrangRaVien_RaVien = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TinhTrangRaVien_RaVien"].ToString());
        public static int TinhTrangRaVien_ChuyenVien = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TinhTrangRaVien_ChuyenVien"].ToString());
        public static int TinhTrangRaVien_TronVien = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TinhTrangRaVien_TronVien"].ToString());
        public static int TinhTrangRaVien_XinRaVien = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TinhTrangRaVien_XinRaVien"].ToString());
        public static int KetQuaDieuTri_Khoi = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KetQuaDieuTri_Khoi"].ToString());
        public static int KetQuaDieuTri_Do = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KetQuaDieuTri_Do"].ToString());
        public static int KetQuaDieuTri_KhongThayDoi = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KetQuaDieuTri_KhongThayDoi"].ToString());
        public static int KetQuaDieuTri_NangHon = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KetQuaDieuTri_NangHon"].ToString());
        public static int KetQuaDieuTri_TuVong = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["KetQuaDieuTri_TuVong"].ToString());
        public static string Prefix_LuotKham = System.Web.Configuration.WebConfigurationManager.AppSettings["Prefix_LuotKham"].ToString();

        public static string UserInfo_HoVaTen = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_HoVaTen"].ToString();
        public static string UserInfo_MaBenhNhan = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_MaBenhNhan"].ToString();
        public static string UserInfo_NgaySinh = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_NgaySinh"].ToString();
        public static string UserInfo_CMND = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_CMND"].ToString();
        public static string UserInfo_BHYT = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_BHYT"].ToString();
        public static string UserInfo_NgayVao = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_NgayVao"].ToString();
        public static string UserInfo_NgayRa = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_NgayRa"].ToString();
        public static string UserInfo_NgayTaiKham = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_NgayTaiKham"].ToString();
        public static string UserInfo_MaBenh = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_MaBenh"].ToString();
        public static string UserInfo_KetQuaDieuTri = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_KetQuaDieuTri"].ToString();
        public static string UserInfo_TenBenh = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_TenBenh"].ToString();
        public static string UserInfo_ChuanDoan = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_ChuanDoan"].ToString();
        public static string UserInfo_PhuongPhapDieuTri = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_PhuongPhapDieuTri"].ToString();
        public static string UserInfo_LoiDanThayThuoc = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_LoiDanThayThuoc"].ToString();
        public static string UserInfo_GhiChu = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_GhiChu"].ToString();
        public static string UserInfo_FileDinhKem = System.Web.Configuration.WebConfigurationManager.AppSettings["UserInfo_FileDinhKem"].ToString();

        public static string ImportData_SheetName_User = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_User"].ToString();
        public static string ImportData_SheetName_KCB = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_KCB"].ToString();
        public static string ImportData_SheetName_CDHA = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_CDHA"].ToString();
        public static string ImportData_SheetName_XNK = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_XNK"].ToString();
        public static string ImportData_SheetName_XNNT = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_XNNT"].ToString();
        public static string ImportData_SheetName_XNCTM = System.Web.Configuration.WebConfigurationManager.AppSettings["ImportData_SheetName_XNCTM"].ToString();

        public static string ControllerName_User = System.Web.Configuration.WebConfigurationManager.AppSettings["ControllerName_User"].ToString();
        public static string ControllerName_XML1 = System.Web.Configuration.WebConfigurationManager.AppSettings["ControllerName_XML1"].ToString();
        public static int TiengViet = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TiengViet"].ToString());
        public static int TiengAnh = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["TiengAnh"].ToString());
        public static int CategoryTinTucVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryTinTucVN"].ToString());
        public static int CategoryTinTucEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryTinTucEN"].ToString());
        public static int CategoryDaoTaoVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryDaoTaoVN"].ToString());
        public static int CategoryDaoTaoEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryDaoTaoEN"].ToString());
        public static int CategoryGoiKhamVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryGoiKhamVN"].ToString());
        public static int CategoryGoiKhamEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryGoiKhamEN"].ToString());
        public static int CategoryHuongDanVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryHuongDanVN"].ToString());
        public static int CategoryHuongDanEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryHuongDanEN"].ToString());
        public static int CategoryDichVuVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryDichVuVN"].ToString());
        public static int CategoryDichVuEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryDichVuEN"].ToString());
        public static int CategoryGioiThieuVN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryGioiThieuVN"].ToString());
        public static int CategoryGioiThieuEN = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["CategoryGioiThieuEN"].ToString());
        public static string BackgoundApp = System.Web.Configuration.WebConfigurationManager.AppSettings["BackgoundApp"].ToString();
        public static string BackgoundGmail = System.Web.Configuration.WebConfigurationManager.AppSettings["BackgoundGmail"].ToString();
        public static string BackgoundSMS = System.Web.Configuration.WebConfigurationManager.AppSettings["BackgoundSMS"].ToString();

        public static string NewsType_Guide = System.Web.Configuration.WebConfigurationManager.AppSettings["NewsType_Guide"].ToString();
        public static string NewsType_Package = System.Web.Configuration.WebConfigurationManager.AppSettings["NewsType_Package"].ToString();
        public static string NewsType_STAFF = System.Web.Configuration.WebConfigurationManager.AppSettings["NewsType_STAFF"].ToString();
        public static string NewsType_FEEDBACK = System.Web.Configuration.WebConfigurationManager.AppSettings["NewsType_FEEDBACK"].ToString();
        public static string NewsType_ChuyenKhoa = System.Web.Configuration.WebConfigurationManager.AppSettings["NewsType_ChuyenKhoa"].ToString();
        
        public static string TienToMaBenhNhanMacDinh = System.Web.Configuration.WebConfigurationManager.AppSettings["TienToMaBenhNhanMacDinh"].ToString();

        public static string ThoiGianBuoiSang = System.Web.Configuration.WebConfigurationManager.AppSettings["ThoiGianBuoiSang"].ToString();
        public static string ThoiGianBuoiChieu = System.Web.Configuration.WebConfigurationManager.AppSettings["ThoiGianBuoiChieu"].ToString();

        public static List<ThoiGianKhamBenhDto> GetListThoiGianKhamBenh()
        {
            return new List<ThoiGianKhamBenhDto>() {
                new ThoiGianKhamBenhDto()
                {
                    Name = "Buổi sáng",
                    Icon = "wi wi-day-haze",
                    IconClass = "morning",
                    Times = ThoiGianBuoiSang.Split(';').ToList()
                },
                new ThoiGianKhamBenhDto()
                {
                    Name = "Buổi chiều",
                    Icon = "wi wi-day-cloudy",
                    IconClass = "evening",
                    Times = ThoiGianBuoiChieu.Split(';').ToList()
                }
            };
        }

        public static System.DateTime GetNgayBatDauSuDungPhanMem()
        {
            System.DateTime result = System.DateTime.Now;
            System.DateTime.TryParseExact(System.Web.Configuration.WebConfigurationManager.AppSettings["NgayBatDauSuDungPhanMem"].ToString(), Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out result);

            return result;
        }

        public static bool LaChucNangTruyCapChung(string tenController, string tenAction)
        {
            bool result = false;
            string truyCapChucNangChung = System.Web.Configuration.WebConfigurationManager.AppSettings["TruyCapChucNangChung"];
            string[] chucNangs = truyCapChucNangChung.Split(';');

            int i = 0;
            while (!result && i < chucNangs.Length)
            {
                string[] strArr = chucNangs[i].Split(',');

                if ((string.IsNullOrEmpty(strArr[0]) || !string.IsNullOrEmpty(strArr[0]) && strArr[0].Trim().ToLower().Equals(tenController.Trim().ToLower()))
                    && (string.IsNullOrEmpty(strArr[1]) || !string.IsNullOrEmpty(strArr[1]) && strArr[1].Trim().ToLower().Equals(tenAction.Trim().ToLower()))
                    || tenAction.Trim().ToLower().Contains("exporttoexcel")
                    || tenAction.Trim().ToLower().Contains("getlist")
                    || tenAction.Trim().ToLower().Contains("getbyid")
                    || tenAction.Trim().ToLower().Contains("xemchitiet")
                    || tenAction.Trim().ToLower().Contains("thongtinchitiet")
                    || tenAction.Trim().ToLower().Contains("addorupdatelist")

                    )
                {
                    result = true;
                }

                i++;

            }

            return result;
        }

        public static List<BaseEntityDto> GetDanhSachNgonNgu()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = TiengViet,
                    Name = "Tiếng Việt"
                },
                new BaseEntityDto()
                {
                    Id = TiengAnh,
                    Name = "Tiếng Anh"
                }
            };
        }



        public static List<BaseEntityDto> GetListNotificationType_App()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = NotificationType_App_Popup,
                    Name = "Popup"
                },
                new BaseEntityDto()
                {
                    Id = NotificationType_App_TinTuc,
                    Name = "Tin tức"
                },
                new BaseEntityDto()
                {
                    Id = NotificationType_App_DichVu,
                    Name = "Dịch vụ"
                },
                new BaseEntityDto()
                {
                    Id = NotificationType_App_KetQuaKham,
                    Name = "Kết quả khám"
                }
            };
        }

        public static List<BaseEntityDto> GetListKieuChienDich()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = KieuChienDich_ThongBao,
                    Name = "Thông báo"
                },
                new BaseEntityDto()
                {
                    Id = KieuChienDich_Gmail,
                    Name = "Gmail",
                    IsDefault = true
                },
                new BaseEntityDto()
                {
                    Id = KieuChienDich_TinNhan,
                    Name = "Tin nhắn"
                }
            };
        }

        public static List<BaseEntityDto> GetListKetQuaDieuTri()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = KetQuaDieuTri_Khoi,
                    Name = "Khỏi"
                },
                new BaseEntityDto()
                {
                    Id = KetQuaDieuTri_Do,
                    Name = "Đỡ",
                },
                new BaseEntityDto()
                {
                    Id = KetQuaDieuTri_KhongThayDoi,
                    Name = "Không thay đổi"
                },
                new BaseEntityDto()
                {
                    Id = KetQuaDieuTri_NangHon,
                    Name = "Nặng hơn"
                },
                new BaseEntityDto()
                {
                    Id = KetQuaDieuTri_TuVong,
                    Name = "Tử vong"
                }
            };
        }

        public static List<BaseEntityDto> GetListTinhTrangRaVien()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = TinhTrangRaVien_RaVien,
                    Name = "Ra viện"
                },
                new BaseEntityDto()
                {
                    Id = TinhTrangRaVien_ChuyenVien,
                    Name = "Chuyển viện",
                },
                new BaseEntityDto()
                {
                    Id = TinhTrangRaVien_TronVien,
                    Name = "Trốn viện"
                },
                new BaseEntityDto()
                {
                    Id = TinhTrangRaVien_XinRaVien,
                    Name = "Xin ra viện"
                }
            };
        }

        public static List<BaseEntityDto> GetListTrangThaiDangKy()
        {
            return new List<BaseEntityDto>()
            {
                new BaseEntityDto()
                {
                    Id = RegisterService_DangKy,
                    Name = "Đăng ký",
                    IsDefault = true
                },
                new BaseEntityDto()
                {
                    Id = RegisterService_TiepNhan,
                    Name = "Tiếp nhận",
                },
                new BaseEntityDto()
                {
                    Id = RegisterService_Kham,
                    Name = "Khám"
                },
                new BaseEntityDto()
                {
                    Id = RegisterService_KetThuc,
                    Name = "Kết thúc"
                }
            };
        }
    }
}
