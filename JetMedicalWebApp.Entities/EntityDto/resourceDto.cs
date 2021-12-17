using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ResourceDto : BaseEntityDto
    {
        public int id { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public string name { set; get; }
        public bool isView { set; get; }
    }
}
