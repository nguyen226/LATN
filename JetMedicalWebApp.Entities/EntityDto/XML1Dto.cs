using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class XML1Dto : BaseEntityDto
    {
        public string MA_LK { set; get; }
        public int STT { set; get; }//1 bệnh nhân có rất nhiều lượt khám, cái này + thời gian vào là oder của sắp xếp trên bảng, ngày mới nhất + STT trên cùng
        public string MA_BN { set; get; } //Mã bệnh nhân, chỗ này chính là mã MA_BN trong UserInfo
        public string HO_TEN { set; get; }
        public string FIRSTNAME { set; get; }
        public string LASTNAME { set; get; }
        public string NGAY_SINH { set; get; }
        public string GIOI_TINH { set; get; }
        public string DIA_CHI { set; get; }
        public string MA_THE { set; get; }
        public string MA_DKBD { set; get; }
        public string GT_THE_TU { set; get; }
        public string GT_THE_DEN { set; get; }
        public int BENH_ID { set; get; }
        public string DepartmentName { set; get; }
        public int DepartmentId { set; get; }
        public string TEN_BENH { set; get; }
        public string MA_BENH { set; get; }
        public string MA_BENHKHAC { set; get; }
        public string MA_LYDO_VVIEN { set; get; }
        public string MA_NOI_CHUYEN { set; get; }
        public string MA_TAI_NAN { set; get; }
        public DateTime NGAY_VAO { set; get; }
        public string STRNGAY_VAO { set; get; }
        public DateTime? NGAY_RA { set; get; }
        public string STRNGAY_RA { set; get; }
        public DateTime? NGAY_TAI_KHAM { set; get; }
        public string STRNGAY_TAI_KHAM { set; get; }
        public string STRSO_NGAY_DTRI { set; get; }
        public decimal SO_NGAY_DTRI { set; get; }
        public string STRKET_QUA_DTRI { set; get; } //: Khỏi; 2: Đỡ; 3: Không thay đổi; 4: Nặng hơn; 5: Tử vong)
        public decimal KET_QUA_DTRI { set; get; } //: Khỏi; 2: Đỡ; 3: Không thay đổi; 4: Nặng hơn; 5: Tử vong)
        public string STRTINH_TRANG_RV { set; get; } //:Tình trạng ra viện;  (1: Ra viện; 2: Chuyển viện; 3: Trốn viện; 4: Xin ra viện)
        public decimal TINH_TRANG_RV { set; get; } //:Tình trạng ra viện;  (1: Ra viện; 2: Chuyển viện; 3: Trốn viện; 4: Xin ra viện)
        public string MA_KHOA { set; get; }
        public string TEN_KHOA { set; get; }
        public string NGAY_TTOAN { set; get; }
        public string CHUAN_DOAN { set; get; }
        public string PPDIEUTRI { set; get; }
        public string LOIDANTHAYTHUOC { set; get; }
        public string GHICHU { set; get; }
        public string XML1_File { set; get; }
        public int CompanyId { set; get; }
    }
}
