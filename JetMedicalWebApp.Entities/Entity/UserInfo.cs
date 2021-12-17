using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class UserInfo
    {
        [Key]
        public int USER_INFO_ID { set; get; }
        public int UserID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string MA_BN { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string CMND { set; get; }
        public string BHYT { set; get; }
        public bool Sex { set; get; } // 1 nam , 2 nữ
        public string Address { set; get; }
        public decimal Height { set; get; }
        public decimal weight { set; get; }
        public int BloodTypeID { set; get; }
        public string ProvinceID { set; get; }
        public string DistrictID { set; get; }
        public bool IsDefault { set; get; }
        public string Avartar { set; get; }
        public string Occupation { set; get; }
        public int Ma_Cong_Ty { set; get; }
    }
}
