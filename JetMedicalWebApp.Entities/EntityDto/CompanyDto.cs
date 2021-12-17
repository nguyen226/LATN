using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class CompanyDto : BaseEntityDto
    {
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
    }
}
