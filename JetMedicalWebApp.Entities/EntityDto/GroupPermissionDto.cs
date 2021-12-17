using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class GroupPermissionDto : BaseEntityDto
    {       
        public bool IsActive { set; get; }
        public int CompanyId { set; get; }
    }
}
