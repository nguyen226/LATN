using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class Users_GroupPermissionDto : BaseEntityDto
    {    
        public int GroupPermissionId { set; get; }
        public string GroupPermissionName { set; get; }
        
        public int CompanyId { set; get; }
    }
}
