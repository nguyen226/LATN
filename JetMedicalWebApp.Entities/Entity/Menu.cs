using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Menu : BaseEntity
    {
        public bool IsActive { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
        public string MaMenuCapTren { set; get; }
        public string Parameters { set; get; }
        public string Icon { set; get; }
        public virtual ICollection<Menu_GroupPermission> Menu_GroupPermissions { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }

    }
}
