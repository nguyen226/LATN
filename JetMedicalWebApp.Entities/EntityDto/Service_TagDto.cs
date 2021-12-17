using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class Service_TagDto : BaseEntityDto
    {
        public int ServiceId { set; get; }
        public int TagId { set; get; }
    }
}
