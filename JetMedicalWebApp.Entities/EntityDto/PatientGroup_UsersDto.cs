using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class PatientGroup_UsersDto: BaseEntityDto
    {
        public int PatientGroupID { set; get; }
        public int UsersID { set; get; }
        public string HO_TEN { set; get; }
        public int CompanyId { set; get; }
        public string EmailID { set; get; }
        public string Phone { set; get; }
    }
}
