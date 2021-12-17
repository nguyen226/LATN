using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class AppSlideDto : BaseEntityDto
    {       
        public string Images { set; get; }
        public bool Active { set; get; }
        public int NoCategory { set; get; }
        public string NoTitle { set; get; }
        public string NoContentName { set; get; }
        public int NoID { set; get; }
        public int Odx { set; get; }
    }
}
