using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class GroupsDto : BaseEntityDto
    {
        public int GroupID { set; get; }
        public string GroupName { set; get; }
        public bool Active { set; get; }
        public string Notes { set; get; }
    }
}
