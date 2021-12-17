using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class Company_GroupPermissionDto : BaseEntityDto
    {
        public string ComName { set; get; }
        public int GroupPermissionId { set; get; }
        public int CompanyId { set; get; }
    }
}
