using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class GroupPermission : BaseEntity
    {
      
        public bool IsActive { set; get; }
        public virtual ICollection<Menu_GroupPermission> Menu_GroupPermissions { set; get; }
        public virtual ICollection<Users_GroupPermission> Users_GroupPermissions { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
