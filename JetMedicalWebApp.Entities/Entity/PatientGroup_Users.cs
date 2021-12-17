using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class PatientGroup_Users: BaseEntity
    {
        public int PatientGroupID { set; get; }
        public int UsersID { set; get; }
        public int CompanyId { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
