using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class BaseEntityDto
    {
        public int UserID { set; get; }
        public int Id { set; get; }
        public int No { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string CreatedDate { set; get; }
        public string ModifiedDate { set; get; }
        public string NgayGioCapNhat { set; get; }
        public bool IsDisabled { set; get; }
        public bool IsDefault { set; get; }
        public string NguoiTao { set; get; }
        public string NguoiCapNhat { set; get; }
        public string HoTenUserTao { set; get; }
        public string HoTenUserCapNhat { set; get; }
        public string EmailNguoiCapNhat { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
