using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ICD10Dto : BaseEntityDto
    {
        public string Ma { set; get; }
        public string Ten { set; get; }
        public string NhomBenh { set; get; }
        public string MoTa { set; get; }
        public bool ManTinh { set; get; }
        public bool ThuongGap { set; get; }
        public bool KhongBH { set; get; }
        public bool NgoaiDS { set; get; }
        public bool HieuLuc { set; get; }
    }
}
