using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class XML1
    {
        public XML1()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MA_LK { set; get; }
        public int STT { set; get; }//1 bệnh nhân có rất nhiều lượt khám, cái này + thời gian vào là oder của sắp xếp trên bảng, ngày mới nhất + STT trên cùng
        public string MA_BN { set; get; } //Mã bệnh nhân, chỗ này chính là mã MA_BN trong UserInfo
        public int BENH_ID { set; get; }
        public string TEN_BENH { set; get; }
        public string MA_BENH { set; get; }
        public string MA_BENHKHAC { set; get; }
        public DateTime NGAY_VAO { set; get; }
        public DateTime? NGAY_TAI_KHAM { set; get; }
        public DateTime? NGAY_RA { set; get; }
        public decimal KET_QUA_DTRI { set; get; } //: Khỏi; 2: Đỡ; 3: Không thay đổi; 4: Nặng hơn; 5: Tử vong)
        public decimal TINH_TRANG_RV { set; get; } //:Tình trạng ra viện;  (1: Ra viện; 2: Chuyển viện; 3: Trốn viện; 4: Xin ra viện)
        public string MA_KHOA { set; get; }// khoa khám bệnh
        public string CHUAN_DOAN { set; get; }
        public string PPDIEUTRI { set; get; }
        public string LOIDANTHAYTHUOC { set; get; }
        public string GHICHU { set; get; }
        public string XML1_File { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
