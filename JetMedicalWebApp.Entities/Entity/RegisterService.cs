using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class RegisterService
    {
        public RegisterService()
        {
            CreateDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int RegisterID { set; get; }
        public int UserID { set; get; }
        public DateTime RegisterDate { set; get; }
        public int? DepartmentId { set; get; }
        public string RegisterNote { set; get; }
        public int? PackageId { set; get; }
        public int StaffId { set; get; }
        public int Status { set; get; }
        public string MA_LK { set; get; }
        public DateTime CreateDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public DateTime? DOB { set; get; }
        public string Emaill { set; get; }
        public string PhoneNumber { set; get; }
        public string FullName { set; get; }
        public int RegisterNo { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
