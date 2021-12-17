using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class AppHistoryDto : BaseEntityDto
    {
      
        public string ThaoTac { set; get; }
        public int UserId { set; get; }
        public string GhiChu { set; get; }
    }
}
