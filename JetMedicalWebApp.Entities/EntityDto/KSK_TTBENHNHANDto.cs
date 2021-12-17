using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class KSK_TTBENHNHANDto
    {
        public int KSK_ID { set; get; }
        public int UserID { set; get; } 
        public string MA_LK { set; get; } 
        public string MA_BN { set; get; }
        public string HO_TEN { set; get; }
        public string NGAY_SINH { set; get; }
        public decimal GIOI_TINH { set; get; }
        public string CMND { set; get; }
        public string DIENTHOAI { set; get; }
        public string TEN_DOANKSK { set; get; }
        public string CAN_NANG { set; get; }
        public string CHIEU_CAO { set; get; }
        public string HUYET_AP { set; get; }
        public string MACH { set; get; }
        public string KHAM_LAM_SANG { set; get; }
        public string PHAN_LOAI { set; get; }
        public string KET_LUAN { set; get; }
        public string TU_VAN { set; get; }
      
    }
}
