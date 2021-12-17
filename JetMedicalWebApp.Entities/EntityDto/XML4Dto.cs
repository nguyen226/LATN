using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class XML4Dto : BaseEntityDto
    {
        public int XML4ID { set; get; }
        public string MA_LK { set; get; } //Mã lượt khám của bảng XML1, để xác định đơn thuốc này của lượt khám nào
        public decimal STT { set; get; } //Số thứ tự, 1 bệnh nhân có rất nhiềudịch vụ, cái này + thời gian vào là oder của sắp xếp trên bảng, ngày mới nhất + STT trên cùng
        public string TEN_CHI_SO { set; get; }
        public string GIA_TRI { set; get; }
        public string MA_DICH_VU { set; get; }
        public string MA_MAY { set; get; }
        public string MO_TA { set; get; }
        public string KET_LUAN { set; get; }
        public DateTime NGAY_KQ { set; get; }
        public string STRNGAY_KQ { set; get; }
        public string XML4_01 { set; get; } //File pdf đơn thuốc đính kèm(URL)
        public int TongCong { set; get; }
    }
}
