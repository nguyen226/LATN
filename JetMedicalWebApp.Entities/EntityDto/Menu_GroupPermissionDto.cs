using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class Menu_GroupPermissionDto : BaseEntityDto
    {
     
        public bool SystemView { set; get; }
        public bool PersonalView { set; get; }
        public bool PersonalDelete { set; get; }
        public bool SystemDelete { set; get; }
        public bool SystemEdit { set; get; }
        public bool PersonalEdit { set; get; }
        public int MenuId { set; get; }
        public int GroupPermissionId { set; get; }
        public string MaMenu { set; get; }
        public string MaMenuCapTren { set; get; }
        public string ActionName { set; get; }
        public string ControllerName { set; get; }
        public bool AllowUpdate { set; get; }
        public bool AllowDelete { set; get; }
        public bool IsSystemUpdate_User { set; get; }
        public bool IsSystemUpdate_XML1 { set; get; }
        public int CompanyId { set; get; }
    }
}
