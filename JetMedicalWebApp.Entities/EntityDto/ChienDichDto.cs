using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ChienDichDto : BaseEntityDto
    {
        public int PatientGroupID { set; get; }
        public string PatientGroupName { set; get; }
        public int NotificationID { set; get; }
        public string FileDinhKem { set; get; }
        public string NotificationTitle { set; get; }
        public int Type { set; get; }
        public string Content { set; get; }
    }
}
