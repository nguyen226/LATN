using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Company
    {
        [Key]
        public int CompanyID { set; get; }
        public string ComCode { set; get; }
        public string ComName { set; get; }
        public string ComAdress { set; get; }
        public decimal Phone { set; get; }
        public decimal Fax { set; get; }
        public string Email { set; get; }
        public int DistrictID { set; get; }
        public int ProvinceID { set; get; }
        public bool IsActive { set; get; }
        public string Note { set; get; }
        public System.DateTime CreatedDate { set; get; }
        public System.DateTime ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
