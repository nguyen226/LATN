using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class UserInfoDto : BaseEntityDto
    {
        public int USER_INFO_ID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string MA_BN { set; get; }
        public string DateOfBirth { set; get; }
        public string CMND { set; get; }
        public string BHYT { set; get; }
        public bool Sex { set; get; } // 1 nam , 2 nữ
        public string Address { set; get; }
        public decimal Height { set; get; }
        public decimal weight { set; get; }
        public int BloodTypeID { set; get; }
        public string BloodName { set; get; }
        public string ProvinceID { set; get; }
        public string ProvinceName { set; get; }
        public string DistrictID { set; get; }
        public string DistrictName { set; get; }
        public string Avartar { set; get; }
        public string Occupation { set; get; }
        public int Ma_Cong_Ty { set; get; }
        public int CompanyId { set; get; }
    }
}
