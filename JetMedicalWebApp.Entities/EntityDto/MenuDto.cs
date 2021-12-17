using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class MenuDto : BaseEntityDto
    {
        public bool IsActive { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
        public string MaMenuCapTren { set; get; }
        public string Parameters { set; get; }
        public string Icon { set; get; }
    }
}
