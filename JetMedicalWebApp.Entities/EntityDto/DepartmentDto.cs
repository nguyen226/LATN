using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class DepartmentDto : BaseEntityDto
    {
        public int id { set; get; }
        public string code { set; get; }
        public string name { set; get; }
        public int? languageId { set; get; }
        public int? isGroup { set; get; }
        public bool isActive { set; get; }
    }
}
