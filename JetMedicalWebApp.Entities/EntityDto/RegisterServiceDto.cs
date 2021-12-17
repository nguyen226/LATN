using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class RegisterServiceDto: BaseEntityDto
    {
        public int RegisterID { set; get; }
        public DateTime RegisterDate { set; get; }
        public string StrRegisterDate { set; get; }
        public int DepartmentId { set; get; }
        public string DepartmentName { set; get; }
        public string TenBacSi { set; get; }
        public string NgayKham { set; get; }
        public string ThoiGianKham { set; get; }
        public string GoiKham { set; get; }
        
        public string RegisterNote { set; get; }
        public int PackageId { set; get; }
        public int StaffId { set; get; }
        public int Status { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string FullName { set; get; }
        public string EmailID { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string MA_LK { set; get; }
    }
}
