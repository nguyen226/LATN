using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class UsersDto : BaseEntityDto
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string EmailID { set; get; }
        public string Phone { set; get; }
        public string Password { set; get; }
        public bool IsVerified { set; get; }
        public string ActivationCode { set; get; }
        public string ResetPasswordCode { set; get; }
        public string GroupName { set; get; }
        public int GroupID { set; get; }
        public bool Active { set; get; }
        public string LastLoginDate { set; get; }
        public string LastAccessedDate { set; get; }
        public int LoginFailedCount { set; get; }
        public string Avartar { set; get; }
        public string CreateDate { set; get; }
        public bool IsMobile { set; get; }
        public bool IsAdmin { set; get; }
        public string HoTen { set; get; }
        public string DateOfBirth { set; get; }
        public string BHYT { set; get; }
        public string CMND { set; get; }
        public string Address { set; get; }
        public string MA_BN { set; get; }
        public int CompanyId { set; get; }

        public string NGAY_VAO { set; get; }
        public string NGAY_RA { set; get; }
        public string NGAY_TAI_KHAM { set; get; }
        public string MA_BENH { set; get; }

        public string KET_QUA_DTRI { set; get; }
        public string TEN_BENH { set; get; }
        public string CHUAN_DOAN { set; get; }
        public string PPDIEUTRI { set; get; }

        public string LOIDANTHAYTHUOC { set; get; }
        public string GHICHU { set; get; }


    }
}
