using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class DistrictDto
    {
        public string DistrictID { set; get; }
        public string ProvinceID { set; get; }
        public string DistrictName { set; get; }
        public bool IsActive { set; get; }
        public int ODX { set; get; }
    }
}
