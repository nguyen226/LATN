using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class XML2
    {
        public XML2()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int XML2ID { set; get; }
        public string MA_LK { set; get; }
        public int STT { set; get; }
        public string TEN_THUOC { set; get; } //Hiển thị text trên app: nếu có dữ liệu thì hiển thị trên lưới, ko thì để --
        public string DON_VI_TINH { set; get; } 
        public string DUONG_DUNG { set; get; }
        public string LIEU_DUNG { set; get; }
        public decimal SO_LUONG { set; get; }
        public DateTime NGAY_YL { set; get; }
        public string XML2_01 { set; get; } //File pdf đơn thuốc đính kèm(URL)
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
