using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class CauHinhEmailDto : BaseEntityDto
    {
        public string Host { set; get; }
        public string Port { set; get; }
        public bool Active { set; get; }
        public bool SSL { set; get; }
        public string Account { set; get; }
        public string Password { set; get; }
    }
}
