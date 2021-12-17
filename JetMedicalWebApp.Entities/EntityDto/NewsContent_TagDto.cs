using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class NewsContent_TagDto : BaseEntityDto
    {
        public int NewsContentId { set; get; }
        public int TagId { set; get; }
    }
}
