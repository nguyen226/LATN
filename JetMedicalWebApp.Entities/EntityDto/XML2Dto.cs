using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class XML2Dto : BaseEntityDto
    {
        public int XML2ID { set; get; }
        public string MA_LK { set; get; }
        public int STT { set; get; }
        public string TEN_THUOC { set; get; } //Hiển thị text trên app: nếu có dữ liệu thì hiển thị trên lưới, ko thì để --
        public string DON_VI_TINH { set; get; }
        public string DUONG_DUNG { set; get; }
        public string LIEU_DUNG { set; get; }
        public decimal SO_LUONG { set; get; }
        public DateTime NGAY_YL { set; get; }
        public string STRNGAY_YL { set; get; }
        public string XML2_01 { set; get; } //File pdf đơn thuốc đính kèm(URL)
    }
}
