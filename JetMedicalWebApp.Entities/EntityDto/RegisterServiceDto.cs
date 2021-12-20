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
        public string StrRegisterDate { set; get; }
        public int DepartmentId { set; get; }
        public string DepartmentName { set; get; }
        public string TenBacSi { set; get; }
        public string RegisterNote { set; get; }
        public int StaffId { set; get; }
        public int Status { set; get; }
        public string FullName { set; get; }
        public string PhoneNumber { set; get; }
        public string Emaill { set; get; }
        public string DOB { set; get; }
        public int RegisterNo { set; get; }
        public DateTime RegisterDate { set; get; }
    }
}
