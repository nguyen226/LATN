using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class ChienDich : BaseEntity
    {
        public int PatientGroupID { set; get; }
        public int NotificationID { set; get; }
        public int Type { set; get; }
        public string Content { set; get; }
        public string FileDinhKem { set; get; }
        
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
