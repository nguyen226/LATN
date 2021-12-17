using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class XML3
    {
        public XML3()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int XML3ID { set; get; }
        public string MA_LK { set; get; } //Mã lượt khám của bảng XML1, để xác định đơn thuốc này của lượt khám nào
        public decimal STT { set; get; } //Số thứ tự, 1 bệnh nhân có rất nhiềudịch vụ, cái này + thời gian vào là oder của sắp xếp trên bảng, ngày mới nhất + STT trên cùng
        public string MA_DICH_VU { set; get; }
        public string TEN_DICH_VU { set; get; }
        public string DON_VI_TINH { set; get; }
        public DateTime NGAY_YL { set; get; }
        public DateTime NGAY_KQ { set; get; }
        public string XML3_01 { set; get; } //File pdf đơn thuốc đính kèm(URL)
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
