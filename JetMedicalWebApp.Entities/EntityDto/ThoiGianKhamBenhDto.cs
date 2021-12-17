using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ThoiGianKhamBenhDto
    {
        public string Name { set; get; }
        public string Icon { set; get; }
        public string IconClass { set; get; }
        public List<string> Times { set; get; }
    }
}
