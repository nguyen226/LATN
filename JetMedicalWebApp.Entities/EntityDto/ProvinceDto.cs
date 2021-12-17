using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ProvinceDto : BaseEntityDto
    {
        public string ProvinceID { set; get; }
        public string ProvinceName { set; get; }
        public int ODX { set; get; }
        public bool IsActive { set; get; }
    }
}
