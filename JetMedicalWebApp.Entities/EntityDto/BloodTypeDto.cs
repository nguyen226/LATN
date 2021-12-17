using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class BloodTypeDto : BaseEntityDto
    {
        public int BloodTypeID { set; get; }
        public string BloodName { set; get; }
    }
}
