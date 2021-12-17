using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Users_GroupPermission : BaseEntity
    {
        public Users Users { set; get; }
        public GroupPermission GroupPermission { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
